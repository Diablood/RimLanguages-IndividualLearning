using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using FactionLanguages;
using FactionLanguages.Patches;
using FactionLanguages.Utilities;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RimLanguages.IndividualLearning
{
    public static class TradeLanguages
    {
        public static bool Mastered(PawnLanguages data, string key) =>
            data != null && !string.IsNullOrEmpty(key) && data.Progress(key) >= 1f;

        public static bool TradeAllowed(bool languageKnown, bool allowUnknownLanguage) =>
            languageKnown || allowUnknownLanguage;

        public static Dialect FactionDialect(Faction faction)
        {
            Dialect dialect;
            var db = CompIndividualLanguages.Database;
            return db != null && db.TryGetLanguage(faction, out dialect) && IndividualLearning.Valid(dialect)
                ? dialect : null;
        }

        public static bool LanguageKnown(Faction faction, Pawn negotiator)
        {
            var actor = IndividualLearning.Ready(negotiator);
            var dialect = FactionDialect(faction);
            return actor != null && dialect != null ? Mastered(actor.Data, dialect.Parent.GetUniqueLoadID()) :
                LanguageUtility.FactionLanguageIsLearned(faction);
        }

        public static bool DialectKnown(Faction faction, Pawn negotiator)
        {
            var actor = IndividualLearning.Ready(negotiator);
            var dialect = FactionDialect(faction);
            return actor != null && dialect != null ? Mastered(actor.Data, dialect.Parent.GetUniqueLoadID()) &&
                Mastered(actor.Data, dialect.GetUniqueLoadID()) : LanguageUtility.FactionDialectIsLearned(faction);
        }

        // Availability and generation have no player actor. Defer only the linguistic
        // rejection; prison, comms, and trade execution check the real actor.
        public static bool DeferredAvailability(Faction faction) =>
            FactionDialect(faction) != null || LanguageUtility.FactionLanguageIsLearned(faction);

        public static bool CanTrade(Faction faction, Pawn negotiator) =>
            faction == null || TradeAllowed(LanguageKnown(faction, negotiator), LanguageUtility.Settings.CanTradeWithSettings);

        public static bool CanCall(Faction faction, Pawn negotiator) =>
            faction == null || faction.IsPlayer || LanguageKnown(faction, negotiator);

        // Profile: 0 = radio argument, 1 = active trade session, 2 = no actor yet.
        public static IEnumerable<CodeInstruction> ReplaceChecks(IEnumerable<CodeInstruction> instructions, int profile)
        {
            var code = instructions.ToList();
            var language = AccessTools.Method(typeof(LanguageUtility), nameof(LanguageUtility.FactionLanguageIsLearned));
            var dialect = AccessTools.Method(typeof(LanguageUtility), nameof(LanguageUtility.FactionDialectIsLearned));
            if (profile < 0 || profile > 2 || code.Count(i => i.Calls(language)) != 1 ||
                code.Count(i => i.Calls(dialect)) != (profile == 1 ? 1 : 0))
                throw new InvalidOperationException("RLI: unsupported Cybranian trade or communication language checks.");
            foreach (var instruction in code)
            {
                bool isDialect = instruction.Calls(dialect);
                if (!instruction.Calls(language) && !isDialect) { yield return instruction; continue; }
                var helper = AccessTools.Method(typeof(TradeLanguages), profile == 2 ? nameof(DeferredAvailability) :
                    isDialect ? nameof(DialectKnown) : nameof(LanguageKnown));
                var replacement = profile == 2 ? new CodeInstruction(OpCodes.Call, helper) : profile == 0 ?
                    new CodeInstruction(OpCodes.Ldarg_1) :
                    new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(TradeSession), nameof(TradeSession.playerNegotiator)));
                replacement.labels.AddRange(instruction.labels);
                replacement.blocks.AddRange(instruction.blocks);
                yield return replacement;
                if (profile != 2) yield return new CodeInstruction(OpCodes.Call, helper);
            }
        }
    }

    [HarmonyPatch]
    public static class IndividualCommsMenuPatch
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(Faction_CommFloatMenuOption_FactionLanguagesPatch), "Postfix");
            yield return AccessTools.Method(typeof(PassingShip_CommFloatMenuOption_FactionLanguagesPatch), "Postfix");
        }
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
            TradeLanguages.ReplaceChecks(instructions, 0);
    }

    [HarmonyPatch]
    public static class IndividualTradePricePatch
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(TradeUtility_GetPricePlayerBuy_FactionLanguagesPatch), "Postfix");
            yield return AccessTools.Method(typeof(TradeUtility_GetPricePlayerSell_FactionLanguagesPatch), "Postfix");
            yield return AccessTools.Method(typeof(Tradeable_GetPriceTooltip_FactionLanguagesPatch), "Postfix");
        }
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
            TradeLanguages.ReplaceChecks(instructions, 1);
    }

    [HarmonyPatch]
    public static class IndividualTraderAvailabilityPatch
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(Pawn_CanTradeNow_FactionLanguagesPatch), "Postfix");
            yield return AccessTools.Method(typeof(TradeShip_CanTradeNow_FactionLanguagesPatch), "Postfix");
            yield return AccessTools.Method(typeof(Settlement_CanTradeNow_FactionLanguagesPatch), "Postfix");
            yield return AccessTools.Method(typeof(Caravan_CanTradeNow_FactionLanguagesPatch), "Postfix");
        }
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
            TradeLanguages.ReplaceChecks(instructions, 2);
    }

    [HarmonyPatch(typeof(FactionUtility), nameof(FactionUtility.CanTradeWith),
        new[] { typeof(Pawn), typeof(Faction), typeof(TraderKindDef) })]
    public static class IndividualTradePermissionPatch
    {
        [HarmonyPriority(Priority.Last)]
        public static void Postfix(Pawn p, Faction faction, ref AcceptanceReport __result)
        {
            if (__result.Accepted && !TradeLanguages.CanTrade(faction, p))
                __result = new AcceptanceReport("RLI_TradeLanguageRequired".Translate());
        }
    }

    [HarmonyPatch]
    public static class IndividualCommsExecutionPatch
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(Faction), nameof(Faction.TryOpenComms), new[] { typeof(Pawn) });
            yield return AccessTools.Method(typeof(TradeShip), nameof(TradeShip.TryOpenComms), new[] { typeof(Pawn) });
        }
        public static bool Prefix(object __instance, Pawn negotiator)
        {
            var faction = __instance as Faction ?? (__instance as TradeShip)?.Faction;
            if (TradeLanguages.CanCall(faction, negotiator)) return true;
            Messages.Message("RLI_TradeLanguageRequired".Translate(), MessageTypeDefOf.RejectInput, false);
            return false;
        }
    }

    [HarmonyPatch(typeof(TradeDeal), nameof(TradeDeal.TryExecute))]
    public static class IndividualTradeExecutionPatch
    {
        public static bool Prefix(ref bool actuallyTraded, ref bool __result)
        {
            if (TradeLanguages.CanTrade(TradeSession.trader?.Faction, TradeSession.playerNegotiator)) return true;
            actuallyTraded = false;
            __result = false;
            Messages.Message("RLI_TradeLanguageRequired".Translate(), MessageTypeDefOf.RejectInput, false);
            return false;
        }
    }
}
