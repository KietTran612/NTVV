# Design Document: m7b-sprite-wireup

**Date:** 2026-04-21
**Spec:** m7b-sprite-wireup
**Status:** approved
**Full design:** `docs/superpowers/specs/2026-04-21-m7-sprite-reorg-and-wireup-design.md`

---

## Overview

Sau khi m7a đã rename/organize sprite, m7b wire tất cả vào SO/Prefab/Scene + cleanup file không dùng.

**Phạm vi wire-up:**
- 7 `CropDataSO` (crop_01..07)
- 2 `AnimalDataSO` (animal_01, animal_02)
- 1 Prefab: `Assets/_Project/Prefabs/World/CropTile.prefab` — 4 SpriteRenderer (SoilRenderer + WaterVisual + WeedVisual + BugVisual)
- 1 Scene (`SCN_Main.unity`) — 2 BottomNav buttons (Shop, Event)

**Cleanup:**
- `Art/Sprites/UI/Legacy/` (4 file) — delete nếu 0 reference
- Old lowercase world sprites (4 file) — delete sau khi prefab repoint
- Duplicate `icon_Feed_Worm_Atomic_1.png`
- Empty folders sau rename

---

## CropDataSO Field Topology (grep verified)

`CropDataSO : ItemData` — kế thừa `ItemData` nên có field `icon` top-level.

```csharp
// ItemData.cs (base)
public Sprite icon;         // top-level inherited

// CropDataSO.cs
public CropData data;       // nested struct
public Sprite[] growthStageSprites;
public Sprite deadSprite;
public Sprite seedIcon;     // top-level (duplicate với data.seedIcon)
public Sprite cropIcon;

// CropData.cs (struct)
public Sprite seedIcon;     // nested — UI ĐANG DÙNG field này
```

**Usage grep kết quả:**
- `.icon\b` trong Scripts/ → 0 match → top-level `icon` **KHÔNG ai dùng**
- `data.seedIcon` → `ShopPanelController:105`, `StoragePanelController:133` → **UI dùng field này**
- Top-level `seedIcon` → 0 usage → duplicate

**Wire decision:**
| Field | Action |
|-------|--------|
| `icon` (inherited) | **SKIP** — không chạm, để GUID hiện tại |
| `growthStageSprites[0..3]` | WIRE 4 stage |
| `deadSprite` | WIRE |
| `data.seedIcon` | WIRE (UI dùng) |
| `seedIcon` (top-level) | WIRE sync GUID với `data.seedIcon` (tránh drift) |
| `cropIcon` | WIRE = Stage03 GUID (reuse) |

## AnimalDataSO Field Topology

```csharp
// AnimalDataSO.cs
public AnimalData data;
public Sprite[] stageSprites;      // 3 stages
public Sprite deadSprite;
public Sprite readyToCollectIcon;  // egg/milk icon
// icon inherited
```

`AnimalData` struct — không có sprite field (grep xác nhận).

Wire: `stageSprites[0..2]`, `deadSprite`, `readyToCollectIcon`. Skip `icon`.

---

## CropTile Prefab Structure (verified từ prefab YAML)

**Path chính xác:** `Assets/_Project/Prefabs/World/CropTile.prefab`

**Hierarchy:**
```
CropTile (root, có CropTileView component)
├─ SoilRenderer?           (referenced by `_soilRenderer: SpriteRenderer`)
├─ WaterVisual             (GameObject — `_waterVisual: GameObject`)
│   └─ SpriteRenderer component trực tiếp TRÊN CHÍNH GameObject này
├─ WeedVisual              (GameObject — `_weedVisual: GameObject`)
│   └─ SpriteRenderer component trực tiếp
└─ BugVisual               (GameObject — `_bugVisual: GameObject`)
    └─ SpriteRenderer component trực tiếp
```

**Kiểu field trong CropTileView.cs:**
```csharp
[SerializeField] private SpriteRenderer _soilRenderer;    // direct component
[SerializeField] private GameObject _weedVisual;          // GO (với SpriteRenderer ON it)
[SerializeField] private GameObject _bugVisual;           // GO (với SpriteRenderer ON it)
[SerializeField] private GameObject _waterVisual;         // GO (với SpriteRenderer ON it)
```

**Code dùng:**
- Line 208-210: `_weedVisual.SetActive(_hasWeeds)` — GameObject level, chỉ show/hide
- SpriteRenderer trên các GameObject này render sprite tương ứng (Weed_On, Pest_On, WaterNeed_On)

