# Audit local de Cybranian — 2026-10-10

## Affichage collectif — inspection du 2026-10-11, lot 0.17.0

DLL Cybranian revérifiée, empreinte inchangée. FactionUIUtility_DrawFactionRow_FactionLanguagesPatch.Postfix(Faction, ref float, Rect, ref float) appelle DrawUtility.DrawFactionTabRow, qui ajoute 30 pixels et affiche noms, XP/statuts collectifs et accès au diagramme. Le moteur DrawFactionRow renvoie 80 pixels. Le Postfix de conversation du parent contient deux ThrowText(Vector3, Map, string, float) liés aux gains collectifs. Remplacement ciblé de la présentation décrit dans [COLLECTIVE_DISPLAY.md](COLLECTIVE_DISPLAY.md) ; calculs, sauvegardes et configuration initiale conservés. Compilation et 277 assertions hors jeu réussies, aucun moteur exécuté par l'agent.

Ce document conserve les inspections datées des assemblies installées et les décisions de chaque lot. Les états de livraison et limites décrits pour une ancienne version sont historiques. Pour le comportement et les validations actuels en 0.16.0, lire [PROJECT_STATE.md](PROJECT_STATE.md), [TESTING.md](TESTING.md) et les documents des sous-systèmes.

## Audit du cycle de vie — 2026-10-11, version 0.13.0 conservée

Moteur réellement installé : RecruitUtility.Recruit appelle Thing.SetFaction virtuellement sur le pion, sans recréer ses composants ; Pawn.SetFaction conserve l'instance et ses composants. Caravan.ExposeData sérialise le propriétaire « pawns » en profondeur. TravellingTransporters.ExposeData sérialise ActiveTransporterInfo sous « pods », malgré le champ nommé transporters ; ActiveTransporterInfo expose innerContainer et gère aussi un mode de références des pions. ThingOwner<T>.ExposeData expose innerList et restitue holdingOwner. Aucun patch supplémentaire jugé nécessaire sur ces observations, tests moteur toujours distincts.

Parent installé : Prefix RecruitUtility_Recruit conserve autoLearnRecruiteeLanguage et apprend dans la base collective ; il n'est pas remplacé. Le complément n'importe pas ces gains dans les autres pions. IL local exclu sous work/lifecycle-game-il.txt et work/lifecycle-base-il.txt. Trois assertions supplémentaires sur les API installées, total 239 hors jeu réussies ; aucune exécution du moteur.

## Complément d'audit 0.13.0 — 2026-10-11

MoteMaker.MakeInteractionBubble renvoie le MoteBubble créé ; TryInteractWith lui transmet interactionMote, GetSymbol et GetSymbolColor avant de terminer l'interaction. SetupMoteBubble conserve le target et crée la matière/propriété de couleur de l'icône ; DrawAt respecte HideMotes. Marquer le résultat exact évite de créer une deuxième bulle ou de rechercher une ancienne bulle voisine. Le parent n'est pas modifié. Nouveaux Prefix/Finalizer du hook existant et Postfix ciblé de création de bulle ; contexte par invocation, repli sans marquage si aucun résultat exploitable. IL local exclu sous work/bubble-game-il.txt et work/bubble-type-il.txt. Signatures compilées/testées, aucune exécution du moteur.

## Complément d'audit 0.12.1 — 2026-10-11

Postfix TryInteractWith du parent : retour sans gain quand les deux pions ont le même IsPlayerControlled ; textes ThrowText liés aux AddXp collectifs de langue/dialecte, pas aux connaissances personnelles. MoteMaker.ThrowText(Vector3, Map, string, Color, float) inspecté dans le moteur ; crée Mote_Text, utilise Rand pour sa vitesse, d'où l'isolation de l'aléa visuel du complément. Aucune suppression du Postfix parent ni modification de sélection des interactions. IL local exclu sous work/transmission-mote-il.txt et work/transmission-base-il.txt. Voir TRANSMISSION.md et TESTING.md. Aucun moteur exécuté.

## Complément d'audit 0.12.0 — 2026-10-11

