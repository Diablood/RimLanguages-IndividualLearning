# Historique des versions

## 1.1.2 — 2026-10-22 — pions sans origine linguistique exploitable

- Repli individuel étendu aux pions humanoïdes vanilla et moddés : faction d'origine prioritaire, sinon langue existante de la même race et l'un de ses dialectes, à 100 % comme maternels. Exception humaine CreepJoiner conservée ; sans paire compatible, initialisation en attente.
- Tirage différé pendant GeneratePawn pour éviter l'attribution avant affectation de la véritable faction ; contexte transitoire restauré même en génération imbriquée ou en cas d'exception.
- Registres initialisés et acquis conservés. Lors d'une naissance prise en charge sans identité maternelle ou de faction résolue, le repli attribue une identité à 0 %, sans maîtrise adulte. Aucun changement du schéma 1.
- Compilation 1.6 et contrôles XML/FR/EN/versions réussis ; 25 assertions d'origine avec chacun des parents 1.6 et 1.7, 18 assertions de transmission. Suite complète non réexécutée (blocage Unity 1.7 historique). Aucun test moteur par l'agent ; nouveaux arrivants sans faction validés en jeu par utilisateur, sauvegarde/rechargement non détaillé séparément. Eduard 1.1.1 déjà validé par utilisateur.

## 1.1.1 — 2026-10-22 — origine des arrivants mystérieux d'Anomaly

- Cas Eduard Quill : registre vide non initialisé, origine Entités occultes sans langue dans Cybranian ; aucune perte démontrée à la capture.
- Repli strict CreepJoiner si la langue de faction n'est pas exploitable : une langue humaine existante puis un de ses dialectes sont tirés au sort par pion, maternels à 100 % et persistants.
- Réparation des pions existants non initialisés au chargement ; acquis et registres initialisés conservés. Aucune langue inventée ou attribuée globalement aux entités, schéma 1 inchangé.
- Compilation 1.6 réussie, 12 assertions ciblées hors jeu et contrôles XML/FR/EN/versions réussis. Parcours Eduard validé en jeu par utilisateur (« test OK pour Eduard ») ; sauvegarde/rechargement non détaillé séparément dans ce retour. Aucune publication.

## 1.1.0 — 2026-10-19 — écoute passive et transmission maternelle

- Paquet installable local contrôlé et textes de mise à jour FR/EN préparés après accord pour poursuivre. Aucun nouveau build ou test moteur lors de la consolidation ; aucune publication distante.

- Parcours Mendez validé par l'utilisateur : écoute d'une langue maternelle enseignée par un colon recruté d'origine différente des fondateurs. Persistance après sauvegarde/rechargement ensuite validée ; refus des langues acquises encore en observation manuelle prolongée ; aucun moteur exécuté par l'agent.

- Les pions dont Social est désactivé peuvent apprendre en écoutant, à la moitié de la base de vitesse d'un apprenant actif d'aptitude nulle. Ils ne peuvent toujours pas enseigner.
- Dans une même colonie/faction, seules les langues et dialectes maternels de l'enseignant, maîtrisés à 100 %, se transmettent naturellement. Restriction également appliquée aux soins et jeux avec bébé.
- Livres, leçons prisonniers et échanges entre factions différentes conservés. Aucune modification des fréquences sociales, tâches vanilla, acquis ou du schéma 1.
- Compilation avec les références 1.6 locales réussie ; 18 assertions ciblées et contrôles XML/FR/EN/versions réussis. Suite complète bloquée hors jeu par UnityEngine.CoreModule absent dans la distribution 1.7 ; aucun résultat en jeu revendiqué.

## Compatibilité RimWorld 1.7 — 2026-10-14

- « Test OK » utilisateur après mise à jour de la copie Mods. Finalisation GitHub demandée avec tag distinct v1.0.0-rw1.7 et paquet de compatibilité ; tag et ZIP 1.0.0 d'origine préservés.
- Commit 3d4aefd, tag et release v1.0.0-rw1.7 publiés ; téléchargement du paquet identique par SHA256 à l'archive locale. Aucun envoi Workshop par l'agent.

