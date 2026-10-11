extern alias Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using FactionLanguages.AI.WorkGivers;
using FactionLanguages.Patches;
using FactionLanguages.Utilities;
using HarmonyLib;
using RimLanguages.IndividualLearning;

internal static class PrisonerPatchTests
{
    public static void Run(Action<bool, string> check)
    {
        Verify(check, typeof(Pawn_GuestTracker_SetExclusiveInteraction_FactionLanguagesPatch), "Postfix", true);
        Verify(check, typeof(WorkGiver_Warden_LearnLanguage), "JobOnThing", false);
        VerifySocial(check, false);
        VerifySocial(check, true);
        var actor = new PawnLanguages();
        var other = new PawnLanguages();
        actor.Gain("L", "Language", false, 0.9999f);
        check(!PrisonerSocial.Understands(actor, "L"), "Partial mastery cannot recruit or convert.");
        actor.Gain("L", "Language", false, 1f);
        check(PrisonerSocial.Understands(actor, "L") && actor.Progress("D") == 0f,
            "Full language mastery is sufficient without dialect mastery.");
        check(!PrisonerSocial.Understands(other, "L") && !PrisonerSocial.Understands(actor, "Other"),
            "A qualified warden does not qualify another pawn or another language.");
        check(!PrisonerSocial.Understands(null, "L") && !PrisonerSocial.Understands(actor, null),
            "Missing personal knowledge cannot pass the individual gate.");
        check(IndividualPrisonerSocialWorkPatch.TargetMethods().All(m => m != null) &&
            IndividualPrisonerSocialWorkPatch.TargetMethods().Count() == 2,
            "Both installed prisoner social work methods resolve.");
        var known = new PawnLanguages();
        known.Gain("L", "L", false, 1f);
        known.Gain("D", "D", true, 1f);
        var beginner = new PawnLanguages();
        check(!known.NeedsLesson("L", "D") && beginner.NeedsLesson("L", "D"),
            "A fluent warden cannot take the beginner's prisoner lesson.");
        known.Gain("D2", "D2", true, 0.5f);
        check(known.NeedsLesson("L", "D2"), "Another dialect remains useful to an otherwise fluent warden.");
    }

