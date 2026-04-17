# Asset Inventory — NTVV (Nông Trại Vui Vẻ)

**Date:** 2026-04-17  
**Purpose:** Tài liệu tham chiếu tất cả sprites cần có trong game. Dùng để kiểm tra trước mỗi spec execution.

---

## Legend

| Ký hiệu | Ý nghĩa |
|---|---|
| ✅ EXISTS | File tồn tại trên disk |
| ❌ MISSING | Cần tạo, chưa có |
| ⚠️ PLACEHOLDER | Dùng sprite khác tạm thời, chưa có icon riêng |
| 🔁 REUSE | Cố tình dùng chung sprite đã có |

---

## 1. World / Gameplay Sprites

> Dùng bởi `SpriteRenderer` trên CropTileView (overlay và nền tile).

| Sprite | Đường dẫn | Status | Dùng bởi |
|---|---|---|---|
| soil_empty | `Art/Sprites/World/soil_empty.png` | ✅ EXISTS | CropTileView `_soilRenderer` — nền đất trống |
| weed_overlay | `Art/Sprites/World/weed_overlay.png` | ✅ EXISTS | CropTileView `_weedVisual` — overlay cỏ dại |
| bug_overlay | `Art/Sprites/World/bug_overlay.png` | ✅ EXISTS | CropTileView `_bugVisual` — overlay sâu hại |
| water_needed | `Art/Sprites/World/water_needed.png` | ✅ EXISTS | CropTileView `_waterVisual` — overlay thiếu nước |

**Missing: 0**

---

## 2. UI Backgrounds

> Dùng bởi `Image` component trong các panel/popup.

| Sprite | Đường dẫn | Status | Dùng bởi |
|---|---|---|---|
| bg_Panel_Main_Atomic | `Art/Sprites/UI/Backgrounds/bg_Panel_Main_Atomic.png` | ✅ EXISTS | Panel backgrounds |
| bg_Button_Blue_Atomic | `Art/Sprites/UI/Backgrounds/bg_Button_Blue_Atomic.png` | ✅ EXISTS | Primary action buttons |
| bg_Button_Green_Atomic | `Art/Sprites/UI/Backgrounds/bg_Button_Green_Atomic.png` | ✅ EXISTS | Confirm/buy buttons |
| bg_Button_Purple_Atomic | `Art/Sprites/UI/Backgrounds/bg_Button_Purple_Atomic.png` | ✅ EXISTS | Special buttons |
| bg_Button_Red_Atomic | `Art/Sprites/UI/Backgrounds/bg_Button_Red_Atomic.png` | ✅ EXISTS | Danger/sell buttons |
| bg_Banner_Parchment_Atomic | `Art/Sprites/UI/Backgrounds/bg_Banner_Parchment_Atomic.png` | ✅ EXISTS | Panel headers |
| bg_Chip_Resource_Atomic | `Art/Sprites/UI/Backgrounds/bg_Chip_Resource_Atomic.png` | ✅ EXISTS | HUD resource chips |
| bg_Plaque_Wooden_Atomic | `Art/Sprites/UI/bg_Plaque_Wooden_Atomic.png` | ✅ EXISTS | Shop/Storage popup header |

**Missing: 0**

---

## 3. UI Common Icons

> Dùng bởi HUD, popup headers, tab icons dùng chung.

| Sprite | Đường dẫn | Status | Dùng bởi |
|---|---|---|---|
| icon_Gold_Atomic | `Art/Sprites/UI/Icons/Common/icon_Gold_Atomic.png` | ✅ EXISTS | HUD gold chip, shop price |
| icon_XP_Atomic | `Art/Sprites/UI/Icons/Common/icon_XP_Atomic.png` | ✅ EXISTS | HUD XP display |
| icon_Gem_Atomic | `Art/Sprites/UI/Icons/Common/icon_Gem_Atomic.png` | ✅ EXISTS | HUD gem balance |
| icon_Storage_Atomic | `Art/Sprites/UI/Icons/Common/icon_Storage_Atomic.png` | ✅ EXISTS | HUD storage chip, Nav Kho |
| icon_Energy_Atomic | `Art/Sprites/UI/Icons/Common/icon_Energy_Atomic.png` | ✅ EXISTS | HUD energy (future use) |
| icon_WateringCan_Atomic | `Art/Sprites/UI/Icons/Common/icon_WateringCan_Atomic.png` | ✅ EXISTS | CropAction panel — Water button |
| icon_Refresh_Atomic | `Art/Sprites/UI/icon_Refresh_Atomic.png` | ✅ EXISTS | StoragePanel refresh button |
| btn_Close_Circle_Atomic | `Art/Sprites/UI/btn_Close_Circle_Atomic.png` | ✅ EXISTS | Close button tất cả popup |
| icon_Sprout_Header_Atomic | `Art/Sprites/UI/icon_Sprout_Header_Atomic.png` | ✅ EXISTS | Shop popup header icon |
| icon_Tab_Leaf_Atomic | `Art/Sprites/UI/icon_Tab_Leaf_Atomic.png` | ✅ EXISTS | Shop tab Hạt Giống |
| icon_Tab_Star_Atomic | `Art/Sprites/UI/icon_Tab_Star_Atomic.png` | ✅ EXISTS | Shop tab Đặc Biệt |

