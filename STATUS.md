---
mod:          Perfectly Generic Item Renew (unofficial)
packageId:    nelim.perfectlygenericitemrenew
repo:         Rimworld-Perfectly-Generic-Item-Renew
remote:       https://github.com/vbardales/Rimworld-Perfectly-Generic-Item-Renew.git
visibility:   public
detached:     yes
workflow_stage: writeTests[1.0.1]
licence:      silent
licence_at:   "2026-09-12: no upstream LICENSE, a one-sentence Steam description; credit and removal-on-request stand in its place"
upstream_mod_remotes: N/A
settings_audit: partial
localization: complete
translation_en: complete
translation_fr: complete
dependencies: declared
showcase:     partial
tested_on:
workshop:     3806765659 (PublishedFileId.txt present, nothing uploaded by this audit)
published:    no
remaining:
  - unverified: Pickle suite written 2026-10-16 (Tests/Pickle: S1, S2, S4), never played; the fixture save is not made yet (make-fixture pass), S3 and S5 to S7 stay offline or manual with reasons in Tests/Pickle/README.md
  - defect: Art/echo.png should be Art/echo-source.png (minimal set, PUBLISHING.md); owner-made source, not renamed by a session
  - defect: no PUBLICATION.md yet (writeDocs)
  - unverified: the missing-def replacement itself, which needs a save loaded with a mod taken out
  - unverified: the generic item standing in as the stuff of a wall or a garment whose material is gone
  - unverified: the three patches that keep it out of construction, generated bases and quest rewards, in game
  - unverified: the despawn setting and the settings shortcut in game, in French and English
  - unverified: the StuffedFloors postfix, not exercisable while StuffedFloors is not installed
  - unverified: Preview palette (Measure-Palette.sh) not run
  - decision (2026-10-14, Virginie): renamed Patched to Renew, as PUBLISHING.md "Nommage" asks for a port of a dead mod; packageId, repository, name, README title and STATUS `mod` follow. Repository folder still named PerfectlyGenericItemPatched, to rename outside a session
  - unverified: Tests/Program.cs (local test project) is not declared in scripts/LOCAL_SCRIPTS.md (AUDIT.md 7.b: contact the Audit workflow session)
  - unverified: the shipped DLL was rebuilt on 2026-10-14 (SHA256 2141A0A7...CBE63); older hashes below describe past sources
session:      local_bbe6a63f-09d8-4a3c-9070-b9fafe66f132
social_preview_sha256: 17b5a4dba07b598a201465f7f376bd5d57d2151506517d28d3bc0c1def844714
protocols_read_sha: 43dd90d4b7873c500347fba1be01275a48d5795b
updated:      2026-10-15, protocols re-read
---

# Perfectly Generic Item Renew: status

## Audit from the start: 2026-10-14

Previous state (old vocabulary): `dansMonoRepo`. Retained: `writeTests[1.0.1]`.
Audited revision `0fc5b77` (main). Target version 1.0.1: the `v1.0.0` tag (2026-09-04)
predates every fix, nothing was ever released from it; reconcile before publication.

| Transition | Result |
| --- | --- |
| setupRepo -> makeSources | Validated: public repository with its own `.git` and origin, English README/ATTRIBUTION/CHANGELOG, `silent` licence documented, `(unofficial)` suffix and notice in About.xml and README. Front matter completed (`remote`, `workflow_stage`, `upstream_mod_remotes`, `published`). Duplicate search (1.a) not rerun: the original is known and credited. |
| makeSources -> code | Validated at file level: ModIcon.png 20 KB, Preview.png, ICO and sources present, `Art/.render/` ignored. Defect: `Art/echo.png` is not named `echo-source.png`. Not run: palette measure. Sources are the owner's and untouched. |
| code -> declareDependencies | Validated: no `feature` open; release build of the current sources equals the committed DLL (rebuilt and committed 2026-10-14, the previous one differed); `[Unreleased]` written in CHANGELOG. |
| declareDependencies -> settings | Validated in sources: `brrainz.harmony` and Core in `modDependencies`, `loadAfter`, `incompatibleWith` the upstream; StuffedFloors optional and type/method guarded; no LoadFolders. |
| settings -> localize | Validated: option, primary access, hidden MainButtons shortcut (`buttonVisible=false`, no forced visibility), 34 automated assertions green. `settings_audit` stays `partial`: in-game checks pending. |
| localize -> writeTests | Validated offline: `PGIMod.SettingsCategory` now translates `PGI_SettingsCategory`; EN/FR keys and DefInjected resolve (8 keys, 0 errors); FRENCH_REVIEW.md generated. Japanese keys are Ogliss' and not audited. |
| writeTests -> playTests | Done: scenarios, unit tests, XML checks, results, translations and French review, Pickle suite written (not played, justified per scenario in `Tests/Pickle/README.md`). Left: declare `Tests/Program.cs` in `scripts/LOCAL_SCRIPTS.md` (Audit workflow session). |

Closing-pass items done now: intermediates moved to `build/` (PUBLISHING.md; an earlier move to the
parent checkout was wrong), `*.dds` and `Art/.render/` ignored, `Art/*-original` committed then removed, `About.xml` description no longer names a RimWorld version.

## Checks run, 2026-10-14

