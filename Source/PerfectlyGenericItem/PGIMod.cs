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
            return "PGI_SettingsCategory".Translate();
        }

        public override void DoSettingsWindowContents(Rect rect)
        {
            Listing_Standard listing = new Listing_Standard
            {
                ColumnWidth = rect.width
            };
            listing.Begin(rect);
            listing.CheckboxLabeled(
                "PGI_RemoveOption".Translate(),
                ref settings.removePGI,
                "PGI_RemoveOptionToolTip".Translate());
            listing.End();
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
        }
    }
}
