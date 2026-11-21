# Supported API and exclusions

The public client intentionally exposes documented, stable Freebox Server operations through typed facades: discovery/authentication, AirMedia, cameras, notifications, network, retained services, files, system configuration, modern WebSockets, and typed transports.

The following are **not claimed as supported**:

- RRD/statistics contracts;
- storage, RAID, and virtual-machine administration;
- PVR and VPN contracts;
- download trackers, peers, and blacklist administration;
- undocumented Home pairing/node/tile operations;
- explicitly deprecated or unstable Wi-Fi fields;
- legacy HTTP upload in place of the modern WebSocket upload.

These exclusions are deliberate: the authoritative documentation does not provide sufficiently stable routes, encodings, schemas, or compatibility semantics. Excluded types, routes, and source-generated metadata must not be treated as guaranteed API.

Protocol models preserve wire semantics such as `Optional<T>`, encoded Freebox paths, object-or-array unions, and source-generated enums. A PVR permission response is not a supported compatibility promise, even if an individual response omits a field.

The SDK is not exhaustive and does not replace testing against the Freebox model and permission set used by your deployment. The official Freebox API documentation and runtime discovery remain the authorities; consult them when a firmware update changes a contract.

Official references: <https://dev.freebox.fr/sdk/os/> and the API modules published under that documentation site.
