using System;
using System.Linq;
using System.Reflection;
using FactionLanguages.Patches;
using HarmonyLib;
using RimLanguages.IndividualLearning;
using Verse;

internal static class SocialTests
{
    public static void Run(Action<bool, string> check)
    {
        var first = new PawnLanguages();
        var second = new PawnLanguages();
        var third = new PawnLanguages();
        first.Gain("A", "A", false, 1f, true);
        second.Gain("B", "B", false, 1f, true);
        check(!SocialLanguages.Shared(first, second, "A") && !SocialLanguages.Shared(first, second, "B"),
            "Different native languages do not imply a shared language.");
        first.Gain("B", "B", false, 0.9999f);
        check(!SocialLanguages.Shared(first, second, "B"), "Partial understanding does not remove the configured language factor.");
        first.Gain("B", "B", false, 1f);
        check(SocialLanguages.Shared(first, second, "B") && SocialLanguages.Shared(second, first, "B"),
            "Learning the other's language produces symmetric shared mastery.");
        check(!SocialLanguages.Shared(first, third, "B") && third.entries.Count == 0,
            "Shared mastery never grants knowledge to a bystander.");
        first.Gain("C", "C", false, 1f);
        second.Gain("C", "C", false, 1f);
        check(SocialLanguages.Shared(first, second, "C") && !first.Find("C").native && !second.Find("C").native,
            "A third language learned by both can support communication.");
        first.Gain("BD", "BD", true, 1f);
        second.Gain("BD", "BD", true, 0.99f);
        check(!SocialLanguages.Shared(first, second, "BD"), "A dialect incomplete in either pawn remains incomplete for the pair.");
        second.Gain("BD", "BD", true, 1f);
        check(SocialLanguages.Shared(first, second, "BD"), "Both pawns mastering the same dialect removes its factor.");
        first.DevSetLanguageProgress("B", "B", new[] { "BD" }, 0.5f);
        check(!SocialLanguages.Shared(first, second, "B") && !SocialLanguages.Shared(first, second, "BD") &&
            SocialLanguages.Shared(first, second, "C"), "An edit changes only its language; another shared language remains usable.");
        check(!SocialLanguages.Shared(null, second, "B") && !SocialLanguages.Shared(first, second, null),
            "Missing individual data cannot prove shared mastery.");
        check(SocialLanguages.Apply(0.2f, false, false, 2f, 1.5f) == 0.4f,
            "Unknown language applies only the language factor, without cumulative dialect multiplication.");
        check(Math.Abs(SocialLanguages.Apply(0.2f, true, false, 2f, 1.5f) - 0.3f) < 0.00001f,
            "A shared language with different dialects uses the configured dialect factor.");
        check(SocialLanguages.Apply(0.2f, true, true, 2f, 1.5f) == 0.2f,
            "Shared language and dialect preserve the game's input factor.");
        check(SocialLanguages.Apply(0f, false, false, 2f, 1.5f) == 0f &&
            SocialLanguages.Apply(-1f, false, false, 2f, 1.5f) == -1f &&
            float.IsNaN(SocialLanguages.Apply(float.NaN, false, false, 2f, 1.5f)),
            "Do not resurrect vanilla refusals or transform nonpositive/NaN results.");
        check(SocialLanguages.Apply(0.2f, false, false, 0f, 0f) == 0f &&
            SocialLanguages.Apply(0.2f, false, false, 0.5f, 0.8f) == 0.1f,
            "Configured zero and reduced factors remain authoritative.");
        VerifyBinding(check, typeof(NegativeInteractionUtility_NegativeInteractionChanceFactor_FactionLanguagesPatch),
            typeof(IndividualNegativeSocialPatch), 1, 2, "negativeInteractionChanceFactor");
        VerifyBinding(check, typeof(Pawn_RelationsTracker_SecondaryRomanceChanceFactor_FactionLanguagesPatch),
            typeof(IndividualRomanceSocialPatch), 1, 2, "secondaryRomanceChanceFactor");
        VerifyBinding(check, typeof(Pawn_InteractionsTracker_SocialFightChance_FactionLanguagesPatch),
            typeof(IndividualFightSocialPatch), 2, 3, "socialFightChance");
    }

    private static void VerifyBinding(Action<bool, string> check, Type originalType, Type patchType,
        int otherIndex, int valueIndex, string setting)
    {
        var target = AccessTools.Method(originalType, "Postfix");
        var args = target.GetParameters();
        var prefix = patchType.GetMethod("Prefix");
        var added = prefix.GetParameters();
        check(target.ReturnType == typeof(void) && args[0].ParameterType == typeof(Pawn) &&
            args[otherIndex].ParameterType == typeof(Pawn) && args[valueIndex].ParameterType == typeof(float).MakeByRefType(),
            "Installed social Postfix signature matches the real pair and ref factor: " + originalType.Name);
        check(added.Select(p => p.Name).SequenceEqual(new[] { "__0", "__" + otherIndex, "__" + valueIndex }) &&
            added.Select(p => p.ParameterType).SequenceEqual(new[] { typeof(Pawn), typeof(Pawn), typeof(float).MakeByRefType() }) &&
            prefix.ReturnType == typeof(bool), "Harmony argument aliases avoid field/result injection into the base void Postfix.");
        var original = PrisonerPatchTests.ReadInstructions(originalType, "Postfix");
        var patched = PrisonerPatchTests.ReadInstructions(patchType, "Prefix");
        foreach (var suffix in new[] { "Language", "Dialect" })
        {
            string name = "::" + setting + suffix;
            check(original.Any(i => i.operand != null && i.operand.ToString().Contains(name)) &&
                patched.Any(i => i.operand != null && i.operand.ToString().Contains(name)),
                "Use the installed original setting for " + setting + suffix);
        }
    }
}
