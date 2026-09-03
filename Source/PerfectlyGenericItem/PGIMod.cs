using UnityEngine;
using Verse;

namespace PerfectlyGenericItem
{
    public sealed class PGIMod : Mod
    {
        public static PGIMod Instance;

        public PGISettings settings;

        public PGIMod(ModContentPack content)
            : base(content)
        {
            settings = GetSettings<PGISettings>();
            Instance = this;
            PGISettings.Instance = settings;
        }

        public override string SettingsCategory()
        {
            return "Perfectly Generic Item";
        }

        public override void DoSettingsWindowContents(Rect rect)
        {
            Listing_Standard listing = new Listing_Standard
            {
                ColumnWidth = rect.width
            };
            float sectionHeight = Text.LineHeight + listing.verticalSpacing;
            listing.Begin(rect);
            Listing_Standard section = listing.BeginSection(sectionHeight, 4f, 4f);
            section.ColumnWidth *= 0.488f;
            section.CheckboxLabeled(
                "PGI_RemoveOption".Translate(),
                ref settings.removePGI,
                "PGI_RemoveOptionToolTip".Translate());
            listing.EndSection(section);
            listing.End();
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
        }
    }
}
