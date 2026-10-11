# Vérifications — version 0.16.0

## Nature des preuves

Les tests automatisés sont exécutés hors jeu. Ils couvrent le modèle de connaissances, les calculs et des transformations Harmony sur les instructions des assemblies installées, via Mono.Cecil. Ils ne prouvent pas l'exécution des patchs dans le moteur.

La dernière compilation de 0.16.0 a réussi avec **269 assertions hors jeu**, contrôles XML, correspondance FR/EN et versions About/DLL. Le déploiement local a vérifié les SHA256 des cinq fichiers copiés. Les tests utilisent KnowledgeTests.dll dans un processus PowerShell court ; KnowledgeTests.exe n'est plus produit ni exécuté.

**Aucun moteur exécuté par l'agent.** Les validations en jeu ci-dessous sont rapportées par l'utilisateur. Un retour « Tests OK » valide globalement le parcours proposé, sans confirmer séparément toutes les variantes facultatives.

Le lot de consolidation documentaire n'a modifié ni le runtime ni les données : aucun nouveau build ou test en jeu requis pour ce lot. Ses contrôles portent sur la cohérence documentaire, les liens locaux, les chemins neutres et les exclusions Git.

Complément documentaire du 2026-10-11 : licence MIT ajoutée et choix GitHub consignés. 83 liens locaux vérifiés après ces ajouts ; aucun changement runtime ou dépôt créé.

## Parcours acquis

Premier envoi GitHub — 2026-10-11 : reconstruction complète 0.16.0 et 269 assertions hors jeu réussies, XML, FR/EN, versions et exclusion des DLL tierces vérifiés. 59 fichiers suivis et 73 liens locaux contrôlés, sans DLL, archive, sauvegarde ou chemin personnel. Aucun nouveau test en jeu ni déploiement. Deux fichiers de reprise redondants retirés ; consignes et état courant conservés.

Envoi confirmé : commit sources 89eeb0c identique sur GitHub/main et local ; 59 fichiers dans l'arbre distant. API GitHub : dépôt public, branche principale main. Commit initial de licence conservé et aucune divergence après envoi.

- Socle : onglet Langues, initialisation en nouvelle partie, indépendance des niveaux et sauvegarde/rechargement.
- Livres : progression du lecteur et conservation des données.
- Prisonniers : gains du gardien, passage de la langue au dialecte, persistance, arrêt des nouveaux travaux à langue et dialecte 100 %, reprise après baisse. Un geôlier complet suffit à vérifier la fin ; ne pas demander au second de terminer.
- Interface : affichage compact, réglage comme les hediffs, retrait des deux gizmos et ajout développeur individuel.
- 0.5.0 : parcours principal recrutement/résistance/conversion.
- 0.6.0 : parcours principal commerce, malus et communications.
- 0.7.0 : diagnostic des facteurs sociaux ; il ne prouve pas toutes les interactions naturelles.
- 0.9.0 : parcours principal Ideology après clarification du précepte linguistique requis.
- 0.10.0 : diagnostic des filtres de quêtes et marchands ; il ne génère pas d'événement.
- 0.11.0 : ajout à une partie ayant Cybranian, puis sauvegarde/rechargement.
- Transmission : gains automatiques, influence de Social et présentation dans la bulle sociale. Recrutement avec baisse de résistance et apprentissage simultané, apprentissage après recrutement et sauvegarde/rechargement confirmés.
- Caravane : conservation des connaissances confirmée.
- 0.14.0 : case d'activation, arrêt/reprise des gains et conservation du réglage après relance.
- 0.15.1 : disparition de l'erreur de chargement ; parcours principal naissance, nourrissage, persistance et éducation vanilla conservée.
- 0.16.0 : gain après jeu avec bébé, absence de gain lorsque la case est décochée, sauvegarde/rechargement.

Ces confirmations proviennent des retours utilisateur des 2026-10-10 et 2026-10-11. L'historique des modifications est dans [CHANGELOG.md](../CHANGELOG.md). Ne pas remettre ces parcours en attente sans changement pertinent ou nouvelle anomalie.

## Parcours court pour un futur lot

Après remplacement d'une DLL, redémarrer entièrement le jeu. Vérifier l'absence de nouvelles erreurs RLI, Harmony, C# ou XML dans le journal. Tester le comportement modifié avec deux pions aux niveaux distincts, puis sauvegarder/recharger si le changement touche un état persistant.

Pour un changement des enfants : un bébé avec une connaissance incomplète et un adulte Social actif maîtrisant une connaissance utile à 100 %. Comparer avant/après la fin du soin ou jeu, vérifier que l'activité vanilla fonctionne, puis comparer avec transmission décochée. Il n'est pas nécessaire d'attendre l'âge adulte ou 100 %.

Pour un changement d'apprentissage prisonnier : comparer un gardien débutant et un gardien complet, vérifier langue puis dialecte, et les autres modes indépendamment. Utiliser les outils développeur actuels ; les anciens gizmos +10 % n'existent plus.

## Variantes facultatives non confirmées séparément

Sélectionner les cas pertinents pour le changement ou une future publication :

- Soins/jeux : annulation avant exposition (aucun gain), interruption après exposition réelle (gain partiel), rechargement au milieu de l'activité ; jouets et promenade distincts.
- Transmission : langue apprise plutôt que maternelle, dialecte après parent complet, Social désactivé, niveaux 0 et 20, absence de cible admissible, plafonnement et absence de gain aux tiers.
- Options : chargement des anciennes valeurs nulles, négatives, positives ou non finies, et priorité du nouveau booléen explicite.
- Prisonniers : contrôle du diagnostic NegotiationAbility désactivée, baisse du niveau pendant un travail, refus vanilla, collectif inconnu.
- Commerce : niveau abaissé après ouverture du menu, cadeaux, titres et replis sans acteur/données.
- Social : interactions naturelles, couple inversé, pions non joueurs et facteurs nuls.
- Génération : arrivées naturelles de quêtes et marchands, au-delà du diagnostic validé.
- Migration : associations de langues absentes, cycles répétés et langues retirées du parent.
- Compatibilité avec des mods remplaçant interactions, naissance, soins ou bulles.

Les capsules de transport ont été explicitement écartées par l'utilisateur du parcours actuel : **non testées et non bloquantes**. Leur sérialisation a été inspectée, ce qui ne vaut pas un essai en jeu.

## Lire les résultats correctement

Les textes d'XP du parent peuvent suivre le collectif ; comparer les niveaux dans Langues pour mesurer la progression personnelle. La transmission pendant le recrutement n'empêche pas la réduction de résistance.

Ideology actif ne suffit pas : les pensées linguistiques nécessitent un précepte LanguageLearning_* du parent. En mode classique sans ce précepte, leur absence est attendue.

Les bébés de scénario, événement ou debug ne passent pas nécessairement par le hook de naissance ; leur initialisation habituelle à 100 % ne teste pas le parcours nouveau-né à 0 %. Voir [CHILDREN.md](CHILDREN.md).

Les incidents anciens, notamment le PatchAll trop précoce de 0.15.0, restent décrits dans le changelog. La correction 0.15.1 et le parcours principal enfants sont acquis.

Contrôles du lot documentaire du 2026-10-11 : 24 documents décodés en UTF-8 strict, 81 liens locaux valides, absence de chemins personnels et vérification des exclusions nécessaires. Empreinte de la DLL 0.16.0 inchangée. Modèle de bug relu ; aucun rendu GitHub exécuté, aucun nouveau test du moteur.
