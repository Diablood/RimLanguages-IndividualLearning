using System;
using System.Linq;
using HarmonyLib;
using RimLanguages.IndividualLearning;
using RimWorld;

internal static class TransmissionTests
{
    public static void Run(Action<bool, string> check)
    {
        check(Math.Abs(TransmissionRules.AppliedPercentage(0.99f, 1f) - 1f) < 0.0001f,
            "Transmission feedback shows actual capped gain, not requested XP or total mastery.");
        check(TransmissionRules.AppliedPercentage(1f, 1f) == 0f &&
            TransmissionRules.AppliedPercentage(0.5f, 0.4f) == 0f,
            "No positive personal gain means no transmission notification.");
        check(TransmissionRules.AppliedPercentage(float.NaN, 1f) == 0f &&
            TransmissionRules.AppliedPercentage(0f, float.PositiveInfinity) == 0f,
            "Invalid progress cannot produce a false notification.");
        var teacher = new PawnLanguages();
        var student = new PawnLanguages();
        var language = new TransmissionLesson { key = "L", name = "Learned", xpMax = 100f,
            baseCost = 100f, languageDifficulty = 1f };
        var dialect = new TransmissionLesson { key = "D", parentKey = "L", dialect = true,
            xpMax = 100f, baseCost = 100f, languageDifficulty = 1f };
        teacher.Gain("L", "Learned", false, 0.9999f);
        check(!TransmissionRules.Eligible(student, teacher, language), "Partial mastery never permits transmission.");
        teacher.Gain("L", "Learned", false, 1f);
        check(TransmissionRules.Eligible(student, teacher, language) && !teacher.Find("L").native,
            "Learned languages transmit at full mastery, without a native marker.");
        teacher.Gain("D", "Dialect", true, 1f);
        check(!TransmissionRules.Eligible(student, teacher, dialect), "Dialect requires learner parent mastery.");
        student.Gain("L", "L", false, 1f);
        check(!TransmissionRules.Eligible(student, teacher, language) && TransmissionRules.Eligible(student, teacher, dialect),
            "A mastered parent switches the useful lesson to dialect.");
        teacher.DevSetLanguageProgress("L", "L", new string[0], 0.9f);
        check(!TransmissionRules.Eligible(student, teacher, dialect), "A teacher's orphan dialect cannot transmit.");
        teacher.Gain("L", "L", false, 1f);
        teacher.DevSetDialectProgress("L", "L", "D", "D", 0.9999f);
        check(!TransmissionRules.Eligible(student, teacher, dialect), "Dialect itself requires full teacher mastery.");
        teacher.Gain("D", "D", true, 1f);
        student.Gain("D", "D", true, 1f);
        check(TransmissionRules.Select(student, teacher, new[] { language, dialect }, 0f) == null,
            "No useful target means no transmission, even if teacher is fluent.");

        var beginner = new PawnLanguages();
        var native = new TransmissionLesson { key = "N", xpMax = 100f };
        teacher.Gain("N", "Native", false, 1f, true);
        check(TransmissionRules.Select(beginner, teacher, new[] { language, native }, 0f) == native,
            "Prefer native knowledge when no eligible language has been started.");
        beginner.Gain("L", "L", false, 0.1f);
        check(TransmissionRules.Select(beginner, teacher, new[] { native, language }, 0.99f) == language,
            "Continue a started learned language before introducing a new native language.");
        beginner.Gain("N", "N", false, 0.1f);
        check(TransmissionRules.Select(beginner, teacher, new[] { language, native }, 0f) == language &&
            TransmissionRules.Select(beginner, teacher, new[] { language, native }, 1f) == native,
            "Equally useful targets can both be selected without index overflow.");
        check(TransmissionRules.Select(beginner, teacher, new[] { language }, float.NaN) == null &&
            !TransmissionRules.Eligible(null, teacher, language), "Invalid selection cannot create a lesson.");

        Func<float, int, float, float> xp = (ability, social, difficulty) =>
            TransmissionRules.Xp(language, ability, social, false, false, true, difficulty, 1f);
        float normal = xp(0.65f, 10, 1f);
        check(xp(0f, 0, 1f) > 0f && xp(0f, 0, 1f) < normal, "Social level zero remains slow but usable.");
        check(xp(1.3f, 10, 1f) > normal && xp(0.65f, 20, 1f) > normal,
            "Both learner aptitude and speaker Social raise transmission speed.");
        check(Math.Abs(xp(0.65f, 10, 2f) - normal / 2f) < 0.0001f,
            "Parent difficulty alone scales automatic transmission XP.");
        check(TransmissionRules.Xp(language, 1f, 10, false, false, false, 1f, 1f) == 0f,
            "The toggle stops automatic gains without a second speed multiplier.");
        check(TransmissionRules.Xp(language, 1f, 10, true, false, true, 1f, 1f) > 0f &&
            TransmissionRules.Xp(language, 1f, 10, false, true, true, 1f, 1f) == 0f,
            "Disabled Social allows passive listening but prevents teaching.");
        check(xp(float.NaN, 10, 1f) == 0f && xp(1f, 10, float.PositiveInfinity) == 0f &&
            TransmissionRules.Xp(language, 1f, 10, false, false, true, 0f, 1f) == 0f,
            "Reject nonfinite or invalid transmission factors.");
        check(!TransmissionSettings.LegacyEnabled(0f) && !TransmissionSettings.LegacyEnabled(-1f),
            "An old disabled speed setting remains disabled after migration.");
        check(TransmissionSettings.LegacyEnabled(0.5f) && TransmissionSettings.LegacyEnabled(3f),
            "Positive old speed settings migrate to enabled without retaining a multiplier.");
        check(TransmissionSettings.LegacyEnabled(float.NaN) && TransmissionSettings.LegacyEnabled(float.PositiveInfinity),
            "Invalid old settings keep the former safe enabled default.");

        var hook = AccessTools.Method(typeof(IndividualConversationPatch), "Postfix");
        var outer = new InteractionPresentation(null, null);
        var inner = new InteractionPresentation(null, null);
        check(InteractionPresentation.Current == inner, "Nested interaction uses its own presentation context.");
        inner.End();
        check(InteractionPresentation.Current == outer, "Nested cleanup restores the outer interaction.");
        outer.End();
        check(InteractionPresentation.Current == null, "Final cleanup retains no pawn or bubble context.");
        check(AccessTools.Method(typeof(IndividualConversationPatch), "Finalizer") != null,
            "Presentation cleanup also runs when an interaction throws.");
        var grantCode = PrisonerPatchTests.ReadInstructions(typeof(AutomaticTransmission), "Grant");
        check(!grantCode.Any(i => i.operand is System.Reflection.MethodInfo m && m.Name == "ThrowText"),
            "Individual transmission no longer produces numerical floating text.");
        var bubbleMethod = AccessTools.Method(typeof(MoteMaker), "MakeInteractionBubble");
        var captureMethod = AccessTools.Method(typeof(CaptureInteractionBubblePatch), "Postfix");
        check(bubbleMethod.ReturnType == typeof(MoteBubble) && captureMethod.GetParameters().All(p =>
            p.Name == "__result" ? p.ParameterType == bubbleMethod.ReturnType :
            bubbleMethod.GetParameters().Any(original => original.Name == p.Name && original.ParameterType == p.ParameterType)),
            "Bubble capture matches the installed engine's exact arguments and result.");
        var presentationCode = PrisonerPatchTests.ReadInstructions(typeof(InteractionPresentation), "MarkTransmission");
        check(presentationCode.Count(i => i.operand is System.Reflection.MethodInfo m && m.Name == "SetupMoteBubble") == 1 &&
            !presentationCode.Any(i => i.operand is System.Reflection.MethodInfo m &&
                (m.Name == "MakeInteractionBubble" || m.Name == "ThrowText" || m.DeclaringType == typeof(Verse.Rand))),
            "Presentation reuses the existing bubble without creating another or consuming random numbers.");
        var engine = AccessTools.Method(typeof(Pawn_InteractionsTracker), "TryInteractWith");
        check(engine.ReturnType == typeof(bool) && engine.GetParameters().Any(p => p.Name == "intDef" &&
            p.ParameterType == typeof(InteractionDef)) && hook.GetParameters().Any(p => p.Name == "intDef" &&
            p.ParameterType == typeof(InteractionDef)), "The installed interaction argument matches the postfix.");
        var code = PrisonerPatchTests.ReadInstructions(typeof(IndividualConversationPatch), "Postfix");
        check(code.Count(i => i.Calls(AccessTools.Method(typeof(AutomaticTransmission), "Exchange"))) == 1 &&
            code.Count(i => i.Calls(AccessTools.Method(typeof(IndividualLearning), "Conversation"))) == 2,
            "One hook routes automatic exchange and the two legacy prisoner gains separately.");
    }
}
