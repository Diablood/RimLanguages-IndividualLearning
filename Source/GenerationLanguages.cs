using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using FactionLanguages.Patches;
using FactionLanguages.Utilities;
using HarmonyLib;
using LudeonTK;
using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace RimLanguages.IndividualLearning
{
    // Quest candidates and incident factions are not the player's eventual interlocutor.
    // Defer only the language gate when the faction language is resolved; execution
    // remains guarded by the actual pawn's prison, trade, and comms checks.
    [HarmonyPatch]
    public static class IndividualGenerationLanguagePatch
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(QuestNode_GetPawn_IsGoodPawn_FactionLanguagesPatch), "Postfix");
            yield return AccessTools.Method(typeof(QuestNode_GetFaction_IsGoodFaction_FactionLanguagesPatch), "Postfix");
            yield return AccessTools.Method(typeof(IncidentWorker_TraderCaravanArrival_CanFireNowSub_FactionLanguagesPatch), "Postfix");
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            var original = AccessTools.Method(typeof(LanguageUtility), nameof(LanguageUtility.FactionLanguageIsLearned));
            if (code.Count(i => i.Calls(original)) != 1)
                throw new InvalidOperationException("RLI: unsupported Cybranian quest or trader generation language gate.");
            var replacement = AccessTools.Method(typeof(TradeLanguages), nameof(TradeLanguages.DeferredAvailability));
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

    public static class GenerationLanguageDeveloperActions
    {
        [DebugAction("Rim Languages", "RLI: inspect quest and trader language gates", allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void Inspect()
        {
            var map = Find.CurrentMap;
            var options = new List<DebugMenuOption>();
            foreach (var faction in Find.FactionManager.AllFactionsListForReading
                .Where(f => !f.IsPlayer && TradeLanguages.FactionDialect(f) != null).OrderBy(f => f.Name))
            {
                var selected = faction;
                options.Add(new DebugMenuOption(selected.Name, DebugMenuOptionMode.Action, () => Report(map, selected)));
            }
            Find.WindowStack.Add(new Dialog_DebugOptionListLister(options, "RLI_DevGenerationTitle".Translate()));
        }

        private static void Report(Map map, Faction faction)
        {
            if (!Prefs.DevMode || Find.CurrentMap != map || TradeLanguages.FactionDialect(faction) == null) return;
            var slate = new Slate();
            // Invoke the actual patched base filters, without generating an event or quest.
            var questFilter = AccessTools.Method(typeof(QuestNode_GetFaction_IsGoodFaction_FactionLanguagesPatch), "Postfix");
            bool quest = InspectFilter(questFilter, faction, slate, true);
            bool questRejected = InspectFilter(questFilter, faction, slate, false);
            var parms = new IncidentParms { faction = faction, target = map };
            var traderFilter = AccessTools.Method(typeof(IncidentWorker_TraderCaravanArrival_CanFireNowSub_FactionLanguagesPatch), "Postfix");
            bool trader = InspectFilter(traderFilter, parms, true);
            bool traderRejected = InspectFilter(traderFilter, parms, false);
            string pawnResult = "RLI_DevGenerationNoPawn".Translate();
            var candidate = faction.leader;
            if (candidate != null && candidate.Faction == faction)
            {
                var pawnFilter = AccessTools.Method(typeof(QuestNode_GetPawn_IsGoodPawn_FactionLanguagesPatch), "Postfix");
                bool pawn = InspectFilter(pawnFilter, null, candidate, slate, true);
                bool rejected = InspectFilter(pawnFilter, null, candidate, slate, false);
                pawnResult = pawn + " / " + rejected;
            }
            string report = "RLI_DevGenerationReport".Translate(new NamedArgument[] { faction.Name,
                LanguageUtility.FactionLanguageIsLearned(faction).ToString(), quest.ToString(), questRejected.ToString(),
                trader.ToString(), traderRejected.ToString(), pawnResult });
            Log.Message("[RLI generation] " + report);
            Find.WindowStack.Add(new Dialog_MessageBox(report));
        }

        private static bool InspectFilter(MethodInfo filter, params object[] arguments)
        {
            // Base Postfix methods are non-public; their verified last parameter is ref bool.
            filter.Invoke(null, arguments);
            return (bool)arguments[arguments.Length - 1];
        }
    }
}
