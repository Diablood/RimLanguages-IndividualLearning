# Quêtes et visites marchandes — 0.10.0

État fonctionnel en 0.16.0. Le diagnostic 0.10.0 est acquis ; il ne prouve pas les apparitions naturelles. Validations actuelles dans [TESTING.md](TESTING.md).

## API inspectée

Cybranian installé : SHA256 7053DA2F804BB9D0C9360B9CBF6F422C2DB27F7C67C23F30F510717B4174DD11, inchangé. Trois Postfix non publics inspectés :

- QuestNode_GetPawn_IsGoodPawn_FactionLanguagesPatch.Postfix(QuestNode_GetPawn, Pawn, Slate, ref bool).
- QuestNode_GetFaction_IsGoodFaction_FactionLanguagesPatch.Postfix(Faction, Slate, ref bool).
- IncidentWorker_TraderCaravanArrival_CanFireNowSub_FactionLanguagesPatch.Postfix(IncidentParms, ref bool).

Chaque méthode contient exactement un FactionLanguageIsLearned(Faction). Elle conserve un refus initial et ignore une faction nulle. Le filtre de visite marchande conserve aussi CanTradeWithSettings. Le pion candidat d'une quête est le sujet recherché, pas le futur interlocuteur du joueur. Ni ce pion, ni la faction d'incident ne désignent un négociateur.

Dans le jeu installé, IsGoodPawn et IsGoodFaction effectuent leurs sélections vanilla avant ces Postfix ; CanFireNowSub du marchand conserve notamment les contrôles de groupe neutre et de seigneur hostile bloquant. Les signatures et instructions ont été inspectées avant choix des patches. Relevés intermédiaires exclus du suivi sous work/remaining-base-il.txt et work/remaining-game-il.txt.

## Politique

Les quêtes et visites peuvent se présenter sans exiger qu'une langue soit collectivement apprise. Les trois appels linguistiques sont remplacés par le DeferredAvailability déjà utilisé pour le commerce. Si la faction possède une langue valide dans la base, le contrôle linguistique est différé. Aucun colon n'est désigné arbitrairement, aucun interprète fictif et aucune connaissance distribuée.

Cela permet aussi une présentation lorsqu'aucun pion ne maîtrise encore la langue. Le commerce, les appels radio et les interactions de prisonnier restent contrôlés au moment de l'action selon les connaissances du pion réel, comme dans les lots précédents. Le réglage de commerce autorisant une langue inconnue et les malus d'origine restent applicables aux échanges.

L'acceptation d'une quête n'a pas de nouveau verrou linguistique : ces nœuds génériques servent également des récompenses, visiteurs ou situations sans négociation. Le lot ne prétend pas imposer une vérification linguistique à chaque action d'une quête. Les autres règles de sélection, diplomatie, réservations, tirages et execution restent originales.

Sans correspondance linguistique exploitable, repli vers FactionLanguageIsLearned d'origine. Les refus préalables du jeu ne sont jamais transformés en acceptations. Aucun patch général des utilitaires globaux, du générateur de quêtes ou des incidents. XP globale conservée pour la compatibilité, les interfaces et les replis.

## Diagnostic développeur

Actions de debug → Rim Languages → **RLI: inspect quest and trader language gates**. Choisir une faction dans la liste. L'outil appelle les véritables Postfix patchés avec un résultat initial True puis False, par réflexion car ils sont non publics.

Pour une langue résolue, le résultat attendu est **True / False** pour la faction de quête et la visite marchande, même lorsque « Langue collective connue » vaut False. Le filtre de pion est inspecté sur le chef de faction si disponible ; sinon la ligne signale son absence. Le diagnostic ne crée aucun pion, quête, visite ou gain, et ne teste pas les conditions vanilla complètes. Il ne remplace pas un test d'apparition réel. Aucun gizmo ajouté.

## Sauvegardes et vérifications

Schéma 1, types, clés et références inchangés. Quêtes déjà actives et historique conservés ; seulement les futurs appels aux filtres changent. Aucun rattrapage ou import collectif.

Compilation et 196 assertions hors jeu réussies, dont 18 nouvelles sur les trois méthodes installées : appel unique, remplacement, instructions voisines préservées, labels/blocs et signatures, refus des dispositions incompatibles. XML, FR/EN, version et exclusion des DLL tierces réussis. Tests oK utilisateur reçus le 2026-10-11 pour le diagnostic proposé, sans détail séparé des factions et lignes. Aucun moteur exécuté par l'agent ; génération naturelle et régressions encore à vérifier selon TESTING.md. Parcours principaux précédents déjà confirmés par utilisateur.