TryInteractWith(Pawn recipient, InteractionDef intDef) renvoie bool dans le moteur installé. Le job Cybranian LearnPrisonerLanguage utilise DeepTalk, prisonnier en cible B. LanguageLearningAbility : SkillNeed_BaseBonus Social, base 0 et bonus 0,065 par niveau ; ValueAtLevel confirmé base + bonus × niveau dans le jeu. Pas de second facteur Social apprenant ; baseline positive pour niveau 0 et facteur Social locuteur remplacent la négociation sur le nouveau chemin. Mod.SettingsCategory, DoSettingsWindowContents et Listing_Standard.Slider(float,float,float) inspectés ; le parent utilise aussi les widgets standards. Un seul Postfix du complément étendu, aucun nouveau patch de sélection d'interaction. IL intermédiaire exclu sous work/transmission-*.txt. Détails dans TRANSMISSION.md ; aucun moteur exécuté.

## Complément d'audit 0.11.0 — 2026-10-11

Parent installé : empreinte DLL inchangée, GameComponent_FactionLanguages.FinalizeInit initialise la base et LearnPlayerLanguage ; ExposeData conserve uniqueIDsManager et raceLanguageDatabase. Moteur : ThingWithComps.ExposeData appelle InitializeComps pendant LoadingVars avant PostExposeData, depuis les composants des Defs actuelles. Game.FillComponents instancie les GameComponents absents. Game.LoadGame finalise Scribe, appelle FinalizeInit puis LoadedGame ; FinalizeInit termine les composants avant de passer à Playing. L'ancien parcours des cartes/monde n'inclut pas les caravanes ; All_AliveOrDead y ajoute AllCaravansAndTravellingTransporters_AliveOrDead. Migration au chargement conservée, parcours élargi sans nouveau patch Harmony. IL local exclu sous work/migration-game-il.txt, work/migration-base-il.txt et work/migration-scribe-il.txt. Politique dans SAVE_MIGRATION.md ; aucune exécution du moteur.

## Complément d'audit 0.10.0 — 2026-10-11

Empreinte DLL inchangée. Postfix non publics de QuestNode_GetPawn.IsGoodPawn, QuestNode_GetFaction.IsGoodFaction et IncidentWorker_TraderCaravanArrival.CanFireNowSub : un appel FactionLanguageIsLearned chacun, garde-fou de résultat initial et faction nulle ; réglage CanTradeWithSettings conservé dans la visite marchande. Signatures et conditions vanilla inspectées dans le jeu. Candidat de quête distinct d'un négociateur ; contrôle différé avec le même helper que les disponibilités marchandes. Détails et limites dans QUEST_GENERATION.md. Les trois filtres du diagnostic sont appelés par réflexion ; aucun événement généré. IL intermédiaire exclu du suivi sous work/remaining-base-il.txt et work/remaining-game-il.txt.

## Complément d'audit 0.9.0 — 2026-10-11

Empreinte DLL inchangée. RaceLanguage.AddXp(float, Pawn) et Dialect.AddXp(float, Pawn) : deux appels RecordEvent chacun, événements LanguageExperienceGain et maîtrise collective. HistoryEvent, SignalArgs.TryGetArg<Pawn>, RecordEvent et PreceptComp_SelfTookMemoryThought inspectés dans le jeu installé. Cinq préceptes et pensées du parent inspectés dans Ideology/Defs/Precepts/Precepts_LanguageLearning.xml. Politique individuelle, repli et limites dans IDEOLOGY_EVENTS.md ; IL intermédiaire exclu du suivi sous work/ideology-base-il.txt et work/ideology-game-il.txt. Aucun test moteur exécuté.

## Complément d'audit 0.8.0 — outils développeur

Empreinte Cybranian revérifiée et inchangée. FactionLanguages.DebugTools : LearnMainLanguage, LearnLanguageFull et LearnLanguageDialect parcourent la base et ouvrent une Dialog_DebugOptionListLister de DebugMenuOption. Le parent filtre sur IsLearned collectif ; le complément propose les entrées personnelles absentes/incomplètes et écrit uniquement dans le registre du pion cliqué. Aucun patch parent ajouté. Constructeur de liste avec options/header inspecté dans le jeu installé. Voir DEVELOPER_TOOLS.md pour le parcours et la cohérence.

## Complément d'audit 0.7.0 — 2026-10-11

Cybranian SHA256 inchangé. Trois Postfix inspectés : NegativeInteractionChanceFactor, SecondaryRomanceChanceFactor et SocialFightChance. Distinction originale joueur/collectif contre autres/comparaison de factions ; facteurs de langue/dialecte exclusifs. Les trois prefixes du complément remplacent seulement le calcul linguistique lorsqu'une paire personnelle est résolue. Signatures, indices Harmony et six champs de réglage vérifiés ; IL local work/social-base-il.txt. API DebugAction/DebugTool et trois méthodes de calcul publiques inspectées dans l'assembly du jeu pour un outil de diagnostic sans interaction. Voir SOCIAL_INTERACTIONS.md ; aucun test moteur exécuté.

