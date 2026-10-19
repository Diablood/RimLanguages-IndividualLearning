# État du projet — 2026-10-19

## Version actuelle

**1.1.0 locale**, DLL **1.1.0.0**, schéma **1**, nouveau lot du 2026-10-19. Parcours Mendez validé par l'utilisateur : apprentissage par écoute d'une langue maternelle de l'enseignant, lui-même recruté et d'origine différente des colons fondateurs. La règle vise bien l'identité maternelle de chaque enseignant, pas la langue de fondation de la colonie. Persistance après sauvegarde/rechargement ensuite validée par l'utilisateur. Refus de transmission d'une langue acquise encore en observation manuelle prolongée ; filtre déjà couvert par les assertions ciblées.

Réception passive autorisée si Social est TotallyDisabled, à la moitié de la base d'un apprenant actif d'aptitude nulle ; enseignement toujours interdit sans Social. Dans une même faction non nulle, les conversations et soins/jeux avec bébé transmettent uniquement les connaissances maternelles de l'enseignant, maîtrisées à 100 %. Un dialecte exige aussi sa langue parente maternelle et maîtrisée ; langue parente maîtrisée également chez l'apprenant. Autres méthodes explicites et échanges entre factions différentes inchangés, aucun acquis retiré.

Compilation réussie grâce aux références locales krafs.rimworld.ref 1.6.4871/net472 et Harmony 1.6 ; pas de compilation ciblant exclusivement .NET 10. build.ps1 accepte ManagedReferencesDir ; test.ps1 sépare références de compilation et assemblies de runtime, avec mode PolicyOnly explicite. 18 nouvelles assertions de politique exécutées avec succès sur la DLL produite, XML/FR/EN/versions/exclusion DLL tierces vérifiés. Suite complète tentée mais bloquée par UnityEngine.CoreModule manquant dans le runtime 1.7 lors des tests de patchs de commerce ; aucun succès complet ni nouveau test moteur revendiqué. DLL SHA256 54B53BD1DD3200774B1B397410AD07C5D644AE70D65C3B7595B1202259450331.

Dernière release avec paquet public inchangée : v1.0.0-rw1.7. L'utilisateur demande ensuite le commit/push et le tag v1.1.0 le 2026-10-19. Cette demande concerne les sources ; aucune nouvelle release GitHub ou mise à jour Workshop incluse dans cette étape. La politique ne modifie ni les tâches, ni leur fréquence, ni le schéma, ni les niveaux existants ; les connaissances étrangères déjà commencées restent disponibles pour lecture et autres voies.

