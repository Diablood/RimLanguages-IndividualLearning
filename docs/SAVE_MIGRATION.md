# Sauvegardes et migration — schéma 1

## Données persistantes

CompIndividualLanguages appartient au pion et sauvegarde PawnLanguages en profondeur sous RLI_individualLanguages.

Champs : schema, initialized, originCaptured, originFaction et entries. Chaque entrée contient key, lastKnownName, isDialect, native et progress entre 0 et 1. Les identifiants RaceLanguage_{id} et Dialect_{id} sont ceux du parent ; sa base et ses références restent sauvegardées par ses propres composants.

Types, clés et schéma **1** conservés jusqu'à 0.16.0. Les adaptations de restrictions ne réécrivent pas les souvenirs, relations ou quêtes existants.

## Ajouter le complément à une partie ayant Cybranian

1. Garder une copie de la sauvegarde d'origine.
2. Activer Harmony, Cybranian puis le complément, et charger cette copie.
3. Vérifier Langues sur des pions de factions différentes ; sauvegarder sous un nouveau nom puis recharger.

Un composant absent produit un registre vide. La passe unique de LoadedGame, après désérialisation et disponibilité du parent, parcourt PawnsFinder.All_AliveOrDead : cartes, monde, caravanes et transports. Elle initialise seulement les registres absents/non initialisés à partir d'une association valide de faction.

**Attribution minimale :** langue et dialecte de la faction observée à 100 %. Aucun autre savoir collectif importé, aucun gain passé rattrapé ni événement Ideology ajouté. Les registres déjà initialisés gardent leurs niveaux, même abaissés en mode développeur.

Pour un colon recruté avant l'installation, l'ancienne faction n'est pas reconstituable : la faction observée sert d'origine. Cette attribution ne prouve pas sa langue historique réelle.

Sans association valide, initialisation en attente ; pas de langue aléatoire inventée. Une ligne [RLI initialization] peut résumer les nombres initialisés/en attente, sans noms de pions. Pas de fenêtre de migration.

## Faction et déplacements

La faction observée avant SetFaction est capturée si nécessaire. Un registre déjà initialisé n'est pas réattribué au recrutement, au chargement ou à l'apparition.

Le même pion et son composant persistent dans les caravanes et conteneurs. Leur sérialisation a été inspectée dans les assemblies installées. Recrutement, sauvegarde/rechargement après recrutement et caravane confirmés par utilisateur ; capsules de transport non testées et explicitement écartées du parcours actuel.

## Enfants

Les nouveaux bébés passant par le hook ApplyBirthOutcome reçoivent leur identité maternelle à **0 %** dans les mêmes champs. Aucun enfant existant réinitialisé. Les bébés générés par scénario/événement/debug gardent leur initialisation habituelle.

Soins et jeux utilisent les références de début déjà sauvegardées par les drivers vanilla. Aucun champ Scribe de job ajouté ; le garde-fou de crédit est transitoire. Les gains terminés persistent dans le registre. Rechargement au milieu d'une activité non confirmé séparément. Replis de naissance : [CHILDREN.md](CHILDREN.md).

## Options depuis 0.14.0

conversationTransmissionEnabled est un paramètre global du mod, distinct du schéma des pions. L'ancienne clé conversationTransmissionSpeed est lue uniquement au chargement :

- 0 ou valeur négative : désactivé.
- Valeur positive : activé ; l'ancien multiplicateur est abandonné.
- NaN ou infini : activé, comme l'ancien défaut réparé.
- Nouvelle clé booléenne présente : prioritaire.

Seule la nouvelle clé est écrite. Aucun niveau personnel ou réglage du parent modifié. Activation/désactivation et persistance après relance confirmées ; variantes anciennes non détaillées séparément.

## Édition et retrait

Les outils développeur écrivent dans le registre ordinaire. Ajouter un dialecte complète sa langue ; baisser une langue remet ses dialectes présents dans la base à zéro. Origine et marqueurs maternels conservés. Ces règles s'appliquent à l'édition, sans réparation automatique au chargement.

Le retrait du complément n'a pas de migration dédiée ; une désinstallation transparente n'est pas garantie. Cybranian doit rester présent pour utiliser ce complément. Validations et variantes : [TESTING.md](TESTING.md).