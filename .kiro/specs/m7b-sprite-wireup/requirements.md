# Requirements: m7b-sprite-wireup

## Overview

Wire tất cả sprite (đã rename ở m7a) vào `CropDataSO`, `AnimalDataSO`, `CropTile` prefab, và `SCN_Main` scene. Sau đó cleanup file duplicates/legacy. Play mode smoke test cuối để confirm 7 crop + 2 animal render đúng.

**Prerequisite:** m7a-sprite-reorg hoàn thành. Naming + folder đã đúng chuẩn.

**Design doc:** `docs/superpowers/specs/2026-04-21-m7-sprite-reorg-and-wireup-design.md`

---

## Global Rules

> ⚠️ ĐỌC TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO

- `assets-get-data` trước khi `assets-modify` — hiểu cấu trúc current trước khi patch
- `assets-refresh` sau mỗi SO modify, chờ re-import
- `console-get-logs` filter=Error|Warning sau mỗi task → 0 error
- Quick Test FAIL → fix trong task hiện tại, KHÔNG sang task tiếp theo (tối đa 2 retry)
- KHÔNG sửa script C# ở spec này
- Delete file chỉ khi đã verify 0 reference — dùng `assets-find` search GUID trước khi delete

---

## Functional Requirements

### Req 1 — CropDataSO Wire-up (7 SO)
- 1.1 `crop_01.asset` (Carrot): `growthStageSprites[0..3]` wire vào `World_Crop_Carrot_Body_Stage00..03.png`
- 1.2 `crop_01..07.asset`: mỗi SO có `deadSprite` trỏ `World_Crop_[X]_Body_Dead.png`
- 1.3 `crop_01..07.asset`: `seedIcon` trỏ `UI_Icon_Seed_[X]_Default.png`
- 1.4 `crop_01..07.asset`: `cropIcon` reuse Stage03 GUID (hiển thị trong Shop/Storage)
- 1.5 `crop_02..07.asset`: `growthStageSprites[0..3]` wire đủ 4 stage
- 1.6 Sau wire: `assets-get-data` → 0 field sprite nào `fileID: 0` trên 7 SO

### Req 2 — AnimalDataSO Wire-up (2 SO)
- 2.1 `animal_01.asset` (Chicken): `stageSprites[0..2]` wire `World_Animal_Chicken_Body_Stage00..02.png`
- 2.2 `animal_01.asset`: `deadSprite` = `World_Animal_Chicken_Body_Dead.png`
- 2.3 `animal_01.asset`: `readyToCollectIcon` = `World_Product_Egg_Collect_Ready.png`
- 2.4 `animal_02.asset` (Duck): `stageSprites[0..2]` wire 3 stage
- 2.5 `animal_02.asset`: `deadSprite`, `readyToCollectIcon` (DuckEgg) wire

### Req 3 — CropTile Prefab Wire-up
- 3.1 `CropTile.prefab` SpriteRenderer `_soilRenderer`: sprite = `World_Tile_Soil_Base_Default.png`
- 3.2 `_weedVisual`: sprite = `World_Overlay_Tile_Weed_On.png`
- 3.3 `_bugVisual`: sprite = `World_Overlay_Tile_Pest_On.png`
- 3.4 `_waterVisual`: sprite = `World_Overlay_Tile_WaterNeed_On.png`

### Req 4 — BottomNav Scene Wire-up
- 4.1 `SCN_Main.unity` `NavBtn_Shop` Image component: sprite = `UI_Icon_Nav_Shop_Default.png`
- 4.2 `NavBtn_Event` Image: sprite = `UI_Icon_Nav_Event_Default.png`
- 4.3 Các nút khác (Farm, Storage, Barn) verify vẫn đúng sau rename

### Req 5 — Cleanup (Delete unused files)
- 5.1 Delete `Art/Sprites/UI/Legacy/` (4 file: Apple, Carrot-flat, Sprout-flat, Wheat-flat) sau khi verify 0 reference
- 5.2 Delete duplicates: `icon_Feed_Worm_Atomic_1.png`
- 5.3 Delete old lowercase world sprites sau khi CropTile prefab đã repoint:
  - `soil_empty.png`, `weed_overlay.png`, `bug_overlay.png`, `water_needed.png`
- 5.4 Delete empty folders: `Icons/Animals/` (nếu còn), `Icons/Crops/[old]/`

### Req 6 — Integration
- 6.1 Play mode: plant 7 crop khác nhau → verify Stage0 → Stage3 sprite hiện đúng trên tile
- 6.2 Force ailment (weed/bug/water) → overlay mới hiện đúng
- 6.3 AnimalPen: verify Chicken Stage00-02 + Duck Stage00-02 render đúng
- 6.4 Collect egg/duck egg → readyToCollectIcon hiện
- 6.5 BottomNav: icon Shop + Event hiện sprite mới (không còn placeholder)
- 6.6 Console: 0 "missing sprite", 0 NullReferenceException, 0 pink render
