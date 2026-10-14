# Run history

Folded out of STATUS.md on 2026-10-14. Old stage vocabulary; the earlier text stays in git.

### Audit — 2026-09-13

**Previous stage: done. Retained stage: dansMonoRepo.** The stage uses the literal
workflow labels supplied for this audit. This is a cumulative gate result, not a
claim that the repository has physically returned to the monorepo: `detached: yes`
remains true. The first transition fails its English documentation requirement.

Audited revision: `ab5c4598767fc39676858b849d79e42777d4861b` (main).
At audit start, the only tracked local change was three `unchecked` localization
fields in this file. Those fields were retained and updated from actual findings.
No source, distributed DLL, artwork or publication was changed. This audit changes
STATUS.md only; temporary compilation outputs were isolated and removed afterwards.
The previous narrative below is preserved as history, not current certification.

Repository: `C:\Users\nelim\Documents\rimworld\PerfectlyGenericItemPatched`.
Distributed content: its `Mod\` directory. Read `../AGENTS.md`, `../PUBLISHING.md`,
`../STYLE_RIMWORLD.md`, `../MOD_SETTINGS.md` and `../TRANSLATIONS.md`; the user's
explicit audit rules override conflicting runtime-gate wording in those documents.

### Ordered transition findings

| Transition | Finding |
| --- | --- |
| dansMonoRepo -> horsMonoRepo | **Defect found:** tracked `CLAUDE.md` is French technical documentation, including its explicit French-language exception. PUBLISHING requires English documentation even for working notes. **Validated independently:** autonomous `.git`, origin URL, public GitHub repository, remote HEAD equal to audited SHA, initialized status, coherent folder/repository/package identifiers, English README/ATTRIBUTION/CHANGELOG, identical attribution copies. `silent` is documented and supported by the installed upstream snapshot: versions 1.1-1.4 only, no LICENSE/COPYING and no permission text in its About description. No third-party license has been invented. This is a workflow classification, not redistribution permission; current Steam comments were not rechecked. |
| horsMonoRepo -> ModIcon generated | **Validated independently:** isolated Release build succeeds and matches the shipped DLL byte for byte; PNG is 128x128, 25,269 bytes and directly inspected. The port implementation is present, but the settings shortcut requirement below is an outstanding implementation defect, so this audit does not certify all development finished. The icon's tiny lettering also departs from the no-text style guidance; its size/format pass is separate. |
| ModIcon generated -> Preview generated | **Validated independently:** directly inspected `Mod/About/Preview.png`, PNG 896x504, 136,509 bytes, below 1 MB. It is a composed derivative of the credited upstream art; no historical generation report is required. No concrete camera defect was found in this isolated object image. |
| Preview generated -> preOptions | **Defects found:** About/README lack the exact public-`silent` `(unofficial)` suffix and opening notice; the About description ends with ADOPTION rather than `[url=https://github.com/vbardales/Rimworld-Perfectly-Generic-Item-Patched]Source code on GitHub[/url]`. Preview uses `Patched` at full title size and principal color, without the required secondary suffix treatment or unofficial tag. The amber rule contrasts with the green subject, but no secondary suffix color is actually provided. Description is otherwise English; no linking word requires reduction in this title. |
| preOptions -> options | **Defect found / partial:** useful despawn option and primary Mod settings overrides exist, but no MainButtonDef or shared settings shortcut implementation exists. Applicable automated effect/persistence tests have not been executed. In-game checks do not block this transition under the user's override. |
| options -> l10n | **Defect found / partial:** `PGIMod.SettingsCategory()` returns `Perfectly Generic Item` as hardcoded UI text. Existing two Keyed keys and three French DefInjected values pass resource checks independently. The settings gate remains partial. |
| l10n -> preTest | **Validated independently at source/metadata level:** Harmony and Core are used and declared with loadAfter; upstream package is incompatible; StuffedFloors is optional and type/method-guarded, not mandatory. No LoadFolders or conditional XML patches need reconciliation. Actual StuffedFloors compatibility has not been exercised. |
| preTest -> done | **Incomplete:** XML validators executed successfully below. No automated behavioral test project/suite exists in the inspected repository; meaningful logic is present, so no blanket non-applicability is justified. CLAUDE contains one missing-item save scenario with setup/actions/expectation, but not the full scenario coverage for material replacement, filters, settings, languages and optional integration. |
| done -> tested | **Not verified:** no in-game execution in this audit; existing history explicitly states none. FR/EN UI, logs, new game, existing-save replacement, options persistence and optional integrations remain unverified. RimWorld was not launched. |

### Executed checks and results

- `git status --short`, `git diff -- STATUS.md`, `git rev-parse --show-toplevel`,
  `git remote -v`, `git log -1`: repository identity and pre-existing local edits verified.
- `gh repo view vbardales/Rimworld-Perfectly-Generic-Item-Patched --json name,visibility,url,defaultBranchRef`
  returned PUBLIC/main; `git ls-remote origin HEAD` returned the audited commit.
  Initial sandbox access failed; the authorized read-only retry succeeded.
- Isolated build command, from the repository root:

  ```powershell
  dotnet build Source\PerfectlyGenericItem\PerfectlyGenericItem.csproj --no-restore -c Release -p:BaseIntermediateOutputPath=C:\Users\nelim\Documents\rimworld\PerfectlyGenericItemPatched\.audit\obj\ -p:MSBuildProjectExtensionsPath=C:\Users\nelim\Documents\rimworld\.build\obj\PerfectlyGenericItemPatched\ -p:OutputPath=C:\Users\nelim\Documents\rimworld\PerfectlyGenericItemPatched\.audit\bin\
  ```

  Initial sandbox SDK access failed; authorized retry succeeded with zero warnings/errors.
  Cached references: Krafs.Rimworld.Ref 1.6.4871, Lib.Harmony 2.4.2, net48 reference
  assemblies 1.0.3. Both rebuilt and distributed DLL SHA256:
  `B909BA0059EF1065F84C5ECDA963CC193A218EA42A628957988909B766D5380C`.
- All seven distributed XML documents parsed successfully.
- `powershell -NoProfile -ExecutionPolicy Bypass -File ..\scripts\Check-XmlFields.ps1 -ModPath Mod -ExtraAssemblies Mod\Assemblies\PerfectlyGenericItem.dll`:
  2 files checked, no unknown 1.6 fields, exit 0.
- `powershell -NoProfile -ExecutionPolicy Bypass -File ..\scripts\Check-DefRefs.ps1 -ModPath Mod -Brief`:
  one mod def, XML valid, references found and correctly typed, parents resolved, exit 0.
- `& ..\scripts\Check-DefInjected.ps1 -TransMod "$PWD\Mod"`:
  11,587 defs indexed, six keys checked (French and Japanese), zero errors, exit 0.
- PowerShell XML comparison: both owned Keyed entries present exactly once and nonempty
  in EN/FR. Manual inventory confirms both `.Translate()` uses. Neither has parameters,
  grammar tokens or rich-text tags needing matching. French nested stuff adjective targets
  `PerfectlyGenericItem.stuffProps.stuffAdjective`; label and description also resolve.
  Native English Def text provides the three English values without redundant injections.
- PNG dimensions/format measured with System.Drawing; both actual images visually inspected.
- Root and distributed ATTRIBUTION SHA256 both:
  `1A01A12612E55ADCEF1831047A6A6B12D5B87E09A4E601DB398C7A6659BF7792`.

### Settings audit

`settings_audit: partial`. `removePGI` is a useful global bool, default false,
serialized under `PGI_remove`, exposed as a checkbox via PGIMod. The component reads
it each rare tick and despawns only spawned parent things; no restart gate appears
in the source. Numeric bounds/invalid text input are not applicable to this checkbox.
`replaceFactions` is a serialized, unused legacy bool, not a relevant missing option.
There is no reason to invent a control for it. Primary access and serialization are
implemented but not proven by interactive tests. Missing MainButtons support is a
confirmed defect, distinct from unexecuted default/effect/persistence tests.
No RIMMSQOL, StuffedFloors or other customization integration was tested.

### Translation audit

`localization`, `translation_en`, `translation_fr`: partial. Existing resources pass
as detailed above; the settings-category UI string lacks a translation mechanism and
French coverage. English text exists but full translation-gate readiness cannot be
certified while the UI mechanism and settings gate remain incomplete. Technical log
strings and About metadata are excluded appropriately. FR/EN layout, clipping and
runtime key resolution were not tested; resource checks do not establish them.

### Next transition and separate follow-ups

To pass **dansMonoRepo -> horsMonoRepo**, translate the tracked technical documentation
`CLAUDE.md` to English, preserving its technical and historical content, then recheck
that gate. No repository recreation, remote restoration, new image or game launch is
needed for that transition. The suffix/notices and later defects remain tracked above
for their own transitions; clearing this first blocker does not certify those gates.

Optional/outside the next gate: improve the icon's tiny lettering; retain a reusable
palette/composition source when reworking the Preview. The absence of historical
image-generation evidence or a recorded camera comparison is not a blocker.
The local `v1.0.0` tag points to `45953976d25a791f0db0a69353050833a3e8784e`, not HEAD;
release reconciliation is a later publication concern. Remote release assets and
historical download counts were not checked. No commit, push or publication was made.

### Artwork refresh — 2026-09-27

The lower title ribbon, including all lettering, was removed from the ModIcon source
artwork. `Mod/About/ModIcon.png` was regenerated as a 128x128 PNG and
`Art/ModIcon.ico` as a 256x256 ICO from the refreshed source; both files were loaded
and dimension-checked locally. This visual correction does not change the retained
workflow stage or establish any in-game result.

### Historical status narrative — 2026-09-12 (superseded by audit above)

Status card, read by a pass over every mod rather than by asking each thread one at a time. It
lives at the root, never inside `Mod/`, so Steam never receives it.

The sweep filled these fields off the disk on 2026-09-12. The four it cannot read are settled
here, and one of them was wrong.

- **`stage`** — `done`. The port is finished, the three defects the "Patched" suffix names are
  fixed, the showcase is cut to size and the public repository holds the mod. Nothing waits on
  code. What is left is not this field's business: the run in game and the Workshop upload are
  read off `tested_on` and `workshop`, both empty.
- **`tested_on`** — empty, and exact. This mod has never been launched. Nothing below has ever
  been seen working.
- **`dependencies`** — `declared`, as of today, and it was **not** before. The About named Core
  alone, with Harmony carried by a `loadAfter` guarded by `IfModActive`. A `loadAfter` asks the
  player for nothing: it only orders mods that happen to be there. And RimWorld ships no Harmony
  of its own — there is no `0Harmony.dll` among the 106 assemblies of `Managed/` — while this mod
  deliberately does not bundle one. So a subscriber without the Harmony mod got a mod that loads
  and does nothing, which is the exact silent death 1.6 had already inflicted on it once.
  `brrainz.harmony` is now declared, as every other C# mod here declares it.
- **`remaining`** — no feature missing from a first release, no known defect left standing. What
  remains is unverified, and it is spelled out rather than left as the sweep's catch-all line.
  The replacement of missing defs comes first: it is the whole mod, it fires exactly once per
  save load, and its failure is indistinguishable from the errors it exists to absorb. The
  StuffedFloors line is last because it is the only one that cannot be checked at all from here.

Outside those categories, one thing is out of step. The `v1.0.0` tag and its GitHub release were
cut on 2026-09-04 and predate everything fixed today, so what the release holds is no longer what
`CHANGELOG.md` describes under that number. Nobody has downloaded it and the mod has never been
uploaded to Steam, so the choice is open: move the tag onto the current state, or cut `1.0.1`
before the Workshop upload. It has to be settled before that upload, not after.

The `remaining` categories: `feature` for something missing from the first pass, `defect` for a
known fault left unfixed, `unverified` for what could not be checked.

The `session` field was not touched: it comes from the sweep and names the session group, not
this conversation.

The `licence` vocabulary: `open` an explicit licence, `silent` no licence and a dead source,
`alive` no licence but a living source, `forbidden` a written refusal, `original` owing nothing
to anyone — not a name, not an idea traceable to one mod, not a value derived from its assets.
Ogliss' mod is `silent`: no `LICENSE` file, and a Steam description one sentence long that says
nothing about reuse. The credit and the removal-on-request promise stand in its place, and they
are in `About.xml`, `README.md` and `ATTRIBUTION.md` alike.