**Missing: 0**

---

## 4. UI Navigation Icons (BottomNav — 5 nút)

> Mỗi nút cần 1 icon 32×32. Thứ tự: Farm, Kho, Shop, Event, Barn.

| Nút | Sprite | Đường dẫn | Status | Ghi chú |
|---|---|---|---|---|
| NavBtn_Farm | icon_Farm_Atomic | `Art/Sprites/UI/Icons/Nav/icon_Farm_Atomic.png` | ✅ EXISTS | |
| NavBtn_Storage (Kho) | icon_Storage_Atomic | (reuse Common) | 🔁 REUSE | Dùng chung với HUD chip |
| NavBtn_Shop | icon_Shop_Atomic | `Art/Sprites/UI/Icons/Nav/icon_Shop_Atomic.png` | ⚠️ PLACEHOLDER | Đang dùng icon_Sprout_Header_Atomic |
| NavBtn_Event | icon_Event_Atomic | `Art/Sprites/UI/Icons/Nav/icon_Event_Atomic.png` | ⚠️ PLACEHOLDER | Đang dùng icon_Tab_Star_Atomic |
| NavBtn_Barn | icon_Barn_Atomic | `Art/Sprites/UI/Icons/Nav/icon_Barn_Atomic.png` | ✅ EXISTS | |

**Missing: 0 / Placeholder: 2** (Shop + Event — chạy được nhưng dùng sprite tạm)

---

## 5. Crop Sprites

> CropDataSO cần: `growthStageSprites[4]` (Seedling, Growing, Large, Ripe) + `deadSprite` + `seedIcon` + `cropIcon`.
> **Tổng: 7 sprites/crop × 7 crops = 49 sprites.**

### Naming Convention
```
seedIcon       → icon_Seeds_[CropName]_Atomic.png
growthStage[0] → icon_[CropName]_Stage0_Atomic.png  (Seedling)
growthStage[1] → icon_[CropName]_Stage1_Atomic.png  (Growing)
growthStage[2] → icon_[CropName]_Stage2_Atomic.png  (Large)
growthStage[3] → icon_[CropName]_Stage3_Atomic.png  (Ripe)
deadSprite     → icon_[CropName]_Dead_Atomic.png
cropIcon       → 🔁 REUSE Stage3 (assign cùng file vào 2 slot khác nhau)
```

### crop_01 — Cà rốt (Carrot)

| Slot | File | Status |
|---|---|---|
| seedIcon | `Icons/Crops/Carrot/icon_Seeds_Carrot_Atomic.png` | ✅ EXISTS |
| growthStage[0] Seedling | `Icons/Crops/Carrot/icon_Carrot_Stage0_Atomic.png` | ✅ EXISTS |
| growthStage[1] Growing | `Icons/Crops/Carrot/icon_Carrot_Stage1_Atomic.png` | ✅ EXISTS |
| growthStage[2] Large | `Icons/Crops/Carrot/icon_Carrot_Stage2_Atomic.png` | ✅ EXISTS |
| growthStage[3] Ripe | `Icons/Crops/Carrot/icon_Carrot_Stage3_Atomic.png` | ✅ EXISTS |
| deadSprite | `Icons/Crops/Carrot/icon_Carrot_Dead_Atomic.png` | ❌ MISSING |
| cropIcon | 🔁 REUSE Stage3 | 🔁 REUSE |

### crop_02 — Khoai tây (Potato)

