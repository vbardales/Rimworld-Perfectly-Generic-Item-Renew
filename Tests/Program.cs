using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using PerfectlyGenericItem;
using RimWorld;
using Verse;

internal static class Program
{
    private static int checks;
    private static int Main(string[] args)
    {
        if (args.Length != 2) { Console.Error.WriteLine("Usage: PerfectlyGenericItem.Tests.exe <game Managed folder> <test output folder>"); return 2; }
        AppDomain.CurrentDomain.AssemblyResolve += (sender, request) =>
        {
            string path = Path.Combine(args[0], new AssemblyName(request.Name).Name + ".dll");
            return File.Exists(path) ? Assembly.LoadFrom(path) : null;
        };
        try { Run(args[1]); Console.WriteLine($"PASS: {checks} assertions against the compiled mod and real game assemblies; no game process."); return 0; }
        catch (Exception ex) { Console.Error.WriteLine(ex); return 1; }
    }

    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
        checks++;
        Console.WriteLine("PASS: " + message);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void Run(string output)
    {
        Directory.CreateDirectory(output);
        var settings = new PGISettings();
        Check(!settings.removePGI && !settings.replaceFactions, "Clean defaults are false");
        Check(ReferenceEquals(PGISettings.Instance, settings), "Active settings instance is shared");
        string path = Path.Combine(output, "settings.xml");
        foreach (bool enabled in new[] { true, false })
        {
            settings.removePGI = enabled;
            settings.replaceFactions = true;
            Scribe.saver.InitSaving(path, "settings"); settings.ExposeData(); Scribe.saver.FinalizeSaving();
            var restored = Load(path);
            Check(restored.removePGI == enabled && restored.replaceFactions, "Real Scribe round trip: " + enabled);
            Check(ReferenceEquals(PGISettings.Instance, restored), "Reload replaces active settings instance");
        }
        File.WriteAllText(path, "<settings />");
        var empty = Load(path);
        Check(!empty.removePGI && !empty.replaceFactions, "Missing stored values use defaults");
        File.WriteAllText(path, "<settings><PGI_remove>true</PGI_remove><PGI_replaceFactions>true</PGI_replaceFactions></settings>");
        var legacy = Load(path);
        Check(legacy.removePGI && legacy.replaceFactions, "Legacy serialized names are preserved");

        // Construct only the state needed by Thing.Spawned. Never start or load a game.
        var game = (Game)FormatterServices.GetUninitializedObject(typeof(Game));
        typeof(Game).GetField("maps", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .SetValue(game, new List<Map> { (Map)FormatterServices.GetUninitializedObject(typeof(Map)) });
        Current.Game = game;
        var thing = new DespawnProbe();
        var comp = new PGICompRemover { parent = thing };
        typeof(Thing).GetField("mapIndexOrState", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(thing, (sbyte)0);
        Check(thing.Spawned, "Probe uses actual spawned-state predicate");
        PGISettings.Instance = null;
        comp.CompTickRare(); Check(thing.Despawns == 0, "Missing settings do not despawn");
        PGISettings.Instance = new PGISettings();
        comp.CompTickRare(); Check(thing.Despawns == 0, "Disabled setting preserves spawned item");
        PGISettings.Instance.removePGI = true;
        comp.CompTickRare(); Check(thing.Despawns == 1, "Enabled setting invokes despawn on next rare tick");
        PGISettings.Instance.removePGI = false;
        comp.CompTickRare(); Check(thing.Despawns == 1, "Disabling prevents further despawn calls");
        PGISettings.Instance.removePGI = true;
        thing.ForceSetStateToUnspawned();
        comp.CompTickRare(); Check(thing.Despawns == 1, "Unspawned item is preserved");
        Current.Game = null;

        // DefOf static constructors warn through Unity unless the helper is mid-binding.
        typeof(DefOfHelper).GetField("bindingNow", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).SetValue(null, true);
        var generic = Factory.NewDef<ThingDef>("PerfectlyGenericItem");
        var ordinary = Factory.NewDef<ThingDef>("PGITest_Ordinary");
        DefDatabase<ThingDef>.Add(generic); DefDatabase<ThingDef>.Add(ordinary);
        PGIThingDefOf.PerfectlyGenericItem = generic;
        foreach (bool initial in new[] { true, false })
        {
            bool result = initial;
            PGI_StuffProperties_CanMake_Patch.Postfix(new StuffProperties { parent = generic }, ref result);
            Check(!result, "Generic stuff cannot be used for construction: " + initial);
            result = initial;
            PGI_StuffProperties_CanMake_Patch.Postfix(new StuffProperties { parent = ordinary }, ref result);
            Check(result == initial, "Ordinary construction decision preserved: " + initial);
            result = initial; PGI_SketchGenUtility_IsStuffAllowed_Patch.Postfix(generic, ref result);
            Check(!result, "Generic stuff excluded from sketches: " + initial);
            result = initial; PGI_ThingSetMakerUtility_IsDerpAndDisallowed_Patch.Postfix(generic, ref result);
            Check(result, "Generic stuff excluded from rewards: " + initial);
            result = initial; PGI_SketchGenUtility_IsStuffAllowed_Patch.Postfix(ordinary, ref result);
            Check(result == initial, "Ordinary sketch decision preserved: " + initial);
            result = initial; PGI_ThingSetMakerUtility_IsDerpAndDisallowed_Patch.Postfix(ordinary, ref result);
            Check(result == initial, "Ordinary reward decision preserved: " + initial);
        }
        CheckReplacement(typeof(ThingDef), "PGITest_Missing", false, null, "PerfectlyGenericItem", "Missing ThingDef replaced");
        CheckReplacement(typeof(ThingDef), "PGITest_Missing", true, null, null, "DefInjected lookup left untouched");
        CheckReplacement(typeof(FactionDef), "PGITest_Missing", false, null, null, "Other def types left untouched");
        CheckReplacement(typeof(ThingDef), "PGITest_Ordinary", false, "PGITest_Ordinary", "PGITest_Ordinary", "Existing ThingDef preserved");
        CheckReplacement(typeof(ThingDef), "PGITest_Old", false, "PGITest_Ordinary", "PGITest_Ordinary", "Successful vanilla rename preserved");
        CheckReplacement(typeof(ThingDef), "PGITest_Missing", false, "PGITest_AlsoMissing", "PerfectlyGenericItem", "Unresolved rename replaced");
        // Native visibility is deliberately not overridden by the mod; the getter itself needs a live game
        // (ModsConfig), so the hidden-by-default decision is read from the def XML, run from the repository root.
        Check(File.ReadAllText(Path.Combine("Mod", "Defs", "MainButtons.xml")).Contains("<buttonVisible>false</buttonVisible>"), "MainButton is hidden by its definition");
        Check(typeof(MainButtonWorker_PGISettings).GetProperty("Visible").DeclaringType == typeof(MainButtonWorker), "No forced visibility override defeats customization");
    }

    private static void CheckReplacement(Type type, string name, bool injection, string initial, string expected, string label)
    {
        string result = initial;
        PGI_BackCompatibility_BackCompatibleDefName_Patch.Postfix(type, name, injection, ref result);
        Check(result == expected, label);
    }

    private static PGISettings Load(string path)
    {
        var settings = new PGISettings();
        Scribe.loader.InitLoading(path); settings.ExposeData();
        Scribe.mode = LoadSaveMode.PostLoadInit; settings.ExposeData(); Scribe.ForceStop();
        return settings;
    }

    // Def constructors reach Unity resources (shaders) that a console process lacks: skip them.
    // Nested so its Assembly-CSharp constraint loads only after the resolver is registered.
    private static class Factory
    {
        public static T NewDef<T>(string name, bool? visible = null) where T : Def
        {
            var def = (T)FormatterServices.GetUninitializedObject(typeof(T));
            def.defName = name;
            if (visible.HasValue) ((MainButtonDef)(object)def).buttonVisible = visible.Value;
            return def;
        }
    }

    // Observe the engine-boundary call without running map removal or Unity.
    private sealed class DespawnProbe : ThingWithComps
    {
        public int Despawns;
        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish) { Despawns++; }
    }
}
