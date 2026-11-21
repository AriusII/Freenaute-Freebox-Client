# Discovery and networking

Discovery is LAN oriented. When available, DNS-SD/mDNS service `_fbx-api._tcp.local.` is preferred; HTTP discovery is the fallback used when multicast produces no usable answer. Initial pairing must not send credentials to an arbitrary remote host.

The discovery result is untrusted input. TXT, SRV, host, UID, API base path, HTTPS flag, and port are validated before use. The discovered UID belongs in the credential identity, so changing IP addresses do not create duplicate persistent identities. A token must never follow an unvalidated redirect or be sent to a different UID/origin without an explicit trust decision.

## HTTPS and remote use

Use the Freebox HTTPS endpoint and the documented certificate trust policy. Discovery does not make an announced remote origin automatically trusted. Later remote sessions must still follow configured origin and Freebox identity constraints.

Redirect handling and TLS belong to the HTTP handler configured by the SDK/container. Consumers overriding that handler remain responsible for preserving host validation, certificate validation, and redaction of sensitive headers.

## Network constraints

- Windows and Linux need permission to use UDP port 5353 and multicast interfaces.
- VPNs, VLANs, wireless isolation, firewalls, and duplicate responders can hide or duplicate results.
- Multiple interfaces may return the same service or conflicting UIDs; do not merge credentials across identities.
- Containers commonly require host networking or an explicit unsupported mode. Do not assume bridge networking propagates multicast.
- IPv6 interfaces need the correct link-local scope.

Cancellation is bounded by the operation timeout and caller cancellation token. A multicast timeout falls back only according to the discovery policy; malformed security-relevant metadata is rejected rather than converted into a less strict endpoint.
