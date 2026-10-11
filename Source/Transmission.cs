using System;
using System.Collections.Generic;
using System.Linq;
using FactionLanguages;
using FactionLanguages.Utilities;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimLanguages.IndividualLearning
{
    public sealed class TransmissionSettings : ModSettings
    {
        public bool enabled = true;
        public static bool LegacyEnabled(float value) => float.IsNaN(value) || float.IsInfinity(value) || value > 0f;
        public override void ExposeData()
        {
            bool defaultEnabled = true;
            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                // Read the removed setting only while loading; never write it again.
                float legacySpeed = 1f;
                Scribe_Values.Look(ref legacySpeed, "conversationTransmissionSpeed", 1f);
                defaultEnabled = LegacyEnabled(legacySpeed);
            }
            Scribe_Values.Look(ref enabled, "conversationTransmissionEnabled", defaultEnabled);
        }
        public void Draw(Rect rect)
        {
            var listing = new Listing_Standard();
            listing.Begin(rect);
            listing.CheckboxLabeled("RLI_TransmissionEnabled".Translate(), ref enabled);
            listing.Label("RLI_TransmissionDescription".Translate());
            listing.End();
        }
    }

    // A lesson represents exactly one known language or dialect; no saved selection or task.
    public sealed class TransmissionLesson
    {
        public string key, name, parentKey;
        public bool dialect;
        public float xpMax, baseCost, languageDifficulty, dialectDifficulty = 1f;
    }

    public static class TransmissionRules
    {
        public static float AppliedPercentage(float before, float after)
        {
            if (!Finite(before) || !Finite(after)) return 0f;
            return Math.Max(0f, ProgressMath.Clamp(after) - ProgressMath.Clamp(before)) * 100f;
        }
        public static bool Eligible(PawnLanguages student, PawnLanguages teacher, TransmissionLesson lesson)
        {
            if (student == null || teacher == null || lesson == null || string.IsNullOrEmpty(lesson.key) ||
                !ProgressMath.PositiveFinite(lesson.xpMax) || teacher.Progress(lesson.key) < 1f ||
                student.Progress(lesson.key) >= 1f) return false;
            var entry = teacher.Find(lesson.key);
            return entry.isDialect == lesson.dialect && (!lesson.dialect ||
                (!string.IsNullOrEmpty(lesson.parentKey) && teacher.Progress(lesson.parentKey) >= 1f &&
                 student.Progress(lesson.parentKey) >= 1f));
        }

        public static TransmissionLesson Select(PawnLanguages student, PawnLanguages teacher,
            IEnumerable<TransmissionLesson> lessons, float choice)
        {
            if (lessons == null || !Finite(choice)) return null;
            var eligible = lessons.Where(l => Eligible(student, teacher, l)).ToList();
            if (eligible.Count == 0) return null;
            Func<TransmissionLesson, int> rank = l => student.Progress(l.key) > 0f ? 2 :
                teacher.Find(l.key).native ? 1 : 0;
            int best = eligible.Max(rank);
            var pool = eligible.Where(l => rank(l) == best).ToList();
            return pool[Math.Min(pool.Count - 1, (int)(ProgressMath.Clamp(choice) * pool.Count))];
        }

        public static float Xp(TransmissionLesson lesson, float learningAbility, int teacherSocial,
            bool studentDisabled, bool teacherDisabled, bool enabled, float difficulty, float variation)
        {
            if (lesson == null || studentDisabled || teacherDisabled || !Finite(learningAbility) ||
                learningAbility < 0f || !enabled) return 0f;
            // The parent's ability already includes learner Social. A small baseline permits level 0.
            float teacherFactor = 0.5f + 0.065f * Math.Max(0, Math.Min(20, teacherSocial));
            float xp = ProgressMath.ConversationXp(lesson.baseCost, 0.5f + learningAbility, teacherFactor,
                difficulty, lesson.languageDifficulty, lesson.dialectDifficulty, variation);
            return ProgressMath.PositiveFinite(xp) ? xp : 0f;
        }
        private static bool Finite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);
    }

    public static class AutomaticTransmission
    {
        public static bool Exchange(Pawn first, Pawn second)
        {
            if (first == second) return false;
            var a = IndividualLearning.Ready(first);
            var b = IndividualLearning.Ready(second);
            if (a == null || b == null) return false;
            var sa = first.skills?.GetSkill(SkillDefOf.Social);
            var sb = second.skills?.GetSkill(SkillDefOf.Social);
            if (sa == null || sb == null || sa.TotallyDisabled || sb.TotallyDisabled) return false;
            if (!(IndividualLearningMod.Settings?.enabled ?? true)) return false;
            var lessons = AvailableLessons();
            int seed = unchecked(Find.TickManager.TicksGame ^ (first.thingIDNumber * 397) ^ (second.thingIDNumber * 7919));
            Rand.PushState(seed);
            try
            {
                // Decide both ways before either learner reaches a new mastery threshold.
                var forA = TransmissionRules.Select(a.Data, b.Data, lessons, Rand.Value);
                var forB = TransmissionRules.Select(b.Data, a.Data, lessons, Rand.Value);
                bool firstLearned = Grant(first, a.Data, forA, sb.Level, Rand.Range(0.5f, 1.5f));
                bool secondLearned = Grant(second, b.Data, forB, sa.Level, Rand.Range(0.5f, 1.5f));
                return firstLearned || secondLearned;
            }
            finally { Rand.PopState(); }
        }

        internal static List<TransmissionLesson> AvailableLessons()
        {
            var result = new List<TransmissionLesson>();
            var db = CompIndividualLanguages.Database;
            if (db == null) return result;
            foreach (var language in db.RaceLanguages)
            {
                if (language?.Def == null) continue;
                result.Add(new TransmissionLesson { key = language.GetUniqueLoadID(), name = language.Name,
                    xpMax = language.XpMax, baseCost = language.Def.GetBaseCost(),
                    languageDifficulty = language.Def.GetDifficulty() });
                foreach (var dialect in language.Dialects)
                    if (IndividualLearning.Valid(dialect))
                        result.Add(new TransmissionLesson { key = dialect.GetUniqueLoadID(), name = dialect.Name,
                            parentKey = language.GetUniqueLoadID(), dialect = true, xpMax = dialect.XpMax,
                            baseCost = dialect.Def.GetBaseCost(), languageDifficulty = language.Def.GetDifficulty(),
                            dialectDifficulty = dialect.Def.GetDifficulty(dialect.TechLevel) });
            }
            return result;
        }

        private static bool Grant(Pawn pawn, PawnLanguages data, TransmissionLesson lesson,
            int teacherSocial, float variation)
        {
            if (lesson == null || StatDefOfLocal.LanguageLearningAbility.Worker.IsDisabledFor(pawn)) return false;
            float xp = TransmissionRules.Xp(lesson, pawn.GetStatValue(StatDefOfLocal.LanguageLearningAbility),
                teacherSocial, false, false, true, LanguageUtility.Settings.languageLearnDifficulty, variation);
            float gain = xp / lesson.xpMax;
            if (!ProgressMath.PositiveFinite(gain)) return false;
            float before = data.Progress(lesson.key);
            data.Gain(lesson.key, lesson.name, lesson.dialect, gain);
            LearningHistory.Record(pawn, LearningHistory.Changes(before, data.Progress(lesson.key), lesson.dialect));
            return TransmissionRules.AppliedPercentage(before, data.Progress(lesson.key)) > 0f;
        }
    }
}
