# Livraison et publication

## Versionnement

Source unique : modVersion dans [About/About.xml](../About/About.xml), actuellement **0.17.0**. build.ps1 génère AssemblyVersion et AssemblyFileVersion **0.17.0.0**, AssemblyInformationalVersion **0.17.0**. test.ps1 refuse une divergence.

Selon la demande utilisateur, chaque lot testable modifiant le contenu reçoit un numéro distinct : correctif pour une correction (prochain 0.17.1), mineur pour une fonctionnalité (prochain 0.18.0). Une modification documentaire seule ou la copie d'un contenu identique ne change pas la version. Le schéma de sauvegarde est distinct : **1**.

RimSort affiche la version d'About ; sa détection automatique a été confirmée par utilisateur. Un numéro local ne constitue ni un tag ni une publication.

## Livraison locale

1. Relire l'état, les consignes et les vérifications du sous-système modifié.
2. Mettre à jour la version pour le lot testable et le changelog ; préserver les clés de sauvegarde ou documenter la migration.
3. Exécuter build.ps1 puis test.ps1 ; vérifier XML, traductions et versions.
4. Fermer RimWorld, puis exécuter deploy.ps1 : sauvegarde des cinq fichiers précédents sous work, contrôle d'identité, copie et comparaison SHA256.
5. Redémarrer le jeu ; vérifier le parcours modifié et la persistance, puis consigner précisément les résultats.

Voir [BUILDING.md](BUILDING.md). Build et tests ne déploient pas ; le déploiement local ne publie rien. Les anciens scripts sous work sont historiques.

## Paquet local préparé

./package.ps1 reconstruit, exécute les tests et crée une archive installable contrôlée sous artifacts/packages. Contenu autorisé : About/About.xml, About/ModIcon.png, LoadFolders.xml, la DLL propre du complément, les deux traductions, LICENSE et INSTALLATION.md. Chaque fichier du ZIP est comparé par SHA256 à sa source ; aucun MANIFEST ajouté.

Préparation 0.17.0 exécutée le 2026-10-11 : 277 assertions hors jeu et huit contenus d'archive vérifiés. DLL identique à la copie installée et validée par utilisateur. Pas de nouveau test en jeu requis pour cet empaquetage, aucun nouveau déploiement. Une archive locale ne constitue pas une release publiée.

## Publication externe

La version 0.17.0 reste une version de développement locale. Parcours principaux jusqu'à 0.17.0 confirmés par utilisateur ; variantes et limites dans [TESTING.md](TESTING.md), sans validation exhaustive de publication.

La création d'un dépôt est distincte d'une release. Nom retenu : RimLanguages-IndividualLearning, visibilité publique, licence MIT pour le code et la documentation originaux. Avant une publication explicitement demandée : vérifier attributions et provenance des assets, examiner les fichiers suivis, reconstruire, effectuer les contrôles et tests nécessaires au périmètre annoncé, puis vérifier le paquet final et les références distantes.

Distribuer uniquement About, LoadFolders.xml, Languages et la DLL propre au complément dans 1.6/Assemblies. Ne pas redistribuer les DLL du jeu, Harmony ou Cybranian, ni références de compilation, sauvegardes, journaux, archives locales ou fichiers *_MANIFEST.txt.

Aucun PublishedFileId défini. Aucun tag, release GitHub ou Workshop à créer sans instruction explicite. Préparation du dépôt : [GITHUB_SETUP.md](GITHUB_SETUP.md).

Une mise à jour des sources GitHub est un commit suivi d'un push. Une publication technique est une étape distincte : créer un tag et une release GitHub avec le ZIP installable, ou créer/envoyer l'article Steam Workshop et enregistrer son PublishedFileId. La demande du 2026-10-11 autorise les métadonnées et la mise à jour des sources GitHub, sans demander ces publications.
