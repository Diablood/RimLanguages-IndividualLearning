using System;
using System.IO;
using System.Reflection;

internal static class KnowledgeTests
{
    private static string root;
    private static string managed;
    private static int checks;
    private static string[] dependencyDirs;
    public static int Main(string[] args)
    {
        root = args[0]; managed = args[1];
        dependencyDirs = new[] { Path.Combine(root, "1.6", "Assemblies"), managed, args[2], args[3], args[4] };
        AppDomain.CurrentDomain.AssemblyResolve += Resolve;
        try { Run(); Console.WriteLine("PASS: " + checks + " knowledge assertions (outside RimWorld)."); return 0; }
        catch (Exception e) { Console.Error.WriteLine(e); return 1; }
    }
    private static Assembly Resolve(object sender, ResolveEventArgs args)
    {
        string file = new AssemblyName(args.Name).Name + ".dll";
        foreach (string dir in dependencyDirs)
            if (File.Exists(Path.Combine(dir, file))) return Assembly.LoadFrom(Path.Combine(dir, file));
        return null;
    }
    private static void Check(bool value, string description)
    {
        checks++; if (!value) throw new Exception(description);
    }
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static void Run()
    {
        var first = new RimLanguages.IndividualLearning.PawnLanguages();
        var second = new RimLanguages.IndividualLearning.PawnLanguages();
        first.Gain("RaceLanguage_1", "Original", false, 1, true);
        first.Gain("Dialect_1", "Dialect", true, 1, true);
        first.Gain("RaceLanguage_2", "Foreign", false, 0.1f);
        Check(second.entries.Count == 0, "A second pawn must not share entries.");
        Check(first.entries.Count == 3, "Language and dialect IDs must stay distinct.");
        first.Gain("RaceLanguage_2", "Renamed", false, 0.2f);
        Check(Math.Abs(first.Find("RaceLanguage_2").progress - 0.3f) < 0.0001, "Accumulate personal progress.");
        Check(first.Find("RaceLanguage_2").lastKnownName == "Renamed", "Names do not define identity.");
        first.Gain("RaceLanguage_2", "Renamed", false, 50f);
        Check(first.Find("RaceLanguage_2").progress == 1f, "Cap at mastery.");
        Check(!first.Find("RaceLanguage_2").native, "Learning must not imply native knowledge.");
        Check(first.Find("Dialect_1").native, "Preserve native dialect.");
        first.Gain("bad", "bad", false, float.NaN);
        first.Gain("bad", "bad", false, float.PositiveInfinity);
        first.Gain("bad", "bad", false, -1);
        Check(first.Find("bad") == null, "Reject invalid or negative increments.");
        Check(RimLanguages.IndividualLearning.ProgressMath.Clamp(float.NaN) == 0, "Repair invalid saved progress.");
        Check(RimLanguages.IndividualLearning.ProgressMath.Clamp(-4) == 0, "Clamp negative saved progress.");
        Check(RimLanguages.IndividualLearning.ProgressMath.Clamp(4) == 1, "Clamp excessive saved progress.");
        LearningScenarios();
        DeveloperEditScenarios();
        PrisonerPatchTests.Run(Check);
        TradePatchTests.Run(Check);
        SocialTests.Run(Check);
        LearningHistoryTests.Run(Check);
        GenerationPatchTests.Run(Check);
        MigrationTests.Run(Check);
        TransmissionTests.Run(Check);
        ChildLearningTests.Run(Check);
        CollectivePresentationTests.Run(Check);
    }

