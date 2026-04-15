# SCN_Main UI Rebuild — Implementation Plan

> **REQUIRED WORKFLOW:** Use the `executing-plans` skill to execute this plan in single-flow mode.

**Goal:** Build toàn bộ UI cho SCN_Main từ đầu qua Pure MCP — 4 canvas, 7 UI components, wire scripts, extract prefabs.

**Architecture:** Hybrid — giữ scripts logic hiện có, build hierarchy trực tiếp trong SCN_Main qua MCP tools, tách prefab sau khi structure ổn. PopupManager load prefab từ `Resources/UI/Default/`.

**Tech Stack:** Unity uGUI, TextMeshPro, C# (scripts tái sử dụng), Pure MCP (scene_new, game_object_create, component_add, component_set)

**Design doc:** `docs/plans/2026-04-15-scn-main-ui-rebuild-design.md`

---

## Global Rules — ĐỌC TRƯỚC KHI LÀM BẤT KỲ TASK NÀO

- **Naming:** suffix `_Label`, `_Icon`, `_Button`, `_Fill`, `_Panel` — bắt buộc
- **Colors:** Farm Green `#69C34D`, Cream White `#FFF7E8`, Sun Yellow `#FFD75E`, Warning `#FFB547`, Danger `#FF6B5E`, Soft Orange `#FFA94D`
- **Font:** Dosis-Bold cho tất cả TMP_Text
- **Resolution:** 1920×1080 landscape
- **Canvas Sort Order:** HUD=10, Popup=20, System=30
- **Verify sau mỗi task:** screenshot + console check (no errors)

---

## Task 1: Tạo SCN_Main và 4 Canvas

**Files:**
- Create: `Assets/_Project/Scenes/SCN_Main.unity`

**Step 1: Tạo scene mới**

Dùng `scene_new` để tạo scene `SCN_Main`.

**Step 2: Tạo 4 root GameObjects**

Dùng `game_object_create` lần lượt:
- `[WORLD_ROOT]` — empty, position (0,0,0)
- `[HUD_CANVAS]` — thêm component `Canvas`, `CanvasScaler`, `GraphicRaycaster`
- `[POPUP_CANVAS]` — thêm component `Canvas`, `CanvasScaler`, `GraphicRaycaster`
- `[SYSTEM_CANVAS]` — thêm component `Canvas`, `CanvasScaler`, `GraphicRaycaster`

**Step 3: Cấu hình Canvas settings**

Với mỗi Canvas, dùng `component_set`:
- Render Mode: `Screen Space - Overlay`
- Sort Order: HUD=10, Popup=20, System=30

Với mỗi CanvasScaler:
- UI Scale Mode: `Scale With Screen Size`
- Reference Resolution: 1920×1080
- Screen Match Mode: `Match Width Or Height`, Match=0.5

**Step 4: Tạo Main Camera trong WORLD_ROOT**

Dùng `game_object_create_primitive` hoặc tạo Camera GameObject trong `[WORLD_ROOT]`.

**Step 5: Save scene**

Dùng `editor_save_scene`.

**Step 6: Verify**

Dùng `screenshot_scene` để xác nhận 4 root objects tồn tại trong hierarchy.

---

## Task 2: Build TopHUDBar

**Files:**
- Modify: Scene `SCN_Main.unity` — thêm vào `[HUD_CANVAS]`
- Script: `Assets/_Project/Scripts/UI/HUD/HUDTopBarController.cs` (giữ nguyên)

**Step 1: Tạo TopHUDBar container**

Trong `[HUD_CANVAS]`, tạo GameObject `TopHUDBar`:
- Component: `Image` (background, color `#FFF7E8`)
- Component: `HorizontalLayoutGroup` (padding: left=16, right=16, spacing=12, childAlignment=MiddleLeft)
- RectTransform: anchor top-stretch, height=80, pos Y=0

**Step 2: Tạo các child elements**

Trong `TopHUDBar`, tạo theo thứ tự:

```
TopHUDBar
├── Avatar_Icon          ← Image, width=48, height=48
├── LevelChip            ← Image (#4FA63A), HorizontalLayoutGroup
│   ├── Level_Label      ← TMP_Text "Level 1", Dosis-Bold 20pt, white
│   └── XP_Fill          ← Image (fill, #8ED8FF), fillMethod=Horizontal
├── GoldChip             ← Image (#FFF7E8), HorizontalLayoutGroup
│   ├── Gold_Icon        ← Image (coin sprite), width=24
│   └── Gold_Label       ← TMP_Text "0", Dosis-Bold 24pt, #FFD75E
├── StorageChip          ← Image (#FFF7E8), HorizontalLayoutGroup
│   ├── Storage_Icon     ← Image (warehouse sprite), width=24
│   └── Storage_Label    ← TMP_Text "0/50", Dosis-Bold 20pt, #B97A4A
└── Settings_Button      ← Button, Image (gear icon), width=40, height=40
```

