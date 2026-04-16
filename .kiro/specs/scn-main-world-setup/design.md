# Design Document — scn-main-world-setup

## Overview

Setup world gameplay layer cho SCN_Main theo approach **Pure MCP + 1 Editor Script mới**: tạo `CropGridSpawner.cs` để generate tiles, setup physics layer, input system, camera pan. Không viết code gameplay mới — tất cả logic đã có trong `CropTileView`, `WorldObjectPicker`, `FarmCameraController`.

**Milestone:** M2 từ `farm_game_unity_build_map_v1.md` — "Crop core: loop trồng → chờ → thu chạy hoàn chỉnh."

---

## Architecture

### Scene Hierarchy sau spec này

```
SCN_Main
├── [WORLD_ROOT]
│   ├── Main Camera          (tag=MainCamera, Camera, AudioListener, FarmCameraController)
│   ├── CropArea
│   │   ├── tile_r0_c0       (CropTileView, BoxCollider, SpriteRenderers)
│   │   ├── tile_r0_c1
│   │   ├── tile_r0_c2
│   │   ├── tile_r1_c0
│   │   ├── tile_r1_c1
│   │   └── tile_r1_c2
│   └── BarnArea             (empty, reserved cho M4)
│
└── [SYSTEMS]
    ├── GameManager          (từ spec cũ)
    ├── EconomySystem        (từ spec cũ)
    ├── StorageSystem        (từ spec cũ)
    ├── LevelSystem          (từ spec cũ)
    ├── SaveLoadManager      (từ spec cũ)
    ├── PopupManager         (từ spec cũ)
    ├── TimeManager          (thêm trong spec này, _tickRate=1.0)
    ├── QuestManager         (thêm trong spec này)
    └── WorldObjectPicker    (thêm trong spec này — có cả PlayerInput trên cùng GO)
```

---

## Component Designs

### CropTile Prefab

```
CropTile (root GO)
├── BoxCollider           — size=(1,1,0.1), layer=Interactable
│                           Physics.Raycast (3D) — KHÔNG dùng BoxCollider2D
├── CropTileView          — component, wire tất cả refs bên dưới
├── SoilRenderer          — SpriteRenderer (soil/đất trống sprite)
├── CropRenderer          — SpriteRenderer (cây, SetActive=false)
├── WeedVisual            — GameObject + SpriteRenderer (SetActive=false)
├── BugVisual             — GameObject + SpriteRenderer (SetActive=false)
└── WaterVisual           — GameObject + SpriteRenderer (SetActive=false)
```

**CropTileView wire map:**

| Field | Target |
|-------|--------|
| `_soilRenderer` | SoilRenderer |
| `_cropRenderer` | CropRenderer |
| `_weedVisual` | WeedVisual |
| `_bugVisual` | BugVisual |
| `_waterVisual` | WaterVisual |
| `_registry` | GameDataRegistry.asset |

**Save path:** `Assets/_Project/Prefabs/World/CropTile.prefab`

> Note: `CropTileView._tileId` là private field không có setter. SaveLoad sử dụng `gameObject.name` làm fallback tileId. CropGridSpawner đặt tên GO `"tile_r{r}_c{c}"` để đảm bảo SaveLoad hoạt động đúng.

---

### CropGridSpawner.cs

Editor-only tool, **không chạy runtime**. Script file giữ trong project để tái dùng khi cần thêm tiles.

```csharp
[ExecuteInEditMode]
public class CropGridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _cropTilePrefab;
    [SerializeField] private int _rows = 2;
    [SerializeField] private int _cols = 3;
    [SerializeField] private float _cellWidth = 1.2f;
    [SerializeField] private float _cellHeight = 0.7f;
    [SerializeField] private GameDataRegistrySO _registry;

    [ContextMenu("Generate Grid")]
    public void GenerateGrid()
    {
        // 1. Clear existing children
        // 2. for row in rows: for col in cols:
        //    - Instantiate prefab at (col*cellWidth, row*cellHeight, 0)
        //    - Name: "tile_r{row}_c{col}"
        //    - Wire _registry vào CropTileView
    }

    [ContextMenu("Clear Grid")]
    public void ClearGrid()
    {
        // Destroy all children of this transform
    }
}
```

**Sau khi GenerateGrid():** xóa component `CropGridSpawner` khỏi `CropArea` GO (bake tiles vào scene).

---

