# Injection de dépendances

## Enregistrement

`AddFreeboxClient(Action<FreeboxClientOptions>)` enregistre le client par défaut. La surcharge acceptant `IConfiguration` lie la configuration à l'aide des métadonnées générées. La surcharge action plus `validateOnStart` active la validation au démarrage. Les clients nommés utilisent `AddFreeboxClient(string name, ...)` et se résolvent avec les API DI à clé.

Les options couvrent l'origine, une éventuelle version majeure d'API explicite, le chemin de base, le délai, l'identité applicative, les noms affichés, le profil d'identifiants, l'authentification automatique, le nom du client HTTP et la résilience opt-in. Gardez un `HttpClientName` unique par client sauf partage volontaire.

Le conteneur expose `IFreeboxClient`, `IFreeboxAuthorizationCoordinator`, les sous-façades publiques, `IFreeboxTransport`, `IFreeboxBinaryTransport` et `IFreeboxWebSocketTransport`. Les enregistrements nommés exposent leurs équivalents à clé. Chaque façade utilise l'état et la session de son client propriétaire.

## Magasins et durées de vie

`TryAddSingleton` ajoute uniquement à défaut le magasin mémoire. En production, remplacez-le par un `IFreeboxCredentialStore` sécurisé, disponible et thread-safe. Le magasin doit vivre aussi longtemps que l'état singleton du client ; un singleton ne doit pas capturer un magasin scoped.

## Handlers et résilience

`AddFreeboxClient` retourne `IHttpClientBuilder` et permet d'utiliser les API standard de Microsoft Extensions pour configurer le client, le handler principal et les handlers additionnels. Conservez la validation de certificat/origine, la politique de redirection et la redaction des secrets.

La résilience est opt-in. Les mutations et opérations à effet de bord documenté ne sont pas rejouées automatiquement ; l'appelant doit choisir une politique selon l'idempotence. N'activez la résilience des lectures qu'à travers la configuration prise en charge.

## Configuration et AOT

Le binding repose sur les métadonnées de configuration générées et le validateur d'options généré. Une origine, une version majeure explicite, un délai ou un identifiant invalide provoque une erreur de validation d'options, sans binding réflexif tardif. Ne placez pas les tokens dans la configuration ordinaire : ses providers ne sont pas des magasins de secrets.
