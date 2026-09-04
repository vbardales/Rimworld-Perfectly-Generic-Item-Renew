using System;
using HarmonyLib;
using RimWorld;
using RimWorld.SketchGen;
using Verse;

namespace PerfectlyGenericItem
{
    // The generic item is a stuff, through its stuffProps, or it could not stand
    // in for a wall or a garment whose material is gone. It must nevertheless
    // never be offered as a material: these three patches keep it out of
    // construction, out of generated bases and out of quest rewards.

    [HarmonyPatch(typeof(StuffProperties), nameof(StuffProperties.CanMake))]
    public static class PGI_StuffProperties_CanMake_Patch
    {
        [HarmonyPostfix]
        [HarmonyPriority(Priority.Last)]
        public static void Postfix(StuffProperties __instance, ref bool __result)
        {
            if (__instance.parent != null && __instance.parent == PGIThingDefOf.PerfectlyGenericItem)
            {
                __result = false;
            }
        }
    }

    [HarmonyPatch(typeof(SketchGenUtility), nameof(SketchGenUtility.IsStuffAllowed))]
    public static class PGI_SketchGenUtility_IsStuffAllowed_Patch
    {
        [HarmonyPostfix]
        [HarmonyPriority(Priority.Last)]
        public static void Postfix(ThingDef stuff, ref bool __result)
        {
            if (stuff == PGIThingDefOf.PerfectlyGenericItem)
            {
                __result = false;
            }
        }
    }

    [HarmonyPatch(typeof(ThingSetMakerUtility), nameof(ThingSetMakerUtility.IsDerpAndDisallowed))]
    public static class PGI_ThingSetMakerUtility_IsDerpAndDisallowed_Patch
    {
        [HarmonyPostfix]
        [HarmonyPriority(Priority.Last)]
        public static void Postfix(ThingDef stuff, ref bool __result)
        {
            if (stuff == PGIThingDefOf.PerfectlyGenericItem)
            {
                __result = true;
            }
        }
    }

    // The heart of the mod: any ThingDef the save asks for that the current mod
    // list no longer provides is read back as the generic item, instead of being
    // lost with an error message.
    [HarmonyPatch(typeof(BackCompatibility), nameof(BackCompatibility.BackCompatibleDefName))]
    public static class PGI_BackCompatibility_BackCompatibleDefName_Patch
    {
        [HarmonyPostfix]
        [HarmonyPriority(Priority.Last)]
        public static void Postfix(Type defType, string defName, bool forDefInjections, ref string __result)
        {
            // Translation injections come through the same entry point. Answering
            // "PerfectlyGenericItem" here would write a missing def's labels over
            // those of the generic item.
            if (forDefInjections || defType != typeof(ThingDef))
            {
                return;
            }
            if (GenDefDatabase.GetDefSilentFail(defType, defName, false) != null)
            {
                return;
            }
            // Do not override a compatibility conversion that did succeed.
            if (!__result.NullOrEmpty() && GenDefDatabase.GetDefSilentFail(defType, __result, false) != null)
            {
                return;
            }
            __result = "PerfectlyGenericItem";
        }
    }
}
