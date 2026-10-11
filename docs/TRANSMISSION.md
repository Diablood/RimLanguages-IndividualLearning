# Transmission automatique

## Intégration aux conversations

Après Pawn_InteractionsTracker.TryInteractWith réussi et exécuté par le moteur, Chitchat et DeepTalk peuvent transmettre une connaissance personnelle. Aucun job, InteractionDef, poids de sélection, fréquence ou priorité sociale ajouté.

Les autres interactions, notamment insultes, bagarres, RecruitAttempt, BuildRapport et ConvertIdeoAttempt, ne déclenchent pas elles-mêmes ce chemin. Le job LearnPrisonerLanguage, qui emploie DeepTalk, garde sa formule maternelle distincte sans seconde attribution automatique. Les interactions d'autres mods nécessitent un audit.

## Connaissance choisie par le pion

Le locuteur doit maîtriser la cible à 100 %, maternelle ou apprise, et celle-ci doit être incomplète chez l'apprenant. Une langue peut être enseignée sans dialecte. Un dialecte exige sa langue parente à 100 % chez les deux pions et le dialecte complet chez le locuteur.

Le choix privilégie une connaissance commencée, puis une connaissance maternelle du locuteur, puis une autre connaissance maîtrisée. Aléa entre cibles de même priorité. Cela choisit le contenu après l'interaction ; aucune priorité sur les interactions vanilla.

Une seule entrée progresse par sens, sans débordement au dialecte. Les deux cibles sont choisies avant les gains : aucune maîtrise acquise dans l'échange n'est immédiatement retransmise. Aléa isolé avec Rand.PushState/PopState ; aucun choix sauvegardé ou apprentissage par proximité.

## Social et vitesse

Social doit être présent et actif chez les deux adultes. Niveau 0 admissible avec une base lente ; TotallyDisabled interdit le gain. Une sous-tâche Diplomatie désactivée ne suffit pas à déduire que Social entier l'est. Aucun facteur Intellectuel supplémentaire.

LanguageLearningAbility du parent inclut déjà Social de l'apprenant et ses modificateurs. Le calcul évite de le compter deux fois :

```text
XP = coût de base × 0,03
     × (0,5 + aptitude linguistique de l'apprenant)
     × (0,5 + 0,065 × Social du locuteur, niveau borné à 0–20)
     × variation 0,5–1,5
     / difficulté globale du parent
     / difficulté de la langue
     / difficulté du dialecte, si cible dialecte

progression = XP / XpMax, plafonnée à 100 %
```

Coûts/difficultés invalides et facteurs non finis ne donnent rien. NegotiationAbility n'intervient pas dans ce chemin. À paramètres identiques, difficulté parent 200 % divise par deux l'XP automatique de référence à 100 %. La lecture conserve le calcul original du parent, qui emploie la difficulté différemment ; voir [LEARNING.md](LEARNING.md).

## Réglage unique

Options → Paramètres des mods → Rim Languages — Individual Learning : **Activer la transmission par conversation**, activé par défaut. Aucun second curseur de vitesse.

Décocher suspend les gains automatiques personnels, y compris [soins et jeux avec bébé](CHILDREN.md). Lecture, job prisonnier et collectif restent indépendants. Migration des anciens paramètres : [SAVE_MIGRATION.md](SAVE_MIGRATION.md).

La fréquence des interactions n'a pas été modifiée. Le rythme a été jugé rapide lors des premiers essais ; la difficulté de Cybranian reste la référence de réglage. Aucun résultat de calibration uniforme ni nouveau défaut imposé.

## Prisonniers et recrutement

Les conversations admissibles peuvent transmettre au prisonnier la langue du gardien pendant le recrutement, en parallèle de la réduction de résistance. Après recrutement, le nouveau colon garde son registre et peut continuer à apprendre ; sa nouvelle faction ne complète pas automatiquement la langue.

Les anciens textes d'XP du parent décrivaient des points collectifs, parfois affichés sur le pion non contrôlé ; ils sont masqués depuis 0.17.0, sans suppression des gains collectifs. La fiche Langues reste la référence. Apprentissage après recrutement et sauvegarde/rechargement confirmés par utilisateur. Voir [COLLECTIVE_DISPLAY.md](COLLECTIVE_DISPLAY.md).

## Présentation

Après un gain automatique réel dans une conversation, le symbole de sa bulle sociale existante est teinté en cyan. Même bulle, forme et durée, sans seconde bulle, texte chiffré ou infobulle de survol ajoutée. La teinte signale un gain, pas sa langue ou son sens. Sans bulle exploitable, affichage habituel ; les textes de conversation collectifs du parent sont masqués depuis 0.17.0.

Un Prefix ouvre le contexte de la paire TryInteractWith, le Postfix MakeInteractionBubble capture la bulle/symbole, puis le gain déclenche le marquage. Finalizer nettoie même après exception ; contextes imbriqués isolés, aucun champ sauvegardé.

La teinte concerne l'icône, pas le fond. Les bulles masquées restent masquées ; un autre mod remplaçant la bulle peut empêcher le marquage. Les soins/jeux ne créent pas de nouvelle notification.

Les gains passent par Gain et LearningHistory, sans nouveau marqueur maternel ni gain au chargement. Schéma 1 inchangé. Parcours principaux et options confirmés par utilisateur ; variantes dans [TESTING.md](TESTING.md). Aucun moteur exécuté par l'agent.
