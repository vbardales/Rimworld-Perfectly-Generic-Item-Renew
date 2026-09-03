# Perfectly Generic Item Patched — attribution

Mise a jour en 1.6 de **Perfectly Generic Item**
(https://steamcommunity.com/sharedfiles/filedetails/?id=2169353373), par
**Ogliss**, ecrit pour MercuryDoll. Reste en 1.4.

`packageId` d'origine : `Ogliss.PerfectlyGenericItem`.

**La conception et le code sont d'Ogliss.** Ce depot n'ajoute aucune
fonctionnalite : il recompile l'assemblage contre la 1.6, remplace le seul point
d'accroche disparu, et corrige trois defauts — d'ou le suffixe « Patched »
plutot qu'un simple « 1.6 ».

## Licence

**Aucune licence declaree** : pas de fichier `LICENSE` dans le mod, et la
description Steam tient en une phrase, sans mention de reutilisation. La
republication suit donc l'usage etabli des reprises de mods abandonnes sur le
Workshop — credit explicite a l'auteur d'origine, lien vers le mod source, et
retrait immediat sur simple demande de sa part. C'est dit noir sur blanc dans
la description du mod, pas seulement ici.

## Ce qui a ete repris

| Element | Source |
| --- | --- |
| `PGIThingDefOf`, les quatre patchs Harmony, `PGICompRemover`, `PGIMod`, `PGISettings`, `Main` | `1.4/Assemblies/PerfectlyGenericItem.dll`, decompile |
| `Defs/PerfectlyGenericItem.xml` | racine du mod |
| Textures `PGI_single` | racine du mod |
| Clefs anglaises et japonaises | `Languages/` |

**L'espace de noms, le nom d'assemblage `PerfectlyGenericItem` et le `defName`
`PerfectlyGenericItem` sont conserves.** La def declare
`PerfectlyGenericItem.PGICompRemover` en `compClass`, RimWorld enregistre les
reglages sous le nom de la classe `Mod`, et le `defName` est ce que les
sauvegardes venant de la version d'Ogliss contiennent deja. Seuls le nom
affiche et le `packageId` changent, comme le veut la convention des reprises.

## Ce que la 1.6 a demande de changer

Le mod n'etait pas seulement signale en rouge : il etait **mort**. `PatchAll`
applique les quatre patchs en un seul appel, l'un d'eux visait une methode
disparue, et l'exception a emporte tout le constructeur statique :

```
Error in static constructor of PerfectlyGenericItem.Main:
  Undefined target method for patch method
  PGI_BaseGenUtility_IsCheapWallStuff_Patch::Postfix(ThingDef d, bool& __result)
```

Le patch sur `BackCompatibility.BackCompatibleDefName` — c'est-a-dire la
fonction meme du mod — ne s'appliquait donc jamais.

### `BaseGenUtility.IsCheapWallStuff` a disparu

La methode n'existe plus en 1.6. Elle servait a ecarter l'objet generique des
murs de bases generees. En 1.6, ce filtrage passe par
`GenStuff.AllowedStuffsFor(..., checkAllowedInStuffGeneration: true)`, qui teste
`stuffProps.allowedInStuffGeneration` :

```csharp
if (thingDef.IsStuff && ... && thingDef.stuffProps.CanMake(td))
{
    found = thingDef;
    if (!checkAllowedInStuffGeneration || thingDef.stuffProps.allowedInStuffGeneration)
```

Le patch a donc ete supprime et remplace par
`<allowedInStuffGeneration>false</allowedInStuffGeneration>` dans les
`stuffProps` de la def — le mecanisme que la 1.6 emploie a sa place, sans
Harmony. `RandomCheapWallStuff`, `TryRandomInexpensiveFloor` et
`BuildCostsAllowedInStuffGen` passent tous par la.

### `StuffProperties.soundImpactStuff` a ete renomme

Le champ s'appelle `soundImpactBullet` en 1.6. L'ancien nom produisait une
erreur XML a chaque chargement :

```
XML error: <soundImpactStuff>BulletImpact_Metal</soundImpactStuff> doesn't
correspond to any field in type StuffProperties.
```

### Le reste tient

Les trois autres cibles existent en 1.6 avec la meme signature, et sont
recompilees telles quelles. Chaque patch les designe desormais par `nameof`,
donc leur disparition future casserait la compilation au lieu de casser le mod
au chargement :

| Cible | Signature 1.6 |
| --- | --- |
| `StuffProperties.CanMake` | `public bool CanMake(BuildableDef t)` |
| `SketchGenUtility.IsStuffAllowed` | `public static bool IsStuffAllowed(ThingDef stuff, bool, Map, bool, ThingDef)` |
| `ThingSetMakerUtility.IsDerpAndDisallowed` | `public static bool IsDerpAndDisallowed(ThingDef thing, ThingDef stuff, QualityGenerator?)` |
| `BackCompatibility.BackCompatibleDefName` | `public static string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)` |

Les noms de parametres employes par les postfixes (`stuff`, `defType`,
`defName`) sont inchanges : Harmony les lie par nom, pas par position.

## Les defauts corriges

### 1. Les injections de traduction etaient traitees comme des objets perdus

`BackCompatibleDefName` sert a deux choses : relire une sauvegarde, et resoudre
les `DefInjected` d'une traduction. Le postfix d'origine ne faisait pas la
difference :

```csharp
if (GenDefDatabase.GetDefSilentFail(defType, defName, false) == null && defType == typeof(ThingDef))
{
    __result = "PerfectlyGenericItem";
}
```

Consequence : pour toute def absente citee par un fichier de traduction, le jeu
injectait ses libelles **dans l'objet generique**, qui se retrouvait renomme au
petit bonheur. Le parametre `forDefInjections` est desormais teste et les
injections sont laissees tranquilles.

### 2. Les conversions de compatibilite reussies etaient ecrasees

Le test portait sur `defName`, l'entree, jamais sur `__result`, la sortie. Quand
la chaine de compatibilite de RimWorld avait su renommer une ancienne def en une
def bien presente, le postfix repondait quand meme « objet generique » : un
objet recuperable etait perdu. Le postfix ne se declenche plus que si `__result`
ne resout rien non plus.

### 3. Le patch StuffedFloors etait enregistre en prefix

```csharp
harmony.Patch(methodInfo, new HarmonyMethod(method));
```

Le deuxieme parametre positionnel de `Harmony.Patch` est le **prefix**. La
methode passee est pourtant ecrite en postfix : elle lit `ref List<ThingDef>
__result`, qui n'existe pas encore avant l'appel de la methode d'origine. Le
retrait de l'objet generique de la liste des sols de StuffedFloors n'a donc
jamais eu lieu, et aurait leve une `NullReferenceException` si la liste avait
ete touchee. C'est desormais `postfix:`, nomme explicitement.

Au passage, `AccessTools.Method` etait appele meme quand StuffedFloors est
absent, ce qui journalise une erreur Harmony sur un type nul. Le type est
teste avant.

## Ajout

`Languages/French/` — clefs des reglages et libelles de l'objet. Rien d'autre
n'a ete ajoute.