**Step 3: Add HUDTopBarController**

Trên `TopHUDBar` GameObject, dùng `component_add` để thêm `HUDTopBarController`.

**Step 4: Wire references**

Dùng `component_set` trên `HUDTopBarController`:
- `_gold_Label` → `TopHUDBar/GoldChip/Gold_Label`
- `_level_Label` → `TopHUDBar/LevelChip/Level_Label`
- `_storage_Label` → `TopHUDBar/StorageChip/Storage_Label`
- `_xp_Fill` → `TopHUDBar/LevelChip/XP_Fill`

**Step 5: Save + Screenshot**

`editor_save_scene` → `screenshot_scene` → verify layout đúng top band.

---

## Task 3: Build BottomNav

**Files:**
- Modify: Scene `SCN_Main.unity` — thêm vào `[HUD_CANVAS]`
- Script: `Assets/_Project/Scripts/UI/Common/BottomNavController.cs` (giữ nguyên)

**Step 1: Tạo BottomNav container**

Trong `[HUD_CANVAS]`, tạo `BottomNav`:
- Component: `Image` (background `#FFF7E8`, shadow nhẹ)
- Component: `HorizontalLayoutGroup` (spacing=0, childForceExpandWidth=true)
- RectTransform: anchor bottom-stretch, height=80

**Step 2: Tạo 5 nav buttons**

Mỗi button có cấu trúc:
```
NavBtn_Farm (Button + Image)
├── NavIcon_Farm    ← Image (sprite), width=32, height=32
└── NavLabel_Farm   ← TMP_Text "Farm", Dosis-Bold 16pt
```

Tạo tương tự cho: `NavBtn_Storage`, `NavBtn_Shop`, `NavBtn_Barn`, `NavBtn_Event`

**Step 3: Add BottomNavController**

Trên `BottomNav`, thêm `BottomNavController`.

**Step 4: Wire references**

Dùng `component_set`:
- `_btnFarm` → `BottomNav/NavBtn_Farm`
- `_btnStorage` → `BottomNav/NavBtn_Storage`
- `_btnShop` → `BottomNav/NavBtn_Shop`
- `_btnBarn` → `BottomNav/NavBtn_Barn`
- `_btnEvent` → `BottomNav/NavBtn_Event`

**Step 5: Save + Screenshot**

Verify bottom band đúng vị trí, 5 buttons đều hiện.

---

## Task 4: Wire PopupManager

**Files:**
- Modify: Scene `SCN_Main.unity` — thêm vào `[POPUP_CANVAS]`
- Script: `Assets/_Project/Scripts/UI/Common/PopupManager.cs` (giữ nguyên)

**Step 1: Tạo structure trong POPUP_CANVAS**

```
[POPUP_CANVAS]
├── ModalParent      ← RectTransform, anchor stretch-stretch (full canvas)
├── HUDParent        ← RectTransform, anchor stretch-stretch
└── DimOverlay       ← Image, color black alpha=0.5, disabled by default
```

**Step 2: Tạo PopupManager GameObject**

Tạo empty GameObject `PopupManager` trong scene root (không nằm trong canvas).

Thêm component `PopupManager`.

**Step 3: Wire references**

Dùng `component_set` trên `PopupManager`:
- `_mainOverlayCanvas` → `[POPUP_CANVAS]` (Canvas component)
- `_modalParent` → `[POPUP_CANVAS]/ModalParent`
- `_hudParent` → `[POPUP_CANVAS]/HUDParent`

**Step 4: Save + Verify**

`editor_save_scene` → `console_get_logs` — không có error về missing references.

---

## Task 5: Build ContextActionPanel

**Files:**
- Modify: Scene `SCN_Main.unity` — thêm vào `[POPUP_CANVAS]/HUDParent`
- Script: `Assets/_Project/Scripts/UI/Panels/CropActionPanelController.cs` (giữ nguyên)
- Data: `GameDataRegistrySO` cần được assign

**Step 1: Tạo panel container**

