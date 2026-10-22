# Préparation Workshop — copie auteur locale

Copie auteur actuelle : 1.1.2, identifiant 3817893569 restauré via deploy.ps1 -IncludePublishedFileId. Nouveaux arrivants sans faction validés en jeu par utilisateur. Finalisation commit/push/tag demandée ; l'utilisateur effectuera la mise à jour Steam. Descriptions FR/EN actualisées, [patchnotes 1.1.2](workshop/Patchnotes-1.1.2.txt) prêtes. Aucun envoi Steam par l'agent.

Lot 1.1.0 préparé le 2026-10-19 : écoute passive, transmission maternelle, test Mendez et sauvegarde/rechargement validés par utilisateur. La restriction des connaissances acquises est couverte par les tests ciblés et suivie pendant les parties ; aucune session prolongée dédiée demandée. Copie Mods déjà installée, DLL inchangée depuis le test. Descriptions FR/EN actualisées et [patchnotes 1.1.0](workshop/Patchnotes-1.1.0.txt) prêtes. Aucun envoi Steam pour ce lot par l'agent.

Mise à jour du 2026-10-14 : copie Mods prête avec About déclarant 1.6/1.7 et LoadFolders partagé. DLL 1.0.0 conservée, identifiant de l'article 3817893569 vérifié dans cette copie. Descriptions FR/EN actualisées et [patchnotes de compatibilité 1.7](workshop/Patchnotes-Compatibility-1.7.txt) prêtes. Aucun envoi Workshop effectué pour cette mise à jour ; preview avec bandeau 1.6 inchangée.

Article identifié le 2026-10-11 par le fichier fourni par l'utilisateur : **3817893569**, [page Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3817893569). Copie conservée dans About/PublishedFileId.txt du projet. L'utilisateur confirme avoir vérifié manuellement la présence du projet sur le Workshop. La page n'est pas accessible à l'outil web au moment du contrôle ; son contenu n'est pas vérifié séparément par l'agent.

L'utilisateur publiera depuis RimWorld. Copie actuelle : version 1.1.0 dans `<RimWorld>/Mods/RimLanguages-IndividualLearning`, sans sources, références de compilation ou fichiers de développement. Elle précède une éventuelle publication GitHub 1.1.0 ; la release GitHub actuelle reste v1.0.0-rw1.7. Preview incluse : PNG 640 × 640, 862 346 octets, yeux et bandeau 1.6.

## Textes à copier

Préparation de la copie auteur : ./deploy.ps1 -IncludePublishedFileId. Le fichier exact est About/PublishedFileId.txt ; il relie la copie locale à l'article existant 3817893569. L'option le restaure s'il manque, préserve un identifiant identique et refuse un identifiant différent. Aucun envoi automatique. Les installations de joueurs et forks restent sans copie automatique de cet identifiant ; package.ps1 l'exclut toujours des ZIP.

- Titre : **Rim Languages — Individual Learning**.
- [Description française](workshop/Description-FR.txt).
- [Description anglaise](workshop/Description-EN.txt).
- [Notes de première publication 1.0.0, FR/EN](workshop/Patchnotes-1.0.0.txt).
- [Notes de mise à jour 1.1.0, FR/EN](workshop/Patchnotes-1.1.0.txt).
- [Notes de mise à jour 1.1.2, FR/EN](workshop/Patchnotes-1.1.2.txt).

Les descriptions utilisent le BBCode Steam. Copier le contenu du fichier, sans ajouter de bloc de code Markdown. Pour une page internationale, utiliser l'anglais puis ajouter la version française en dessous, ou proposer les traductions séparément si l'interface le permet. Les drafts restent dans le projet, hors du dossier à envoyer.

## Après l'envoi depuis le jeu

Ajouter Harmony et Cybranian — Rim Languages aux éléments requis de la page Steam. Aucun DLC obligatoire ; Biotech et Ideology sont des fonctionnalités facultatives. Conserver les liens GitHub et le crédit au parent dans la description.

Vérifier le titre, la preview, la version 1.6 annoncée et les notes 1.0.0. Le fichier `About/PublishedFileId.txt` créé lors de l'envoi identifie l'article pour les mises à jour suivantes : le conserver, ne pas recréer un article à chaque version. Le déploiement ne remplace pas ce fichier.

Identifiant et lien consignés après réception du fichier utilisateur ; aucun article Workshop ni identifiant n'a été créé par l'agent. Les mises à jour doivent cibler ce même article. Pour un fork publié comme mod distinct, retirer l'identifiant d'origine de sa copie avant un premier envoi ; ne pas cibler l'article original.

## Vérifications effectuées

Jeu fermé, déploiement de neuf fichiers, sauvegarde de la copie précédente et comparaison SHA256 réussis le 2026-10-11. Vérification supplémentaire contre les contenus du ZIP GitHub, identiques. Syntaxe de deploy.ps1 vérifiée. Aucun nouveau moteur exécuté ni résultat de publication Steam revendiqué.
