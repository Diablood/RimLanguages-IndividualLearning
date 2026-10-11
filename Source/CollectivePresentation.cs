using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using FactionLanguages;
using FactionLanguages.Patches;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimLanguages.IndividualLearning
{
    // Keep collective calculations and saved data; replace only their presentation.
    [HarmonyPatch(typeof(FactionUIUtility_DrawFactionRow_FactionLanguagesPatch), "Postfix")]
    public static class IndividualFactionRowPatch
    {
        // The target is itself a void Postfix: __3 refers to its ref float argument,
        // not to the return value of that void method.
        public static bool Prefix(Faction __0, ref float __1, Rect __2, ref float __3)
        {
            var db = CompIndividualLanguages.Database;
            Dialect dialect;
            if (__0 == null || db == null || !db.TryGetLanguage(__0, out dialect) ||
                dialect?.Parent == null) return true;

            var rect = new Rect(__2.x + 10f, __1 + __3, Math.Max(0f, __2.width - 20f), 26f);
            var anchor = Text.Anchor;
            var font = Text.Font;
            var color = GUI.color;
            try
            {
                Text.Anchor = TextAnchor.MiddleLeft;
                Text.Font = GameFont.Small;
                GUI.color = Color.white;
                string identity = "RLI_FactionLanguageIdentity".Translate(dialect.Parent.Name, dialect.Name);
                Widgets.Label(rect, identity.Truncate(rect.width));
                TooltipHandler.TipRegion(rect, identity + "\n" + "RLI_FactionIndividualLevels".Translate());
            }
            finally
            {
                Text.Anchor = anchor;
                Text.Font = font;
                GUI.color = color;
            }
            __3 += 30f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Pawn_InteractionsTracker_TryInteractWith_FactionLanguagesPatch), "Postfix")]
    public static class CollectiveConversationTextPatch
    {
        public static void HideText(Vector3 position, Map map, string text, float timeBeforeStartFadeout)
        {
            // Only calls from Cybranian's collective conversation Postfix reach this sink.
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = instructions.ToList();
            var original = AccessTools.Method(typeof(MoteMaker), nameof(MoteMaker.ThrowText),
                new[] { typeof(Vector3), typeof(Map), typeof(string), typeof(float) });
            if (code.Count(i => i.Calls(original)) != 2)
                throw new InvalidOperationException("RLI: unsupported Cybranian collective XP display calls.");
            var replacement = AccessTools.Method(typeof(CollectiveConversationTextPatch), nameof(HideText));
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
}
