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
            foreach (var instruction in code)
            {
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
