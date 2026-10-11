# Livraison et publication

## Versionnement

Source unique : modVersion dans [About/About.xml](../About/About.xml), actuellement **1.0.0**. build.ps1 génère AssemblyVersion et AssemblyFileVersion **1.0.0.0**, AssemblyInformationalVersion **1.0.0**. test.ps1 refuse une divergence.

Selon la demande utilisateur, chaque lot testable modifiant le contenu reçoit un numéro distinct : correctif pour une correction (prochain 1.0.1), mineur pour une fonctionnalité (prochain 1.1.0). Une modification documentaire seule ou la copie d'un contenu identique ne change pas la version. Le schéma de sauvegarde est distinct : **1**.

RimSort affiche la version d'About ; sa détection automatique a été confirmée par utilisateur. Un numéro local ne constitue ni un tag ni une publication.

## Livraison locale

1. Relire l'état, les consignes et les vérifications du sous-système modifié.
2. Mettre à jour la version pour le lot testable et le changelog ; préserver les clés de sauvegarde ou documenter la migration.
3. Exécuter build.ps1 puis test.ps1 ; vérifier XML, traductions et versions.
4. Fermer RimWorld, puis exécuter deploy.ps1 : sauvegarde des cinq fichiers précédents sous work, contrôle d'identité, copie et comparaison SHA256.
5. Redémarrer le jeu ; vérifier le parcours modifié et la persistance, puis consigner précisément les résultats.

Voir [BUILDING.md](BUILDING.md). Build et tests ne déploient pas ; le déploiement local ne publie rien. Les anciens scripts sous work sont historiques.

## Paquet local préparé

Paquet 1.0.0 reconstruit le 2026-10-11 : 277 assertions hors jeu et neuf fichiers d'archive vérifiés, preview incluse et sous 1 000 000 octets. Code et traductions inchangés depuis la 0.17.0, version DLL 1.0.0.0. Notes FR/EN dans [RELEASE_NOTES_1.0.0.md](RELEASE_NOTES_1.0.0.md). Aucun déploiement local de cette version.

./package.ps1 reconstruit, exécute les tests et crée une archive installable contrôlée sous artifacts/packages. Contenu autorisé : About/About.xml, About/ModIcon.png, About/Preview.png, LoadFolders.xml, la DLL propre du complément, les deux traductions, LICENSE et INSTALLATION.md. Chaque fichier du ZIP est comparé par SHA256 à sa source ; aucun MANIFEST ajouté. Les archives précédant l'ajout de la preview contenaient huit fichiers ; les nouveaux paquets en contiennent neuf.

Préparation 0.17.0 exécutée le 2026-10-11 : 277 assertions hors jeu et huit contenus d'archive vérifiés. DLL identique à la copie installée et validée par utilisateur. Pas de nouveau test en jeu requis pour cet empaquetage, aucun nouveau déploiement. Une archive locale ne constitue pas une release publiée.

## Publication externe

[Release GitHub 1.0.0](https://github.com/Diablood/RimLanguages-IndividualLearning/releases/tag/v1.0.0) publiée le 2026-10-11, tag annoté v1.0.0 sur ecf60dd. ZIP installable téléchargé après publication et comparé par SHA256 au paquet local, identique ; état stable confirmé par API. Workshop non publié et aucun PublishedFileId défini.

La première version stable 1.0.0 est demandée par l'utilisateur. Parcours principaux jusqu'à 0.17.0 confirmés par utilisateur, y compris arrivées naturelles de marchands/quêtes et interruption de soin ou jeu avec bébé ; comportements inchangés en 1.0.0, variantes et limites dans [TESTING.md](TESTING.md). Cette publication n'annonce pas une compatibilité exhaustive avec d'autres mods.

La création d'un dépôt est distincte d'une release. Nom retenu : RimLanguages-IndividualLearning, visibilité publique, licence MIT pour le code et la documentation originaux. Avant une publication explicitement demandée : vérifier attributions et provenance des assets, examiner les fichiers suivis, reconstruire, effectuer les contrôles et tests nécessaires au périmètre annoncé, puis vérifier le paquet final et les références distantes.

Distribuer uniquement About, LoadFolders.xml, Languages et la DLL propre au complément dans 1.6/Assemblies. Ne pas redistribuer les DLL du jeu, Harmony ou Cybranian, ni références de compilation, sauvegardes, journaux, archives locales ou fichiers *_MANIFEST.txt.

Aucun PublishedFileId défini. Aucun tag, release GitHub ou Workshop à créer sans instruction explicite. Préparation du dépôt : [GITHUB_SETUP.md](GITHUB_SETUP.md).

Une mise à jour des sources GitHub est un commit suivi d'un push. Une publication technique est une étape distincte : créer un tag et une release GitHub avec le ZIP installable, ou créer/envoyer l'article Steam Workshop et enregistrer son PublishedFileId. La demande du 2026-10-11 autorise les métadonnées et la mise à jour des sources GitHub, sans demander ces publications.

Demande ultérieure du 2026-10-11 : version finale 1.0.0 et tag GitHub autorisés, après création de la preview. Visuel original exporté dans About/Preview.png à 640 × 640, 862 346 octets. L'original haute résolution reste sous artifacts/preview. Plafond prudent pour les exports : inférieur à 1 000 000 octets ; Steam indique moins de 1 MB pour les previews supplémentaires dans [ISteamUGC](https://partner.steamgames.com/doc/api/isteamugc#AddItemPreviewFile). Workshop à traiter étape par étape séparément.
