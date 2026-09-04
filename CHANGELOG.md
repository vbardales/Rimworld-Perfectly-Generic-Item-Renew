# Changelog

Format inspired by [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
This file serves the repository and the writing of Steam patch notes; RimWorld does not display
it in game.

## [1.0.0] — 2026-09-04

First release of the 1.6 update. The mod's own design is unchanged from Ogliss' 1.4 version, apart
from the fixes below.

### Added

- Support for RimWorld 1.6. The mod was dead on 1.6, not merely flagged red: `PatchAll` applies
  its four patches in one call, `BaseGenUtility.IsCheapWallStuff` no longer exists, and the
  resulting exception took the whole static constructor down — including the
  `BackCompatibility.BackCompatibleDefName` patch that is the entire point of the mod.
- `<allowedInStuffGeneration>false</allowedInStuffGeneration>` on the def's `stuffProps`, in place
  of the removed `IsCheapWallStuff` patch. That flag is what 1.6 checks in
  `GenStuff.AllowedStuffsFor`, so the generic item stays out of generated walls, cheap floors and
  build costs without Harmony being involved.
- A French translation: the two settings keys and the item's own labels.
- `incompatibleWith` on the original `Ogliss.PerfectlyGenericItem`, so the two cannot run together.
- A mod icon, generated with an image model for this repository. Ogliss' mod ships none.

### Fixed

- **Translation injections were treated as lost items.** `BackCompatibleDefName` resolves both
  save references and `DefInjected` entries; the postfix answered both, so a missing def's
  translated labels were written onto the generic item and renamed it in-game. The
  `forDefInjections` parameter is now tested.
- **Successful compatibility conversions were overridden.** The test looked at `defName`, the
  input, and never at `__result`, the output. When RimWorld's own compatibility chain had renamed
  an old def into one that is present, the postfix still answered "generic item", losing a thing
  that could have been recovered. It now only fires when `__result` resolves to nothing either.
- **The StuffedFloors patch was registered as a prefix while written as a postfix.** The second
  positional parameter of `Harmony.Patch` is the prefix, and the method reads
  `ref List<ThingDef> __result`, which does not exist before the original runs. The generic item
  was therefore never hidden from StuffedFloors' floor list, and the patch would have thrown had
  the list been touched. It is now passed as `postfix:`, named explicitly.
- **`AccessTools.Method` was called even when StuffedFloors is absent**, which logs a Harmony
  error about a null type. The type is now tested first.
- **`StuffProperties.soundImpactStuff` was renamed `soundImpactBullet` in 1.6.** The old name
  threw an XML error on every load.
- **The French translation carried no accents.** Its labels and tooltips are now written properly.

### Changed

- The four patch targets are named through `nameof` rather than as strings, so a target
  disappearing in a future version breaks the build instead of killing the mod at load time.
- Removed a stray `<!-- Xenobiologis Playable Vespid Mod -->` comment from the English and
  Japanese key files: copy-paste residue from an unrelated mod.
- The published content moved to `Mod/`, with `Source/` and `Art/` beside it. The Workshop
  uploader sends the whole folder with no exclusion possible, so the only way not to publish the
  sources is for them not to be in there.
- The mod icon was resized from 1254×1254 to 128×128, cutting 1.4 MB from a mod that renders it
  at about 32 px. The full-size original is kept in `Art/`.
