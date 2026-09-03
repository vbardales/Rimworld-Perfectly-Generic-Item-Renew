using Verse;

namespace PerfectlyGenericItem
{
    public class PGICompRemover : ThingComp
    {
        public override void CompTickRare()
        {
            base.CompTickRare();
            if (PGISettings.Instance != null && PGISettings.Instance.removePGI && parent.Spawned)
            {
                parent.DeSpawn();
            }
        }
    }
}