## Complément d'audit 0.6.0 — 2026-10-11

Empreinte Cybranian inchangée. Neuf Postfix inspectés : deux menus radio, prix achat/vente et infobulle, quatre CanTradeNow sans acteur. CanTradeWithSettings équivaut à languageTradeFactor < 5. RimWorld inspecté : TradeSession.playerNegotiator public, CanTradeWith(Pawn, Faction, TraderKindDef), FindBestNegotiator et son filtre, menus de commerce, TryOpenComms de faction/vaisseau, constructeur Dialog_Trade et TryExecute(out bool). Prix/facteurs/infobulles originaux réutilisés, disponibilité sans acteur différée au négociateur, garde-fous d'exécution. Incidents d'arrivée marchande collectifs conservés. Détails et sources IL locales dans TRADE_COMMUNICATIONS.md. Tests hors jeu seulement.

## Complément d'audit 0.5.0 — 2026-10-11

Empreinte Cybranian revérifiée et inchangée. Inspection des Postfix Recruitable et SetExclusiveInteraction, du Prefix RecruitUtility.Recruit, des JobOnThing/HasJobOnThing Chat et Convert, de WorkGiver_Scanner.HasJobOnThing et des Toils_Interpersonal de recrutement/conversion. Le Prefix Recruit ne bloque pas l'exécution et applique seulement l'option collective autoLearnRecruiteeLanguage ; il n'est pas modifié. Les menus utilisent FactionLanguageIsLearned sans acteur. Le travail et les interactions RecruitAttempt, BuildRapport, ConvertIdeoAttempt permettent le contrôle du geôlier réel. Signatures, décisions et limites dans PRISONER_SOCIAL.md. IL local : work/prisoner-social-base-il.txt et work/prisoner-social-game-il.txt. Aucune validation moteur.

## Sources effectivement inspectées

- RimWorld : `<RimWorld>`, `Version.txt` = **1.6.4871 rev590**.
- Références : `RimWorldWin64_Data/Managed/Assembly-CSharp.dll`, mscorlib, System, System.Core, netstandard et modules UnityEngine.
- Cybranian : `<SteamLibrary>/steamapps/workshop/content/294100/3450332059`.
- Métadonnées : package `DimonSever000.FactionLanguages`, auteur DimonSever000, versions 1.5/1.6. Pas de numéro de release exploitable dans About.xml.
- DLL inspectée : `1.6/Assemblies/FactionLanguages.dll`, SHA256 `7053DA2F804BB9D0C9360B9CBF6F422C2DB27F7C67C23F30F510717B4174DD11`.
- Harmony réellement présent : `<Harmony>/Current/Assemblies/0Harmony.dll`.

Inspection des métadonnées et instructions IL avec Mono.Cecil local, ainsi que des XML installés. Ce rapport distingue ce que révèle le code de ce qui devra être vérifié en partie. Le relevé IL est un intermédiaire local hors livrable.

## Données et sauvegardes du mod de base

`GameComponent_FactionLanguages` détient `RaceLanguageDatabase` et `UniqueIDsManager`. La base associe `Faction` à `Dialect` et possède une liste de `RaceLanguage`. Chaque dialecte a un parent. Les XP appartiennent à ces objets mondiaux, pas à un Pawn.

`RaceLanguage` expose `Xp`, `XpMax`, `Progress`, `IsLearned`, `AddXp(float, Pawn)`, `Learn(bool)`. Le maximum est calculé par `RaceLanguageDef.GetCost()`. `Dialect` expose les mêmes valeurs, `CanLearn()`, `AddXp(float, Pawn)` et `Learn()`. Son maximum est mémorisé ; `CanLearn()` consulte la maîtrise globale du parent. Les `AddXp` bornent les valeurs et peuvent produire des événements d'histoire attribués au pion passé en argument. Ce pion n'est pas propriétaire de l'XP.

Clés Scribe d'origine conservées : `uniqueIDsManager`, `raceLanguageDatabase`, `raceLanguages`, `factionDialects`, `dialects`, `xp`, `xpMax`, `parent`, `id`, `def`, `name`, `mainRace`, `techLevel`, `overridenDescription`. Langues et dialectes sont enregistrés en profondeur, les associations par référence. Les identifiants persistants sont `RaceLanguage_{id}` et `Dialect_{id}`.

