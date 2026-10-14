# Perfectly Generic Item Patched (unofficial) — working notes

This is a standalone, single-mod repository. It is no longer part of
`C:\Users\nelim\Documents\rimworld`: the parent repository ignores it and tracks none
of its files. The only remote is `origin`, pointing to
https://github.com/vbardales/Rimworld-Perfectly-Generic-Item-Patched (public).
The branch is `main`; the old `master` rule applied only to the monorepo.

| Item | Value |
| --- | --- |
| Mod | Perfectly Generic Item Patched (unofficial) |
| packageId | nelim.perfectlygenericitempatched |
| Target | RimWorld 1.6 only |
| Original author | Ogliss, for MercuryDoll; Workshop 2169353373, installed source supports up to 1.4 |
| Source license | None declared in the installed source; see ATTRIBUTION.md |
| Content | One ThingDef, four Harmony patches, optional StuffedFloors postfix and settings |
| Required dependencies | Harmony (brrainz.harmony) and Core |

## Purpose

When a mod is removed from an existing save, `BackCompatibility.BackCompatibleDefName`
resolves definitions requested by the save. This mod substitutes `PerfectlyGenericItem`
for missing ThingDefs. The generic item can also replace a missing material. The
material filters and automatic despawn setting support this recovery behavior.

## Layout

`Mod/` is the distributed folder and the intended RimWorld/Mods junction target.
The Workshop uploader sends that whole directory, without exclusions.

- `Mod/`: About, Assemblies, Defs, Languages, Textures and distributed attribution.
- `Source/`: C# source, never distributed. The project outputs to `Mod/Assemblies/`.
- `build/`: ignored build intermediates and test output, inside this repository and outside Mod.
- `Art/`: image sources and generation records, never distributed.
- `Tests/` and `TEST_SCENARIOS.md`: technical tests, results and in-game validation plan.
- Root documentation, including this file and STATUS.md, stays outside Mod.

Keep the root and distributed ATTRIBUTION.md copies identical. They previously drifted
by one row in the source table. Intermediate output contains game reference assemblies;
never copy it into Mod or redistribute it with the mod.

## Porting constraints

Ogliss owns the original design and code. Preserve credit and the removal-on-request
commitment. `silent` is a workflow classification, not permission to redistribute.
The public unofficial suffix and exact notice are required by PUBLISHING.md.
Do not invent a license for third-party content.

Keep changes focused on porting, defect fixes and the required settings/localization
workflow. Do not add unrelated features. The original additions were French resources,
the icon and the composed Preview. The 2026-09-13 fixes add the required optional settings
shortcut and translated category, not a new gameplay system.

Do not rename the namespace, assembly or `PerfectlyGenericItem` defName: the compClass,
existing saves and stored settings depend on those identities. The public packageId and
displayed name are intentionally distinct from upstream. Use `nameof` for Harmony targets
so a removed method fails compilation instead of breaking startup silently.

`PGISettings.replaceFactions` is unused legacy serialized data. Keep it for compatibility;
no UI control is warranted. The useful `removePGI` setting is global, false by default,
and read on each rare tick. Disabling despawning does not restore previously removed items.

The StuffedFloors integration stays optional: look up the type before its method and skip
when absent. Register the patch with named `postfix:`; the second positional Harmony.Patch
argument is a prefix and previously caused the broken result handling.

Settings use the native Mod options page. The MainButtons shortcut opens the same
Dialog_ModSettings instance type for PGIMod.Instance. Native `buttonVisible=false` keeps
it hidden while permitting customization tools to reveal it. Never force-hide it every frame.

## Build

```powershell
dotnet build Source/PerfectlyGenericItem/PerfectlyGenericItem.csproj -c Release
```

The DLL is emitted to Mod/Assemblies. Deterministic compilation is enabled. Historically,
on 2026-09-12, rebuilding unchanged code produced MD5 `36444a4f7abd5942c135016bcbd29297`.
The initial 2026-09-13 audit also reproduced the shipped DLL byte for byte; its SHA256 and
command remain in STATUS.md. Those hashes describe the historical source, not later fixes.
A changed DLL from unchanged sources warrants investigation.

Directory.Build.props sends intermediates to this repository's ignored `build/`
(PUBLISHING.md: no dot entry at the repository root; the old `.build/` was renamed).

## Testing

Never launch RimWorld yourself. The user runs the in-game scenarios and supplies the log:
`C:\Users\nelim\AppData\LocalLow\Ludeon Studios\RimWorld by Ludeon Studios\Player.log`.
Do not describe a build or headless test as in-game validation.

The original minimal scenario: create a save with an item mod, place its items, save,
disable that mod and reload. The missing items should become green blocks without
missing ThingDef reference errors. A menu-only check cannot validate that behavior.
The expanded plan is in TEST_SCENARIOS.md. Automated test scope and actual outcomes
are recorded in Tests/RESULTS.md; unexecuted game scenarios remain pending.

## XML checks

The shared validators live in the parent repository:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File ..\scripts\Check-XmlFields.ps1 -ModPath Mod -ExtraAssemblies Mod\Assemblies\PerfectlyGenericItem.dll
powershell -NoProfile -ExecutionPolicy Bypass -File ..\scripts\Check-DefRefs.ps1 -ModPath Mod -Brief
powershell -NoProfile -ExecutionPolicy Bypass -File ..\scripts\Check-DefInjected.ps1 -TransMod Mod -ExtraAssemblies Mod\Assemblies\PerfectlyGenericItem.dll
```

The historical 2026-09-11 run found no unknown XML fields, unresolved references or invalid
translation paths (six keys then). Current results supersede that count. Check-XmlFields
catches renamed fields such as the soundImpactStuff failure fixed during the 1.6 port.

## STATUS.md

Keep its front matter field names and the shared workflow vocabulary. Update the stage
only from evidence, preserve historical results and the sweep-owned `session` identifier.
Record date, revision, local changes, actual checks and pending verification. The user
requires the task title to match the retained stage whenever STATUS.md is updated.

## Before publication

No Workshop upload has been recorded. The local `v1.0.0` tag points to the 2026-09-04
revision and predates the 2026-09-12 fixes. Reconcile releases before publication, preferably
with a new version; no tag rewrite or publication is authorized by routine fixes.
Historical notes claimed no downloads; that count was not verified by the audit.

Final in-game validation is still required. After any future Workshop upload, preserve
About/PublishedFileId.txt immediately. Steam takes About.xml's description only on first
creation; later description updates must be made on the Workshop page. An earlier false
claim of in-game testing was removed on 2026-09-12 before publication.

The governing documents are ../PUBLISHING.md, ../STYLE_RIMWORLD.md, ../MOD_SETTINGS.md and
../TRANSLATIONS.md. Explicit user workflow clarifications take priority.

## Language

All tracked documentation, code comments, About metadata and new commit messages are
English. French language resources and conversation with the user remain French.
The old exception that left this file in French was removed on 2026-09-13.