Trong `[POPUP_CANVAS]/HUDParent`, tạo `ContextActionPanel`:
- Component: `Image` (background `#FFF7E8`, rounded corners)
- Component: `VerticalLayoutGroup` (padding=16, spacing=8)
- RectTransform: width=280, height=auto (content size fitter)
- **SetActive: false** (hidden by default)

**Step 2: Tạo Header**

```
ContextActionPanel
├── Header
│   ├── CropIcon_Icon    ← Image, width=40, height=40
│   └── Header_Label     ← TMP_Text "Mảnh Đất", Dosis-Bold 22pt, #4FA63A
```

**Step 3: Tạo Action Buttons**

Mỗi button: Image background + TMP_Text label, height=44, width=fill

```
├── Plant_Button      ← Image #69C34D, label "Gieo Hạt" white
├── Harvest_Button    ← Image #69C34D, label "Thu Hoạch" white
├── Reset_Button      ← Image #B97A4A, label "Dọn Đất" white
├── Water_Button      ← Image #67DCC8, label "Tưới Nước" white
├── Cure_Button       ← Image #FFB547, label "Bắt Sâu" white
├── Weed_Button       ← Image #FFB547, label "Cắt Cỏ" white
├── Buy_Button        ← Image #69C34D, label "Mua Giống" white
├── Feed_Button       ← Image #69C34D, label "Cho Ăn" white
├── Sell_Button       ← Image #FFA94D, label "Bán Ngay" white
├── Collect_Button    ← Image #69C34D, label "Thu Sản Phẩm" white
└── Close_Button      ← Image #B97A4A, label "✕" white, width=44
```

**Step 4: Add CropActionPanelController**

Thêm `CropActionPanelController` vào `ContextActionPanel`.

**Step 5: Wire tất cả references**

Dùng `component_set`:
- `_headerText` → `Header/Header_Label`
- `_plantButton` → `Plant_Button`
- `_harvestButton` → `Harvest_Button`
- `_resetButton` → `Reset_Button`
- `_waterButton` → `Water_Button`
- `_cureButton` → `Cure_Button`
- `_weedButton` → `Weed_Button`
- `_buyButton` → `Buy_Button`
- `_feedButton` → `Feed_Button`
- `_sellButton` → `Sell_Button`
- `_collectButton` → `Collect_Button`
- `_closeButton` → `Close_Button`
- `_registry` → `GameDataRegistrySO` asset (tìm trong project)

**Step 6: Save + Verify**

Screenshot → confirm panel tồn tại nhưng hidden. Console check no errors.

---

## Task 6: Build ShopPopup

**Files:**
- Modify: Scene `SCN_Main.unity` — build trong scene, sau đó extract prefab
- Script: `Assets/_Project/Scripts/UI/Panels/ShopPanelController.cs` (giữ nguyên)
- Script: `Assets/_Project/Scripts/UI/Panels/ShopEntryController.cs` (giữ nguyên)

**Step 1: Tạo ShopPopup root**

Trong `[POPUP_CANVAS]/ModalParent`, tạo `ShopPopup`:
- Component: `Image` (background `#FFF7E8`)
- RectTransform: anchor center, width=800, height=600
- **SetActive: false**

**Step 2: Build Header**

```
ShopPopup
├── Header                    ← Image #6B3A1F, height=60, HorizontalLayoutGroup
│   ├── ShopIcon_Icon         ← Image (seed icon), width=36
│   ├── Title_Label           ← TMP_Text "SEED SHOP", Dosis-Bold 24pt, white
│   └── Close_Button          ← Button, Image #FF6B5E circle, width=40
│       └── X_Label           ← TMP_Text "✕", white
```

**Step 3: Build TabBar**

```
├── TabBar                    ← HorizontalLayoutGroup, height=44
│   ├── Tab_Seeds_Button      ← Button, Image #69C34D (active)
│   │   └── TabSeeds_Label    ← TMP_Text "Hạt Giống", Dosis-Bold 18pt
│   └── Tab_Special_Button    ← Button, Image #CCCCCC (disabled)
│       └── TabSpecial_Label  ← TMP_Text "Đặc Biệt", Dosis-Bold 18pt
```

**Step 4: Build ScrollView**

```
├── ScrollView                ← ScrollRect component
│   └── Viewport              ← Image (mask), RectMask2D
│       └── Content           ← GridLayoutGroup
│                               cellSize=(180,220), spacing=(12,12)
│                               constraint=FixedColumnCount, columns=4
```

