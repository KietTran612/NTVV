# Implementation Plan: scn-main-ui-rebuild

## Overview

Build toàn bộ UI cho SCN_Main qua Pure MCP. Giữ scripts logic, build hierarchy trong Unity Editor, wire references, extract prefabs. Không viết code mới.

**Design doc:** `.kiro/specs/scn-main-ui-rebuild/design.md`
**Requirements:** `.kiro/specs/scn-main-ui-rebuild/requirements.md`

> ⚠️ ĐỌC GLOBAL RULES trong requirements.md TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO.

---

## Tasks

- [ ] 1. Tạo SCN_Main, 4 Canvas và Systems
  - Dùng `scene_new` tạo scene `SCN_Main` tại `Assets/_Project/Scenes/SCN_Main.unity`
  - Tạo root GameObject `[WORLD_ROOT]` (empty)
  - Tạo `Main Camera` trong `[WORLD_ROOT]` với Camera + AudioListener
  - Tạo `CropArea` và `BarnArea` (empty) trong `[WORLD_ROOT]`
  - Tạo root GameObject `[SYSTEMS]` (empty) — chứa tất cả singletons
  - Trong `[SYSTEMS]`, tạo và add components:
    - `GameManager` → add `GameManager` component, wire `_dataRegistry` → `GameDataRegistry.asset`
    - `EconomySystem` → add `EconomySystem` component
    - `StorageSystem` → add `StorageSystem` component
    - `LevelSystem` → add `LevelSystem` component
    - `SaveLoadManager` → add `SaveLoadManager` component
    - `PopupManager` → add `PopupManager` component
  - Tạo 3 canvas root GameObjects: `[HUD_CANVAS]`, `[POPUP_CANVAS]`, `[SYSTEM_CANVAS]`
  - Add `Canvas` + `CanvasScaler` + `GraphicRaycaster` vào mỗi canvas object
  - Set Canvas Render Mode = Screen Space Overlay cho cả 3 canvas
  - Set Sort Order: HUD_CANVAS=10, POPUP_CANVAS=20, SYSTEM_CANVAS=30
  - Set CanvasScaler: UI Scale Mode=Scale With Screen Size, Reference Resolution=1920×1080, Match=0.5
  - Trong `[POPUP_CANVAS]`: tạo `ModalParent` (RectTransform stretch-stretch), `HUDParent` (RectTransform stretch-stretch), `DimOverlay` (Image color=black alpha=0.5, SetActive=false)
  - Trong `[SYSTEM_CANVAS]`: tạo `LoadingOverlay` và `SaveIndicator` (cả 2 SetActive=false)
  - Dùng `editor_save_scene` lưu scene
  - Verify: `screenshot_scene` — thấy đủ root objects trong hierarchy
  - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 1.6_

