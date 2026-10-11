# Événements d'apprentissage — 0.9.0

État fonctionnel en 0.16.0. Les comptes de tests et retours 0.9.0 ci-dessous sont historiques ; validations actuelles et cas facultatifs dans [TESTING.md](TESTING.md).

## API et données inspectées

Cybranian installé, DLL SHA256 7053DA2F804BB9D0C9360B9CBF6F422C2DB27F7C67C23F30F510717B4174DD11. RaceLanguage.AddXp(float add, Pawn doer) et Dialect.AddXp(float add, Pawn doer) contiennent chacun deux appels à HistoryEventsManager.RecordEvent(HistoryEvent, bool).

Le parent émet LanguageExperienceGain pour une XP positive, puis LanguageLearned ou DialectLearned lorsque le niveau collectif est maîtrisé. Le seuil est collectif, sans contrôle explicite de franchissement dans ces méthodes. Tick > 0 et doer non nul sont requis. Dialect.AddXp conserve aussi CanLearn collectif.

Dans le jeu 1.6 inspecté, HistoryEvent contient def et args ; SignalArgs.TryGetArg<Pawn> résout Doer. RecordEvent avertit IdeoUtility et conserve l'historique. PreceptComp_SelfTookMemoryThought utilise l'événement, le pion Doer et le drapeau canApplySelfTookThoughts, puis applique la pensée par le système de mémoires du jeu.

Les cinq préceptes du parent sont conservés : LanguageLearning_Exalted, Encouraged, Neutral, Condemned et Hatred. Les quatre non neutres associent leurs propres pensées aux trois événements ; Neutral n'en ajoute pas. Leurs facteurs LanguageLearningAbility, pensées, durées et effets restent ceux de Cybranian. Aucun Def dupliqué et aucun DLC requis par le complément.

## Politique individuelle

Livres et conversations comparent le niveau personnel de l'étape réellement apprise avant et après le gain. Une hausse réelle émet LanguageExperienceGain avec l'apprenant comme Doer. Le franchissement vers 100 % émet ensuite LanguageLearned ou DialectLearned. Aucun débordement dans l'autre étape, aucune réémission de maîtrise pour une étape déjà complète. Une baisse puis un nouvel apprentissage peuvent produire un nouveau franchissement.

La notification utilise les événements existants et RecordEvent avec les pensées activées. Le jeu reste responsable des mémoires, cumuls, durées, besoins et préceptes du pion. Aucun réglage d'humeur réimplémenté. Pour un livre, la notification suit les ticks de gain individuel, et non les paquets collectifs de 500 ticks ; la cadence de rafraîchissement peut donc différer, sans changer les règles des pensées.

Initialisation maternelle, chargement, ajout développeur et curseur n'émettent pas ces événements : ils ne passent pas par l'apprentissage naturel. Pas de rattrapage de gains ou de mémoires anciens. Aucun nouveau champ sauvegardé, schéma 1 inchangé.

## Éviter le double déclenchement

Deux transpilers remplacent seulement les appels RecordEvent dans les deux AddXp du parent, en conservant les écritures XP, conditions et reste des instructions. La fonction de remplacement supprime ces trois événements collectifs uniquement avec Ideology actif et un pion Doer dont le registre individuel peut être initialisé. Les événements individuels sont enregistrés directement et ne passent pas par ce filtre.

Sans Ideology, avec un autre événement, sans Doer valide ou sans registre individuel résolu : appel original conservé. Aucun patch global de HistoryEventsManager. Les quêtes, incidents, recrutement automatique et autres événements restent inchangés. XP globale toujours active. Depuis 0.10.0, les filtres de quêtes et visites marchandes diffèrent le contrôle linguistique aux actions individuelles : voir QUEST_GENERATION.md.

## Vérifications

Retour utilisateur du 2026-10-11 : seules les pensées de lecture sont visibles. Dernière sauvegarde de test disponible : mode classique, aucun précepte LanguageLearning_* ; le DLC est actif. Cela explique l'absence de pensée linguistique dans cette partie, sans prouver le fonctionnement des notifications dans une idéologie avec précepte. Une configuration personnalisée avec précepte explicite est nécessaire pour le parcours positif ; aucun précepte injecté automatiquement et aucune sauvegarde modifiée.

Compilation et 178 assertions hors jeu réussies, dont 18 nouvelles couvrant les franchissements et les transformations sur les deux méthodes installées : écritures collectives préservées, métadonnées de branche conservées et refus des dispositions incompatibles. XML, FR/EN, versions et exclusion des DLL tierces réussis.

Aucun test en jeu de 0.9.0 exécuté par l'agent. Tests OK utilisateur reçus le 2026-10-11 après clarification du précepte requis : validation générale du parcours principal proposé. Les pensées négatives, absence de double notification, conversations et fonctionnement sans DLC ne sont pas confirmés séparément ; voir TESTING.md. Les tests principaux jusqu'à 0.9.0 sont acquis par retour utilisateur.
