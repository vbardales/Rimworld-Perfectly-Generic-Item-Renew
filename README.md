# Perfectly Generic Item Patched 1.6

Update of Ogliss' **Perfectly Generic Item** to RimWorld 1.6.

**I am not the author of this mod.** The idea, the design and the original code are entirely
Ogliss', made for MercuryDoll — all I did was the work needed to make it run on 1.6, plus three
bug fixes. Credit goes to them; mistakes in the update are mine. If the original author comes back
to it, or asks me to take this down, I will.

Original mod: https://steamcommunity.com/sharedfiles/filedetails/?id=2169353373 (stays on 1.4)

## What the mod does

When you remove a mod from an ongoing save, every item it added is lost, and the log fills with
"could not load reference to ThingDef". This mod catches those missing things as the save is read
and turns each one into a perfectly generic item — a stack of green placeholder blocks, the way
Starbound does it. Nothing disappears silently, and you can haul, sell or destroy the leftovers at
your own pace. A mod setting makes them despawn on their own a few seconds after loading.

The generic item is also a stuff, so it can stand in for a wall or a garment whose material is
gone. It is excluded from every material picker, so it is never offered for construction, never
used by generated bases, and never handed out as a quest reward.

Available in English, Japanese (both Ogliss') and French.

**Harmony is required.** Everything the mod does, it does through Harmony patches, and neither
RimWorld nor this mod ships the library. Without it the mod loads and does nothing at all.

Safe to add to an ongoing save — that is what it is for. Removing it mid-save turns any generic
items still lying around into the same missing-reference errors it was there to absorb, so destroy
them first.

## What changed in the 1.6 update

The mod was **dead** on 1.6, not merely flagged red. Its Harmony setup patches all its hooks in
one `PatchAll` call, one of those hooks was gone, and the exception took the whole static
constructor down with it — including the missing-def replacement that is the entire point of the
mod.

- **`BaseGenUtility.IsCheapWallStuff` no longer exists in 1.6.** The patch that used it kept the
  generic item out of generated settlement walls; that job is now done by
  `allowedInStuffGeneration` on the def itself, which is what 1.6 checks in its place. The patch
  is gone, replaced by one line of XML.
- **`StuffProperties.soundImpactStuff` was renamed `soundImpactBullet`**, which was throwing an
  XML error on every load.
- The three remaining hooks — `StuffProperties.CanMake`, `SketchGenUtility.IsStuffAllowed`,
  `ThingSetMakerUtility.IsDerpAndDisallowed` — and `BackCompatibility.BackCompatibleDefName` still
  exist in 1.6 with the same signatures, and were recompiled unchanged against the 1.6 reference
  assemblies. Each is now named through `nameof`, so the next disappearance breaks the build
  instead of the mod.

## The bugs that were fixed

This is why the mod is "Patched" rather than a plain 1.6 update.

- **Translation injections were treated as lost items.** `BackCompatibleDefName` resolves both
  save references and `DefInjected` entries, and the mod answered both. So a missing def's
  translated labels were written onto the generic item, renaming it in-game. The
  `forDefInjections` flag is now tested, and injections are left alone.
- **Successful compatibility conversions were overridden.** The test looked at the input def name
  and never at the result, so when RimWorld's own compatibility chain had renamed an old def into
  a present one, the mod replaced it with the generic item anyway — losing a thing that could have
  been recovered. It now only steps in when nothing else resolved.
- **The optional StuffedFloors patch was registered as a prefix while being written as a
  postfix**, so it read a `__result` that did not exist yet. The generic item was therefore never
  hidden from StuffedFloors' floor list, as originally intended, and the patch would have thrown
  had the list been touched. It is now registered as a postfix.

## Licence and attribution

Ogliss' mod declares **no licence**: there is no `LICENSE` file in it, and its Steam description
carries no reuse statement. This repository therefore carries no licence of its own — relicensing
someone else's code is not mine to do. It follows the established Workshop practice for reviving
abandoned mods instead: explicit credit to the original author, a link to the source mod, and
immediate removal on their request. That is stated in the mod's own description, not only here.

See [ATTRIBUTION.md](ATTRIBUTION.md) for the full record of what was taken and what was changed.

## Layout

```
Mod/       what is published, and what the RimWorld/Mods junction points at
Source/    C# sources, never published
Art/       image sources kept out of the published folder
```

The Workshop uploader sends the junction target whole, with no exclusion possible, so anything
that must not ship simply lives outside `Mod/`.
