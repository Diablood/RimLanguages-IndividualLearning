using System;
using System.Collections.Generic;
using System.Linq;
using FactionLanguages;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RimLanguages.IndividualLearning
{
    public sealed class IndividualLearningMod : Mod
    {
        public static TransmissionSettings Settings { get; private set; }
        public IndividualLearningMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<TransmissionSettings>();
        }
        public override string SettingsCategory() => "Rim Languages — Individual Learning";
        public override void DoSettingsWindowContents(UnityEngine.Rect inRect) => Settings.Draw(inRect);
    }

    [StaticConstructorOnStartup]
    public static class Bootstrap
    {
        static Bootstrap()
        {
            // Harmony can trigger static constructors of patched game types.
            // PregnancyUtility needs resolved DefOf references before its constructor runs.
            new Harmony("diablood.rimlanguages.individuallearning").PatchAll(typeof(Bootstrap).Assembly);
            // Resolved race Defs cover inherited humanlike races as well as vanilla humans.
            foreach (var def in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                if (def.race == null || !def.race.Humanlike) continue;
                if (def.comps == null) def.comps = new List<CompProperties>();
                if (!def.comps.Any(p => p.compClass == typeof(CompIndividualLanguages)))
                    def.comps.Add(new CompProperties { compClass = typeof(CompIndividualLanguages) });
                if (def.inspectorTabsResolved == null) def.inspectorTabsResolved = new List<InspectTabBase>();
                if (!def.inspectorTabsResolved.Any(t => t is ITab_IndividualLanguages))
                    def.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(typeof(ITab_IndividualLanguages)));
            }
            Log.Message("[Rim Languages — Individual Learning] " + typeof(Bootstrap).Assembly.GetName().Version +
                " loaded: individual learning and history, prisoner and negotiator checks, and shared social language factors. Collective compatibility remains active.");
        }
    }

    public sealed class CompIndividualLanguages : ThingComp
    {
        private PawnLanguages data = new PawnLanguages();
        public PawnLanguages Data => data;
        public Pawn Pawn => parent as Pawn;
        public static RaceLanguageDatabase Database => Current.Game?.GetComponent<GameComponent_FactionLanguages>()?.LanguageDatabase;

        public bool EnsureInitialized()
        {
            if (data.initialized) return true;
            if (NewbornLanguages.Generating) return false;
            // Never initialize while Scribe is resolving references or from a preview game.
            if (Scribe.mode != LoadSaveMode.Inactive || Current.ProgramState != ProgramState.Playing) return false;
            var pawn = Pawn;
            if (pawn == null) return false;
            if (!data.originCaptured && pawn.Faction != null)
            {
                data.originFaction = pawn.Faction;
                data.originCaptured = true;
            }
            var db = Database;
            Dialect dialect;
            if (db == null || data.originFaction == null || !db.TryGetLanguage(data.originFaction, out dialect) || dialect?.Parent == null)
                return false;
            return data.InitializeNative(dialect.Parent.GetUniqueLoadID(), dialect.Parent.Name,
                dialect.GetUniqueLoadID(), dialect.Name);
        }

        public override void PostSpawnSetup(bool respawningAfterLoad) => EnsureInitialized();

        public override void PostExposeData()
        {
            // No attempt to read faction XP when this node is absent in an older save.
            Scribe_Deep.Look(ref data, "RLI_individualLanguages");
            if (data == null) data = new PawnLanguages();
        }

    }

    public sealed class GameComponent_IndividualLanguages : GameComponent
    {
        public GameComponent_IndividualLanguages(Game game) { }
        public override void LoadedGame() => InitializePawns();
        public override void StartedNewGame() => InitializePawns();
        private static void InitializePawns()
        {
            int initialized = 0, pending = 0;
            // The narrower maps/world accessor excludes pawns held by caravans and transporters.
            // Snapshot because PawnsFinder reuses its result lists during other lookups.
            foreach (var pawn in PawnsFinder.All_AliveOrDead.Distinct().ToList())
            {
                var comp = pawn.GetComp<CompIndividualLanguages>();
                if (comp == null || comp.Data.initialized) continue;
                if (comp.EnsureInitialized()) initialized++;
                else pending++;
            }
            if (initialized > 0 || pending > 0)
                Log.Message("[RLI initialization] Native language and dialect initialized for " + initialized +
                    " pawns; unresolved: " + pending + ". No collective knowledge imported.");
        }
    }

    [HarmonyPatch(typeof(Pawn), nameof(Pawn.SetFaction), new Type[] { typeof(Faction), typeof(Pawn) })]
    public static class PreserveOriginPatch
    {
        public static void Prefix(Pawn __instance)
        {
            var comp = __instance.GetComp<CompIndividualLanguages>();
            if (comp == null) return;
            // Capture the previous faction even if the language database is temporarily unavailable.
            if (!comp.Data.initialized && !comp.Data.originCaptured && __instance.Faction != null)
            {
                comp.Data.originFaction = __instance.Faction;
                comp.Data.originCaptured = true;
            }
            comp.EnsureInitialized();
        }
    }

    [HarmonyPatch(typeof(PawnGenerator), nameof(PawnGenerator.GeneratePawn), new Type[] { typeof(PawnGenerationRequest) })]
    public static class GeneratedPawnPatch
    {
        public static void Postfix(Pawn __result) => __result?.GetComp<CompIndividualLanguages>()?.EnsureInitialized();
    }
}
