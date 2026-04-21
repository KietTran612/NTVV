# Design Document: m7a-sprite-reorg

**Date:** 2026-04-21
**Spec:** m7a-sprite-reorg
**Status:** approved
**Full design:** `docs/superpowers/specs/2026-04-21-m7-sprite-reorg-and-wireup-design.md`

---

## Overview

Rename tất cả sprite về 1 naming convention `[Domain]_[Category]_[Entity]_[Variant]_[State].png` và tổ chức folder theo `World/`, `UI/`. Mass rename qua `assets-move` (Unity API) để bảo toàn GUID → references trong SO/Prefab/Scene không vỡ.

**Phạm vi:**
- Rename ~35 file (Carrot 6 + Chicken 4 + Egg 1 + UI/Backgrounds 8 + UI/Icons ~14 + UI/Buttons 1 + seeds 7)
- Move folder path ~40 file (6 crop folder + Duck + Products + World overlays/tiles)
- Tổng: ~75 file di chuyển

**KHÔNG làm ở spec này:**
- Không sửa SO data (m7b)
- Không sửa prefab/scene (m7b)
- Không xóa old files (m7b)

---

## Naming Map (rename list)

### Carrot (6 file)

| Old | New |
|-----|-----|
| `Icons/Crops/Carrot/icon_Carrot_Stage0_Atomic.png` | `World/Crops/Carrot/World_Crop_Carrot_Body_Stage00.png` |
| `Icons/Crops/Carrot/icon_Carrot_Stage1_Atomic.png` | `World/Crops/Carrot/World_Crop_Carrot_Body_Stage01.png` |
| `Icons/Crops/Carrot/icon_Carrot_Stage2_Atomic.png` | `World/Crops/Carrot/World_Crop_Carrot_Body_Stage02.png` |
| `Icons/Crops/Carrot/icon_Carrot_Stage3_Atomic.png` | `World/Crops/Carrot/World_Crop_Carrot_Body_Stage03.png` |
| `Icons/Crops/Carrot/icon_Carrot_Dead_Atomic.png` | `World/Crops/Carrot/World_Crop_Carrot_Body_Dead.png` |
| `Icons/Crops/Carrot/icon_Seeds_Carrot_Atomic.png` | `UI/Icons/Seed/UI_Icon_Seed_Carrot_Default.png` |

### Other 6 crops (Potato, Corn, Wheat, Tomato, Strawberry, Pumpkin — 6 file mỗi loại)

Pattern: 5 stage/dead files đã đúng naming, chỉ move folder. Seed icon rename + move.

| Old path | New path |
|----------|----------|
| `Icons/Crops/[X]/World_Crop_[X]_Body_Stage00.png` | `World/Crops/[X]/World_Crop_[X]_Body_Stage00.png` |
| ... (Stage01, 02, 03, Dead) | ... |
| `Icons/Crops/[X]/icon_Seeds_[X]_Atomic.png` | `UI/Icons/Seed/UI_Icon_Seed_[X]_Default.png` |

### Chicken (5 file)

| Old | New |
|-----|-----|
| `Icons/Animals/Chicken/icon_Chicken_Stage1_Atomic.png` | `World/Animals/Chicken/World_Animal_Chicken_Body_Stage00.png` |
| `Icons/Animals/Chicken/icon_Chicken_Stage2_Atomic.png` | `World/Animals/Chicken/World_Animal_Chicken_Body_Stage01.png` |
| `Icons/Animals/Chicken/icon_Chicken_Stage3_Atomic.png` | `World/Animals/Chicken/World_Animal_Chicken_Body_Stage02.png` |
| `Icons/Animals/Chicken/icon_Chicken_Dead_Atomic.png` | `World/Animals/Chicken/World_Animal_Chicken_Body_Dead.png` |
| `Icons/Animals/Chicken/icon_Egg_Atomic.png` | `World/Products/World_Product_Egg_Collect_Ready.png` |
| `Icons/Animals/Chicken/icon_Feed_Worm_Atomic.png` | `UI/Icons/Common/UI_Icon_Common_Worm_Default.png` |

