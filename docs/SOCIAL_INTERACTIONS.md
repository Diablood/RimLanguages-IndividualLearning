# Romance, interactions négatives et bagarres — 0.7.0

État fonctionnel en 0.16.0. Les comptes de tests et retours 0.7.0 ci-dessous sont historiques ; validations actuelles et cas facultatifs dans [TESTING.md](TESTING.md).

## Règle du couple

Pour deux pions dont les registres sont résolus, une langue disponible maîtrisée à 100 % par chacun suffit à supprimer le facteur de langue. Il peut s'agir de la langue maternelle de l'un ou d'une troisième langue apprise par les deux. Un dialecte commun maîtrisé à 100 % supprime aussi le facteur de dialecte, à condition que sa langue parente soit maîtrisée par les deux. Une langue ou un dialecte à 99,99 % ne suffit pas.

Sans langue commune : facteur de langue de Cybranian. Avec langue commune mais sans dialecte commun : facteur de dialecte. Avec les deux : facteur 1. Les facteurs ne se cumulent pas. Cette règle est symétrique et vaut pour les humanoïdes, quelle que soit leur faction ; l'opinion, la compatibilité, les traits, l'âge et les autres critères du jeu restent directionnels et inchangés.

Les calculs concernés sont le facteur d'interactions négatives, le facteur secondaire de romance et la chance de bagarre après une interaction. Aucun nouveau malus d'opinion permanent ni blocage général des conversations. Aucun apprentissage gratuit à l'inspection ; la progression existante continue via les interactions réussies et les livres.

## API effectivement inspectée

Cybranian installé, SHA256 revérifié : 7053DA2F804BB9D0C9360B9CBF6F422C2DB27F7C67C23F30F510717B4174DD11. IL local : work/social-base-il.txt.

- NegativeInteractionUtility_NegativeInteractionChanceFactor_FactionLanguagesPatch.Postfix(Pawn initiator, Pawn recipient, ref float result) : compare CanInteractEachOther et HaveSameDialect, puis multiplie par negativeInteractionChanceFactorLanguage ou negativeInteractionChanceFactorDialect.
- Pawn_RelationsTracker_SecondaryRomanceChanceFactor_FactionLanguagesPatch.Postfix(Pawn pawn, Pawn otherPawn, ref float result) : acteur joueur utilisant le savoir collectif de l'autre faction ; autre acteur utilisant la comparaison de factions. Facteurs secondaryRomanceChanceFactorLanguage et secondaryRomanceChanceFactorDialect.
- Pawn_InteractionsTracker_SocialFightChance_FactionLanguagesPatch.Postfix(Pawn pawn, InteractionDef interaction, Pawn initiator, ref float result) : même distinction joueur/autres, facteurs socialFightChanceLanguage et socialFightChanceDialect. Le premier pion est le destinataire, le second celui qui a initié l'interaction.

Source/SocialLanguages.cs applique un prefix à chacun de ces trois Postfix de Cybranian. Lorsque le couple personnel est résolu, le prefix applique le facteur puis saute uniquement ce calcul linguistique original pour éviter un double malus. Le résultat du jeu est déjà calculé ; un résultat nul, négatif ou NaN reste inchangé. Aucun recalcul du résultat vanilla et aucune division par le facteur collectif (un réglage nul reste possible). En cas de données indisponibles, le Postfix original s'exécute.

Les alias Harmony __0/__1/__2 ou __0/__2/__3 correspondent aux indices des arguments du Postfix original. Son paramètre nommé ___pawn n'est pas injecté comme champ, et le ref float ne doit pas être confondu avec le résultat de cette méthode void. Les signatures et les six champs de réglage sont vérifiés sur les DLL installées. LanguageUtility n'est pas patché globalement.

## Diagnostic développeur

Mode développeur, menu des actions de debug : catégorie Rim Languages, entrée **RLI: inspect social language factors**. Cliquer un premier pion puis un second. Le panneau et le journal [RLI social] affichent la langue/dialecte communs maîtrisés, les trois multiplicateurs et les résultats des méthodes du jeu : NegativeInteractionChanceFactor, SecondaryRomanceChanceFactor et SocialFightChance après Insult. Les trois méthodes sont publiques dans Assembly-CSharp.dll installé.

Cet outil consulte les calculs avec leurs patchs actifs ; il ne lance aucune interaction, aucun tirage de bagarre, aucune tentative de romance, ni gain. Les facteurs négatifs et de romance ne sont pas des probabilités absolues ; seule la ligne bagarre est formatée comme une chance. Un résultat zéro peut provenir d'une impossibilité vanilla et doit rester zéro. Si les réglages valent tous 1, changer les niveaux ne produit logiquement aucun effet sur les valeurs. Les outils du jeu gèrent la sortie du curseur ; changer de carte annule la sélection. Aucun gizmo ajouté.

## Conservation et limites

Livres, configuration, registres, apprentissage et collectif conservés. Schéma 1 inchangé ; aucun nouveau champ sauvegardé. Les entrées supprimées de la base ne constituent pas une langue commune. Sans deux registres initialisés ou base disponible, repli sur Cybranian.

Le calcul des conséquences sociales utilise les connaissances communes. La transmission ordinaire, intégrée depuis 0.12.0, peut enseigner une connaissance maternelle ou apprise maîtrisée, choisie automatiquement : voir TRANSMISSION.md. Le job prisonnier garde son chemin maternel distinct ; commerce/radio conservent leur cible de faction.

Les événements Ideology sont personnels depuis 0.9.0 et les filtres de quêtes/visites marchandes sont adaptés depuis 0.10.0. Pas de modification des interactions sociales indépendantes de ces trois facteurs ni des règles de consentement/éligibilité du jeu. Les effets existants de Cybranian sont réutilisés, sans garantir une romance ou une bagarre.

## Preuves

Compilation 0.7.0 et 160 assertions hors jeu réussies, dont 26 nouvelles couvrant langue étrangère ou tierce commune, symétrie, seuil, dialecte, tiers inchangés, édition, facteurs non cumulés, zéro et réductions configurés, signatures et alias Harmony, champs de réglage originaux. XML, FR/EN, versions et exclusion des DLL tierces vérifiés. Aucun KnowledgeTests.exe lancé. Ces tests n'exécutent pas les prefixes dans Unity ni l'outil graphique. Test OK utilisateur reçu le 2026-10-11 pour le diagnostic selon le parcours proposé ; aucun moteur lancé par l'agent.


## Confirmation utilisateur 0.7.0

Le 2026-10-11, utilisateur : « Test OK » après livraison de la 0.7.0 et proposition du diagnostic social avec trois situations de maîtrise commune. Validation générale du parcours proposé en jeu par utilisateur ; aucun moteur exécuté par l'agent. Les interactions naturelles, pions non joueurs, inversions du couple, réglages nuls et migrations ne sont pas confirmés séparément par ce retour. Version conservée 0.7.0 ; mise à jour documentaire uniquement, aucune recompilation ni copie.
