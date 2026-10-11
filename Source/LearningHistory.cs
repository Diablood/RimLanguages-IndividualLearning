using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using FactionLanguages;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RimLanguages.IndividualLearning
{
    [Flags]
    public enum LearningEvents { None = 0, Experience = 1, LanguageMastered = 2, DialectMastered = 4 }

    public static class LearningHistory
    {
        // Only a real increase in the current learning stage produces an event.
        public static LearningEvents Changes(float before, float after, bool dialect)
        {
            if (float.IsNaN(before) || float.IsNaN(after) || float.IsInfinity(before) || float.IsInfinity(after))
                return LearningEvents.None;
            before = ProgressMath.Clamp(before);
            after = ProgressMath.Clamp(after);
            if (after <= before) return LearningEvents.None;
            return LearningEvents.Experience | (after >= 1f ?
                dialect ? LearningEvents.DialectMastered : LearningEvents.LanguageMastered : LearningEvents.None);
        }

        public static void Record(Pawn learner, LearningEvents events)
        {
            if (!ModsConfig.IdeologyActive || events == LearningEvents.None || learner == null ||
                Scribe.mode != LoadSaveMode.Inactive || Current.ProgramState != ProgramState.Playing ||
                Find.TickManager == null || Find.TickManager.TicksGame <= 0 || Find.HistoryEventsManager == null) return;
            if ((events & LearningEvents.Experience) != 0) Emit(learner, HistoryEventDefOfLocal.LanguageExperienceGain);
            if ((events & LearningEvents.LanguageMastered) != 0) Emit(learner, HistoryEventDefOfLocal.LanguageLearned);
            if ((events & LearningEvents.DialectMastered) != 0) Emit(learner, HistoryEventDefOfLocal.DialectLearned);
        }

        private static void Emit(Pawn learner, HistoryEventDef def)
        {
            // The base's Ideology defs are optional; do not create substitutes.
            if (def != null)
                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(def, learner.Named(HistoryEventArgsNames.Doer)), true);
        }

        // Replaces only RecordEvent calls inside the two base AddXp methods.
        // XP still runs normally. Other callers of RecordEvent are untouched.
        public static void RecordCollective(HistoryEventsManager manager, HistoryEvent ev, bool selfThoughts)
        {
            Pawn doer;
            bool managed = ev.def != null && (ev.def == HistoryEventDefOfLocal.LanguageExperienceGain ||
                ev.def == HistoryEventDefOfLocal.LanguageLearned || ev.def == HistoryEventDefOfLocal.DialectLearned);
            if (ModsConfig.IdeologyActive && managed && ev.args.TryGetArg(HistoryEventArgsNames.Doer, out doer) &&
                IndividualLearning.Ready(doer) != null) return;
            manager.RecordEvent(ev, selfThoughts);
        }

        public static IEnumerable<CodeInstruction> ReplaceCollectiveEvents(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            var original = AccessTools.Method(typeof(HistoryEventsManager), nameof(HistoryEventsManager.RecordEvent),
                new[] { typeof(HistoryEvent), typeof(bool) });
            if (code.Count(i => i.Calls(original)) != 2)
                throw new InvalidOperationException("RLI: unsupported Cybranian learning history calls.");
            var replacement = AccessTools.Method(typeof(LearningHistory), nameof(RecordCollective));
            foreach (var instruction in code)
            {
                if (!instruction.Calls(original)) { yield return instruction; continue; }
                var call = new CodeInstruction(OpCodes.Call, replacement);
                call.labels.AddRange(instruction.labels);
                call.blocks.AddRange(instruction.blocks);
                yield return call;
            }
        }
    }

    [HarmonyPatch]
    public static class IndividualLearningHistoryPatch
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(RaceLanguage), nameof(RaceLanguage.AddXp), new[] { typeof(float), typeof(Pawn) });
            yield return AccessTools.Method(typeof(Dialect), nameof(Dialect.AddXp), new[] { typeof(float), typeof(Pawn) });
        }
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
            LearningHistory.ReplaceCollectiveEvents(instructions);
    }
}
