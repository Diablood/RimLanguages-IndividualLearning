# Préparation du dépôt GitHub

## État

Dépôt public créé par utilisateur : [Diablood/RimLanguages-IndividualLearning](https://github.com/Diablood/RimLanguages-IndividualLearning). Dépôt local initialisé, origin configuré, branche main fondée sur le commit distant 09f6b13. Premier envoi des sources du complément ; aucun tag, release ou Workshop publié.

Premier envoi vérifié le 2026-10-11 : commit 89eeb0c, 59 fichiers, référence distante identique. Visibilité publique et branche main confirmées par l'API GitHub. Les mises à jour documentaires suivantes restent séparées des releases du mod.

Choix du 2026-10-11 : dépôt **RimLanguages-IndividualLearning**, nom accepté par utilisateur, visibilité **publique**, licence **MIT** choisie selon la délégation utilisateur et ajoutée dans LICENSE. Le packageId du mod est conservé.

## Contenu à suivre

Sources C#, traductions FR/EN, About et icône du complément, LoadFolders.xml, scripts, tests, consignes et documentation. Les DLL sont compilées localement à partir des dépendances installées ; elles ne font pas partie du dépôt de sources.

.gitignore exclut les DLL/PDB, références et dépendances, bin/obj, artifacts, work, archives locales et *_MANIFEST.txt. .gitattributes normalise les fichiers texte et identifie les images comme binaires. Le modèle de signalement de bug est sous .github/ISSUE_TEMPLATE.

La licence MIT du commit GitHub initial est conservée, y compris la notice choisie par l'utilisateur. Elle couvre le code et la documentation originaux du complément et ne change pas les droits de RimWorld, Cybranian ou Harmony. Aucun fichier de licence trouvé dans la copie installée de Cybranian ; ne pas lui attribuer MIT. Les droits des assets doivent rester distincts de ceux du code.

## Pour les envois suivants

1. Vérifier les fichiers et le diff avant commit ; aucune sauvegarde, information locale, référence propriétaire ou archive.
2. Reconstruire et exécuter les vérifications pertinentes ; vérifier les versions et documenter les résultats réels.
3. Vérifier origin, envoyer les commits puis comparer la référence distante au commit local.
4. Mettre à jour PROJECT_STATE et les procédures concernées. Ne pas réinitialiser le dépôt distant ni forcer l'envoi pour remplacer son historique.

La création du dépôt n'autorise pas automatiquement une release, un tag ou le Workshop. Leur procédure est distincte : [RELEASE.md](RELEASE.md).

Pour un fork, suivre [BUILDING.md](BUILDING.md), renseigner les dépendances de son poste et compiler la DLL du complément. Les chemins personnels du poste d'origine ne sont pas nécessaires.
