using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using FactionLanguages.Utilities;
using HarmonyLib;
using RimLanguages.IndividualLearning;
using RimWorld;
using Verse;

internal static class TradePatchTests
{
    public static void Run(Action<bool, string> check)
    {
        Verify(check, IndividualCommsMenuPatch.TargetMethods(), 0);
        Verify(check, IndividualTradePricePatch.TargetMethods(), 1);
        Verify(check, IndividualTraderAvailabilityPatch.TargetMethods(), 2);
        var actor = new PawnLanguages();
        var other = new PawnLanguages();
        actor.Gain("L", "L", false, 0.9999f);
        check(!TradeLanguages.Mastered(actor, "L"), "Incomplete language mastery cannot remove a trade penalty.");
        actor.Gain("L", "L", false, 1f);
        check(TradeLanguages.Mastered(actor, "L") && !TradeLanguages.Mastered(actor, "D"),
            "Mastered language and unknown dialect remain separate trade conditions.");
        check(!TradeLanguages.Mastered(other, "L") && !TradeLanguages.Mastered(actor, "Other"),
            "Another negotiator or another language does not inherit mastery.");
        check(!TradeLanguages.Mastered(null, "L") && !TradeLanguages.Mastered(actor, null), "Missing trade records are not mastery.");
        check(TradeLanguages.TradeAllowed(false, true) && TradeLanguages.TradeAllowed(true, false) &&
            !TradeLanguages.TradeAllowed(false, false), "Keep Cybranian's choice between penalized and forbidden trade.");
        check(IndividualCommsExecutionPatch.TargetMethods().Count() == 2 &&
            IndividualCommsExecutionPatch.TargetMethods().All(m => m != null && m.GetParameters().Single().ParameterType == typeof(Pawn)),
            "Both installed radio execution targets receive the actual negotiator.");
        check(AccessTools.Method(typeof(FactionUtility), nameof(FactionUtility.CanTradeWith),
            new[] { typeof(Pawn), typeof(Faction), typeof(TraderKindDef) }) != null &&
            AccessTools.Method(typeof(TradeDeal), nameof(TradeDeal.TryExecute), new[] { typeof(bool).MakeByRefType() }) != null,
            "Installed actor permission and transaction cancellation signatures resolve.");
        var language = AccessTools.Method(typeof(LanguageUtility), nameof(LanguageUtility.FactionLanguageIsLearned));
        for (int profile = 0; profile <= 2; profile++)
        {
            foreach (var invalid in new[] { new List<CodeInstruction>(), new List<CodeInstruction> {
                new CodeInstruction(OpCodes.Call, language), new CodeInstruction(OpCodes.Call, language) } })
            {
                bool refused = false;
                try { TradeLanguages.ReplaceChecks(invalid, profile).ToList(); }
                catch (InvalidOperationException) { refused = true; }
                check(refused, "Reject missing or ambiguous installed trade checks: " + profile);
            }
        }
    }

    private static void Verify(Action<bool, string> check, IEnumerable<MethodBase> targets, int profile)
    {
        var language = AccessTools.Method(typeof(LanguageUtility), nameof(LanguageUtility.FactionLanguageIsLearned));
        var dialect = AccessTools.Method(typeof(LanguageUtility), nameof(LanguageUtility.FactionDialectIsLearned));
        var languageHelper = AccessTools.Method(typeof(TradeLanguages), profile == 2 ? nameof(TradeLanguages.DeferredAvailability) : nameof(TradeLanguages.LanguageKnown));
        var dialectHelper = AccessTools.Method(typeof(TradeLanguages), nameof(TradeLanguages.DialectKnown));
        foreach (var target in targets)
        {
            var original = PrisonerPatchTests.ReadInstructions(target.DeclaringType, target.Name);
            var replaced = original.Where(i => i.Calls(language) || i.Calls(dialect)).ToList();
            var generator = new DynamicMethod("TradeIL", typeof(void), Type.EmptyTypes).GetILGenerator();
            foreach (var i in replaced)
            {
                i.labels.Add(generator.DefineLabel());
                i.blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginExceptionBlock));
            }
            var changed = TradeLanguages.ReplaceChecks(original, profile).ToList();
            int checks = profile == 1 ? 2 : 1;
            check(changed.Count == original.Count + (profile == 2 ? 0 : checks) &&
                changed.Count(i => i.Calls(languageHelper)) == 1 &&
                changed.Count(i => i.Calls(dialectHelper)) == (profile == 1 ? 1 : 0) &&
                changed.All(i => !i.Calls(language) && !i.Calls(dialect)), "Replace installed trade checks exactly: " + target.DeclaringType.Name);
            var injected = new HashSet<CodeInstruction>();
            foreach (var old in replaced)
            {
                var helper = old.Calls(dialect) ? dialectHelper : languageHelper;
                int at = changed.FindIndex(i => i.Calls(helper));
                injected.Add(changed[at]);
                var load = changed[profile == 2 ? at : at - 1];
                injected.Add(load);
                check(profile == 2 || (profile == 0 ? load.opcode == OpCodes.Ldarg_1 :
                    load.opcode == OpCodes.Ldsfld && Equals(load.operand, AccessTools.Field(typeof(TradeSession), nameof(TradeSession.playerNegotiator)))),
                    "Use the menu actor or verified session actor, never a previous session for menus.");
                check(old.labels.SequenceEqual(load.labels) && old.blocks.SequenceEqual(load.blocks),
                    "Preserve trade branch labels and exception metadata.");
            }
            check(original.Except(replaced).SequenceEqual(changed.Where(i => !injected.Contains(i))),
                "Preserve all prices, settings, tooltip text and non-language refusals: " + target.DeclaringType.Name);
        }
    }
}
