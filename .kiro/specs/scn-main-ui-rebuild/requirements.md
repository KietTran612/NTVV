# Requirements Document — SCN_Main UI Rebuild

## Introduction

Xây dựng lại toàn bộ UI cho game NTVV (Nông Trại Vui Vẻ) trong scene mới `SCN_Main`. Scene cũ `SCN_Gameplay` giữ nguyên để tham khảo. Approach là **Hybrid**: giữ toàn bộ scripts logic hiện có, build UI hierarchy trực tiếp trong Unity Editor qua Pure MCP tools, sau đó tách prefab.

Mục tiêu: có một scene hoàn chỉnh với 7 UI components của core loop, đúng visual spec, đúng data binding, sẵn sàng để test gameplay.

## Glossary

- **SCN_Main**: Scene mới, sạch hoàn toàn, thay thế SCN_Gameplay về mặt gameplay
- **Pure MCP**: Build UI trực tiếp trong Unity Editor qua MCP tools (không viết code mới)
- **HUDCanvas**: Canvas layer Sort Order 10, chứa TopHUDBar và BottomNav — persistent
- **PopupCanvas**: Canvas layer Sort Order 20, chứa tất cả modal popups và ContextActionPanel
- **SystemCanvas**: Canvas layer Sort Order 30, chứa loading/saving overlays
- **WorldRoot**: Layer world 2.5D isometric, chứa camera, crop tiles, animal pens
- **ContextActionPanel**: Panel contextual mở khi tap crop tile hoặc animal — không phải modal
- **ModalParent**: RectTransform trong PopupCanvas, là parent của tất cả popup được spawn bởi PopupManager
- **ui-standardization**: Naming convention bắt buộc — suffix `_Label`, `_Icon`, `_Button`, `_Fill`, `_Panel`
- **Atomic Sprite**: Sprite đã có sẵn trong `Assets/_Project/Art/Sprites/UI/` — dùng trực tiếp, không tạo mới

---

## Global Rules (Áp dụng cho MỌI task)

### R-GLOBAL-1: Naming Convention
- Tất cả TMP_Text components: suffix `_Label` (ví dụ: `Gold_Label`, `Level_Label`)
- Tất cả Image components dùng làm icon: suffix `_Icon` (ví dụ: `Gold_Icon`, `Storage_Icon`)
- Tất cả Button components: suffix `_Button` (ví dụ: `Close_Button`, `Buy_Button`)
- Tất cả Image components dùng làm fill/progress: suffix `_Fill` (ví dụ: `XP_Fill`)
- GameObject names: PascalCase, không dùng space

### R-GLOBAL-2: Color Palette (Locked)
| Token | Hex | Dùng cho |
|-------|-----|---------|
| Farm Green | #69C34D | Primary CTA, healthy states, selected nav |
| Deep Leaf Green | #4FA63A | Heading accents, selected states |
| Sky Blue | #8ED8FF | XP bar fill, info surfaces |
| Sun Yellow | #FFD75E | Coins, rewards, gold values |
| Warm Soil Brown | #B97A4A | Subheads, labels, panel accents |
| Cream White | #FFF7E8 | Panel backgrounds, card bases |
| Soft Orange | #FFA94D | Sell CTA, commerce buttons ONLY |
| Berry Pink | #FF6FAE | Event highlights ONLY |
| Aqua Mint | #67DCC8 | Water action, food shop, support |
| Warning | #FFB547 | Needs care, near full, caution states |
| Danger | #FF6B5E | Near death, blocked, urgent states |

