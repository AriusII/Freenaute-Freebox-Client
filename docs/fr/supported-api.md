# API prise en charge et exclusions

Le client expose volontairement les opérations Freebox Server documentées et stables via des façades typées : découverte/authentification, AirMedia, caméras, notifications, réseau, services retenus, fichiers, configuration système, WebSockets modernes et transports typés.

Les surfaces suivantes ne sont **pas revendiquées comme prises en charge** :

- contrats RRD/statistiques ;
- stockage, RAID et administration des machines virtuelles ;
- contrats PVR et VPN ;
- trackers, pairs et blacklist des téléchargements ;
- opérations Home non documentées de pairing, nœuds et tuiles ;
- champs Wi-Fi explicitement dépréciés ou instables ;
- ancien upload HTTP à la place de l'upload WebSocket moderne.

Ces exclusions sont intentionnelles : la documentation de référence ne fournit pas de routes, encodages, schémas ou garanties de compatibilité suffisamment stables. Aucun type, chemin ou contexte généré exclu ne doit être considéré comme API garantie.

Les modèles de protocole conservent la sémantique filaire de `Optional<T>`, des chemins Freebox encodés, des unions objet/tableau et des enums générés. Une réponse de permission PVR n'est pas une promesse de compatibilité, même si un champ est omis dans une réponse observée.

Le SDK n'est pas exhaustif et ne remplace pas les tests sur le modèle et les permissions Freebox de votre déploiement. La documentation officielle Freebox et la découverte runtime restent les autorités en cas de changement de firmware.

Références officielles : <https://dev.freebox.fr/sdk/os/> et les modules API publiés sur ce site.
