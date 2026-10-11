using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using FactionLanguages.Patches;
using FactionLanguages.Utilities;
using HarmonyLib;
using RimLanguages.IndividualLearning;

internal static class GenerationPatchTests
{
    public static void Run(Action<bool, string> check)
    {
        var global = AccessTools.Method(typeof(LanguageUtility), nameof(LanguageUtility.FactionLanguageIsLearned));
        var helper = AccessTools.Method(typeof(TradeLanguages), nameof(TradeLanguages.DeferredAvailability));
        var types = new[] { typeof(QuestNode_GetPawn_IsGoodPawn_FactionLanguagesPatch),
            typeof(QuestNode_GetFaction_IsGoodFaction_FactionLanguagesPatch),
            typeof(IncidentWorker_TraderCaravanArrival_CanFireNowSub_FactionLanguagesPatch) };
        foreach (var type in types)
        {
            var method = AccessTools.Method(type, "Postfix");
            var original = PrisonerPatchTests.ReadInstructions(type, "Postfix");
            check(original.Count(i => i.Calls(global)) == 1, "Installed generation method has one linguistic gate: " + type.Name);
            int at = original.FindIndex(i => i.Calls(global));
            original[at].labels.Add(new DynamicMethod("GenerationIL", typeof(void), Type.EmptyTypes)
                .GetILGenerator().DefineLabel());
            original[at].blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginExceptionBlock));
            var changed = IndividualGenerationLanguagePatch.Transpiler(original).ToList();
            check(changed.Count == original.Count && changed.Count(i => i.Calls(helper)) == 1 &&
                !changed.Any(i => i.Calls(global)), "Replace exactly one generation gate: " + type.Name);
            check(original.Take(at).SequenceEqual(changed.Take(at)) &&
                original.Skip(at + 1).SequenceEqual(changed.Skip(at + 1)),
                "Preserve initial rejection, null-faction guard and trade settings: " + type.Name);
            check(changed[at].opcode == OpCodes.Call && changed[at].labels.SequenceEqual(original[at].labels) &&
                changed[at].blocks.SequenceEqual(original[at].blocks), "Preserve generation branch and exception metadata.");
            check(method.IsStatic && method.ReturnType == typeof(void) &&
                method.GetParameters().Last().ParameterType == typeof(bool).MakeByRefType(),
                "Verified base filter accepts an existing ref bool result: " + type.Name);
        }
        var targets = IndividualGenerationLanguagePatch.TargetMethods().ToList();
        check(targets.Count == 3 && targets.All(m => m != null) &&
            types.All(t => targets.Contains(AccessTools.Method(t, "Postfix"))),
            "Patch only the three installed Cybranian filters, not all quest or incident workers.");
        foreach (int count in new[] { 0, 2 })
        {
            bool refused = false;
            try { IndividualGenerationLanguagePatch.Transpiler(Enumerable.Range(0, count)
                .Select(i => new CodeInstruction(OpCodes.Call, global))).ToList(); }
            catch (InvalidOperationException) { refused = true; }
            check(refused, "Refuse an unsupported generation gate layout: " + count);
        }
    }
}
