# Test scenarios

Each behavior the README promises, with its preconditions, actions and expected result.
Automated coverage is listed in `Tests/RESULTS.md`. Every scenario below that needs a running
game is **pending**: RimWorld is never launched by a session, the owner plays them.

## S1. Missing ThingDef becomes the generic item (core behavior)

- Precondition: a save made with an item mod enabled, items placed on the map, saved.
- Actions: disable the item mod, enable this mod, load the save.
- Expected: the missing items are green generic blocks; no "could not load reference to
  ThingDef" errors for them in `Player.log`; items can be hauled, sold and destroyed.
- Status: pending (in game).

## S2. Missing material becomes the generic item as stuff

- Precondition: a save with a wall or a garment made of a material from a mod.
- Actions: remove that mod, load the save.
- Expected: the wall or garment loads with the generic item as its stuff, without errors.
- Status: pending (in game).

## S3. Generic item never offered or generated

- Preconditions: this mod active, a new game.
- Actions: open the material pickers (architect menu, bills), generate a settlement and a
  quest reward.
- Expected: the generic item appears in none of them (patches `CanMake`, `IsStuffAllowed`,
  `IsDerpAndDisallowed`, plus `allowedInStuffGeneration=false`).
- Status: patches decisions covered by unit tests; in-game confirmation pending.

## S4. Automatic despawn setting

- Preconditions: a save holding generic items.
- Actions: Options > Mods > Perfectly Generic Item Renew, tick the checkbox, return to the game,
  wait about 250 ticks; untick, reload a save with generic items.
- Expected: ticked, spawned generic items vanish on the next rare tick; unticked, they stay.
  The setting survives a restart. Items already removed do not come back.
- Status: logic covered by unit tests; in-game and persistence over restart pending.

## S5. Settings access

- Actions: reach the settings through Options > Mods; check the main menu bar.
- Expected: the options page opens and shows the single checkbox. The MainButtons shortcut is
  absent by default. With a customization mod revealing it, it opens the same page.
- Status: pending (in game, with and without RIMMSQOL).

## S6. Languages

- Actions: run the game in English then in French, open the settings page and the shortcut
  tooltip, and hover the generic item.
- Expected: no raw key visible, French text fits without clipping.
- Status: resources checked offline (`Check-DefInjected`); in-game pending.

## S7. StuffedFloors (optional)

- Preconditions: StuffedFloors installed.
- Expected: no log error at startup; the generic item is absent from its floor list.
- Status: pending, needs StuffedFloors.

## S8. Harmony missing

Not a scenario for this mod: the game reports a missing hard dependency itself. The mod
declares `brrainz.harmony` in `modDependencies`, which the sources and About.xml confirm.

## Pickle

No `Tests/Pickle/` exists yet. S1 to S4 are exactly what only a running game can show and
belong in a Pickle suite (a save loaded with a mod removed needs a map step that does not
exist yet). Recorded as a defect in `STATUS.md`, not as non-applicable.

`@requires` (when the suite is written): `Ludeon.RimWorld`, `brrainz.harmony` (hard dependencies,
in the minimal set) and `StuffedFloors` (S7, optional; its package id and the map that mounts it
are still to be chosen). One pass without the optional mod, one with it.
