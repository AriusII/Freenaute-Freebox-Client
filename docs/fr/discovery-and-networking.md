# Découverte et réseau

La découverte vise le réseau local. Quand il est disponible, le service DNS-SD/mDNS `_fbx-api._tcp.local.` est prioritaire ; la découverte HTTP sert de repli lorsqu'aucune réponse multicast utilisable n'arrive. L'association initiale ne doit jamais envoyer d'identifiants vers un hôte distant arbitraire.

Le résultat de découverte est une donnée non fiable. Les champs TXT, SRV, hôte, UID, chemin API, HTTPS et port sont validés avant usage. L'UID découvert participe à l'identité du credential afin qu'une adresse IP changeante ne crée pas de doublon persistant. Aucun token ne doit suivre une redirection non validée ni être envoyé à une autre origine/UID sans décision explicite de confiance.

## HTTPS et utilisation distante

Utilisez l'endpoint HTTPS Freebox et la politique de certificat documentée. Une origine distante annoncée par la découverte n'est pas automatiquement approuvée. Les sessions distantes ultérieures restent soumises aux contraintes d'origine et d'identité.

La redirection et TLS appartiennent au handler HTTP configuré par le SDK/conteneur. Un handler personnalisé doit préserver la validation d'hôte, du certificat et la redaction des en-têtes sensibles.

## Contraintes réseau

- Windows et Linux doivent autoriser UDP 5353 et le multicast sur les interfaces concernées.
- VPN, VLAN, isolation Wi-Fi, pare-feu et répondants dupliqués peuvent masquer ou multiplier les résultats.
- Plusieurs interfaces peuvent fournir le même service ou des UID contradictoires ; les identifiants ne doivent pas être fusionnés.
- Un conteneur exige souvent le réseau host ou un mode explicitement non pris en charge. Le bridge ne propage généralement pas mDNS.
- IPv6 exige le bon scope link-local.

L'annulation est bornée par le délai d'opération et le jeton appelant. Un timeout multicast applique le repli prévu ; des métadonnées de sécurité malformées sont refusées plutôt que converties en endpoint moins strict.