> `icon_Feed_Worm_Atomic_1.png` — duplicate, KHÔNG move (m7b delete).

### Duck (4 file — move path, giữ tên)

| Old | New |
|-----|-----|
| `Icons/Animals/Duck/World_Animal_Duck_Body_Stage00.png` | `World/Animals/Duck/World_Animal_Duck_Body_Stage00.png` |
| Stage01, Stage02, Dead | (tương tự) |
| `Icons/Animals/Duck/World_Product_DuckEgg_Collect_Ready.png` | `World/Products/World_Product_DuckEgg_Collect_Ready.png` |

### World Overlays + Tiles (4 file — move path)

| Old | New |
|-----|-----|
| `World/World_Overlay_Tile_Pest_On.png` | `World/Overlays/World_Overlay_Tile_Pest_On.png` |
| `World/World_Overlay_Tile_Weed_On.png` | `World/Overlays/World_Overlay_Tile_Weed_On.png` |
| `World/World_Overlay_Tile_WaterNeed_On.png` | `World/Overlays/World_Overlay_Tile_WaterNeed_On.png` |
| `World/World_Tile_Soil_Base_Default.png` | `World/Tiles/World_Tile_Soil_Base_Default.png` |

> `soil_empty.png`, `weed_overlay.png`, `bug_overlay.png`, `water_needed.png` — KHÔNG động (m7b xử lý sau khi prefab repoint).

### UI Backgrounds (8 file)

| Old | New |
|-----|-----|
| `UI/Backgrounds/bg_Panel_Main_Atomic.png` | `UI/Backgrounds/UI_Background_Panel_Main_Default.png` |
| `UI/Backgrounds/bg_Button_Blue_Atomic.png` | `UI/Backgrounds/UI_Background_Button_Blue_Default.png` |
| `UI/Backgrounds/bg_Button_Green_Atomic.png` | `UI/Backgrounds/UI_Background_Button_Green_Default.png` |
| `UI/Backgrounds/bg_Button_Purple_Atomic.png` | `UI/Backgrounds/UI_Background_Button_Purple_Default.png` |
| `UI/Backgrounds/bg_Button_Red_Atomic.png` | `UI/Backgrounds/UI_Background_Button_Red_Default.png` |
| `UI/Backgrounds/bg_Banner_Parchment_Atomic.png` | `UI/Backgrounds/UI_Background_Banner_Parchment_Default.png` |
| `UI/Backgrounds/bg_Chip_Resource_Atomic.png` | `UI/Backgrounds/UI_Background_Chip_Resource_Default.png` |
| `UI/bg_Plaque_Wooden_Atomic.png` (misplaced) | `UI/Backgrounds/UI_Background_Plaque_Wooden_Default.png` |

### UI Icons Common (6 file)

| Old | New |
|-----|-----|
| `UI/Icons/Common/icon_Gold_Atomic.png` | `UI/Icons/Common/UI_Icon_Common_Gold_Default.png` |
| `UI/Icons/Common/icon_XP_Atomic.png` | `UI/Icons/Common/UI_Icon_Common_XP_Default.png` |
| `UI/Icons/Common/icon_Gem_Atomic.png` | `UI/Icons/Common/UI_Icon_Common_Gem_Default.png` |
| `UI/Icons/Common/icon_Storage_Atomic.png` | `UI/Icons/Common/UI_Icon_Common_Storage_Default.png` |
| `UI/Icons/Common/icon_Energy_Atomic.png` | `UI/Icons/Common/UI_Icon_Common_Energy_Default.png` |
| `UI/Icons/Common/UI_Icon_Common_Grass_Default.png` | (đã đúng, không đổi) |