- RimWorld 1.7 ajouté aux versions prises en charge, en conservant 1.6.
- LoadFolders.xml charge explicitement le dossier 1.6 pour les deux versions du jeu ; aucune copie ou divergence du code.
- Fonctionnement sur la branche instable confirmé par l'utilisateur. Installation observée : 1.7.4977 rev1248 ; aucune validation en jeu par l'agent.
- Aucun changement des connaissances, des clés de sauvegarde ou du schéma 1. Publication GitHub/Workshop non effectuée pour ce lot.
- DLL et version 1.0.0 conservées : compilation bloquée par la nouvelle organisation .NET 10 de la 1.7, hors du workflow de compilation Mono 1.6 actuel. Le build interrompu n'a pas remplacé la DLL existante.

## 1.0.0 — 2026-10-11 — première version stable

- Passage à la version stable demandé par l'utilisateur, sans changement des comportements de la 0.17.0 ni du schéma de sauvegarde 1.
- Preview originale style RimWorld avec yeux, livre, bulles linguistiques et bandeau 1.6 ; incluse dans le paquet installable.
- Difficulté et calculs de Cybranian conservés. Parcours principaux, événements naturels et interruption d'un soin ou jeu avec bébé validés par l'utilisateur pendant le développement.
- Tag GitHub v1.0.0 et release stable publiés avec le ZIP installable ; téléchargement de contrôle identique par SHA256. Workshop traité séparément, étape par étape. Preuves de reconstruction et de publication dans docs/TESTING.md et docs/PROJECT_STATE.md.

## 0.17.0 — 2026-10-11 — affichage individuel

- Liste des factions : noms de langue/dialecte conservés, progression collective et accès au diagramme par cette ligne retirés ; infobulle renvoyant à Langues du pion.
- Deux appels de texte d'XP collective des conversations masqués ; calculs et gains collectifs, textes vanilla et bulle de transmission personnelle conservés.
- Aucun changement du schéma 1, des niveaux, livres, configuration initiale ou réglages. Diagramme parent conservé pour ses autres chemins.
- Compilation et 277 assertions hors jeu réussies, XML/FR/EN/versions vérifiés. Utilisateur « Tests OK » le 2026-10-11 pour le parcours principal proposé ; variantes facultatives non détaillées séparément. Aucun moteur exécuté par l'agent.

Les vérifications ci-dessous sont datées de chaque lot. Les attentes initiales sont remplacées par les confirmations ultérieures ; l'état actuel des validations est centralisé dans [docs/TESTING.md](docs/TESTING.md).

## Documentation — 2026-10-11

- Présence du projet sur le Workshop vérifiée manuellement par l'utilisateur ; confirmation consignée avant l'envoi GitHub demandé des dernières modifications. Version et release 1.0.0 conservées.

- Identifiant Workshop 3817893569 fourni par l'utilisateur, conservé dans About/PublishedFileId.txt et liens Steam ajoutés aux documents. Page non accessible à l'outil web ; visibilité et contenu non confirmés. Aucun changement runtime, version, archive GitHub ou publication par l'agent.

- Préparation de la copie 1.0.0 dans Mods pour publication Workshop par l'utilisateur : neuf fichiers identiques au ZIP GitHub, preview incluse, backup précédent. deploy.ps1 copie désormais le paquet complet ; descriptions FR/EN et patchnotes fournies. Aucun changement runtime, version ou test moteur, aucun envoi Steam par l'agent.

- Preview originale ajoutée dans About/Preview.png : pions stylisés RimWorld, livre et bulles linguistiques, bandeau 1.6. Incluse dans les futurs paquets ; version 0.17.0 conservée pendant la préparation précédant la version finale demandée.