- [ ] 2. Build TopHUDBar
  - Trong `[HUD_CANVAS]`, tạo `TopHUDBar` GameObject
  - Add `Image` component, color=#FFF7E8
  - Add `HorizontalLayoutGroup`: padding left=16 right=16 top=8 bottom=8, spacing=12, childAlignment=MiddleLeft, childControlHeight=true, childForceExpandHeight=true
  - Set RectTransform: anchorMin=(0,1) anchorMax=(1,1) pivot=(0.5,1), height=80, offsetMin.x=0, offsetMax.x=0, pos Y=0
  - Tạo child `Avatar_Icon`: Image, width=48 height=48, raycastTarget=false
  - Tạo child `LevelChip`: Image color=#4FA63A, HorizontalLayoutGroup spacing=6 padding=6, ContentSizeFitter
    - Child `Level_Label`: TMP_Text "Level 1", Dosis-Bold 20pt, color=white
    - Child `XP_Fill`: Image color=#8ED8FF, fillMethod=Horizontal, fillAmount=0.3, raycastTarget=false, width=80 height=12
  - Tạo child `GoldChip`: Image color=#FFF7E8, HorizontalLayoutGroup spacing=6 padding=4, ContentSizeFitter
    - Child `Gold_Icon`: Image sprite=icon_Gold_Atomic, width=28 height=28, raycastTarget=false
    - Child `Gold_Label`: TMP_Text "0", Dosis-Bold 24pt, color=#FFD75E
  - Tạo child `StorageChip`: Image color=#FFF7E8, HorizontalLayoutGroup spacing=6 padding=4, ContentSizeFitter
    - Child `Storage_Icon`: Image sprite=icon_Storage_Atomic, width=28 height=28, raycastTarget=false
    - Child `Storage_Label`: TMP_Text "0/50", Dosis-Bold 20pt, color=#B97A4A
  - Tạo child `Settings_Button`: Button + Image, width=44 height=44
  - Add `HUDTopBarController` vào `TopHUDBar`
  - Wire via `component_set`: `_gold_Label`→GoldChip/Gold_Label, `_level_Label`→LevelChip/Level_Label, `_storage_Label`→StorageChip/Storage_Label, `_xp_Fill`→LevelChip/XP_Fill
  - `editor_save_scene` → `screenshot_scene` → verify top band đúng
  - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.5, 2.6, 2.8, 2.9_

- [ ] 3. Build BottomNav
  - Trong `[HUD_CANVAS]`, tạo `BottomNav` GameObject
  - Add `Image` component, color=#FFF7E8
  - Add `HorizontalLayoutGroup`: childForceExpandWidth=true, childForceExpandHeight=true, spacing=0
  - Set RectTransform: anchorMin=(0,0) anchorMax=(1,0) pivot=(0.5,0), height=80, pos Y=0
  - Tạo 5 nav buttons theo cấu trúc sau (lặp lại cho Farm, Storage, Shop, Barn, Event):
    ```
    NavBtn_Farm [Button + Image color=#FFF7E8, VerticalLayoutGroup childAlignment=MiddleCenter spacing=4]
    ├── NavIcon_Farm  [Image sprite=tương ứng, width=32 height=32, raycastTarget=false]
    └── NavLabel_Farm [TMP_Text "Farm", Dosis-Bold 16pt, color=#B97A4A]
    ```
  - Sprites cho icons: Farm=icon_Sprout_Header_Atomic, Storage=icon_Storage_Atomic, Shop=icon_Sprout_Header_Atomic, Barn=icon_Storage_Atomic, Event=icon_Tab_Star_Atomic (dùng tạm, thay sau)
  - Add `BottomNavController` vào `BottomNav`
  - Wire: `_btnFarm`→NavBtn_Farm, `_btnStorage`→NavBtn_Storage, `_btnShop`→NavBtn_Shop, `_btnBarn`→NavBtn_Barn, `_btnEvent`→NavBtn_Event
  - `editor_save_scene` → `screenshot_scene` → verify bottom band 5 buttons
  - _Requirements: 3.1, 3.2, 3.3, 3.4, 3.5, 3.6, 3.7_

- [ ] 4. Wire PopupManager
  - Tìm `PopupManager` GameObject trong `[SYSTEMS]`
  - Dùng `component_set` trên `PopupManager` component:
    - `_mainOverlayCanvas` → `[POPUP_CANVAS]` (Canvas component)
    - `_modalParent` → `[POPUP_CANVAS]/ModalParent` (RectTransform)
    - `_hudParent` → `[POPUP_CANVAS]/HUDParent` (RectTransform)
  - `editor_save_scene`
  - Enter Play Mode (`sim_play`) → `editor_read_log` filter Error → không có NullRef từ PopupManager
  - Exit Play Mode (`sim_stop`)
  - _Requirements: 1.5, 1.6_

