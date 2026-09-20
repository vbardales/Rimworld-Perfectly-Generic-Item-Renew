using RimWorld;
using Verse;

namespace PerfectlyGenericItem
{
    // Use native visibility so customization tools can reveal this optional shortcut.
    public sealed class MainButtonWorker_PGISettings : MainButtonWorker
    {
        public override void Activate()
        {
            Find.WindowStack.Add(new Dialog_ModSettings(PGIMod.Instance));
        }
    }
}
