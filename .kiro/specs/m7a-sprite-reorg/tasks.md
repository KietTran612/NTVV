# Implementation Plan: m7a-sprite-reorg

## Overview

Mass rename + reorganize ~75 sprite file về 1 naming convention duy nhất. Dùng `assets-move` để preserve GUID, references không vỡ.

**Design doc:** `.kiro/specs/m7a-sprite-reorg/design.md`
**Requirements:** `.kiro/specs/m7a-sprite-reorg/requirements.md`

> ⚠️ ĐỌC GLOBAL RULES trong requirements.md TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO.

---

## Tasks

- [ ] 1. Tạo folder structure mới (empty)
  - [ ] 1.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [ ] 1.4 · Asset Agent — Tạo folders
    - `assets-create-folder` parent=`Assets/_Project/Art/Sprites/World/`, tạo: `Crops/`, `Animals/`, `Products/`, `Overlays/`, `Tiles/`
    - Trong `World/Crops/`: tạo `Carrot/`, `Corn/`, `Potato/`, `Pumpkin/`, `Strawberry/`, `Tomato/`, `Wheat/`
    - Trong `World/Animals/`: tạo `Chicken/`, `Duck/`
    - `assets-create-folder` parent=`Assets/_Project/Art/Sprites/UI/Icons/`, tạo: `Seed/`, `Tab/`, `Header/`, `Action/`
    - `assets-create-folder` parent=`Assets/_Project/Art/Sprites/UI/`, tạo: `Buttons/` (nếu chưa có)
    - _Requirements: 1.1, 1.2, 1.3, 1.4_
  - [ ] 1.✓ · Quick Test
    - Verify folder structure bằng `Bash ls "Assets/_Project/Art/Sprites/World/"` và `UI/Icons/`
    - Verify: 2 folder top-level World/ + UI/; World/ có 5 sub; UI/Icons/ có 6 sub
    - Nếu FAIL → fix task 1, KHÔNG sang task 2

- [ ] 2. Rename + move Carrot sprites (6 file)
  - [ ] 2.0 · Resource Check
    - Verify 6 file nguồn tồn tại trong `Art/Sprites/UI/Icons/Crops/Carrot/`
    - BLOCKING nếu thiếu file
  - [ ] 2.4 · Asset Agent — Carrot mass rename
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
  - [ ] 2.✓ · Quick Test
    - 6 file mới tồn tại ở path mới, 6 file cũ KHÔNG tồn tại ở path cũ
    - `assets-find` filter=`World_Crop_Carrot_Body_Stage00` → 1 match
    - `crop_01.asset` Inspector vẫn hiện Carrot Stage0-3 sprite không "Missing" (GUID preserved)
    - Nếu FAIL → fix task 2, KHÔNG sang task 3

- [ ] 3. Move 6 crop folders khác (Corn, Potato, Wheat, Tomato, Strawberry, Pumpkin — 36 file)
  - [ ] 3.0 · Resource Check
    - Mỗi crop 6 file (5 body + 1 seed) trong `Art/Sprites/UI/Icons/Crops/[X]/`
    - BLOCKING nếu thiếu
  - [ ] 3.4 · Asset Agent — Batch move 6 crops
    - Với mỗi crop X ∈ {Corn, Potato, Wheat, Tomato, Strawberry, Pumpkin}:
      - `assets-move` 5 body file: `Icons/Crops/[X]/World_Crop_[X]_Body_*.png` → `World/Crops/[X]/World_Crop_[X]_Body_*.png` (giữ tên)
      - `assets-move` 1 seed: `Icons/Crops/[X]/icon_Seeds_[X]_Atomic.png` → `UI/Icons/Seed/UI_Icon_Seed_[X]_Default.png`
    - `assets-refresh` sau mỗi crop
    - `console-get-logs` filter=Warning → 0 warning mỗi crop
    - _Requirements: 3.3, 3.4, 8.2_
  - [ ] 3.✓ · Quick Test
    - `World/Crops/` có 7 subfolder (Carrot + 6 này), mỗi folder có 5 file
    - `UI/Icons/Seed/` có 7 file seed
    - `Art/Sprites/UI/Icons/Crops/` KHÔNG tồn tại (đã empty và cleanup tự động) HOẶC empty folder
    - Nếu FAIL → fix task 3, KHÔNG sang task 4

