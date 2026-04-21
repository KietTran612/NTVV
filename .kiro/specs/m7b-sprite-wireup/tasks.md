# Implementation Plan: m7b-sprite-wireup

## Overview

Wire ~60 sprite slot vào 9 SO + 1 prefab + 1 scene, cleanup ~10 file không dùng, smoke test Play mode. Sprite đã rename/organize ở m7a.

**Design doc:** `.kiro/specs/m7b-sprite-wireup/design.md`
**Requirements:** `.kiro/specs/m7b-sprite-wireup/requirements.md`

> ⚠️ ĐỌC GLOBAL RULES trong requirements.md TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO.
> ⚠️ Prerequisite: m7a-sprite-reorg DONE.

---

## Tasks

- [ ] 1. Wire crop_01 (Carrot) — pattern smoke test
  - [ ] 1.0 · Resource Check
    - `assets-find` filter=`World_Crop_Carrot_Body_Stage00` → 1 match, lấy GUID
    - Tương tự lấy GUID Stage01, 02, 03, Dead
    - `assets-find` filter=`UI_Icon_Seed_Carrot_Default` → lấy GUID
    - BLOCKING nếu thiếu file
  - [ ] 1.4 · Asset Agent — Patch crop_01.asset
    - `assets-get-data` path=`Assets/_Project/Data/Crops/crop_01.asset` → xem cấu trúc hiện tại
    - `assets-modify` path=`crop_01.asset` với modifications:
      - `growthStageSprites[0..3]` = [Stage00, Stage01, Stage02, Stage03] GUIDs
      - `deadSprite` = Dead GUID
      - `seedIcon` = Seed_Carrot GUID
      - `cropIcon` = Stage03 GUID (reuse)
    - `assets-refresh`
    - `console-get-logs` filter=Error → 0
    - _Requirements: 1.1, 1.2, 1.3, 1.4_
  - [ ] 1.✓ · Quick Test
    - `assets-get-data` path=`crop_01.asset` → verify 4 `growthStageSprites` guid khác null, `deadSprite` guid != 0, `seedIcon` guid != 0, `cropIcon` == `growthStageSprites[3]` guid
    - Nếu FAIL → fix task 1, KHÔNG sang task 2

- [ ] 2. Wire crop_02..07 (Potato, Corn, Wheat, Tomato, Strawberry, Pumpkin)
  - [ ] 2.0 · Resource Check
    - Mỗi crop X: lấy GUID của Stage00-03, Dead, Seed
    - 6 crop × 6 sprite = 36 GUID lookup
    - BLOCKING nếu thiếu
  - [ ] 2.4 · Asset Agent — Batch 6 crop SOs
    - Theo thứ tự (để dễ debug nếu fail):
      - crop_02 = Potato → wire theo pattern task 1
      - crop_03 = Corn
      - crop_04 = Wheat
      - crop_05 = Tomato
      - crop_06 = Strawberry
      - crop_07 = Pumpkin
    - Mỗi SO: `assets-modify` với 4 stage + dead + seed + cropIcon (reuse Stage03)
    - `assets-refresh` sau mỗi SO
    - `console-get-logs` filter=Error → 0
    - _Requirements: 1.5, 1.6_
  - [ ] 2.✓ · Quick Test
    - Duyệt 6 SO: mỗi SO có 4 growth + dead + seed + crop icon guid != 0
    - Nếu FAIL → fix task 2, KHÔNG sang task 3

- [ ] 3. Wire animal_01 (Chicken) + animal_02 (Duck)
  - [ ] 3.0 · Resource Check
    - GUID: Chicken Stage00-02, Dead, Egg product
    - GUID: Duck Stage00-02, Dead, DuckEgg product
    - BLOCKING nếu thiếu
  - [ ] 3.4 · Asset Agent — Wire 2 animal SOs
    - `animal_01.asset` (Chicken):
      - `stageSprites[0..2]` = Stage00, Stage01, Stage02 GUIDs
      - `deadSprite` = Dead GUID
      - `readyToCollectIcon` = Egg GUID
    - `animal_02.asset` (Duck):
      - `stageSprites[0..2]` = Duck Stage00-02 GUIDs
      - `deadSprite` = Duck Dead GUID
      - `readyToCollectIcon` = DuckEgg GUID
    - `assets-refresh`
    - `console-get-logs` filter=Error → 0
    - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.5_
  - [ ] 3.✓ · Quick Test
    - `assets-get-data` animal_01 → 3 stageSprites + deadSprite + readyToCollectIcon guid != 0
    - `assets-get-data` animal_02 → tương tự
    - Nếu FAIL → fix task 3, KHÔNG sang task 4

