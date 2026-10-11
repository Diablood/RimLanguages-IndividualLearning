# Compilation et déploiement sur un autre poste

Prérequis : PowerShell, dotnet avec un SDK, RimWorld 1.6, Cybranian et Harmony installés. Les tests nécessitent également Mono.Cecil. Aucun téléchargement ni dépendance propriétaire redistribuée.

`./build.ps1` et `./test.ps1` détectent Steam via le registre Windows ou STEAM_DIR, lisent ses bibliothèques et cherchent le jeu et les deux mods. Le compilateur est celui du SDK le plus récent détecté par dotnet --list-sdks. Mono.Cecil est cherché dans le cache NuGet microsoft.net.illink.tasks ; une autre copie peut être fournie explicitement. La détection ne parcourt pas toutes les unités et ignore les bibliothèques inaccessibles.

Les chemins explicites ont priorité, puis les variables d'environnement, puis la détection. Variables : RIMWORLD_DIR, CYBRANIAN_DIR, HARMONY_ASSEMBLY, DOTNET_CSC et CECIL_ASSEMBLY. Une dépendance absente produit une erreur nommant le paramètre à fournir.

Exemple PowerShell, avec chemins à remplacer sur son poste :

```powershell
./build.ps1 -RimWorldDir '<RimWorld>' -CybranianDir '<Cybranian>' -HarmonyAssembly '<Harmony>/Current/Assemblies/0Harmony.dll' -Compiler '<SDK>/Roslyn/bincore/csc.dll'
./test.ps1 -RimWorldDir '<RimWorld>' -CybranianDir '<Cybranian>' -HarmonyAssembly '<Harmony>/Current/Assemblies/0Harmony.dll' -Compiler '<SDK>/Roslyn/bincore/csc.dll' -CecilAssembly '<Mono.Cecil.dll>'
./deploy.ps1 -RimWorldModsDir '<RimWorld>/Mods'
```

Sans paramètre, deploy.ps1 utilise Mods sous le jeu détecté. Il exige un dossier Mods existant et le jeu fermé. Il crée le complément pour une première installation ou vérifie son packageId avant remplacement. Il refuse un dossier existant sans métadonnées valides, vérifie la version et les chemins de destination, sauvegarde les anciens fichiers sous work puis compare les SHA256 des cinq fichiers copiés. Aucun autre mod modifié et aucune publication.

Sources et documents utilisent des chemins relatifs ou des placeholders ; les chemins réellement détectés ne sont affichés qu'à l'exécution. work et artifacts sont exclus du suivi Git.

## Préparer une archive installable

Exécuter ./package.ps1. Les paramètres RimWorldDir, CybranianDir, HarmonyAssembly, Compiler et CecilAssembly sont les mêmes que pour build/test ; détection automatique par défaut.

Le script reconstruit systématiquement puis exécute test.ps1. Il copie seulement huit fichiers autorisés, crée un ZIP sous artifacts/packages avec un dossier RimLanguages-IndividualLearning à sa racine, puis vérifie la liste exacte et chaque contenu décompressé par SHA256. Un fichier absent, un échec de compilation/test ou une différence de contenu interrompt la préparation.

Chaque exécution crée un chemin unique, sans écraser un paquet précédent. Les dossiers de staging restent sous artifacts/packages, exclus de Git. Aucun déploiement, tag, release ou envoi distant. Installation du ZIP : [INSTALLATION.md](../INSTALLATION.md).
