# Rim Languages — Individual Learning : RimWorld 1.6 / 1.7

Mise à jour de compatibilité du 2026-10-14, tag **v1.0.0-rw1.7**.

- RimWorld 1.7 ajouté aux versions prises en charge, en conservant 1.6.
- Chargement explicite du dossier d'assembly 1.6 pour les deux versions du jeu.
- « Test OK » utilisateur après mise à jour des métadonnées sur la branche instable ; installation observée : 1.7.4977 rev1248. Les prochaines builds peuvent nécessiter une adaptation.
- Aucun changement C#, de gameplay, de données ou de schéma de sauvegarde. DLL et version du mod conservées en 1.0.0.
- Détection Harmony adaptée à ses nouveaux dossiers de version. Les scripts de compilation nécessitent encore les références Mono de RimWorld 1.6 ; la nouvelle distribution 1.7/.NET 10 n'est pas prise en charge pour reconstruire le mod.

Télécharger **RimLanguages-IndividualLearning-1.0.0-rw1.7.zip**, extraire le dossier dans `<RimWorld>/Mods`, puis redémarrer le jeu. Harmony et Cybranian restent nécessaires ; aucun DLC obligatoire. Le tag et le ZIP 1.0.0 d'origine sont conservés.

Le paquet réutilise la DLL de release dont l'empreinte a été vérifiée, puis contrôle versions, XML, traductions et chaque contenu du ZIP par SHA256. Aucune reconstruction ni nouvelle exécution des 277 assertions historiques revendiquée ; aucun moteur exécuté par l'agent. Workshop : mise à jour à envoyer séparément depuis le jeu.

---

Compatibility update for RimWorld 1.6 and 1.7, tag **v1.0.0-rw1.7**. Explicitly routes both game versions to the same assembly folder. User-tested after metadata updates on the unstable branch; observed installation: 1.7.4977 rev1248. Future unstable builds may need further updates.

No gameplay or save changes. Reuses the validated 1.0.0 DLL; mod version and save schema remain unchanged. Archive contents, DLL hash, versions, XML and translation keys checked. No rebuild or new knowledge-test run claimed: the current build workflow still requires RimWorld 1.6 Mono references, while the installed 1.7 uses .NET 10.

Download **RimLanguages-IndividualLearning-1.0.0-rw1.7.zip**, extract into `<RimWorld>/Mods`, and restart. Requires Harmony and Cybranian; DLC features remain optional. The original v1.0.0 tag and release archive are preserved. Workshop updating is a separate in-game action.