- [ ] 4. Wire CropTile prefab — 4 overlay SpriteRenderer
  - [ ] 4.0 · Resource Check
    - `assets-find` filter=`CropTile.prefab` → 1 match
    - GUID: Soil_Base, Weed_On, Pest_On, WaterNeed_On sprites
    - BLOCKING nếu thiếu
  - [ ] 4.4 · Prefab Agent — Update CropTile prefab
    - `assets-prefab-open` path=`Assets/_Project/Prefabs/CropTile.prefab`
    - `gameobject-find` trong prefab: tìm child `_soilRenderer`, `_weedVisual`, `_bugVisual`, `_waterVisual` (hoặc tên hiện có trong prefab — check hierarchy)
    - `gameobject-component-modify` SpriteRenderer.sprite cho mỗi renderer:
      - Soil → `World_Tile_Soil_Base_Default.png`
      - Weed → `World_Overlay_Tile_Weed_On.png`
      - Bug/Pest → `World_Overlay_Tile_Pest_On.png`
      - Water → `World_Overlay_Tile_WaterNeed_On.png`
    - `assets-prefab-save`
    - `assets-prefab-close`
    - `console-get-logs` filter=Error → 0
    - _Requirements: 3.1, 3.2, 3.3, 3.4_
  - [ ] 4.✓ · Quick Test
    - `assets-get-data` CropTile.prefab → 4 SpriteRenderer sprite reference mới (không "Missing")
    - Nếu FAIL → fix task 4, KHÔNG sang task 5

- [ ] 5. Wire SCN_Main — BottomNav Shop + Event buttons
  - [ ] 5.0 · Resource Check
    - GUID: `UI_Icon_Nav_Shop_Default`, `UI_Icon_Nav_Event_Default`
    - BLOCKING nếu thiếu
  - [ ] 5.4 · Scene Agent — Update NavBtn sprites
    - `scene-open` `Assets/_Project/Scenes/SCN_Main.unity`
    - `gameobject-find` name=`NavBtn_Shop` → lấy ID
    - `gameobject-component-modify` Image.sprite = Shop GUID
    - `gameobject-find` name=`NavBtn_Event` → lấy ID
    - `gameobject-component-modify` Image.sprite = Event GUID
    - `scene-save`
    - `console-get-logs` filter=Error → 0
    - _Requirements: 4.1, 4.2_
  - [ ] 5.✓ · Quick Test
    - Verify `NavBtn_Shop` Image.sprite = Shop GUID
    - Verify `NavBtn_Event` Image.sprite = Event GUID
    - Nếu FAIL → fix task 5, KHÔNG sang task 6

- [ ] 6. Reference integrity check (pre-cleanup)
  - [ ] 6.0 · Resource Check — verify 0 broken reference
    - `console-clear-logs` → `assets-refresh`
    - `console-get-logs` filter=Warning → 0 "missing sprite", "Could not extract GUID", "Broken reference"
    - NON-BLOCKING nhưng cần fix trước cleanup
  - [ ] 6.1 · Verify Legacy unused
    - Với mỗi file trong `Art/Sprites/UI/Legacy/` (4 file):
      - `assets-get-data` lấy GUID từ `.meta`
      - `assets-find` filter=`<GUID>` → verify 0 reference (chỉ chính file + .meta)
    - Nếu có reference → KHÔNG delete, ghi vào HANDOVER để điều tra
    - _Requirements: 5.1_
  - [ ] 6.2 · Verify old world sprites chỉ CropTile prefab reference
    - `soil_empty.png`, `weed_overlay.png`, `bug_overlay.png`, `water_needed.png`
    - Sau task 4 đã repoint CropTile prefab → 0 reference expected
    - `assets-find` filter=GUID → 0 match
    - _Requirements: 5.3_

- [ ] 7. Cleanup — delete unused files
  - [ ] 7.0 · Resource Check
    - Verify task 6 PASS (0 broken reference)
    - BLOCKING nếu task 6 fail
  - [ ] 7.4 · Asset Agent — Delete files
    - `assets-delete` paths:
      - `Assets/_Project/Art/Sprites/UI/Legacy/` (4 file — Apple, Carrot-flat, Sprout-flat, Wheat-flat)
      - `Assets/_Project/Art/Sprites/UI/Icons/Animals/Chicken/icon_Feed_Worm_Atomic_1.png`
      - `Assets/_Project/Art/Sprites/World/soil_empty.png`
      - `Assets/_Project/Art/Sprites/World/weed_overlay.png`
      - `Assets/_Project/Art/Sprites/World/bug_overlay.png`
      - `Assets/_Project/Art/Sprites/World/water_needed.png`
    - `assets-delete` empty folders: `Icons/Animals/`, `Icons/Crops/` (nếu đã rỗng)
    - `assets-refresh`
    - `console-get-logs` filter=Error → 0
    - _Requirements: 5.1, 5.2, 5.3, 5.4_
  - [ ] 7.✓ · Quick Test
    - `Bash ls Assets/_Project/Art/Sprites/UI/Legacy/` → không tồn tại
    - `Bash ls Assets/_Project/Art/Sprites/World/*.png` → chỉ file naming `World_*_On.png` / `World_*_Default.png` (không còn lowercase)
    - Console: 0 broken reference
    - Nếu FAIL → fix task 7, KHÔNG sang task 8