- Console tests: 34 assertions, exit 0 (command in `Tests/RESULTS.md`).
- `Check-XmlFields`, `Check-DefRefs`, `Check-DefInjected`: all exit 0.
- Release build: 0 warnings, 0 errors, reproducible (two builds, same SHA256).
- `Check-Status.ps1`: 0 errors, 0 warnings (one INFO: social preview not due yet).
- Not run: RimWorld, Pickle, Preview palette.

## Settings audit, 2026-10-14

`settings_audit: partial` (MOD_SETTINGS.md, § 5). Revision `0fc5b77` plus the `build/` move.

- Inventory: one option, `removePGI` (checkbox, global, default false, serialized under
  `PGI_remove`). Effect: spawned generic items despawn on their next rare tick (up to 250 ticks),
  applied immediately to all saves, no restart. Turning it off does not restore removed items.
  `replaceFactions` is unused legacy serialized data, kept for compatibility, no control.
- Access: Options > Mods > Perfectly Generic Item Renew (primary). MainButtons shortcut
  `PGI_Settings`: `buttonVisible=false`, no override of `Visible`, opens the same
  `Dialog_ModSettings` for `PGIMod.Instance`. Neither customization mod nor RIMMSQOL tested.
- Automated (`Tests/RESULTS.md`, 34 assertions green): clean defaults, serialization round trip,
  despawn decision per setting state, hidden-by-default read from the def XML.
- Not verified: opening, effect, persistence over restart, reveal through RIMMSQOL, log
  robustness. All need a running game: `unverified` above, played at `playTests`.
- Not applicable: input validation (a checkbox has no bounds), optional dependencies (the
  StuffedFloors postfix has no setting), restore-defaults (single bool).

## Translation audit, 2026-10-14

`localization`, `translation_en`, `translation_fr`: `complete` (TRANSLATIONS.md), offline; in-game text checks (raw keys, clipping) stay `unverified` until `playTests`.

- Inventory, traced to source: `PGI_SettingsCategory`, `PGI_RemoveOption`,
  `PGI_RemoveOptionToolTip` (Keyed, all three used through `.Translate()` in `PGIMod.cs`);
  Def text of `PerfectlyGenericItem` (label, description, stuff adjective); MainButtonDef
  `PGI_Settings` label and description. No gizmo, letter, message or generated text in the code.
  Log strings are English by rule.
- English: Keyed and Def values in the sources. French lives in
  `Mod/Languages/French/Keyed/Keys.xml`, `DefInjected/ThingDef/PerfectlyGenericItem.xml` and
  `DefInjected/MainButtonDef/MainButtons.xml`. Japanese (Ogliss') is not audited.
- Checks: `Check-DefInjected.ps1` 8 keys, 0 errors; XML valid; no pawn text, so no gender
  agreement or plural keys apply.
- Doubt for the review: "Objet parfaitement générique Renew" keeps `Renew` untranslated,
  like the display name; tell me if the French name should differ.
- `FRENCH_REVIEW.md` regenerated 2026-10-14 from the clean tree at `0040531`.
- French review: Virginie, 2026-10-14, revision `0040531` (`FRENCH_REVIEW.md` at `cc571e0`), "Validé", no correction. "Objet parfaitement générique Renew" kept. English unchanged.

## Pending observations (not blockers)

- `Mod/Textures/PGI_single/*.dds` are game-generated caches, now ignored by `*.dds`; `Mod/About/PublishedFileId.txt` holds an id although no upload is recorded.
- `.claude/` at the repository root is session-owned and ignored.

## History

Older audits (2026-09-12, 2026-09-13, 2026-09-27) moved to `docs/runs/history.md`.

## Pickle plan (NPT answer, 2026-10-14)

- No step is needed to stage a save with a mod removed: one pass map stages the fixture mod M and
  saves ("the game is saved as {string}", then "the saved game {string} is handed to the mod
  {string}", SaveSteps `nelim.pickletools.savesteps`, not played yet); a second pass map without M
  loads it as `Pickle/Fixtures/<name>.rws` in this repository.
- Fixture mod M: ours to write and keep under `Tests/Pickle`, a tiny local mod with 2 or 3
  ThingDefs and a stuff, staged only in the making pass.
- Local steps to write here first (promoted by NPT only when a second mod needs them, on a request
  from the Ticket Manager): count the things of a defName on the map
  (`Find.CurrentMap.listerThings.ThingsOfDef`); assert the log holds no "could not load reference
  to ThingDef" (read through Pickle's own facility; "no errors were logged" exists upstream, to
  check in Rimworld-Pickle `Docs/steps.md`); wait one rare tick (S4).
- Correction 2026-10-15: Pickle's own catalogue (`Rimworld-Pickle` `Docs/steps.md`, WSL checkout
  `1af08c0`) already has the count, wait and log steps, so **no local C# step is needed**:
  `{int} {string} exist`, `a {string} exists`, `no {string} exists`, `I spawn a {string} at
  ({int}, {int})`, `a {string} is built at ({int}, {int})`, `I wait {int} ticks` (S4), `no errors
  were logged` (the missing-reference message is a Log.Error), `the save {string} is loaded`,
  `mod {string} is not loaded`. Only the fixture mod M and the fixture save remain to make.
- Written 2026-10-16: `Tests/Pickle/` (README, fixture item mod, companion, 3 features, 2 pass maps, despawn seed). Not played. NPT owes no ticket or commit; tell NPT when a step is played and shared by
  two mods.