| Slot | File | Status |
|---|---|---|
| seedIcon | `Icons/Crops/Potato/icon_Seeds_Potato_Atomic.png` | ✅ EXISTS |
| growthStage[0] Seedling | `Icons/Crops/Potato/icon_Potato_Stage0_Atomic.png` | ❌ MISSING |
| growthStage[1] Growing | `Icons/Crops/Potato/icon_Potato_Stage1_Atomic.png` | ❌ MISSING |
| growthStage[2] Large | `Icons/Crops/Potato/icon_Potato_Stage2_Atomic.png` | ❌ MISSING |
| growthStage[3] Ripe | `Icons/Crops/Potato/icon_Potato_Stage3_Atomic.png` | ❌ MISSING |
| deadSprite | `Icons/Crops/Potato/icon_Potato_Dead_Atomic.png` | ❌ MISSING |
| cropIcon | 🔁 REUSE Stage3 | 🔁 REUSE |

### crop_03 — Ngô (Corn)

| Slot | File | Status |
|---|---|---|
| seedIcon | `Icons/Crops/Corn/icon_Seeds_Corn_Atomic.png` | ✅ EXISTS |
| growthStage[0] Seedling | `Icons/Crops/Corn/icon_Corn_Stage0_Atomic.png` | ❌ MISSING |
| growthStage[1] Growing | `Icons/Crops/Corn/icon_Corn_Stage1_Atomic.png` | ❌ MISSING |
| growthStage[2] Large | `Icons/Crops/Corn/icon_Corn_Stage2_Atomic.png` | ❌ MISSING |
| growthStage[3] Ripe | `Icons/Crops/Corn/icon_Corn_Stage3_Atomic.png` | ❌ MISSING |
| deadSprite | `Icons/Crops/Corn/icon_Corn_Dead_Atomic.png` | ❌ MISSING |
| cropIcon | 🔁 REUSE Stage3 | 🔁 REUSE |

### crop_04 — Lúa mì (Wheat)

| Slot | File | Status |
|---|---|---|
| seedIcon | `Icons/Crops/Wheat/icon_Seeds_Wheat_Atomic.png` | ✅ EXISTS |
| growthStage[0] Seedling | `Icons/Crops/Wheat/icon_Wheat_Stage0_Atomic.png` | ❌ MISSING |
| growthStage[1] Growing | `Icons/Crops/Wheat/icon_Wheat_Stage1_Atomic.png` | ❌ MISSING |
| growthStage[2] Large | `Icons/Crops/Wheat/icon_Wheat_Stage2_Atomic.png` | ❌ MISSING |
| growthStage[3] Ripe | `Icons/Crops/Wheat/icon_Wheat_Stage3_Atomic.png` | ❌ MISSING |
| deadSprite | `Icons/Crops/Wheat/icon_Wheat_Dead_Atomic.png` | ❌ MISSING |
| cropIcon | 🔁 REUSE Stage3 | 🔁 REUSE |

### crop_05 — Cà chua (Tomato)

| Slot | File | Status |
|---|---|---|
| seedIcon | `Icons/Crops/Tomato/icon_Seeds_Tomato_Atomic.png` | ✅ EXISTS |
| growthStage[0] Seedling | `Icons/Crops/Tomato/icon_Tomato_Stage0_Atomic.png` | ❌ MISSING |
| growthStage[1] Growing | `Icons/Crops/Tomato/icon_Tomato_Stage1_Atomic.png` | ❌ MISSING |
| growthStage[2] Large | `Icons/Crops/Tomato/icon_Tomato_Stage2_Atomic.png` | ❌ MISSING |
| growthStage[3] Ripe | `Icons/Crops/Tomato/icon_Tomato_Stage3_Atomic.png` | ❌ MISSING |
| deadSprite | `Icons/Crops/Tomato/icon_Tomato_Dead_Atomic.png` | ❌ MISSING |
| cropIcon | 🔁 REUSE Stage3 | 🔁 REUSE |

### crop_06 — Dâu tây (Strawberry)

| Slot | File | Status |
|---|---|---|
| seedIcon | `Icons/Crops/Strawberry/icon_Seeds_Strawberry_Atomic.png` | ✅ EXISTS |
| growthStage[0] Seedling | `Icons/Crops/Strawberry/icon_Strawberry_Stage0_Atomic.png` | ❌ MISSING |
| growthStage[1] Growing | `Icons/Crops/Strawberry/icon_Strawberry_Stage1_Atomic.png` | ❌ MISSING |
| growthStage[2] Large | `Icons/Crops/Strawberry/icon_Strawberry_Stage2_Atomic.png` | ❌ MISSING |
| growthStage[3] Ripe | `Icons/Crops/Strawberry/icon_Strawberry_Stage3_Atomic.png` | ❌ MISSING |
| deadSprite | `Icons/Crops/Strawberry/icon_Strawberry_Dead_Atomic.png` | ❌ MISSING |
| cropIcon | 🔁 REUSE Stage3 | 🔁 REUSE |

