# Nouveau-nés, soins et jeux — 0.16.0

## Éducation préservée

L'apprentissage linguistique s'ajoute aux activités existantes. Aucun patch des leçons, compétences, passions, points ou choix de croissance ; aucun niveau linguistique requis pour l'éducation vanilla. Aucun nouveau job ni changement de priorité ou de durée.

Les enfants déjà présents gardent leurs niveaux. À partir du stade enfant, la transmission ordinaire applique ses règles Social ; une langue apprise lentement ne bloque pas la formation.

## Naissance Biotech

Le seul appel GeneratePawn(PawnGenerationRequest) de PregnancyUtility.ApplyBirthOutcome est remplacé par un wrapper recevant geneticMother. Il résout les marqueurs maternels de la mère avant génération, sans exiger leur maîtrise actuelle. Un contexte transitoire try/finally diffère l'initialisation normale.

Le bébé humain nouvellement créé, avec registre vide, reçoit langue et dialecte maternels à **0 %**. Le marqueur maternel indique l'origine, pas la maîtrise. Aucun savoir collectif importé ou événement Ideology à la naissance.

La faction actuelle de la mère ne remplace pas son identité enregistrée après recrutement. Si cette identité manque, l'association de faction du bébé est utilisée à 0 % lorsqu'elle est résolue. Depuis 1.1.2, sans identité maternelle ou de faction valide, une paire existante compatible avec la race peut être tirée au sort à 0 %. Sans paire compatible, l'initialisation reste éventuellement différée et l'identité à 0 % n'est pas garantie. Une association partiellement valide ne crée pas d'entrées.

Ce chemin ne couvre pas les bébés de scénario, événement ou debug. Ceux-ci gardent leur initialisation habituelle. Aucun registre existant réinitialisé ; naissances en cuve ou remplacées par d'autres mods non confirmées séparément.

Le correctif 0.15.1 applique Harmony depuis Bootstrap [StaticConstructorOnStartup], après résolution des Defs. Il évite l'initialisation prématurée de PregnancyUtility constatée en 0.15.0.

## Règles communes de transmission

Biotech, bébé admissible et adulte Social actif maîtrisant une cible à 100 %. Depuis 1.1.0, dans une même faction, seules les connaissances maternelles de l'enseignant sont admissibles ; entre factions différentes, les connaissances apprises restent admissibles. Une entrée par soin, langue parente avant dialecte.

Exception bébé : son Social n'est pas exigé et sa statistique désactivée n'est pas évaluée. Aptitude de référence 0, donc base 0,5 du calcul existant. Social adulte, variation, difficultés du parent et case d'activation conservés.

BabyCareLearning.Learn applique le gain de référence d'une conversation multiplié par l'exposition réelle bornée à 0–1. Aucun gain au trajet ou à la planification. Pas de bulle chiffrée ou de nouvelle notification ; consulter Langues. LearningHistory suit les gains réels.

## Allaitement et nourrissage

Postfix des fabriques JobDriver_Breastfeed.Breastfeed et JobDriver_BottleFeedBaby.FeedBabyFoodFromInventory : ajout d'une action de fin après les actions originales, sans patch des ticks ou callback de soin.

Exposition = augmentation positive de la proportion de nourriture depuis initialFoodPercentage, sauvegardé par le driver vanilla. Aucun gain sans nourriture reçue ; une interruption après consommation peut créditer la partie réalisée. Crédit unique par Toil courant.

Père et autres adultes peuvent enseigner pendant le nourrissage. Nutrition, allaitement, souvenirs et exposition idéologique d'origine conservent leur exécution. Le simple portage et les autres soins ne donnent pas d'XP supplémentaire.

## Jeux avec bébé

Postfix de Verse.AI.JobDriver_BabyPlay.AddPlayThoughtIfAppropriate, helper de fin partagé par jeux sur place, jouets et promenade. Exécution originale conservée ; aucun patch du besoin de jeu, des loisirs, pensées ou ticks.

finishedSetup doit être vrai et initialPlayPercentage valide, non négatif. Exposition = augmentation nette positive du besoin de jeu, bornée à 0–1. Préparation inachevée, données non finies ou absence d'augmentation : aucun gain. Interruption après jeu effectif : gain de la partie réalisée.

ConditionalWeakTable associe un crédit unique à chaque driver sans retenir les travaux terminés. Les références de début et préparation sont déjà sauvegardées par le jeu ; aucun champ Scribe ajouté.

## Persistance et vérifications

Schéma 1 et clés inchangés. Les niveaux maternels à 0 % persistent sans réattribution au chargement ou changement de faction ; anciens enfants préservés.

Dernière compilation : 269 assertions hors jeu, dont contrôles des signatures, transformation de naissance, fabriques de soins, préparation, exposition proportionnelle, anti-doublon et sérialisation vanilla.

Utilisateur : correctif de démarrage et parcours principal naissance/nourrissage/persistance/éducation confirmés en 0.15.1 ; jeu, case désactivée et persistance confirmés en 0.16.0. Ne pas redemander ces parcours. Interruptions, rechargement au milieu d'une activité et variantes distinctes de jeu non confirmés séparément : [TESTING.md](TESTING.md). Aucun moteur exécuté par l'agent.