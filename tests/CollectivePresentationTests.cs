using System;
using System.Linq;
using System.Reflection.Emit;
using FactionLanguages.Patches;
using HarmonyLib;
using RimLanguages.IndividualLearning;
using RimWorld;
using UnityEngine;
using Verse;

internal static class CollectivePresentationTests
{
    public static void Run(Action<bool, string> check)
    {
        var original = PrisonerPatchTests.ReadInstructions(
            typeof(Pawn_InteractionsTracker_TryInteractWith_FactionLanguagesPatch), "Postfix");
        var draw = AccessTools.Method(typeof(MoteMaker), nameof(MoteMaker.ThrowText),
            new[] { typeof(Vector3), typeof(Map), typeof(string), typeof(float) });
        var sink = AccessTools.Method(typeof(CollectiveConversationTextPatch), nameof(CollectiveConversationTextPatch.HideText));
        int first = original.FindIndex(i => i.Calls(draw));
        original[first].labels.Add(new DynamicMethod("CollectiveTextIL", typeof(void), Type.EmptyTypes)
            .GetILGenerator().DefineLabel());
        original[first].blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginExceptionBlock));
        var changed = CollectiveConversationTextPatch.Transpiler(original).ToList();
        check(changed.Count == original.Count && changed.Count(i => i.Calls(sink)) == 2 &&
            !changed.Any(i => i.Calls(draw)), "Hide both collective conversation texts, without removing the Postfix.");
        check(original.Where(i => !i.Calls(draw)).SequenceEqual(changed.Where(i => !i.Calls(sink))),
            "Collective XP, conditions and every non-display instruction remain intact.");
        check(changed[first].labels.SequenceEqual(original[first].labels) &&
            changed[first].blocks.SequenceEqual(original[first].blocks), "Hidden text calls preserve branch and exception metadata.");
        check(draw.GetParameters().Select(p => p.ParameterType).SequenceEqual(sink.GetParameters().Select(p => p.ParameterType)),
            "Text replacement consumes the exact installed four-argument stack.");
        foreach (int count in new[] { 0, 1, 3 })
        {
            bool refused = false;
            try { CollectiveConversationTextPatch.Transpiler(Enumerable.Range(0, count)
                .Select(i => new CodeInstruction(OpCodes.Call, draw))).ToList(); }
            catch (InvalidOperationException) { refused = true; }
            check(refused, "Refuse unexpected collective display layout: " + count);
        }
        var row = AccessTools.Method(typeof(FactionUIUtility_DrawFactionRow_FactionLanguagesPatch), "Postfix");
        check(row.ReturnType == typeof(void) && row.GetParameters().Select(p => p.ParameterType)
            .SequenceEqual(new[] { typeof(Faction), typeof(float).MakeByRefType(), typeof(Rect), typeof(float).MakeByRefType() }),
            "Faction row patch addresses the installed void Postfix and its ref height argument.");
    }
}
