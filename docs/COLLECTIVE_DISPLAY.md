# Affichage individuel — 0.17.0

## Liste des factions

La langue et le dialecte attribués à une faction restent des informations utiles pour identifier ses interlocuteurs. En revanche, sa progression collective ne représente pas les connaissances des pions.

Le Postfix FactionUIUtility_DrawFactionRow_FactionLanguagesPatch du parent est remplacé uniquement pour son affichage, lorsque l'association de langue est résolue. Une ligne compacte indique les noms, sans XP, pourcentage ou statut collectif « appris ». L'infobulle renvoie vers Langues du pion. Son ancien clic ouvrant le diagramme collectif est retiré de cette ligne.

La hauteur supplémentaire reste de 30 pixels. Le placement part de la hauteur de ligne vanilla, inspectée à 80 pixels ; les états GUI, police et alignement sont restaurés après dessin. Les paramètres indexés évitent de confondre le quatrième argument ref float avec un résultat de méthode : le Postfix ciblé renvoie void.

Sans faction, base ou association exploitable, repli sur l'affichage parent. La configuration initiale et DrawLanguagesDiagram ne sont pas patchés : le diagramme peut rester accessible par d'autres chemins du parent ou de mods tiers. Ce lot ne supprime pas ses données.

## Textes d'XP collective

Le Postfix Pawn_InteractionsTracker_TryInteractWith_FactionLanguagesPatch contient deux appels MoteMaker.ThrowText(Vector3, Map, string, float), pour la langue et le dialecte collectifs. Un transpiler remplace uniquement ces appels par une méthode sans affichage, de même signature ; toutes les autres instructions sont conservées, y compris conditions et AddXp.

Un nombre d'appels différent de deux produit une erreur de compatibilité explicite. Étiquettes et blocs d'exception sont conservés. Aucun patch global de MoteMaker, aucune suppression des textes de résistance, recrutement, compétences ou d'autres mods. La bulle sociale de transmission personnelle reste utilisée.

Le texte supprimé n'étant plus créé, ses tirages d'aléa visuels n'ont plus lieu. Les formules d'XP et les poids/fréquences des interactions ne sont pas modifiés ; une partie n'est pas garantie de suivre exactement les mêmes tirages ultérieurs qu'avant.

## Conservé et preuves

XP collective, sauvegardes, paramètres, langues/dialectes, livres, autoLearnRecruiteeLanguage et replis inchangés. Aucun champ Scribe ajouté, schéma 1. Les niveaux personnels restent la référence ; masquer l'affichage ne signifie pas supprimer le système collectif.

Assemblies effectivement installées inspectées le 2026-10-11. Empreinte Cybranian inchangée : 7053DA2F804BB9D0C9360B9CBF6F422C2DB27F7C67C23F30F510717B4174DD11. Relevés locaux sous work/collective-display-base-il.txt et work/faction-row-game-il.txt, exclus de Git.

Compilation 0.17.0 et 277 assertions hors jeu réussies. Huit assertions supplémentaires vérifient remplacement des seuls appels, conservation des instructions de gain et métadonnées, signature de pile, refus des formes inconnues et signature réelle du Postfix de faction. Utilisateur « Tests OK » le 2026-10-11 pour le parcours principal de liste des factions et conversation avec un prisonnier ; variantes facultatives non détaillées séparément. Preuves et procédure de référence : [TESTING.md](TESTING.md). Aucun moteur exécuté par l'agent.
