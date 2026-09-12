# Perfectly Generic Item Patched — notes de travail

Dépôt **autonome**, un seul mod. Il n'appartient plus au monorepo
`C:\Users\nelim\Documents\rimworld`, qui l'exclut par son `.gitignore` et n'en suit plus aucun
fichier. Un seul remote : `origin` → https://github.com/vbardales/Rimworld-Perfectly-Generic-Item-Patched,
**public**. La branche est `main`, pas `master` : la règle du `master` ne valait que pour le monorepo.

| | |
|---|---|
| Mod | Perfectly Generic Item Patched |
| packageId | `nelim.perfectlygenericitempatched` |
| Version cible | RimWorld 1.6, et elle seule |
| Auteur d'origine | Ogliss, pour MercuryDoll — [Workshop 2169353373](https://steamcommunity.com/sharedfiles/filedetails/?id=2169353373), resté en 1.4 |
| Licence de la source | aucune, nulle part — voir `ATTRIBUTION.md` |
| Contenu | une `ThingDef`, `PerfectlyGenericItem`, quatre patchs Harmony et une fenêtre de réglages |
| Dépendances | Harmony (`brrainz.harmony`) et Core, toutes deux déclarées. StuffedFloors est facultatif |

## Ce que fait le mod, en une phrase

Quand un mod disparaît d'une partie en cours, `BackCompatibility.BackCompatibleDefName` est appelé
pour chaque def que la sauvegarde réclame sans la trouver ; le postfix du mod répond
« PerfectlyGenericItem » et l'objet perdu revient en bloc vert au lieu de s'évaporer avec une erreur.
Tout le reste — l'objet comme matériau, les trois patchs qui l'excluent des menus, le réglage de
disparition automatique — n'existe que pour rendre ce remplacement supportable.

## Disposition

Le mod est dans **`Mod/`**, pas à la racine, et c'est `Mod/` que vise la jonction de
`RimWorld\Mods`. `SteamUGC.SetItemContent` envoie le dossier **entier**, sans exclusion possible :
ce qui ne doit pas partir sur le Workshop n'est pas dans `Mod/`, et il n'y a pas d'autre parade.