**Step 5: Build Footer**

```
└── Footer                    ← HorizontalLayoutGroup, height=50
    ├── GoldChip              ← Image #B97A4A, HorizontalLayoutGroup
    │   ├── GoldIcon_Icon     ← Image (coin), width=24
    │   └── GoldBalance_Label ← TMP_Text "0", Dosis-Bold 20pt, #FFD75E
    └── (space)
```

**Step 6: Add ShopPanelController**

Thêm `ShopPanelController` vào `ShopPopup`.

**Step 7: Wire references**

- `_goldBalanceLabel` → `Footer/GoldChip/GoldBalance_Label`
- `_shopContentContainer` → `ScrollView/Viewport/Content`
- `_btnClose` → `Header/Close_Button`
- `_tabSeeds` → `TabBar/Tab_Seeds_Button`
- `_tabSpecial` → `TabBar/Tab_Special_Button`
- `_shopItemPrefab` → ShopEntry_Seed prefab (từ `Assets/_Project/Prefabs/UI/Components/`)

**Step 8: Extract prefab**

Drag `ShopPopup` từ scene vào `Assets/_Project/Resources/UI/Default/ShopPopup.prefab` (override existing).

**Step 9: Save + Screenshot**

Bật `ShopPopup.SetActive(true)` tạm thời → screenshot → verify layout → tắt lại.

---

## Task 7: Build StoragePopup

**Files:**
- Modify: Scene `SCN_Main.unity`
- Script: `Assets/_Project/Scripts/UI/Panels/StoragePanelController.cs` (giữ nguyên)
- Script: `Assets/_Project/Scripts/UI/Panels/InventorySlotController.cs` (giữ nguyên)

**Step 1: Tạo StoragePopup root**

Trong `[POPUP_CANVAS]/ModalParent`, tạo `StoragePopup`:
- Component: `Image` (background `#FFF7E8`)
- RectTransform: anchor center, width=900, height=650
- **SetActive: false**

**Step 2: Build Header với Capacity**

```
StoragePopup
├── Header                     ← Image #6B3A1F, height=60, HorizontalLayoutGroup
│   ├── StorageIcon_Icon       ← Image, width=36
│   ├── Title_Label            ← TMP_Text "KHO ĐỒ", Dosis-Bold 24pt, white
│   ├── Capacity_Label         ← TMP_Text "0/50", Dosis-Bold 20pt, #FFD75E
│   └── Close_Button           ← Button, Image #FF6B5E, width=40
│       └── X_Label            ← TMP_Text "✕"
```

**Step 3: Build TabBar**

```
├── TabBar                     ← HorizontalLayoutGroup, height=44
│   ├── Tab_All_Button         ← Button (default active, #69C34D)
│   │   └── TMP_Text "Tất Cả"
│   ├── Tab_Crops_Button       ← Button (#FFF7E8)
│   │   └── TMP_Text "Nông Sản"
│   └── Tab_Animals_Button     ← Button (#FFF7E8)
│       └── TMP_Text "Vật Nuôi"
```

**Step 4: Build ScrollView + SellSubPanel**

```
├── ScrollView                 ← ScrollRect
│   └── Viewport → Content    ← GridLayoutGroup, cellSize=(160,200), columns=5
│
└── SellSubPanel               ← Image #FFF7E8, VerticalLayoutGroup, SetActive=false
    ├── SelectedItem_Label     ← TMP_Text "Item Name", Dosis-Bold 20pt
    ├── Stepper                ← HorizontalLayoutGroup
    │   ├── Minus_Button       ← Button "−"
    │   ├── Quantity_Label     ← TMP_Text "1"
    │   └── Plus_Button        ← Button "+"
    ├── TotalPrice_Label       ← TMP_Text "0g", #FFD75E
    └── SellNow_Button         ← Button #FFA94D, "Bán Ngay"
```

**Step 5: Build Footer**

```
└── Footer                     ← HorizontalLayoutGroup, height=50
    ├── SellAll_Button         ← Button #FFA94D, "Bán Tất Cả"
    └── Upgrade_Button         ← Button #69C34D, "Nâng Cấp Kho"
        └── UpgradeCost_Label  ← TMP_Text "500g"
```

**Step 6: Add StoragePanelController + Wire**

