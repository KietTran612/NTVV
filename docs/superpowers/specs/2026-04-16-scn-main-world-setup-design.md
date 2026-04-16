# Design Document: scn-main-world-setup

**Date:** 2026-04-16
**Spec:** scn-main-world-setup
**Status:** approved
**Kiro Spec Path:** `.kiro/specs/scn-main-world-setup/`

---

## Overview

Kiro spec cho M2 milestone của NTVV — world gameplay layer cho SCN_Main. Sau khi `scn-main-ui-rebuild` hoàn thành UI scaffold, spec này setup phần world: CropTile prefab, physics interaction, input system, camera pan, data validation. Kết quả: player tap vào ô đất → CropActionPanel mở → gameplay loop trồng/chăm/thu hoạt động.

---

## Approach

**Option B Revised** (được chọn): Pure MCP + 1 Editor Script mới (`CropGridSpawner.cs`)

- Tạo CropTile prefab với BoxCollider 3D + CropTileView
- Viết `CropGridSpawner.cs` (ExecuteInEditMode, ContextMenu) để generate 6 tiles tự động
- Setup WorldObjectPicker + PlayerInput (cùng GO), FarmCameraController
- Verify TimeManager, QuestManager, GameDataRegistrySO

Không dùng Option A (manual placement) vì sẽ khó scale khi thêm tiles sau.
Không dùng Option B cũ (runtime spawner) để giữ scene-baked approach cho SaveLoad.

---

## Architecture

```
SCN_Main
├── [WORLD_ROOT]
│   ├── Main Camera (tag=MainCamera, FarmCameraController)
│   ├── CropArea
│   │   ├── tile_r0_c0 ... tile_r1_c2  (6 CropTile instances)
│   └── BarnArea (reserved)
└── [SYSTEMS]
    ├── ... (từ spec cũ)
    ├── TimeManager (_tickRate=1.0)
    ├── QuestManager
    └── WorldObjectPicker (+ PlayerInput cùng GO)
```

---

## Critical Design Decisions

| Decision | Lý do |
|----------|-------|
| BoxCollider 3D (không phải 2D) | WorldObjectPicker dùng `Physics.Raycast` 3D |
| PlayerInput cùng GO với WorldObjectPicker | Send Messages chỉ dispatch đến cùng GO |
| tag "MainCamera" bắt buộc | PopupManager.PositionContextPanel() dùng `Camera.main` |
| Tên GO = "tile_r{r}_c{c}" | CropTileView._tileId private, SaveLoad dùng gameObject.name fallback |
| postRipeLifeMin > 0 (critical) | = 0 → cây chết ngay khi chín (LifeAfterRipeInSeconds = 0) |

---

## Issues Phát Hiện Trong Review

### BUG-08 (HIGH): postRipeLifeMin = 0 → cây chết ngay
- `LifeAfterRipeInSeconds = postRipeLifeMin * 60f = 0`
- `if (_timeSinceRipe > 0)` → true ngay frame đầu → Dead state
- Fix: Set `postRipeLifeMin = 5` trong CropDataSO, validate trong Registry.Initialize()

### BUG-09 (LOW): CropTileView._tileId no setter
- Private field, không set được từ CropGridSpawner
- Workaround: Dùng gameObject.name = "tile_r{r}_c{c}" làm tileId
- Fix dài hạn: Thêm public SetTileId() hoặc dùng SerializedObject trong Editor

### NOTE-01: FarmCameraController Old/New Input conflict
- Camera dùng `Input.GetMouseButtonDown` (old), WorldObjectPicker dùng PlayerInput (new)
- Cùng fire khi tap → acceptable prototype, fix sau

### NOTE-02: EconomySystem gold = 0 ban đầu
- Player cần gold ≥ seedCostGold để plant
- Integration test cần set gold ban đầu hoặc set seedCostGold = 0 cho test

---

## Task Summary (9 tasks)

| Task | Mô tả | Agent |
|------|-------|-------|
| 0 | Interactable Layer + Camera tag | Unity Agent |
| 1 | CropGridSpawner.cs | Script Agent |
| 2 | CropTile prefab | Unity + Script Agent |
| 3 | Generate 6 tiles | Unity Agent |
| 4 | WorldObjectPicker + PlayerInput | Script Agent |
| 5 | FarmCameraController | Script Agent |
| 6 | Verify TimeManager + QuestManager | Unity Agent |
| 7 | Data check GameDataRegistrySO | Script Agent |
| 8 | Integration Test | Test Agent |
| 9 | Update HANDOVER.md | - |

---

## References

- `docs/document_md/farm_game_unity_build_map_v1.md` — M2 milestone
- `docs/document_md/farm_game_overview_7day_v2.md` — balance targets
- `.kiro/specs/scn-main-ui-rebuild/design.md` — scene hierarchy gốc
- `docs/backlog/bug-backlog.md` — BUG-08, BUG-09, NOTE-01 đến NOTE-03
- `docs/superpowers/specs/2026-04-16-kiro-agent-pipeline-design.md` — task format reference
