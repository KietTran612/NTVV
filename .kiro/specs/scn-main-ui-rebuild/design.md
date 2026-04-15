# Design Document — SCN_Main UI Rebuild

## Overview

Rebuild toàn bộ UI cho NTVV trong scene mới `SCN_Main` theo approach **Hybrid + Pure MCP**: giữ scripts logic, build hierarchy trực tiếp trong Unity Editor qua MCP tools, tách prefab sau. Không viết code mới.

---

## Architecture

### Scene Hierarchy

```
SCN_Main
├── [WORLD_ROOT]
│   ├── Main Camera          (Camera, AudioListener)
│   ├── CropArea             (empty parent cho crop tiles)
│   └── BarnArea             (empty parent cho animal pens)
│
├── [SYSTEMS]                ← Tất cả Singleton GameObjects
│   ├── GameManager          (GameManager + _dataRegistry wired)
│   ├── EconomySystem        (EconomySystem)
│   ├── StorageSystem        (StorageSystem)
│   ├── LevelSystem          (LevelSystem)
│   ├── SaveLoadManager      (SaveLoadManager)
│   └── PopupManager         (PopupManager + canvas refs wired)
│
├── [HUD_CANVAS]             (Canvas SO=10, CanvasScaler 1920×1080)
│   ├── TopHUDBar            (HUDTopBarController)
│   └── BottomNav            (BottomNavController)
│
├── [POPUP_CANVAS]           (Canvas SO=20, CanvasScaler 1920×1080)
│   ├── ModalParent          (RectTransform stretch-stretch)
│   ├── HUDParent            (RectTransform stretch-stretch)
│   │   └── ContextActionPanel  (CropActionPanelController, disabled)
│   └── DimOverlay           (Image black alpha=0.5, disabled)
│
└── [SYSTEM_CANVAS]          (Canvas SO=30, CanvasScaler 1920×1080)
    ├── LoadingOverlay       (disabled)
    └── SaveIndicator        (disabled)
```

### Popup Loading Flow

```
User tap BottomNav button
  → BottomNavController.OnNavClick("Shop")
  → PopupManager.ShowScreen("Shop")
  → ResourcesUIProvider.LoadPrefab("ShopPopup")
  → Resources.Load("UI/Default/ShopPopup")
  → Instantiate vào ModalParent
  → ShopPanelController.OnEnable() → bind data
```

### ContextAction Flow

```
User tap CropTile (world object)
  → CropTileView.OnPointerClick()
  → PopupManager.ShowContextAction(this)
  → CropActionPanelController.Setup(target)
  → RefreshUI() → hiện đúng buttons theo state
  → PositionContextPanel(worldPos)
```

---

## Component Designs

### TopHUDBar

```
TopHUDBar [Image #FFF7E8, HorizontalLayoutGroup padding=16 spacing=12]
  anchor: top-stretch, height=80
├── Avatar_Icon          [Image, 48×48]
├── LevelChip            [Image #4FA63A, HorizontalLayoutGroup]
│   ├── Level_Label      [TMP Dosis-Bold 20pt white]
│   └── XP_Fill          [Image #8ED8FF, fillMethod=Horizontal]
├── GoldChip             [Image #FFF7E8, HorizontalLayoutGroup gap=6]
│   ├── Gold_Icon        [Image icon_Gold_Atomic, 28×28]
│   └── Gold_Label       [TMP Dosis-Bold 24pt #FFD75E]
├── StorageChip          [Image #FFF7E8, HorizontalLayoutGroup gap=6]
│   ├── Storage_Icon     [Image icon_Storage_Atomic, 28×28]
│   └── Storage_Label    [TMP Dosis-Bold 20pt #B97A4A]
└── Settings_Button      [Button, Image gear, 44×44]
```

### BottomNav

```
BottomNav [Image #FFF7E8, HorizontalLayoutGroup childForceExpand=true]
  anchor: bottom-stretch, height=80
├── NavBtn_Farm          [Button + Image, VerticalLayoutGroup]
│   ├── NavIcon_Farm     [Image 32×32]
│   └── NavLabel_Farm    [TMP "Farm" Dosis-Bold 16pt]
├── NavBtn_Storage       [Button + Image]
│   ├── NavIcon_Storage  [Image icon_Storage_Atomic 32×32]
│   └── NavLabel_Storage [TMP "Kho" Dosis-Bold 16pt]
├── NavBtn_Shop          [Button + Image]
│   ├── NavIcon_Shop     [Image icon_Sprout_Header_Atomic 32×32]
│   └── NavLabel_Shop    [TMP "Shop" Dosis-Bold 16pt]
├── NavBtn_Barn          [Button + Image]
│   ├── NavIcon_Barn     [Image 32×32]
│   └── NavLabel_Barn    [TMP "Chuồng" Dosis-Bold 16pt]
└── NavBtn_Event         [Button + Image]
    ├── NavIcon_Event    [Image 32×32]
    └── NavLabel_Event   [TMP "Event" Dosis-Bold 16pt]
```

