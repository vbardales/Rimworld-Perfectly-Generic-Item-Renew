using System;
using HarmonyLib;
using RimWorld;
using RimWorld.SketchGen;
using Verse;

namespace PerfectlyGenericItem
{
    // L'objet generique est une "stuff" par ses stuffProps, sans quoi il ne
    // pourrait pas remplacer un mur ou un vetement disparu. Il ne doit pour
    // autant jamais etre propose comme matiere : ces trois patchs le retirent
    // de la construction, de la generation de bases et des recompenses.

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

    // Le coeur du mod : toute ThingDef que la sauvegarde reclame et que la liste
    // de mods courante ne fournit plus est relue comme l'objet generique, au lieu
    // d'etre perdue avec un message d'erreur.
    [HarmonyPatch(typeof(BackCompatibility), nameof(BackCompatibility.BackCompatibleDefName))]
    public static class PGI_BackCompatibility_BackCompatibleDefName_Patch
    {
        [HarmonyPostfix]
        [HarmonyPriority(Priority.Last)]
        public static void Postfix(Type defType, string defName, bool forDefInjections, ref string __result)
        {
            // Les injections de traduction passent par le meme point d'entree.
            // Y repondre "PerfectlyGenericItem" ferait ecrire les libelles d'une
            // def absente par-dessus ceux de l'objet generique.
            if (forDefInjections || defType != typeof(ThingDef))
            {
                return;
            }
            if (GenDefDatabase.GetDefSilentFail(defType, defName, false) != null)
            {
                return;
            }
            // Ne pas ecraser une conversion de compatibilite qui, elle, a abouti.
            if (!__result.NullOrEmpty() && GenDefDatabase.GetDefSilentFail(defType, __result, false) != null)
            {
                return;
            }
            __result = "PerfectlyGenericItem";
        }
    }
}