- Métadonnées About actualisées et URL GitHub ajoutée ; version 0.17.0 et DLL inchangées. Nouveau paquet local contrôlé : 277 assertions hors jeu et huit contenus d'archive vérifiés.
- Arrivées naturelles de marchands/quêtes et interruption d'un soin ou jeu avec bébé validées par l'utilisateur. Conservation des calculs de difficulté de Cybranian confirmée ; mise à jour GitHub des sources demandée, sans tag, release ou Workshop.

- Préparation locale d'un ZIP installable par package.ps1 et notice FR/EN. Reconstruction 0.17.0, 277 assertions hors jeu et huit fichiers d'archive vérifiés par SHA256. Runtime et version inchangés, aucune release publiée.

- Rattachement au dépôt public fourni par utilisateur, conservation du commit initial et de sa licence MIT. Premier envoi des sources en 0.16.0 ; reconstruction et 269 assertions hors jeu réussies, sans release ou tag.
- Retrait de CONTINUER_SUR_CODEX.md et CODEX_START.md, redondants avec les consignes et l'état courant.

- Choix du dépôt futur : RimLanguages-IndividualLearning, public. Licence MIT ajoutée pour le code et la documentation originaux selon délégation utilisateur ; droits des dépendances conservés.
- Consolidation de README, état, roadmap, reprise, procédures et documents des sous-systèmes autour de 0.16.0.
- Retrait des anciennes attentes de parcours validés et clarification des replis collectifs et des variantes facultatives.
- Préparation GitHub : contribution, exclusions, attributs texte et modèle de bug.
- Aucun changement runtime, version, réglage ou déploiement ; aucun nouveau test en jeu revendiqué.

## 0.16.0 — 2026-10-11 — jeux avec bébé

- Tests OK utilisateur le 2026-10-11 pour le parcours proposé jeu, case désactivée et persistance. Validation générale du parcours principal ; variantes facultatives non détaillées séparément, aucun moteur exécuté par l'agent. Version conservée, documents uniquement ; remplace l'attente initiale ci-dessous.

- Transmission personnelle en fin de jeu sur place, avec jouets ou en promenade, selon l'augmentation nette du besoin de jeu. Aucun gain avant préparation ou sans jeu effectif ; anti-doublon par travail.
- Même formule que les soins, adulte Social actif, difficulté parent et case existante. Activités, loisirs, pensées et éducation vanilla conservés ; schéma 1 et enfants existants inchangés.
- Compilation et 269 assertions hors jeu/cohérence réussis, installation locale avec cinq SHA256 vérifiés et sauvegarde précédente. Parcours principal confirmé par utilisateur ; variantes facultatives distinctes. Aucun moteur exécuté par l'agent ni publication.

## 0.15.1 — 2026-10-11 — démarrage Harmony

- Précision utilisateur le 2026-10-11 : test enfant effectué et validé. Parcours principal naissance/nourrissage/persistance/éducation validé globalement, en plus du démarrage. Variantes facultatives non détaillées séparément ; aucun moteur exécuté par l'agent. Attentes initiales remplacées par ce retour.

- Correctif de chargement validé par utilisateur (« Test OK », 2026-10-11). Parcours enfants non détaillé séparément. Version conservée, documentation uniquement ; attente initiale de validation du démarrage remplacée par ce retour.

- Correction de l'erreur au chargement de 0.15.0 : PregnancyUtility initialisée avant disponibilité des DefOf lors de PatchAll dans le constructeur Mod. Application déplacée dans le Bootstrap après résolution des Defs.
- Connaissances, soins, éducation et schéma inchangés. Compilation et 262 assertions hors jeu/cohérence réussies, installée localement avec cinq fichiers SHA256 vérifiés et sauvegarde précédente.
- Redémarrage complet sans erreur et lot enfants encore à confirmer en jeu. Journal utilisateur inspecté, aucun moteur exécuté par l'agent ni publication.

## 0.15.0 — 2026-10-11 — nouveau-nés et soins