`FinalizeInit()` de Cybranian initialise la base au besoin puis apprend globalement la langue du joueur. Le complément ne modifie pas cette étape.

## Génération et interface

Les Defs installées comprennent Human/Alien, les dialectes standard, impérial, Horax, ancien et xéno, ainsi que des familles associées aux races et factions. Les stratégies de génération incluent regroupement par culture, niveau technologique, race et génération aléatoire réglable. La page `Page_ConfigureLanguage`, les outils de création/renommage et le diagramme mondial restent ceux de Cybranian.

Le `LoadFolders.xml` charge la racine, `1.6`, et `Ideology` si ce DLC est actif. Aucun DLC n'est ajouté comme dépendance obligatoire par le complément.

## Apprentissage

Le postfix Cybranian de `Pawn_InteractionsTracker.TryInteractWith` intervient après une interaction réussie entre humanoïdes dont un seul est contrôlé par le joueur. Il choisit le pion contrôlé comme apprenant et la faction de l'autre comme source. Il progresse d'abord dans la langue, puis dans le dialecte. La quantité utilise 3 % du coût de base, un aléa de 0,5 à 1,5 et `CalculateLanguageLearnFactor(Pawn, Pawn)`.

Ce facteur dépend des capacités sociales, de `LanguageLearningAbility`, de `NegotiationAbility`, de la difficulté configurée et de celles des Defs. La surcharge par faction renvoie zéro si langue et dialecte sont déjà globalement appris : intercepter seulement `AddXp` ne suffirait pas pour une individualisation complète.

Le travail `WorkGiver_Warden_LearnLanguage` et le job `JobDriver_LearnPrisonerLanguage` utilisent les interactions de prisonnier et des conversations, dont DeepTalk. Ils n'offrent pas un registre distinct du geôlier ou du prisonnier. L'option `autoLearnRecruiteeLanguage` appelle les méthodes globales `Learn` au recrutement.

## Livres

Le ThingDef `LanguageBook`, dérivé de BookBase, instancie `BookOutcomeProperties_GainLanguageExp` et `BookOutcomeDoerGainLanguageExp`. Les textures, règles de noms, descriptions et stocks des marchands exotiques terrestres/orbitaux sont fournis par Cybranian et restent inchangés.

Signatures vérifiées : `ProgressPerTick(Pawn reader, float factor)`, `DoesProvidesOutcome(Pawn reader)`, `OnReadingTick(Pawn reader, float factor)`, `PostExposeData()`, propriété `Dialect`.

Le dialecte vient de la faction de l'auteur ou d'un tirage pondéré ; il est sauvegardé par référence sous `dialect`. `DoesProvidesOutcome` ne consulte que les maîtrises globales. `OnReadingTick` applique des paquets de 500 ticks lorsque `TicksGame % 500 == 0`, à la langue puis au dialecte si son parent est appris. La progression combine coût de base, facteur de lecture, aptitude du lecteur, qualité du livre et paramètre de difficulté. À noter pour le lot suivant : le livre multiplie par `languageLearnDifficulty`, alors que le calcul de conversation divise par ce réglage. Ne pas unifier silencieusement ces formules.

## Restrictions et malus toujours d'origine au lot 1

- Communications de faction et vaisseaux : `Faction_CommFloatMenuOption` et `PassingShip_CommFloatMenuOption` consultent `FactionLanguageIsLearned`.
- Commerce : prix d'achat/vente, infobulle des prix, `CanTradeNow` des pions, caravanes, colonies et vaisseaux, ainsi que l'incident de caravane marchande. Plusieurs accès n'ont pas de négociateur explicite.
- Prisonniers : `Pawn_GuestTracker.Recruitable` et `SetExclusiveInteraction`. Ce dernier peut refuser recrutement/réduction de résistance/conversion et refuser l'apprentissage d'une langue déjà globalement connue.
- Quêtes : filtres `QuestNode_GetPawn.IsGoodPawn` et `QuestNode_GetFaction.IsGoodFaction`.
- Romance et bagarres : pour le joueur, consultation des maîtrises globales ; autres cas, comparaison des langues des factions.
- Interactions négatives : `CanInteractEachOther(Pawn, Pawn)` et `HaveSameDialect(Pawn, Pawn)` comparent les associations de faction, sans connaissances personnelles.

