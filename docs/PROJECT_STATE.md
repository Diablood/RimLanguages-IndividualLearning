# État du projet — 2026-10-11

## Version actuelle

**0.16.0**, DLL **0.16.0.0**, schéma individuel **1**. Dernier lot fonctionnel : transmission pendant les jeux vanilla avec bébé. Installé localement, cinq fichiers contrôlés par SHA256 ; dernière compilation et **269 assertions hors jeu** réussies.

Le parcours principal 0.16.0 a reçu « Tests OK » utilisateur : gain après jeu, transmission désactivée et sauvegarde/rechargement. Les variantes facultatives ne sont pas confirmées séparément. Aucun moteur exécuté par l'agent.

Dernier lot : rattachement au dépôt GitHub fourni par l'utilisateur et premier envoi des sources terminé. Reconstruction 0.16.0 et 269 assertions hors jeu réussies le 2026-10-11 ; XML, FR/EN et versions cohérents. Aucun changement de code, numéro, réglage ou installation ; aucun nouveau test en jeu.

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

Livres, Defs, langues/dialectes, écran initial, paramètres, facteurs et préceptes réutilisés. La XP collective, le diagramme de faction et les utilitaires du parent restent actifs pour la compatibilité et les replis. L'option autoLearnRecruiteeLanguage garde son effet collectif ; elle ne distribue aucun savoir personnel.

Les contrôles individuels utilisent le comportement original si le pion, le registre ou la langue cible ne sont pas exploitables. Les menus sans acteur reportent le contrôle lorsqu'une langue valide est résolue ; les restrictions vanilla restent appliquées.

La lecture réutilise le calcul du parent, dont la difficulté intervient différemment de celle des conversations. Ne pas annoncer un ralentissement uniforme de tous les apprentissages en augmentant cette jauge.

## Validations acquises et limites

La synthèse des confirmations utilisateur et des vérifications hors jeu se trouve dans [TESTING.md](TESTING.md). Ne pas remettre les parcours acquis en attente : socle, livres, progression prisonnier et dialecte, fin/reprise des leçons, interface, ajout développeur, contrôles individuels, migration, transmission, recrutement/rechargement, caravane et parcours principal enfants.

Les diagnostics sociaux et de génération ne prouvent pas l'exécution de tous les événements naturels. Les variantes d'interruption, certains replis et les interactions avec d'autres mods restent distincts. Les capsules de transport ont été explicitement écartées du parcours actuel : non testées et non bloquantes.

## Décisions et suite

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