- [ ] 5. Build ContextActionPanel
  - Trong `[POPUP_CANVAS]/HUDParent`, tạo `ContextActionPanel` GameObject
  - Add `Image` color=#FFF7E8
  - Add `VerticalLayoutGroup`: padding=12, spacing=8, childForceExpandWidth=true, childControlHeight=false
  - Add `ContentSizeFitter`: verticalFit=PreferredSize
  - Set RectTransform: width=280, pivot=(0.5,0) — height sẽ auto
  - SetActive=false
  - Tạo child `Header` [HorizontalLayoutGroup spacing=8]:
    - `CropIcon_Icon` [Image 40×40 raycastTarget=false]
    - `Header_Label` [TMP_Text "Mảnh Đất" Dosis-Bold 22pt #4FA63A]
  - Tạo các action buttons (mỗi button: Image + TMP_Text child, height=44, width=fill):
    - `Plant_Button` [Image #69C34D] → child TMP "Gieo Hạt" white Dosis-Bold 20pt
    - `Harvest_Button` [Image #69C34D] → child TMP "Thu Hoạch" white Dosis-Bold 20pt
    - `Reset_Button` [Image #B97A4A] → child TMP "Dọn Đất" white Dosis-Bold 20pt
    - `Water_Button` [Image #67DCC8] → child TMP "Tưới Nước" white Dosis-Bold 20pt
    - `Cure_Button` [Image #FFB547] → child TMP "Bắt Sâu" white Dosis-Bold 20pt
    - `Weed_Button` [Image #FFB547] → child TMP "Cắt Cỏ" white Dosis-Bold 20pt
    - `Buy_Button` [Image #69C34D] → child TMP "Mua Giống" white Dosis-Bold 20pt
    - `Feed_Button` [Image #69C34D] → child TMP "Cho Ăn" white Dosis-Bold 20pt
    - `Sell_Button` [Image #FFA94D] → child TMP "Bán Ngay" white Dosis-Bold 20pt
    - `Collect_Button` [Image #69C34D] → child TMP "Thu Sản Phẩm" white Dosis-Bold 20pt
    - `Close_Button` [Image #B97A4A, width=44 height=44] → child TMP "✕" white Dosis-Bold 20pt
  - Add `CropActionPanelController` vào `ContextActionPanel`
  - Wire tất cả button references và `_headerText`→Header/Header_Label
  - Wire `_registry` → tìm `GameDataRegistrySO` asset trong project và assign
  - `editor_save_scene` → `screenshot_scene` (bật SetActive=true tạm để chụp, tắt lại sau)
  - _Requirements: 4.1, 4.2, 4.3, 4.4, 4.5, 4.6, 4.7, 4.8, 4.9, 4.10_

- [ ] 6. Build ShopPopup
  - Trong `[POPUP_CANVAS]/ModalParent`, tạo `ShopPopup` GameObject
  - Add `Image` color=#FFF7E8
  - Add `VerticalLayoutGroup`: spacing=0, childForceExpandWidth=true
  - Set RectTransform: anchorMin=(0.5,0.5) anchorMax=(0.5,0.5) pivot=(0.5,0.5), width=800 height=600
  - SetActive=false
  - Tạo child `Header` [Image sprite=bg_Plaque_Wooden_Atomic, HorizontalLayoutGroup padding=12 spacing=8, height=60]:
    - `ShopIcon_Icon` [Image sprite=icon_Sprout_Header_Atomic, 36×36, raycastTarget=false]
    - `Title_Label` [TMP "SEED SHOP" Dosis-Bold 24pt white, LayoutElement flexibleWidth=1]
    - `Close_Button` [Button, Image sprite=btn_Close_Circle_Atomic, 40×40]
  - Tạo child `TabBar` [HorizontalLayoutGroup spacing=4 padding=8, height=44]:
    - `Tab_Seeds_Button` [Button, Image #69C34D, LayoutElement flexibleWidth=1]:
      - Child `TabSeeds_Icon` [Image sprite=icon_Tab_Leaf_Atomic, 24×24]
      - Child TMP "Hạt Giống" Dosis-Bold 18pt white
    - `Tab_Special_Button` [Button, Image #AAAAAA, interactable=false, LayoutElement flexibleWidth=1]:
      - Child `TabSpecial_Icon` [Image sprite=icon_Tab_Star_Atomic, 24×24]
      - Child TMP "Đặc Biệt" Dosis-Bold 18pt #888888
  - Tạo child `ScrollView` [ScrollRect, LayoutElement flexibleHeight=1]:
    - Child `Viewport` [Image, RectMask2D, anchor stretch-stretch]:
      - Child `Content` [GridLayoutGroup: cellSize=180×220, spacing=12×12, constraint=FixedColumnCount, constraintCount=4, padding=12]
  - Tạo child `Footer` [Image #F5EDD8, HorizontalLayoutGroup padding=12 spacing=8, height=50]:
    - `GoldChip` [Image #B97A4A, HorizontalLayoutGroup padding=8 spacing=6, ContentSizeFitter]:
      - `GoldIcon_Icon` [Image sprite=icon_Gold_Atomic, 24×24, raycastTarget=false]
      - `GoldBalance_Label` [TMP "0" Dosis-Bold 20pt #FFD75E]
  - Add `ShopPanelController` vào `ShopPopup`
  - Wire: `_goldBalanceLabel`→Footer/GoldChip/GoldBalance_Label, `_gemsBalanceLabel`→Footer/GemsChip/GemsBalance_Label (nếu có), `_shopContentContainer`→ScrollView/Viewport/Content, `_btnClose`→Header/Close_Button, `_btnRefresh`→Footer/Refresh_Button (nếu có), `_tabSeeds`→TabBar/Tab_Seeds_Button, `_tabSpecial`→TabBar/Tab_Special_Button
  - Wire `_shopItemPrefab` → `Assets/_Project/Prefabs/UI/Components/ShopEntry_Seed.prefab`
  - Bật SetActive=true → `screenshot_scene` → so sánh với `Design/Seed shop menu in farming game.png`
  - Tắt SetActive=false
  - `editor_save_scene`
  - _Requirements: 5.1, 5.2, 5.3, 5.4, 5.5, 5.6, 5.7, 5.8, 5.9, 5.10_

- [ ] 7. Build StoragePopup
  - Trong `[POPUP_CANVAS]/ModalParent`, tạo `StoragePopup` GameObject
  - Add `Image` color=#FFF7E8
  - Add `VerticalLayoutGroup`: spacing=0, childForceExpandWidth=true
  - Set RectTransform: anchor center, width=900 height=650
  - SetActive=false
  - Tạo child `Header` [Image sprite=bg_Plaque_Wooden_Atomic, HorizontalLayoutGroup padding=12 spacing=8, height=60]:
    - `StorageIcon_Icon` [Image sprite=icon_Storage_Atomic, 36×36]
    - `Title_Label` [TMP "KHO ĐỒ" Dosis-Bold 24pt white, flexibleWidth=1]
    - `Capacity_Label` [TMP "0/50" Dosis-Bold 20pt #FFD75E]
    - `Close_Button` [Button, Image sprite=btn_Close_Circle_Atomic, 40×40]
  - Tạo child `TabBar` [HorizontalLayoutGroup spacing=4 padding=8, height=44]:
    - `Tab_All_Button` [Button Image #69C34D, TMP "Tất Cả" Dosis-Bold 18pt white, flexibleWidth=1]
    - `Tab_Crops_Button` [Button Image #FFF7E8, TMP "Nông Sản" Dosis-Bold 18pt #B97A4A, flexibleWidth=1]
    - `Tab_Animals_Button` [Button Image #FFF7E8, TMP "Vật Nuôi" Dosis-Bold 18pt #B97A4A, flexibleWidth=1]
  - Tạo child `MainArea` [HorizontalLayoutGroup spacing=0, LayoutElement flexibleHeight=1]:
    - `ScrollView` [ScrollRect, LayoutElement flexibleWidth=1]:
      - `Viewport` [Image, RectMask2D]:
        - `Content` [GridLayoutGroup: cellSize=160×200, spacing=10×10, constraint=FixedColumnCount, constraintCount=5, padding=10]
    - `SellSubPanel` [Image #F5EDD8, VerticalLayoutGroup padding=12 spacing=8, width=220, SetActive=false]:
      - `SelectedItem_Label` [TMP "Chọn vật phẩm" Dosis-Bold 20pt #4FA63A]
      - `Stepper` [HorizontalLayoutGroup spacing=8, height=44]:
        - `Minus_Button` [Button Image #B97A4A, TMP "−" white Dosis-Bold 24pt, 40×40]
        - `Quantity_Label` [TMP "1" Dosis-Bold 22pt #4FA63A center, flexibleWidth=1]
        - `Plus_Button` [Button Image #69C34D, TMP "+" white Dosis-Bold 24pt, 40×40]
      - `TotalPrice_Label` [TMP "0g" Dosis-Bold 24pt #FFD75E center]
      - `SellNow_Button` [Button Image #FFA94D, TMP "Bán Ngay" white Dosis-Bold 20pt, height=48]
  - Tạo child `Footer` [Image #F5EDD8, HorizontalLayoutGroup padding=12 spacing=8, height=50]:
    - `SellAll_Button` [Button Image #FFA94D, TMP "Bán Tất Cả" white Dosis-Bold 18pt, flexibleWidth=1, height=40]
    - `Upgrade_Button` [Button Image #69C34D, VerticalLayoutGroup, flexibleWidth=1, height=40]:
      - TMP "Nâng Cấp Kho" white Dosis-Bold 16pt
      - `UpgradeCost_Label` [TMP "500g" #FFD75E Dosis-Bold 14pt]
  - Add `StoragePanelController` vào `StoragePopup`
  - Wire tất cả references theo Script Wire Map trong design.md
  - Wire `_itemCardPrefab` → tìm InventorySlot prefab trong `Assets/_Project/Prefabs/UI/Components/`
  - Bật SetActive=true → `screenshot_scene` → verify layout → tắt lại
  - `editor_save_scene`
  - _Requirements: 6.1, 6.2, 6.3, 6.4, 6.5, 6.6, 6.7, 6.8, 6.9, 6.10_

- [ ] 8. Build AnimalDetailPanel
  - Trong `[POPUP_CANVAS]/ModalParent`, tạo `AnimalDetailPanel` GameObject
  - Add `Image` color=#FFF7E8
  - Add `VerticalLayoutGroup`: spacing=0, childForceExpandWidth=true
  - Set RectTransform: anchorMin=(1,0) anchorMax=(1,1) pivot=(1,0.5), width=400, offsetMin.y=0 offsetMax.y=0
  - SetActive=false
  - Tạo child `Header` [Image #4FA63A, VerticalLayoutGroup padding=16, height=80]:
    - `AnimalName` [TMP_Text "Gà" Dosis-Bold 26pt white] ← field name phải là `AnimalName` (wire → `_animalName`)
  - Tạo child `StatusBlock` [VerticalLayoutGroup padding=16 spacing=8]:
    - `GrowthInfo` [TMP_Text "GĐ: 1" Dosis-Bold 16pt #B97A4A] ← field name phải là `GrowthInfo` (wire → `_growthText`)
  - Tạo child `ActionFooter` [VerticalLayoutGroup padding=16 spacing=8]:
    - `FeedButtonGO` [GameObject, Image #69C34D, height=52, childForceExpandWidth=true]:
      - Child `Feed_Button` [Button, TMP "Cho Ăn" white Dosis-Bold 20pt]
      - Button.onClick → wire `AnimalDetailPanelController.OnFeedClick()`
    - `SellButtonGO` [GameObject, Image #FFA94D, height=52, childForceExpandWidth=true]:
      - Child `Sell_Button` [Button, TMP "Bán Ngay" white Dosis-Bold 20pt]
      - Button.onClick → wire `AnimalDetailPanelController.OnSellClick()`
  - Add `AnimalDetailPanelController` vào `AnimalDetailPanel`
  - Wire via `component_set`:
    - `_animalName` → `Header/AnimalName` (TMP_Text component)
    - `_growthText` → `StatusBlock/GrowthInfo` (TMP_Text component)
    - `_feedButton` → `ActionFooter/FeedButtonGO` (GameObject)
    - `_sellButton` → `ActionFooter/SellButtonGO` (GameObject)
  - Bật SetActive=true → `screenshot_scene` → verify layout → tắt lại
  - `editor_save_scene`
  - _Requirements: 7.1, 7.2, 7.3, 7.4, 7.5, 7.6, 7.7, 7.9, 7.10_

- [ ] 9. Extract Prefabs
  - Dùng `assets_prefab_create` để tạo prefab từ `[POPUP_CANVAS]/ModalParent/ShopPopup` → save tới `Assets/_Project/Resources/UI/Default/ShopPopup.prefab`
  - Dùng `assets_prefab_create` để tạo prefab từ `[POPUP_CANVAS]/ModalParent/StoragePopup` → save tới `Assets/_Project/Resources/UI/Default/StoragePopup.prefab`
  - Dùng `assets_prefab_create` để tạo prefab từ `[POPUP_CANVAS]/ModalParent/AnimalDetailPanel` → save tới `Assets/_Project/Resources/UI/Default/AnimalDetailPopup.prefab`
  - Dùng `assets_prefab_create` để tạo prefab từ `[POPUP_CANVAS]/HUDParent/ContextActionPanel` → save tới `Assets/_Project/Resources/UI/Default/ContextActionPanel.prefab`
  - Sau khi extract: dùng `game_object_destroy` xóa các instances trong scene (PopupManager sẽ spawn runtime)
  - Dùng `editor_refresh_assets` để Unity import tất cả prefabs mới
  - Verify: `assets_find` tìm từng prefab — tất cả 4 prefabs phải tồn tại
  - Verify: `console_get_logs` filter=Error — không có missing script references
  - `editor_save_scene`
  - _Requirements: 8.1, 8.2, 8.3, 8.4, 8.5, 8.6_

- [ ] 10. Integration Smoke Test
  - Enter Play Mode: `sim_play`
  - Đợi scene load: `editor_wait_ready`
  - Check console: `editor_read_log` filter=Error — phải có 0 errors về NullRef, Missing Component, Failed to load prefab
  - Check PopupManager: `component_get` trên PopupManager — `_modalParent` và `_hudParent` không null
  - Test BottomNav Shop button: `component_invoke` trên NavBtn_Shop Button → verify ShopPopup spawn trong ModalParent
  - Test BottomNav Storage button: `component_invoke` trên NavBtn_Storage Button → verify StoragePopup spawn
  - Close popup: verify `PopupManager.CloseActiveModal()` destroy popup
  - Check HUD: `component_get` trên HUDTopBarController — tất cả references không null
  - Exit Play Mode: `sim_stop`
  - `editor_save_scene`
  - _Requirements: 9.1, 9.2, 9.3, 9.4, 9.5, 9.6_

- [ ] 11. Update HANDOVER.md
  - Mở `docs/HANDOVER.md`
  - Cập nhật section "Phiên 15/04/2026" thêm:
    - SCN_Main đã tạo với 4 canvas (HUD=10, Popup=20, System=30)
    - 7 UI components đã build: TopHUDBar, BottomNav, ContextActionPanel, ShopPopup, StoragePopup, AnimalDetailPanel + PopupManager wiring
    - Prefabs extracted vào `Resources/UI/Default/`
    - SCN_Gameplay giữ nguyên để tham khảo
  - Cập nhật section "Cần làm ngay" → đánh dấu "UI Prototype Assembly" là DONE
  - _Requirements: tất cả_
