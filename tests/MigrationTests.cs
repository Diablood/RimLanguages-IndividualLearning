extern alias Cecil;
using System;
using System.Linq;
using RimLanguages.IndividualLearning;
using RimWorld;
using Verse;

internal static class MigrationTests
{
    public static void Run(Action<bool, string> check)
    {
        var oldSavePawn = new PawnLanguages();
        check(oldSavePawn.InitializeNative("L", "Native", "D", "Dialect"), "An absent personal registry can initialize.");
        check(oldSavePawn.initialized && oldSavePawn.entries.Count == 2 &&
            oldSavePawn.Progress("L") == 1f && oldSavePawn.Progress("D") == 1f &&
            oldSavePawn.entries.All(e => e.native), "Migration grants only the native language and dialect.");
        check(!oldSavePawn.Find("L").isDialect && oldSavePawn.Find("D").isDialect,
            "Native language and dialect keep their distinct types.");
        oldSavePawn.DevSetLanguageProgress("L", "Native", new[] { "D" }, 0.25f);
        oldSavePawn.Gain("Foreign", "Learned", false, 0.4f);
        var entries = oldSavePawn.entries.ToArray();
        check(oldSavePawn.InitializeNative("OtherL", "New faction", "OtherD", "New dialect") &&
            oldSavePawn.Progress("L") == 0.25f && oldSavePawn.Progress("D") == 0f &&
            oldSavePawn.Progress("Foreign") == 0.4f && entries.SequenceEqual(oldSavePawn.entries),
            "Repeated initialization preserves edited and learned records, even with another faction.");
        var pending = new PawnLanguages();
        check(!pending.InitializeNative(null, "Unknown", "D", "D") &&
            !pending.InitializeNative("L", "L", "", "Unknown") &&
            !pending.InitializeNative("L", "L", "L", "Invalid") &&
            !pending.initialized && pending.entries.Count == 0, "Missing or invalid association defers migration without partial data.");
        pending.Gain("Foreign", "Existing", false, 0.6f);
        check(pending.InitializeNative("L", "L", "D", "D") && pending.Progress("Foreign") == 0.6f &&
            pending.entries.Count == 3, "A later resolved association preserves any preexisting personal progress.");
        check(new PawnLanguages().entries.Count == 0, "Migrating one pawn never initializes another pawn's registry.");

        // These assertions inspect the installed engine; they are not an executed game load.
        using (var engine = Cecil::Mono.Cecil.AssemblyDefinition.ReadAssembly(typeof(Game).Assembly.Location))
        {
            Func<Type, string, Cecil::Mono.Cecil.MethodDefinition> method = (type, name) =>
                engine.MainModule.Types.Single(t => t.FullName == type.FullName).Methods.Single(m => m.Name == name);
            var expose = method(typeof(ThingWithComps), "ExposeData").Body.Instructions;
            int create = CallIndex(expose, "Verse.ThingWithComps", "InitializeComps");
            int read = CallIndex(expose, "Verse.ThingComp", "PostExposeData");
            check(create >= 0 && read > create, "Installed engine recreates current Def comps before reading their save data.");
            var load = method(typeof(Game), "LoadGame").Body.Instructions;
            int scribe = CallIndex(load, "Verse.ScribeLoader", "FinalizeLoading");
            int finalize = CallIndex(load, "Verse.Game", "FinalizeInit");
            int loaded = CallIndex(load, "Verse.GameComponentUtility", "LoadedGame");
            check(scribe >= 0 && finalize > scribe && loaded > finalize,
                "Installed engine finishes Scribe and finalizes the language database before LoadedGame.");
            var init = method(typeof(Game), "FinalizeInit").Body.Instructions;
            int state = CallIndex(init, "Verse.Current", "set_ProgramState");
            check(state > CallIndex(init, "Verse.GameComponentUtility", "FinalizeInit") &&
                (int)ProgramState.Playing == 2 && init[state - 1].OpCode.Code == Cecil::Mono.Cecil.Cil.Code.Ldc_I4_2,
                "LoadedGame runs after ProgramState becomes Playing, including paused saves.");
            var finder = method(typeof(PawnsFinder), "get_All_AliveOrDead").Body.Instructions;
            check(CallIndex(finder, "RimWorld.PawnsFinder", "get_AllMapsWorldAndTemporary_AliveOrDead") >= 0 &&
                CallIndex(finder, "RimWorld.PawnsFinder", "get_AllCaravansAndTravellingTransporters_AliveOrDead") >= 0,
                "The installed full pawn accessor includes caravans and travelling transporters.");
            var recruit = method(typeof(RecruitUtility), "Recruit").Body.Instructions;
            check(CallIndex(recruit, "Verse.Thing", "SetFaction") >= 0 &&
                CallIndex(recruit, "Verse.ThingWithComps", "InitializeComps") < 0,
                "Installed recruitment changes faction without recreating the pawn's comps.");
            var caravan = method(typeof(RimWorld.Planet.Caravan), "ExposeData").Body.Instructions;
            check(CallIndex(caravan, "Verse.Scribe_Deep", "Look") >= 0 && caravan.Any(i =>
                i.OpCode.Code == Cecil::Mono.Cecil.Cil.Code.Ldstr && (string)i.Operand == "pawns"),
                "The installed caravan saves its pawn owner through deep serialization.");
            var transport = method(typeof(RimWorld.Planet.TravellingTransporters), "ExposeData").Body.Instructions;
            check(CallIndex(transport, "Verse.Scribe_Collections", "Look") >= 0 && transport.Any(i =>
                i.OpCode.Code == Cecil::Mono.Cecil.Cil.Code.Ldstr && (string)i.Operand == "pods"),
                "The installed travelling transport saves its transporter contents.");
        }
    }

    private static int CallIndex(System.Collections.Generic.IList<Cecil::Mono.Cecil.Cil.Instruction> instructions,
        string type, string name)
    {
        for (int i = 0; i < instructions.Count; i++)
        {
            var called = instructions[i].Operand as Cecil::Mono.Cecil.MethodReference;
            if (called != null && called.DeclaringType.FullName == type && called.Name == name) return i;
        }
        return -1;
    }
}
