using RimWorld;
using Verse;

namespace PerfectlyGenericItem
{
    [DefOf]
    public static class PGIThingDefOf
    {
        public static ThingDef PerfectlyGenericItem;

        static PGIThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(PGIThingDefOf));
        }
    }
}
