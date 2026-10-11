# Recrutement et conversion individuels — 0.5.0

État fonctionnel en 0.16.0. Les comptes de tests ci-dessous décrivent l'introduction du sous-système en 0.5.0 ; l'état des validations actuelles est centralisé dans [TESTING.md](TESTING.md). Recrutement/rechargement, migration et caravane ont été confirmés ultérieurement.

## Règle

Pour un prisonnier dont les données personnelles sont résolues, Recruter, Réduire la résistance et Convertir peuvent être sélectionnés sans maîtrise collective. Le menu ne choisit aucun geôlier. Le travail demande au geôlier réel une maîtrise de 100 % de la langue maternelle personnelle du prisonnier ; le dialecte n'est pas requis, conformément au contrôle de langue seul de Cybranian. Un autre colon compétent ne rend pas le geôlier débutant compétent. La connaissance par le prisonnier de la langue du geôlier ne remplace pas ce seuil dans ce lot.

## Inspection et intégration

DLL installée Cybranian : SHA256 7053DA2F804BB9D0C9360B9CBF6F422C2DB27F7C67C23F30F510717B4174DD11. Sources de preuve locales : work/prisoner-social-base-il.txt et work/prisoner-social-game-il.txt. Signatures vérifiées dans les assemblies effectivement installées.

- Postfix de Pawn_GuestTracker_Recruitable_FactionLanguagesPatch : remplacement de son unique FactionLanguageIsLearned(Faction) par MenuRecruitable(Faction, Pawn). Le refus préalable du résultat vanilla reste intact : aucun recrutement forcé d'un prisonnier inébranlable, non recrutable ou exclu par le jeu.
- Postfix de Pawn_GuestTracker_SetExclusiveInteraction_FactionLanguagesPatch(Pawn_GuestTracker, PrisonerInteractionModeDef, ref Pawn, ref PrisonerInteractionModeDef) : remplacement du seul contrôle de langue, avec prisonnier et mode réels. Le patch existant du contrôle de dialecte pour apprendre reste distinct ; les deux ordres de transformation sont testés.
- Postfix des JobOnThing(Pawn, Thing, bool) de WorkGiver_Warden_Chat et WorkGiver_Warden_Convert : rejette seulement un travail déjà proposé si le geôlier ne maîtrise pas la langue. HasJobOnThing passe par WorkGiver_Scanner.HasJobOnThing, qui appelle JobOnThing virtuellement. Délais, résistance, parole, éveil, réservation, lit et conditions Ideology restent ceux du jeu.
- Prefix de Pawn_InteractionsTracker.TryInteractWith : contrôle aussi RecruitAttempt, BuildRapport et ConvertIdeoAttempt envers un prisonnier, pour une tâche déjà engagée lorsque le niveau du geôlier a changé. Une interaction rejetée retourne false ; le postfix d'apprentissage ne crédite pas les appels dont l'original n'a pas été exécuté.

Les transpilers refusent explicitement une forme inconnue ou plusieurs appels au contrôle attendu. Ils ne remplacent pas LanguageUtility globalement. Source : Source/PrisonerSocial.cs.

## Conservé et limites

Livres, configuration initiale, modes et jobs d'origine, gains collectifs, réglages et autoLearnRecruiteeLanguage restent actifs. L'option d'apprentissage à recrutement conserve uniquement son effet collectif ; aucune distribution personnelle à la colonie ou à la recrue. Le registre et l'origine personnelle restent attachés au pion.

Sans données personnelles exploitables, le contrôle linguistique collectif original est conservé. Le résolveur actuel demande un dialecte marqué maternel à 100 % chez le prisonnier : l'abaisser en mode développeur provoque ce repli jusqu'à restauration. Ne pas modifier ce niveau source pour le parcours principal. Le seuil du geôlier porte uniquement sur la langue parente.

Commerce et communications sont individualisés depuis 0.6.0, facteurs sociaux depuis 0.7.0 et événements Ideology depuis 0.9.0. Les filtres de génération sont adaptés depuis 0.10.0 ; voir les documents correspondants. Ce lot couvre la conversion par geôlier d'un prisonnier, pas les capacités ou rituels de conversion ni les interactions avec des non-prisonniers. Aucun champ sauvegardé ajouté ; schéma 1 inchangé. Les validations de migration ancienne partie et de changement de faction restent distinctes.

## Preuves

Compilation 0.5.0, 79 assertions hors jeu et contrôles XML, FR/EN, versions et exclusion des DLL tierces réussis. Les 18 nouvelles assertions couvrent les deux transformations supplémentaires sur l'IL installé, la conservation des autres instructions et métadonnées de branche/exception, les formes incompatibles, la composition avec l'apprentissage, le seuil individuel et la résolution des deux cibles de travail. Ces tests n'exécutent pas Harmony dans Unity, les jobs ou le moteur du jeu. Parcours manuel dans TESTING.md ; Test OK utilisateur reçu pour 0.5.0 le 2026-10-11 ; régressions complémentaires non confirmées séparément.


## Confirmation utilisateur 0.5.0

Le 2026-10-11, utilisateur : « Test OK » après livraison de la 0.5.0 et proposition du parcours court recrutement/résistance/conversion. Validation générale du lot en jeu par utilisateur ; aucun test en jeu exécuté par l'agent. Les régressions complémentaires (baisse pendant une tâche, collectif inconnu, refus vanilla, origine après recrutement, migrations et caravanes) ne sont pas confirmées séparément par ce retour. Version conservée 0.5.0, aucune recompilation ni copie nécessaire.
