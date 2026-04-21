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
- 1 Prefab (`CropTile.prefab`) — 4 SpriteRenderer overlay
- 1 Scene (`SCN_Main.unity`) — 2 BottomNav buttons (Shop, Event)

**Cleanup:**
- `Art/Sprites/UI/Legacy/` (4 file) — delete nếu 0 reference
- Old lowercase world sprites (4 file) — delete sau khi prefab repoint
- Duplicate `icon_Feed_Worm_Atomic_1.png`
- Empty folder còn sót

---

## Wire-up Strategy

### Crop SO — cấu trúc cần patch

File `crop_XX.asset` là YAML ScriptableObject, cần update các field:
```yaml
growthStageSprites:
  - {fileID: 21300000, guid: <Stage00_GUID>, type: 3}
  - {fileID: 21300000, guid: <Stage01_GUID>, type: 3}
  - {fileID: 21300000, guid: <Stage02_GUID>, type: 3}
  - {fileID: 21300000, guid: <Stage03_GUID>, type: 3}
deadSprite: {fileID: 21300000, guid: <Dead_GUID>, type: 3}
seedIcon: {fileID: 21300000, guid: <Seed_GUID>, type: 3}
cropIcon: {fileID: 21300000, guid: <Stage03_GUID>, type: 3}  # reuse
```

**Lấy GUID:** dùng `assets-find` name=`World_Crop_Carrot_Body_Stage00` → trả GUID.

**Patch:** dùng `assets-modify` path=`Assets/_Project/Data/Crops/crop_01.asset`, modifications = JSON với các field sprite + GUID tương ứng.

### Animal SO — tương tự

```yaml
stageSprites:
  - {fileID: 21300000, guid: <Stage00_GUID>, type: 3}
  - {fileID: 21300000, guid: <Stage01_GUID>, type: 3}
  - {fileID: 21300000, guid: <Stage02_GUID>, type: 3}
deadSprite: {fileID: 21300000, guid: <Dead_GUID>, type: 3}
readyToCollectIcon: {fileID: 21300000, guid: <Egg_GUID>, type: 3}
```

### CropTile Prefab wire-up

`CropTile.prefab` có 4 child GameObject với SpriteRenderer:
- `_soilRenderer` → sprite mới `World_Tile_Soil_Base_Default.png`
- `_weedVisual` → `World_Overlay_Tile_Weed_On.png`
- `_bugVisual` → `World_Overlay_Tile_Pest_On.png`
- `_waterVisual` → `World_Overlay_Tile_WaterNeed_On.png`

Dùng `assets-prefab-open` → `gameobject-find` → `gameobject-component-modify` SpriteRenderer.sprite → `assets-prefab-save` → `assets-prefab-close`.

### SCN_Main Scene wire-up

BottomNav 2 nút:
- `NavBtn_Shop` Image.sprite → `UI_Icon_Nav_Shop_Default.png`
- `NavBtn_Event` Image.sprite → `UI_Icon_Nav_Event_Default.png`

Dùng `scene-open` → `gameobject-find` → `gameobject-component-modify` Image.sprite → `scene-save`.

---

## Cleanup Strategy

**Step 1 — Verify unused before delete:**
Dùng `assets-find` search GUID của mỗi file sắp delete. Nếu có reference trong SO/Prefab/Scene → KHÔNG delete, điều tra.

**Step 2 — Delete safely:**
```
assets-delete paths = [
  "Assets/_Project/Art/Sprites/UI/Legacy/",
  "Assets/_Project/Art/Sprites/UI/Icons/Animals/Chicken/icon_Feed_Worm_Atomic_1.png",
  "Assets/_Project/Art/Sprites/World/soil_empty.png",
  "Assets/_Project/Art/Sprites/World/weed_overlay.png",
  "Assets/_Project/Art/Sprites/World/bug_overlay.png",
  "Assets/_Project/Art/Sprites/World/water_needed.png",
]
```

**Order quan trọng:**
1. Wire CropTile prefab (Task 3) TRƯỚC khi delete old world sprites (Task 7) — nếu ngược lại, prefab sẽ có `Missing` reference trong khoảng giữa.

---

## Critical Design Decisions

| Decision | Lý do |
|----------|--------|
| Wire prefab TRƯỚC khi delete old world sprites | Tránh "Missing" reference trong khoảng giữa |
| `cropIcon` = reuse `Stage03_GUID` | Tiết kiệm asset, Stage03 chín là hình ảnh crop representative nhất |
| Verify GUID search trước delete | Safety — không rơi hide-reference |
| Smoke test Play mode cuối cùng | Confirm không có "pink sprite" visual regression |
| Dùng `assets-find` lấy GUID tại runtime | Không hardcode GUID trong tasks (dễ sai) |

---

## Execution Order

1. Task 1: Wire crop_01 (Carrot) — smoke test pattern
2. Task 2: Wire crop_02..07 — batch 6 crops
3. Task 3: Wire animal_01 (Chicken), animal_02 (Duck)
4. Task 4: Wire CropTile prefab (4 SpriteRenderer)
5. Task 5: Wire SCN_Main BottomNav (2 buttons)
6. Task 6: Verify reference integrity (no broken link)
7. Task 7: Cleanup — delete Legacy + duplicates + old world sprites
8. Task 8: Integration smoke test Play mode
9. Task 9: Update HANDOVER + commit
