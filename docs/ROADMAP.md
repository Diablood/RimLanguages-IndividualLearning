# Roadmap — 1.0.0

## Réalisé

- Audit de Cybranian installé et socle individuel persistant avec onglet Langues.
- Livres et apprentissage auprès des prisonniers, puis restrictions individuelles du geôlier.
- Commerce, communications et facteurs sociaux selon les pions concernés.
- Interface compacte, réglage développeur comme les hediffs et ajout individuel.
- Événements Ideology personnels et adaptation des filtres de génération.
- Ajout sur une partie ayant déjà Cybranian ; persistance confirmée après recrutement et en caravane.
- Transmission automatique intégrée aux conversations ordinaires, présentation dans la bulle existante et difficulté unique.
- Biotech : naissance, nourrissage et jeux avec bébé, sans bloquer l'éducation vanilla.
- Consolidation documentaire et préparation locale des fichiers pour GitHub.
- Paquet installable local et notice FR/EN : reconstruction, tests et vérification SHA256 du ZIP automatisés par package.ps1, sans publication.
- Métadonnées About actualisées et sources 0.17.0 envoyées sur GitHub le 2026-10-11 ; commit 758c2a9 vérifié sur main, sans release, tag ou Workshop.

Parcours principaux jusqu'à 0.17.0 validés par l'utilisateur ; portée et variantes dans [TESTING.md](TESTING.md). Première version stable : 1.0.0, mêmes comportements et schéma 1. Aucun moteur exécuté par l'agent.

## Prochaine étape

Première version finale 1.0.0 et tag GitHub demandés par l'utilisateur le 2026-10-11. Preview créée avant le changement de version : About/Preview.png, style RimWorld avec yeux et bandeau 1.6, PNG 640 × 640 de 862 346 octets. Workshop à traiter ensuite étape par étape ; aucune publication Workshop autorisée par cette préparation.

Liste des factions sans niveaux collectifs et conversations sans textes d'XP collective : parcours principal acquis, ne pas le redemander. Aucun retrait de la XP globale avant audit des replis ; voir [COLLECTIVE_DISPLAY.md](COLLECTIVE_DISPLAY.md). Suite : choisir le prochain lot ou préparer les vérifications ciblées d'une première release lorsqu'elle sera demandée.

Le dépôt public [RimLanguages-IndividualLearning](https://github.com/Diablood/RimLanguages-IndividualLearning) a été créé par l'utilisateur, sous MIT. Le premier envoi du projet conserve son commit initial de licence. Procédure et exclusions dans [GITHUB_SETUP.md](GITHUB_SETUP.md). Suite : préparer les vérifications ciblées d'une première release lorsqu'elle sera demandée.

## Avant une première publication

- Vérifier les variantes utiles encore non confirmées, en ciblant les systèmes qui auront changé ; ne pas refaire systématiquement tous les parcours acquis.
- Arrivées naturelles de quêtes et marchands et interruption d'un soin ou jeu avec bébé : validées par l'utilisateur le 2026-10-11, ne pas redemander sans changement pertinent.
- Examiner la compatibilité avec d'autres mods sociaux, de naissance ou de soins selon le périmètre annoncé.
- Vérifier les attributions et la provenance des assets, puis constituer le paquet avec uniquement les fichiers du complément ; licence MIT pour son code et sa documentation originaux.
- Reconstruire, vérifier versions/XML/traductions et suivre [RELEASE.md](RELEASE.md).

L'utilisateur confirme le 2026-10-11 la conservation de la difficulté et des calculs du parent, y compris la différence entre lecture et conversation. Toute modification des valeurs par défaut ou de la fréquence doit être discutée ; aucun deuxième curseur de vitesse prévu.

## Évolutions facultatives

- API publique stable pour les autres mods, notamment GateRim, sans dépendance inverse.
- Extension aux chemins de naissance remplacés par d'autres mods après audit de leurs API.
- Interprètes : reportés à une éventuelle mise à jour, sans implémentation prévue actuellement.
- Capsules de transport : hors du parcours de test demandé, non bloquantes pour cette étape.

Ces idées ne constituent pas des lots autorisés ou des engagements de livraison.
