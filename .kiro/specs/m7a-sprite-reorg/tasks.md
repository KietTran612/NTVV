# Implementation Plan: m7a-sprite-reorg

## Overview

Mass rename + reorganize ~75 sprite file về 1 naming convention duy nhất. Dùng `assets-move` để preserve GUID, references không vỡ.

**Design doc:** `.kiro/specs/m7a-sprite-reorg/design.md`
**Requirements:** `.kiro/specs/m7a-sprite-reorg/requirements.md`

> ⚠️ ĐỌC GLOBAL RULES trong requirements.md TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO.

---

## Tasks

- [x] 0. Prerequisite — commit working tree clean
  - [x] 0.1 · Git Check
    - `Bash git status --short` → kiểm tra file uncommitted
    - Nếu có file chưa commit: hỏi user commit/stash trước khi execute m7a (git rename tracking chính xác khi working tree clean)
    - BLOCKING nếu có file uncommitted không liên quan m7
    - _Reason: m7a rename ~75 file; nếu fail giữa chừng cần `git checkout` để rollback — dirty tree sẽ mất thay đổi khác_

- [x] 1. Tạo folder structure mới (empty) — **theo thứ tự dependency**
  - [x] 1.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [x] 1.4 · Asset Agent — Tạo folders (tool `assets-create-folder` KHÔNG đệ quy; parent PHẢI tồn tại trước)
    - **Bước A (parent top-level):** Verify `Assets/_Project/Art/Sprites/World/` + `UI/` đã tồn tại (có sẵn)
    - **Bước B (World subs):** `assets-create-folder` parent=`Art/Sprites/World/` — tạo từng folder: `Crops`, `Animals`, `Products`, `Overlays`, `Tiles` (5 call, không nested)
    - **Bước C (Crop sub-subs):** `assets-create-folder` parent=`Art/Sprites/World/Crops/` — tạo: `Carrot`, `Corn`, `Potato`, `Pumpkin`, `Strawberry`, `Tomato`, `Wheat` (7 call)
    - **Bước D (Animal sub-subs):** `assets-create-folder` parent=`Art/Sprites/World/Animals/` — tạo: `Chicken`, `Duck` (2 call)
    - **Bước E (UI/Icons subs):** `assets-create-folder` parent=`Art/Sprites/UI/Icons/` — tạo: `Seed`, `Tab`, `Header`, `Action` (4 call; Common + Nav đã có)
    - **Bước F (UI/Buttons):** `assets-create-folder` parent=`Art/Sprites/UI/` — tạo: `Buttons` (1 call)
    - Tổng: 5 + 7 + 2 + 4 + 1 = **19 folder creation call**
    - _Requirements: 1.1, 1.2, 1.3, 1.4_
  - [x] 1.✓ · Quick Test
    - Verify folder structure bằng `Bash ls "Assets/_Project/Art/Sprites/World/"` và `UI/Icons/`
    - Verify: 2 folder top-level World/ + UI/; World/ có 5 sub; UI/Icons/ có 6 sub
    - Nếu FAIL → fix task 1, KHÔNG sang task 2

