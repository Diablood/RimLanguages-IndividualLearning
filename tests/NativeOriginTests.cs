extern alias Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using FactionLanguages;
using RimLanguages.IndividualLearning;
using Verse;

internal static class NativeOriginTests
{
    // Build parent database objects without name generation or a running game.
    private static T Empty<T>() => (T)FormatterServices.GetUninitializedObject(typeof(T));
    private static void Set(object target, string field, object value) =>
        target.GetType().GetField(field, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(target, value);
    private static RaceLanguage Language(int id, string race, params int[] dialectIds)
    {
        var language = Empty<RaceLanguage>();
        Set(language, "id", id);
        Set(language, "name", "Language " + id);
        Set(language, "def", Empty<RaceLanguageDef>());
        var raceDef = Empty<ThingDef>();
        raceDef.defName = race;
        Set(language, "mainRace", raceDef);
        var dialects = new List<Dialect>();
        foreach (int dialectId in dialectIds)
        {
            var dialect = Empty<Dialect>();
            Set(dialect, "id", dialectId);
            Set(dialect, "name", "Dialect " + dialectId);
            Set(dialect, "def", Empty<DialectDef>());
            Set(dialect, "parent", language);
            dialects.Add(dialect);
        }
        Set(language, "dialects", dialects);
        return language;
    }
    public static void Run(Action<bool, string> check)
    {
        var a = Language(1, "Human", 1, 2);
        var b = Language(2, "Human", 3);
        var alien = Language(3, "Alien", 4);
        var noDialect = Language(4, "Human");
        var candidates = new[] { a, b, alien, noDialect, null };
        var data = new PawnLanguages { originCaptured = true };
        check(NativeOriginLanguages.TryInitialize(data, "CreepJoiner", candidates, 114506), "An unresolved existing creepjoiner receives native knowledge.");
        check(data.initialized && data.originCaptured && data.originFaction == null && data.entries.Count == 2 &&
            data.entries.All(e => e.native && e.progress == 1f), "Only one native language/dialect pair is granted without rewriting origin.");
        var selected = data.entries.Single(e => !e.isDialect).key == a.GetUniqueLoadID() ? a : b;
        check(data.Find(selected.GetUniqueLoadID()) != null && selected.Dialects.Any(d => data.Find(d.GetUniqueLoadID()) != null),
            "The native dialect belongs to the chosen existing human language.");
        data.DevSetLanguageProgress(selected.GetUniqueLoadID(), selected.Name, selected.Dialects.Select(d => d.GetUniqueLoadID()), 0.2f);
        check(!NativeOriginLanguages.TryInitialize(data, "CreepJoiner", candidates, 999) &&
            data.Progress(selected.GetUniqueLoadID()) == 0.2f, "Initialized progress is preserved even when faction, seed or candidate pool changes.");
        var repeat = new PawnLanguages();
        NativeOriginLanguages.TryInitialize(repeat, "CreepJoiner", Enumerable.Reverse(candidates), 114506);
        check(repeat.Find(selected.GetUniqueLoadID()) != null, "The same pawn gets the same language independent of enumeration order.");
        var ordinary = new PawnLanguages();
        check(NativeOriginLanguages.TryInitialize(ordinary, "Human", candidates, 1) && ordinary.entries.Count == 2,
            "An ordinary human without a usable faction language now receives a human native pair.");
        ordinary = new PawnLanguages();
        check(!NativeOriginLanguages.TryInitialize(ordinary, "CreepJoiner", new[] { alien, noDialect }, 1) && !ordinary.initialized,
            "No valid human language/dialect pair leaves initialization pending.");
        check(!NativeOriginLanguages.TryInitialize(ordinary, "CreepJoiner", null, 1), "An unavailable database grants no knowledge.");
        ordinary.Gain("Foreign", "Learned", false, 0.3f);
        check(NativeOriginLanguages.TryInitialize(ordinary, "CreepJoiner", new[] { a }, 1) && ordinary.Progress("Foreign") == 0.3f,
            "Repair preserves knowledge learned before initialization.");
        var partialNative = new PawnLanguages();
        partialNative.Gain("Native", "Existing native", false, 0.4f, true);
        check(!NativeOriginLanguages.TryInitialize(partialNative, "CreepJoiner", candidates, 1) && partialNative.entries.Count == 1,
            "An existing native identity is never replaced by the fallback.");
        var outcomes = new HashSet<string>();
        for (int id = 1; id <= 50; id++)
        {
            var individual = new PawnLanguages();
            NativeOriginLanguages.TryInitialize(individual, "CreepJoiner", candidates, id);
            outcomes.Add(individual.entries.Single(e => !e.isDialect).key);
        }
        check(outcomes.Count == 2, "Different mysterious arrivals can have different native languages.");
        check(a.Xp == 0f && a.Dialects.All(d => d.Xp == 0f), "The fallback never grants collective experience.");

        var modded = new PawnLanguages();
        check(NativeOriginLanguages.TryInitialize(modded, "Alien", candidates, 7) && modded.Find(alien.GetUniqueLoadID()) != null &&
            modded.Find(a.GetUniqueLoadID()) == null && modded.Find(b.GetUniqueLoadID()) == null,
            "A modded race selects its own existing language, never an arbitrary human language.");
        var unknown = new PawnLanguages();
        check(!NativeOriginLanguages.TryInitialize(unknown, "UnmappedRace", candidates, 7) &&
            !unknown.initialized && unknown.entries.Count == 0, "An unknown race stays pending without inventing or borrowing a language.");
        check(!NativeOriginLanguages.TryInitialize(unknown, null, candidates, 7) &&
            !NativeOriginLanguages.TryInitialize(unknown, "", candidates, 7), "A missing race cannot grant knowledge.");
        var newborn = new PawnLanguages();
        newborn.InitializeNewborn("MotherL", "Mother", "MotherD", "Dialect");
        check(!NativeOriginLanguages.TryInitialize(newborn, "Human", candidates, 8) &&
            newborn.entries.All(e => e.native && e.progress == 0f) && newborn.entries.Count == 2,
            "A newborn's recorded maternal identity stays at zero.");
        var fallbackBaby = new PawnLanguages();
        check(NativeOriginLanguages.TryInitialize(fallbackBaby, "Human", candidates, 8, newborn: true) &&
            fallbackBaby.initialized && fallbackBaby.entries.Count == 2 && fallbackBaby.entries.All(e => e.native && e.progress == 0f),
            "A supported birth with no maternal or faction language receives fallback identity at zero, never mastery.");
        var populatedBaby = new PawnLanguages();
        populatedBaby.Gain("Existing", "Existing", false, 0.2f);
        check(!NativeOriginLanguages.TryInitialize(populatedBaby, "Human", candidates, 8, newborn: true) &&
            populatedBaby.entries.Count == 1 && populatedBaby.Progress("Existing") == 0.2f,
            "The newborn fallback does not replace a populated registry.");
        var delayed = new PawnLanguages();
        check(!NativeOriginLanguages.TryInitialize(delayed, "Alien", new[] { a }, 4) &&
            NativeOriginLanguages.TryInitialize(delayed, "Alien", candidates, 4),
            "A pending race can initialize when its compatible language becomes available later.");
        // Exercise nested generation and failure cleanup without generating a game pawn.
        int outer, inner;
        GeneratedPawnPatch.Prefix(out outer);
        GeneratedPawnPatch.Prefix(out inner);
        check(GeneratedPawnPatch.Generating, "Nested pawn creation remains marked as generating.");
        GeneratedPawnPatch.Postfix(null, inner);
        GeneratedPawnPatch.Finalizer(inner);
        check(GeneratedPawnPatch.Generating, "Completing an inner generation cannot enable the fallback for its outer pawn.");
        GeneratedPawnPatch.Finalizer(outer);
        check(!GeneratedPawnPatch.Generating, "A failed outer generation clears its transient guard.");
        GeneratedPawnPatch.Prefix(out outer);
        GeneratedPawnPatch.Postfix(null, outer);
        GeneratedPawnPatch.Finalizer(outer);
        check(!GeneratedPawnPatch.Generating, "Successful generation also restores the guard.");

        using (var module = Cecil::Mono.Cecil.ModuleDefinition.ReadModule(typeof(PawnLanguages).Assembly.Location))
        {
            var method = module.GetType(typeof(CompIndividualLanguages).FullName).Methods.Single(m => m.Name == "EnsureInitialized");
            var calls = method.Body.Instructions.Where(i => i.Operand is Cecil::Mono.Cecil.MethodReference)
                .Select(i => ((Cecil::Mono.Cecil.MethodReference)i.Operand).FullName).ToList();
            int factionLookup = calls.FindIndex(n => n.Contains("::TryGetLanguage("));
            int fallback = calls.FindIndex(n => n.Contains("NativeOriginLanguages::TryInitialize("));
            int generating = calls.FindIndex(n => n.Contains("GeneratedPawnPatch::get_Generating("));
            check(factionLookup >= 0 && generating > factionLookup && fallback > generating,
                "The compiled initializer checks origin faction and generation guard before entering the fallback.");
            check(calls.Any(n => n.Contains("NewbornLanguages::get_Generating(")) &&
                calls.Any(n => n.Contains("RaceProperties::get_Humanlike(")),
                "The compiled initializer retains birth-context and humanlike guards.");
        }
    }
}
