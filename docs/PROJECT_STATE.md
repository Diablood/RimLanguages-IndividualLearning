# État du projet — 2026-10-11

## Version actuelle

**0.17.0**, DLL **0.17.0.0**, schéma individuel **1**. Dernier lot : affichage des factions sans progression collective et retrait des textes d'XP collective des conversations. Compilation et **277 assertions hors jeu** réussies. Utilisateur « Tests OK » le 2026-10-11 pour le parcours principal proposé : liste des factions et conversation avec un prisonnier. Variantes facultatives non confirmées séparément ; aucun moteur exécuté par l'agent. Détails dans [COLLECTIVE_DISPLAY.md](COLLECTIVE_DISPLAY.md).

Installée jeu fermé le 2026-10-11 : cinq fichiers vérifiés par SHA256, sauvegarde précédente sous work/runtime-before-0.17.0-20261011-161011-896. DLL livrée : 72BCBFEB77F71A0877768CBB2FACC96C2FDEF67FA4AA0F92B22B4CEABB331BCB. Aucun test en jeu exécuté.

Le parcours principal 0.16.0 a reçu « Tests OK » utilisateur : gain après jeu, transmission désactivée et sauvegarde/rechargement. Les variantes facultatives ne sont pas confirmées séparément. Aucun moteur exécuté par l'agent.

Sources 0.17.0, métadonnées et empaquetage envoyés sur GitHub le 2026-10-11 à la demande utilisateur : commit 758c2a9358c8a5e2d9d73371669727c076f15298, référence distante main vérifiée identique par git ls-remote. Aucun tag, release ou Workshop créé. Aucun moteur exécuté par l'agent.

Dernière consolidation du 2026-10-11 : ajout de package.ps1 et d'une notice INSTALLATION.md FR/EN, préparation d'une archive locale installable 0.17.0. Reconstruction et 277 assertions hors jeu réussies ; huit fichiers du ZIP vérifiés par SHA256. DLL inchangée par rapport à l'installation validée. Version 0.17.0 conservée, aucun nouveau déploiement ni test en jeu. Archive et staging sous artifacts/packages, exclus de Git ; aucune release distante.

Archive locale actuelle après actualisation des métadonnées : RimLanguages-IndividualLearning-0.17.0-20261011-162953-7eb54b98.zip (43 460 octets). SHA256 : B70C6234E36673F124E89063486004ED36D6215C24ECF26FEDD4194A7384C6CC. Reconstruction et 277 assertions hors jeu réussies ; huit contenus d'archive vérifiés, DLL inchangée. Aucun nouveau déploiement.

## Fonctionnalités intégrées

- Registres persistants par pion, origine maternelle et onglet Langues compact.
- Lecture individuelle des livres d'origine ; langue puis dialecte, plafonnés à 100 %.
- Ordre prisonnier original adapté au besoin du geôlier ; arrêt des nouveaux travaux une fois les deux niveaux complets.
- Contrôles du geôlier et du négociateur réels ; facteurs sociaux d'après les connaissances communes.
- Événements Ideology personnels utilisant les préceptes et pensées du parent.
- Filtres de quêtes et visites marchandes différant le contrôle linguistique vers les actions individuelles.
- Transmission de connaissances maîtrisées lors de Chitchat/DeepTalk, sans modifier leur fréquence ou leur priorité. Social requis chez les deux pions, vitesse selon leurs aptitudes ; pas de facteur Intellectuel supplémentaire.
- Une seule difficulté, celle du parent ; case d'activation du complément.
- Avec Biotech, identité maternelle à 0 % à la naissance prise en charge, apprentissage après nourrissage et jeu, éducation vanilla conservée.
- Édition développeur précise et ajout individuel, sans les deux anciens gizmos.

## Conservé du parent et encore collectif

Livres, Defs, langues/dialectes, écran initial, paramètres, facteurs et préceptes réutilisés. La XP collective et les utilitaires du parent restent actifs pour la compatibilité et les replis. Le diagramme n'est pas supprimé, mais son accès par la ligne de faction et les niveaux affichés dans cette ligne sont retirés en 0.17.0. L'option autoLearnRecruiteeLanguage garde son effet collectif ; elle ne distribue aucun savoir personnel.