- [x] 2. Rename + move Carrot sprites (6 file)
  - [x] 2.0 · Resource Check
    - Verify 6 file nguồn tồn tại trong `Art/Sprites/UI/Icons/Crops/Carrot/`
    - BLOCKING nếu thiếu file
  - [x] 2.4 · Asset Agent — Carrot mass rename
    - `assets-move` theo map (xem design.md Carrot section):
      - `icon_Carrot_Stage0_Atomic.png` → `World/Crops/Carrot/World_Crop_Carrot_Body_Stage00.png`
      - `icon_Carrot_Stage1_Atomic.png` → `Stage01.png`
      - `icon_Carrot_Stage2_Atomic.png` → `Stage02.png`
      - `icon_Carrot_Stage3_Atomic.png` → `Stage03.png`
      - `icon_Carrot_Dead_Atomic.png` → `Dead.png`
      - `icon_Seeds_Carrot_Atomic.png` → `UI/Icons/Seed/UI_Icon_Seed_Carrot_Default.png`
    - `assets-refresh` → chờ import
    - `console-get-logs` filter=Warning → 0 "missing sprite" / "Could not extract GUID"
    - _Requirements: 3.1, 3.2, 8.2_
  - [x] 2.✓ · Quick Test
    - 6 file mới tồn tại ở path mới, 6 file cũ KHÔNG tồn tại ở path cũ
    - `assets-find` filter=`World_Crop_Carrot_Body_Stage00` → 1 match
    - `crop_01.asset` Inspector vẫn hiện Carrot Stage0-3 sprite không "Missing" (GUID preserved)
    - Nếu FAIL → fix task 2, KHÔNG sang task 3

- [x] 3. Move 6 crop folders khác (Potato, Corn, Wheat, Tomato, Strawberry, Pumpkin — 36 file)
  - [x] 3.0 · Resource Check
    - Mỗi crop 6 file (5 body + 1 seed) trong `Art/Sprites/UI/Icons/Crops/[X]/`
    - BLOCKING nếu thiếu
  - [x] 3.4 · Asset Agent — Batch move 6 crops (thứ tự match crop_02..07 ID để align với m7b)
    - Với mỗi crop X theo thứ tự: **Potato (crop_02), Corn (crop_03), Wheat (crop_04), Tomato (crop_05), Strawberry (crop_06), Pumpkin (crop_07)**:
      - `assets-move` 5 body file: `Icons/Crops/[X]/World_Crop_[X]_Body_*.png` → `World/Crops/[X]/World_Crop_[X]_Body_*.png` (giữ tên)
      - `assets-move` 1 seed: `Icons/Crops/[X]/icon_Seeds_[X]_Atomic.png` → `UI/Icons/Seed/UI_Icon_Seed_[X]_Default.png`
    - `assets-refresh` sau mỗi crop
    - `console-get-logs` filter=Warning → 0 warning mỗi crop
    - _Requirements: 3.3, 3.4, 8.2_
  - [x] 3.✓ · Quick Test
    - `World/Crops/` có 7 subfolder (Carrot + 6 này), mỗi folder có 5 file
    - `UI/Icons/Seed/` có 7 file seed
    - `Art/Sprites/UI/Icons/Crops/` KHÔNG tồn tại (đã empty và cleanup tự động) HOẶC empty folder
    - Nếu FAIL → fix task 3, KHÔNG sang task 4

- [x] 4. Rename + move Chicken sprites (6 file)
  - [x] 4.0 · Resource Check
    - Verify 6 file trong `Art/Sprites/UI/Icons/Animals/Chicken/`: Stage1-3, Dead, Egg, Feed_Worm
    - BLOCKING nếu thiếu
  - [x] 4.4 · Asset Agent — Chicken rename (re-index stage)
    - `assets-move` map:
      - `icon_Chicken_Stage1_Atomic.png` → `World/Animals/Chicken/World_Animal_Chicken_Body_Stage00.png`
      - `icon_Chicken_Stage2_Atomic.png` → `Stage01.png`
      - `icon_Chicken_Stage3_Atomic.png` → `Stage02.png`
      - `icon_Chicken_Dead_Atomic.png` → `Dead.png`
      - `icon_Egg_Atomic.png` → `World/Products/World_Product_Egg_Collect_Ready.png`
      - `icon_Feed_Worm_Atomic.png` → `UI/Icons/Common/UI_Icon_Common_Worm_Default.png`
    - KHÔNG move `icon_Feed_Worm_Atomic_1.png` (duplicate — m7b delete)
    - `assets-refresh`
    - `console-get-logs` filter=Warning → 0
    - _Requirements: 4.1, 4.2, 4.3, 5.1_
  - [x] 4.✓ · Quick Test
    - `World/Animals/Chicken/` có 4 file body
    - `World/Products/` có `Egg_Collect_Ready.png`
    - `UI/Icons/Common/` có `Worm_Default.png`
    - `animal_01.asset` Inspector: stage sprite không "Missing"
    - Nếu FAIL → fix task 4, KHÔNG sang task 5