### crop_07 — Bí đỏ (Pumpkin)

| Slot | File | Status |
|---|---|---|
| seedIcon | `Icons/Crops/Pumpkin/icon_Seeds_Pumpkin_Atomic.png` | ✅ EXISTS |
| growthStage[0] Seedling | `Icons/Crops/Pumpkin/icon_Pumpkin_Stage0_Atomic.png` | ❌ MISSING |
| growthStage[1] Growing | `Icons/Crops/Pumpkin/icon_Pumpkin_Stage1_Atomic.png` | ❌ MISSING |
| growthStage[2] Large | `Icons/Crops/Pumpkin/icon_Pumpkin_Stage2_Atomic.png` | ❌ MISSING |
| growthStage[3] Ripe | `Icons/Crops/Pumpkin/icon_Pumpkin_Stage3_Atomic.png` | ❌ MISSING |
| deadSprite | `Icons/Crops/Pumpkin/icon_Pumpkin_Dead_Atomic.png` | ❌ MISSING |
| cropIcon | 🔁 REUSE Stage3 | 🔁 REUSE |

**Crop Summary:**
- ✅ EXISTS: 13 (7 seeds + Carrot 4 stages + Stage0)
- ❌ MISSING: 31 (6 crops × 5 sprites + Carrot dead)
- 🔁 REUSE: 7 (cropIcon dùng lại Stage3)

---

## 6. Animal Sprites

> AnimalDataSO cần: `stageSprites[3]` (Baby, Stage2, Mature) + `deadSprite` + `readyToCollectIcon`.
> **Tổng: 5 sprites/animal × 2 animals = 10 sprites.**

### Naming Convention
```
stageSprites[0] Baby   → icon_[AnimalName]_Stage1_Atomic.png
stageSprites[1] Stage2 → icon_[AnimalName]_Stage2_Atomic.png
stageSprites[2] Mature → icon_[AnimalName]_Stage3_Atomic.png
deadSprite             → icon_[AnimalName]_Dead_Atomic.png
readyToCollectIcon     → icon_[ProductName]_Atomic.png
```

### animal_01 — Gà (Chicken) | produceItemId: item_egg

| Slot | File | Status |
|---|---|---|
| stageSprites[0] Baby | `Icons/Animals/Chicken/icon_Chicken_Stage1_Atomic.png` | ✅ EXISTS |
| stageSprites[1] Stage2 | `Icons/Animals/Chicken/icon_Chicken_Stage2_Atomic.png` | ✅ EXISTS |
| stageSprites[2] Mature | `Icons/Animals/Chicken/icon_Chicken_Stage3_Atomic.png` | ✅ EXISTS |
| deadSprite | `Icons/Animals/Chicken/icon_Chicken_Dead_Atomic.png` | ❌ MISSING |
| readyToCollectIcon (trứng) | `Icons/Animals/Chicken/icon_Egg_Atomic.png` | ✅ EXISTS |

### animal_02 — Vịt (Duck) | produceItemId: item_duck_egg *(xác nhận lại)*

| Slot | File | Status |
|---|---|---|
| stageSprites[0] Baby | `Icons/Animals/Duck/icon_Duck_Stage1_Atomic.png` | ❌ MISSING |
| stageSprites[1] Stage2 | `Icons/Animals/Duck/icon_Duck_Stage2_Atomic.png` | ❌ MISSING |
| stageSprites[2] Mature | `Icons/Animals/Duck/icon_Duck_Stage3_Atomic.png` | ❌ MISSING |
| deadSprite | `Icons/Animals/Duck/icon_Duck_Dead_Atomic.png` | ❌ MISSING |
| readyToCollectIcon | `Icons/Animals/Duck/icon_DuckEgg_Atomic.png` | ❌ MISSING |

**Animal Summary:**
- ✅ EXISTS: 4 (Chicken Stage1-3 + Egg icon)
- ❌ MISSING: 6 (Chicken dead + Duck tất cả)

---

## 7. Item Icons (Storage / Inventory Display)

> Dùng bởi StoragePanelController khi hiển thị items trong kho.

