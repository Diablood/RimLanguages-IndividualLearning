using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using FactionLanguages;
using HarmonyLib;
using RimLanguages.IndividualLearning;
using RimWorld;

internal static class LearningHistoryTests
{
    public static void Run(Action<bool, string> check)
    {
        var data = new PawnLanguages();
        float before = data.Progress("L");
        data.Learn("L", "Language", 100f, "D", "Dialect", 50f, 30f);
        check(LearningHistory.Changes(before, data.Progress("L"), false) == LearningEvents.Experience,
            "A partial personal gain produces experience, without collective mastery.");
        before = data.Progress("L");
        data.Learn("L", "Language", 100f, "D", "Dialect", 50f, 500f);
        check(LearningHistory.Changes(before, data.Progress("L"), false) ==
            (LearningEvents.Experience | LearningEvents.LanguageMastered) && data.Progress("D") == 0f,
            "Personal language crossing emits mastery once, with no dialect overflow.");
        before = data.Progress("D");
        data.Learn("L", "Language", 100f, "D", "Dialect", 50f, 50f);
        check(LearningHistory.Changes(before, data.Progress("D"), true) ==
            (LearningEvents.Experience | LearningEvents.DialectMastered),
            "Personal dialect crossing emits dialect mastery, not language mastery.");
        before = data.Progress("D");
        data.Learn("L", "Language", 100f, "D", "Dialect", 50f, 50f);
        check(LearningHistory.Changes(before, data.Progress("D"), true) == LearningEvents.None,
            "A fully learned stage does not repeatedly emit experience or mastery.");
        check(LearningHistory.Changes(0.3f, 0.2f, false) == LearningEvents.None &&
            LearningHistory.Changes(float.NaN, 1f, false) == LearningEvents.None &&
            LearningHistory.Changes(0f, float.PositiveInfinity, true) == LearningEvents.None,
            "Drops and invalid levels do not produce learning history.");
        check(LearningHistory.Changes(0.9999f, 1f, false) ==
            (LearningEvents.Experience | LearningEvents.LanguageMastered), "The last fraction counts as personal mastery.");

        var record = AccessTools.Method(typeof(HistoryEventsManager), nameof(HistoryEventsManager.RecordEvent),
            new[] { typeof(HistoryEvent), typeof(bool) });
        var helper = AccessTools.Method(typeof(LearningHistory), nameof(LearningHistory.RecordCollective));
        foreach (var type in new[] { typeof(RaceLanguage), typeof(Dialect) })
        {
            var original = PrisonerPatchTests.ReadInstructions(type, "AddXp");
            int first = original.FindIndex(i => i.Calls(record));
            original[first].labels.Add(new DynamicMethod("HistoryIL", typeof(void), Type.EmptyTypes)
                .GetILGenerator().DefineLabel());
            original[first].blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginExceptionBlock));
            var changed = IndividualLearningHistoryPatch.Transpiler(original).ToList();
            check(changed.Count == original.Count && changed.Count(i => i.Calls(helper)) == 2 &&
                !changed.Any(i => i.Calls(record)), "Replace only both installed event calls: " + type.Name);
            check(original.Where(i => !i.Calls(record)).SequenceEqual(changed.Where(i => !i.Calls(helper))),
                "Collective XP writes and original conditions remain intact: " + type.Name);
            check(changed[first].labels.SequenceEqual(original[first].labels) &&
                changed[first].blocks.SequenceEqual(original[first].blocks), "History replacement preserves branch metadata.");
            check(changed.Where(i => i.Calls(helper)).All(i => i.opcode == OpCodes.Call),
                "Replacement consumes manager, event, and thought flag with a static call.");
        }
        foreach (int count in new[] { 0, 1, 3 })
        {
            bool refused = false;
            try { LearningHistory.ReplaceCollectiveEvents(Enumerable.Range(0, count)
                .Select(i => new CodeInstruction(OpCodes.Callvirt, record))).ToList(); }
            catch (InvalidOperationException) { refused = true; }
            check(refused, "Refuse an incompatible installed event layout: " + count);
        }
        check(IndividualLearningHistoryPatch.TargetMethods().Count() == 2 &&
            IndividualLearningHistoryPatch.TargetMethods().All(m => m != null && m.Name == "AddXp"),
            "Only the two base learning methods are patched, not the global history manager.");
    }
}
