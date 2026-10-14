# Perfectly Generic Item Renew — attribution

A 1.6 update of **Perfectly Generic Item**
(https://steamcommunity.com/sharedfiles/filedetails/?id=2169353373), by **Ogliss**,
written for MercuryDoll. It stays on 1.4.

Original `packageId`: `Ogliss.PerfectlyGenericItem`.

**The design and the code are Ogliss'.** This repository adds no feature: it
recompiles the assembly against 1.6, replaces the one hook that disappeared, and
fixes three defects along the way.

## Licence

**No licence is declared** upstream: there is no `LICENSE` file in the mod, and its
Steam description is one sentence long, with no reuse statement. This repository
therefore carries no licence of its own — relicensing someone else's code is not
mine to do. It follows the established Workshop practice for reviving abandoned
mods instead: explicit credit to the original author, a link to the source mod, and
immediate removal on their request. That is stated in the mod's own description, not
only here.

## What was taken

| Item | Source |
| --- | --- |
| `PGIThingDefOf`, the four Harmony patches, `PGICompRemover`, `PGIMod`, `PGISettings`, `Main` | `1.4/Assemblies/PerfectlyGenericItem.dll`, decompiled |
| `Defs/PerfectlyGenericItem.xml` | mod root |
| `PGI_single` textures | mod root |
| `About/Preview.png` | mod root — his image, recut to 896x504 with the mod name laid over it |
| English and Japanese keys | `Languages/` |

**The namespace, the assembly name `PerfectlyGenericItem` and the `defName`
`PerfectlyGenericItem` are all kept.** The def declares
`PerfectlyGenericItem.PGICompRemover` as its `compClass`, RimWorld stores mod
settings under the name of the `Mod` class, and the `defName` is what saves coming
from Ogliss' version already contain. Only the displayed name and the `packageId`
change, as the convention for revivals asks.

## What 1.6 required

The mod was not merely flagged red: it was **dead**. `PatchAll` applies its four
patches in a single call, one of them targeted a method that no longer exists, and
the exception took the whole static constructor down with it:

```
Error in static constructor of PerfectlyGenericItem.Main:
  Undefined target method for patch method
  PGI_BaseGenUtility_IsCheapWallStuff_Patch::Postfix(ThingDef d, bool& __result)
```

So the patch on `BackCompatibility.BackCompatibleDefName` — the mod's entire
purpose — never applied at all.

### `BaseGenUtility.IsCheapWallStuff` is gone

The method no longer exists in 1.6. It was used to keep the generic item out of the
walls of generated settlements. In 1.6 that filtering goes through
`GenStuff.AllowedStuffsFor(..., checkAllowedInStuffGeneration: true)`, which tests
`stuffProps.allowedInStuffGeneration`:

```csharp
if (thingDef.IsStuff && ... && thingDef.stuffProps.CanMake(td))
{
    found = thingDef;
    if (!checkAllowedInStuffGeneration || thingDef.stuffProps.allowedInStuffGeneration)
```

The patch was therefore removed and replaced with
`<allowedInStuffGeneration>false</allowedInStuffGeneration>` in the def's
`stuffProps` — the mechanism 1.6 uses in its place, without Harmony.
`RandomCheapWallStuff`, `TryRandomInexpensiveFloor` and `BuildCostsAllowedInStuffGen`
all go through it.

### `StuffProperties.soundImpactStuff` was renamed

The field is called `soundImpactBullet` in 1.6. The old name produced an XML error
on every load:

```
XML error: <soundImpactStuff>BulletImpact_Metal</soundImpactStuff> doesn't
correspond to any field in type StuffProperties.
```

### The rest holds

The three remaining targets exist in 1.6 with the same signature, and were
recompiled unchanged. Each patch now names them through `nameof`, so a future
disappearance would break the build instead of breaking the mod at load time:

| Target | 1.6 signature |
| --- | --- |
| `StuffProperties.CanMake` | `public bool CanMake(BuildableDef t)` |
| `SketchGenUtility.IsStuffAllowed` | `public static bool IsStuffAllowed(ThingDef stuff, bool, Map, bool, ThingDef)` |
| `ThingSetMakerUtility.IsDerpAndDisallowed` | `public static bool IsDerpAndDisallowed(ThingDef thing, ThingDef stuff, QualityGenerator?)` |
| `BackCompatibility.BackCompatibleDefName` | `public static string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)` |

The parameter names the postfixes use (`stuff`, `defType`, `defName`) are unchanged:
Harmony binds them by name, not by position.

## The defects that were fixed

### 1. Translation injections were treated as lost items

`BackCompatibleDefName` serves two purposes: reading a save back, and resolving the
`DefInjected` entries of a translation. The original postfix did not tell them
apart:

```csharp
if (GenDefDatabase.GetDefSilentFail(defType, defName, false) == null && defType == typeof(ThingDef))
{
    __result = "PerfectlyGenericItem";
}
```

The consequence: for any missing def named by a translation file, the game wrote its
labels **onto the generic item**, which ended up renamed at random. The
`forDefInjections` parameter is now tested, and injections are left alone.

### 2. Successful compatibility conversions were overridden

The test looked at `defName`, the input, and never at `__result`, the output. When
RimWorld's own compatibility chain had managed to rename an old def into one that is
present, the postfix still answered "generic item": a recoverable thing was lost.
The postfix now only fires when `__result` resolves to nothing either.

### 3. The StuffedFloors patch was registered as a prefix

```csharp
harmony.Patch(methodInfo, new HarmonyMethod(method));
```

The second positional parameter of `Harmony.Patch` is the **prefix**. The method
passed to it is written as a postfix, though: it reads `ref List<ThingDef> __result`,
which does not exist yet before the original method runs. So the generic item was
never removed from StuffedFloors' floor list, and the patch would have thrown a
`NullReferenceException` had the list been touched. It is now `postfix:`, named
explicitly.

Along the way, `AccessTools.Method` was being called even when StuffedFloors is
absent, which logs a Harmony error about a null type. The type is now tested first.

## Additions

- `Languages/French/` — settings keys and the item's labels.
- `About/ModIcon.png`, generated with an image model for this repository. Ogliss'
  mod ships none.

Nothing else was added.
