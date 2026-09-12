---
mod:          Perfectly Generic Item Patched
packageId:    nelim.perfectlygenericitempatched
repo:         Rimworld-Perfectly-Generic-Item-Patched
visibility:   public
detached:     yes
stage:        done
licence:      silent
licence_at:   no upstream LICENSE, a one-sentence Steam description
dependencies: declared
showcase:     complete
tested_on:
workshop:
remaining:
  - unverified: the missing-def replacement itself, which needs a save loaded with a mod taken out
  - unverified: the generic item standing in as the stuff of a wall or a garment whose material is gone
  - unverified: the three patches that keep it out of construction, generated bases and quest rewards
  - unverified: the despawn setting, and the French settings window it is read from
  - unverified: the StuffedFloors postfix, not exercisable while StuffedFloors is not installed
session:      local_e2e7811e-c9c1-4302-aa88-5765d6926970
updated:      2026-09-12, the mod's own session
---

# Perfectly Generic Item Patched — status

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
