using System;
using System.Collections.Generic;
using Verse;

namespace RimLanguages.IndividualLearning
{
    public static class ProgressMath
    {
        public static float Clamp(float value)
        {
            if (float.IsNaN(value) || float.IsNegativeInfinity(value)) return 0f;
            return Math.Max(0f, Math.Min(1f, value));
        }

        public static float Add(float current, float amount)
        {
            current = Clamp(current);
            if (float.IsNaN(amount) || float.IsInfinity(amount) || amount <= 0f) return current;
            return Clamp(current + amount);
        }

        public static bool PositiveFinite(float value) => value > 0f && !float.IsInfinity(value) && !float.IsNaN(value);

        public static float ConversationXp(float baseCost, float ability, float negotiation,
            float settingDifficulty, float languageDifficulty, float dialectDifficulty, float variation)
        {
            if (!PositiveFinite(baseCost) || !PositiveFinite(ability) || !PositiveFinite(negotiation) ||
                !PositiveFinite(settingDifficulty) || !PositiveFinite(languageDifficulty) ||
                !PositiveFinite(dialectDifficulty) || !PositiveFinite(variation)) return 0f;
            float xp = baseCost * 0.03f * ability * negotiation * variation /
                settingDifficulty / languageDifficulty / dialectDifficulty;
            return PositiveFinite(xp) ? xp : 0f;
        }
    }

    // Identity comes from Cybranian's persistent load IDs, never from translated names.
    public sealed class LanguageKnowledge : IExposable
    {
        public string key;
        public string lastKnownName;
        public bool isDialect;
        public bool native;
        public float progress;

        public void ExposeData()
        {
            Scribe_Values.Look(ref key, "key");
            Scribe_Values.Look(ref lastKnownName, "lastKnownName");
            Scribe_Values.Look(ref isDialect, "isDialect");
            Scribe_Values.Look(ref native, "native");
            Scribe_Values.Look(ref progress, "progress");
            if (Scribe.mode == LoadSaveMode.PostLoadInit) progress = ProgressMath.Clamp(progress);
        }
    }

    public sealed class PawnLanguages : IExposable
    {
        public int schema = 1;
        public bool initialized;
        public bool originCaptured;
        public RimWorld.Faction originFaction;
        public List<LanguageKnowledge> entries = new List<LanguageKnowledge>();

        // Used equally for a newly generated pawn and an older save without this comp's node.
        // Existing personal records are never reattributed on loading or recruitment.
        public bool InitializeNative(string languageKey, string languageName, string dialectKey, string dialectName)
        {
            if (initialized) return true;
            if (string.IsNullOrEmpty(languageKey) || string.IsNullOrEmpty(dialectKey) || languageKey == dialectKey)
                return false;
            Gain(languageKey, languageName, false, 1f, true);
            Gain(dialectKey, dialectName, true, 1f, true);
            initialized = true;
            return true;
        }

        public LanguageKnowledge Find(string key) => entries.Find(e => e.key == key);

        public bool InitializeNewborn(string languageKey, string languageName, string dialectKey, string dialectName)
        {
            // Only a freshly born pawn with an empty registry can enter this path.
            if (initialized || entries.Count != 0 || string.IsNullOrEmpty(languageKey) ||
                string.IsNullOrEmpty(dialectKey) || languageKey == dialectKey) return false;
            SetProgress(languageKey, languageName, false, 0f);
            SetProgress(dialectKey, dialectName, true, 0f);
            Find(languageKey).native = true;
            Find(dialectKey).native = true;
            initialized = true;
            return true;
        }

        public float Progress(string key) => ProgressMath.Clamp(Find(key)?.progress ?? 0f);

        public bool NeedsLesson(string languageKey, string dialectKey) =>
            Progress(languageKey) < 1f || Progress(dialectKey) < 1f;

        // Developer edits use the same records and preserve native identity.
        public bool DevSetLanguageProgress(string key, string name, IEnumerable<string> dialectKeys, float progress)
        {
            if (string.IsNullOrEmpty(key) || dialectKeys == null || !Finite(progress)) return false;
            progress = ProgressMath.Clamp(progress);
            SetProgress(key, name, false, progress);
            if (progress < 1f)
                foreach (var dialectKey in dialectKeys)
                {
                    var dialect = Find(dialectKey);
                    if (dialect?.isDialect == true) dialect.progress = 0f;
                }
            return true;
        }

        public bool DevSetDialectProgress(string languageKey, string languageName,
            string dialectKey, string dialectName, float progress)
        {
            if (string.IsNullOrEmpty(languageKey) || string.IsNullOrEmpty(dialectKey) ||
                languageKey == dialectKey || !Finite(progress)) return false;
            progress = ProgressMath.Clamp(progress);
            if (progress > 0f) SetProgress(languageKey, languageName, false, 1f);
            SetProgress(dialectKey, dialectName, true, progress);
            return true;
        }

        private static bool Finite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);

        private void SetProgress(string key, string name, bool dialect, float progress)
        {
            var entry = Find(key);
            if (entry == null)
            {
                entry = new LanguageKnowledge { key = key, isDialect = dialect };
                entries.Add(entry);
            }
            entry.lastKnownName = name;
            entry.progress = progress;
        }

        // One learning event affects exactly one stage. No overflow into a second stage.
        public void Learn(string languageKey, string languageName, float languageCost,
            string dialectKey, string dialectName, float dialectCost, float xp)
        {
            if (!ProgressMath.PositiveFinite(xp) || string.IsNullOrEmpty(languageKey) || string.IsNullOrEmpty(dialectKey)) return;
            bool dialectStage = Progress(languageKey) >= 1f;
            float cost = dialectStage ? dialectCost : languageCost;
            if (!ProgressMath.PositiveFinite(cost)) return;
            float gain = xp / cost;
            if (!ProgressMath.PositiveFinite(gain)) return;
            Gain(dialectStage ? dialectKey : languageKey, dialectStage ? dialectName : languageName, dialectStage, gain);
        }

        public void Gain(string key, string name, bool dialect, float amount, bool native = false)
        {
            if (string.IsNullOrEmpty(key) || float.IsNaN(amount) || float.IsInfinity(amount) || amount <= 0) return;
            var entry = Find(key);
            if (entry == null)
            {
                entry = new LanguageKnowledge { key = key, lastKnownName = name, isDialect = dialect };
                entries.Add(entry);
            }
            entry.lastKnownName = name;
            entry.native |= native;
            entry.progress = ProgressMath.Add(entry.progress, amount);
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref schema, "schema", 1);
            Scribe_Values.Look(ref initialized, "initialized");
            Scribe_Values.Look(ref originCaptured, "originCaptured");
            Scribe_References.Look(ref originFaction, "originFaction");
            Scribe_Collections.Look(ref entries, "entries", LookMode.Deep);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                if (entries == null) entries = new List<LanguageKnowledge>();
                entries.RemoveAll(e => e == null || string.IsNullOrEmpty(e.key));
                var seen = new HashSet<string>();
                entries.RemoveAll(e => !seen.Add(e.key));
            }
        }
    }
}
