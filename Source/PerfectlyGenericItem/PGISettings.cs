using Verse;

namespace PerfectlyGenericItem
{
    public class PGISettings : ModSettings
    {
        public bool removePGI = false;

        public bool replaceFactions = false;

        public static PGISettings Instance;

        public PGISettings()
        {
            Instance = this;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref removePGI, "PGI_remove", false);
            Scribe_Values.Look(ref replaceFactions, "PGI_replaceFactions", false);
        }
    }
}
