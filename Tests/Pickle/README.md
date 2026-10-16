# Pickle suite of Perfectly Generic Item Renew

Test companion `nelim.perfectlygenericitemrenew.pickletests` (`Mod/`), development only, never published.
Written 2026-10-16, **not played**: no ticket filed, every outcome below is `unverified`. No local C# step:
Pickle's own catalogue (Docs/steps.md) and PickleTools' SaveSteps cover everything.

The mod's whole behavior needs a save made with another mod, loaded without it, so the suite has a making pass
and a loading pass. The throwaway item mod `nelim.pgifixture.items` (`FixtureMod/`: two items and a cloth) is
staged only in the making pass.

| Pass | Map | Filter | What it establishes |
| --- | --- | --- | --- |
| make-fixture (once) | `wsl-deps.make-fixture.map` | `00-make-fixture` | makes `pgi-missing-mod.rws` with the item mod active and hands it to the companion. The file must then be copied into `Mod/Pickle/Fixtures/` and committed (whether the hand-over reaches the repository or only the staged copy is unknown) |
| minimal | none | `01-missing-things-replaced` | S1 (missing items become PerfectlyGenericItem, no error), S2 (a garment whose cloth is gone loads), item mod absent |
| despawn | `wsl-deps.despawn.map` + `config/despawn/` seed | `02-despawn` | S4 (setting on: generic items vanish within 600 ticks) |
| English and French | `-Language English` / `-Language French` | as above | the mod adds no scenario text; one run per language is only worth it if the settings page is exercised, which no scenario does yet |

Not in Pickle, with the reason:
- S3 (never offered in material pickers, generated bases, quest rewards): the decisions are asserted offline
  (`Tests/RESULTS.md`, `CanMake`, `IsStuffAllowed`, `IsDerpAndDisallowed`) and `allowedInStuffGeneration=false` is
  XML. No step reads a picker or generates a base.
- S4 with the setting off: covered by feature 01 (items still exist after load).
- S5 and S6 (settings window, hidden shortcut, FR/EN layout): need RIMMSQOL and a window read, played by hand.
- S7 (StuffedFloors): needs that mod and its package id.

Assumptions to check at the first run: the fixture cells (130,150), (131,150) are free on `test-colony` (copied from
AncientSalvage); `Apparel_Parka` accepts Fabric cloth; the settings seed name assumes the staged folder is
`local-nelim.perfectlygenericitemrenew`; the map path names this repository's current folder
(`PerfectlyGenericItemPatched`), to change when the folder is renamed to Renew.