**Nghiêm cấm:** Dùng Warning (#FFB547) thay cho Soft Orange (#FFA94D) hoặc ngược lại.

### R-GLOBAL-3: Typography
- Font: **Dosis-Bold** cho tất cả TMP_Text
- HUD numbers: minimum 24pt
- Button labels: minimum 20pt
- Helper/microcopy text: minimum 16pt

### R-GLOBAL-4: Layout
- Base resolution: **1920×1080, landscape-first**
- Top safe band (HUD): tối đa 80px height
- Bottom safe band (BottomNav): tối đa 80px height
- Center zone: dành cho world interaction, không đặt UI cố định
- Popup max width: 900px (không để popup quá rộng trên landscape)

### R-GLOBAL-5: Canvas Layer Order
| Canvas | Sort Order | Render Mode |
|--------|-----------|-------------|
| [HUD_CANVAS] | 10 | Screen Space Overlay |
| [POPUP_CANVAS] | 20 | Screen Space Overlay |
| [SYSTEM_CANVAS] | 30 | Screen Space Overlay |

### R-GLOBAL-6: Script Reuse
- **Không xóa** bất kỳ `.cs` file nào trong `Assets/_Project/Scripts/`
- **Không viết code mới** — chỉ wire references qua MCP `component_set`
- Tất cả scripts đã có sẵn và compile clean

### R-GLOBAL-7: Available Sprites
Dùng sprites đã có trong `Assets/_Project/Art/Sprites/UI/`:
- `Icons/Common/icon_Gold_Atomic.png` — icon vàng
- `Icons/Common/icon_Storage_Atomic.png` — icon kho
- `Icons/Common/icon_XP_Atomic.png` — icon XP
- `Icons/Common/icon_WateringCan_Atomic.png` — icon tưới nước
- `icon_Sprout_Header_Atomic.png` — icon shop header
- `icon_Tab_Leaf_Atomic.png` — icon tab seeds
- `icon_Tab_Star_Atomic.png` — icon tab special
- `btn_Close_Circle_Atomic.png` — nút đóng
- `bg_Plaque_Wooden_Atomic.png` — background gỗ cho headers

---

## Requirements

### Requirement 1: Scene Setup — 4 Canvas Architecture

**User Story:** As a developer, I want SCN_Main to have a clean 4-canvas hierarchy, so that UI layers are properly separated and manageable.

#### Acceptance Criteria

1. SCN_Main SHALL tồn tại tại `Assets/_Project/Scenes/SCN_Main.unity`
2. SCN_Main SHALL có đúng 4 root GameObjects: `[WORLD_ROOT]`, `[HUD_CANVAS]`, `[POPUP_CANVAS]`, `[SYSTEM_CANVAS]`
3. Mỗi Canvas SHALL có CanvasScaler với Reference Resolution 1920×1080, Scale With Screen Size, Match=0.5
4. Sort Order SHALL đúng: HUD=10, Popup=20, System=30
5. `[POPUP_CANVAS]` SHALL có child `ModalParent` (RectTransform stretch-stretch) và `DimOverlay` (Image đen alpha=0.5, disabled)
6. `PopupManager` singleton SHALL tồn tại trong scene với `_modalParent` và `_hudParent` được wire đúng

---

### Requirement 2: TopHUDBar

**User Story:** As a player, I want to always see my Gold, Level, XP, and Storage at the top of the screen, so that I can make informed decisions without opening any menu.

#### Acceptance Criteria

1. `TopHUDBar` SHALL nằm trong `[HUD_CANVAS]`, anchor top-stretch, height=80px
2. `TopHUDBar` SHALL hiển thị: Avatar_Icon, Level_Label, XP_Fill (progress bar), Gold_Icon + Gold_Label, Storage_Icon + Storage_Label, Settings_Button
3. `HUDTopBarController` SHALL được wire với: `_gold_Label`, `_level_Label`, `_storage_Label`, `_xp_Fill`
4. Gold_Label SHALL hiển thị số vàng hiện tại từ `EconomySystem`
5. XP_Fill SHALL fill amount tương ứng với XP progress từ `LevelSystem`
6. Storage_Label SHALL hiển thị `{used}/{max}` từ `StorageSystem`
7. Storage_Label SHALL đổi màu Warning (#FFB547) khi ≥80% capacity, Danger (#FF6B5E) khi full
8. XP_Fill color SHALL là Sky Blue (#8ED8FF)
9. Gold_Label color SHALL là Sun Yellow (#FFD75E)

---

### Requirement 3: BottomNav

**User Story:** As a player, I want a persistent bottom navigation bar, so that I can quickly switch between Farm, Storage, Shop, Barn, and Event from anywhere.

#### Acceptance Criteria

1. `BottomNav` SHALL nằm trong `[HUD_CANVAS]`, anchor bottom-stretch, height=80px
2. `BottomNav` SHALL có đúng 5 buttons: Farm, Storage, Shop, Barn, Event
3. `BottomNavController` SHALL được wire với tất cả 5 button references
4. Tap mỗi button SHALL gọi `PopupManager.ShowScreen(destination)` với đúng tên
5. Selected button SHALL hiển thị Farm Green (#69C34D) background
6. Unselected buttons SHALL hiển thị Cream White (#FFF7E8) background với Warm Soil Brown text
7. Mỗi button SHALL có icon + label text

---

### Requirement 4: ContextActionPanel

**User Story:** As a player, I want to see only the relevant actions when I tap a crop tile or animal, so that I'm never confused by irrelevant buttons.

#### Acceptance Criteria

1. `ContextActionPanel` SHALL nằm trong `[POPUP_CANVAS]/HUDParent`, disabled by default
2. `CropActionPanelController` SHALL được wire với tất cả button references và `_registry`
3. Khi tap **empty tile**: chỉ hiện `Plant_Button` (Farm Green)
4. Khi tap **growing crop với needs care**: chỉ hiện buttons phù hợp — `Water_Button` (Aqua Mint), `Cure_Button` (Warning), `Weed_Button` (Warning)
5. Khi tap **ripe crop**: chỉ hiện `Harvest_Button` (Farm Green)
6. Khi tap **dead crop**: chỉ hiện `Reset_Button` (Warm Soil Brown)
7. Khi tap **animal pen (empty)**: chỉ hiện `Buy_Button` (Farm Green)
8. Khi tap **hungry animal**: chỉ hiện `Feed_Button` (Farm Green) và `Sell_Button` (Soft Orange)
9. Khi tap **ready animal**: hiện `Collect_Button` (Farm Green) và `Sell_Button` (Soft Orange)
10. Panel SHALL có `Close_Button` luôn hiện
11. Panel SHALL position theo world-to-screen của object được tap (offset +100px Y)

---

### Requirement 5: ShopPopup (Seed Shop)

**User Story:** As a player, I want to browse and buy seeds from a clean popup, so that I can quickly plant without leaving the farm view.

#### Acceptance Criteria

1. `ShopPopup` SHALL được load bởi `PopupManager.ShowScreen("Shop")` từ `Resources/UI/Default/ShopPopup.prefab`
2. ShopPopup SHALL có Header với: `icon_Sprout_Header_Atomic`, Title "SEED SHOP", Close_Button (`btn_Close_Circle_Atomic`)
3. ShopPopup SHALL có TabBar với 2 tabs: "Hạt Giống" (active, Farm Green) và "Đặc Biệt" (disabled, grey)
4. ShopPopup SHALL có ScrollView với GridLayout hiển thị seed cards
5. Mỗi seed card SHALL hiển thị: crop icon, tên cây, giá, stepper (−/qty/+), Buy_Button
6. Buy_Button SHALL màu Farm Green (#69C34D)
7. ShopPopup SHALL có Footer với GoldBalance_Label hiển thị vàng hiện tại
8. `ShopPanelController` SHALL được wire với tất cả references
9. ShopPopup width SHALL không vượt quá 800px
10. Background SHALL là Cream White (#FFF7E8), Header background SHALL dùng `bg_Plaque_Wooden_Atomic`

---

### Requirement 6: StoragePopup

**User Story:** As a player, I want to view my inventory and sell items quickly, so that I can free up storage space without friction.

#### Acceptance Criteria

1. `StoragePopup` SHALL được load bởi `PopupManager.ShowScreen("Storage")` từ `Resources/UI/Default/StoragePopup.prefab`
2. StoragePopup SHALL có Header với: Storage icon, Title "KHO ĐỒ", Capacity_Label `{used}/{max}`, Close_Button
3. Capacity_Label SHALL đổi màu Warning khi ≥80%, Danger khi full
4. StoragePopup SHALL có 3 tabs: "Tất Cả", "Nông Sản", "Vật Nuôi"
5. StoragePopup SHALL có ScrollView với GridLayout hiển thị item cards
6. Tap item card SHALL mở SellSubPanel (slide in, không phải popup mới)
7. SellSubPanel SHALL có: tên item, stepper +/−, TotalPrice_Label (Sun Yellow), SellNow_Button (Soft Orange)
8. StoragePopup SHALL có Footer với: SellAll_Button (Soft Orange), Upgrade_Button (Farm Green)
9. `StoragePanelController` SHALL được wire với tất cả references
10. StoragePopup width SHALL không vượt quá 900px

---

### Requirement 7: AnimalDetailPanel

**User Story:** As a player, I want to see my animal's status and take action (feed/sell) from a side panel, so that I can manage animals without losing sight of the farm.

> **Trigger:** AnimalDetailPanel mở từ **Barn tab** (BottomNav → Barn → tap animal trong danh sách). Đây là full detail view.
> KHÔNG nhầm với **ContextActionPanel** (Requirement 4) — ContextActionPanel là quick-action panel khi tap trực tiếp animal tile trên farm world, không phải từ Barn tab.

#### Acceptance Criteria

1. `AnimalDetailPanel` SHALL được mở khi tap animal trong Barn area
2. AnimalDetailPanel SHALL slide in từ phải, width=400px, height=full screen
3. AnimalDetailPanel SHALL hiển thị: tên animal (`_animalName`) và growth stage (`_growthText`)
4. `_feedButton` GameObject SHALL chỉ SetActive=true khi `_target.IsHungry == true`
5. `_sellButton` GameObject SHALL luôn visible
6. Feed action SHALL gọi `OnFeedClick()` → `_target.Feed()` → RefreshUI
7. Sell action SHALL gọi `OnSellClick()` → `_target.Sell()` → panel SetActive=false
8. `AnimalDetailPanelController` SHALL được wire: `_animalName`, `_growthText`, `_feedButton`, `_sellButton`
9. Panel background SHALL là Cream White (#FFF7E8), Header background SHALL là Farm Green (#4FA63A)

---

### Requirement 8: Prefab Extraction

**User Story:** As a developer, I want all UI components extracted as prefabs in Resources/UI/Default/, so that PopupManager can load them at runtime.

#### Acceptance Criteria

1. `ShopPopup.prefab` SHALL tồn tại tại `Assets/_Project/Resources/UI/Default/ShopPopup.prefab`
2. `StoragePopup.prefab` SHALL tồn tại tại `Assets/_Project/Resources/UI/Default/StoragePopup.prefab`
3. `AnimalDetailPopup.prefab` SHALL tồn tại tại `Assets/_Project/Resources/UI/Default/AnimalDetailPopup.prefab`
4. `ContextActionPanel.prefab` SHALL tồn tại tại `Assets/_Project/Resources/UI/Default/ContextActionPanel.prefab`
5. Tất cả prefabs SHALL không có missing script references
6. `ResourcesUIProvider.LoadPrefab(key)` SHALL load thành công tất cả prefabs trên

---

### Requirement 9: Integration & Smoke Test

**User Story:** As a developer, I want the scene to run without errors in Play Mode, so that I can confirm all wiring is correct before moving to gameplay testing.

#### Acceptance Criteria

1. SCN_Main SHALL enter Play Mode không có NullReferenceException
2. SCN_Main SHALL enter Play Mode không có "Missing Component" errors
3. SCN_Main SHALL enter Play Mode không có "Failed to load UI prefab" errors
4. BottomNav buttons SHALL gọi đúng `PopupManager.ShowScreen()` khi tap
5. `PopupManager.Instance` SHALL không null khi scene load
6. `HUDTopBarController` SHALL subscribe đúng events từ EconomySystem, LevelSystem, StorageSystem