### WorldObjectPicker + PlayerInput

```
GO: "WorldObjectPicker" trong [SYSTEMS]
├── WorldObjectPicker component
│   ├── _mainCamera → Main Camera
│   └── _interactableLayer → Interactable (layer mask)
└── PlayerInput component (CÙNG GO — Send Messages dispatch đến cùng GO)
    ├── Actions → Assets/_Project/Input/PlayerInputActions.inputactions
    └── Behavior → Send Messages
```

**Input Action Asset:** `Assets/_Project/Input/PlayerInputActions.inputactions`

```
Action Map: "Player"
└── Action: "Tap"
    ├── Type: Value
    ├── Control Type: Vector2
    ├── Binding: <Pointer>/position
    └── Interaction: Press
```

> Warning: PlayerInput phải trên CÙNG GO với WorldObjectPicker. "Send Messages" chỉ dispatch đến cùng GameObject — không dispatch sang sibling hoặc object khác trong hierarchy.

---

### FarmCameraController

Attach lên Main Camera trong `[WORLD_ROOT]`:

| Field | Value |
|-------|-------|
| `_panSpeed` | 1.0 |
| `_boundX` | (-5, 5) |
| `_boundY` | (-3, 3) |
| `_zoomSpeed` | 4 |
| `_minOrtho` | 3 |
| `_maxOrtho` | 8 |

---

## Interaction Flow

```
Tap vào CropTile
  → PlayerInput fires "Tap" action (Press interaction)
  → OnTap(InputValue) được gọi trên WorldObjectPicker (Send Messages, cùng GO)
  → WorldObjectPicker: Physics.Raycast từ Camera qua screen position
  → Hit BoxCollider (3D) trên CropTile (layer=Interactable)
  → GetComponentInParent<CropTileView>()
  → OnTileSelected(tile)
  → PopupManager.Instance.ShowContextAction(tile)
  → EnsureContextPanel() [load từ Resources/UI/Default/ContextActionPanel nếu chưa có]
  → CropActionPanelController.Setup(tile)
  → RefreshUI() → SetAllButtonsActive theo TileState
  → PositionContextPanel(tile.transform.position)
     └── Camera.main.WorldToScreenPoint() ← cần tag "MainCamera" trên Main Camera
```

---

## Data: Minimum Viable CropDataSO (Carrot Placeholder)

Tạo nếu `GameDataRegistrySO.crops` rỗng:

| Field | Value | Ghi chú |
|-------|-------|---------|
| `cropId` | `"crop_carrot"` | unique, non-empty |
| `cropName` | `"Cà Rốt"` | display name |
| `unlockLevel` | `0` | available từ đầu |
| `seedCostGold` | `10` | |
| `growTimeMin` | `1` | 1 phút để test nhanh |
| `phase1Pct` | `0.25` | default |
| `phase2Pct` | `0.35` | default |
| `baseYieldUnits` | `3` | |
| `sellPriceGold` | `5` | |
| `xpReward` | `10` | |
| `weedChancePct` | `0.3` | |
| `bugChancePct` | `0.2` | |
| `waterChancePct` | `0.2` | |
| `perfectWindowMin` | `0.5` | |
| `postRipeLifeMin` | `5` | ⚠️ CRITICAL: phải > 0 |

> ⚠️ `postRipeLifeMin = 0` → `LifeAfterRipeInSeconds = 0` → cây chết ngay frame đầu sau khi chín. Xem BUG-08 trong bug-backlog.md.

---

## Known Issues (Out of Scope)

| Issue | Backlog ref |
|-------|------------|
| FarmCameraController (Old Input) + WorldObjectPicker (New Input) cùng fire khi tap | Known Issue, acceptable prototype |
| Offline growth chưa tính ticks bị miss | FEAT-05 |
| Land expansion chưa implement | FEAT-06 |
| CropTileView._tileId không thể set từ ngoài (private, no setter) → dùng gameObject.name fallback | BUG-09 |

---

## Design References

- `docs/document_md/farm_game_unity_build_map_v1.md` — M2 milestone, world prefab catalog
- `docs/document_md/farm_game_overview_7day_v2.md` — crop loop và balance targets
- `.kiro/specs/scn-main-ui-rebuild/design.md` — scene hierarchy gốc (HUD, Popup, System canvas)
- `docs/backlog/bug-backlog.md` — BUG-08, BUG-09 liên quan spec này