- Identité maternelle personnelle enregistrée à 0 % à la naissance Biotech. Repli de faction si identité maternelle indisponible, aucun ancien registre réinitialisé.
- XP personnelle en fin d'allaitement/nourrissage, proportionnelle à la nourriture reçue. Adulte Social actif, bébé dispensé de Social, difficulté parent et activation existante conservées. Jeux différés.
- Éducation, compétences, croissance et passions vanilla inchangées ; langue lente ne bloque pas la formation. Schéma personnel 1 inchangé.
- Compilation et 260 assertions hors jeu/cohérence réussies ; copie Mods et sauvegarde précédente vérifiées. Tests en jeu attendus, aucun moteur exécuté par l'agent ni publication.

## 0.14.0 — 2026-10-11 — difficulté unique

- Tests OK utilisateur le 2026-10-11 pour le parcours proposé de la case, arrêt/reprise des gains et conservation après relance. Validation générale ; variantes de migration non détaillées séparément, aucun moteur exécuté par l'agent. Version conservée, documents uniquement ; attente initiale du test principal remplacée par ce retour.

- Curseur de vitesse retiré, remplacé par « Activer la transmission par conversation ». Difficulté réglée uniquement chez Cybranian ; facteurs Social et fréquence sociale conservés.
- Migration des options : ancien 0 % désactivé, positif activé ; multiplicateur abandonné. Nouvelle clé booléenne prioritaire, seul ce réglage est écrit. Connaissances personnelles et schéma 1 inchangés.
- Compilation et 242 assertions hors jeu/XML/FR/EN/version réussis. Installée localement avec sauvegarde précédente et comparaison SHA256 des cinq fichiers. Aucun moteur exécuté par l'agent ; test d'option et relance encore attendu, aucune publication.

## 0.13.0 — 2026-10-11 — bulle sociale de transmission

- Tests OK utilisateur le 2026-10-11 pour le parcours principal proposé : rendu, arrêt à 0 % et sauvegarde/rechargement. Variantes non détaillées séparément ; aucun moteur exécuté par l'agent. Version conservée, documents uniquement ; ce retour remplace l'attente initiale ci-dessous.

- Texte chiffré provisoire retiré. Symbole de la bulle sociale existante teinté en cyan après gain automatique réel ; forme, symbole et durée conservés, pas de deuxième bulle. Repli habituel sans bulle exploitable.
- Capture du résultat exact dans un contexte transitoire nettoyé même après exception et isolé entre appels imbriqués. Aucun aléa, nouvelle priorité ou interaction ; formule, réglage et schéma 1 inchangés.
- Compilation et 236 assertions hors jeu, XML/FR/EN/version réussis. Tests de rendu en jeu attendus ; aucun moteur exécuté par l'agent.

## 0.12.1 — 2026-10-11 — texte de transmission individuelle

- Tests OK utilisateur pour le parcours proposé, reçus le 2026-10-11. Texte numérique cyan provisoire : retrait demandé pour version finale, bulle d'interaction dédiée si possible ou bulle habituelle. Remplacement à auditer et implémenter ; version inchangée, documents uniquement.

- Texte flottant cyan sur l'apprenant après un gain automatique positif : nom de langue/dialecte et incrément effectivement reçu, après plafond. Petits gains explicités. Aucun message lors d'une simple édition ou du chargement.
- Aléa visuel isolé, formule et fréquence inchangées, textes collectifs conservés. Aucun nouveau champ sauvegardé. Compilation et 229 assertions hors jeu, XML/FR/EN/version réussis ; rendu encore à valider en jeu.
- Utilisateur confirme transmission et effet de Social en 0.12.0 ; signale fréquence perçue élevée et gains parfois sans texte. Arrêt à 0 %, options et autres cas non confirmés séparément. Aucun moteur exécuté par l'agent.

## 0.12.0 — 2026-10-11 — transmission automatique