Les valeurs de réglage des malus, traits, statistique d'apprentissage et préceptes/événements Ideology ne sont pas changés.

## Choix du lot 1 et points de patch

Le premier lot est le socle décrit par AGENTS.md. Aucune méthode d'apprentissage de Cybranian n'est interceptée, aucune projection individuelle vers l'XP globale n'est ajoutée. L'hypothèse de projection évoquée avant la découverte du véritable dossier projet n'est pas retenue.

Deux patchs Harmony ciblent les signatures vérifiées du jeu : `Pawn.SetFaction(Faction, Pawn)` en prefix pour mémoriser l'origine avant changement ; `PawnGenerator.GeneratePawn(PawnGenerationRequest)` en postfix pour initialiser les nouveaux pions en partie. Aucun transpiler ni suppression de patch Cybranian.

Après résolution des Defs, un ThingComp et un ITab sont ajoutés aux races Humanlike. Les hooks `LoadedGame`/`StartedNewGame`, `PostSpawnSetup` et l'ouverture de l'onglet complètent l'initialisation. `Game.FinalizeInit` précède les notifications de début/chargement dans l'assembly inspectée. Les données restent attachées au pion, sans boucle par tick ni cache statique de partie.

Le lot suivant devra traiter conjointement les verrous d'éligibilité des livres, le destinataire de l'XP, les appels de conversations et les consommateurs concernés. Supprimer prématurément le collectif bloquerait des accès encore mondiaux.

## Complément d'audit pour le lot 2

Empreinte de la DLL revérifiée identique le 2026-10-10. Méthodes publiques vérifiées : ProgressPerTick, DoesProvidesOutcome, OnReadingTick et les deux surcharges de CalculateLanguageLearnFactor. Paramètres du livre : reader, factor. Signature du jeu : Pawn_InteractionsTracker.TryInteractWith(Pawn recipient, InteractionDef intDef), champ privé pawn présent pour l'injection Harmony ___pawn.

RimWorld Book.OnBookReadTick(Pawn, int, float) multiplie déjà le facteur par la vitesse de lecture et le nombre de ticks écoulés avant OnReadingTick. Ne pas multiplier à nouveau la progression personnelle par 500 ou par la durée. Le système collectif original reste inchangé.

Le lot 2 ajoute trois postfix : éligibilité du livre, gain personnel de lecture et conversations réussies. Le calcul personnel de conversation reprend les facteurs inspectés mais ne consulte pas la maîtrise collective ; il résout la langue maternelle enregistrée du locuteur. Aucun patch de job supplémentaire ni interception d'AddXp. Voir LEARNING.md pour la politique des prisonniers, événements d'histoire et autoLearnRecruiteeLanguage.

## Audit du lot 3a / 0.3.0

DLL revérifiée le 2026-10-10 : empreinte inchangée. WorkGiver_Warden_LearnLanguage.JobOnThing(Pawn pawn, Thing t, bool forced) contient exactement un FactionDialectIsLearned(Faction), après un test de faction non nulle et avant mode/délai/statut/capacités/réservation/éveil.

Le Postfix de Pawn_GuestTracker_SetExclusiveInteraction_FactionLanguagesPatch a les paramètres (Pawn_GuestTracker __instance, PrisonerInteractionModeDef def, ref Pawn ___pawn, ref PrisonerInteractionModeDef ___interactionMode). Sa branche LearnLanguage contient exactement un FactionDialectIsLearned(Faction), après un test de faction non nulle. Les autres modes utilisent FactionLanguageIsLearned(Faction) et restent intacts.

Le job effectue trois séquences Chitchat, puis DeepTalk, avec ses contrôles d'échec, réservations et délais. Il ne contient pas de nouvelle attribution individuelle à ajouter. Les Defs originales LearnLanguage, LearnLanguagePrisoner et LearnPrisonerLanguage sont réutilisées. Le WorkGiver demande Warden et Talking. IL intermédiaire dans work/prisoner-il.txt, non redistribué.

Deux transpilers remplacent seulement les appels linguistiques identifiés. L'argument de menu est un Pawn par référence, donc ldarg.2 / ldind.ref ; ceux du travail sont pawn et t, donc ldarg.1 / ldarg.2. Le Faction déjà sur la pile est conservé pour le repli. Détails et limites : PRISONER_LEARNING.md.

## Diagnostic des conversations 0.3.1

