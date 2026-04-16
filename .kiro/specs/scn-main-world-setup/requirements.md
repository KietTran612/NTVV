# Requirements: scn-main-world-setup

## Overview

Setup world gameplay layer cho SCN_Main: physics layer, input system, CropTile prefab, camera pan, và data verification. Kết quả cuối: player tap vào ô đất → CropActionPanel mở → gameplay loop trồng/chăm/thu hoạt động hoàn chỉnh.

**Prerequisite:** `scn-main-ui-rebuild` phải hoàn thành (ít nhất Task 1–5) trước khi chạy Integration Test của spec này.

---

## Global Rules

> ⚠️ ĐỌC TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO

- KHÔNG viết code mới ngoài `CropGridSpawner.cs` (Task 1)
- KHÔNG xóa assets có sẵn
- `editor_save_scene` sau mỗi sub-task
- Nếu asset thiếu → dùng Unity default + ghi `// TODO: replace with {filename}`
- Quick Test FAIL → fix trong task hiện tại, KHÔNG sang task tiếp theo (tối đa 2 lần retry, sau đó escalate)
- **Known Issue (không fix trong spec này):** FarmCameraController dùng Old Input System (`Input.GetMouseButtonDown`) còn WorldObjectPicker dùng New Input System (`PlayerInput`). Cả hai sẽ cùng fire khi tap — acceptable ở prototype.

---

## Functional Requirements

### Req 1 — Physics Layer & Camera Tag
- 1.1 Layer `"Interactable"` phải tồn tại trong Project Settings → Tags and Layers
- 1.2 Main Camera trong `[WORLD_ROOT]` phải có tag `"MainCamera"` để `Camera.main` hoạt động (dùng bởi `PopupManager.PositionContextPanel()`)

### Req 2 — Input System
- 2.1 File `PlayerInputActions.inputactions` phải tồn tại tại `Assets/_Project/Input/`
- 2.2 Action Map `"Player"` có Action `"Tap"` — Type: Value, Control Type: Vector2, Binding: `<Pointer>/position`, Interaction: Press
- 2.3 `PlayerInput` component phải nằm trên **cùng GameObject** với `WorldObjectPicker` (Send Messages chỉ dispatch đến cùng GO)
- 2.4 PlayerInput Behavior = `Send Messages`

### Req 3 — CropGridSpawner Script (Editor Tool)
- 3.1 Script `[ExecuteInEditMode]` với `[ContextMenu]` `GenerateGrid` và `ClearGrid`
- 3.2 `GenerateGrid()` tạo `rows × cols` tiles, đặt tên GO `"tile_r{row}_c{col}"` — SaveLoad dùng `gameObject.name` làm tileId fallback
- 3.3 Sau khi generate → xóa component khỏi scene, giữ script file để tái dùng

### Req 4 — CropTile Prefab
- 4.1 Có `BoxCollider` (3D, không phải 2D) size=(1,1,0.1) trên root, layer=`Interactable` — `WorldObjectPicker` dùng `Physics.Raycast` (3D)
- 4.2 Có `SpriteRenderer` cho soil (root level)
- 4.3 Có child `CropRenderer` (SpriteRenderer, disabled by default)
- 4.4 Có children `WeedVisual`, `BugVisual`, `WaterVisual` (disabled by default)
- 4.5 `CropTileView` component wired: `_soilRenderer`, `_cropRenderer`, `_weedVisual`, `_bugVisual`, `_waterVisual`, `_registry`
- 4.6 Prefab saved tại `Assets/_Project/Prefabs/World/CropTile.prefab`

### Req 5 — World Tile Placement
- 5.1 `CropArea` trong `[WORLD_ROOT]` chứa 6 CropTile instances (2 rows × 3 cols)
- 5.2 Mỗi tile tên `"tile_r{r}_c{c}"` — cần thiết cho SaveLoad compatibility
- 5.3 Mỗi tile trên layer `Interactable`

### Req 6 — World Interaction
- 6.1 GO `"WorldObjectPicker"` trong `[SYSTEMS]`
- 6.2 `WorldObjectPicker._mainCamera` → Main Camera, `_interactableLayer` → Interactable layer
- 6.3 Tap vào CropTile → `PopupManager.ShowContextAction(tile)` → CropActionPanel hiện đúng buttons theo `TileState`

### Req 7 — Camera Pan
- 7.1 `FarmCameraController` attach lên Main Camera
- 7.2 `_panSpeed=1.0`, `_boundX=(-5,5)`, `_boundY=(-3,3)`
- 7.3 `_zoomSpeed=4`, `_minOrtho=3`, `_maxOrtho=8`

### Req 8 — Core Systems
- 8.1 `TimeManager` tồn tại trong `[SYSTEMS]`, `_tickRate=1.0`
- 8.2 `QuestManager` tồn tại trong `[SYSTEMS]` — `GameManager.InitializeCoreSystems()` gọi `QuestManager.Instance.LoadData()`

### Req 9 — Data Validity
- 9.1 `GameDataRegistrySO.crops.Count >= 1`
- 9.2 Mỗi CropDataSO phải có: `cropId != ""`, `growTimeMin > 0`, `baseYieldUnits > 0`
- 9.3 `postRipeLifeMin > 0` — **CRITICAL**: nếu = 0 thì `LifeAfterRipeInSeconds = 0` và cây chết ngay frame đầu sau khi chín (xem BUG-08 trong bug-backlog.md)