- Chitchat/DeepTalk réussies peuvent transmettre une langue ou un dialecte appris à 100 %. Cible choisie automatiquement, continuité puis connaissance maternelle, aléa entre équivalents. Une cible par sens, choix avant les gains. Aucune tâche ou priorité d'interaction ajoutée.
- Social requis chez les deux pions et vitesse selon leurs aptitudes ; niveau 0 actif admissible avec base lente. Pas de facteur Intellectuel ajouté. Curseur du complément 0–300 %, défaut 100 %, persistant via ModSettings. Lecture et apprentissage prisonnier conservés.
- Schéma 1, registres existants, événements Ideology et XP collective conservés. Compilation et 226 assertions hors jeu, XML/FR/EN/version réussis. Tests en jeu attendus ; aucun moteur exécuté par l'agent.

## 0.11.0 — 2026-10-11 — ajout à une sauvegarde existante

- Test OK utilisateur reçu pour le parcours principal d'ajout et sauvegarde/rechargement le 2026-10-11. Caravanes, transports et faction non confirmés séparément. Version inchangée, documents seulement ; ce retour remplace l'attente initiale ci-dessous.

- Initialisation au chargement étendue aux pions déjà en caravane et transport. Ajout sur une partie ayant Cybranian sans nouveau départ ni édition du fichier de sauvegarde.
- Attribution limitée à la langue et au dialecte de la faction observée ; pas d'import collectif. Registres individuels existants conservés, même après baisse développeur ou changement de faction. Schéma 1 inchangé, aucun événement de gain lors de la migration.
- Audit du chargement de l'assembly installée et 11 assertions supplémentaires : compilation, 207 assertions hors jeu et cohérence XML/FR/EN/version réussies. Parcours principal de migration confirmé par retour utilisateur ; caravanes non confirmées séparément, aucun moteur exécuté par l'agent.

## 0.10.0 — 2026-10-11 — quêtes et visites marchandes

- Trois filtres de génération diffèrent le rejet linguistique collectif aux actions individuelles lorsque la faction possède une langue valide. Quêtes et visites peuvent se présenter sans maîtrise collective, même sans pion parlant encore cette langue.
- Refus vanilla, autres conditions et paramètres d'échange conservés. Aucun savoir distribué ; repli original si la langue n'est pas résolue. Schéma 1 et XP collective inchangés.
- Diagnostic développeur RLI: inspect quest and trader language gates ; inspecte les Postfix réels sans créer de quête ou visite. Aucun gizmo.
- Compilation et 196 assertions hors jeu, XML/FR/EN/version réussis. Tests oK utilisateur reçus le 2026-10-11 pour le diagnostic ; apparitions réelles et régressions non confirmées séparément. Aucun moteur exécuté par l'agent.

## 0.9.0 — 2026-10-11 — événements personnels et portabilité

- Expérience et maîtrise Ideology selon la progression du pion ; préceptes et pensées du parent conservés. Notifications collectives filtrées dans les deux AddXp pour éviter les doublons, XP globale inchangée.
- Aucun événement pour une édition développeur, initialisation ou chargement. Aucun nouveau champ sauvegardé ; schéma 1 inchangé. Repli original sans DLC ou registre résolu.
- Compilation, 178 assertions hors jeu, XML/FR/EN/versions réussis ; Tests OK utilisateur le 2026-10-11 pour le parcours principal après clarification du précepte requis. Aucun moteur exécuté par l'agent, variantes non confirmées séparément.
- Documents altérés corrigés, script fautif supprimé et chemins personnels retirés. Scripts avec détection des dépendances ou paramètres explicites ; première installation et remplacement vérifiés dans work.

## 0.8.0 — 2026-10-11 — ajout développeur individuel

- Action RLI: add language or dialect to pawn : cliquer un pion puis choisir une connaissance absente ou incomplète, indépendamment du collectif.
- Ajout à 100 % ; dialecte complétant sa langue parente. Réglage par bouton cyan. Aucun doublon, marqueur maternel, gizmo ou gain collectif ajouté.
- Schéma 1 inchangé. Compilation, 160 assertions existantes hors jeu et XML/FR/EN/versions réussis. Test OK utilisateur le 2026-10-11. Aucun moteur exécuté par l'agent.

## 0.7.0 — 2026-10-11 — facteurs sociaux individuels

