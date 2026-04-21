# Implementation Plan: m7b-sprite-wireup

## Overview

Wire sprite vào 9 SO + 1 prefab + 1 scene, cleanup file không dùng, smoke test Play mode. Sprite đã rename/organize ở m7a.

**Design doc:** `.kiro/specs/m7b-sprite-wireup/design.md`
**Requirements:** `.kiro/specs/m7b-sprite-wireup/requirements.md`

> ⚠️ ĐỌC GLOBAL RULES trong requirements.md TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO.
> ⚠️ Prerequisite: m7a-sprite-reorg DONE.

---

## CropDataSO Field Topology (QUAN TRỌNG — đọc trước Task 1)

CropDataSO kế thừa `ItemData` → có field `icon` top-level (inherited). Bản thân nó cũng chứa `data: CropData` (nested struct có `data.seedIcon` riêng). Tổng 4 sprite field cần quyết định:

| Field | Location | Usage (đã grep code) | Quyết định wire |
|-------|----------|----------------------|-----------------|
| `icon` | top-level (từ `ItemData`) | **0 usage trong Scripts/** (grep `\.icon\b`) | **KHÔNG wire** — để nguyên |
| `data.seedIcon` | nested (`CropData` struct) | `ShopPanelController.cs:105` + `StoragePanelController.cs:133` | **WIRE** — field chính UI đang dùng |
| `seedIcon` | top-level (`CropDataSO`) | 0 usage tìm thấy — duplicate hoặc legacy | **WIRE** đồng bộ GUID với `data.seedIcon` (tránh drift) |
| `cropIcon` | top-level (`CropDataSO`) | Storage/Inventory display (theo comment) | **WIRE** — reuse Stage03 GUID |
| `growthStageSprites[0..3]` | top-level | `CropTileView` rendering | **WIRE** — 4 stage |
| `deadSprite` | top-level | `CropTileView` dead state | **WIRE** |

**Lưu ý:** Tất cả 7 `crop_XX.asset` hiện có `icon` top-level đã trỏ GUID (vd crop_01: `33b9f4eb...`) — sprite đó không nằm trong `Art/Sprites/` nữa (có thể đã xóa, hoặc là Icon Atomic cũ). KHÔNG đụng để tránh vô tình thay đổi semantic Shop/Storage list display. Nếu sau này muốn đồng nhất, làm spec riêng.

## AnimalDataSO Field Topology

| Field | Location | Usage | Quyết định |
|-------|----------|-------|-----------|
| `icon` | top-level (từ `ItemData`) | 0 usage code | **KHÔNG wire** |
| `stageSprites[0..2]` | top-level | `AnimalView` rendering | **WIRE** |
| `deadSprite` | top-level | Dead state render | **WIRE** |
| `readyToCollectIcon` | top-level | Collect button icon | **WIRE** |

---

## Tasks

- [x] 1. Wire crop_01 (Carrot) — pattern smoke test
  - [x] 1.0 · Resource Check
    - `assets-find` filter=`World_Crop_Carrot_Body_Stage00` → 1 match, lấy GUID
    - Tương tự lấy GUID: Stage01, Stage02, Stage03, Dead
    - `assets-find` filter=`UI_Icon_Seed_Carrot_Default` → lấy GUID
    - BLOCKING nếu bất kỳ file nào thiếu (m7a chưa DONE)
  - [x] 1.4 · Asset Agent — Patch crop_01.asset
    - `assets-get-data` path=`Assets/_Project/Data/Crops/crop_01.asset` → record GUID hiện tại của `icon` top-level (để verify không đổi)
    - `assets-modify` path=`crop_01.asset` modifications:
      - `growthStageSprites` = [Stage00, Stage01, Stage02, Stage03] GUIDs (4 phần tử)
      - `deadSprite` = Dead GUID
      - `data.seedIcon` = Seed_Carrot GUID  (field UI dùng)
      - `seedIcon` = Seed_Carrot GUID (top-level — đồng bộ)
      - `cropIcon` = Stage03 GUID (reuse)
      - **KHÔNG chạm** `icon` top-level (inherited)
    - `assets-refresh`
    - `console-get-logs` filter=Error → 0
    - _Requirements: 1.1, 1.2, 1.3, 1.4_
  - [x] 1.✓ · Quick Test
    - `assets-get-data` crop_01.asset verify:
      - `growthStageSprites[0..3]` có 4 GUID đúng
      - `deadSprite` GUID match Dead sprite
      - `data.seedIcon` GUID == `seedIcon` top-level (cả 2 field đồng bộ)
      - `cropIcon` GUID == `growthStageSprites[3]` GUID
      - `icon` top-level GUID KHÔNG đổi so với trước patch
    - Nếu FAIL → fix task 1, KHÔNG sang task 2

- [x] 2. Wire crop_02..07 (6 SO: Potato, Corn, Wheat, Tomato, Strawberry, Pumpkin)
  - [x] 2.0 · Resource Check
    - Map SO ID → Crop Name:
      - crop_02 = Potato, crop_03 = Corn, crop_04 = Wheat
      - crop_05 = Tomato, crop_06 = Strawberry, crop_07 = Pumpkin
    - Mỗi SO: `assets-find` lấy 6 GUID (Stage00-03 + Dead + Seed)
    - BLOCKING nếu thiếu bất kỳ GUID nào
  - [x] 2.4 · Asset Agent — Batch 6 crop SOs
    - Lặp theo thứ tự crop_02 → crop_07, mỗi SO áp dụng cùng pattern task 1.4:
      - `growthStageSprites` = 4 stage GUIDs
      - `deadSprite`, `data.seedIcon`, `seedIcon`, `cropIcon` (reuse Stage03)
      - KHÔNG chạm `icon` top-level
    - `assets-refresh` sau mỗi SO (hoặc batch 3 rồi refresh)
    - `console-get-logs` filter=Error → 0
    - _Requirements: 1.5, 1.6_
  - [x] 2.✓ · Quick Test
    - 6 SO: mỗi SO có 4 stage + dead + 2 seedIcon đồng bộ + cropIcon GUID != 0
    - `icon` top-level không đổi (record trước + sau)
    - Nếu FAIL → fix task 2, KHÔNG sang task 3

- [x] 3. Wire animal_01 (Chicken) + animal_02 (Duck)
  - [x] 3.0 · Resource Check
    - Chicken GUID: Stage00, Stage01, Stage02, Dead, `World_Product_Egg_Collect_Ready`
    - Duck GUID: Stage00, Stage01, Stage02, Dead, `World_Product_DuckEgg_Collect_Ready`
    - BLOCKING nếu thiếu
  - [x] 3.4 · Asset Agent — Patch 2 animal SOs
    - `animal_01.asset` (Chicken):
      - `stageSprites` = [Chicken Stage00, Stage01, Stage02] GUIDs
      - `deadSprite` = Chicken Dead GUID
      - `readyToCollectIcon` = Egg GUID
      - KHÔNG chạm `icon` top-level
    - `animal_02.asset` (Duck):
      - `stageSprites` = [Duck Stage00, Stage01, Stage02] GUIDs
      - `deadSprite` = Duck Dead GUID
      - `readyToCollectIcon` = DuckEgg GUID
    - `assets-refresh`
    - `console-get-logs` filter=Error → 0
    - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.5_
  - [x] 3.✓ · Quick Test
    - animal_01: 3 stageSprites + deadSprite + readyToCollectIcon GUID != 0
    - animal_02: tương tự
    - Nếu FAIL → fix task 3, KHÔNG sang task 4

- [x] 4. Wire CropTile prefab — 4 overlay sprite (SpriteRenderer ON each Visual GameObject)
  - [x] 4.0 · Resource Check
    - **Path chính xác:** `Assets/_Project/Prefabs/World/CropTile.prefab` (không phải `Prefabs/` gốc)
    - `assets-find` filter=`CropTile.prefab` → 1 match, verify path
    - GUID sprite mới:
      - `World_Tile_Soil_Base_Default.png`
      - `World_Overlay_Tile_Weed_On.png`
      - `World_Overlay_Tile_Pest_On.png`
      - `World_Overlay_Tile_WaterNeed_On.png`
    - BLOCKING nếu thiếu
  - [x] 4.4 · Prefab Agent — Update CropTile prefab SpriteRenderers
    - **Cấu trúc thực tế** (verified bằng grep prefab):
      - CropTile root GameObject có `CropTileView` component
      - `_soilRenderer` field kiểu `SpriteRenderer` → trực tiếp là component (có thể trên chính root hoặc child "SoilRenderer")
      - `_weedVisual`, `_bugVisual`, `_waterVisual` field kiểu `GameObject` (tên child: `WeedVisual`, `BugVisual`, `WaterVisual`) — **mỗi GameObject có `SpriteRenderer` component trực tiếp trên chính nó** (không phải child)
    - `assets-prefab-open` path=`Assets/_Project/Prefabs/World/CropTile.prefab`
    - `gameobject-find` trong prefab: lấy ID của `WaterVisual`, `WeedVisual`, `BugVisual` (tên GameObject exact)
    - Với mỗi GameObject (WaterVisual/WeedVisual/BugVisual):
      - `gameobject-component-get` component=SpriteRenderer → confirm có component
      - `gameobject-component-modify` sửa field `m_Sprite` → GUID sprite mới tương ứng:
        - `WaterVisual` → WaterNeed GUID
        - `WeedVisual` → Weed GUID
        - `BugVisual` → Pest GUID
    - Cho SoilRenderer: tìm GameObject có SpriteRenderer + là `_soilRenderer` reference. Nếu có GameObject tên `SoilRenderer` → wire tương tự. Nếu `_soilRenderer` trực tiếp trên root → `gameobject-component-modify` root's SpriteRenderer → Soil GUID.
    - `assets-prefab-save`
    - `assets-prefab-close`
    - `console-get-logs` filter=Error → 0
    - _Requirements: 3.1, 3.2, 3.3, 3.4_
  - [x] 4.✓ · Quick Test
    - `assets-get-data` `CropTile.prefab` → verify 4 SpriteRenderer m_Sprite GUID match sprite mới
    - Không còn GUID cũ `b73d372af6cd515499d753df7754f2ec` (water_needed.png) trong prefab
    - Nếu FAIL → fix task 4, KHÔNG sang task 5

- [x] 5. Wire SCN_Main — BottomNav Shop + Event buttons
  - [x] 5.0 · Resource Check
    - GUID: `UI_Icon_Nav_Shop_Default`, `UI_Icon_Nav_Event_Default`
    - Scene có GO `NavBtn_Shop`, `NavBtn_Event` (đã verified bằng grep SCN_Main.unity)
    - BLOCKING nếu thiếu
  - [x] 5.4 · Scene Agent — Update NavBtn sprites
    - `scene-open` `Assets/_Project/Scenes/SCN_Main.unity`
    - `gameobject-find` name=`NavBtn_Shop` → lấy ID
    - `gameobject-component-modify` component=Image field=sprite value=Shop GUID
    - `gameobject-find` name=`NavBtn_Event` → lấy ID
    - `gameobject-component-modify` component=Image field=sprite value=Event GUID
    - `scene-save`
    - `console-get-logs` filter=Error → 0
    - _Requirements: 4.1, 4.2_
  - [x] 5.✓ · Quick Test
    - `gameobject-component-get` `NavBtn_Shop` Image → sprite GUID = Shop GUID
    - `gameobject-component-get` `NavBtn_Event` Image → sprite GUID = Event GUID
    - Nếu FAIL → fix task 5, KHÔNG sang task 6

- [x] 6. Reference integrity check (pre-cleanup)
  - [x] 6.0 · Resource Check — verify 0 broken reference
    - `console-clear-logs` → `assets-refresh`
    - `console-get-logs` filter=Warning → 0 "missing sprite", "Could not extract GUID", "Broken reference"
    - BLOCKING nếu có warning
  - [x] 6.1 · Verify Legacy sprites unused (grep GUID trong .asset/.prefab/.unity)
    - Với mỗi file trong `Art/Sprites/UI/Legacy/` (4 file):
      - `Bash cat` đọc `.meta` file → extract `guid:` value (regex `guid:\s*([a-f0-9]{32})`)
      - `Bash grep -rn "<GUID>" Assets/_Project/` → expected 0 match (ngoài chính .meta + .png file)
    - Cách này chắc chắn hơn `assets-find` (tool đó search theo name/type, không search reference)
    - Nếu có match trong .asset/.prefab/.unity → KHÔNG delete, ghi HANDOVER để điều tra
    - _Requirements: 5.1_
  - [x] 6.2 · Verify old world sprites chỉ CropTile prefab reference (đã repoint ở task 4)
    - 4 file: `soil_empty.png`, `weed_overlay.png`, `bug_overlay.png`, `water_needed.png`
    - Extract GUID từ mỗi `.meta`, grep reference trong `Assets/_Project/`
    - Expected: 0 match (task 4 đã repoint CropTile prefab)
    - Nếu có match → KHÔNG delete, điều tra
    - _Requirements: 5.3_
  - [x] 6.3 · Verify duplicate unused
    - `icon_Feed_Worm_Atomic_1.png` → grep GUID → 0 match
    - _Requirements: 5.2_

- [x] 7. Cleanup — delete unused files
  - [x] 7.0 · Resource Check
    - Task 6 PASS (0 broken reference, 0 match trên các GUID sẽ delete)
    - BLOCKING nếu task 6 fail
  - [x] 7.4 · Asset Agent — Delete files (theo thứ tự file → folder)
    - **Bước A: xóa 4 file trong Legacy/:**
      - `assets-delete` paths: 4 file `icon_Apple_Atomic.png`, `icon_Carrot_Atomic.png`, `icon_Sprout_Atomic.png`, `icon_Wheat_Atomic.png` (+ .meta tự cleanup)
    - **Bước B: xóa Legacy folder rỗng:** `assets-delete` path=`Assets/_Project/Art/Sprites/UI/Legacy/` (nếu tool không xóa folder empty → dùng `Bash rmdir` qua Unity Editor command)
    - **Bước C: xóa duplicate:**
      - `assets-delete` path=`Art/Sprites/UI/Icons/Animals/Chicken/icon_Feed_Worm_Atomic_1.png`
    - **Bước D: xóa 4 old world sprites (sau khi task 4 đã repoint):**
      - `assets-delete` paths: `World/soil_empty.png`, `weed_overlay.png`, `bug_overlay.png`, `water_needed.png`
    - **Bước E: xóa empty folders:** `Icons/Animals/`, `Icons/Crops/[crop]/` nếu đã rỗng từ m7a
    - `assets-refresh` cuối
    - `console-get-logs` filter=Error → 0
    - _Requirements: 5.1, 5.2, 5.3, 5.4_
  - [x] 7.✓ · Quick Test
    - `Bash ls Art/Sprites/UI/Legacy/` → "No such file" (folder xóa)
    - `Bash ls Art/Sprites/World/*.png` → chỉ naming `World_*.png` (không lowercase)
    - `Bash find Art/Sprites -name "icon_Feed_Worm_Atomic_1*"` → 0 match
    - Console: 0 broken reference
    - Nếu FAIL → fix task 7, KHÔNG sang task 8

- [x] 8. Integration smoke test — Play mode (API-accurate)
  - ⚠️ **Prerequisite:** Tất cả task 1-7 PASS
  - [x] 8.1 Boot check + CropTile visual check
    - `editor-application-set-state` → Play mode
    - `console-get-logs` filter=Error → 0 khi load
    - `screenshot-game-view` → không có pink sprite (magenta) nào trong view
    - _Requirements: 6.6_
  - [x] 8.2 Crop lifecycle — tăng tốc tick rate để test nhanh
    - `reflection-method-call` → set `TimeManager.Instance._tickRate = 0.01f` (field `_tickRate` là private SerializeField; dùng reflection `SetField`)
    - `reflection-method-call` → `EconomySystem.Instance.AddGold(200)` để plant
    - Tap tile (hoặc gọi `CropActionPanelController.OnPlantButton`) → plant crop_01 (Carrot)
    - Chờ ~10-30s (tick 0.01s thì 30s = 3000 tick → đủ grow qua 4 stage nếu `growTimeMin=2` = 120s thực × speedup 100× = 1.2s lý thuyết)
    - `screenshot-game-view` → verify sprite trên tile match Stage03 (Ripe) GUID
    - Hoặc: `reflection-method-call` lấy `CropTileView.CurrentStage` property → verify = 3 (Ripe)
    - `reflection-method-call` restore `_tickRate = 1.0f` cuối task
    - _Requirements: 6.1_
  - [x] 8.3 Ailment overlay — force via reflection
    - Chọn 1 tile đang Growing (từ task 8.2)
    - `reflection-method-call` → `SetField(tile, "_hasWeeds", true)`
    - `reflection-method-call` → gọi `tile.RefreshVisuals()` (private method; dùng reflection `InvokeMethod`)
    - `screenshot-game-view` → verify `WeedVisual` GameObject active + render sprite `World_Overlay_Tile_Weed_On.png`
    - Reset: `SetField(tile, "_hasWeeds", false)` + `RefreshVisuals()`
    - Lặp cho `_hasPests` (BugVisual) và `_needsWater` (WaterVisual)
    - _Requirements: 6.2_
  - [x] 8.4 Animal rendering test
    - Nếu có AnimalPen spawn sẵn trong scene → `gameobject-find` AnimalView instance
    - Nếu không: `reflection-method-call` spawn `AnimalView` từ `animal_01` SO (pattern trong m4-animal-care)
    - `screenshot-game-view` → verify Chicken Stage00 sprite (baby) render đúng
    - Force mature: `reflection-method-call` set `AnimalView._currentStage = 2` + refresh
    - Verify Stage02 sprite render
    - Collect produce: verify egg icon hiện (readyToCollectIcon)
    - Lặp cho animal_02 (Duck)
    - _Requirements: 6.3, 6.4_
  - [x] 8.5 BottomNav sprite verify
    - `screenshot-game-view` → focus BottomNav area (zoom nếu cần)
    - Verify: icon Shop không phải placeholder (Sprout), icon Event không phải placeholder (Star)
    - `gameobject-component-get` `NavBtn_Shop` Image.sprite GUID == Shop GUID (runtime check)
    - _Requirements: 6.5_
  - [x] 8.✓ · Integration Test Report
    - `editor-application-set-state` → Stop
    - `screenshot-game-view` → lưu `docs/screenshots/2026-04-21-m7-sprite-wireup-integration.png`
    - Output:
      ```
      📋 INTEGRATION TEST REPORT: m7b-sprite-wireup
      ✅ PASSED: [N] checks
      ⚠️ WARNINGS: [danh sách]
      ❌ FAILED: [N] — [mô tả]
      Crop lifecycle rendering (crop_01 Carrot): ✅
      Ailment overlays (Weed/Bug/Water): ✅
      Animal rendering (Chicken + Duck): ✅
      BottomNav icons: ✅
      Console: 0 "missing sprite", 0 pink sprite ✅
      ```
    - Nếu FAIL critical → fix, KHÔNG sang task 9

- [x] 9. Cập nhật HANDOVER.md + commit
  - Mở `docs/HANDOVER.md`
  - Thêm section phiên 21/04/2026 — m7b-sprite-wireup DONE:
    - Wire sprite vào 7 `CropDataSO` (growthStageSprites + deadSprite + data.seedIcon + seedIcon + cropIcon), skip top-level `icon` inherited từ ItemData
    - Wire sprite vào 2 `AnimalDataSO` (stageSprites + deadSprite + readyToCollectIcon)
    - Update `CropTile.prefab` (path: `Prefabs/World/CropTile.prefab`): 4 SpriteRenderer (SoilRenderer + WaterVisual + WeedVisual + BugVisual)
    - Update `SCN_Main` BottomNav: `NavBtn_Shop` + `NavBtn_Event` Image.sprite
    - Cleanup: xóa `Art/Sprites/UI/Legacy/` (4 file), duplicate `icon_Feed_Worm_Atomic_1`, 4 old lowercase world sprite
    - Play mode smoke test PASS: 7 crop lifecycle, ailment overlays, 2 animal rendering, BottomNav icons
  - Cập nhật Kiro Specs: thêm Spec 9 `m7a-sprite-reorg` + Spec 10 `m7b-sprite-wireup` = DONE
  - Commit:
    - `git add Assets/_Project/Art/Sprites/ Assets/_Project/Data/ Assets/_Project/Prefabs/World/CropTile.prefab Assets/_Project/Scenes/SCN_Main.unity docs/HANDOVER.md docs/screenshots/`
    - Commit message: `feat(assets): m7b — wire all sprites to SOs/Prefab/Scene + cleanup`
  - _Requirements: tất cả_
