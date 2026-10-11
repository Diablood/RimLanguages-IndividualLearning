using FactionLanguages;
using FactionLanguages.Utilities;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RimLanguages.IndividualLearning
{
    public static class IndividualLearning
    {
        public static CompIndividualLanguages Ready(Pawn pawn)
        {
            if (pawn?.RaceProps?.Humanlike != true || pawn.Dead) return null;
            var comp = pawn.GetComp<CompIndividualLanguages>();
            return comp != null && comp.EnsureInitialized() ? comp : null;
        }

        public static bool Valid(Dialect dialect)
        {
            var db = CompIndividualLanguages.Database;
            return dialect?.Parent?.Def != null && dialect.Def != null && db != null &&
                db.RaceLanguages.Contains(dialect.Parent) && dialect.Parent.Dialects.Contains(dialect);
        }

        public static bool NeedsLesson(PawnLanguages data, Dialect dialect) =>
            data.NeedsLesson(dialect.Parent.GetUniqueLoadID(), dialect.GetUniqueLoadID());

        public static void Learn(PawnLanguages data, Dialect dialect, float xp, Pawn learner)
        {
            bool dialectStage = data.Progress(dialect.Parent.GetUniqueLoadID()) >= 1f;
            string key = dialectStage ? dialect.GetUniqueLoadID() : dialect.Parent.GetUniqueLoadID();
            float before = data.Progress(key);
            data.Learn(dialect.Parent.GetUniqueLoadID(), dialect.Parent.Name, dialect.Parent.XpMax,
                dialect.GetUniqueLoadID(), dialect.Name, dialect.XpMax, xp);
            LearningHistory.Record(learner, LearningHistory.Changes(before, data.Progress(key), dialectStage));
        }

        public static Dialect NativeDialect(PawnLanguages data)
        {
            var db = CompIndividualLanguages.Database;
            if (db == null) return null;
            foreach (var language in db.RaceLanguages)
                foreach (var dialect in language.Dialects)
                {
                    var known = data.Find(dialect.GetUniqueLoadID());
                    if (known?.native == true && known.isDialect && known.progress >= 1f) return dialect;
                }
            return null;
        }

        public static void Conversation(Pawn learner, Pawn speaker)
        {
            if (learner == speaker) return;
            var student = Ready(learner);
            var teacher = Ready(speaker);
            if (student == null || teacher == null) return;
            var dialect = NativeDialect(teacher.Data);
            if (!Valid(dialect) || !NeedsLesson(student.Data, dialect)) return;
            var studentSocial = learner.skills?.GetSkill(SkillDefOf.Social);
            var teacherSocial = speaker.skills?.GetSkill(SkillDefOf.Social);
            // Being unable to perform social work does not prevent a native speaker
            // from providing language exposure during an already successful exchange.
            if (studentSocial == null || teacherSocial == null || studentSocial.TotallyDisabled) return;
            // Evaluating a disabled stat emits a developer-mode consistency error.
            // Query the stat's own availability, including compatibility overrides.
            float negotiation = StatDefOf.NegotiationAbility.Worker.IsDisabledFor(speaker)
                ? StatDefOf.NegotiationAbility.minValue
                : speaker.GetStatValue(StatDefOf.NegotiationAbility);
            bool languageStage = student.Data.Progress(dialect.Parent.GetUniqueLoadID()) < 1f;
            float baseCost = languageStage ? dialect.Parent.Def.GetBaseCost() : dialect.Def.GetBaseCost();
            // An isolated seed preserves Cybranian's and the game's existing random stream.
            int seed = unchecked(Find.TickManager.TicksGame ^ (learner.thingIDNumber * 397) ^ (speaker.thingIDNumber * 7919));
            float variation;
            Rand.PushState(seed);
            try { variation = Rand.Range(0.5f, 1.5f); }
            finally { Rand.PopState(); }
            float xp = ProgressMath.ConversationXp(baseCost,
                learner.GetStatValue(StatDefOfLocal.LanguageLearningAbility), negotiation,
                LanguageUtility.Settings.languageLearnDifficulty, dialect.Parent.Def.GetDifficulty(),
                dialect.Def.GetDifficulty(dialect.TechLevel), variation);
            Learn(student.Data, dialect, xp, learner);
        }
    }

    [HarmonyPatch(typeof(BookOutcomeDoerGainLanguageExp), nameof(BookOutcomeDoerGainLanguageExp.DoesProvidesOutcome))]
    public static class IndividualBookEligibilityPatch
    {
        public static void Postfix(BookOutcomeDoerGainLanguageExp __instance, Pawn reader, ref bool __result)
        {
            // Preserve the original collective outcome; also allow personal catch-up after global mastery.
            if (__result) return;
            var comp = IndividualLearning.Ready(reader);
            if (comp == null) return;
            var dialect = __instance.Dialect;
            if (IndividualLearning.Valid(dialect)) __result = IndividualLearning.NeedsLesson(comp.Data, dialect);
        }
    }

    [HarmonyPatch(typeof(BookOutcomeDoerGainLanguageExp), nameof(BookOutcomeDoerGainLanguageExp.OnReadingTick))]
    public static class IndividualBookReadingPatch
    {
        public static void Postfix(BookOutcomeDoerGainLanguageExp __instance, Pawn reader, float factor, bool __runOriginal)
        {
            if (!__runOriginal) return;
            var comp = IndividualLearning.Ready(reader);
            if (comp == null) return;
            var dialect = __instance.Dialect;
            if (!IndividualLearning.Valid(dialect) || !IndividualLearning.NeedsLesson(comp.Data, dialect)) return;
            // Personal XP is credited only for actual reading ticks, never in prepaid 500-tick blocks.
            IndividualLearning.Learn(comp.Data, dialect, __instance.ProgressPerTick(reader, factor), reader);
        }
    }

    [HarmonyPatch(typeof(Pawn_InteractionsTracker), nameof(Pawn_InteractionsTracker.TryInteractWith))]
    public static class IndividualConversationPatch
    {
        public static void Prefix(Pawn ___pawn, Pawn recipient, out InteractionPresentation __state) =>
            __state = new InteractionPresentation(___pawn, recipient);

        public static void Finalizer(InteractionPresentation __state) => __state?.End();

        [HarmonyPriority(Priority.Last)]
        public static void Postfix(bool __result, bool __runOriginal, Pawn ___pawn, Pawn recipient,
            InteractionDef intDef, InteractionPresentation __state)
        {
            if (!__runOriginal || !__result) return;
            // The existing prisoner lesson also uses DeepTalk. Keep its original personal formula.
            if (___pawn?.CurJob?.def == JobDefOfLocal.LearnPrisonerLanguage &&
                ___pawn.CurJob.GetTarget(Verse.AI.TargetIndex.B).Thing == recipient)
            {
                IndividualLearning.Conversation(___pawn, recipient);
                IndividualLearning.Conversation(recipient, ___pawn);
            }
            else if (intDef == InteractionDefOf.Chitchat || intDef == InteractionDefOf.DeepTalk)
            {
                if (AutomaticTransmission.Exchange(___pawn, recipient)) __state?.MarkTransmission();
            }
        }
    }
}
