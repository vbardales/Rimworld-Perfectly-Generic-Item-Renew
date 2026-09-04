using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace PerfectlyGenericItem
{
    [StaticConstructorOnStartup]
    internal static class Main
    {
        private const string Prefix = "Perfectly Generic Item: ";

        static Main()
        {
            Harmony harmony = new Harmony("com.ogliss.rimworld.mod.PerfectlyGenericItem");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            PatchStuffedFloors(harmony);
        }

        // Optional compatibility: StuffedFloors builds the list of materials a
        // floor can be made of, and the generic item has no business in it.
        private static void PatchStuffedFloors(Harmony harmony)
        {
            Type controller = GenTypes.GetTypeInAnyAssembly("StuffedFloors.Controller", "StuffedFloors");
            if (controller == null)
            {
                return;
            }
            MethodInfo target = AccessTools.Method(controller, "GetStuffDefsFor", new Type[1] { typeof(StuffCategoryDef) });
            if (target == null)
            {
                return;
            }
            MethodInfo postfix = typeof(Main).GetMethod(nameof(StuffedFloors_GetStuffDefsFor_Postfix));
            if (postfix == null)
            {
                Log.Warning(Prefix + "patch method " + nameof(StuffedFloors_GetStuffDefsFor_Postfix) + " is null");
                return;
            }
            // Deliberately a postfix: the method reads __result, which does not
            // exist yet in a prefix.
            if (harmony.Patch(target, postfix: new HarmonyMethod(postfix)) == null)
            {
                Log.Warning(Prefix + "StuffedFloors.Controller patch failed to apply");
            }
        }

        public static void StuffedFloors_GetStuffDefsFor_Postfix(ref List<ThingDef> __result)
        {
            __result?.RemoveAll((ThingDef x) => x == PGIThingDefOf.PerfectlyGenericItem);
        }
    }
}