- [ ] 4. Rename + move Chicken sprites (6 file)
  - [ ] 4.0 · Resource Check
    - Verify 6 file trong `Art/Sprites/UI/Icons/Animals/Chicken/`: Stage1-3, Dead, Egg, Feed_Worm
    - BLOCKING nếu thiếu
  - [ ] 4.4 · Asset Agent — Chicken rename (re-index stage)
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
  - [ ] 4.✓ · Quick Test
    - `World/Animals/Chicken/` có 4 file body
    - `World/Products/` có `Egg_Collect_Ready.png`
    - `UI/Icons/Common/` có `Worm_Default.png`
    - `animal_01.asset` Inspector: stage sprite không "Missing"
    - Nếu FAIL → fix task 4, KHÔNG sang task 5

- [ ] 5. Move Duck sprites (5 file — giữ tên)
  - [ ] 5.0 · Resource Check
    - Verify 5 file trong `Art/Sprites/UI/Icons/Animals/Duck/`: 3 Body stage, 1 Dead, 1 DuckEgg
    - BLOCKING nếu thiếu
  - [ ] 5.4 · Asset Agent — Duck move path
    - `assets-move` map (giữ tên, chỉ đổi folder):
      - 4 file body/dead: `Icons/Animals/Duck/*.png` → `World/Animals/Duck/*.png`
      - 1 file product: `Icons/Animals/Duck/World_Product_DuckEgg_Collect_Ready.png` → `World/Products/World_Product_DuckEgg_Collect_Ready.png`
    - `assets-refresh`
    - `console-get-logs` filter=Warning → 0
    - _Requirements: 4.4, 5.2_
  - [ ] 5.✓ · Quick Test
    - `World/Animals/Duck/` có 4 file
    - `World/Products/` có 2 file (Egg + DuckEgg)
    - `Icons/Animals/` empty → có thể delete sau
    - Nếu FAIL → fix task 5, KHÔNG sang task 6

- [ ] 6. Move World overlays + tiles (4 file)
  - [ ] 6.0 · Resource Check
    - Verify 4 file trong `Art/Sprites/World/` với naming `World_*`
    - BLOCKING nếu thiếu
  - [ ] 6.4 · Asset Agent — World move path
    - `assets-move`:
      - `World/World_Overlay_Tile_Pest_On.png` → `World/Overlays/World_Overlay_Tile_Pest_On.png`
      - `World/World_Overlay_Tile_Weed_On.png` → `World/Overlays/World_Overlay_Tile_Weed_On.png`
      - `World/World_Overlay_Tile_WaterNeed_On.png` → `World/Overlays/World_Overlay_Tile_WaterNeed_On.png`
      - `World/World_Tile_Soil_Base_Default.png` → `World/Tiles/World_Tile_Soil_Base_Default.png`
    - KHÔNG đụng: `soil_empty.png`, `weed_overlay.png`, `bug_overlay.png`, `water_needed.png` (m7b xóa)
    - `assets-refresh`
    - `console-get-logs` filter=Warning → 0
    - _Requirements: 6.1, 6.2, 6.3_
  - [ ] 6.✓ · Quick Test
    - `World/Overlays/` có 3 file
    - `World/Tiles/` có 1 file
    - `Art/Sprites/World/` còn 4 file lowercase (OK — m7b xử lý)
    - Nếu FAIL → fix task 6, KHÔNG sang task 7

- [ ] 7. Rename UI Backgrounds (8 file)
  - [ ] 7.0 · Resource Check
    - Verify 7 file trong `UI/Backgrounds/` + 1 file `UI/bg_Plaque_Wooden_Atomic.png` (misplaced)
    - BLOCKING nếu thiếu
  - [ ] 7.4 · Asset Agent — UI/Backgrounds rename
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
  - [ ] 7.✓ · Quick Test
    - `UI/Backgrounds/` có 8 file, tất cả naming `UI_Background_*_Default.png`
    - `UI/` root KHÔNG còn file `bg_*`
    - Nếu FAIL → fix task 7, KHÔNG sang task 8

