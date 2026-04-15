# SCN_Main UI Rebuild — Design Document

**Date:** 2026-04-15  
**Status:** Approved  
**Approach:** Hybrid — giữ scripts logic, build UI trực tiếp trong scene mới, tách prefab sau

---

## 1. Tổng quan

Xây dựng lại toàn bộ UI cho game NTVV trong scene mới `SCN_Main`. Scene cũ `SCN_Gameplay` giữ nguyên để tham khảo. Tất cả scripts logic hiện có được tái sử dụng — chỉ rebuild phần hierarchy và visual trong Unity Editor qua Pure MCP.

---

## 2. Global Rules (Quy tắc bắt buộc cho toàn project)

### 2.1 Layout & Resolution
- **Base resolution:** 1920×1080, landscape-first
- **Không dùng portrait layout** trong bất kỳ component nào
- **Safe band top:** HUD chỉ chiếm tối đa 12-14% chiều cao màn hình
- **Safe band bottom:** BottomNav chỉ chiếm tối đa 12-14% chiều cao màn hình
- **Center zone (72-76% giữa):** dành cho world interaction — không đặt UI cố định ở đây

### 2.2 Canvas Layer Order
| Canvas | Sort Order | Render Mode |
|--------|-----------|-------------|
| WorldRoot | — | Camera (Main Camera) |
| HUDCanvas | 10 | Screen Space Overlay |
| PopupCanvas | 20 | Screen Space Overlay |
| SystemCanvas | 30 | Screen Space Overlay |

### 2.3 Naming Convention (ui-standardization)
- **Suffix bắt buộc:** `_Label` (TMP_Text), `_Icon` (Image), `_Button` (Button), `_Fill` (Image fill), `_Panel` (container)
- **Ví dụ đúng:** `Gold_Label`, `XP_Fill`, `Close_Button`, `Item_Icon`
- **Ví dụ sai:** `goldText`, `xpBar`, `closeBtn`
- **GameObject names:** PascalCase, không dùng space

### 2.4 Color Palette (Locked)
| Token | Hex | Dùng cho |
|-------|-----|---------|
| Farm Green | #69C34D | Primary CTA, healthy states |
| Deep Leaf Green | #4FA63A | Selected states, heading accents |
| Sky Blue | #8ED8FF | HUD freshness, XP bar fill |
| Sun Yellow | #FFD75E | Coins, rewards, XP accents |
| Warm Soil Brown | #B97A4A | Farm material accents, subheads |
| Cream White | #FFF7E8 | Panels, cards, popup bases |
| Soft Orange | #FFA94D | Sell CTA, commerce accent |
| Berry Pink | #FF6FAE | Event highlight |
| Aqua Mint | #67DCC8 | Water/support/recovery |
| Warning | #FFB547 | Needs care, near full, caution |
| Danger | #FF6B5E | Near death, blocked, urgent |

**Quy tắc màu nghiêm ngặt:**
- Warning `#FFB547` ≠ Soft Orange `#FFA94D` — KHÔNG dùng lẫn lộn
- Danger chỉ dùng cho near-death và blocked states
- Soft Orange chỉ dùng cho sell/commerce

### 2.5 Typography
- Font: **Dosis-Bold** cho tất cả TMP_Text
- Số liệu HUD: minimum 24pt để đọc được trên mobile
- Button label: minimum 20pt
- Helper text / microcopy: minimum 16pt

### 2.6 Interaction Rules (Phase 1)
- **Tap object → contextual panel** — không chuyển scene
- **Không dùng global quick-tool** hay thao tác hàng loạt
- **Mỗi object chỉ hiện đúng actions phù hợp với state hiện tại**
- **Harvest/Feed phải cực nhanh** — tối thiểu bước xác nhận

### 2.7 Script Reuse Policy
- **Giữ nguyên** tất cả scripts trong `Assets/_Project/Scripts/UI/`
- **Không xóa** bất kỳ `.cs` file nào
- **Chỉ rebuild** hierarchy trong scene và prefab references

---

## 3. Scene Architecture

```
SCN_Main
├── [WORLD_ROOT]                    ← Isometric world layer
│   ├── Main Camera                 ← Camera với pan support
│   ├── CropArea                    ← Crop tiles cluster
│   └── BarnArea                    ← Animal pens cluster (camera pan target)
│
├── [HUD_CANVAS]                    ← Sort Order: 10
│   ├── TopHUDBar                   ← HUDTopBarController
│   │   ├── Avatar_Icon
│   │   ├── Level_Label
│   │   ├── XP_Fill (progress bar)
│   │   ├── Gold_Label + Gold_Icon
│   │   ├── Storage_Label
│   │   └── Settings_Button
│   └── EventBadge                  ← Floating top-right (Berry Pink)
│
├── [POPUP_CANVAS]                  ← Sort Order: 20
│   ├── ModalParent                 ← PopupManager._modalParent
│   ├── ContextActionPanel          ← CropActionPanelController (contextual)
│   └── DimOverlay                  ← Image black alpha 0.5, disabled default
│
└── [SYSTEM_CANVAS]                 ← Sort Order: 30
    ├── LoadingOverlay
    └── SaveIndicator
```