| Item ID | Sprite | Đường dẫn | Status | Ghi chú |
|---|---|---|---|---|
| item_grass | icon_Grass_Atomic | `Icons/Common/icon_Grass_Atomic.png` | ❌ MISSING | Sản phẩm từ ClearWeeds |
| item_worm | icon_Worm_Atomic | `Icons/Common/icon_Worm_Atomic.png` | 🔁 REUSE | Dùng icon_Feed_Worm_Atomic.png đã có |
| crop_01 (Cà rốt) | 🔁 REUSE Stage3 | `Icons/Crops/Carrot/icon_Carrot_Stage3_Atomic.png` | 🔁 REUSE | |
| crop_02 (Khoai tây) | 🔁 REUSE Stage3 | Phụ thuộc crop | phụ thuộc crop | |
| crop_03 (Ngô) | 🔁 REUSE Stage3 | Phụ thuộc crop | phụ thuộc crop | |
| crop_04 (Lúa mì) | 🔁 REUSE Stage3 | Phụ thuộc crop | phụ thuộc crop | |
| crop_05 (Cà chua) | 🔁 REUSE Stage3 | Phụ thuộc crop | phụ thuộc crop | |
| crop_06 (Dâu tây) | 🔁 REUSE Stage3 | Phụ thuộc crop | phụ thuộc crop | |
| crop_07 (Bí đỏ) | 🔁 REUSE Stage3 | Phụ thuộc crop | phụ thuộc crop | |
| item_egg | icon_Egg_Atomic | `Icons/Animals/Chicken/icon_Egg_Atomic.png` | ✅ EXISTS | Sản phẩm Gà |
| item_duck_egg | icon_DuckEgg_Atomic | `Icons/Animals/Duck/icon_DuckEgg_Atomic.png` | ❌ MISSING | Sản phẩm Vịt |

---

## 8. Feed Icons (Hiển thị trong AnimalDetailPanel)

| Feed Item | Sprite | Đường dẫn | Status | Dùng bởi |
|---|---|---|---|---|
| item_grass (cỏ) | icon_Grass_Atomic | `Icons/Common/icon_Grass_Atomic.png` | ❌ MISSING | Gà + Vịt feed display |
| item_worm (sâu) | icon_Feed_Worm_Atomic | `Icons/Animals/Chicken/icon_Feed_Worm_Atomic.png` | ✅ EXISTS | Gà + Vịt feed display |

---

## Tổng kết

| Hạng mục | Tổng | ✅ EXISTS | ❌ MISSING | ⚠️ PLACEHOLDER | 🔁 REUSE |
|---|---|---|---|---|---|
| World Sprites | 4 | 4 | 0 | 0 | 0 |
| UI Backgrounds | 8 | 8 | 0 | 0 | 0 |
| UI Common Icons | 11 | 11 | 0 | 0 | 0 |
| UI Nav Icons | 5 | 2 | 0 | 2 | 1 |
| Crop Sprites | 49 | 13 | 31 | 0 | 7 |
| Animal Sprites | 10 | 4 | 6 | 0 | 0 |
| Item Icons | 11 | 1 | 2 | 0 | 8 |
| Feed Icons | 2 | 1 | 1 | 0 | 0 |
| **Tổng** | **100** | **44** | **40** | **2** | **16** |

### **Cần tạo: 40 sprites mới + 2 placeholder cần icon riêng**

---

## Danh sách 40 Sprites Cần Tạo

### Nhóm A — Crops (31 sprites)

> Style: Isometric 2.5D, cute cartoon, transparent background, 512×512.