- Romance, interactions négatives et bagarres selon une langue et un dialecte communs maîtrisés par les deux pions.
- Facteurs originaux conservés, sans cumul ni double application collective. Zéros vanilla et repli conservés.
- Diagnostic RLI: inspect social language factors sans interaction ni gizmo.
- Compilation et 160 assertions hors jeu, schéma 1 inchangé. Test OK utilisateur.

## 0.6.0 — 2026-10-11 — commerce et communications

- Prix et infobulles selon TradeSession.playerNegotiator ; radio selon le pion choisi. Contrôles CanTradeWith et TryExecute sur l'acteur réel.
- Réglages et facteurs d'origine conservés, arrivées marchandes encore collectives.
- Compilation et 134 assertions hors jeu. Tests OK utilisateur.

## 0.5.0 — 2026-10-11 — prisonniers

- Recrutement, résistance et conversion exigent la langue personnelle du prisonnier à 100 % chez le geôlier réel ; dialecte facultatif.
- Refus vanilla et autoLearnRecruiteeLanguage collectif conservés.
- Compilation et 79 assertions hors jeu. Test OK utilisateur.

## 0.4.2 — 2026-10-11 — retrait des gizmos

- Retrait des deux gizmos et de leur menu à la demande utilisateur ; bouton cyan conservé.
- Compilation et 61 assertions hors jeu ; retrait confirmé par utilisateur. Aucun changement de sauvegarde ou d'apprentissage.

## 0.4.1 — 2026-10-11 — réglage comme les hediffs

- Panneau contextuel 300 × 125, curseur et bouton Régler à … % à deux décimales. Validation laissant le panneau ouvert, fermeture sans appliquer possible.
- Compilation et 61 assertions hors jeu. Test OK utilisateur ; reprise après baisse confirmée.

## 0.4.0 — 2026-10-11 — interface compacte et édition

- Lignes de 32 pixels et scrollbar conditionnelle. Édition développeur des niveaux personnels et cohérence parent/dialectes.
- Deux gizmos initialement fournis, retirés ensuite en 0.4.2.
- Compilation et 61 assertions hors jeu. Test OK utilisateur, schéma 1 inchangé.

## 0.3.2 — 2026-10-10 — négociation désactivée

- Worker.IsDisabledFor vérifié avant GetStatValue du locuteur, minValue pour la statistique désactivée afin d'éviter l'erreur développeur.
- Compilation et 47 assertions hors jeu. Absence de l'erreur non confirmée séparément.

## 0.3.1 — 2026-10-10 — locuteur sans Social

- Un locuteur sans Social peut exposer à sa langue maternelle lors d'une conversation réussie ; contrôle Social de l'apprenant conservé.
- Compilation et 47 assertions hors jeu. Gains, passage au dialecte et persistance confirmés ; fin des leçons à 100 % confirmée ensuite.

## 0.3.0 — 2026-10-10 — apprentissage auprès des prisonniers

- Choix apprendre la langue et travail selon les niveaux personnels du geôlier, même après maîtrise collective.
- Travail et conversations du parent réutilisés ; réservations et conditions vanilla conservées.

## 0.2.0 — 2026-10-10 — versionnement

- Numéro intégré à About et DLL pour identifier le lot dans RimSort ; détection automatique confirmée.
- KnowledgeTests.exe remplacé ensuite par une DLL exécutée via PowerShell pour éviter les alertes Avast.

## Lots initiaux — 2026-10-10

- Audit du parent ; connaissances individuelles persistantes, langue et dialecte maternels, onglet Langues.
- Livres et conversations d'origine réutilisés. XP collective conservée, sans distribution silencieuse de savoir.
- Socle, lecture, comparaison de fiches et sauvegarde/rechargement confirmés par utilisateur.

## Documentation — 2026-10-11

- Correction de cinq documents altérés par un remplacement PowerShell de caractères. Script fautif supprimé, textes reconstruits à partir des preuves et confirmations utilisateur. Aucun changement du code, de la version ou du mod installé.
