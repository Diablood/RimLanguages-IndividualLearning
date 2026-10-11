using FactionLanguages.Patches;
using FactionLanguages.Utilities;
using HarmonyLib;
using LudeonTK;
using RimWorld;
using Verse;

namespace RimLanguages.IndividualLearning
{
    public static class SocialLanguages
    {
        public static bool Shared(PawnLanguages first, PawnLanguages second, string key) =>
            TradeLanguages.Mastered(first, key) && TradeLanguages.Mastered(second, key);

        public static float Apply(float value, bool language, bool dialect, float languageFactor, float dialectFactor) =>
            value > 0f ? value * (!language ? languageFactor : !dialect ? dialectFactor : 1f) : value;

        public static bool TryPair(Pawn first, Pawn second, out bool language, out bool dialect)
        {
            language = dialect = false;
            var a = IndividualLearning.Ready(first);
            var b = IndividualLearning.Ready(second);
            var db = CompIndividualLanguages.Database;
            if (a == null || b == null || db == null || db.RaceLanguages.Count == 0) return false;
            bool available = false;
            foreach (var candidate in db.RaceLanguages)
            {
                if (candidate.Def == null) continue;
                available = true;
                if (!Shared(a.Data, b.Data, candidate.GetUniqueLoadID())) continue;
                language = true;
                foreach (var variant in candidate.Dialects)
                    if (IndividualLearning.Valid(variant) && Shared(a.Data, b.Data, variant.GetUniqueLoadID()))
                        { dialect = true; return true; }
            }
            return available;
        }

        public static bool TryApply(Pawn first, Pawn second, float languageFactor, float dialectFactor, ref float value)
        {
            bool language, dialect;
            if (!TryPair(first, second, out language, out dialect)) return false;
            value = Apply(value, language, dialect, languageFactor, dialectFactor);
            return true;
        }
    }

    // Index aliases are intentional: the base Postfix parameter called ___pawn is
    // an ordinary argument here, and its ref float is not the void method's result.
    [HarmonyPatch(typeof(NegativeInteractionUtility_NegativeInteractionChanceFactor_FactionLanguagesPatch), "Postfix")]
    public static class IndividualNegativeSocialPatch
    {
        public static bool Prefix(Pawn __0, Pawn __1, ref float __2) =>
            !SocialLanguages.TryApply(__0, __1, LanguageUtility.Settings.negativeInteractionChanceFactorLanguage,
                LanguageUtility.Settings.negativeInteractionChanceFactorDialect, ref __2);
    }

    [HarmonyPatch(typeof(Pawn_RelationsTracker_SecondaryRomanceChanceFactor_FactionLanguagesPatch), "Postfix")]
    public static class IndividualRomanceSocialPatch
    {
        public static bool Prefix(Pawn __0, Pawn __1, ref float __2) =>
            !SocialLanguages.TryApply(__0, __1, LanguageUtility.Settings.secondaryRomanceChanceFactorLanguage,
                LanguageUtility.Settings.secondaryRomanceChanceFactorDialect, ref __2);
    }

    [HarmonyPatch(typeof(Pawn_InteractionsTracker_SocialFightChance_FactionLanguagesPatch), "Postfix")]
    public static class IndividualFightSocialPatch
    {
        public static bool Prefix(Pawn __0, Pawn __2, ref float __3) =>
            !SocialLanguages.TryApply(__0, __2, LanguageUtility.Settings.socialFightChanceLanguage,
                LanguageUtility.Settings.socialFightChanceDialect, ref __3);
    }

    public static class SocialLanguageDiagnostics
    {
        [DebugAction("Rim Languages", "RLI: inspect social language factors", allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void InspectPair()
        {
            Pawn first = null;
            var map = Find.CurrentMap;
            System.Action click = null;
            click = () =>
            {
                if (Find.CurrentMap != map) { DebugTools.curTool = null; return; }
                var pawn = UI.MouseCell().GetFirstPawn(map);
                if (IndividualLearning.Ready(pawn) == null) return;
                if (first == null)
                {
                    first = pawn;
                    DebugTools.curTool = new DebugTool("RLI_DevSocialSecond".Translate(first.LabelShortCap), click);
                    return;
                }
                DebugTools.curTool = null;
                bool language, dialect;
                if (pawn == first || !SocialLanguages.TryPair(first, pawn, out language, out dialect)) return;
                var settings = LanguageUtility.Settings;
                string text = first.LabelShortCap + " -> " + pawn.LabelShortCap +
                    "\nShared mastered language: " + language + "; dialect: " + dialect +
                    "\nLanguage multipliers (negative / romance / fight): " +
                    SocialLanguages.Apply(1f, language, dialect, settings.negativeInteractionChanceFactorLanguage, settings.negativeInteractionChanceFactorDialect) + " / " +
                    SocialLanguages.Apply(1f, language, dialect, settings.secondaryRomanceChanceFactorLanguage, settings.secondaryRomanceChanceFactorDialect) + " / " +
                    SocialLanguages.Apply(1f, language, dialect, settings.socialFightChanceLanguage, settings.socialFightChanceDialect);
                // Query the game's patched calculations; never trigger an interaction or RNG roll.
                if (first.story != null && first.relations != null && pawn.relations != null && pawn.interactions != null)
                    text += "\nGame negative factor: " + NegativeInteractionUtility.NegativeInteractionChanceFactor(first, pawn) +
                        "\nGame romance factor: " + first.relations.SecondaryRomanceChanceFactor(pawn) +
                        "\nGame fight chance after insult: " + pawn.interactions.SocialFightChance(InteractionDefOf.Insult, first).ToString("P2");
                Log.Message("[RLI social] " + text);
                Find.WindowStack.Add(new Dialog_MessageBox(text));
            };
            DebugTools.curTool = new DebugTool("RLI_DevSocialFirst".Translate(), click);
        }
    }
}