- [x] 5. Move Duck sprites (5 file — giữ tên)
  - [x] 5.0 · Resource Check
    - Verify 5 file trong `Art/Sprites/UI/Icons/Animals/Duck/`: 3 Body stage, 1 Dead, 1 DuckEgg
    - BLOCKING nếu thiếu
  - [x] 5.4 · Asset Agent — Duck move path
    - `assets-move` map (giữ tên, chỉ đổi folder):
      - 4 file body/dead: `Icons/Animals/Duck/*.png` → `World/Animals/Duck/*.png`
      - 1 file product: `Icons/Animals/Duck/World_Product_DuckEgg_Collect_Ready.png` → `World/Products/World_Product_DuckEgg_Collect_Ready.png`
    - `assets-refresh`
    - `console-get-logs` filter=Warning → 0
    - _Requirements: 4.4, 5.2_
  - [x] 5.✓ · Quick Test
    - `World/Animals/Duck/` có 4 file
    - `World/Products/` có 2 file (Egg + DuckEgg)
    - `Icons/Animals/` empty → có thể delete sau
    - Nếu FAIL → fix task 5, KHÔNG sang task 6

- [x] 6. Move World overlays + tiles (4 file)
  - [x] 6.0 · Resource Check
    - Verify 4 file trong `Art/Sprites/World/` với naming `World_*`
    - BLOCKING nếu thiếu
  - [x] 6.4 · Asset Agent — World move path
    - `assets-move`:
      - `World/World_Overlay_Tile_Pest_On.png` → `World/Overlays/World_Overlay_Tile_Pest_On.png`
      - `World/World_Overlay_Tile_Weed_On.png` → `World/Overlays/World_Overlay_Tile_Weed_On.png`
      - `World/World_Overlay_Tile_WaterNeed_On.png` → `World/Overlays/World_Overlay_Tile_WaterNeed_On.png`
      - `World/World_Tile_Soil_Base_Default.png` → `World/Tiles/World_Tile_Soil_Base_Default.png`
    - KHÔNG đụng: `soil_empty.png`, `weed_overlay.png`, `bug_overlay.png`, `water_needed.png` (m7b xóa)
    - `assets-refresh`
    - `console-get-logs` filter=Warning → 0
    - _Requirements: 6.1, 6.2, 6.3_
  - [x] 6.✓ · Quick Test
    - `World/Overlays/` có 3 file
    - `World/Tiles/` có 1 file
    - `Art/Sprites/World/` còn 4 file lowercase (OK — m7b xử lý)
    - Nếu FAIL → fix task 6, KHÔNG sang task 7

- [x] 7. Rename UI Backgrounds (8 file)
  - [x] 7.0 · Resource Check
    - Verify 7 file trong `UI/Backgrounds/` + 1 file `UI/bg_Plaque_Wooden_Atomic.png` (misplaced)
    - BLOCKING nếu thiếu
  - [x] 7.4 · Asset Agent — UI/Backgrounds rename
    - `assets-move` 7 file trong Backgrounds: `bg_*_Atomic.png` → `UI_Background_*_Default.png` (giữ folder):
      - `bg_Panel_Main_Atomic.png` → `UI_Background_Panel_Main_Default.png`
      - `bg_Button_Blue_Atomic.png` → `UI_Background_Button_Blue_Default.png`
      - `bg_Button_Green_Atomic.png`, `Purple`, `Red` → (tương tự)
      - `bg_Banner_Parchment_Atomic.png` → `UI_Background_Banner_Parchment_Default.png`
      - `bg_Chip_Resource_Atomic.png` → `UI_Background_Chip_Resource_Default.png`
    - `assets-move` 1 file misplaced: `UI/bg_Plaque_Wooden_Atomic.png` → `UI/Backgrounds/UI_Background_Plaque_Wooden_Default.png`
    - `assets-refresh`
    - `console-get-logs` filter=Warning → 0
    - _Requirements: 7.1_
  - [x] 7.✓ · Quick Test
    - `UI/Backgrounds/` có 8 file, tất cả naming `UI_Background_*_Default.png`
    - `UI/` root KHÔNG còn file `bg_*`
    - Nếu FAIL → fix task 7, KHÔNG sang task 8