    private static void DeveloperEditScenarios()
    {
        var pawn = new RimLanguages.IndividualLearning.PawnLanguages();
        var other = new RimLanguages.IndividualLearning.PawnLanguages();
        Check(pawn.DevSetDialectProgress("L", "Language", "D", "Dialect", 0.1234f), "Accept a precise developer dialect level.");
        Check(Math.Abs(pawn.Progress("D") - 0.1234f) < 0.00001f && pawn.Progress("L") == 1f,
            "A nonzero dialect edit promotes its parent to mastery and retains precision.");
        Check(!pawn.Find("L").native && !pawn.Find("D").native, "Developer mastery does not create native identity.");
        pawn.DevSetDialectProgress("L", "Language", "D2", "Sibling", 0.8f);
        pawn.DevSetDialectProgress("B", "Other language", "BD", "Other dialect", 0.7f);
        pawn.DevSetDialectProgress("L", "Renamed language", "D", "Renamed dialect", 0.05f);
        Check(pawn.Progress("D") == 0.05f && pawn.Progress("D2") == 0.8f && pawn.entries.Count == 5,
            "Lowering one dialect leaves its sibling unchanged and never duplicates records.");
        Check(pawn.Find("D").lastKnownName == "Renamed dialect", "Developer edits retain load ID identity across renames.");
        pawn.Gain("L", "Language", false, 1f, true);
        pawn.Gain("D", "Dialect", true, 1f, true);
        pawn.DevSetLanguageProgress("L", "Language", new[] { "D", "D2" }, 0.4567f);
        Check(Math.Abs(pawn.Progress("L") - 0.4567f) < 0.00001f && pawn.Progress("D") == 0f && pawn.Progress("D2") == 0f,
            "Lowering a language clears all its known dialect levels.");
        Check(pawn.Find("L").native && pawn.Find("D").native, "Lowered native records keep their native markers.");
        Check(pawn.Progress("B") == 1f && pawn.Progress("BD") == 0.7f && other.entries.Count == 0,
            "Developer edits leave other languages and other pawns untouched.");
        pawn.DevSetLanguageProgress("L", "Language", new[] { "D", "D2" }, 1f);
        pawn.DevSetDialectProgress("L", "Language", "D", "Dialect", 1f);
        Check(!pawn.NeedsLesson("L", "D"), "Developer mastery can prepare the completed-lesson test case.");
        pawn.DevSetDialectProgress("L", "Language", "D", "Dialect", 0.99f);
        Check(pawn.NeedsLesson("L", "D") && pawn.Progress("L") == 1f, "Lowering only the dialect makes learning useful again.");
        pawn.DevSetLanguageProgress("L", "Language", new[] { "D", "D2" }, 5f);
        Check(pawn.Progress("L") == 1f && pawn.Progress("D") == 0.99f, "An excessive finite language level is clamped without resetting dialects.");
        var empty = new RimLanguages.IndividualLearning.PawnLanguages();
        Check(!empty.DevSetDialectProgress("L", "L", "D", "D", float.NaN) &&
            !empty.DevSetLanguageProgress("L", "L", new[] { "D" }, float.PositiveInfinity) && empty.entries.Count == 0,
            "Invalid developer levels cannot partially create or change records.");
        Check(!empty.DevSetDialectProgress("L", "L", "L", "D", 0.5f) &&
            !empty.DevSetLanguageProgress("", "L", new[] { "D" }, 0.5f) && empty.entries.Count == 0,
            "Reject invalid developer record identities before any change.");
        empty.DevSetDialectProgress("L", "Language", "D", "Dialect", -1f);
        Check(empty.Progress("D") == 0f && empty.Find("L") == null, "A zero-clamped dialect does not promote or create its parent.");
    }

