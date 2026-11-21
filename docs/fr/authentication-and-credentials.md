# Authentification et identifiants

## Flux d'approbation unique

1. Le client découvre la Freebox et son UID.
2. `BeginPairingAsync` envoie une demande contenant l'identité de l'application et son nom d'affichage.
3. L'`app_token` et l'identifiant de suivi retournés sont stockés à l'état **Pending** avant le début du polling.
4. L'utilisateur doit approuver l'application physiquement sur la Freebox. Aucune étape logicielle ne contourne cette approbation.
5. `WaitForApprovalAsync` interroge la Freebox. Le serveur impose le rythme et l'état terminal ; le SDK ne fixe pas artificiellement une durée d'approbation.
6. Sur `granted`, l'identifiant devient **Approved** et est réutilisable après redémarrage par le magasin configuré.

`denied`, `timeout` et `unknown` sont des états terminaux, pas une approbation réussie. `invalid_token` exige un renouvellement ou une nouvelle association explicite ; ne programmez pas de boucle de relance infinie.

## Séparation entre session et token applicatif

L'`app_token` est une identité applicative durable qui doit rester dans un magasin sécurisé. Il ne doit jamais apparaître dans les diagnostics. Son identité est délimitée par l'identifiant d'application, un profil choisi par l'appelant et, lorsqu'il est connu, l'UID de la Freebox.

Le challenge de connexion sert à produire le mot de passe HMAC, puis le `session_token` résultant reste uniquement en mémoire. Les tokens de session, challenges, signatures, corps de session et valeurs `X-Fbx-App-Auth` ne doivent être ni journalisés ni persistés.

`CloseSessionAsync` efface uniquement la session locale. `IFreeboxAuthorizationCoordinator.ForgetAsync` supprime l'association locale. L'oubli local ne prouve pas qu'un droit a été révoqué côté Freebox. Déconnexion, révocation distante et suppression locale sont trois opérations distinctes.

## Choix du magasin

Aucun magasin persistant sécurisé n'est choisi implicitement. `InMemoryFreeboxCredentialStore` est réservé aux tests et au développement. Sous Windows, utilisez un magasin privé par utilisateur, protégé par le système, par exemple DPAPI `CurrentUser`. Sous Linux avec bureau, un Secret Service déverrouillé peut convenir ; sans bureau, injectez un magasin chiffré appartenant à l'application ou un service de secrets externe.

Si le magasin sécurisé est indisponible, refusez l'opération et demandez sa configuration. N'utilisez pas de repli JSON en clair. Les mêmes hypothèses d'utilisateur et de machine s'appliquent entre redémarrages ou machines.

Les variables d'environnement et les secrets utilisateur facilitent la configuration : ce ne sont pas des magasins chiffrés au repos. N'inscrivez aucun vrai token dans les exemples, sources, logs CI, exceptions ou captures. Toute valeur diagnostique doit être remplacée par `<redacted>`.

## Diagnostic

- **Magasin indisponible :** configurez explicitement un `IFreeboxCredentialStore` disponible ; ne basculez pas vers un backend plus faible.
- **Association encore en attente :** vérifiez l'action physique sur la bonne Freebox ; l'annulation arrête le polling.
- **Token invalide ou révoqué :** effacez la session puis réalisez une nouvelle association explicitement autorisée.
- **UID ou origine inattendu :** stoppez l'authentification ; n'envoyez aucun identifiant vers une adresse non approuvée.