- [x] 8a. Rename UI Icons nhóm 1 — Common + Nav + Action (8 file op)
  - [x] 8a.0 · Resource Check
    - Verify files: `UI/Icons/Common/` (5 file `icon_*_Atomic` + 1 `icon_WateringCan`), `UI/Icons/Nav/` (2 file `icon_*_Atomic`), `UI/icon_Refresh_Atomic.png`
    - BLOCKING nếu thiếu
  - [x] 8a.4 · Asset Agent — Common + Nav + Action
    - **Common rename (5 file, giữ folder):**
      - `icon_Gold_Atomic.png` → `UI_Icon_Common_Gold_Default.png`
      - `icon_XP_Atomic.png` → `UI_Icon_Common_XP_Default.png`
      - `icon_Gem_Atomic.png` → `UI_Icon_Common_Gem_Default.png`
      - `icon_Storage_Atomic.png` → `UI_Icon_Common_Storage_Default.png`
      - `icon_Energy_Atomic.png` → `UI_Icon_Common_Energy_Default.png`
    - **Common → Action move (1 file, đổi folder):**
      - `UI/Icons/Common/icon_WateringCan_Atomic.png` → `UI/Icons/Action/UI_Icon_Action_Water_Default.png`
    - **Nav rename (2 file, giữ folder):**
      - `UI/Icons/Nav/icon_Farm_Atomic.png` → `UI_Icon_Nav_Farm_Default.png`
      - `UI/Icons/Nav/icon_Barn_Atomic.png` → `UI_Icon_Nav_Barn_Default.png`
    - **UI root → Action move (1 file):**
      - `UI/icon_Refresh_Atomic.png` → `UI/Icons/Action/UI_Icon_Action_Refresh_Default.png`
    - Tổng: 5 + 1 + 2 + 1 = 9 file ops (không 8 — đã sửa)
    - `assets-refresh` sau nhóm
    - `console-get-logs` filter=Warning → 0
    - _Requirements: 8.1, 8.3, 8.6_
  - [x] 8a.✓ · Quick Test
    - `UI/Icons/Common/` có 6 file (5 rename + Grass cũ từ trước + 1 Worm từ task 4 = 7 — cũng OK)
    - `UI/Icons/Action/` có 2 file (Water + Refresh)
    - `UI/Icons/Nav/` có 4 file (Farm, Barn, Shop, Event)
    - Nếu FAIL → fix 8a, KHÔNG sang 8b

- [x] 8b. Rename UI Icons nhóm 2 — Tab + Header (3 file op)
  - [x] 8b.0 · Resource Check
    - Verify 3 file tại `UI/` root: `icon_Tab_Leaf_Atomic.png`, `icon_Tab_Star_Atomic.png`, `icon_Sprout_Header_Atomic.png`
    - BLOCKING nếu thiếu
  - [x] 8b.4 · Asset Agent — Tab + Header (từ UI/ root, move + rename)
    - `UI/icon_Tab_Leaf_Atomic.png` → `UI/Icons/Tab/UI_Icon_Tab_Leaf_Default.png`
    - `UI/icon_Tab_Star_Atomic.png` → `UI/Icons/Tab/UI_Icon_Tab_Star_Default.png`
    - `UI/icon_Sprout_Header_Atomic.png` → `UI/Icons/Header/UI_Icon_Header_Sprout_Default.png`
    - `assets-refresh`
    - `console-get-logs` filter=Warning → 0
    - _Requirements: 8.4, 8.5_
  - [x] 8b.✓ · Quick Test
    - `UI/Icons/Tab/` có 2 file, `Header/` có 1 file
    - `UI/` root KHÔNG còn file `icon_Tab_*` hoặc `icon_Sprout_Header_*`
    - Nếu FAIL → fix 8b, KHÔNG sang task 9

