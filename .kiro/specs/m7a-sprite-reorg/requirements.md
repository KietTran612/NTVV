# Requirements: m7a-sprite-reorg

## Overview

Rename tất cả sprite hiện có về 1 naming convention duy nhất `[Domain]_[Category]_[Entity]_[Variant]_[State].png` và tổ chức lại folder `Assets/_Project/Art/Sprites/` cho gọn gàng. KHÔNG wire SO ở spec này — tách sang m7b.

**Prerequisite:** Không. Chạy độc lập. Nhưng trước m7b.

**Design doc:** `docs/superpowers/specs/2026-04-21-m7-sprite-reorg-and-wireup-design.md`

---

## Global Rules

> ⚠️ ĐỌC TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO

- BẮT BUỘC dùng `assets-move` (Unity API) để đổi tên/di chuyển — không dùng `Bash mv`. Lý do: `.meta` đi theo file, GUID giữ nguyên, references không vỡ.
- `assets-refresh` sau mỗi batch, CHỜ import xong trước khi tiếp tục
- `console-get-logs` filter=Error|Warning sau mỗi batch → 0 "missing sprite", 0 "Could not extract GUID"
- Quick Test FAIL → fix trong task hiện tại, KHÔNG sang task tiếp theo (tối đa 2 retry)
- KHÔNG xóa file ở spec này — để m7b xử lý sau khi wire xong
- KHÔNG sửa SO/prefab/scene content ở spec này — chỉ rename/move

---

## Functional Requirements

### Req 1 — Folder Structure
- 1.1 Top-level chỉ có 2 folder trong `Art/Sprites/`: `World/` và `UI/`
- 1.2 `World/` chứa: `Crops/`, `Animals/`, `Products/`, `Overlays/`, `Tiles/`
- 1.3 `UI/` chứa: `Backgrounds/`, `Icons/`, `Buttons/`
- 1.4 `UI/Icons/` chứa subfolder: `Common/`, `Seed/`, `Nav/`, `Tab/`, `Header/`, `Action/`
- 1.5 Không còn folder `UI/Icons/Animals/` (đã chuyển sang `World/Animals/`)
- 1.6 Folder `UI/Legacy/` tồn tại nhưng không thay đổi (m7b xóa sau)

### Req 2 — Naming Convention
- 2.1 Format thống nhất: `[Domain]_[Category]_[Entity]_[Variant]_[State].png`
- 2.2 Domain: `World` hoặc `UI`
- 2.3 Stage index 0-based: `Stage00`, `Stage01`, `Stage02`, `Stage03`
- 2.4 Single-state: suffix `_Default`
- 2.5 Bỏ suffix `_Atomic` khỏi tất cả file
- 2.6 0 file còn naming old (`icon_*_Atomic.png`, `bg_*_Atomic.png`, `btn_*_Atomic.png`) trừ trong Legacy folder

### Req 3 — Crop Sprites (World/Crops/)
- 3.1 7 loại crop, mỗi loại 5 file: `Stage00`, `Stage01`, `Stage02`, `Stage03`, `Dead`
- 3.2 Carrot rename từ `icon_Carrot_Stage0..3_Atomic.png` + `icon_Carrot_Dead_Atomic.png` → `World_Crop_Carrot_Body_Stage00..03.png` + `Dead.png`
- 3.3 6 loại khác (Potato, Corn, Wheat, Tomato, Strawberry, Pumpkin): đã có new naming, chỉ move folder path
- 3.4 Seed icon KHÔNG ở đây — move sang `UI/Icons/Seed/`

### Req 4 — Animal Sprites (World/Animals/)
- 4.1 Chicken 4 file: `World_Animal_Chicken_Body_Stage00..02.png` + `Dead.png`
- 4.2 Chicken rename từ `icon_Chicken_Stage1..3_Atomic.png` → `Stage00..02.png` (1→0, 2→1, 3→2 — re-index)
- 4.3 Chicken `icon_Chicken_Dead_Atomic.png` → `World_Animal_Chicken_Body_Dead.png`
- 4.4 Duck 4 file: giữ naming hiện có, chỉ move folder từ `UI/Icons/Animals/Duck/` → `World/Animals/Duck/`