> `icon_WateringCan_Atomic.png` trong Common → chuyển sang `Action/` (xem bên dưới).

### UI Icons Nav (3 file rename — 2 đã đúng)

| Old | New |
|-----|-----|
| `UI/Icons/Nav/icon_Farm_Atomic.png` | `UI/Icons/Nav/UI_Icon_Nav_Farm_Default.png` |
| `UI/Icons/Nav/icon_Barn_Atomic.png` | `UI/Icons/Nav/UI_Icon_Nav_Barn_Default.png` |
| `UI/Icons/Nav/UI_Icon_Nav_Shop_Default.png` | (đã đúng) |
| `UI/Icons/Nav/UI_Icon_Nav_Event_Default.png` | (đã đúng) |

### UI Icons Tab (2 file, move từ UI/ root)

| Old | New |
|-----|-----|
| `UI/icon_Tab_Leaf_Atomic.png` | `UI/Icons/Tab/UI_Icon_Tab_Leaf_Default.png` |
| `UI/icon_Tab_Star_Atomic.png` | `UI/Icons/Tab/UI_Icon_Tab_Star_Default.png` |

### UI Icons Header (1 file, move từ UI/ root)

| Old | New |
|-----|-----|
| `UI/icon_Sprout_Header_Atomic.png` | `UI/Icons/Header/UI_Icon_Header_Sprout_Default.png` |

### UI Icons Action (2 file)

| Old | New |
|-----|-----|
| `UI/Icons/Common/icon_WateringCan_Atomic.png` | `UI/Icons/Action/UI_Icon_Action_Water_Default.png` |
| `UI/icon_Refresh_Atomic.png` (misplaced) | `UI/Icons/Action/UI_Icon_Action_Refresh_Default.png` |

### UI Buttons (1 file)

| Old | New |
|-----|-----|
| `UI/btn_Close_Circle_Atomic.png` | `UI/Buttons/UI_Button_Close_Circle_Default.png` |

---

## Critical Design Decisions

| Decision | Lý do |
|----------|--------|
| Dùng `assets-move` chứ không `Bash mv` | `.meta` theo file, GUID preserved → references không vỡ |
| Re-index Chicken 1-based → 0-based | Đồng nhất với Duck, Potato, Corn, v.v. (tất cả dùng Stage00+) |
| Seed icon về `UI/Icons/Seed/` | Seed là UI icon (hiện trong Shop), không phải world sprite |
| Products folder riêng | Egg/DuckEgg là "item drop từ animal", không phải body sprite của animal |
| Animal move từ `UI/Icons/Animals/` → `World/Animals/` | Animal body là world entity, không phải UI icon |
| Drop `_Atomic` suffix | Redundant — tất cả sprite đều là atomic |
| Giữ Legacy folder nguyên | m7b xóa sau khi verify 4 file không reference |

---

## Execution Order (dependency-aware)

0. **Prerequisite**: git working tree clean (commit uncommitted files trước) — Task 0
1. Tạo folder structure mới (empty, 19 folder creation call, theo thứ tự parent → child vì tool `assets-create-folder` KHÔNG đệ quy) — Task 1
2. Rename Carrot (4 stage + dead + seed) — Task 2
3. Rename/move 6 crop khác (thứ tự ID crop_02..07 = Potato, Corn, Wheat, Tomato, Strawberry, Pumpkin — match m7b) — Task 3
4. Rename Chicken (3 stage + dead) + move Egg → Products + move Worm → Common — Task 4
5. Move Duck (4 stage/dead) + move DuckEgg → Products — Task 5
6. Move World overlays + tiles — Task 6
7. Rename UI backgrounds (8) — Task 7
8. Rename UI Icons: 8a (Common + Nav + Action, 9 ops) → 8b (Tab + Header, 3 ops)
9. Rename UI Buttons (1) — Task 9
10. Post-check: Unity console 0 warning, git status check — Task 10
11. HANDOVER + commit — Task 11
