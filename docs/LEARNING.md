# Apprentissage individuel

## Registres individuel et collectif

Livres et conversations alimentent le registre du pion concerné. Les gains collectifs de Cybranian continuent selon ses règles, sans import dans les fiches personnelles et sans gain attribué aux tiers.

Les restrictions intégrées consultent maintenant les pions concernés : [prisonniers](PRISONER_SOCIAL.md), [commerce et communications](TRADE_COMMUNICATIONS.md), [facteurs sociaux](SOCIAL_INTERACTIONS.md). Les événements [Ideology](IDEOLOGY_EVENTS.md) suivent les gains personnels lorsque leurs conditions sont réunies. Les utilitaires collectifs restent utilisés pour les replis sans données exploitables.

## Lecture

Les livres et leurs Defs, textures, génération et références sauvegardées sont ceux du parent.

- DoesProvidesOutcome(Pawn reader) conserve une réponse vraie du parent et permet aussi le rattrapage du lecteur lorsque son niveau personnel est incomplet, même avec un collectif à 100 %.
- OnReadingTick(Pawn reader, float factor) crédite uniquement le lecteur après l'exécution originale. Aucun crédit si un autre prefix a supprimé cette exécution.
- ProgressPerTick(reader, factor) fournit le calcul original : qualité, aptitude, vitesse de lecture et difficulté. Le facteur transmis par Book.OnBookReadTick incorpore déjà la durée ; aucun second multiplicateur de ticks.
- Langue avant dialecte ; une seule entrée progresse par appel, sans débordement vers le dialecte lorsqu'une langue atteint 100 %.

Le collectif peut encore bénéficier d'un livre lorsque le lecteur a terminé ses niveaux personnels. Une interruption ne donne pas de gain personnel anticipé ou différé.

**Différence conservée :** le calcul de lecture du parent multiplie par sa difficulté d'apprentissage, tandis que les conversations utilisent cette difficulté comme diviseur. Augmenter la jauge ne doit donc pas être présenté comme un ralentissement uniforme de tous les chemins.

## Conversations

Deux chemins sont distingués dans le hook de Pawn_InteractionsTracker.TryInteractWith :

- Le job original LearnPrisonerLanguage garde l'exposition maternelle et sa formule antérieure ; voir [PRISONER_LEARNING.md](PRISONER_LEARNING.md).
- Chitchat et DeepTalk ordinaires réussis transmettent automatiquement une connaissance maîtrisée selon [TRANSMISSION.md](TRANSMISSION.md). Aucune transmission supplémentaire du nouveau chemin sur le job dédié.

Les sens sont indépendants. L'aléa personnel est isolé avec Rand.PushState/PopState pour préserver le flux du jeu. Aucun apprentissage par simple proximité.

La case de transmission ne désactive pas la lecture, le job prisonnier ou la XP collective. Les soins et jeux avec bébé réutilisent la transmission : [CHILDREN.md](CHILDREN.md).

## Persistance et limites

Les gains utilisent le registre sauvegardé du pion, plafonné à 100 %, et LearningHistory pour les événements admissibles. Initialisation, chargement et édition développeur ne créent pas d'événements d'apprentissage.

autoLearnRecruiteeLanguage conserve son effet collectif ; il ne complète pas automatiquement les connaissances des colons ou de la recrue. Les associations non résolues attendent une initialisation valide ; les langues retirées de la base ne deviennent pas de nouvelles cibles.

Parcours principaux de lecture, prisonnier, transmission et persistance confirmés par utilisateur. Preuves et variantes : [TESTING.md](TESTING.md). Aucun moteur exécuté par l'agent.