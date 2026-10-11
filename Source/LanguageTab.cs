using System.Collections.Generic;
using System.Linq;
using FactionLanguages;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimLanguages.IndividualLearning
{
    public sealed class ITab_IndividualLanguages : ITab
    {
        private Vector2 scroll;
        private Pawn previousPawn;
        public ITab_IndividualLanguages()
        {
            size = new Vector2(510f, 420f);
            labelKey = "RLI_Tab";
        }
        public override bool IsVisible => SelPawn?.GetComp<CompIndividualLanguages>() != null;

        protected override void FillTab()
        {
            var pawn = SelPawn;
            var comp = pawn?.GetComp<CompIndividualLanguages>();
            if (comp == null) return;
            if (pawn != previousPawn) { scroll = Vector2.zero; previousPawn = pawn; }
            comp.EnsureInitialized();
            var rect = new Rect(10f, 10f, size.x - 20f, size.y - 20f);
            Widgets.Label(new Rect(rect.x, rect.y, rect.width, 30f), "RLI_Title".Translate(pawn.LabelShortCap));
            var notice = "RLI_LearningNotice".Translate().ToString();
            var noticeHeight = Text.CalcHeight(notice, rect.width);
            Widgets.Label(new Rect(rect.x, rect.y + 32f, rect.width, noticeHeight), notice);
            float top = rect.y + 40f + noticeHeight;
            if (!comp.Data.initialized)
            {
                Widgets.Label(new Rect(rect.x, top, rect.width, 100f), "RLI_Unassigned".Translate());
                return;
            }
            var names = new Dictionary<string, string>();
            var languages = new Dictionary<string, RaceLanguage>();
            var dialects = new Dictionary<string, Dialect>();
            var db = CompIndividualLanguages.Database;
            if (db != null)
                foreach (var language in db.RaceLanguages)
                {
                    names[language.GetUniqueLoadID()] = language.Name;
                    languages[language.GetUniqueLoadID()] = language;
                    foreach (var dialect in language.Dialects)
                    {
                        names[dialect.GetUniqueLoadID()] = dialect.Name;
                        dialects[dialect.GetUniqueLoadID()] = dialect;
                    }
                }
            var entries = comp.Data.entries.OrderBy(e => e.isDialect).ThenBy(e => e.lastKnownName).ToList();
            var outer = new Rect(rect.x, top, rect.width, rect.yMax - top);
            const float rowHeight = 32f;
            bool needsScroll = entries.Count * rowHeight > outer.height;
            if (!needsScroll) scroll = Vector2.zero;
            var view = new Rect(0f, 0f, outer.width - (needsScroll ? 18f : 0f), entries.Count * rowHeight);
            Widgets.BeginScrollView(outer, ref scroll, view, needsScroll);
            try
            {
                float y = 0;
                foreach (var entry in entries)
                {
                    string name;
                    bool available = names.TryGetValue(entry.key, out name);
                    string suffix = entry.native ? "RLI_Native".Translate().ToString() : "";
                    string label = (entry.isDialect ? "RLI_Dialect" : "RLI_Language").Translate(available ? name : entry.lastKnownName);
                    string fullLabel = label + suffix;
                    bool editable = Prefs.DevMode && available;
                    float contentWidth = view.width - (Prefs.DevMode ? 26f : 0f);
                    Widgets.Label(new Rect(0f, y + 2f, contentWidth - 186f, 26f), fullLabel.Truncate(contentWidth - 186f));
                    Widgets.FillableBar(new Rect(contentWidth - 176f, y + 8f, 100f, 16f), ProgressMath.Clamp(entry.progress));
                    Widgets.Label(new Rect(contentWidth - 68f, y + 2f, 68f, 26f), (entry.progress * 100f).ToString("0.00") + " %");
                    string tip = available ? fullLabel : fullLabel + "\n" + "RLI_Unavailable".Translate();
                    TooltipHandler.TipRegion(new Rect(0f, y, contentWidth, rowHeight), tip);
                    if (editable)
                    {
                        var editRect = new Rect(view.width - 22f, y + 6f, 20f, 20f);
                        TooltipHandler.TipRegion(editRect, "RLI_DevEditDesc".Translate());
                        var previousColor = GUI.color;
                        bool clicked;
                        GUI.color = Color.cyan;
                        try { clicked = GUI.Button(editRect, TexButton.Save); }
                        finally { GUI.color = previousColor; }
                        if (clicked)
                        {
                            var dialect = entry.isDialect ? dialects[entry.key] : null;
                            var language = dialect?.Parent ?? languages[entry.key];
                            Find.WindowStack.Add(new Dialog_IndividualLanguageProgress(comp, language, dialect));
                        }
                    }
                    y += rowHeight;
                }
            }
            finally { Widgets.EndScrollView(); }
        }
    }
}
