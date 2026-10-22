using System;
using System.Collections.Generic;
using System.Linq;
using FactionLanguages;

namespace RimLanguages.IndividualLearning
{
    public static class NativeOriginLanguages
    {
        // Called only when the captured faction has no usable language.
        // Never attach a language to a shared faction or replace an existing identity.
        public static bool TryInitialize(PawnLanguages data, string raceDefName,
            IEnumerable<RaceLanguage> languages, int pawnId, bool newborn = false)
        {
            if (string.IsNullOrEmpty(raceDefName) || data == null || data.initialized ||
                data.entries.Any(e => e.native) || languages == null) return false;
            // Anomaly defines a separate human-derived race; other races require an exact match.
            string languageRace = raceDefName == "CreepJoiner" ? "Human" : raceDefName;
            var candidates = languages.Where(l => l?.Def != null && l.MainRace?.defName == languageRace &&
                l.Dialects != null && l.Dialects.Any(d => d?.Def != null && d.Parent == l))
                .OrderBy(l => l.GetUniqueLoadID(), StringComparer.Ordinal).ToList();
            if (candidates.Count == 0) return false;
            // Local RNG: independent of game randomness and of dictionary/enumeration order.
            // Persisted native entries, rather than a seed, are the authoritative result.
            var random = new Random(pawnId);
            var language = candidates[random.Next(candidates.Count)];
            var dialects = language.Dialects.Where(d => d?.Def != null && d.Parent == language)
                .OrderBy(d => d.GetUniqueLoadID(), StringComparer.Ordinal).ToList();
            var dialect = dialects[random.Next(dialects.Count)];
            if (newborn)
                return data.InitializeNewborn(language.GetUniqueLoadID(), language.Name,
                    dialect.GetUniqueLoadID(), dialect.Name);
            return data.InitializeNative(language.GetUniqueLoadID(), language.Name,
                dialect.GetUniqueLoadID(), dialect.Name);
        }
    }
}
