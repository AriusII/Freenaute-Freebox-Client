# Démarrage rapide

## Prérequis

- SDK .NET 11, ou une application consommatrice compatible sous Windows ou Linux x64.
- Une Freebox Server accessible sur le réseau local pour l'association initiale.
- Un accès physique à la Freebox afin d'approuver l'application.

Installez le client (Mapper est installé comme dépendance) :

```bash
dotnet add package Freenaute.Freebox.Client
```

Enregistrez un client par défaut :

```csharp
services.AddFreeboxClient(options => options
    .UseApplication("org.example.my-app")
    .WithApplicationVersion("1.0.0"));
```

Pour une autre identité, passez un nom de client en premier argument à `AddFreeboxClient`. Ne réutilisez un profil d'identifiants que si plusieurs clients doivent partager la même identité de Freebox et d'application.

## Associer une seule fois

Résolvez `IFreeboxAuthorizationCoordinator`, appelez `BeginPairingAsync`, puis demandez immédiatement à l'utilisateur d'approuver physiquement la demande sur la Freebox. Appelez ensuite `WaitForApprovalAsync` avec un jeton d'annulation appartenant à l'application.

Le magasin mémoire intégré sert au développement et disparaît à l'arrêt du processus. En production, enregistrez explicitement une implémentation sécurisée de `IFreeboxCredentialStore` avant de construire le client. N'utilisez jamais, en repli, des fichiers en clair, des variables d'environnement ou des secrets utilisateur pour conserver l'`app_token`.

Après approbation, les appels authentifiés réutilisent le token applicatif stocké. Le token de session est créé en mémoire et n'est jamais persisté.

## Utiliser le client

Résolvez `IFreeboxClient` (ou son client nommé), puis utilisez par exemple `Discovery`, `Network`, `Files` ou `WebSockets`. Transmettez l'annulation aux opérations réseau. `Authentication.CloseSessionAsync` ferme la session mémoire sans supprimer le token applicatif réutilisable ; `ForgetAsync` du coordinateur supprime les identifiants locaux. La révocation distante reste une opération administrative distincte.

Consultez [authentification et identifiants](authentication-and-credentials.md) puis [découverte et réseau](discovery-and-networking.md).
