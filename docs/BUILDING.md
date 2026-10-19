# Compilation et déploiement sur un autre poste

Prérequis : PowerShell, dotnet avec un SDK, RimWorld 1.6, Cybranian et Harmony installés. Les tests nécessitent également Mono.Cecil. Aucun téléchargement ni dépendance propriétaire redistribuée.

`./build.ps1` et `./test.ps1` détectent Steam via le registre Windows ou STEAM_DIR, lisent ses bibliothèques et cherchent le jeu et les deux mods. Harmony est recherché sous le dossier de la version du jeu lue dans Version.txt, puis Current pour les anciennes installations. Le compilateur est celui du SDK le plus récent détecté par dotnet --list-sdks. Mono.Cecil est cherché dans le cache NuGet microsoft.net.illink.tasks ; une autre copie peut être fournie explicitement. La détection ne parcourt pas toutes les unités et ignore les bibliothèques inaccessibles.

La prise en charge d'exécution de RimWorld 1.7 n'est pas une compilation ciblant son nouveau runtime. Ses assemblies sont à la racine, sous .NET 10/WaylithCLR, au lieu de RimWorldWin64_Data/Managed. Pour conserver la base compatible 1.6, build.ps1 accepte -ManagedReferencesDir '<références 1.6>/ref/net472', par exemple celles du paquet local krafs.rimworld.ref 1.6.4871, avec -HarmonyAssembly '<Harmony>/1.6/Assemblies/0Harmony.dll'. Ne jamais distribuer ces références dans le mod.

test.ps1 accepte le même ManagedReferencesDir et -RuntimeManagedDir '<assemblies d'exécution>'. Les assemblies de référence servent uniquement à compiler, pas à exécuter les tests. Dans l'installation 1.7 actuelle, -RuntimeManagedDir '<RimWorld>' -PolicyOnly permet d'exécuter les tests ciblés TransmissionPolicyTests ; ce mode est annoncé dans la sortie et ne remplace pas la suite complète. Celle-ci reste bloquée par UnityEngine.CoreModule absent du runtime 1.7. La compilation 1.1.0 et 18 assertions ciblées ont réussi ainsi ; ne pas annoncer les 277 assertions historiques comme réexécutées.

Les chemins explicites ont priorité, puis les variables d'environnement, puis la détection. Variables : RIMWORLD_DIR, CYBRANIAN_DIR, HARMONY_ASSEMBLY, DOTNET_CSC et CECIL_ASSEMBLY. Une dépendance absente produit une erreur nommant le paramètre à fournir.

Exemple PowerShell, avec chemins à remplacer sur son poste :

```powershell
./build.ps1 -RimWorldDir '<RimWorld>' -CybranianDir '<Cybranian>' -HarmonyAssembly '<Harmony>/Current/Assemblies/0Harmony.dll' -Compiler '<SDK>/Roslyn/bincore/csc.dll'
./test.ps1 -RimWorldDir '<RimWorld>' -CybranianDir '<Cybranian>' -HarmonyAssembly '<Harmony>/Current/Assemblies/0Harmony.dll' -Compiler '<SDK>/Roslyn/bincore/csc.dll' -CecilAssembly '<Mono.Cecil.dll>'
./deploy.ps1 -RimWorldModsDir '<RimWorld>/Mods'
```

Sans paramètre, deploy.ps1 utilise Mods sous le jeu détecté. Il exige un dossier Mods existant et le jeu fermé. Il crée le complément pour une première installation ou vérifie son packageId avant remplacement. Il refuse un dossier existant sans métadonnées valides, vérifie la version et les chemins de destination, sauvegarde les anciens fichiers sous work puis compare les SHA256 des neuf fichiers copiés : métadonnées, icône, preview, LoadFolders, DLL du complément, deux traductions, licence et notice. Un éventuel About/PublishedFileId.txt reste conservé. Aucun autre mod modifié et aucune publication.

Sources et documents utilisent des chemins relatifs ou des placeholders ; les chemins réellement détectés ne sont affichés qu'à l'exécution. work et artifacts sont exclus du suivi Git.

## Préparer une archive installable

Exécuter ./package.ps1. Les paramètres RimWorldDir, CybranianDir, HarmonyAssembly, Compiler et CecilAssembly sont les mêmes que pour build/test ; détection automatique par défaut.

Pour une mise à jour de métadonnées utilisant une DLL déjà validée et inchangée, sans les références Mono 1.6 disponibles : ./package.ps1 -VerifiedAssemblySha256 '<SHA256 de la DLL validée>' -PackageLabel 'rw1.7'. Ce mode exige l'empreinte explicite, vérifie les versions About/DLL, XML et clés FR/EN, puis les contenus de l'archive. Il ne reconstruit pas et n'exécute pas les tests de connaissances ; consigner les validations antérieures et le test utilisateur. Le mode par défaut continue de reconstruire et tester.

Le script reconstruit systématiquement puis exécute test.ps1. Il copie seulement huit fichiers autorisés, crée un ZIP sous artifacts/packages avec un dossier RimLanguages-IndividualLearning à sa racine, puis vérifie la liste exacte et chaque contenu décompressé par SHA256. Un fichier absent, un échec de compilation/test ou une différence de contenu interrompt la préparation.

Chaque exécution crée un chemin unique, sans écraser un paquet précédent. Les dossiers de staging restent sous artifacts/packages, exclus de Git. Aucun déploiement, tag, release ou envoi distant. Installation du ZIP : [INSTALLATION.md](../INSTALLATION.md).