- [ ] 8. Integration smoke test — Play mode
  - [ ] 8.1 Boot check
    - `editor-application-set-state` → Play mode
    - `console-get-logs` filter=Error → 0 error khi load
    - `screenshot-game-view` → verify 0 pink sprite trong view
    - _Requirements: 6.6_
  - [ ] 8.2 Crop lifecycle test
    - `reflection-method-call` → `EconomySystem.Instance.AddGold(100)` để có gold
    - Tap 1 tile → "Gieo Hạt" → chọn crop_01 (Carrot) → plant
    - `screenshot-game-view` → verify Stage00 sprite hiện trên tile
    - `reflection-method-call` → `TimeManager.Instance.AddTime(120)` (speed up)
    - Verify: sprite chuyển Stage01 → Stage02 → Stage03
    - Hoặc force stage: `reflection-method-call` → gọi `CropTileView.SetStage(3)`
    - _Requirements: 6.1_
  - [ ] 8.3 Ailment overlay test
    - Chọn 1 tile đang Growing
    - `reflection-method-call` → force `tile.ApplyAilment(Weed)` hoặc edit `_weed=true`
    - `screenshot-game-view` → verify overlay `World_Overlay_Tile_Weed_On.png` hiện
    - Tương tự cho Pest, Water
    - _Requirements: 6.2_
  - [ ] 8.4 Animal rendering test
    - Mở AnimalPen (hoặc spawn animal_01 prefab)
    - `screenshot-game-view` → verify Chicken Stage00 sprite hiện
    - Force mature: `reflection-method-call` hoặc AddTime
    - Verify Stage01 → Stage02
    - Collect egg: verify egg sprite hiện
    - _Requirements: 6.3, 6.4_
  - [ ] 8.5 BottomNav test
    - `screenshot-game-view` → focus BottomNav area
    - Verify: icon Shop (không phải Sprout placeholder), icon Event (không phải Tab_Star placeholder)
    - _Requirements: 6.5_
  - [ ] 8.✓ · Integration Test Report
    - `screenshot-game-view` → lưu `docs/screenshots/2026-04-21-m7-sprite-wireup-integration.png`
    - Output:
      ```
      📋 INTEGRATION TEST REPORT: m7b-sprite-wireup
      ✅ PASSED: [N] checks
      ⚠️ WARNINGS: [danh sách]
      ❌ FAILED: [N] — [mô tả]
      Crop lifecycle rendering: ✅ 7/7
      Ailment overlays: ✅ 3/3
      Animal rendering: ✅ 2/2
      BottomNav icons: ✅
      Console: 0 "missing sprite" ✅
      ```
    - Nếu FAIL critical → fix, KHÔNG sang task 9

- [ ] 9. Cập nhật HANDOVER.md + commit
  - Mở `docs/HANDOVER.md`
  - Thêm section phiên 21/04/2026 — m7b-sprite-wireup DONE:
    - Wire ~60 sprite slot vào 9 SO (7 crop + 2 animal)
    - Update CropTile prefab (4 overlay SpriteRenderer)
    - Update SCN_Main BottomNav (2 button sprite)
    - Cleanup: delete Legacy folder, old lowercase world sprites, duplicates
    - Play mode smoke test PASS: 7 crop lifecycle render đúng, animal 2 loài render đúng, 0 pink sprite
  - Cập nhật Kiro Specs: thêm Spec 9 `m7a-sprite-reorg` + Spec 10 `m7b-sprite-wireup` = DONE
  - Commit:
    - `git add Assets/_Project/Art/Sprites/ Assets/_Project/Data/ Assets/_Project/Prefabs/ Assets/_Project/Scenes/SCN_Main.unity docs/HANDOVER.md .kiro/specs/m7b-sprite-wireup/ docs/screenshots/`
    - Commit message: `feat(assets): m7b — wire all sprites to SOs/Prefab/Scene + cleanup`
  - _Requirements: tất cả_
