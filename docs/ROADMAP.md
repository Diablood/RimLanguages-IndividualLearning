# Roadmap — 0.16.0

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

Parcours principaux validés par l'utilisateur ; portée et variantes dans [TESTING.md](TESTING.md). Version de développement actuelle : 0.16.0.

## Prochaine étape

Le dépôt public [RimLanguages-IndividualLearning](https://github.com/Diablood/RimLanguages-IndividualLearning) a été créé par l'utilisateur, sous MIT. Le premier envoi du projet conserve son commit initial de licence. Procédure et exclusions dans [GITHUB_SETUP.md](GITHUB_SETUP.md). Suite : préparer les vérifications ciblées d'une première release lorsqu'elle sera demandée.

## Avant une première publication

- Vérifier les variantes utiles encore non confirmées, en ciblant les systèmes qui auront changé ; ne pas refaire systématiquement tous les parcours acquis.
- Observer les arrivées naturelles de quêtes et marchands, au-delà du diagnostic de filtres.
- Examiner la compatibilité avec d'autres mods sociaux, de naissance ou de soins selon le périmètre annoncé.
- Vérifier les attributions et la provenance des assets, puis constituer le paquet avec uniquement les fichiers du complément ; licence MIT pour son code et sa documentation originaux.
- Reconstruire, vérifier versions/XML/traductions et suivre [RELEASE.md](RELEASE.md).

L'équilibrage utilise la difficulté du parent. Toute modification des valeurs par défaut ou de la fréquence doit être discutée ; aucun deuxième curseur de vitesse prévu.

## Évolutions facultatives

- API publique stable pour les autres mods, notamment GateRim, sans dépendance inverse.
- Extension aux chemins de naissance remplacés par d'autres mods après audit de leurs API.
- Interprètes : reportés à une éventuelle mise à jour, sans implémentation prévue actuellement.
- Capsules de transport : hors du parcours de test demandé, non bloquantes pour cette étape.

Ces idées ne constituent pas des lots autorisés ou des engagements de livraison.
