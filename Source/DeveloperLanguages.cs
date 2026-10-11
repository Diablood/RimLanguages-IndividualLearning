using System.Linq;
using System.Collections.Generic;
using FactionLanguages;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimLanguages.IndividualLearning
{
    public static class IndividualLanguageDeveloperActions
    {
        [DebugAction("Rim Languages", "RLI: add language or dialect to pawn", allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void AddKnowledge()
        {
            var map = Find.CurrentMap;
            LudeonTK.DebugTools.curTool = new DebugTool("RLI_DevAddPawn".Translate(), () =>
            {
                if (!Prefs.DevMode || Find.CurrentMap != map) { LudeonTK.DebugTools.curTool = null; return; }
                var comp = IndividualLearning.Ready(UI.MouseCell().GetFirstPawn(map));
                if (comp == null) return;
                LudeonTK.DebugTools.curTool = null;
                var db = CompIndividualLanguages.Database;
                if (db == null) return;
                var options = new List<DebugMenuOption>();
                foreach (var language in db.RaceLanguages.OrderBy(l => l.Name))
                {
                    if (language.Def == null) continue;
                    if (comp.Data.Progress(language.GetUniqueLoadID()) < 1f)
                        options.Add(new DebugMenuOption("RLI_Language".Translate(language.Name), DebugMenuOptionMode.Action,
                            () => Apply(comp, language, null)));
                    foreach (var dialect in language.Dialects.OrderBy(d => d.Name))
                        if (IndividualLearning.Valid(dialect) && comp.Data.Progress(dialect.GetUniqueLoadID()) < 1f)
                            options.Add(new DebugMenuOption("RLI_Dialect".Translate(language.Name + " / " + dialect.Name),
                                DebugMenuOptionMode.Action, () => Apply(comp, language, dialect)));
                }
                if (options.Count == 0)
                    Find.WindowStack.Add(new Dialog_MessageBox("RLI_DevAddNone".Translate(comp.Pawn.LabelShortCap)));
                else
                    Find.WindowStack.Add(new Dialog_DebugOptionListLister(options, "RLI_DevAddTitle".Translate(comp.Pawn.LabelShortCap)));
            });
        }

        private static void Apply(CompIndividualLanguages comp, RaceLanguage language, Dialect dialect)
        {
            if (!Prefs.DevMode || Scribe.mode != LoadSaveMode.Inactive || IndividualLearning.Ready(comp.Pawn) != comp) return;
            var db = CompIndividualLanguages.Database;
            if (db == null || language.Def == null || !db.RaceLanguages.Contains(language)) return;
            if (dialect == null)
                comp.Data.DevSetLanguageProgress(language.GetUniqueLoadID(), language.Name,
                    language.Dialects.Select(d => d.GetUniqueLoadID()), 1f);
            else if (IndividualLearning.Valid(dialect) && dialect.Parent == language)
                comp.Data.DevSetDialectProgress(language.GetUniqueLoadID(), language.Name,
                    dialect.GetUniqueLoadID(), dialect.Name, 1f);
        }
    }

    public sealed class Dialog_IndividualLanguageProgress : Window
    {
        private readonly CompIndividualLanguages comp;
        private readonly RaceLanguage language;
        private readonly Dialect dialect;
        private float percent;
        public override Vector2 InitialSize => new Vector2(300f, 125f);

        public Dialog_IndividualLanguageProgress(CompIndividualLanguages comp, RaceLanguage language, Dialect dialect)
        {
            this.comp = comp;
            this.language = language;
            this.dialect = dialect;
            percent = comp.Data.Progress(dialect == null ? language.GetUniqueLoadID() : dialect.GetUniqueLoadID()) * 100f;
            forcePause = true;
            absorbInputAroundWindow = true;
            closeOnClickedOutside = true;
            layer = WindowLayer.Super;
            doWindowBackground = false;
            drawShadow = false;
            preventCameraMotion = false;
        }

        protected override void SetInitialSizeAndPosition()
        {
            var position = UI.MousePositionOnUIInverted + new Vector2(4f, 0f);
            var size = InitialSize;
            windowRect = new Rect(Mathf.Clamp(position.x, 0f, Mathf.Max(0f, UI.screenWidth - size.x)),
                Mathf.Clamp(position.y, 0f, Mathf.Max(0f, UI.screenHeight - size.y)), size.x, size.y);
        }

        public override void DoWindowContents(Rect rect)
        {
            if (!Prefs.DevMode) { Close(); return; }
            Widgets.DrawWindowBackground(rect);
            string target = dialect == null ? "RLI_Language".Translate(language.Name).ToString()
                : "RLI_Dialect".Translate(language.Name + " / " + dialect.Name).ToString();
            var previousAnchor = Text.Anchor;
            var previousFont = Text.Font;
            Text.Font = GameFont.Small;
            try
            {
                Text.Anchor = TextAnchor.MiddleCenter;
                Widgets.Label(new Rect(10f, 0f, rect.width - 20f, 40f), target.Truncate(rect.width - 20f));
            }
            finally { Text.Anchor = previousAnchor; Text.Font = previousFont; }
            TooltipHandler.TipRegion(new Rect(0f, 0f, rect.width, 40f), "RLI_DevEditTitle".Translate(comp.Pawn.LabelShortCap, target));
            percent = GUI.HorizontalSlider(new Rect(20f, 40f, rect.width - 40f, 10f), percent, 0f, 100f);
            var button = new Rect(20f, rect.height - 30f, rect.width - 40f, 20f);
            string rule = dialect == null ? "RLI_DevLanguageRule" : "RLI_DevDialectRule";
            TooltipHandler.TipRegion(button, rule.Translate());
            float roundedPercent = Mathf.Round(percent * 100f) / 100f;
            if (Widgets.ButtonText(button, "RLI_DevSetTo".Translate(roundedPercent.ToString("0.00"))))
            {
                percent = roundedPercent;
                Apply();
            }
        }

        private void Apply()
        {
            if (!Prefs.DevMode || Scribe.mode != LoadSaveMode.Inactive || !comp.EnsureInitialized()) return;
            var db = CompIndividualLanguages.Database;
            if (db == null || !db.RaceLanguages.Contains(language)) return;
            if (dialect == null)
                comp.Data.DevSetLanguageProgress(language.GetUniqueLoadID(), language.Name,
                    language.Dialects.Select(d => d.GetUniqueLoadID()), percent / 100f);
            else if (IndividualLearning.Valid(dialect) && dialect.Parent == language)
                comp.Data.DevSetDialectProgress(language.GetUniqueLoadID(), language.Name,
                    dialect.GetUniqueLoadID(), dialect.Name, percent / 100f);
        }
    }
}
