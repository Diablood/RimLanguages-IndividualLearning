# Vérifications — version 1.0.0

## Première version stable

Publication 1.0.0 demandée par l'utilisateur le 2026-10-11. Gameplay identique à la 0.17.0 validée ; schéma 1 et données persistantes inchangés. Preview contrôlée visuellement après restauration des yeux et ajout du bandeau 1.6 : PNG 640 × 640, 862 346 octets, inférieur au plafond prudent de 1 000 000 octets. Aucun nouveau parcours moteur exécuté par l'agent.

package.ps1 exécuté le 2026-10-11 : reconstruction 1.0.0, 277 assertions hors jeu, XML, correspondance FR/EN, versions About/DLL et exclusion des DLL tierces réussis. Neuf fichiers du ZIP vérifiés par SHA256 ; preview incluse et poids contrôlé. Diff Source/Languages vide depuis la 0.17.0 ; aucune modification du comportement ou de l'état persistant. Aucun déploiement local, aucune exécution du moteur.

## Lot actuel : affichage individuel

Compilation 0.17.0 et 277 assertions hors jeu réussies. XML, clés FR/EN, versions et exclusion des DLL tierces vérifiés. Huit nouveaux contrôles portent sur les deux appels d'affichage collectif et la signature réelle de la ligne de faction. Utilisateur « Tests OK » le 2026-10-11 pour le parcours principal de liste des factions et conversation avec un prisonnier. Validation générale rapportée, variantes facultatives non détaillées séparément ; aucun moteur exécuté par l'agent. Ne pas redemander ce parcours sans nouvelle anomalie ou changement pertinent.

Déploiement 0.17.0 effectué jeu fermé : cinq fichiers vérifiés par SHA256, copie précédente sauvegardée sous work. Aucun nouveau test en jeu revendiqué.

Parcours principal validé, conservé comme référence :

1. Ouvrir la liste des factions : noms de langue/dialecte présents, aucun XP, pourcentage ou statut collectif « appris ». Survoler la ligne : renvoi à Langues ; le clic n'ouvre plus le diagramme collectif. Vérifier le placement, aussi avec la liste défilée.
2. Faire converser un colon avec un prisonnier encore admissible : progression personnelle dans Langues, sans texte de gain d'XP collective. En recrutement, les textes et la réduction de résistance ordinaires doivent rester présents ; la bulle sociale personnelle est conservée.

Pas de nouvelle migration ni de test jusqu'à 100 % requis pour ce lot. Variante facultative : sauvegarder/recharger avec niveaux distincts, vérifier l'absence de nouvelle erreur Harmony et la configuration initiale en nouvelle partie. Les parcours acquis ci-dessous restent acquis.

## Nature des preuves

Actualisation des métadonnées — 2026-10-11 : package.ps1 exécuté à nouveau, compilation et 277 assertions hors jeu réussies, huit fichiers du ZIP vérifiés. DLL identique à celle validée en jeu ; aucun déploiement ni moteur exécuté. Description About et URL GitHub actualisées, version 0.17.0 conservée.

Envoi GitHub 0.17.0 — 2026-10-11 : commit 758c2a9358c8a5e2d9d73371669727c076f15298 envoyé sur main ; git ls-remote confirme la même référence distante. Diff indexé sans erreur d'espacement ; aucun binaire, archive ou référence tierce ajouté. Aucun tag, release ou Workshop.

Préparation locale d'archive — 2026-10-11 : package.ps1 exécuté, reconstruction 0.17.0 et 277 assertions hors jeu réussies. Le ZIP contient exactement huit fichiers sous un seul dossier de mod ; chaque contenu décompressé est identique à sa source par SHA256. Aucune DLL du jeu, Harmony ou Cybranian incluse. DLL du complément identique à celle validée par utilisateur ; aucune exécution du moteur, aucun nouveau déploiement. Aucun résultat d'installation du ZIP dans le moteur revendiqué.

Les tests automatisés sont exécutés hors jeu. Ils couvrent le modèle de connaissances, les calculs et des transformations Harmony sur les instructions des assemblies installées, via Mono.Cecil. Ils ne prouvent pas l'exécution des patchs dans le moteur.

