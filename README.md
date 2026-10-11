# Rim Languages — Individual Learning

Complément de **Cybranian — Rim Languages** pour **RimWorld 1.6**. Version de développement locale : **0.16.0**.

Chaque pion possède ses propres connaissances linguistiques, conservées après sauvegarde, recrutement et déplacement en caravane. L'onglet **Langues** affiche ses langues et dialectes, leur origine maternelle et leurs niveaux.

## Fonctionnalités

- Lecture des livres de Cybranian et apprentissage auprès des prisonniers selon les niveaux du pion.
- Transmission pendant les discussions ordinaires : une connaissance maîtrisée à 100 % peut être enseignée automatiquement, sans nouvel ordre ni priorité sociale.
- Recrutement, résistance, conversion, commerce et communications selon le pion qui agit. Les facteurs sociaux tiennent compte des connaissances communes.
- Avec Biotech, langue et dialecte maternels à 0 % pour les nouveaux bébés issus du chemin de naissance pris en charge, puis apprentissage pendant le nourrissage et les jeux. L'éducation, les compétences, la croissance et les passions vanilla restent indépendantes.
- Avec Ideology et un précepte linguistique de Cybranian, événements et pensées associés à la progression personnelle.
- Outils développeur individuels : réglage précis comme pour les hediffs et ajout d'une langue ou d'un dialecte à un pion.

Les livres, langues, dialectes, configuration initiale, facteurs et préceptes du parent sont réutilisés. Le collectif continue de progresser pour la compatibilité ; ses acquis ne sont pas copiés sur tous les pions.

## Installation

RimWorld 1.6, Harmony et Cybranian sont nécessaires. Ordre relatif : **Harmony → Cybranian → Individual Learning**. Aucun DLC obligatoire ; les soins des bébés nécessitent Biotech, les pensées linguistiques nécessitent Ideology et le précepte approprié. Aucune dépendance à GateRim.

Installer le complément dans `<RimWorld>/Mods/RimLanguages-IndividualLearning` et redémarrer le jeu après remplacement de sa DLL. Les sources sont hébergées dans le [dépôt public GitHub](https://github.com/Diablood/RimLanguages-IndividualLearning). Cette version reste une livraison de développement, sans release ni Workshop.

Pour construire depuis les sources, voir [Compilation et dépendances](docs/BUILDING.md). La DLL du complément doit être compilée ; les DLL du jeu et des dépendances ne sont pas fournies.

## Options et sauvegardes

La **difficulté de Cybranian** est le seul réglage du rythme. La case **Activer la transmission par conversation** contrôle les gains automatiques du complément, y compris pendant les soins et jeux avec bébé. Elle ne désactive pas la lecture, l'ordre d'apprentissage prisonnier ou les gains collectifs du parent. Voir [Transmission](docs/TRANSMISSION.md) pour les facteurs et différences de calcul.

L'ajout à une partie ayant déjà Cybranian est pris en charge. Pour un pion sans registre, seules la langue et le dialecte de sa faction observée sont attribués à 100 % ; aucun autre savoir collectif n'est importé. L'origine historique d'un colon recruté avant l'installation ne peut pas être reconstituée. Faire l'essai sur une copie : [Migration des sauvegardes](docs/SAVE_MIGRATION.md).

## État et limites

Les parcours principaux ont été validés en jeu par l'utilisateur, notamment persistance, recrutement, caravane, transmission et enfants. La dernière compilation a passé **269 assertions hors jeu**. L'agent n'a pas exécuté le moteur ; ces résultats ne constituent pas une validation exhaustive de publication. Voir [Vérifications et cas facultatifs](docs/TESTING.md).

Les chemins sans données individuelles exploitables utilisent encore des replis collectifs. Les arrivées naturelles de quêtes et marchands, certaines interruptions de soins et la compatibilité avec d'autres mods restent à vérifier séparément. Les interprètes sont reportés à une éventuelle mise à jour.

## Licence

Code et documentation originaux sous [licence MIT](LICENSE). RimWorld, Cybranian et Harmony conservent leurs propres droits et ne sont pas redistribués avec ce projet.

## Documents

- [État actuel](docs/PROJECT_STATE.md), [Roadmap](docs/ROADMAP.md) et [Changelog](CHANGELOG.md).
- [Audit du parent installé](docs/AUDIT_CYBRANIAN.md), [Apprentissage](docs/LEARNING.md), [Prisonniers](docs/PRISONER_LEARNING.md) et [Enfants](docs/CHILDREN.md).
- [Prisonniers : interactions sociales](docs/PRISONER_SOCIAL.md), [Commerce et communications](docs/TRADE_COMMUNICATIONS.md), [Facteurs sociaux](docs/SOCIAL_INTERACTIONS.md), [Ideology](docs/IDEOLOGY_EVENTS.md) et [Quêtes](docs/QUEST_GENERATION.md).
- [Outils développeur](docs/DEVELOPER_TOOLS.md), [Contribution](CONTRIBUTING.md), [Livraison](docs/RELEASE.md) et [Préparation GitHub](docs/GITHUB_SETUP.md).