Envoi effectué le 2026-10-19 : commit 7204f23 sur main, tag annoté [v1.1.0](https://github.com/Diablood/RimLanguages-IndividualLearning/tree/v1.1.0), références distantes vérifiées par git ls-remote. DLL, ZIP, journaux et sauvegardes exclus du commit. Aucune release GitHub avec asset ni mise à jour Workshop créée ; le paquet 1.1.0 reste disponible localement.

Consolidation locale du 2026-10-19 après accord pour poursuivre : paquet installable 1.1.0 préparé avec la DLL testée, neuf contenus contrôlés par SHA256, XML/FR/EN/versions vérifiés. Descriptions Workshop FR/EN, patchnotes et [notes de release 1.1.0](RELEASE_NOTES_1.1.0.md) préparées. Refus des langues non maternelles laissé en observation pendant les parties, sans redemander une longue session dédiée ; parcours Mendez et persistance acquis. Aucun nouveau build, test moteur ou envoi distant pendant cette consolidation.

Archive locale 1.1.0 : RimLanguages-IndividualLearning-1.1.0-20261019-233722-20bab5cc.zip, 906 549 octets, SHA256 37C3531BA98A7C238C89AA6DF08008EAA6326DC3FE2077D2183290334FC62C0B. Archive et staging exclus de Git. Aucune DLL tierce, sauvegarde ou PublishedFileId inclus dans le ZIP.

Copie locale 1.1.0 installée dans Mods, jeu fermé : neuf fichiers comparés par SHA256, identifiant Workshop 3817893569 conservé séparément. Première copie recréée puis consolidation de la description About ; backup de cette copie sous work/runtime-before-1.1.0-20261019-232413-444. La version publique reste accessible séparément via Steam ; sélectionner explicitement la copie locale 1.1.0 pour le test. Aucun nouveau résultat moteur revendiqué.

Compatibilité 1.7 ajoutée le 2026-10-14 à la demande utilisateur : About annonce 1.6 et 1.7, LoadFolders route les deux versions vers / et 1.6. Fonctionnement sur la branche instable confirmé par l'utilisateur ; installation observée 1.7.4977 rev1248. Aucun changement C#, schéma ou DLL. Version 1.0.0 conservée pour cette déclaration ; SHA256 original de la DLL vérifié. Première préparation locale sans publication ; finalisation GitHub ensuite demandée, décrite ci-dessous. Aucun envoi Workshop par l'agent.

Retour du 2026-10-14 : « Test OK » utilisateur pour la copie de compatibilité, puis commit/push/tag et finalisation autorisés. Tag retenu : v1.0.0-rw1.7, distinct du v1.0.0 initial et cohérent avec la DLL 1.0.0 conservée. Notes de publication dans [RELEASE_NOTES_RW1.7.md](RELEASE_NOTES_RW1.7.md). Package préparé en mode explicite de réutilisation de DLL validée : SHA256, versions, XML, FR/EN et neuf contenus du ZIP vérifiés ; aucune reconstruction ou nouvelle assertion de connaissances revendiquée.

Archive de compatibilité : RimLanguages-IndividualLearning-1.0.0-rw1.7-20261014-233036-60c801da.zip, 906 342 octets, SHA256 83CC06182E48C8791846FBAD562CD5D056106706E9C594F6E7099233343686BC. PublishedFileId exclu du ZIP d'installation et conservé dans la copie d'envoi Mods.

Finalisation GitHub effectuée le 2026-10-14 : commit 3d4aefd, tag annoté v1.0.0-rw1.7 et [release de compatibilité](https://github.com/Diablood/RimLanguages-IndividualLearning/releases/tag/v1.0.0-rw1.7). API : draft=false, prerelease=false, asset RimLanguages-IndividualLearning-1.0.0-rw1.7.zip de 906 342 octets. Digest distant et téléchargement de contrôle identiques au SHA256 local. Version du mod 1.0.0, tag v1.0.0 et archive originale conservés. Dernière étape externe : l'utilisateur envoie la copie Mods sur l'article Workshop existant si ce n'est pas déjà fait ; aucun envoi Steam par l'agent.

Compilation tentée mais bloquée : Harmony a déplacé Current/Assemblies vers 1.6/Assemblies et 1.7/Assemblies ; détection corrigée selon Version.txt. Le jeu 1.7 expose maintenant les assemblies à la racine et un runtime net10.0/WaylithCLR ; le workflow build/test Mono 1.6 ne trouve pas RimWorldWin64_Data/Managed/mscorlib.dll. Aucune reconstruction ni exécution des 277 assertions avec la 1.7 revendiquée. XML, routage de versions et conservation de la DLL contrôlés ; anciens résultats 1.6 conservés comme preuves historiques.

Copie d'envoi recréée dans Mods, jeu fermé : neuf fichiers déployés et comparés par SHA256, DLL originale conservée. PublishedFileId 3817893569 restauré séparément et vérifié pour mettre à jour l'article existant. Aucun envoi Steam effectué. Notes de mise à jour dans docs/workshop/Patchnotes-Compatibility-1.7.txt ; preview 1.6 existante conservée.

**1.0.0**, DLL **1.0.0.0**, schéma individuel **1**. Première version stable demandée par l'utilisateur, avec tag GitHub v1.0.0. Aucun changement du gameplay depuis la 0.17.0 validée : version et preview uniquement. Reconstruction et 277 assertions hors jeu réussies ; neuf contenus du paquet contrôlés, preuves dans [TESTING.md](TESTING.md). Aucun moteur exécuté par l'agent.

Paquet 1.0.0 : RimLanguages-IndividualLearning-1.0.0-20261011-164334-dbea0a60.zip, 906 137 octets, SHA256 743FB9E27F7B92DA97F15559ECC7003030F0C5D7653872330A72FF801882280E. DLL SHA256 27C9C3F5803D4FBD1A418ECFC4B4E3012F780E35C0FFF56FAECF1E1F051B175D. Notes de release FR/EN dans [RELEASE_NOTES_1.0.0.md](RELEASE_NOTES_1.0.0.md).

Préparation Workshop du 2026-10-11 : 1.0.0 installée dans `<RimWorld>/Mods/RimLanguages-IndividualLearning`, jeu fermé. deploy.ps1 copie désormais les neuf fichiers du paquet (preview, icône, licence et notice comprises), avec backup et vérification SHA256. Dossier installé contrôlé : exactement neuf fichiers, tous identiques aux contenus du ZIP de release. Copie précédente sauvegardée sous work/runtime-before-1.0.0-20261011-165302-225. Aucun nouveau build ni test moteur ; l'utilisateur publiera lui-même depuis le jeu. Description FR/EN et patchnotes dans [WORKSHOP.md](WORKSHOP.md), aucun envoi Steam effectué par l'agent.

Publication GitHub effectuée le 2026-10-11 : [release stable 1.0.0](https://github.com/Diablood/RimLanguages-IndividualLearning/releases/tag/v1.0.0), tag annoté v1.0.0 sur le commit ecf60dd. L'API confirme draft=false et prerelease=false ; asset RimLanguages-IndividualLearning-1.0.0.zip de 906 137 octets, digest identique au paquet local. Téléchargement de contrôle effectué, SHA256 identique. Aucun envoi Workshop ; aucun PublishedFileId créé.

Rattachement Workshop du 2026-10-11 : l'utilisateur fournit le PublishedFileId créé dans sa copie installée, **3817893569**. Identifiant conservé dans About/PublishedFileId.txt du projet, lien ajouté au README et à [WORKSHOP.md](WORKSHOP.md) : [page Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=3817893569). L'utilisateur confirme ensuite avoir vérifié manuellement la présence du projet sur le Workshop. L'outil web n'a pas pu accéder à la page ; description et éléments requis non vérifiés séparément par l'agent. Aucun envoi ni changement Steam par l'agent, aucune modification de version, DLL ou release GitHub.

Consolidation des sources demandée le 2026-10-11 : envoi GitHub des textes Workshop, de l'identifiant de publication, du déploiement complet et de leurs documents. Aucun nouveau tag ni modification de la release 1.0.0 ; preuves historiques conservées et DLL inchangée.

Installée jeu fermé le 2026-10-11 : cinq fichiers vérifiés par SHA256, sauvegarde précédente sous work/runtime-before-0.17.0-20261011-161011-896. DLL livrée : 72BCBFEB77F71A0877768CBB2FACC96C2FDEF67FA4AA0F92B22B4CEABB331BCB. Aucun test en jeu exécuté.

Le parcours principal 0.16.0 a reçu « Tests OK » utilisateur : gain après jeu, transmission désactivée et sauvegarde/rechargement. Les variantes facultatives ne sont pas confirmées séparément. Aucun moteur exécuté par l'agent.

Sources 0.17.0, métadonnées et empaquetage envoyés sur GitHub le 2026-10-11 à la demande utilisateur : commit 758c2a9358c8a5e2d9d73371669727c076f15298, référence distante main vérifiée identique par git ls-remote. Aucun tag, release ou Workshop créé. Aucun moteur exécuté par l'agent.

Dernière consolidation du 2026-10-11 : ajout de package.ps1 et d'une notice INSTALLATION.md FR/EN, préparation d'une archive locale installable 0.17.0. Reconstruction et 277 assertions hors jeu réussies ; huit fichiers du ZIP vérifiés par SHA256. DLL inchangée par rapport à l'installation validée. Version 0.17.0 conservée, aucun nouveau déploiement ni test en jeu. Archive et staging sous artifacts/packages, exclus de Git ; aucune release distante.

Archive historique 0.17.0 après actualisation des métadonnées : RimLanguages-IndividualLearning-0.17.0-20261011-162953-7eb54b98.zip (43 460 octets). SHA256 : B70C6234E36673F124E89063486004ED36D6215C24ECF26FEDD4194A7384C6CC. Reconstruction et 277 assertions hors jeu réussies ; huit contenus d'archive vérifiés, DLL inchangée. Aucun nouveau déploiement.

## Fonctionnalités intégrées

- Registres persistants par pion, origine maternelle et onglet Langues compact.
- Lecture individuelle des livres d'origine ; langue puis dialecte, plafonnés à 100 %.
- Ordre prisonnier original adapté au besoin du geôlier ; arrêt des nouveaux travaux une fois les deux niveaux complets.
- Contrôles du geôlier et du négociateur réels ; facteurs sociaux d'après les connaissances communes.
- Événements Ideology personnels utilisant les préceptes et pensées du parent.
- Filtres de quêtes et visites marchandes différant le contrôle linguistique vers les actions individuelles.
- Transmission de connaissances maîtrisées lors de Chitchat/DeepTalk, sans modifier leur fréquence ou leur priorité. Social requis chez les deux pions, vitesse selon leurs aptitudes ; pas de facteur Intellectuel supplémentaire.
- Une seule difficulté, celle du parent ; case d'activation du complément.
- Avec Biotech, identité maternelle à 0 % à la naissance prise en charge, apprentissage après nourrissage et jeu, éducation vanilla conservée.
- Édition développeur précise et ajout individuel, sans les deux anciens gizmos.

## Conservé du parent et encore collectif

Livres, Defs, langues/dialectes, écran initial, paramètres, facteurs et préceptes réutilisés. La XP collective et les utilitaires du parent restent actifs pour la compatibilité et les replis. Le diagramme n'est pas supprimé, mais son accès par la ligne de faction et les niveaux affichés dans cette ligne sont retirés en 0.17.0. L'option autoLearnRecruiteeLanguage garde son effet collectif ; elle ne distribue aucun savoir personnel.

Les contrôles individuels utilisent le comportement original si le pion, le registre ou la langue cible ne sont pas exploitables. Les menus sans acteur reportent le contrôle lorsqu'une langue valide est résolue ; les restrictions vanilla restent appliquées.

La lecture réutilise le calcul du parent, dont la difficulté intervient différemment de celle des conversations. Ne pas annoncer un ralentissement uniforme de tous les apprentissages en augmentant cette jauge.

## Validations acquises et limites

La synthèse des confirmations utilisateur et des vérifications hors jeu se trouve dans [TESTING.md](TESTING.md). Ne pas remettre les parcours acquis en attente : socle, livres, progression prisonnier et dialecte, fin/reprise des leçons, interface, ajout développeur, contrôles individuels, migration, transmission, recrutement/rechargement, caravane et parcours principal enfants.

Le 2026-10-11, l'utilisateur a validé les arrivées naturelles de marchands et quêtes ainsi que l'interruption d'un soin ou jeu avec bébé. Cette confirmation ne détaille pas séparément chaque moment d'interruption ni le rechargement au milieu de l'activité. Certains replis et les interactions avec d'autres mods restent distincts. Les capsules de transport ont été explicitement écartées du parcours actuel : non testées et non bloquantes.

## Décisions et suite

Incident RimSort du 2026-10-19 pendant la préparation du test 1.1.0 : journal utilisateur RimSort.1.log, crash _load_no_version_warning avec AttributeError ('str' object has no attribute 'get'). Cache externe NoVersionWarning/1.7/ModIdsToFix.xml vérifié vide (70 octets, racine sans entrée) ; le code RimSort applique .get à cette racine convertie en chaîne. Erreur avant le parcours des mods, pas une exception de la DLL RLI. Contournement local réversible : ce seul fichier vide renommé ModIdsToFix.xml.disabled-20261019-233017 dans son dossier, original préservé et chemin chargé absent. Aucun changement de liste de mods, paramètres RimSort ou sauvegardes ; redémarrage de RimSort et test en jeu encore à confirmer par l'utilisateur. Une actualisation de la base externe peut réintroduire le fichier vide tant que le parseur RimSort ne le gère pas.

Analyse du 2026-10-19 : utilisateur signale un colon secouru avec abasie puis recruté, d'origine linguistique différente, sans apprentissage ni difficulté visible. Pion ensuite identifié comme Mendez, registre et antécédents inspectés en lecture seule. Parent 1.6 réellement installé revérifié (SHA256 7053DA2F804BB9D0C9360B9CBF6F422C2DB27F7C67C23F30F510717B4174DD11) : CanInteractEachOther/HaveSameDialect résolvent les langues par faction actuelle ; RecruitUtility_Recruit peut apprendre langue/dialecte du recruté dans le collectif via autoLearnRecruiteeLanguage ; les gains de conversation du parent excluent les paires ayant le même IsPlayerControlled. Il ne simule donc pas une barrière individuelle persistante entre deux colons de la même faction.

Le complément conserve l'origine avant Pawn.SetFaction et sa transmission Chitchat/DeepTalk s'applique aussi entre colons. Elle nécessite une interaction réussie, Social actif chez les deux pions, option activée, aptitude d'apprentissage disponible et enseignant maîtrisant la connaissance à 100 %. Pas de progression temporelle automatique ni de cours spécifiques aux adultes immobilisés. Les malus internes sont limités aux facteurs négatifs/romance/bagarre du parent ; pas de pénalité générale de travail ou de pensée permanente d'incompréhension. L'abasie installée limite Moving à 0.1, sans effet direct sur Social dans sa Def ; raréfaction des conversations possible, cause exacte non confirmée.

Log 1.7 lu dans le sous-dossier Logs : Bootstrap 1.0.0 chargé, avertissement de faction historique absente Faction_54. Aucun lien établi entre cet avertissement et le colon signalé. Conditions du cas réel à contrôler avant de choisir un correctif ; aucune modification C#, de sauvegarde, de paramètres ou de version. IL d'audit sous work/recruited-abasia-base-il.txt, exclu de Git.

Diagnostic Mendez : sauvegarde récente du 2026-10-19 à 23:13 inspectée. Human159057, faction actuelle Faction_39, origine Faction_26 conservée et registre initialisé. Seulement Gallique (RaceLanguage_3) et son dialecte Colonial Fleet Standard Communications, 5th Edition (Dialect_3), natifs à 100 %. Conversation Chitchat enregistrée au tick 15057423 ; aucune abasie dans les hediffs actuels. Enfance ZCB_FeralChild de Childhood Backstories : Def installée 1.7 désactivant WorkTags Social et Intellectual. L'assembly du jeu confirme que SkillRecord.TotallyDisabled calcule ces incapacités via CombinedDisabledWorkTags/GetDisabledWorkTypes ; AutomaticTransmission.Exchange refuse les échanges si Social est TotallyDisabled chez l'un des deux pions. Cause identifiée de l'absence de transmission, conforme à la règle demandée auparavant, pas un effacement de langue au recrutement. Les réglages du complément omettent la clé enabled, donc valeur par défaut active. Une éventuelle réception passive malgré Social indisponible serait un changement de règle à décider, pas un correctif silencieux. Fragment de pion sous work/mendez-pawn-snapshot.xml, exclu de Git ; aucune sauvegarde modifiée.

Audit de logs du 2026-10-11 sur une partie modifiée en cours : copie Workshop 1.0.0 et DLL du parent 1.6 identiques aux empreintes vérifiées. Boucle principale : 161 occurrences d'Exception ticking sur un pion, pile GiddyUp.MountUtility.CancelRopers → GoMount → HandleVisitorsMounting, Giddy-Up 2 — Continued 2.2.5. Aucun appel RLI ou Cybranian dans cette pile ; pas de patch RLI sur DetermineNextJob. Source directe de cette exception attribuée à Giddy-up, impact sur les ralentissements probable sans mesure de performance ni comparaison A/B.

Un avertissement RLI au chargement : référence de faction Faction_45 introuvable dans PawnLanguages ; VEF.Storyteller.RaidGroup signale la même faction absente. Une occurrence seulement, sans répétition dans les ticks observés. Origine de faction sérialisée par Scribe_References ; durcissement des références historiques manquantes à envisager séparément sans réinitialiser les connaissances. Onze initialisations non résolues signalées une fois ; cet état ne prouve pas onze corruptions.

Autres erreurs : 12 exceptions Graphic_Multi au chargement, Defs tiers absentes, exceptions Building_Door.Tick/GetThingList pour porte/clôture dont une remontant par ThingOwner d'un pion. Aucune pile RLI observée pour ces exceptions ; auteur exact des données de porte et graphiques non déterminé. Relevés conservés sous work/player-log-audit-20261011-173842.log et work/player-log-audit-latest.log, exclus de Git. Aucun fichier du jeu, mod installé ou sauvegarde modifié. Essai ciblé proposé : désactiver le module Caravans de Giddy-up via ses paramètres, puis redémarrer (exigence indiquée par sa traduction installée) et comparer ; aucun essai exécuté par l'agent.

Demande du 2026-10-11 : version finale 1.0.0 avec tag GitHub, après création de la preview. Visuel dans About/Preview.png : pions stylisés avec yeux, apprentissage autour d'un livre et bandeau 1.6 dans le coin supérieur droit. Génération originale avec l'outil image_gen, sans reprise d'image du parent. Export PNG 640 × 640, 862 346 octets, SHA256 EE119A13E8110018804FE2F58510E0A2343F81AB14512888439842C7E91DA46F ; original conservé sous artifacts/preview, exclu de Git. package.ps1 inclut la preview dans sa liste autorisée de neuf fichiers et contrôle son poids inférieur à 1 000 000 octets. Workshop à préparer étape par étape, sans envoi à cette étape.

Le 2026-10-11, l'utilisateur confirme la conservation de la difficulté et des calculs de Cybranian, y compris la différence entre lecture et conversation. Métadonnées About actualisées : fonctionnalités actuelles, DLC facultatifs, migration et URL GitHub. Version 0.17.0 conservée : aucune modification du code, des traductions ou des données persistantes dans cette consolidation.

Les interprètes sont reportés à une éventuelle mise à jour ; leur proposition n'a pas été retenue pour implémentation. Une API publique pour GateRim est facultative et doit être cadrée séparément, sans dépendance en sens inverse.

Dépôt public fourni par utilisateur : [Diablood/RimLanguages-IndividualLearning](https://github.com/Diablood/RimLanguages-IndividualLearning). Dépôt local initialisé, origin configuré et branche main rattachée au commit initial distant 09f6b13. Licence MIT distante conservée, y compris sa notice de copyright. Premier envoi du projet ; aucune release, aucun tag ou Workshop.

Envoi des sources confirmé le 2026-10-11 : commit 89eeb0c57d8681fb9372a62afef06bd9864af8a6 sur main, référence distante identique. 59 fichiers présents ; anciens fichiers de reprise et DLL absents. L'API GitHub confirme visibility=public, private=false et default_branch=main.

Les fichiers CONTINUER_SUR_CODEX.md et CODEX_START.md sont retirés à la demande utilisateur : doublons de reprise. AGENTS.md, l'état courant et la roadmap restent les points d'entrée du développement.

Voir [ROADMAP.md](ROADMAP.md) et [GITHUB_SETUP.md](GITHUB_SETUP.md).

## Reprise et preuves

Relire AGENTS.md, README.md, ce fichier, la roadmap et les documents concernés à chaque reprise ; appliquer les vrais skills rimworld-mod-developer et rimworld-release-qa. Mettre à jour les documents après chaque lot, sans empiler des états actuels contradictoires.

L'audit de la DLL Cybranian est dans [AUDIT_CYBRANIAN.md](AUDIT_CYBRANIAN.md). Historique des lots dans [CHANGELOG.md](../CHANGELOG.md). Les copies antérieures et journaux locaux restent sous work, exclu du suivi Git.

Empreinte de la DLL du complément livrée en 0.16.0 :
`F2473A5D73489C28C8E66A0719B191982BF536AF1652A212F4AD59BD180689C5`.

Contrôles du lot documentaire du 2026-10-11 : 24 documents décodés en UTF-8 strict, 81 liens locaux valides, absence de chemins personnels et vérification des exclusions nécessaires. Empreinte de la DLL 0.16.0 inchangée. Modèle de bug relu ; aucun rendu GitHub exécuté, aucun nouveau test du moteur.