    internal static List<CodeInstruction> ReadInstructions(Type type, string name)
    {
        var target = AccessTools.Method(type, name);
        using (var assembly = Cecil::Mono.Cecil.AssemblyDefinition.ReadAssembly(type.Assembly.Location))
        {
            var method = assembly.MainModule.Types.Single(t => t.FullName == type.FullName).Methods.Single(m => m.Name == name);
            var opcodes = typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof(OpCode)).Select(f => (OpCode)f.GetValue(null)).ToDictionary(o => o.Name);
            return method.Body.Instructions.Select(i => new CodeInstruction(opcodes[i.OpCode.Name],
                i.Operand is Cecil::Mono.Cecil.MethodReference ?
                target.Module.ResolveMethod(((Cecil::Mono.Cecil.MethodReference)i.Operand).MetadataToken.ToInt32()) :
                i.Operand)).ToList();
        }
    }

    private static void VerifySocial(Action<bool, string> check, bool menu)
    {
        var type = menu ? typeof(Pawn_GuestTracker_SetExclusiveInteraction_FactionLanguagesPatch) :
            typeof(Pawn_GuestTracker_Recruitable_FactionLanguagesPatch);
        var original = ReadInstructions(type, "Postfix");
        var global = AccessTools.Method(typeof(LanguageUtility), nameof(LanguageUtility.FactionLanguageIsLearned));
        var helper = AccessTools.Method(typeof(PrisonerSocial), menu ? nameof(PrisonerSocial.MenuModeAllowed) :
            nameof(PrisonerSocial.MenuRecruitable));
        int at = original.FindIndex(i => i.Calls(global));
        original[at].labels.Add(new DynamicMethod("SocialIL", typeof(void), Type.EmptyTypes).GetILGenerator().DefineLabel());
        original[at].blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginExceptionBlock));
        var changed = PrisonerSocial.ReplaceGlobalCheck(original, menu).ToList();
        int added = menu ? 3 : 1;
        check(changed.Count == original.Count + added && changed.Count(i => i.Calls(helper)) == 1 &&
            changed.All(i => !i.Calls(global)), "Replace exactly one installed social gate: " + type.Name);
        check(changed[at].opcode == (menu ? OpCodes.Ldarg_2 : OpCodes.Ldarg_0) &&
            (!menu || (changed[at + 1].opcode == OpCodes.Ldind_Ref && changed[at + 2].opcode == OpCodes.Ldarg_1)),
            "Pass the verified prisoner and mode argument signature: " + type.Name);
        check(original.Take(at).SequenceEqual(changed.Take(at)) &&
            original.Skip(at + 1).SequenceEqual(changed.Skip(at + added + 1)),
            "Preserve all other conditions including existing Recruitable rejection: " + type.Name);
        check(original[at].labels.SequenceEqual(changed[at].labels) &&
            original[at].blocks.SequenceEqual(changed[at].blocks), "Preserve social branch and exception metadata.");
        foreach (var invalid in new[] { new List<CodeInstruction>(), new List<CodeInstruction> {
            new CodeInstruction(OpCodes.Call, global), new CodeInstruction(OpCodes.Call, global) } })
        {
            bool refused = false;
            try { PrisonerSocial.ReplaceGlobalCheck(invalid, menu).ToList(); }
            catch (InvalidOperationException) { refused = true; }
            check(refused, "Reject missing or ambiguous social gates.");
        }
        if (menu)
        {
            var learningHelper = AccessTools.Method(typeof(PrisonerLearning), nameof(PrisonerLearning.MenuLanguageComplete));
            var firstLearning = PrisonerSocial.ReplaceGlobalCheck(IndividualPrisonerMenuPatch.Transpiler(original), true).ToList();
            var firstSocial = IndividualPrisonerMenuPatch.Transpiler(changed).ToList();
            check(firstLearning.Count(i => i.Calls(helper)) == 1 && firstLearning.Count(i => i.Calls(learningHelper)) == 1 &&
                firstLearning.Select(i => i.opcode).SequenceEqual(firstSocial.Select(i => i.opcode)) &&
                firstLearning.Select(i => i.operand).SequenceEqual(firstSocial.Select(i => i.operand)),
                "Learning and recruitment transpilers compose in either order.");
        }
    }

    private static void Verify(Action<bool, string> check, Type type, string name, bool menu)
    {
        MethodInfo target = AccessTools.Method(type, name);
        // Read the installed IL without invoking MonoMod's Mono-specific emitter.
        var generator = new DynamicMethod("InspectPrisonerIL", typeof(void), Type.EmptyTypes).GetILGenerator();
        List<CodeInstruction> original;
        using (var assembly = Cecil::Mono.Cecil.AssemblyDefinition.ReadAssembly(type.Assembly.Location))
        {
            var method = assembly.MainModule.Types.Single(t => t.FullName == type.FullName).Methods.Single(m => m.Name == name);
            var opcodes = typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof(OpCode)).Select(f => (OpCode)f.GetValue(null)).ToDictionary(o => o.Name);
            original = method.Body.Instructions.Select(i => new CodeInstruction(opcodes[i.OpCode.Name],
                i.Operand is Cecil::Mono.Cecil.MethodReference ?
                target.Module.ResolveMethod(((Cecil::Mono.Cecil.MethodReference)i.Operand).MetadataToken.ToInt32()) :
                i.Operand)).ToList();
        }
        var global = AccessTools.Method(typeof(LanguageUtility), nameof(LanguageUtility.FactionDialectIsLearned));
        int at = original.FindIndex(i => i.Calls(global));
        if (at >= 0) original[at].labels.Add(generator.DefineLabel());
        var changed = (menu ? IndividualPrisonerMenuPatch.Transpiler(original) :
            IndividualPrisonerWorkPatch.Transpiler(original)).ToList();
        var helper = AccessTools.Method(typeof(PrisonerLearning), menu ?
            nameof(PrisonerLearning.MenuLanguageComplete) : nameof(PrisonerLearning.WardenLanguageComplete));
        check(at >= 0 && changed.Count == original.Count + 2 && changed.Count(i => i.Calls(helper)) == 1 &&
            changed.All(i => !i.Calls(global)), "Replace exactly the installed language check: " + name);
        check(changed[at].opcode == (menu ? OpCodes.Ldarg_2 : OpCodes.Ldarg_1) &&
            changed[at + 1].opcode == (menu ? OpCodes.Ldind_Ref : OpCodes.Ldarg_2),
            "Pass the actual prisoner/warden with the verified stack signature: " + name);
        check(original.Take(at).SequenceEqual(changed.Take(at)) &&
            original.Skip(at + 1).SequenceEqual(changed.Skip(at + 3)),
            "Preserve every other instruction, including rejection and physical checks: " + name);
        check(original[at].labels.SequenceEqual(changed[at].labels) &&
            original[at].blocks.SequenceEqual(changed[at].blocks), "Preserve branch labels: " + name);
        bool refused = false;
        try { PrisonerLearning.ReplaceCheck(new List<CodeInstruction>(), helper, menu).ToList(); }
        catch (InvalidOperationException) { refused = true; }
        check(refused, "Reject an unsupported base-mod method: " + name);
        refused = false;
        try { PrisonerLearning.ReplaceCheck(new[] { new CodeInstruction(OpCodes.Call, global),
            new CodeInstruction(OpCodes.Call, global) }, helper, menu).ToList(); }
        catch (InvalidOperationException) { refused = true; }
        check(refused, "Reject ambiguous multiple checks: " + name);
    }
}
