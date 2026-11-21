# Authentication and credentials

## One-time approval flow

1. The client discovers the Freebox and its UID.
2. `BeginPairingAsync` sends an authorization request containing the application identity and device display name.
3. The returned `app_token` and tracking ID are stored as **Pending** before polling starts.
4. The user must physically approve the application on the Freebox. Software cannot bypass this step.
5. `WaitForApprovalAsync` polls the Freebox. The server controls timing and terminal status; the SDK does not assign a fixed approval duration.
6. On `granted`, the credential becomes **Approved** and can be reused after a restart by the configured store.

`denied`, `timeout`, and `unknown` are terminal outcomes rather than successful approval. `invalid_token` requires explicit renewal or re-pairing; callers should not implement an infinite retry loop.

## Session and app-token separation

The `app_token` is durable application identity material and belongs in a secure store. It is never placed in diagnostic text. Credential identity is scoped by application ID, caller-defined profile, and the discovered Freebox UID when available.

A login challenge is exchanged for an HMAC-derived password, and the resulting `session_token` is held only in process memory. Session tokens, challenges, signatures, session bodies, and `X-Fbx-App-Auth` values must not be logged or persisted.

`CloseSessionAsync` clears the local session only. `IFreeboxAuthorizationCoordinator.ForgetAsync` deletes local pairing data. Forgetting is not proof that a Freebox-side grant was remotely revoked. Treat logout, remote revocation, and local deletion as distinct operations.

## Store selection

There is no secure persistent store selected implicitly. `InMemoryFreeboxCredentialStore` is a test/development implementation. Windows deployments should use a private per-user store backed by operating-system protection such as CurrentUser DPAPI. Linux desktop applications may use an unlocked Secret Service collection; headless environments must inject an application-owned encrypted store or external secret backend.

If a secure store is unavailable, fail closed and ask the operator to configure one. Do not use a plaintext JSON fallback. The same user-scope and machine assumptions apply when moving between restarts or hosts.

Environment variables and user secrets are configuration conveniences, not at-rest credential stores. Do not place a real app token in samples, source, CI logs, exception messages, or screenshots. A value in diagnostics must be replaced with `<redacted>`.

## Troubleshooting

- **Store unavailable:** configure an explicitly available `IFreeboxCredentialStore`; do not retry against a weaker backend.
- **Approval still pending:** verify that the user completed the action on the intended Freebox; cancellation stops polling.
- **Invalid/revoked token:** clear the session and perform an explicit controlled re-pairing after local policy permits it.
- **Unexpected UID or origin:** stop authentication; do not send credentials to an untrusted address.
