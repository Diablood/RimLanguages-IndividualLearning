# Commerce et communications — 0.6.0

État fonctionnel en 0.16.0. Les comptes de tests et retours 0.6.0 ci-dessous sont historiques ; validations actuelles et cas facultatifs dans [TESTING.md](TESTING.md).

## Règles conservées et individualisées

Le négociateur réel doit maîtriser à 100 % la langue attribuée à la faction du partenaire pour appeler cette faction ou un vaisseau par console. Le dialecte n'est pas requis pour appeler. Les refus d'origine (chef indisponible, capacités, réservation, titre requis, hostilité, vaisseau parti) sont conservés.

Le commerce conserve le réglage de Cybranian : CanTradeWithSettings est vrai lorsque languageTradeFactor < 5. Une langue inconnue autorise alors le commerce avec son malus ; sinon elle l'interdit. Le complément remplace le savoir collectif par celui du négociateur. Une langue à 100 % et un dialecte incomplet appliquent le malus de dialecte ; langue et dialecte à 100 % retirent ces seuls malus. Aucun cumul nouveau : les branches et facteurs de Cybranian sont réutilisés, achat multiplié et vente divisée par le facteur. Les infobulles suivent les mêmes niveaux que les prix.

Les appels radio continuent d'exiger la langue, même lorsque les paramètres permettent le commerce avec une langue inconnue : c'est la règle originale des communications. Le partenaire est identifié par sa faction pour ces parcours, pas par les marqueurs maternels d'un marchand recruté auparavant. Les différences de Social, inspirations et autres facteurs de prix restent ceux du jeu.

## Audit des assemblies installées

Cybranian SHA256 revérifié : 7053DA2F804BB9D0C9360B9CBF6F422C2DB27F7C67C23F30F510717B4174DD11. RimWorld et dépendances sont ceux des scripts build.ps1/test.ps1. IL local : work/trade-base-il.txt, work/trade-game-il.txt, work/trade-entry-il.txt, work/trade-flow-il.txt.

- Postfix de Faction_CommFloatMenuOption_FactionLanguagesPatch et PassingShip_CommFloatMenuOption_FactionLanguagesPatch : signature (Building_CommsConsole console, Pawn negotiator, ref instance, ref FloatMenuOption result). Remplace exactement un FactionLanguageIsLearned par LanguageKnown(Faction, Pawn), avec l'argument negotiator.
- Postfix de TradeUtility_GetPricePlayerBuy_FactionLanguagesPatch, TradeUtility_GetPricePlayerSell_FactionLanguagesPatch et Tradeable_GetPriceTooltip_FactionLanguagesPatch : remplacent exactement un contrôle de langue et un de dialecte. Le pion est le champ public TradeSession.playerNegotiator ; la faction provient du trader de cette même session. Aucun contexte global temporaire ajouté.
- Postfix de Pawn_CanTradeNow_FactionLanguagesPatch, TradeShip_CanTradeNow_FactionLanguagesPatch, Settlement_CanTradeNow_FactionLanguagesPatch et Caravan_CanTradeNow_FactionLanguagesPatch : n'ont aucun négociateur. Le contrôle linguistique seul est différé lorsque la langue de faction est résolue. Le résultat faux préalable et toutes les autres instructions restent intacts. Cela permet d'afficher le partenaire et de choisir un négociateur, sans déclarer tous les pions compétents.
- Postfix de FactionUtility.CanTradeWith(Pawn p, Faction faction, TraderKindDef traderKind) : ajoute seulement un refus linguistique si le résultat original était accepté. L'IL de FloatMenuOptionProvider_Trade.GetOptionsFor utilise cette méthode après CanTradeNow. BestCaravanPawnUtility.FindBestNegotiator filtre aussi ses candidats par CanTradeWith : le meilleur négociateur parmi les candidats admissibles est conservé, sans nouveau choix du complément.
- Prefix des TryOpenComms(Pawn negotiator) de Faction et TradeShip : vérifie de nouveau la langue pour une tâche engagée avant baisse du niveau.
- Prefix de TradeDeal.TryExecute(out bool actuallyTraded) : refuse la transaction si le réglage interdit le commerce dans une langue inconnue et que le négociateur réel ne la maîtrise plus. Retour false, actuallyTraded false et message localisé ; aucune transaction partielle exécutée par ce prefix. Inclut les cadeaux de cette session. Le constructeur Dialog_Trade et TradeSession.SetupWith restent exécutés normalement.

Les transpilers déplacent les labels et métadonnées d'exception sur la première instruction remplacée. Une forme absente ou ambiguë est refusée explicitement. Source complète : Source/TradeLanguages.cs.

## Replis et limites

Sans registre personnel ou correspondance de faction exploitable, les tests linguistiques utilisent Cybranian. Sans faction, aucune nouvelle langue n'est inventée. Sans négociateur, un prix reste collectif ; les menus et l'ouverture radio n'utilisent jamais le pion d'une ancienne session de commerce.

Historique 0.6.0 : l'incident TraderCaravanArrival.CanFireNowSub conservait son filtre collectif. Depuis 0.10.0, ce filtre et ceux des quêtes diffèrent le contrôle linguistique aux actions individuelles pour une faction résolue ; voir QUEST_GENERATION.md. Romance et facteurs sociaux sont intégrés depuis 0.7.0, événements Ideology depuis 0.9.0. Les sous-classes spéciales de PassingShip gardent leurs propres méthodes d'exécution ; leur menu de base est adapté, mais le contrôle de réexécution explicite couvre Faction et TradeShip seulement.

Une tâche de commerce à pied déjà ordonnée peut encore ouvrir sa fenêtre après baisse du niveau : le garde-fou de transaction empêche l'échange lorsque la langue est obligatoire. Les prix peuvent être mis en cache par la fenêtre ; fermer et rouvrir après une édition développeur pour comparer les nouveaux niveaux. Aucun rechargement forcé d'interface ajouté.

Collectif, livres, configuration initiale et sauvegardes d'origine conservés. Schéma personnel 1 inchangé ; aucune importation, aucun nouveau champ sauvegardé. Les fonctions linguistiques globales ne sont pas patchées universellement.

## Vérifications

Compilation 0.6.0, 134 assertions hors jeu, XML, FR/EN, versions et exclusion des DLL tierces réussis. Les 55 nouvelles assertions couvrent les neuf méthodes installées, la provenance du négociateur, les contrôles exacts, la conservation des facteurs/textes/refus/labels/blocs, le refus des formes incompatibles, le seuil individuel et les signatures des gardes d'exécution. Elles ne lancent pas Harmony dans Unity ni un échange réel. Aucun test en jeu de 0.6.0 exécuté par l'agent ; parcours court dans TESTING.md.


## Confirmation utilisateur 0.6.0

Le 2026-10-11, utilisateur : « Tests OK » après livraison de la 0.6.0 et parcours proposé de comparaison des niveaux du négociateur, des malus de langue/dialecte et des appels radio. Validation générale du lot en jeu par utilisateur ; aucun moteur exécuté par l'agent. Les régressions complémentaires (transaction après baisse en cours de tâche, cadeaux, caravanes, titres, migrations et cas sans données) ne sont pas confirmées séparément. Version conservée 0.6.0 ; mise à jour documentaire uniquement, aucune recompilation ni copie.
