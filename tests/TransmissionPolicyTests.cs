using System;
using RimLanguages.IndividualLearning;

internal static class TransmissionPolicyTests
{
    public static void Run(Action<bool, string> check)
    {
        var teacher = new PawnLanguages();
        var student = new PawnLanguages();
        var native = new TransmissionLesson { key = "native", xpMax = 100f, baseCost = 100f, languageDifficulty = 1f };
        var learned = new TransmissionLesson { key = "learned", xpMax = 100f, baseCost = 100f, languageDifficulty = 1f };
        var dialect = new TransmissionLesson { key = "dialect", parentKey = "native", dialect = true,
            xpMax = 100f, baseCost = 100f, languageDifficulty = 1f };
        teacher.Gain("native", "Native", false, 1f, true);
        teacher.Gain("learned", "Learned", false, 1f);
        teacher.Gain("dialect", "Native dialect", true, 1f, true);
        check(TransmissionRules.Eligible(student, teacher, native, true), "A native mastered language can transmit within a colony.");
        check(!TransmissionRules.Eligible(student, teacher, learned, true), "A fully learned foreign language cannot spread naturally within a colony.");
        check(TransmissionRules.Eligible(student, teacher, learned, false), "Cross-faction conversation retains learned-language transmission.");
        student.Gain("learned", "Learned", false, 0.9f);
        check(TransmissionRules.Select(student, teacher, new[] { learned, native }, 0f, true) == native,
            "Started foreign knowledge cannot override the native-only colony filter.");
        check(Math.Abs(student.Progress("learned") - 0.9f) < 0.0001f, "Restricting transmission preserves previously learned progress.");
        check(!TransmissionRules.Eligible(student, teacher, dialect, true), "A native dialect still requires learner parent mastery.");
        student.Gain("native", "Native", false, 1f);
        check(TransmissionRules.Eligible(student, teacher, dialect, true), "A mastered native dialect transmits after the learner's language is mastered.");
        var learnedDialect = new TransmissionLesson { key = "foreignDialect", parentKey = "native", dialect = true, xpMax = 100f };
        teacher.Gain("foreignDialect", "Learned dialect", true, 1f);
        check(!TransmissionRules.Eligible(student, teacher, learnedDialect, true), "A foreign dialect stays excluded despite a native parent.");
        var inconsistent = new PawnLanguages();
        inconsistent.Gain("native", "Learned parent", false, 1f);
        inconsistent.Gain("dialect", "Marked dialect", true, 1f, true);
        check(!TransmissionRules.Eligible(student, inconsistent, dialect, true), "A native dialect marker alone cannot bypass a non-native parent.");
        teacher.DevSetDialectProgress("native", "Native", "dialect", "Dialect", 0.5f);
        check(!TransmissionRules.Eligible(student, teacher, dialect, true), "Native identity never substitutes for full mastery.");
        check(TransmissionRules.Select(student, teacher, new[] { learned, learnedDialect }, 0f, true) == null,
            "No eligible native knowledge means no colony lesson.");
        float baseline = TransmissionRules.Xp(native, 0f, 10, false, false, true, 1f, 1f);
        float passive = TransmissionRules.Xp(native, 0f, 10, true, false, true, 1f, 1f);
        check(passive > 0f && Math.Abs(passive - baseline / 2f) < 0.0001f,
            "Disabled-Social listeners learn at half the active level-zero baseline.");
        check(TransmissionRules.Xp(native, 100f, 10, true, false, true, 1f, 1f) == passive,
            "Passive listeners cannot benefit from a cached disabled skill stat.");
        check(TransmissionRules.Xp(native, 0f, 10, true, true, true, 1f, 1f) == 0f,
            "Two disabled-Social pawns cannot teach one another.");
        check(TransmissionRules.Xp(native, 0f, 10, false, true, true, 1f, 1f) == 0f,
            "A disabled-Social speaker cannot teach an active listener.");
        check(TransmissionRules.Xp(native, 0f, 10, true, false, false, 1f, 1f) == 0f,
            "The existing toggle also stops passive learning.");
        check(Math.Abs(TransmissionRules.Xp(native, 0f, 10, true, false, true, 2f, 1f) - passive / 2f) < 0.0001f,
            "Passive learning uses Cybranian difficulty without a new slider.");
        check(TransmissionRules.Xp(native, 0f, 0, true, false, true, 1f, 1f) > 0f,
            "An active level-zero speaker can still teach a passive listener.");
    }
}