Les contrôles individuels utilisent le comportement original si le pion, le registre ou la langue cible ne sont pas exploitables. Les menus sans acteur reportent le contrôle lorsqu'une langue valide est résolue ; les restrictions vanilla restent appliquées.

La lecture réutilise le calcul du parent, dont la difficulté intervient différemment de celle des conversations. Ne pas annoncer un ralentissement uniforme de tous les apprentissages en augmentant cette jauge.

## Validations acquises et limites

La synthèse des confirmations utilisateur et des vérifications hors jeu se trouve dans [TESTING.md](TESTING.md). Ne pas remettre les parcours acquis en attente : socle, livres, progression prisonnier et dialecte, fin/reprise des leçons, interface, ajout développeur, contrôles individuels, migration, transmission, recrutement/rechargement, caravane et parcours principal enfants.

Le 2026-10-11, l'utilisateur a validé les arrivées naturelles de marchands et quêtes ainsi que l'interruption d'un soin ou jeu avec bébé. Cette confirmation ne détaille pas séparément chaque moment d'interruption ni le rechargement au milieu de l'activité. Certains replis et les interactions avec d'autres mods restent distincts. Les capsules de transport ont été explicitement écartées du parcours actuel : non testées et non bloquantes.

## Décisions et suite

Le 2026-10-11, l'utilisateur confirme la conservation de la difficulté et des calculs de Cybranian, y compris la différence entre lecture et conversation. Métadonnées About actualisées : fonctionnalités actuelles, DLC facultatifs, migration et URL GitHub. Version 0.17.0 conservée : aucune modification du code, des traductions ou des données persistantes dans cette consolidation.

Les interprètes sont reportés à une éventuelle mise à jour ; leur proposition n'a pas été retenue pour implémentation. Une API publique pour GateRim est facultative et doit être cadrée séparément, sans dépendance en sens inverse.

Dépôt public fourni par utilisateur : [Diablood/RimLanguages-IndividualLearning](https://github.com/Diablood/RimLanguages-IndividualLearning). Dépôt local initialisé, origin configuré et branche main rattachée au commit initial distant 09f6b13. Licence MIT distante conservée, y compris sa notice de copyright. Premier envoi du projet ; aucune release, aucun tag ou Workshop.

Envoi des sources confirmé le 2026-10-11 : commit 89eeb0c57d8681fb9372a62afef06bd9864af8a6 sur main, référence distante identique. 59 fichiers présents ; anciens fichiers de reprise et DLL absents. L'API GitHub confirme visibility=public, private=false et default_branch=main.

Les fichiers CONTINUER_SUR_CODEX.md et CODEX_START.md sont retirés à la demande utilisateur : doublons de reprise. AGENTS.md, l'état courant et la roadmap restent les points d'entrée du développement.

Voir [ROADMAP.md](ROADMAP.md) et [GITHUB_SETUP.md](GITHUB_SETUP.md).

## Reprise et preuves

Relire AGENTS.md, README.md, ce fichier, la roadmap et les documents concernés à chaque reprise ; appliquer les vrais skills rimworld-mod-developer et rimworld-release-qa. Mettre à jour les documents après chaque lot, sans empiler des états actuels contradictoires.

L'audit de la DLL Cybranian est dans [AUDIT_CYBRANIAN.md](AUDIT_CYBRANIAN.md). Historique des lots dans [CHANGELOG.md](../CHANGELOG.md). Les copies antérieures et journaux locaux restent sous work, exclu du suivi Git.

Empreinte de la DLL du complément livrée en 0.16.0 :
`F2473A5D73489C28C8E66A0719B191982BF536AF1652A212F4AD59BD180689C5`.

Contrôles du lot documentaire du 2026-10-11 : 24 documents décodés en UTF-8 strict, 81 liens locaux valides, absence de chemins personnels et vérification des exclusions nécessaires. Empreinte de la DLL 0.16.0 inchangée. Modèle de bug relu ; aucun rendu GitHub exécuté, aucun nouveau test du moteur.
