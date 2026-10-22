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

## Repli général depuis 1.1.2

La faction d'origine reste prioritaire. Sans association exploitable, un humanoïde non initialisé et sans identité maternelle reçoit une paire existante dans la base : langue dont MainRace correspond exactement à sa race, puis un de ses dialectes valides. Chaque langue candidate a la même chance, puis chaque dialecte de la langue choisie. Seule exception de race explicite : CreepJoiner correspond à Human. Les xénotypes humains utilisent déjà la race Human ; les autres races modées ne sont pas assimilées automatiquement aux humains.

Tirage individuel local, sans modifier l'aléatoire du jeu ni la langue collective d'une faction. Le choix est sauvegardé dans les mêmes entrées maternelles ; schéma 1, types et clés conservés. Un registre initialisé n'est jamais retiré ou redistribué. Les apprentissages préexistants d'un registre en attente sont conservés ; une identité maternelle existante bloque le repli.

Le chargement des anciennes sauvegardes et les accès habituels peuvent résoudre les registres en attente. Pendant GeneratePawn, le repli est différé pour ne pas confondre absence transitoire de faction et origine réellement inconnue. Aucun gain rattrapé ou pensée Ideology ajouté. Sans paire de la race, l'initialisation reste en attente jusqu'à disponibilité de données compatibles ; aucun langage inventé et aucune langue humaine imposée. L'attribution est une convention de gameplay, pas une reconstitution historique.

Les bébés déjà initialisés à 0 % sont inchangés. Pour une naissance prise en charge sans identité maternelle ou de faction, le choix par race commence également à 0 %. Sans candidat compatible, le comportement différé antérieur demeure ; les naissances remplacées par d'autres mods ne sont pas garanties.

## Historique : exception Anomaly en 1.1.1

Choix explicite de l'utilisateur : les arrivants mystérieux (race CreepJoiner) sans langue d'origine exploitable reçoivent une langue humaine déjà présente dans la base puis l'un de ses dialectes, tirés au sort à 100 % comme maternels. Tirage local indépendant de l'aléatoire du jeu, langues puis dialectes ordonnés par identifiant, graine liée au pion. Les entrées sauvegardées font ensuite autorité : aucun nouveau tirage pour un registre initialisé, même après changement de faction, baisse développeur ou modification de la base. Schéma 1 et référence d'origine inchangés.

Le chargement répare aussi les arrivants restés non initialisés, sans retirer leurs éventuels apprentissages. Une identité maternelle existante bloque le repli ; les autres races restent en attente si leur faction n'est pas résolue. Sans paire humaine valide, aucune connaissance n'est inventée. La faction collective Entities n'est pas modifiée. Cette origine est attribuée pour le gameplay, sans prétendre reconstituer l'histoire de l'arrivant.

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