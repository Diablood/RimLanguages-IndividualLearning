# Contribuer

Lire [AGENTS.md](AGENTS.md), [l'état actuel](docs/PROJECT_STATE.md) et [la roadmap](docs/ROADMAP.md) avant une modification.

## Construire et vérifier

Les scripts PowerShell utilisent RimWorld 1.6, Harmony et Cybranian installés sur le poste. Les tests nécessitent Mono.Cecil. Aucun téléchargement automatique ni DLL propriétaire fournie. Détection et paramètres : [BUILDING.md](docs/BUILDING.md).

```powershell
./build.ps1
./test.ps1
```

Le déploiement dans Mods est une opération séparée, jeu fermé : ./deploy.ps1. Ne pas le confondre avec une publication.

## Proposer une modification

- Vérifier les signatures dans la version installée avant tout patch Harmony.
- Préserver defNames, types sérialisés et clés Scribe ; expliquer toute migration.
- Préserver les comportements vanilla et réutiliser les composants de Cybranian.
- Maintenir les clés FR/EN correspondantes et les libellés compréhensibles.
- Fournir un parcours manuel court et distinguer tests hors jeu, tests exécutés en jeu et résultats rapportés.
- Mettre à jour les documents concernés et le changelog pour un lot fonctionnel. Ne pas empiler des états actuels contradictoires.
- Garder les chemins publics neutres. Ne pas joindre de DLL du jeu ou du parent, sauvegardes personnelles ou archives locales.

Pour signaler un problème, préciser versions, ordre des mods, étapes de reproduction, résultat attendu et observé. Joindre seulement l'extrait utile du journal, après retrait des informations personnelles.

## Licence et publication

Le code et la documentation originaux du complément sont sous [licence MIT](LICENSE). Les forks et la réutilisation sont permis sous ses conditions, notamment conservation des notices. Le code, les données et les assets des dépendances restent soumis à leurs propres conditions.

Dépôt externe, tag, release et Workshop nécessitent une instruction explicite du propriétaire. Voir [GITHUB_SETUP.md](docs/GITHUB_SETUP.md) et [RELEASE.md](docs/RELEASE.md).