### Req 5 — Product Sprites (World/Products/)
- 5.1 `World_Product_Egg_Collect_Ready.png` (rename từ `icon_Egg_Atomic.png`)
- 5.2 `World_Product_DuckEgg_Collect_Ready.png` (move từ `UI/Icons/Animals/Duck/`)

### Req 6 — World Overlays + Tiles
- 6.1 `World/Overlays/`: 3 file `World_Overlay_Tile_Pest/Weed/WaterNeed_On.png` (move từ `World/`)
- 6.2 `World/Tiles/`: 1 file `World_Tile_Soil_Base_Default.png` (move từ `World/`)
- 6.3 Old lowercase sprite (`soil_empty.png`, `weed_overlay.png`, `bug_overlay.png`, `water_needed.png`) KHÔNG động ở spec này — m7b xóa sau khi CropTile prefab đã repoint

### Req 7 — UI Backgrounds
- 7.1 8 file rename sang format `UI_Background_[Variant]_Default.png`:
  - `bg_Panel_Main_Atomic.png` → `UI_Background_Panel_Main_Default.png`
  - `bg_Button_Blue_Atomic.png` → `UI_Background_Button_Blue_Default.png` (+ Green, Purple, Red)
  - `bg_Banner_Parchment_Atomic.png` → `UI_Background_Banner_Parchment_Default.png`
  - `bg_Chip_Resource_Atomic.png` → `UI_Background_Chip_Resource_Default.png`
  - `bg_Plaque_Wooden_Atomic.png` → `UI_Background_Plaque_Wooden_Default.png` (move từ `UI/` root)

### Req 8 — UI Icons
- 8.1 Common (7): Gold, XP, Gem, Storage, Energy, WateringCan, Grass (1 đã có `UI_Icon_Common_Grass_Default.png`)
  - `icon_Gold_Atomic.png` → `UI_Icon_Common_Gold_Default.png`
  - `icon_WateringCan_Atomic.png` → di chuyển sang `UI/Icons/Action/` (không phải Common — là action icon)
  - `icon_Feed_Worm_Atomic.png` → `UI_Icon_Common_Worm_Default.png` (move từ Animals/Chicken/)
- 8.2 Seed (7): `icon_Seeds_[Crop]_Atomic.png` → `UI_Icon_Seed_[Crop]_Default.png` (move từ `Icons/Crops/[Crop]/`)
- 8.3 Nav (5): 2 file đã có `UI_Icon_Nav_Shop/Event_Default.png`; rename còn lại:
  - `icon_Farm_Atomic.png` → `UI_Icon_Nav_Farm_Default.png`
  - `icon_Barn_Atomic.png` → `UI_Icon_Nav_Barn_Default.png`
  - Storage reuse: `UI_Icon_Common_Storage_Default.png` — không tạo bản Nav riêng
- 8.4 Tab (2): `icon_Tab_Leaf/Star_Atomic.png` → `UI_Icon_Tab_Leaf/Star_Default.png`
- 8.5 Header (1): `icon_Sprout_Header_Atomic.png` → `UI_Icon_Header_Sprout_Default.png`
- 8.6 Action (2): `icon_WateringCan_Atomic.png` → `UI_Icon_Action_Water_Default.png`; `icon_Refresh_Atomic.png` → `UI_Icon_Action_Refresh_Default.png`

### Req 9 — UI Buttons
- 9.1 `btn_Close_Circle_Atomic.png` → `UI_Button_Close_Circle_Default.png`

### Req 10 — Integration Verification
- 10.1 Sau khi mass rename, mở Unity → Console: 0 "missing sprite", 0 "Could not extract GUID"
- 10.2 Tất cả SO và Prefab reference sprite vẫn trỏ đúng file (Inspector không hiện "Missing")
- 10.3 Git diff: ~75 rename entries (R), không phải delete+create