IL de CalculateLanguageLearnFactor(Pawn, Pawn) revérifié : Cybranian rejette les deux côtés si Social est TotallyDisabled. Le complément avait repris cette condition, ce qui bloque l'exposition à une langue maternelle quand seul le locuteur est incapable de travail social. Le correctif personnel conserve la condition sur l'apprenant et retire celle sur le locuteur.

La Def du jeu Jailbird40 désactive Social et Caring ; cette Def figure dans le passé du prisonnier de Test mod.rws. La Def NegotiationAbility utilise une base 0.4, un bonus Social de 0.075 par niveau et un minimum 0.4. La Def LanguageLearningAbility de Cybranian utilise une base 0, bonus Social 0.065 et minimum 0. Cette différence explique pourquoi un gardien apte peut apprendre d'un locuteur incapable sans que ce dernier apprenne automatiquement en retour. Les stats effectivement calculées en partie restent à confirmer en cas de nouveaux symptômes.

## Diagnostic 0.3.2 — statistique désactivée

Player.log utilisateur avec DLL 0.3.1.0 signale « Attempted to calculate value for disabled stat NegotiationAbility » pour Kin'Wuys. Le complément appelle GetStatValue après avoir retiré son rejet côté locuteur ; Cybranian conserve son rejet préalable. Le contrôle de cohérence du jeu explique cette erreur visible en mode développeur.

Assembly-CSharp.dll inspectée avec Mono.Cecil : StatDef.minValue est public float ; StatWorker.IsDisabledFor(Verse.Thing) est public virtual bool. Son IL consulte neverDisabled, les compétences requises des skillNeedFactors/skillNeedOffsets et disableIfSkillDisabled. Utiliser ce contrôle direct avant GetStatValue permet aussi de respecter les méthodes et patches de compatibilité. Data/Core/Defs/Stats/Stats_Pawns_Social.xml donne à NegotiationAbility un minimum 0,4. La 0.3.2 utilise ce minimum si la statistique est désactivée. Aucun nouveau point Harmony ; Cybranian garde son empreinte précédente.

Précision utilisateur : la diplomatie est présentée comme une sous-tâche de Social. La description du correctif doit viser la statistique de négociation désactivée, sans assimiler tous les travaux sociaux affichés à ce cas.

## API UI du lot 0.4.0

Assembly-CSharp.dll installée inspectée avec Mono.Cecil : Dialog_Slider utilise Widgets.HorizontalSlider mais reçoit et stocke des entiers. Le widget natif accepte Rect, valeur/min/max float, alignement, trois labels et roundTo float ; TextFieldNumeric<T> prend valeur et buffer par référence ainsi que min/max. Window expose InitialSize virtuel, forcePause, absorbInputAroundWindow et closeOnClickedOutside. GenText.Truncate(string, float, cache optionnel) permet de conserver des lignes compactes ; BeginScrollView expose showScrollbars bool. L'éditeur réutilise ces widgets dans une fenêtre décimale propre. Aucun point Harmony ajouté ; Cybranian revérifié avec empreinte inchangée.

## API UI du lot 0.4.1

Capture utilisateur du panneau de sévérité inspectée. Méthode HealthCardUtility.<DrawHediffRow>b__7 : GUI.Button(Rect, TexButton.Save) teinté cyan ouvre Dialog_DebugSetSeverity(Hediff). Cette fenêtre a InitialSize 300 × 125, SetInitialSizeAndPosition virtuel plaçant le panneau à MousePositionOnUIInverted + (4, 0), WindowLayer.Super, fond manuel Widgets.DrawWindowBackground et GUI.HorizontalSlider(Rect, float, float, float) de hauteur 10. Son bouton de hauteur 20 applique sans fermer ; fermeture extérieure activée. La 0.4.1 reprend le rendu et les composants sans simuler ou créer un hediff ; arrondi linguistique à deux décimales. API compilée avec les assemblies installées. Aucun nouveau patch Harmony.
# Réglages 0.14.0 — 2026-10-11

Empreinte de la DLL parent revérifiée : 7053DA2F804BB9D0C9360B9CBF6F422C2DB27F7C67C23F30F510717B4174DD11, inchangée. Son réglage languageLearnDifficulty reste le diviseur de la transmission personnelle. API Listing_Standard.CheckboxLabeled inspectée dans le moteur installé avant remplacement du curseur. Aucun nouveau point Harmony ; le booléen du complément est testé avant sélection des leçons. 242 assertions hors jeu et compilation réussies, aucun moteur exécuté par l'agent.