**BottomNav** nằm trong `[HUD_CANVAS]` ở bottom, persistent.

---

## 4. Component Specifications

### 4.1 TopHUDBar
- **Script:** `HUDTopBarController`
- **Anchor:** Top-stretch, height 80px
- **Layout:** Horizontal, padding 16px sides
- **Binds:** Gold, Level, XP progress, Storage used/max
- **State:** Storage_Label đổi màu Warning khi ≥80% capacity, Danger khi full

### 4.2 BottomNav
- **Script:** `BottomNavController`
- **Anchor:** Bottom-stretch, height 80px
- **Buttons:** Farm | Storage | Shop | Barn | Event
- **Selected state:** Farm Green fill, Deep Leaf Green text
- **Unselected:** Cream White, Warm Soil Brown text

### 4.3 ContextActionPanel
- **Script:** `CropActionPanelController`
- **Trigger:** Tap crop tile hoặc animal pen/animal
- **Position:** World-to-screen, offset +100px Y
- **Buttons hiện theo state:**
  - Empty tile: Plant_Button (Farm Green)
  - Growing: Water_Button, Cure_Button, Weed_Button (chỉ khi cần)
  - Ripe: Harvest_Button (Farm Green)
  - Dead: Reset_Button
  - Animal hungry: Feed_Button (Farm Green)
  - Animal ready: Collect_Button, Sell_Button (Soft Orange)

### 4.4 ShopPopup (Seed Shop)
- **Script:** `ShopPanelController`
- **Trigger:** BottomNav Shop button hoặc từ ContextActionPanel khi không đủ hạt
- **Layout:** Centered modal, max width 800px
- **Tabs:** Seeds (active) | Special (disabled phase 1)
- **Item card:** Icon + Name + Price + Stepper (−/qty/+) + Buy_Button
- **Footer:** Gold balance chip

### 4.5 StoragePopup
- **Script:** `StoragePanelController`
- **Trigger:** BottomNav Storage button
- **Layout:** Centered modal, max width 900px
- **Tabs:** All | Crops | Animals
- **Capacity strip:** Warning khi ≥80%, Danger khi full
- **Sell sub-panel:** Slide in khi chọn item

### 4.6 SellPopup (embedded trong StoragePopup)
- Không phải popup riêng — là sub-panel trong StoragePopup
- Hiện khi tap item card
- Stepper +/− số lượng, tổng vàng Sun Yellow

### 4.7 AnimalDetailPanel
- **Script:** `AnimalDetailPanelController`
- **Trigger:** Tap animal trong Barn area
- **Layout:** Side panel từ phải, width 400px
- **Sections:** Header (tên + stage) | Status (hunger, timer) | Value block | Action footer
- **Actions:** Feed (Farm Green) | Sell (Soft Orange) | Go to Food Shop (Aqua Mint)

---

## 5. Popup Loading Architecture

`PopupManager.ShowScreen(name)` load từ `Resources/UI/Default/{name}Popup.prefab`

| Screen name | Prefab key | Script |
|-------------|-----------|--------|
| "Shop" | ShopPopup | ShopPanelController |
| "Storage" | StoragePopup | StoragePanelController |
| "Inventory" | InventoryPopup | — |
| "Quest" | QuestPopup | QuestPanelController |

**Sau khi build trong scene → tách thành prefab → lưu vào `Resources/UI/Default/`**

---

## 6. Data Flow

```
User tap world object
    → CropTileView / AnimalView / AnimalPenView
    → PopupManager.ShowContextAction(target)
    → CropActionPanelController.Setup(target)
    → RefreshUI() → hiện đúng buttons theo state

User tap BottomNav button
    → BottomNavController.OnNavClick(destination)
    → PopupManager.ShowScreen(destination)
    → Instantiate prefab từ Resources/UI/Default/
    → Panel controller OnEnable() → bind data từ systems
```

---

## 7. Build Order (Task Sequence)

Thứ tự build được thiết kế để mỗi task có thể verify độc lập:

1. **Scene Setup** — Tạo SCN_Main, 4 canvas, camera
2. **HUDTopBar** — Build hierarchy + wire HUDTopBarController
3. **BottomNav** — Build hierarchy + wire BottomNavController
4. **PopupManager wiring** — Wire ModalParent, HUDParent, DimOverlay
5. **ContextActionPanel** — Build hierarchy + wire CropActionPanelController
6. **ShopPopup** — Build hierarchy + wire ShopPanelController
7. **StoragePopup** — Build hierarchy + wire StoragePanelController
8. **AnimalDetailPanel** — Build hierarchy + wire AnimalDetailPanelController
9. **Prefab extraction** — Tách từng component thành prefab
10. **Integration smoke test** — Play mode, test full flow

---

## 8. Out of Scope (Phase 1)

- SellPopup riêng (embedded trong Storage)
- FoodShopPopup (safety net — phase sau)
- EventPopup (phase sau)
- SystemCanvas loading/saving overlay (placeholder only)
- Quick tools / batch actions
- Portrait layout
