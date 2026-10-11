# Apprentissage auprès des prisonniers

## Comportement

L'ordre original « Apprendre la langue » reste sélectionnable si une langue maternelle du prisonnier est résolue. Le menu ne choisit pas de geôlier ; le mode peut rester sélectionné lorsque tous les gardiens ont terminé.

Le travail est proposé au gardien dont la langue ou le dialecte maternel du prisonnier est encore incomplet. Le savoir collectif ne décide plus de cette éligibilité lorsque les registres sont exploitables. L'origine du prisonnier reste celle enregistrée avant un changement de faction.

Ce job instruit le gardien ; il n'est pas planifié seulement pour enseigner au prisonnier. Une fois langue et dialecte à 100 %, aucun nouveau travail d'apprentissage pour ce gardien. Une tâche déjà commencée peut finir ; le plafond empêche un dépassement. Les repas, soins et autres tâches de gardien restent indépendants.

## Patchs ciblés

Signatures vérifiées dans la DLL installée de Cybranian ; voir [AUDIT_CYBRANIAN.md](AUDIT_CYBRANIAN.md).

- Transpiler du Postfix Pawn_GuestTracker_SetExclusiveInteraction_FactionLanguagesPatch : remplace son unique appel FactionDialectIsLearned par le contrôle personnel adapté au menu.
- Transpiler de WorkGiver_Warden_LearnLanguage.JobOnThing(Pawn pawn, Thing t, bool forced) : remplace l'unique appel FactionDialectIsLearned par le besoin du gardien réel.
- Aucun patch du JobDriver_LearnPrisonerLanguage ni duplication de Def ou de job. Les échanges réussis donnent les gains ; ni la sélection du mode ni la création du job.

Les autres instructions et étiquettes sont conservées ; une forme IL inattendue produit une erreur de compatibilité. Les critères vanilla restent appliqués : prisonnier sécurisé, mode, délai, éveil, déplacement, parole, réservation et prise en charge.

Recrutement, résistance et conversion ont leurs contrôles individuels distincts : [PRISONER_SOCIAL.md](PRISONER_SOCIAL.md).

## Social et calcul

Social doit être actif chez l'apprenant. Pour ce chemin maternel, un prisonnier dont Social est désactivé peut néanmoins fournir une exposition ; cette exception ne s'applique pas à la transmission ordinaire entre adultes.

NegotiationAbility n'est évaluée chez le locuteur que si Worker.IsDisabledFor le permet. Sinon, le minimum de sa Def est utilisé (0,4 dans les définitions auditées). Une sous-tâche Diplomatie désactivée ne signifie pas que tout Social est désactivé.

Facteurs d'aptitude, négociation, difficultés et coûts conservés ; une aptitude d'apprentissage nulle peut donner zéro. Langue avant dialecte, pas de rattrapage des conversations passées.

## Replis et sauvegardes

Sans registre ou langue maternelle exploitable, contrôle collectif original. Les gardes de faction non nulle restent présentes. Un dialecte maternel abaissé sous 100 % par édition développeur cesse d'être une source personnelle de ce chemin jusqu'à restauration ; le repli collectif peut alors intervenir.

Aucun champ de job ajouté. Mode prisonnier sauvegardé par le jeu/parent ; niveaux dans le schéma individuel 1. Collectif original conservé.

Gains, passage au dialecte, persistance, fin des nouvelles leçons et reprise après baisse confirmés par utilisateur. L'absence du diagnostic NegotiationAbility désactivée n'est pas confirmée séparément. Voir [TESTING.md](TESTING.md).