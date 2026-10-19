# Rim Languages — Individual Learning

Complément de **Cybranian — Rim Languages** pour **RimWorld 1.6 et 1.7**. Version des sources : **1.1.0**, tag **v1.1.0** ; parcours Mendez et persistance validés en jeu par l'utilisateur, autres contrôles dans [TESTING.md](docs/TESTING.md). Dernière release avec paquet téléchargeable : **v1.0.0-rw1.7**. La branche 1.7 instable peut nécessiter de nouvelles adaptations.

Chaque pion possède ses propres connaissances linguistiques, conservées après sauvegarde, recrutement et déplacement en caravane. L'onglet **Langues** affiche ses langues et dialectes, leur origine maternelle et leurs niveaux.

## Fonctionnalités

- Lecture des livres de Cybranian et apprentissage auprès des prisonniers selon les niveaux du pion.
- Transmission pendant les discussions ordinaires, sans nouvel ordre ni priorité sociale : au sein d'une même colonie, seules les langues et dialectes maternels de l'enseignant maîtrisés à 100 % sont transmis. Les connaissances acquises s'apprennent par livres ou autres méthodes explicites. Social actif reste requis pour enseigner ; un pion dont Social est désactivé peut apprendre par écoute à vitesse réduite.
- Recrutement, résistance, conversion, commerce et communications selon le pion qui agit. Les facteurs sociaux tiennent compte des connaissances communes.
- Avec Biotech, langue et dialecte maternels à 0 % pour les nouveaux bébés issus du chemin de naissance pris en charge, puis apprentissage pendant le nourrissage et les jeux. L'éducation, les compétences, la croissance et les passions vanilla restent indépendantes.
- Avec Ideology et un précepte linguistique de Cybranian, événements et pensées associés à la progression personnelle.
- Outils développeur individuels : réglage précis comme pour les hediffs et ajout d'une langue ou d'un dialecte à un pion.

Les livres, langues, dialectes, configuration initiale, facteurs et préceptes du parent sont réutilisés. Le collectif continue de progresser pour la compatibilité ; ses acquis ne sont pas copiés sur tous les pions.

Dans la liste des factions, seuls les noms de langue et dialecte sont affichés, sans niveau collectif. Les textes d'XP collective des conversations sont masqués ; les niveaux restent consultables dans Langues du pion. Voir [Affichage individuel](docs/COLLECTIVE_DISPLAY.md).

## Installation

RimWorld 1.6 ou 1.7, Harmony et Cybranian sont nécessaires. Ordre relatif : **Harmony → Cybranian → Individual Learning**. Aucun DLC obligatoire ; les soins des bébés nécessitent Biotech, les pensées linguistiques nécessitent Ideology et le précepte approprié. Aucune dépendance à GateRim. Les deux versions chargent le dossier d'assembly 1.6 via LoadFolders.xml.

Page [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3817893569). Pour une installation manuelle, télécharger le ZIP installable de la [release de compatibilité 1.6/1.7](https://github.com/Diablood/RimLanguages-IndividualLearning/releases/tag/v1.0.0-rw1.7), extraire le dossier dans `<RimWorld>/Mods/RimLanguages-IndividualLearning` et redémarrer le jeu après remplacement de sa DLL. Les sources sont hébergées dans le [dépôt public GitHub](https://github.com/Diablood/RimLanguages-IndividualLearning).

Pour construire depuis les sources, voir [Compilation et dépendances](docs/BUILDING.md). La DLL du complément doit être compilée ; les DLL du jeu et des dépendances ne sont pas fournies.

Une archive locale installable se prépare avec ./package.ps1 : reconstruction, tests et contrôle du contenu inclus. Elle n'est pas publiée automatiquement. Voir [Notice d'installation FR/EN](INSTALLATION.md).

## Options et sauvegardes

La **difficulté de Cybranian** est le seul réglage du rythme. La case **Activer la transmission par conversation** contrôle les gains automatiques du complément, y compris pendant les soins et jeux avec bébé. Elle ne désactive pas la lecture, l'ordre d'apprentissage prisonnier ou les gains collectifs du parent. Voir [Transmission](docs/TRANSMISSION.md) pour les facteurs et différences de calcul.

L'ajout à une partie ayant déjà Cybranian est pris en charge. Pour un pion sans registre, seules la langue et le dialecte de sa faction observée sont attribués à 100 % ; aucun autre savoir collectif n'est importé. L'origine historique d'un colon recruté avant l'installation ne peut pas être reconstituée. Faire l'essai sur une copie : [Migration des sauvegardes](docs/SAVE_MIGRATION.md).

## État et limites

Les parcours principaux jusqu'à 0.17.0 ont été validés en jeu par l'utilisateur, notamment persistance, recrutement, caravane, transmission, enfants et affichage sans progression collective. La 1.0.0 conserve ces comportements ; sa reconstruction a passé **277 assertions hors jeu** et le contrôle des neuf fichiers du paquet. L'agent n'a pas exécuté le moteur ; les variantes facultatives et la compatibilité avec d'autres mods ne sont pas exhaustivement validées. Voir [Vérifications et cas facultatifs](docs/TESTING.md).

Les chemins sans données individuelles exploitables utilisent encore des replis collectifs. Les arrivées naturelles de quêtes et marchands et l'interruption d'un soin ou jeu avec bébé ont été validées par l'utilisateur. Les moments d'interruption n'ont pas été détaillés séparément ; certains replis et la compatibilité avec d'autres mods restent à vérifier. Les interprètes sont reportés à une éventuelle mise à jour.

## Licence

Code et documentation originaux sous [licence MIT](LICENSE). RimWorld, Cybranian et Harmony conservent leurs propres droits et ne sont pas redistribués avec ce projet.

## Documents

- [État actuel](docs/PROJECT_STATE.md), [Roadmap](docs/ROADMAP.md) et [Changelog](CHANGELOG.md).
- [Audit du parent installé](docs/AUDIT_CYBRANIAN.md), [Apprentissage](docs/LEARNING.md), [Prisonniers](docs/PRISONER_LEARNING.md) et [Enfants](docs/CHILDREN.md).
- [Prisonniers : interactions sociales](docs/PRISONER_SOCIAL.md), [Commerce et communications](docs/TRADE_COMMUNICATIONS.md), [Facteurs sociaux](docs/SOCIAL_INTERACTIONS.md), [Ideology](docs/IDEOLOGY_EVENTS.md) et [Quêtes](docs/QUEST_GENERATION.md).
- [Outils développeur](docs/DEVELOPER_TOOLS.md), [Contribution](CONTRIBUTING.md), [Livraison](docs/RELEASE.md) et [Préparation GitHub](docs/GITHUB_SETUP.md).