- `_capacityText` → `Header/Capacity_Label`
- `_storageContentContainer` → `ScrollView/Viewport/Content`
- `_btnClose` → `Header/Close_Button`
- `_tabAll` → `TabBar/Tab_All_Button`
- `_tabCrops` → `TabBar/Tab_Crops_Button`
- `_tabAnimals` → `TabBar/Tab_Animals_Button`
- `_sellSubPanel` → `SellSubPanel`
- `_selectedItemNameText` → `SellSubPanel/SelectedItem_Label`
- `_quantityText` → `SellSubPanel/Stepper/Quantity_Label`
- `_totalPriceText` → `SellSubPanel/TotalPrice_Label`
- `_btnPlus` → `SellSubPanel/Stepper/Plus_Button`
- `_btnMinus` → `SellSubPanel/Stepper/Minus_Button`
- `_btnSellNow` → `SellSubPanel/SellNow_Button`
- `_btnSellAll` → `Footer/SellAll_Button`
- `_btnUpgrade` → `Footer/Upgrade_Button`
- `_upgradeCostText` → `Footer/Upgrade_Button/UpgradeCost_Label`
- `_itemCardPrefab` → InventorySlot prefab

**Step 7: Extract prefab → `Resources/UI/Default/StoragePopup.prefab`**

**Step 8: Save + Screenshot verify**

---

## Task 8: Build AnimalDetailPanel

**Files:**
- Modify: Scene `SCN_Main.unity`
- Script: `Assets/_Project/Scripts/UI/Panels/AnimalDetailPanelController.cs` (giữ nguyên)

**Step 1: Tạo AnimalDetailPanel**

Trong `[POPUP_CANVAS]/ModalParent`, tạo `AnimalDetailPanel`:
- Component: `Image` (background `#FFF7E8`)
- RectTransform: anchor right, width=400, height=full stretch
- **SetActive: false**

**Step 2: Build layout**

```
AnimalDetailPanel
├── Header                      ← Image #4FA63A, height=80, VerticalLayoutGroup
│   ├── AnimalName_Label        ← TMP_Text "Gà", Dosis-Bold 24pt, white
│   └── AnimalStage_Label       ← TMP_Text "Giai đoạn 1", Dosis-Bold 16pt, white
│
├── StatusBlock                 ← VerticalLayoutGroup, padding=16
│   ├── HungerStatus_Label      ← TMP_Text "Đang đói!", #FFB547, Dosis-Bold 18pt
│   ├── GrowthTimer_Label       ← TMP_Text "Còn 2h 30m", #B97A4A
│   └── LifeRemaining_Label     ← TMP_Text "Tuổi thọ: 12h", #B97A4A
│
├── ValueBlock                  ← Image #FFF7E8 card, VerticalLayoutGroup
│   ├── CurrentValue_Label      ← TMP_Text "Giá hiện tại: 80g", #FFD75E
│   └── NextValue_Label         ← TMP_Text "Giai đoạn sau: 150g", #69C34D
│
└── ActionFooter                ← HorizontalLayoutGroup, height=60
    ├── Feed_Button             ← Button #69C34D, "Cho Ăn"
    ├── Sell_Button             ← Button #FFA94D, "Bán Ngay"
    └── FoodShop_Button         ← Button #67DCC8, "Mua Thức Ăn"
```

**Step 3: Add AnimalDetailPanelController + Wire**

Wire tất cả references theo field names trong script.

**Step 4: Extract prefab → `Resources/UI/Default/AnimalDetailPopup.prefab`**

**Step 5: Save + Screenshot**

---

## Task 9: Integration Smoke Test

**Step 1: Enter Play Mode**

Dùng `sim_play` để vào Play Mode.

**Step 2: Check console**

Dùng `editor_read_log` — filter Error. Không được có:
- NullReferenceException
- Missing Component
- Failed to load UI prefab

**Step 3: Test BottomNav**

Verify mỗi nav button gọi đúng `PopupManager.ShowScreen()`.

**Step 4: Test ContextActionPanel**

Tap một crop tile → verify panel hiện đúng buttons theo state.

**Step 5: Exit Play Mode**

Dùng `sim_stop`.

**Step 6: Final save**

`editor_save_scene`.

---

## Task 10: Update HANDOVER.md

**Files:**
- Modify: `docs/HANDOVER.md`

Cập nhật section "Phiên 15/04/2026" với:
- SCN_Main đã được tạo với 4 canvas
- 7 UI components đã build và wire
- Prefabs đã extract vào `Resources/UI/Default/`
- SCN_Gameplay giữ nguyên để tham khảo