| # | File | Mô tả |
|---|---|---|
| 1 | icon_Carrot_Dead_Atomic.png | Cà rốt chết — cây héo úa, màu nâu xám |
| 2 | icon_Potato_Stage0_Atomic.png | Khoai tây — mầm nhỏ mới nhú |
| 3 | icon_Potato_Stage1_Atomic.png | Khoai tây — cây con xếp |
| 4 | icon_Potato_Stage2_Atomic.png | Khoai tây — cây lớn vừa |
| 5 | icon_Potato_Stage3_Atomic.png | Khoai tây — cây chín, lồi củ |
| 6 | icon_Potato_Dead_Atomic.png | Khoai tây chết |
| 7 | icon_Corn_Stage0_Atomic.png | Ngô — mầm nhỏ |
| 8 | icon_Corn_Stage1_Atomic.png | Ngô — cây còn non |
| 9 | icon_Corn_Stage2_Atomic.png | Ngô — cây cao vừa |
| 10 | icon_Corn_Stage3_Atomic.png | Ngô — trái chín vàng |
| 11 | icon_Corn_Dead_Atomic.png | Ngô chết |
| 12 | icon_Wheat_Stage0_Atomic.png | Lúa mì — mầm nhỏ |
| 13 | icon_Wheat_Stage1_Atomic.png | Lúa mì — thân xanh non |
| 14 | icon_Wheat_Stage2_Atomic.png | Lúa mì — bông lúa xuất hiện |
| 15 | icon_Wheat_Stage3_Atomic.png | Lúa mì — bông lúa vàng chín |
| 16 | icon_Wheat_Dead_Atomic.png | Lúa mì chết |
| 17 | icon_Tomato_Stage0_Atomic.png | Cà chua — mầm nhỏ |
| 18 | icon_Tomato_Stage1_Atomic.png | Cà chua — cây con |
| 19 | icon_Tomato_Stage2_Atomic.png | Cà chua — có hoa vàng |
| 20 | icon_Tomato_Stage3_Atomic.png | Cà chua — trái đỏ chín |
| 21 | icon_Tomato_Dead_Atomic.png | Cà chua chết |
| 22 | icon_Strawberry_Stage0_Atomic.png | Dâu tây — mầm nhỏ |
| 23 | icon_Strawberry_Stage1_Atomic.png | Dâu tây — lá non xanh |
| 24 | icon_Strawberry_Stage2_Atomic.png | Dâu tây — hoa trắng |
| 25 | icon_Strawberry_Stage3_Atomic.png | Dâu tây — trái đỏ chín |
| 26 | icon_Strawberry_Dead_Atomic.png | Dâu tây chết |
| 27 | icon_Pumpkin_Stage0_Atomic.png | Bí đỏ — mầm nhỏ |
| 28 | icon_Pumpkin_Stage1_Atomic.png | Bí đỏ — dây bí nhỏ |
| 29 | icon_Pumpkin_Stage2_Atomic.png | Bí đỏ — dây bí lớn, quả nhỏ xanh |
| 30 | icon_Pumpkin_Stage3_Atomic.png | Bí đỏ — quả to cam chín |
| 31 | icon_Pumpkin_Dead_Atomic.png | Bí đỏ chết |

### Nhóm B — Animals (6 sprites)

> Style: Cute cartoon 2D, transparent background, 256×256.

| # | File | Mô tả |
|---|---|---|
| 32 | icon_Chicken_Dead_Atomic.png | Gà chết — nằm xuống, x cross mắt |
| 33 | icon_Duck_Stage1_Atomic.png | Vịt Baby — vịt con màu vàng |
| 34 | icon_Duck_Stage2_Atomic.png | Vịt lớn vừa — bắt đầu màu trắng |
| 35 | icon_Duck_Stage3_Atomic.png | Vịt trưởng thành — vịt trắng, mỏ vàng |
| 36 | icon_Duck_Dead_Atomic.png | Vịt chết |
| 37 | icon_DuckEgg_Atomic.png | Trứng vịt — trứng xanh nhạt |

### Nhóm C — Items & Nav (3 sprites)

| # | File | Mô tả |
|---|---|---|
| 38 | icon_Grass_Atomic.png | Cỏ — bundle cỏ xanh nhỏ (storage + feed display) |
| 39 | icon_Shop_Atomic.png | Nav Shop — cái giỏ hàng hoặc cửa hàng |
| 40 | icon_Event_Atomic.png | Nav Event — ngôi sao hoặc cờ sự kiện |

---

## Lưu ý ưu tiên

| Nhóm | Sprites | Blocking? | Lý do |
|---|---|---|---|
| Duck stages (33-35, 37) | 4 sprites | ⚠️ CÓ nếu dùng M4 integration test | AnimalView cần sprite để render |
| Crop stage (2-30) | 29 sprites | ❌ Không | CropTileView có fallback color placeholder |
| icon_Grass_Atomic (38) | 1 sprite | ❌ Không | Storage hiển thị được, chỉ thiếu icon |
| Dead sprites (1, 6, 11...) | 7 sprites | ❌ Không | Fallback màu xám |
| Nav icons (39-40) | 2 sprites | ❌ Không | Đang dùng placeholder icon |
| Duck dead + egg (36-37) | 2 sprites | ⚠️ Nên có với M4 | AnimalView.CollectProduct() dùng readyToCollectIcon |
