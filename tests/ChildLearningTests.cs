extern alias Cecil;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimLanguages.IndividualLearning;
using RimWorld;
using Verse;
using Verse.AI;

internal static class ChildLearningTests
{
    public static void Run(Action<bool, string> check)
    {
        var baby = new PawnLanguages();
        check(baby.InitializeNewborn("MotherL", "Mother language", "MotherD", "Mother dialect"),
            "A newborn can record maternal identity without granting mastery.");
        check(baby.initialized && baby.entries.Count == 2 && baby.entries.All(e => e.native && e.progress == 0f),
            "Both newborn native markers are visible at zero progress.");
        baby.Gain("MotherL", "L", false, 0.2f);
        check(!baby.InitializeNewborn("OtherL", "L", "OtherD", "D") && baby.Progress("MotherL") == 0.2f,
            "Repeated birth initialization cannot reset learned progress or native identity.");
        var existing = new PawnLanguages();
        existing.InitializeNative("L", "L", "D", "D");
        check(!existing.InitializeNewborn("L", "L", "D", "D") && existing.Progress("D") == 1f,
            "Existing babies and children retain their previous mastery.");
        var pending = new PawnLanguages();
        check(!pending.InitializeNewborn(null, "L", "D", "D") &&
            !pending.InitializeNewborn("L", "L", "L", "D") && pending.entries.Count == 0,
            "Invalid newborn identity never creates partial entries.");
        pending.Gain("Foreign", "Foreign", false, 0.3f);
        check(!pending.InitializeNewborn("L", "L", "D", "D") && pending.Progress("Foreign") == 0.3f,
            "Birth initialization never rewrites a registry populated by another path.");
        check(Math.Abs(BabyCareLearning.Exposure(0.2f, 0.7f) - 0.5f) < 0.0001f &&
            BabyCareLearning.Exposure(0.7f, 0.2f) == 0f,
            "Care exposure follows positive actual feeding, not merely a planned job.");
        check(BabyCareLearning.Exposure(float.NaN, 1f) == 0f &&
            BabyCareLearning.Exposure(0f, float.PositiveInfinity) == 0f,
            "Invalid feeding data cannot produce language gains.");
        var teacher = new PawnLanguages();
        teacher.InitializeNative("MotherL", "L", "MotherD", "D");
        var language = new TransmissionLesson { key = "MotherL", xpMax = 100f, baseCost = 100f, languageDifficulty = 1f };
        var dialect = new TransmissionLesson { key = "MotherD", parentKey = "MotherL", dialect = true, xpMax = 100f };
        check(TransmissionRules.Eligible(baby, teacher, language) && !TransmissionRules.Eligible(baby, teacher, dialect),
            "Feeding can teach the native language but never its dialect before parent mastery.");
        check(TransmissionRules.Xp(language, 0f, 10, false, false, true, 1f, 1f) > 0f,
            "The baby baseline permits learning without evaluating a disabled Social stat.");

        var birth = AccessTools.Method(typeof(PregnancyUtility), "ApplyBirthOutcome");
        check(birth.IsStatic && birth.GetParameters()[4].Name == "geneticMother" &&
            birth.GetParameters()[4].ParameterType == typeof(Pawn), "The birth argument is the installed genetic mother.");
        var code = PrisonerPatchTests.ReadInstructions(typeof(PregnancyUtility), "ApplyBirthOutcome");
        var original = AccessTools.Method(typeof(PawnGenerator), "GeneratePawn", new[] { typeof(PawnGenerationRequest) });
        int at = code.FindIndex(i => i.Calls(original));
        var changed = IndividualBirthPatch.Transpiler(code).ToList();
        check(at >= 0 && changed.Count == code.Count + 1 && changed[at].opcode == OpCodes.Ldarg_S &&
            (byte)changed[at].operand == 4 && changed[at + 1].Calls(AccessTools.Method(typeof(NewbornLanguages), "Generate")),
            "Only the newborn generator call receives maternal language context.");
        check(code.Take(at).SequenceEqual(changed.Take(at)) && code.Skip(at + 1).SequenceEqual(changed.Skip(at + 2)),
            "Every other birth instruction, outcome and consequence stays unchanged.");
        bool rejected = false;
        try { IndividualBirthPatch.Transpiler(new CodeInstruction[0]).ToList(); }
        catch (InvalidOperationException) { rejected = true; }
        check(rejected, "Unknown birth APIs fail explicitly instead of patching another call.");
        check(IndividualBabyFeedingPatch.TargetMethods().Count() == 2 &&
            IndividualBabyFeedingPatch.TargetMethods().All(m => m != null && ((MethodInfo)m).ReturnType == typeof(Verse.AI.Toil)),
            "Only the two installed feeding toil factories are patched.");
        check(AccessTools.Field(typeof(JobDriver_FeedBaby), "initialFoodPercentage")?.FieldType == typeof(float),
            "Feeding exposure uses the actual installed baseline field.");
        using (var assembly = Cecil::Mono.Cecil.AssemblyDefinition.ReadAssembly(typeof(Pawn).Assembly.Location))
        {
            var feed = assembly.MainModule.GetType(typeof(JobDriver_FeedBaby).FullName);
            check(feed.Methods.Single(m => m.Name == "ExposeData").Body.Instructions.Any(i =>
                i.OpCode == Cecil::Mono.Cecil.Cil.OpCodes.Ldstr && (string)i.Operand == "initialFoodPercentage"),
                "Vanilla serializes the feeding baseline for a job resumed after loading.");
        }
        using (var mod = Cecil::Mono.Cecil.AssemblyDefinition.ReadAssembly(typeof(PawnLanguages).Assembly.Location))
        {
            Func<Cecil::Mono.Cecil.MethodDefinition, int> patchCalls = method => method.Body.Instructions.Count(i =>
                i.Operand is Cecil::Mono.Cecil.MethodReference called && called.DeclaringType.FullName == "HarmonyLib.Harmony" &&
                called.Name == "PatchAll");
            check(patchCalls(mod.MainModule.GetType(typeof(IndividualLearningMod).FullName).Methods.Single(m => m.Name == ".ctor")) == 0,
                "The early Mod constructor never patches game types requiring resolved DefOfs.");
            var bootstrap = mod.MainModule.GetType(typeof(Bootstrap).FullName);
            check(bootstrap.CustomAttributes.Any(a => a.AttributeType.FullName == "Verse.StaticConstructorOnStartup") &&
                patchCalls(bootstrap.Methods.Single(m => m.Name == ".cctor")) == 1,
                "Harmony registration runs exactly once from the after-Defs startup bootstrap.");
        }
        var grant = PrisonerPatchTests.ReadInstructions(typeof(BabyCareLearning), "Learn");
        check(!grant.Any(i => i.operand is MethodInfo m && m.Name == "Learn" &&
            (m.DeclaringType == typeof(Pawn_SkillTracker) || m.DeclaringType == typeof(SkillRecord))),
            "Baby language credit neither replaces skill XP nor changes skill records.");
        check(IndividualBabyPlayPatch.Exposure(-1f, 0.8f, true) == 0f &&
            IndividualBabyPlayPatch.Exposure(0.2f, 0.8f, false) == 0f,
            "A planned or unprepared play session cannot grant language progress.");
        check(Math.Abs(IndividualBabyPlayPatch.Exposure(0.2f, 0.8f, true) - 0.6f) < 0.0001f &&
            IndividualBabyPlayPatch.Exposure(0.8f, 0.2f, true) == 0f,
            "Play credit is proportional to actual net play gained, including interrupted sessions.");
        var credit = new BabyPlayCredit();
        check(credit.TryClaim() && !credit.TryClaim() && new BabyPlayCredit().TryClaim(),
            "Repeated completion cannot double-credit a job or suppress another job.");
        check(AccessTools.Method(typeof(JobDriver_BabyPlay), "AddPlayThoughtIfAppropriate").ReturnType == typeof(void) &&
            AccessTools.Field(typeof(JobDriver_BabyPlay), "initialPlayPercentage").FieldType == typeof(float) &&
            AccessTools.Field(typeof(JobDriver_BabyPlay), "finishedSetup").FieldType == typeof(bool),
            "The play postfix matches the actual shared engine method and baseline fields.");
        check(typeof(JobDriver_BabyPlay).IsAssignableFrom(typeof(JobDriver_PlayStatic)) &&
            typeof(JobDriver_BabyPlay).IsAssignableFrom(typeof(JobDriver_PlayToys)) &&
            typeof(JobDriver_BabyPlay).IsAssignableFrom(typeof(JobDriver_PlayWalking)),
            "All three vanilla baby play drivers share the patched completion owner.");
        using (var assembly = Cecil::Mono.Cecil.AssemblyDefinition.ReadAssembly(typeof(Pawn).Assembly.Location))
        {
            var play = assembly.MainModule.GetType(typeof(JobDriver_BabyPlay).FullName);
            var expose = play.Methods.Single(m => m.Name == "ExposeData").Body.Instructions;
            check(new[] { "initialPlayPercentage", "finishedSetup" }.All(key => expose.Any(i =>
                i.OpCode == Cecil::Mono.Cecil.Cil.OpCodes.Ldstr && (string)i.Operand == key)),
                "The vanilla play baseline and setup status survive a save during play.");
            check(play.Methods.Where(m => m.HasBody).Any(m => m.Body.Instructions.Any(i =>
                i.Operand is Cecil::Mono.Cecil.MethodReference called && called.Name == "AddPlayThoughtIfAppropriate")),
                "The shared driver actually invokes the patched completion helper.");
        }
    }
}
