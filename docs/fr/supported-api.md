# API prise en charge et exclusions

Source de réconciliation : `docs/api-model.json` comparé à `.Client` et `.Mapper`.

## Matrice de couverture (exacte)

- **Opérations** : **222 / 222** couvertes (**100%**)
- **Schémas** : **120 / 120** couverts (**100%**)
- Artefact de preuve :  
  `C:\Users\Arius-dev\.copilot\session-state\a6a9000b-eb02-48ef-a676-4fc1cb95fc0e\files\wave2-global-matrix-docs-coverage.json`

## Politique de couverture

- Les éléments **stables** constituent la surface publique normale orientée compatibilité.
- Les éléments **instables** sont inclus et signalés uniquement dans la documentation XML.
- Les éléments **dépréciés** sont inclus avec l’annotation `[Obsolete]`.

## Normalisation d’alias incluse dans la matrice

- Les alias OpenAPI `/[number,address,url,email]/...` sont comptés via les routes concrètes implémentées (`/number`, `/address`, `/url`, `/email`).
- `/contact/{contact_id}/[numbers|addresses|urls|emails]/` est compté via les collections concrètes implémentées.
- `/parental/filter/{id}/planning` est compté via la sous-ressource dédiée de planning.
- `/ws/upload` est compté via la couverture de la route WebSocket d’upload.
- `/api_version` est compté via la couverture de la découverte transport.