### ContextActionPanel

```
ContextActionPanel [Image #FFF7E8 rounded, VerticalLayoutGroup padding=12 spacing=8]
  width=280, height=auto (ContentSizeFitter), disabled by default
├── Header               [HorizontalLayoutGroup]
│   ├── CropIcon_Icon    [Image 40×40]
│   └── Header_Label     [TMP Dosis-Bold 22pt #4FA63A]
├── Plant_Button         [Button Image #69C34D, TMP "Gieo Hạt" white 20pt, h=44]
├── Harvest_Button       [Button Image #69C34D, TMP "Thu Hoạch" white 20pt, h=44]
├── Reset_Button         [Button Image #B97A4A, TMP "Dọn Đất" white 20pt, h=44]
├── Water_Button         [Button Image #67DCC8, TMP "Tưới Nước" white 20pt, h=44]
├── Cure_Button          [Button Image #FFB547, TMP "Bắt Sâu" white 20pt, h=44]
├── Weed_Button          [Button Image #FFB547, TMP "Cắt Cỏ" white 20pt, h=44]
├── Buy_Button           [Button Image #69C34D, TMP "Mua Giống" white 20pt, h=44]
├── Feed_Button          [Button Image #69C34D, TMP "Cho Ăn" white 20pt, h=44]
├── Sell_Button          [Button Image #FFA94D, TMP "Bán Ngay" white 20pt, h=44]
├── Collect_Button       [Button Image #69C34D, TMP "Thu Sản Phẩm" white 20pt, h=44]
└── Close_Button         [Button Image #B97A4A, TMP "✕" white, 44×44]
```

### ShopPopup

```
ShopPopup [Image #FFF7E8, VerticalLayoutGroup]
  anchor=center, width=800, height=600, disabled by default
├── Header               [Image bg_Plaque_Wooden_Atomic, HorizontalLayoutGroup h=60]
│   ├── ShopIcon_Icon    [Image icon_Sprout_Header_Atomic 36×36]
│   ├── Title_Label      [TMP "SEED SHOP" Dosis-Bold 24pt white, flex=1]
│   └── Close_Button     [Image btn_Close_Circle_Atomic 40×40]
├── TabBar               [HorizontalLayoutGroup h=44]
│   ├── Tab_Seeds_Button [Button Image #69C34D, TMP "Hạt Giống" Dosis-Bold 18pt white]
│   │   └── TabSeeds_Icon [Image icon_Tab_Leaf_Atomic 24×24]
│   └── Tab_Special_Button [Button Image #AAAAAA interactable=false]
│       └── TabSpecial_Icon [Image icon_Tab_Star_Atomic 24×24]
├── ScrollView           [ScrollRect, flex=1]
│   └── Viewport         [Image mask, RectMask2D]
│       └── Content      [GridLayoutGroup cellSize=180×220 spacing=12×12 columns=4]
└── Footer               [Image #F5EDD8, HorizontalLayoutGroup h=50 padding=12]
    ├── GoldChip         [Image #B97A4A, HorizontalLayoutGroup gap=6]
    │   ├── GoldIcon_Icon [Image icon_Gold_Atomic 24×24]
    │   └── GoldBalance_Label [TMP Dosis-Bold 20pt #FFD75E]
    └── (flexible space)
```

### StoragePopup

```
StoragePopup [Image #FFF7E8, VerticalLayoutGroup]
  anchor=center, width=900, height=650, disabled by default
├── Header               [Image bg_Plaque_Wooden_Atomic, HorizontalLayoutGroup h=60]
│   ├── StorageIcon_Icon [Image icon_Storage_Atomic 36×36]
│   ├── Title_Label      [TMP "KHO ĐỒ" Dosis-Bold 24pt white, flex=1]
│   ├── Capacity_Label   [TMP "0/50" Dosis-Bold 20pt #FFD75E]
│   └── Close_Button     [Image btn_Close_Circle_Atomic 40×40]
├── TabBar               [HorizontalLayoutGroup h=44]
│   ├── Tab_All_Button   [Button Image #69C34D active, TMP "Tất Cả"]
│   ├── Tab_Crops_Button [Button Image #FFF7E8, TMP "Nông Sản"]
│   └── Tab_Animals_Button [Button Image #FFF7E8, TMP "Vật Nuôi"]
├── MainArea             [HorizontalLayoutGroup flex=1]
│   ├── ScrollView       [ScrollRect, flex=1]
│   │   └── Viewport → Content [GridLayoutGroup cellSize=160×200 spacing=10×10 columns=5]
│   └── SellSubPanel     [Image #F5EDD8, VerticalLayoutGroup w=220, disabled]
│       ├── SelectedItem_Label [TMP Dosis-Bold 20pt #4FA63A]
│       ├── Stepper      [HorizontalLayoutGroup]
│       │   ├── Minus_Button [Button "−" 40×40]
│       │   ├── Quantity_Label [TMP "1" Dosis-Bold 22pt center]
│       │   └── Plus_Button  [Button "+" 40×40]
│       ├── TotalPrice_Label [TMP "0g" Dosis-Bold 24pt #FFD75E]
│       └── SellNow_Button [Button Image #FFA94D, TMP "Bán Ngay" white 20pt, h=48]
└── Footer               [HorizontalLayoutGroup h=50 padding=12]
    ├── SellAll_Button   [Button Image #FFA94D, TMP "Bán Tất Cả" white 18pt]
    └── Upgrade_Button   [Button Image #69C34D, VerticalLayoutGroup]
        ├── TMP "Nâng Cấp Kho" white 16pt
        └── UpgradeCost_Label [TMP "500g" #FFD75E 14pt]
```