**Wiring approach:** Dùng `gameobject-component-modify` trực tiếp trên từng Visual GameObject để update SpriteRenderer.m_Sprite.

**Current sprite references trong prefab (cần update):**
- WaterVisual SpriteRenderer: GUID `b73d372af6cd515499d753df7754f2ec` (= `water_needed.png` OLD) → đổi thành `World_Overlay_Tile_WaterNeed_On.png` GUID
- WeedVisual, BugVisual, SoilRenderer tương tự (GUIDs thấy trong prefab: `dc785afe`, `f5c75128`, `b730247c`, `4a780f59` — một số là Carrot Stage sprites nhầm chỗ hoặc old overlay)

---

## Scene BottomNav Structure (verified từ SCN_Main.unity)

GameObject `NavBtn_Shop` (line 3601), `NavBtn_Event` (line 2780), `NavBtn_Farm` (line 5893) — exist trong scene.

Mỗi nút có `Image` component (UI) với `m_Sprite` field. `gameobject-component-modify` update `m_Sprite` GUID.

---

## Cleanup Strategy

**Step 1 — Verify unused trước delete:**
Dùng `Bash grep -rn "<GUID>"` trên `Assets/_Project/` để cross-check reference. (`assets-find` tool search theo name/type, KHÔNG search reference-của-GUID.)

**Step 2 — Delete theo thứ tự file → folder:**
1. File trong Legacy/ (4 file)
2. Folder Legacy/ (empty)
3. Duplicate `icon_Feed_Worm_Atomic_1.png`
4. 4 old lowercase world sprite (SAU KHI task 4 đã repoint prefab)
5. Empty folders sau rename

**Risk mitigation:** Task 4 (prefab wire) MUST DONE trước task 7 (delete). Giữa task 4 và task 7 là task 6 (verify 0 broken reference).

---

## Smoke Test Strategy (API-accurate)

**Problem:** Spec cũ dùng API fictional (`TimeManager.AddTime`, `CropTileView.SetStage`, `tile.ApplyAilment`) — không tồn tại.

**Fixed approach:**

1. **Speed up time:** `reflection-method-call` set `TimeManager.Instance._tickRate = 0.01f` (private SerializeField). Restore cuối test.
2. **Force stage:** Không có public API. Thay vào đó:
   - Plant + wait với tick_rate 100× speedup
   - HOẶC reflection set private field `_currentStage` nếu tồn tại
3. **Force ailment:** Reflection set private bool `_hasWeeds`/`_hasPests`/`_needsWater` = true (verified fields), rồi gọi `RefreshVisuals()` qua reflection (private method).
4. **Verify visuals:** `screenshot-game-view` + `gameobject-component-get` SpriteRenderer.m_Sprite match expected GUID.

---

## Critical Design Decisions

| Decision | Lý do |
|----------|--------|
| SKIP top-level `icon` field | 0 usage trong Scripts (grep verified). Giữ GUID hiện tại để không break semantic tiềm ẩn. |
| WIRE cả `data.seedIcon` + top-level `seedIcon` sync GUID | UI dùng `data.seedIcon`, top-level có thể legacy nhưng đồng bộ để tránh drift |
| `cropIcon` = reuse Stage03 GUID | Stage03 (Ripe) là hình ảnh representative của crop |
| Wire prefab TRƯỚC delete old world sprites | Tránh "Missing sprite" reference giữa chừng |
| `Bash grep GUID` thay vì `assets-find` cho reference check | Tool `assets-find` search name/type, không phải reference |
| Smoke test dùng reflection, KHÔNG public API giả định | Spec cũ fictional, cần match runtime thật |
| Xóa file → folder (2 step) | `assets-delete` có thể không hỗ trợ folder empty delete; an toàn hơn |

---

## Execution Order

1. Task 1: Wire crop_01 (pattern smoke test)
2. Task 2: Wire crop_02..07 (6 crops)
3. Task 3: Wire animal_01 + animal_02
4. Task 4: Wire CropTile prefab (4 SpriteRenderer)
5. Task 5: Wire SCN_Main BottomNav (2 buttons)
6. Task 6: Reference integrity check (grep GUID)
7. Task 7: Cleanup — delete Legacy + duplicates + old world sprites (theo order file → folder)
8. Task 8: Integration smoke test Play mode (reflection-based)
9. Task 9: Update HANDOVER + commit
