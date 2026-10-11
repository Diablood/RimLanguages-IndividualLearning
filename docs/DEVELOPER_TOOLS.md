# Onglet Langues et outils développeur

## Interface compacte

Onglet de 510 × 420 pixels ; chaque entrée occupe 32 pixels : nom, barre et pourcentage à deux décimales. Noms longs tronqués avec infobulle complète ; marqueurs maternels et avertissement pour les entrées indisponibles conservés. Scrollbar seulement si la liste dépasse la hauteur disponible.

## Régler une connaissance

En mode développeur, sélectionner le pion et ouvrir Langues. Le bouton cyan de l'entrée ouvre un panneau de 300 × 125 près de la souris, comme Dialog_DebugSetSeverity : titre, curseur et **Régler à … %**. Deux décimales pour un réglage fin.

Le jeu est en pause pendant l'édition. Seul le bouton applique la valeur ; fermer ou déplacer le curseur sans validation ne change rien. Le panneau reste ouvert après application. Nom complet, pion et effets sur les connaissances liées sont indiqués par les infobulles.

Les deux anciens gizmos ont été retirés. Ce bouton édite les connaissances déjà présentes ; l'ajout utilise l'action ci-dessous. Les outils sont absents hors mode développeur.

## Ajouter à un seul pion

**Actions de debug → Rim Languages → RLI: add language or dialect to pawn**.

Cliquer un pion humanoïde vivant, puis choisir une connaissance absente ou incomplète de la base Cybranian. Elle est mise à 100 % ; les entrées déjà complètes sont omises. Fermer sans choix ne modifie rien.

Ajouter une langue ne complète pas ses dialectes. Ajouter un dialecte complète aussi sa langue parente. Aucun gain collectif, événement Ideology, changement d'origine ou marqueur maternel ajouté. La cible reste le pion capturé au clic, même si la sélection change.

Mode développeur, validité du pion, état Scribe et existence de la cible dans la base sont revérifiés à l'application. Sans registre initialisable, aucune langue arbitraire attribuée.

## Cohérence des éditions

- Dialecte positif : langue parente à 100 %.
- Langue abaissée sous 100 % : ses dialectes présents dans la base remis à zéro.
- Dialecte à zéro : langue conservée.
- Valeurs finies bornées à 0–100 % ; NaN/infinis refusés.
- Origine, identifiants et marqueurs maternels conservés ; seul le registre visé change.

Un dialecte maternel abaissé cesse d'être une source personnelle du chemin prisonnier jusqu'à restauration. Ces règles s'appliquent aux éditions confirmées ; aucun réajustement automatique au chargement. Les niveaux sont sauvegardés dans le schéma 1.

## Diagnostics sans interaction ou événement

**Actions de debug → Rim Languages → RLI: inspect social language factors** : cliquer deux pions. Affiche maîtrise commune, facteurs et calculs, sans déclencher d'interaction. Voir [SOCIAL_INTERACTIONS.md](SOCIAL_INTERACTIONS.md).

**Actions de debug → Rim Languages → RLI: inspect quest and trader language gates** : choisir une faction. Inspecte les trois Postfix avec résultats d'entrée acceptés puis refusés ; pour une langue résolue, attendu True / False. Le filtre de pion utilise le chef si disponible. Ne génère ni quête ni visite : [QUEST_GENERATION.md](QUEST_GENERATION.md).

## Vérification

Le rendu réutilise les composants du jeu inspectés dans Assembly-CSharp.dll, sans créer de hediff ni modifier son interface. Les outils parent restent collectifs et ne sont pas patchés.

Affichage, réglage, reprise des leçons après baisse, retrait des gizmos, ajout individuel et diagnostics confirmés globalement par utilisateur. Les diagnostics ne valident pas toutes les interactions naturelles. Voir [TESTING.md](TESTING.md). Aucun moteur exécuté par l'agent.