- [ ] 8. Rename UI Icons (Common, Nav, Tab, Header, Action — ~12 file)
  - [ ] 8.0 · Resource Check
    - Verify files ở các location: Common/ (5 file icon_*), Nav/ (2 file icon_*), UI/ root (4 file misplaced: icon_Tab_Leaf, icon_Tab_Star, icon_Sprout_Header, icon_Refresh)
    - BLOCKING nếu thiếu
  - [ ] 8.4 · Asset Agent — UI Icons batch rename
    - **Common (5 rename):**
      - `icon_Gold_Atomic.png` → `UI_Icon_Common_Gold_Default.png`
      - `icon_XP_Atomic.png`, `icon_Gem_Atomic.png`, `icon_Storage_Atomic.png`, `icon_Energy_Atomic.png` → tương tự
    - **Common → Action move (1):**
      - `UI/Icons/Common/icon_WateringCan_Atomic.png` → `UI/Icons/Action/UI_Icon_Action_Water_Default.png`
    - **Nav (2):**
      - `UI/Icons/Nav/icon_Farm_Atomic.png` → `UI/Icons/Nav/UI_Icon_Nav_Farm_Default.png`
      - `icon_Barn_Atomic.png` → `UI_Icon_Nav_Barn_Default.png`
    - **Tab (2, từ UI/ root):**
      - `UI/icon_Tab_Leaf_Atomic.png` → `UI/Icons/Tab/UI_Icon_Tab_Leaf_Default.png`
      - `UI/icon_Tab_Star_Atomic.png` → `UI/Icons/Tab/UI_Icon_Tab_Star_Default.png`
    - **Header (1, từ UI/ root):**
      - `UI/icon_Sprout_Header_Atomic.png` → `UI/Icons/Header/UI_Icon_Header_Sprout_Default.png`
    - **Action (1, từ UI/ root):**
      - `UI/icon_Refresh_Atomic.png` → `UI/Icons/Action/UI_Icon_Action_Refresh_Default.png`
    - `assets-refresh` sau mỗi nhóm
    - `console-get-logs` filter=Warning → 0
    - _Requirements: 8.1, 8.3, 8.4, 8.5, 8.6_
  - [ ] 8.✓ · Quick Test
    - `UI/Icons/Common/` có ≥ 6 file (5 rename + Grass cũ + Worm mới từ task 4)
    - `UI/Icons/Action/` có 2 file (Water + Refresh)
    - `UI/Icons/Nav/` có 4 file (Farm, Barn, Shop, Event)
    - `UI/Icons/Tab/` có 2, `Header/` có 1
    - Nếu FAIL → fix task 8, KHÔNG sang task 9

- [ ] 9. Rename UI Buttons (1 file)
  - [ ] 9.0 · Resource Check
    - Verify `UI/btn_Close_Circle_Atomic.png` tồn tại (misplaced root)
    - BLOCKING nếu thiếu
  - [ ] 9.4 · Asset Agent — Close button rename + move
    - `assets-move`: `UI/btn_Close_Circle_Atomic.png` → `UI/Buttons/UI_Button_Close_Circle_Default.png`
    - `assets-refresh`
    - `console-get-logs` filter=Warning → 0
    - _Requirements: 9.1_
  - [ ] 9.✓ · Quick Test
    - `UI/Buttons/UI_Button_Close_Circle_Default.png` tồn tại
    - `UI/btn_Close_Circle_Atomic.png` KHÔNG tồn tại
    - Nếu FAIL → fix task 9, KHÔNG sang task 10

- [ ] 10. Integration verification
  - [ ] 10.1 Unity Editor check
    - `assets-refresh` lần cuối
    - `console-clear-logs` → `console-get-logs` filter=Warning → 0 "missing sprite", 0 "Could not extract GUID"
    - `assets-find` filter=`t:Sprite icon_.*_Atomic` → 0 match (trừ Legacy folder vẫn còn)
    - `assets-find` filter=`t:Sprite bg_.*_Atomic` → 0 match
    - _Requirements: 10.1, 10.2_
  - [ ] 10.2 Reference integrity check
    - `assets-get-data` path=`Assets/_Project/Data/Crops/crop_01.asset` → verify `growthStageSprites` GUIDs không null
    - `assets-get-data` path=`Assets/_Project/Prefabs/CropTile.prefab` → verify SpriteRenderer references không "Missing"
    - _Requirements: 10.2_
  - [ ] 10.3 Git status check
    - `Bash git status --short | grep "^R"` → ~75 rename entries
    - `Bash git status --short | grep "^D"` → 0 delete không mong muốn
    - Nếu có delete bất thường → FAIL, điều tra
    - _Requirements: 10.3_
  - [ ] 10.✓ · Integration Test Report
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

- [ ] 11. Cập nhật HANDOVER.md + commit
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
