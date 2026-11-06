using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using FactionLanguages;
using FactionLanguages.AI.WorkGivers;
using FactionLanguages.Patches;
using FactionLanguages.Utilities;
using HarmonyLib;
using Verse;
using RimWorld;

namespace RimLanguages.IndividualLearning
{
    public static class PrisonerLearning
    {
        public static bool HasLearningSource(Pawn prisoner)
        {
            if (prisoner == null) return false;
            if (prisoner.Faction != null) return true;
            var teacher = IndividualLearning.Ready(prisoner);
            return teacher != null && IndividualLearning.Valid(IndividualLearning.NativeDialect(teacher.Data));
        }

        // No warden has been selected in the prisoner menu. Keep the order available;
        // the existing work giver will select only a warden who still needs this lesson.
        public static bool MenuLanguageComplete(Faction faction, Pawn prisoner)
        {
            var teacher = IndividualLearning.Ready(prisoner);
            if (teacher != null && IndividualLearning.Valid(IndividualLearning.NativeDialect(teacher.Data)))
                return false;
            return LanguageUtility.FactionDialectIsLearned(faction);
        }

        public static bool WardenLanguageComplete(Faction faction, Pawn warden, Thing target)
        {
            var learner = IndividualLearning.Ready(warden);
            var teacher = IndividualLearning.Ready(target as Pawn);
            var dialect = teacher == null ? null : IndividualLearning.NativeDialect(teacher.Data);
            if (learner == null || !IndividualLearning.Valid(dialect))
                return LanguageUtility.FactionDialectIsLearned(faction);
            return !IndividualLearning.NeedsLesson(learner.Data, dialect);
        }

        // Replace one verified call, preserving branches and all non-language checks.
        // If Cybranian changes this method, reject the patch instead of editing unknown IL.
        public static IEnumerable<CodeInstruction> ReplaceCheck(IEnumerable<CodeInstruction> instructions,
            MethodInfo replacement, bool menu)
        {
            var code = instructions.ToList();
            var original = AccessTools.Method(typeof(LanguageUtility), nameof(LanguageUtility.FactionDialectIsLearned),
                new[] { typeof(Faction) });
            if (code.Count(i => i.Calls(original)) != 1)
                throw new InvalidOperationException("RLI: expected exactly one Cybranian prisoner language check.");
            // The parent rejects factionless prisoners before reaching the XP check.
            // Replace only that first truth test, leaving the later real faction argument intact.
            var factionGetter = AccessTools.PropertyGetter(typeof(Thing), nameof(Thing.Faction));
            int checkAt = code.FindIndex(i => i.Calls(original));
            var guards = Enumerable.Range(0, checkAt).Where(i => code[i].Calls(factionGetter) &&
                (code[i + 1].opcode == OpCodes.Brfalse || code[i + 1].opcode == OpCodes.Brfalse_S)).ToList();
            if (guards.Count != 1)
                throw new InvalidOperationException("RLI: unsupported Cybranian prisoner faction guard.");
            for (int index = 0; index < code.Count; index++)
            {
                var instruction = code[index];
                if (index == guards[0])
                {
                    var sourceCheck = new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(PrisonerLearning), nameof(HasLearningSource)));
                    sourceCheck.labels.AddRange(instruction.labels);
                    sourceCheck.blocks.AddRange(instruction.blocks);
                    yield return sourceCheck;
                    continue;
                }
                if (!instruction.Calls(original)) { yield return instruction; continue; }
                var load = new CodeInstruction(menu ? OpCodes.Ldarg_2 : OpCodes.Ldarg_1);
                load.labels.AddRange(instruction.labels);
                load.blocks.AddRange(instruction.blocks);
                yield return load;
                yield return new CodeInstruction(menu ? OpCodes.Ldind_Ref : OpCodes.Ldarg_2);
                yield return new CodeInstruction(OpCodes.Call, replacement);
            }
        }
    }

    [HarmonyPatch(typeof(Pawn_GuestTracker_SetExclusiveInteraction_FactionLanguagesPatch), "Postfix")]
    public static class IndividualPrisonerMenuPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
            PrisonerLearning.ReplaceCheck(instructions,
                AccessTools.Method(typeof(PrisonerLearning), nameof(PrisonerLearning.MenuLanguageComplete)), true);
    }

    [HarmonyPatch(typeof(WorkGiver_Warden_LearnLanguage), nameof(WorkGiver_Warden_LearnLanguage.JobOnThing))]
    public static class IndividualPrisonerWorkPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
            PrisonerLearning.ReplaceCheck(instructions,
                AccessTools.Method(typeof(PrisonerLearning), nameof(PrisonerLearning.WardenLanguageComplete)), false);
    }
}
