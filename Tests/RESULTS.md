# Test results

Revision: `f67dd17` plus the local changes committed with this file. Date: 2026-10-14.

## Console unit tests

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File ..\scripts\Run-UnitTests.ps1 -ModRoot . -PassManagedToExe
```

Run from the repository root (one check reads `Mod/Defs/MainButtons.xml`).
Result: 34 assertions passed, exit 0. They cover the settings defaults, serialization round
trip, the despawn decision on a probe with the real spawned-state predicate, the three
construction/sketch/reward patches, the back-compat replacement rules, and the shortcut worker.

Not covered: anything that needs Unity (shaders, resources). `MainButtonWorker.Visible` reaches
`ModsConfig` and cannot run in a console, so the hidden-by-default decision is checked on the
def XML instead. No in-game behavior is established by this run.

## XML checks (`../scripts`)

- `Check-XmlFields.ps1`: 3 files, no unknown 1.6 field.
- `Check-DefRefs.ps1`: XML well formed, references and parents resolve.
- `Check-DefInjected.ps1`: 8 keys checked, 0 errors.

## Game scenarios

All pending: see `TEST_SCENARIOS.md`.
