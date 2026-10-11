using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using FactionLanguages.Patches;
using FactionLanguages.Utilities;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace RimLanguages.IndividualLearning
{
    public static class PrisonerSocial
    {
        public static bool Understands(PawnLanguages data, string languageKey) =>
            data != null && !string.IsNullOrEmpty(languageKey) && data.Progress(languageKey) >= 1f;

        public static bool MenuRecruitable(Faction faction, Pawn prisoner)
        {
            var comp = prisoner?.IsPrisoner == true ? IndividualLearning.Ready(prisoner) : null;
            if (comp != null && IndividualLearning.Valid(IndividualLearning.NativeDialect(comp.Data))) return true;
            return LanguageUtility.FactionLanguageIsLearned(faction);
        }

        public static bool MenuModeAllowed(Faction faction, Pawn prisoner, PrisonerInteractionModeDef mode)
        {
            if (mode == PrisonerInteractionModeDefOf.AttemptRecruit ||
                mode == PrisonerInteractionModeDefOf.ReduceResistance || mode == PrisonerInteractionModeDefOf.Convert)
                return MenuRecruitable(faction, prisoner);
            return LanguageUtility.FactionLanguageIsLearned(faction);
        }

        public static bool CanAddress(Pawn warden, Pawn prisoner)
        {
            if (prisoner?.IsPrisoner != true) return true;
            var actor = IndividualLearning.Ready(warden);
            var target = IndividualLearning.Ready(prisoner);
            var dialect = target == null ? null : IndividualLearning.NativeDialect(target.Data);
            if (actor != null && IndividualLearning.Valid(dialect))
                return Understands(actor.Data, dialect.Parent.GetUniqueLoadID());
            // Preserve Cybranian's language gate when personal data cannot be resolved.
            return prisoner.Faction == null || LanguageUtility.FactionLanguageIsLearned(prisoner.Faction);
        }

        public static IEnumerable<CodeInstruction> ReplaceGlobalCheck(IEnumerable<CodeInstruction> instructions, bool menu)
        {
            var code = instructions.ToList();
            var original = AccessTools.Method(typeof(LanguageUtility), nameof(LanguageUtility.FactionLanguageIsLearned),
                new[] { typeof(Faction) });
            if (code.Count(i => i.Calls(original)) != 1)
                throw new InvalidOperationException("RLI: expected exactly one Cybranian prisoner recruitment language check.");
            var helper = AccessTools.Method(typeof(PrisonerSocial), menu ? nameof(MenuModeAllowed) : nameof(MenuRecruitable));
            foreach (var instruction in code)
            {
                if (!instruction.Calls(original)) { yield return instruction; continue; }
                var load = new CodeInstruction(menu ? OpCodes.Ldarg_2 : OpCodes.Ldarg_0);
                load.labels.AddRange(instruction.labels);
                load.blocks.AddRange(instruction.blocks);
                yield return load;
                if (menu)
                {
                    yield return new CodeInstruction(OpCodes.Ldind_Ref);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                }
                yield return new CodeInstruction(OpCodes.Call, helper);
            }
        }
    }

    // Patch the base mod's gate, not the final Recruitable result: vanilla refusals survive.
    [HarmonyPatch(typeof(Pawn_GuestTracker_Recruitable_FactionLanguagesPatch), "Postfix")]
    public static class IndividualRecruitableGatePatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
            PrisonerSocial.ReplaceGlobalCheck(instructions, false);
    }

    [HarmonyPatch(typeof(Pawn_GuestTracker_SetExclusiveInteraction_FactionLanguagesPatch), "Postfix")]
    public static class IndividualRecruitmentMenuPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
            PrisonerSocial.ReplaceGlobalCheck(instructions, true);
    }

    [HarmonyPatch]
    public static class IndividualPrisonerSocialWorkPatch
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(WorkGiver_Warden_Chat), nameof(WorkGiver_Warden_Chat.JobOnThing),
                new[] { typeof(Pawn), typeof(Thing), typeof(bool) });
            yield return AccessTools.Method(typeof(WorkGiver_Warden_Convert), nameof(WorkGiver_Warden_Convert.JobOnThing),
                new[] { typeof(Pawn), typeof(Thing), typeof(bool) });
        }

        [HarmonyPriority(Priority.Last)]
        public static void Postfix(Pawn pawn, Thing t, ref Job __result)
        {
            if (__result != null && !PrisonerSocial.CanAddress(pawn, t as Pawn))
            {
                __result = null;
                JobFailReason.Is("RLI_PrisonerLanguageRequired".Translate());
            }
        }
    }

    [HarmonyPatch(typeof(Pawn_InteractionsTracker), nameof(Pawn_InteractionsTracker.TryInteractWith))]
    public static class IndividualPrisonerSocialInteractionPatch
    {
        [HarmonyPriority(Priority.First)]
        public static bool Prefix(Pawn ___pawn, Pawn recipient, InteractionDef intDef, ref bool __result)
        {
            // Recheck an in-flight job after the actor's levels were changed.
            if (recipient?.IsPrisoner == true &&
                (intDef == InteractionDefOf.RecruitAttempt || intDef == InteractionDefOf.ConvertIdeoAttempt ||
                 intDef == InteractionDefOf.BuildRapport) && !PrisonerSocial.CanAddress(___pawn, recipient))
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
