using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using FactionLanguages;
using FactionLanguages.Utilities;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace RimLanguages.IndividualLearning
{
    public static class NewbornLanguages
    {
        [ThreadStatic] private static int generating;
        public static bool Generating => generating > 0;

        private static Dialect MaternalIdentity(Pawn mother)
        {
            var comp = mother?.GetComp<CompIndividualLanguages>();
            if (comp == null || !comp.EnsureInitialized()) return null;
            var db = CompIndividualLanguages.Database;
            if (db == null) return null;
            // Identity is distinct from mastery: a mother's edited level does not change her origin.
            foreach (var language in db.RaceLanguages)
                foreach (var dialect in language.Dialects)
                    if (comp.Data.Find(dialect.GetUniqueLoadID())?.native == true &&
                        comp.Data.Find(language.GetUniqueLoadID())?.native == true) return dialect;
            return null;
        }

        public static Pawn Generate(PawnGenerationRequest request, Pawn geneticMother)
        {
            var identity = MaternalIdentity(geneticMother);
            Pawn baby;
            generating++;
            try { baby = PawnGenerator.GeneratePawn(request); }
            finally { generating--; }
            var comp = baby?.GetComp<CompIndividualLanguages>();
            if (comp == null || baby.RaceProps?.Humanlike != true ||
                baby.DevelopmentalStage != DevelopmentalStage.Baby) return baby;
            var db = CompIndividualLanguages.Database;
            if (identity == null && (db == null || !db.TryGetLanguage(baby.Faction, out identity)))
            {
                // Retain the normal initialization fallback if no identity can be resolved.
                comp.EnsureInitialized();
                return baby;
            }
            if (IndividualLearning.Valid(identity) && comp.Data.InitializeNewborn(
                identity.Parent.GetUniqueLoadID(), identity.Parent.Name, identity.GetUniqueLoadID(), identity.Name))
            {
                comp.Data.originFaction = geneticMother?.GetComp<CompIndividualLanguages>()?.Data.originFaction ?? baby.Faction;
                comp.Data.originCaptured = comp.Data.originFaction != null;
            }
            return baby;
        }
    }

    [HarmonyPatch(typeof(PregnancyUtility), nameof(PregnancyUtility.ApplyBirthOutcome))]
    public static class IndividualBirthPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            var original = AccessTools.Method(typeof(PawnGenerator), nameof(PawnGenerator.GeneratePawn),
                new[] { typeof(PawnGenerationRequest) });
            if (code.Count(i => i.Calls(original)) != 1)
                throw new InvalidOperationException("RLI: unsupported birth generation call.");
            var replacement = AccessTools.Method(typeof(NewbornLanguages), nameof(NewbornLanguages.Generate));
            foreach (var instruction in code)
            {
                if (!instruction.Calls(original)) { yield return instruction; continue; }
                var mother = new CodeInstruction(OpCodes.Ldarg_S, (byte)4);
                mother.labels.AddRange(instruction.labels);
                mother.blocks.AddRange(instruction.blocks);
                yield return mother;
                yield return new CodeInstruction(OpCodes.Call, replacement);
            }
        }
    }

    public static class BabyCareLearning
    {
        public static float Exposure(float before, float after)
        {
            if (float.IsNaN(before) || float.IsNaN(after) || float.IsInfinity(before) || float.IsInfinity(after)) return 0f;
            return Math.Max(0f, ProgressMath.Clamp(after) - ProgressMath.Clamp(before));
        }

        public static void Feed(Pawn adult, Pawn baby, float exposure)
            => Learn(adult, baby, exposure);

        public static void Learn(Pawn adult, Pawn baby, float exposure)
        {
            if (!ModsConfig.BiotechActive || !(IndividualLearningMod.Settings?.enabled ?? true) ||
                baby?.DevelopmentalStage != DevelopmentalStage.Baby ||
                adult?.DevelopmentalStage != DevelopmentalStage.Adult || !ProgressMath.PositiveFinite(exposure)) return;
            var student = IndividualLearning.Ready(baby);
            var teacher = IndividualLearning.Ready(adult);
            var social = adult.skills?.GetSkill(SkillDefOf.Social);
            if (student == null || teacher == null || social == null || social.TotallyDisabled) return;
            var lessons = AutomaticTransmission.AvailableLessons();
            Rand.PushState(unchecked(Find.TickManager.TicksGame ^ baby.thingIDNumber ^ (adult.thingIDNumber * 397)));
            try
            {
                var lesson = TransmissionRules.Select(student.Data, teacher.Data, lessons, Rand.Value,
                    AutomaticTransmission.SameColony(adult, baby));
                if (lesson == null) return;
                // Babies do not have a usable Social skill. Use the same level-zero baseline,
                // without querying their disabled language stat or changing their skills.
                float xp = TransmissionRules.Xp(lesson, 0f, social.Level, false, false, true,
                    LanguageUtility.Settings.languageLearnDifficulty, Rand.Range(0.5f, 1.5f)) * ProgressMath.Clamp(exposure);
                float gain = xp / lesson.xpMax;
                if (!ProgressMath.PositiveFinite(gain)) return;
                float before = student.Data.Progress(lesson.key);
                student.Data.Gain(lesson.key, lesson.name, lesson.dialect, gain);
                LearningHistory.Record(baby, LearningHistory.Changes(before, student.Data.Progress(lesson.key), lesson.dialect));
            }
            finally { Rand.PopState(); }
        }
    }

    public sealed class BabyPlayCredit
    {
        private bool claimed;
        public bool TryClaim()
        {
            if (claimed) return false;
            claimed = true;
            return true;
        }
    }

    [HarmonyPatch(typeof(JobDriver_BabyPlay), "AddPlayThoughtIfAppropriate")]
    public static class IndividualBabyPlayPatch
    {
        // Completed jobs can be collected; this guard never keeps pawns or jobs alive.
        private static readonly ConditionalWeakTable<JobDriver_BabyPlay, BabyPlayCredit> Credits =
            new ConditionalWeakTable<JobDriver_BabyPlay, BabyPlayCredit>();

        public static float Exposure(float initial, float current, bool finishedSetup) =>
            !finishedSetup || initial < 0f ? 0f : BabyCareLearning.Exposure(initial, current);

        public static void Postfix(JobDriver_BabyPlay __instance, float ___initialPlayPercentage,
            bool ___finishedSetup, bool __runOriginal)
        {
            if (!__runOriginal) return;
            var baby = __instance.job?.GetTarget(TargetIndex.A).Thing as Pawn;
            if (baby?.needs?.play == null) return;
            float exposure = Exposure(___initialPlayPercentage, baby.needs.play.CurLevelPercentage, ___finishedSetup);
            if (!ProgressMath.PositiveFinite(exposure) || !Credits.GetOrCreateValue(__instance).TryClaim()) return;
            BabyCareLearning.Learn(__instance.pawn, baby, exposure);
        }
    }

    [HarmonyPatch]
    public static class IndividualBabyFeedingPatch
    {
        private static readonly FieldInfo InitialFood = AccessTools.Field(typeof(JobDriver_FeedBaby), "initialFoodPercentage");
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(JobDriver_Breastfeed), "Breastfeed");
            yield return AccessTools.Method(typeof(JobDriver_BottleFeedBaby), "FeedBabyFoodFromInventory");
        }

        public static void Postfix(JobDriver_FeedBaby __instance, Toil __result)
        {
            if (__result == null) return;
            bool credited = false;
            __result.AddFinishAction(() =>
            {
                if (credited) return;
                credited = true;
                var baby = __instance.job.GetTarget(TargetIndex.A).Thing as Pawn;
                if (baby?.needs?.food == null) return;
                float exposure = BabyCareLearning.Exposure((float)InitialFood.GetValue(__instance), baby.needs.food.CurLevelPercentage);
                BabyCareLearning.Feed(__instance.pawn, baby, exposure);
            });
        }
    }
}