    private static void LearningScenarios()
    {
        var reader = new RimLanguages.IndividualLearning.PawnLanguages();
        var bystander = new RimLanguages.IndividualLearning.PawnLanguages();
        Action<float> read = xp => reader.Learn("L", "Language", 100f, "D", "Dialect", 50f, xp);
        read(10f);
        Check(Math.Abs(reader.Progress("L") - 0.1f) < 0.00001, "Normalize raw book XP by language cost.");
        Check(reader.Find("D") == null, "Do not learn a dialect before its parent language.");
        Check(bystander.entries.Count == 0, "Reading must not teach a bystander.");
        read(500f);
        Check(reader.Progress("L") == 1f && reader.Find("D") == null, "A mastery-crossing event cannot count again as dialect XP.");
        read(10f);
        Check(Math.Abs(reader.Progress("D") - 0.2f) < 0.00001, "Use dialect cost after language mastery.");
        Check(reader.NeedsLesson("L", "D"), "A partly learned dialect still makes the lesson useful.");
        read(500f);
        Check(!reader.NeedsLesson("L", "D"), "Stop individual learning once both stages are mastered.");
        read(10f);
        Check(reader.entries.Count == 2 && reader.Progress("D") == 1f, "Repeated completed lessons neither duplicate entries nor exceed mastery.");

        var invalid = new RimLanguages.IndividualLearning.PawnLanguages();
        invalid.Learn("L", "L", 0f, "D", "D", 50f, 10f);
        invalid.Learn("L", "L", float.NaN, "D", "D", 50f, 10f);
        invalid.Learn("L", "L", 100f, "D", "D", 50f, float.PositiveInfinity);
        invalid.Learn("L", "L", 100f, "D", "D", 50f, -10f);
        Check(invalid.entries.Count == 0, "Reject invalid lesson costs and XP without creating records.");

        var smallTicks = new RimLanguages.IndividualLearning.PawnLanguages();
        var intervalTick = new RimLanguages.IndividualLearning.PawnLanguages();
        for (int i = 0; i < 100; i++) smallTicks.Learn("L", "L", 100f, "D", "D", 50f, 0.01f);
        intervalTick.Learn("L", "L", 100f, "D", "D", 50f, 1f);
        Check(Math.Abs(smallTicks.Progress("L") - intervalTick.Progress("L")) < 0.00001, "A reading interval containing multiple ticks is credited only once.");
        Check(Math.Abs(smallTicks.Progress("L") - 0.01f) < 0.00001, "Short reading intervals receive only their actual elapsed XP.");

        float standard = RimLanguages.IndividualLearning.ProgressMath.ConversationXp(100f, 1f, 1f, 1f, 1f, 1f, 1f);
        Check(Math.Abs(standard - 3f) < 0.00001, "Conversation keeps Cybranian's three-percent base cost.");
        Check(Math.Abs(RimLanguages.IndividualLearning.ProgressMath.ConversationXp(100f, 2f, 1f, 1f, 1f, 1f, 1f) - 6f) < 0.00001,
            "Learning ability affects personal conversation XP.");
        Check(Math.Abs(RimLanguages.IndividualLearning.ProgressMath.ConversationXp(100f, 1f, 1f, 2f, 2f, 2f, 1f) - 0.375f) < 0.00001,
            "Setting, language and dialect difficulty all reduce conversation XP.");
        Check(RimLanguages.IndividualLearning.ProgressMath.ConversationXp(100f, 0f, 1f, 1f, 1f, 1f, 1f) == 0f,
            "No learning ability means no conversation XP.");
        Check(RimLanguages.IndividualLearning.ProgressMath.ConversationXp(100f, 1f, 1f, 0f, 1f, 1f, 1f) == 0f,
            "Invalid difficulty cannot grant infinite mastery.");
        Check(RimLanguages.IndividualLearning.ProgressMath.ConversationXp(100f, 1f, 1f, 1f, 1f, 1f, float.NaN) == 0f,
            "Reject invalid conversation variation.");
        var alice = new RimLanguages.IndividualLearning.PawnLanguages();
        var bob = new RimLanguages.IndividualLearning.PawnLanguages();
        alice.Gain("A", "Alice native", false, 1f, true);
        bob.Gain("B", "Bob native", false, 1f, true);
        alice.Learn("B", "Bob native", 100f, "BD", "Bob dialect", 50f, standard);
        bob.Learn("A", "Alice native", 100f, "AD", "Alice dialect", 50f, standard);
        Check(Math.Abs(alice.Progress("B") - 0.03f) < 0.00001 && Math.Abs(bob.Progress("A") - 0.03f) < 0.00001,
            "Two participants can gain independent knowledge in opposite directions.");
        Check(alice.Progress("A") == 1f && bob.Progress("B") == 1f, "Conversation preserves native mastery.");
        Check(bystander.entries.Count == 0, "No broadcast of conversation knowledge to an uninvolved pawn.");

        // Saved regression case: a Social-disabled native speaker has the base
        // negotiation factor 0.4; the eligible learner still receives exposure.
        var warden = new RimLanguages.IndividualLearning.PawnLanguages();
        warden.Gain("L", "Language", false, 1f);
        float nativeSpeakerXp = RimLanguages.IndividualLearning.ProgressMath.ConversationXp(500f, 0.845f, 0.4f, 1f, 1f, 1.25f, 1f);
        warden.Learn("L", "Language", 1000f, "D", "Dialect", 625f, nativeSpeakerXp);
        Check(Math.Abs(warden.Progress("D") - 0.0064896f) < 0.00001f,
            "The base negotiation factor still produces dialect progress for an eligible warden.");
        Check(warden.Progress("L") == 1f && bystander.entries.Count == 0,
            "A prisoner dialect lesson preserves parent mastery and does not teach bystanders.");
    }
}