### AnimalDetailPanel

> ⚠️ Wire theo đúng field names trong `AnimalDetailPanelController.cs`:
> `_animalName` (TMP_Text), `_growthText` (TMP_Text), `_feedButton` (GameObject), `_sellButton` (GameObject)

```
AnimalDetailPanel [Image #FFF7E8, VerticalLayoutGroup]
  anchor=right-stretch, width=400, disabled by default
├── Header               [Image #4FA63A, VerticalLayoutGroup padding=16 h=80]
│   └── AnimalName       [TMP Dosis-Bold 26pt white]   ← wire → _animalName
├── StatusBlock          [VerticalLayoutGroup padding=16 spacing=8]
│   └── GrowthInfo       [TMP Dosis-Bold 16pt #B97A4A]  ← wire → _growthText
└── ActionFooter         [VerticalLayoutGroup padding=16 spacing=8]
    ├── FeedButtonGO     [GameObject → Image #69C34D, Button child]  ← wire → _feedButton
    │   └── Feed_Button  [Button, TMP "Cho Ăn" white 20pt h=52]
    └── SellButtonGO     [GameObject → Image #FFA94D, Button child]  ← wire → _sellButton
        └── Sell_Button  [Button, TMP "Bán Ngay" white 20pt h=52]
```

> Note: `OnFeedClick()` và `OnSellClick()` được gọi trực tiếp từ Button.onClick — không wire qua script field.

---

## Script Wire Map

| Script | GameObject | Key Fields to Wire |
|--------|-----------|-------------------|
| HUDTopBarController | TopHUDBar | `_gold_Label`, `_level_Label`, `_storage_Label`, `_xp_Fill` |
| BottomNavController | BottomNav | `_btnFarm`, `_btnStorage`, `_btnShop`, `_btnBarn`, `_btnEvent` |
| PopupManager | PopupManager (root) | `_mainOverlayCanvas`, `_modalParent`, `_hudParent` |
| CropActionPanelController | ContextActionPanel | tất cả buttons + `_headerText` + `_registry` |
| ShopPanelController | ShopPopup | `_goldBalanceLabel`, `_gemsBalanceLabel`, `_shopContentContainer`, `_btnClose`, `_btnRefresh`, `_tabSeeds`, `_tabSpecial`, `_shopItemPrefab` |
| StoragePanelController | StoragePopup | `_capacityText`, `_storageContentContainer`, `_btnClose`, tabs, sell sub-panel fields, footer buttons, `_itemCardPrefab` |
| AnimalDetailPanelController | AnimalDetailPanel | `_animalName` (TMP), `_growthText` (TMP), `_feedButton` (GameObject), `_sellButton` (GameObject) |

---

## Prefab Extraction Map

| Prefab | Source (scene path) | Destination |
|--------|-------------------|-------------|
| ShopPopup.prefab | [POPUP_CANVAS]/ModalParent/ShopPopup | Resources/UI/Default/ShopPopup.prefab |
| StoragePopup.prefab | [POPUP_CANVAS]/ModalParent/StoragePopup | Resources/UI/Default/StoragePopup.prefab |
| AnimalDetailPopup.prefab | [POPUP_CANVAS]/ModalParent/AnimalDetailPanel | Resources/UI/Default/AnimalDetailPopup.prefab |
| ContextActionPanel.prefab | [POPUP_CANVAS]/HUDParent/ContextActionPanel | Resources/UI/Default/ContextActionPanel.prefab |

Sau khi extract prefab: xóa instance trong scene, để PopupManager spawn runtime.

---

## Design References

- `Design/Farm game HUD with cheerful avatar.png` — visual reference cho TopHUDBar
- `Design/Seed shop menu in farming game.png` — visual reference cho ShopPopup
- `docs/document_md/farm_game_ui_style_guide_v2.md` — color palette source of truth
- `docs/document_md/farm_game_ui_component_spec_unity_handoff_v2.md` — component specs
- `docs/document_md/farm_game_wireframe_color_mapping_v2.md` — color mapping per screen