- [x] 9. Rename UI Buttons (1 file)
  - [x] 9.0 · Resource Check
    - Verify `UI/btn_Close_Circle_Atomic.png` tồn tại (misplaced root)
    - BLOCKING nếu thiếu
  - [x] 9.4 · Asset Agent — Close button rename + move
    - `assets-move`: `UI/btn_Close_Circle_Atomic.png` → `UI/Buttons/UI_Button_Close_Circle_Default.png`
    - `assets-refresh`
    - `console-get-logs` filter=Warning → 0
    - _Requirements: 9.1_
  - [x] 9.✓ · Quick Test
    - `UI/Buttons/UI_Button_Close_Circle_Default.png` tồn tại
    - `UI/btn_Close_Circle_Atomic.png` KHÔNG tồn tại
    - Nếu FAIL → fix task 9, KHÔNG sang task 10

- [x] 10. Integration verification
  - [x] 10.1 Unity Editor check
    - `assets-refresh` lần cuối
    - `console-clear-logs` → `console-get-logs` filter=Warning → 0 "missing sprite", 0 "Could not extract GUID"
    - `assets-find` filter=`t:Sprite icon_.*_Atomic` → 0 match (trừ Legacy folder vẫn còn)
    - `assets-find` filter=`t:Sprite bg_.*_Atomic` → 0 match
    - _Requirements: 10.1, 10.2_
  - [x] 10.2 Reference integrity check
    - `assets-get-data` path=`Assets/_Project/Data/Crops/crop_01.asset` → verify `growthStageSprites` GUIDs không null
    - `assets-get-data` path=`Assets/_Project/Prefabs/World/CropTile.prefab` → verify SpriteRenderer references không "Missing"
    - _Requirements: 10.2_
  - [x] 10.3 Git status check
    - `Bash git status --short | grep "^R"` → ~75 rename entries
    - `Bash git status --short | grep "^D"` → 0 delete không mong muốn
    - Nếu có delete bất thường → FAIL, điều tra
    - _Requirements: 10.3_
  - [x] 10.✓ · Integration Test Report
    - Output:
      ```
      📋 INTEGRATION TEST REPORT: m7a-sprite-reorg
      ✅ PASSED: [N] checks
      ⚠️ WARNINGS: [danh sách]
      ❌ FAILED: [N] — [mô tả]
      Sprites renamed: ~75
      Folders created: 13
      Console warnings: 0
      GUID preserved: ✅
      ```
    - Nếu FAIL critical → fix, KHÔNG commit

- [x] 11. Cập nhật HANDOVER.md + commit
  - Mở `docs/HANDOVER.md`
  - Thêm section phiên 21/04/2026 — m7a-sprite-reorg DONE:
    - ~75 sprite rename sang naming mới `[Domain]_[Category]_[Entity]_[Variant]_[State]`
    - Folder structure gọn gàng: World/ (Crops, Animals, Products, Overlays, Tiles) + UI/ (Backgrounds, Icons, Buttons)
    - GUID preserved → references trong SO/Prefab/Scene không vỡ
    - Prerequisite cho m7b-sprite-wireup
  - Commit:
    - `git add Assets/_Project/Art/Sprites/ docs/HANDOVER.md .kiro/specs/m7a-sprite-reorg/`
    - Commit message: `refactor(assets): m7a — mass rename sprites + folder reorg (~75 files)`
  - _Requirements: tất cả_