- `Mod/` — publié : `About/`, `Assemblies/`, `Defs/`, `Languages/`, `Textures/`
- `Source/` — jamais publié. `OutputPath` pointe vers `..\..\Mod\Assemblies\`
- `Art/` — jamais publié : les images pleine résolution dont la vitrine est découpée
- `STATUS.md`, `CLAUDE.md`, `README.md`, `CHANGELOG.md` — racine, donc hors du Workshop

`ATTRIBUTION.md` existe **en double**, à la racine et dans `Mod/`, volontairement : l'un sert le
dépôt, l'autre part avec le mod. Les deux doivent rester identiques
(`diff ATTRIBUTION.md Mod/ATTRIBUTION.md`). La copie de `Mod/` est celle que Steam envoie ; elle
s'était déjà désynchronisée une fois, d'une ligne du tableau des emprunts.

## La règle du portage

Le code et le dessin sont d'Ogliss. Ce dépôt ne contient que le passage en 1.6 et trois
corrections — c'est ce que dit le suffixe « Patched ».

- **Ne rien ajouter.** Aucune fonctionnalité nouvelle. Les seuls ajouts sont la traduction
  française, l'icône et la vitrine.
- **Le namespace, le nom d'assemblage `PerfectlyGenericItem` et le `defName` ne se renomment
  pas.** La def déclare `PerfectlyGenericItem.PGICompRemover` en `compClass`, RimWorld range les
  réglages sous le nom de la classe `Mod`, et le `defName` est ce que contiennent déjà les
  sauvegardes venues de la version d'Ogliss. Seuls le nom affiché et le `packageId` changent.
- **Les cibles des patchs se nomment par `nameof`**, jamais par chaîne. C'est exactement ce qui a
  tué le mod en 1.6 : `PatchAll` applique les quatre patchs d'un coup, une cible avait disparu, et
  l'exception a emporté le constructeur statique entier — donc aussi le patch qui est la raison
  d'être du mod. Avec `nameof`, la prochaine disparition casse la compilation au lieu du mod.
- **`PGISettings.replaceFactions` ne sert à rien**, et on le laisse : il est sauvegardé, aucun code
  ne le lit, aucune case ne l'affiche. C'est un reste d'Ogliss, inoffensif, et le supprimer
  changerait le fichier de réglages d'un joueur venu de sa version pour rien.
- **Le patch StuffedFloors reste facultatif** : le type est cherché d'abord, et on ne patche pas
  si le mod est absent. Il est posé en `postfix:` nommé, jamais en positionnel — le deuxième
  paramètre de `Harmony.Patch` est le préfixe, et c'est le défaut qui a fait qu'il n'a jamais rien
  retiré chez Ogliss.

## Construire

```
dotnet build Source/PerfectlyGenericItem/PerfectlyGenericItem.csproj -c Release
```

La DLL tombe directement dans `Mod/Assemblies/`. `Deterministic` est activé : une reconstruction
sans changement de source rend le **même fichier bit pour bit** — vérifié le 2026-09-12, md5
`36444a4f7abd5942c135016bcbd29297` avant et après. Donc une DLL qui change au build alors qu'on n'a
touché à rien est un signal, pas du bruit.

Les intermédiaires partent dans `..\.build\`, **hors du dossier**, par le `Directory.Build.props`.
Ce n'est pas du rangement : `obj/` contient l'`Assembly-CSharp.dll` publicisé du jeu, et le laisser
dans le dossier reviendrait à redistribuer le code de Ludeon sur le Workshop.

## Tester

**Ne jamais lancer RimWorld moi-même.** Préparer, puis lui demander de lancer et de coller le
journal : `C:\Users\nelim\AppData\LocalLow\Ludeon Studios\RimWorld by Ludeon Studios\Player.log`.

Le mod ne se teste pas en regardant un menu. Ce qu'il fait ne se produit qu'**au chargement d'une
sauvegarde à laquelle on a retiré un mod**, et une seule fois. Un scénario minimal : partie avec un
petit mod d'objets, poser quelques-uns de ses objets, sauvegarder, désactiver ce mod, recharger.
Les objets doivent revenir en blocs verts, et le journal ne doit plus porter de
« could not load reference to ThingDef ».

## Vérifier avant de commiter

Les quatre contrôleurs vivent dans le monorepo et s'appellent d'ici par chemin relatif. PowerShell
refuse les scripts sans `-ExecutionPolicy Bypass` sur cette machine :

```
powershell -NoProfile -ExecutionPolicy Bypass -File ..\scripts\Check-XmlFields.ps1 -ModPath Mod -ExtraAssemblies Mod\Assemblies\PerfectlyGenericItem.dll
powershell -NoProfile -ExecutionPolicy Bypass -File ..\scripts\Check-DefRefs.ps1 -ModPath Mod -Brief
powershell -NoProfile -ExecutionPolicy Bypass -File ..\scripts\Check-DefInjected.ps1 -TransMod Mod -ExtraAssemblies Mod\Assemblies\PerfectlyGenericItem.dll
```

Tous verts le 2026-09-11 : aucun champ XML inconnu, aucune référence de def introuvable, six clés
de traduction sur six valides. `Check-XmlFields` est celui qui compte ici — c'est lui qui attrape
un champ renommé, le genre de panne que RimWorld signale sur une ligne et oublie aussitôt.

## `STATUS.md` — à moi de le tenir

`STATUS.md` vit à la racine, jamais dans `Mod/`, pour que Steam ne le reçoive pas. Il est lu par un
balayage qui passe sur tous les mods, donc **le frontmatter est un format, pas de la prose** : ne
pas renommer de champ, ne pas en retirer, garder le vocabulaire des autres fiches.

C'est moi qui le tiens à jour, sans qu'on me le redemande. Il se met à jour **quand l'état change**,
pas à la fin de chaque échange : une étape franchie, un défaut trouvé ou corrigé, une image posée,
un essai en jeu, la publication. `updated:` prend la date et `2026-09-12, the mod's own session`.
Le champ `session:` ne se touche pas, il vient du balayage et nomme le groupe de sessions.

Le balayage écrit parfois des champs cassés — une clé avalée par la valeur voisine, une étiquette
française là où le vocabulaire est anglais. Les réparer au passage plutôt que les recopier.

## Reste à faire avant publication

Le mod est en `1.0.0`, **jamais envoyé sur le Workshop**. Le tag `v1.0.0` et sa release GitHub
existent depuis le 2026-09-04 et ne contiennent donc pas les corrections du 2026-09-12.

1. Trancher le tag : le déplacer sur l'état courant, ou couper un `1.0.1`. Personne n'a téléchargé
   la release et le mod n'est pas sur Steam, donc les deux se valent — mais cela se décide **avant**
   l'envoi.
2. Un essai en jeu, le premier. `tested_on` est vide et tout le `remaining` de `STATUS.md` en dépend.
3. L'envoi Workshop, puis `Mod/About/PublishedFileId.txt` renvoyé dans le dépôt.

La description Steam est **gelée au premier envoi** : `SetItemDescription` ne repasse jamais. Elle
est donc à relire une dernière fois avant, mot par mot. Elle affirmait un test en jeu qui n'a jamais
eu lieu ; la phrase est partie le 2026-09-12, avant qu'elle ne devienne définitive.

Les deux guides qui commandent ces étapes vivent hors du dépôt, dans le monorepo :
`..\STYLE_RIMWORLD.md` pour la vitrine et l'icône, `..\PUBLISHING.md` pour l'envoi.

## Langue

`About.xml`, `README.md`, `CHANGELOG.md`, `ATTRIBUTION.md`, `STATUS.md`, les commentaires du code et
les messages de commit sont **en anglais** : dépôt public, et un crédit en français ne peut pas être
lu par celui qu'il crédite. Ce fichier et nos échanges sont en français.