La compilation actuelle 0.17.0 a réussi avec **277 assertions hors jeu**, contrôles XML, correspondance FR/EN et versions About/DLL. Le déploiement local a vérifié les SHA256 des cinq fichiers copiés. Les tests utilisent KnowledgeTests.dll dans un processus PowerShell court ; KnowledgeTests.exe n'est plus produit ni exécuté.

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
- Confirmation supplémentaire du 2026-10-11 : arrivées naturelles de marchands et quêtes, interruption d'un soin ou jeu avec bébé. Moments d'interruption et rechargement au milieu de l'activité non détaillés séparément ; aucun moteur exécuté par l'agent.

Ces confirmations proviennent des retours utilisateur des 2026-10-10 et 2026-10-11. L'historique des modifications est dans [CHANGELOG.md](../CHANGELOG.md). Ne pas remettre ces parcours en attente sans changement pertinent ou nouvelle anomalie.

## Parcours court pour un futur lot

Après remplacement d'une DLL, redémarrer entièrement le jeu. Vérifier l'absence de nouvelles erreurs RLI, Harmony, C# ou XML dans le journal. Tester le comportement modifié avec deux pions aux niveaux distincts, puis sauvegarder/recharger si le changement touche un état persistant.

Pour un changement des enfants : un bébé avec une connaissance incomplète et un adulte Social actif maîtrisant une connaissance utile à 100 %. Comparer avant/après la fin du soin ou jeu, vérifier que l'activité vanilla fonctionne, puis comparer avec transmission décochée. Il n'est pas nécessaire d'attendre l'âge adulte ou 100 %.

Pour un changement d'apprentissage prisonnier : comparer un gardien débutant et un gardien complet, vérifier langue puis dialecte, et les autres modes indépendamment. Utiliser les outils développeur actuels ; les anciens gizmos +10 % n'existent plus.

## Variantes facultatives non confirmées séparément

Sélectionner les cas pertinents pour le changement ou une future publication :

- Soins/jeux : interruption validée globalement ; moments avant/après exposition non détaillés séparément, rechargement au milieu de l'activité, jouets et promenade distincts.
- Transmission : langue apprise plutôt que maternelle, dialecte après parent complet, Social désactivé, niveaux 0 et 20, absence de cible admissible, plafonnement et absence de gain aux tiers.
- Options : chargement des anciennes valeurs nulles, négatives, positives ou non finies, et priorité du nouveau booléen explicite.
- Prisonniers : contrôle du diagnostic NegotiationAbility désactivée, baisse du niveau pendant un travail, refus vanilla, collectif inconnu.
- Commerce : niveau abaissé après ouverture du menu, cadeaux, titres et replis sans acteur/données.
- Social : interactions naturelles, couple inversé, pions non joueurs et facteurs nuls.
- Migration : associations de langues absentes, cycles répétés et langues retirées du parent.
- Compatibilité avec des mods remplaçant interactions, naissance, soins ou bulles.

Les capsules de transport ont été explicitement écartées par l'utilisateur du parcours actuel : **non testées et non bloquantes**. Leur sérialisation a été inspectée, ce qui ne vaut pas un essai en jeu.

## Lire les résultats correctement

Les textes d'XP collective des conversations sont masqués depuis 0.17.0 ; comparer les niveaux dans Langues pour mesurer la progression personnelle. La transmission pendant le recrutement n'empêche pas la réduction de résistance ni ses textes vanilla.

Ideology actif ne suffit pas : les pensées linguistiques nécessitent un précepte LanguageLearning_* du parent. En mode classique sans ce précepte, leur absence est attendue.

Les bébés de scénario, événement ou debug ne passent pas nécessairement par le hook de naissance ; leur initialisation habituelle à 100 % ne teste pas le parcours nouveau-né à 0 %. Voir [CHILDREN.md](CHILDREN.md).

Les incidents anciens, notamment le PatchAll trop précoce de 0.15.0, restent décrits dans le changelog. La correction 0.15.1 et le parcours principal enfants sont acquis.

Contrôles du lot documentaire du 2026-10-11 : 24 documents décodés en UTF-8 strict, 81 liens locaux valides, absence de chemins personnels et vérification des exclusions nécessaires. Empreinte de la DLL 0.16.0 inchangée. Modèle de bug relu ; aucun rendu GitHub exécuté, aucun nouveau test du moteur.
