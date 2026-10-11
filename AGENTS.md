# Instructions du projet

## Avant le développement

- Lire `README.md`, `docs/PROJECT_STATE.md` et `docs/ROADMAP.md`.
- L'utilisateur demande explicitement d'utiliser son skill existant `rimworld-mod-developer`. Retrouver et lire son véritable `SKILL.md` avant d'écrire le code du mod. Le résumé historique ne remplace pas ce fichier. S'il est inaccessible, expliquer ce blocage et demander le fichier.
- Pour les vérifications de livraison, utiliser aussi `rimworld-release-qa` s'il est accessible ; ne pas prétendre l'avoir appliqué sans lecture.
- Inspecter les assemblies ou sources de la version effectivement installée de Cybranian avant de choisir les points de patch Harmony. Ne pas inventer de classes, méthodes ou signatures.

## Périmètre

- Cibler RimWorld 1.6.
- Créer un complément autonome à Cybranian — Rim Languages, sans dépendance à GateRim SG-1.
- Conserver les fonctionnalités du mod de base lorsque possible ; l'intégration effective reste à vérifier dans son code.
- Stocker les connaissances par pion et assurer leur persistance lors des changements de faction, des caravanes et des sauvegardes.
- Afficher les niveaux linguistiques dans une interface par pion.
- Prévoir une politique explicite pour les sauvegardes existantes ; ne pas distribuer silencieusement tout le savoir de faction à tous les pions.
- Ne pas désactiver l'apprentissage global avant que les accès individuels concernés soient effectivement intégrés et testés.

## Méthode

- Garder les décisions, procédures et limites dans les fichiers du projet.
- Réutiliser les composants du mod de base plutôt que dupliquer ses livres ou son écran initial.
- Privilégier des patchs ciblés et expliquer les hypothèses restantes.
- Distinguer vérifications statiques, compilation et tests en jeu. Ne jamais déclarer une validation non réalisée.
- Fournir les sources complètes et une procédure de test pour chaque lot testable.
- Ne pas embarquer les DLL du jeu ni redistribuer le mod de base.
- Ne pas publier de release, tag, Workshop ou dépôt externe sans instruction utilisateur.
- Les archives locales sont exclues du suivi Git. Ne pas inclure de fichier `*_MANIFEST.txt`.

## Premier lot

Après inspection de l'API : données individuelles persistantes, attribution initiale d'une langue maternelle et interface consultable. L'apprentissage et les restrictions sociales viennent ensuite si le premier lot ne peut pas être intégré complètement de manière fiable.
