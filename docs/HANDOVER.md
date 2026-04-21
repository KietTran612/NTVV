# Project Handover - NTVV (Nông Trại Vui Vẻ)

Tài liệu này dùng để đồng bộ nhanh "suy nghĩ" của AI Agent khi bạn chuyển sang máy tính mới hoặc bắt đầu một phiên làm việc mới.

## 🧠 Bối cảnh Phiên làm việc (Session Context)
- **Phiên 21/04/2026 (Hiện tại) — m7a-sprite-reorg HOÀN THÀNH**:
    - **`m7a-sprite-reorg` spec DONE**: Tất cả 11 tasks đã execute xong.
    - **Mass rename ~82 sprite files** sang naming convention `[Domain]_[Category]_[Entity]_[Variant]_[State].png`
    - **Folder structure mới**: `World/` (Crops, Animals, Products, Overlays, Tiles) + `UI/` (Backgrounds, Icons/Common/Seed/Nav/Tab/Header/Action, Buttons)
    - **GUID preserved**: `.meta` files di chuyển cùng → references trong SO/Prefab/Scene không vỡ
    - **Notes**:
        - Potato seed icon không tồn tại trong project (chưa có asset)
        - `crop_01.asset` có 9 null sprite refs — expected, m7b sẽ wire SO
        - `icon_Feed_Worm_Atomic_1.png` (duplicate) còn trong `UI/Icons/Animals/Chicken/` — m7b delete
        - Lowercase legacy files (`soil_empty.png`, `weed_overlay.png`, v.v.) còn trong `World/` root — m7b xóa sau khi CropTile prefab repoint
    - **Prerequisite cho m7b-sprite-wireup**: wire SO/Prefab/Scene sang new sprite paths

- **Phiên 21/04/2026 (Hiện tại) — m6b-world-progression HOÀN THÀNH**:
    - **`m6b-world-progression` spec DONE**: Tất cả 9 tasks đã execute xong qua Pure MCP.
    - **FEAT-05 — Offline Animal Growth Fix**:
        - `GameManager.BootSequence()`: set `LastSaveTime` từ `saveData.lastSaveTimestamp` TRƯỚC khi gọi `RestoreWorldState()` — đảm bảo `AnimalView.RestoreState()` tính đúng offline time
        - Welcome toast hiện sau `RestoreWorldState` nếu offline > 60s: dùng `LevelUpToastController.ShowMessage()` với thời gian format "Xg Yph" hoặc "Y phút"
    - **FEAT-06 — Locked Tile Land Expansion**:
        - `PlayerSaveData.unlockedTileIds`: thêm `List<string>` field vào `SaveData.cs` để persist trạng thái unlock
        - `CropTileView`: thêm `[Header("Lock")]` block — `_isLocked`, `_requiredLevel`, `_lockOverlay` fields; `IsLocked`/`RequiredLevel` properties; `Unlock()` method; `HandleTick()` guard `if (_isLocked) return`; `RefreshVisuals()` sync overlay
        - `WorldObjectPicker.OnTileSelected()`: guard `if (tile.IsLocked)` → `PopupManager.ShowLockInfo(tile.RequiredLevel)` + return trước khi mở CropActionPanel
        - `LockInfoPopupController.cs` tạo mới tại `Assets/_Project/Scripts/UI/Panels/` — `Setup(int requiredLevel)` set message label, OK button gọi `CloseActiveModal()`
        - `LockInfoPopup.prefab` tạo tại `Assets/_Project/Resources/UI/Default/LockInfoPopup.prefab` — Background + Message_Label + OK_Button wired
        - `PopupManager.ShowLockInfo(int)`: load `LockInfoPopup` prefab từ `_provider`, instantiate vào `_modalParent`, gọi `ctrl.Setup(requiredLevel)`
        - `GameManager.OnPlayerLevelUp(int newLevel)`: scan tất cả `CropTileView`, unlock tile có `RequiredLevel <= newLevel`, gọi `TriggerSave()` nếu có tile mới unlock
        - `GameManager.CaptureCurrentState()`: lưu `unlockedTileIds` — collect tất cả tile không locked
        - `GameManager.RestoreWorldState()`: restore lock state từ `unlockedTileIds` — unlock tile có id trong list
    - **Integration test PASSED**: 0 errors, boot sequence OK, TriggerSave hoạt động, Tile.Unlock() verified, CropTileView lock fields trong Inspector, 0 NullReferenceException

- **Phiên 21/04/2026 (Hiện tại) — m6a-player-feedback HOÀN THÀNH**:
    - **`m6a-player-feedback` spec DONE**: Tất cả 8 tasks đã execute xong qua Pure MCP.
    - **FEAT-01 — LevelUpToastController**:
        - Tạo mới `Assets/_Project/Scripts/UI/HUD/LevelUpToastController.cs`
        - Subscribe `LevelSystem.OnLevelUp` trong `OnEnable`, unsubscribe trong `OnDisable`
        - Fade out sau 2s dùng `CanvasGroup.alpha` trực tiếp (không qua `UIAnimationHelper`)
        - `ShowMessage(string msg)` public API để tái sử dụng cho welcome toast (m6b)
        - Re-trigger safe: `StopCoroutine` trước khi start coroutine mới
        - GO `LevelUpToast` tại `[HUD_CANVAS]/LevelUpToast` (SetActive=false), child `Toast_Text` (TMP, size 24, trắng)
        - **Fix hierarchy**: Lần đầu tạo bị đặt ở root — đã fix về đúng `[HUD_CANVAS]` trong Task 7
    - **FEAT-07 — Gems Save/Load**:
        - `PlayerSaveData.gems` field thêm vào `SaveData.cs` (sau `storageTier`), default `gems = 25` trong constructor
        - `GameManager.InitializeCoreSystems()`: thêm `EconomySystem.Instance.SetGems(data.gems)`
        - `GameManager.CaptureCurrentState()`: thêm `gems = EconomySystem.Instance.CurrentGems`
        - Integration test verified: set 15 gems → TriggerSave → restart → `_currentGems = 15` ✅
    - **FEAT-07 — Shop Refresh dùng Gems**:
        - `ShopPanelController`: đổi `_refreshCostGold` → `_refreshCostGems = 50`
        - `TryRefreshItems()`: dùng `CanAffordGems` + `AddGems` thay vì `CanAfford` + `AddGold`
        - Log: "Refreshed list for {N} gems" / Warning: "Not enough gems to refresh!"
        - `ShopPopup.prefab`: `GemsBalance_Label` active=true, `Refresh_Button` có child `Refresh_Label` text "Làm mới (50💎)"
    - **Integration test PASSED**: 0 errors, gems persist sau save/load, hierarchy đúng
    - **Known Issue (non-blocking)**: Game view render xanh đơn sắc — camera background issue, pre-existing, không liên quan logic

- **Phiên 20/04/2026 (Hiện tại) — m5-quest-flow HOÀN THÀNH**:
    - **`m5-quest-flow` spec DONE**: Tất cả 5 tasks đã execute xong qua Pure MCP.
    - **4 bugs fixed**:
        - BUG-Q1: `QuestPanelController` refresh — verified đúng, `OnEnable()` subscribe + `RefreshUI("")` đã có sẵn
        - BUG-Q2: `QuestManager.AcceptQuest()` — thêm prerequisite guard sau level check, trước `_activeQuests.Add()`
        - BUG-Q3: `QuestManager.HandleUnlock()` — implement switch statement: `case None: return`, `default: LogWarning` cho unsupported types
        - BUG-Q4: Feedback clarity — verified đủ logs tại accept/progress/claim/unlock
    - **`QuestManager.AcceptQuest()` prerequisite guard hoạt động**:
        - `prerequisiteQuestId` rỗng → accept bình thường
        - `prerequisiteQuestId` có giá trị + chưa completed → `Debug.LogWarning` + return
    - **`HandleUnlock()` switch statement**:
        - `case None: return` — không làm gì
        - `default: LogWarning` — không crash với bất kỳ unlockType nào
        - Audit v1: tất cả 3 quest assets dùng `unlockType=None` → không cần implement ShopTab_Animals/Building_NewNPC
    - **`QuestPanelController` event-driven refresh verified**:
        - `OnEnable()` subscribe + `RefreshUI("")` — initial state load khi panel mở
        - `OnDisable()` unsubscribe — không duplicate listeners
        - `QuestListItem.Setup()` đọc progress từ `QuestManager.GetQuestTotalProgress()`, không từ SO
    - **Integration test PASSED**: Play Mode 0 errors, boot sequence complete, 0 NullReferenceException

- **Phiên 20/04/2026 (Hiện tại) — m4-animal-care HOÀN THÀNH**:
    - **`m4-animal-care` spec DONE**: Tất cả 9 tasks đã execute xong qua Pure MCP.
    - **7 bugs fixed**:
        - BUG-01: `HandleTick()` natural death — thêm `return` trước production block
        - BUG-02: `RemoveAnimal()` được gọi trong tất cả exit paths (Sell + natural death)
        - BUG-03: `Feed()` null-safe — check `StorageSystem.Instance != null` trước `GetItemCount()`
        - BUG-04: `Feed()` warning khi thiếu food — `Debug.LogWarning` với số lượng cần thiết
        - BUG-05: `_sellButton` listener dùng `PopupManager.Instance?.CloseContextAction()` thay `SetActive(false)`
        - BUG-06: `_buyButton` listener thêm `GameManager.Instance?.TriggerSave()` sau `PurchaseAnimal()`
        - BUG-07: `Sell()` gọi `QuestEvents.InvokeActionPerformed(SellAnimal)` trước Destroy
    - **`FindObjectsOfTypeAll<GameDataRegistrySO>()` đã xóa** khỏi `AnimalPenView` — thay bằng `[SerializeField] private GameDataRegistrySO _registry`
    - **Save/Load per-animal hoạt động**:
        - `AnimalSaveData` class thêm vào `SaveData.cs` + `animals` list trong `PlayerSaveData`
        - `AnimalView.GetSaveData()` + `RestoreState()` implemented
        - `AnimalPenView.SpawnAndRestore()` implemented
        - `GameManager.CaptureCurrentState()` + `RestoreWorldState()` cập nhật để collect/restore animals
        - `GameManager.LastSaveTime` property thêm mới, cập nhật trong `TriggerSave()`
    - **Auto-collect product**: `HandleTick()` tự gọi `CollectProduct()` khi timer đủ; `_collectButton` ẩn trong `RefreshUI()`
    - **WIRE-01**: `AnimalPen.prefab` tạo mới tại `Assets/_Project/Prefabs/World/AnimalPen.prefab`:
        - `_registry` → `GameDataRegistry.asset` wired
        - `_animalPrefab` → `Animal_Placeholder.prefab` wired
        - `_animalType` → data từ `animal_01.asset` (Gà, buyCostGold=220, feedQtyWorm=1)
        - Instance trong scene: `[WORLD_ROOT]/BarnArea/AnimalPen`
    - **Integration test PASSED**: Play Mode 0 errors, Buy→Sell cycle verified, TriggerSave confirmed, RemoveAnimal confirmed
    - **Known Issue (non-blocking)**: Camera không nhìn thấy scene trong game view — camera position issue, không liên quan logic
    - **NOTE-06 added to backlog**: AnimalPen prefab creation context documented

- **Phiên 17/04/2026 (Hiện tại) — m3b-storage-sell-flow HOÀN THÀNH**:
    - **`m3b-storage-sell-flow` spec DONE**: Tất cả 5 tasks đã execute xong qua Pure MCP.
    - **3 bugs fixed**:
        - BUG-B1: `OnSellAllClick()` — thêm `if (price <= 0) continue` để skip byproducts (item_grass, item_worm) có sellPrice = 0. Thêm `if (totalGold == 0)` guard trước `AddGold`.
        - BUG-B2: `TryBuySeed()` — thêm `CanAddItem()` check TRƯỚC `CanAfford()` để gold không bị trừ khi storage đầy.
    - **`FindObjectsOfTypeAll<GameDataRegistrySO>()` đã xóa** khỏi cả 2 controllers — thay bằng `[SerializeField] private GameDataRegistrySO _registry` pattern.
    - **WIRE-01**: `StoragePopup.prefab` — `StoragePanelController._registry` wired với `GameDataRegistry.asset`.
    - **WIRE-02**: `ShopPopup.prefab` — `ShopPanelController._registry` wired với `GameDataRegistry.asset`.
    - **Integration test**: Play Mode 0 errors, boot confirmed, StorageSystem (cap=70) + EconomySystem (gold=45) running. Button interaction cần manual verify trong Editor.
- **Phiên 17/04/2026 (Hiện tại) — m3a-crop-care-harvest HOÀN THÀNH**:
    - **`m3a-crop-care-harvest` spec DONE**: Tất cả 6 tasks đã execute xong qua Pure MCP.
    - **9 bugs fixed** trong `CropTileView.cs` và `CropActionPanelController.cs`:
        - BUG-A1: `ClearWeeds()` gọi `RefreshVisuals()` ngay lập tức
        - BUG-A2: `ClearPets()` gọi `RefreshVisuals()` + null-safe `StorageSystem.Instance?`
        - BUG-A3: `HandleTick()` reset `NeedsCare → Growing` khi drainRate = 0
        - BUG-A4: Close/Harvest/Reset buttons gọi `CloseContextAction()` + `TriggerSave()`
        - BUG-A5: `Harvest()` log warning khi storage đầy, tile không reset
        - BUG-A6: `RestoreState()` gọi `RefreshVisuals()` ở cuối — visuals đúng ngay khi load
        - BUG-A7: `RefreshUI()` null-safe `CurrentCropData?.cropName ?? "[Unknown]"`
        - BUG-A9: `RestoreState()` set `GrowthStage.Ripe` đúng khi tile Ripe
        - BUG-A10: Ripe→Dead transition gọi `RefreshVisuals()` ngay lập tức
    - **WIRE-01**: `ContextActionPanel.prefab` — `_registry` wired với `GameDataRegistry.asset` (tại `Assets/_Project/Data/Registry/`)
    - **Full cycle hoạt động**: plant → grow → ailment → care → ripe → harvest → items vào StorageSystem
    - **Integration test PASSED**: Play Mode 0 errors, boot confirmed, save/load cycle hoạt động
    - **Known Issue (non-blocking)**: FarmCameraController (Old Input) + WorldObjectPicker (New Input) input conflict — pre-existing, acceptable ở prototype
- **Phiên 17/04/2026 — scn-main-world-setup HOÀN THÀNH**:
    - **`scn-main-world-setup` spec DONE**: Tất cả 9 tasks đã execute xong qua Pure MCP.
    - **6 CropTile placed** trong CropArea (2×3 grid): `tile_r0_c0` → `tile_r1_c2`, layer=Interactable, CropTileView wired, `_registry` không null.
    - **WorldObjectPicker + PlayerInput** wired trên cùng GO trong `[SYSTEMS]`. PlayerInputActions.inputactions tạo mới tại `Assets/_Project/Input/`.
    - **FarmCameraController** attached lên Main Camera: `_panSpeed=1.0`, `_boundX=(-5,5)`, `_boundY=(-3,3)`, `_zoomSpeed=4`, `_minOrtho=3`, `_maxOrtho=8`.
    - **TimeManager + QuestManager** tạo mới trong `[SYSTEMS]`, `_tickRate=1.0`.
    - **GameDataRegistrySO** verified: 7 crops hợp lệ, tất cả `postRipeLifeMin > 0` (BUG-08 prevented).
    - **Integration test PASSED**: Play Mode 0 errors, `[GameManager] Boot sequence complete.` confirmed.
    - **Known Issue (non-blocking)**: FarmCameraController (Old Input) + WorldObjectPicker (New Input) cùng fire khi tap — acceptable ở prototype.
    - **Cleanup cần làm thủ công**: Stray `CropTile` GO ở root scene (leftover từ Task 2 prefab creation) — xóa trong Unity Editor.
    - **Placeholders pending**: soil_empty, weed_overlay, bug_overlay, water_needed, crop phase sprites — prompts tại `docs/asset-prompts/2026-04-16-scn-main-world-setup-missing-assets.md`.
- **Phiên 16/04/2026 (Hiện tại) — SCN_Main UI Rebuild HOÀN THÀNH**:
    - **`scn-main-ui-rebuild` spec DONE**: Tất cả 11 tasks đã execute xong qua Pure MCP.
    - **SCN_Main đã có**: 4 canvas (HUD=10, Popup=20, System=30, World), TopHUDBar, BottomNav (5 buttons), PopupManager wired, ContextActionPanel, ShopPopup, StoragePopup, AnimalDetailPanel.
    - **Prefabs extracted** vào `Assets/_Project/Resources/UI/Default/`: ShopPopup, StoragePopup, AnimalDetailPopup, ContextActionPanel.
    - **Integration test PASSED**: Play Mode 0 errors, HUD data binding hoạt động (gold=46, storage=6/70 từ save data).
    - **Editor Tools tạo mới** tại `Assets/_Project/Editor/Tools/`: SetTMPFonts, FixTopHUDBarLayout, SetupBottomNav, SetupBottomNavButtons, SetupContextActionPanel, SetupShopPopup, SetupStoragePopup, SetupAnimalDetailPanel, ExtractUIPrefabs.
    - **Bug notes**: Xem NOTE-04 (Refresh_Button thêm ngoài spec) và NOTE-05 (GemsBalance_Label) trong `docs/backlog/bug-backlog.md`.
    - **SCN_Gameplay giữ nguyên** để tham khảo.
- **Phiên 16/04/2026 (Sáng)**:
    - **Spec mới `scn-main-world-setup` đã tạo**: Kiro spec đầy đủ cho M2 milestone (world gameplay layer). Bao gồm `requirements.md`, `design.md`, `tasks.md` với 9 tasks. **Chưa execute — sẵn sàng để implement.**
    - **Brainstorm kỹ**: Đọc 8 scripts, phát hiện 6 issues quan trọng (ghi vào `bug-backlog.md`): `postRipeLifeMin=0` bug, PlayerInput GO placement, Camera.main tag, CropTileView tileId fallback, FarmCamera Old/New Input conflict, QuestManager dependency.
    - **bug-backlog.md cập nhật**: Thêm BUG-08, BUG-09, NOTE-01, NOTE-02, NOTE-03.
- **Phiên 15/04/2026**:
    - **UI/Theme System Removal**: Xóa toàn bộ hệ thống styling tự động (`UIStyleApplier`, `UIStyleProcessor`, `UIStyleDataSO`, `UIInitializer`). Lý do: hệ thống quá phức tạp, không cần thiết cho workflow Pure MCP hiện tại.
    - **Code Logic Review & Cleanup**: Refactor 6 files (`PopupManager`, `CropActionPanelController`, `IUIAssetProvider`, `ResourcesUIProvider`, `GameSceneInitializer`, `GameDataManagerWindow`) để loại bỏ mọi dependency vào styling system.
    - **Simplified Infrastructure**: `ResourcesUIProvider` giờ load thẳng từ `UI/Default/{key}` không qua theme fallback. `PopupManager` không còn inject style vào popup khi spawn.
    - **Data Manager Cleanup**: Tab "UI/Themes" đã bị xóa khỏi `GameDataManagerWindow`. Chỉ còn: Crops, Animals, Quests, Settings.
    - **MCP Unity Connect**: Kết nối UnityMCP cho project NTVV qua `.mcp.json` (bundled Python tại `Assets/StreamingAssets/realvirtual-MCP/`).
    - **SCN_Main UI Spec hoàn thành**: Đã tạo Kiro Spec đầy đủ tại `.kiro/specs/scn-main-ui-rebuild/` gồm `requirements.md`, `design.md`, `tasks.md`. Spec bao gồm 11 tasks để build toàn bộ UI mới qua Pure MCP — 4 canvas, 7 UI components, wire scripts, extract prefabs. **Chưa execute — sẵn sàng để implement.**
- **Phiên 09/04/2026**:
    - **100% Atomic Asset Success**: Hoàn tất sản xuất và làm sạch bộ 21 icons mới (Cà rốt, Gà, Tài nguyên, Bổ sung nút bấm). Tổng cộng 28 assets đạt chuẩn.
    - **Hyper-Granular Asset Organization**: Triển khai cấu trúc thư mục UI Art chuyên sâu tại `Assets/_Project/Art/Sprites/UI/` (Backgrounds, Icons/Common, Icons/Crops, Icons/Animals).
    - **Atomic HUD Audit**: Đối soát thành công giữa Library Prompt và thực tế file, đảm bảo không thiếu hụt Metadata.
- **Phiên 08/04/2026**:
    - **Visual Standardization (PPU Multiplier)**: Thiết lập tiêu chuẩn độ phân giải visual đồng nhất (Buttons/Chips: 5, Banners: 2.5, Panels: 1.5, Icons: 1).
    - **Structural Rebirth [UI_ATOMIC_STAGE]**: Tái cấu trúc Root Hierarchy sang hệ thống mới: `[UI_CAMERA]` và `[UI_ATOMIC_STAGE]/[SAFE_AREA]`.
- **Phiên 17/04/2026 (Hiện tại) — M5 Quest Flow Spec Written**:
    - **`m5-quest-flow` spec WRITTEN**: Tất cả 3 files (design, requirements, tasks) đã tạo qua Pure MCP.
    - **Scope**: Fix 4 bugs trong Quest runtime flow (BUG-Q1..Q4) — prerequisite enforcement, UI refresh, HandleUnlock runtime, integration test.
    - **Chưa execute** — sẵn sàng để implement khi cần.
- **Phiên 17/04/2026 (Hiện tại) — Asset Inventory & Prompt Rework Completed**:
    - **Full sprite inventory** tại `docs/asset-prompts/2026-04-17-asset-inventory-all.md` — 100 total sprites: 44 exist, 40 missing, 2 placeholders, 16 reuse.
    - **Prompt system reworked** vào folder mới: `docs/asset-prompts/2026-04-17-rework/`
        - `master-index.md` — index + global rules
        - `world-overlays.md` — world tile/overlay prompts
        - `entities-crops.md` — full 31 missing crop assets
        - `entities-animals.md` — full 6 missing animal assets
        - `ui.md` — full 3 missing UI assets
    - **Old prompt files removed** để tránh nhầm lẫn với bộ mới:
        - `docs/asset-prompts/2026-04-16-scn-main-ui-rebuild-missing-assets.md`
        - `docs/asset-prompts/2026-04-16-scn-main-world-setup-missing-assets.md`
        - `docs/asset-prompts/2026-04-17-animal-sprites-missing.md`
        - `docs/asset-prompts/2026-04-17-items-nav-sprites-missing.md`
    - Rework rules: English-only naming/prompt/setup, stage `Stage00..Stage03`, no `_v01`, one sprite per file, click by footprint collider.

## 🗺 Bản đồ Hệ thống (System Map)
Nếu bạn mở dự án ở máy tính khác, AI hãy chú ý các file "đầu não" mới:
1.  **Dữ liệu Tập trung & Cấu hình**: `GameDataRegistry.asset` và thư mục `Assets/_Project/Data/Configs/`.
2.  **Kho Art chuẩn Atomic**: `Assets/_Project/Art/Sprites/UI/` - Nơi chứa toàn bộ ảnh đã qua xử lý.
3.  **UI Infrastructure (đã đơn giản hóa)**:
    - `Assets/_Project/Scripts/UI/Infrastructure/IUIAssetProvider.cs` — interface load prefab.
    - `Assets/_Project/Scripts/UI/Infrastructure/ResourcesUIProvider.cs` — load từ `Resources/UI/Default/`.
    - `Assets/_Project/Scripts/UI/Common/PopupManager.cs` — quản lý screen stack.
4.  **Quy chuẩn UI Pure MCP & AI Production**:
    - `docs/guides/AI_UI_Integration_Methodology.md`: Kim chỉ nam về chiến lược gán nhãn, styling và PPU standards.
    - `docs/guides/Atomic_HUD_Prompt_Library.md`: Thư viện Prompt chuẩn cho sản xuất asset.

## 🎯 Trạng thái hiện tại & Bước tiếp theo
- **Đã xong**: 
    - Sản xuất 100% Asset bộ khởi động (Carrot, Chicken, UI Backgrounds, Resource Icons).
    - Quy hoạch thư mục Art khoa học.
    - **✅ `scn-main-ui-rebuild` DONE** — SCN_Main có đầy đủ UI: TopHUDBar, BottomNav, 4 popups, PopupManager wired, prefabs extracted.
    - **✅ `scn-main-world-setup` DONE** — World Setup (M2) hoàn thành
    - **✅ `m3a-crop-care-harvest` DONE** — 9 bugs fixed, full crop cycle hoạt động
    - **✅ `m3b-storage-sell-flow` DONE** — 3 bugs fixed (BUG-B1, BUG-B2), FindObjectsOfTypeAll removed, StoragePopup + ShopPopup `_registry` wired
    - **✅ `m4-animal-care` DONE** — 7 bugs fixed (BUG-01..07), Save/Load per-animal, auto-collect, AnimalPen.prefab created & wired
    - **✅ `m5-quest-flow` DONE** — 4 bugs fixed (BUG-Q1..Q4), prerequisite enforcement, HandleUnlock switch, QuestPanelController verified
    - **✅ `m6b-world-progression` DONE** — FEAT-05: offline animal growth fix (LastSaveTime set trước RestoreWorldState, welcome toast > 60s); FEAT-06: locked tile system (CropTileView lock fields, WorldObjectPicker guard, LockInfoPopup, auto-unlock on level up, save/load unlockedTileIds)
    - **✅ `m6a-player-feedback` DONE** — LevelUpToastController tạo mới, Gems save/load fix, Shop Refresh dùng gems (50💎)
    - **📋 Asset Inventory Completed** — full sprite list and prompt docs for missing sprites (priority: Duck/animal, items/nav, crops)
- **Cần làm ngay**: 
    1. **Cleanup thủ công**: Xóa stray `CropTile` GO ở root scene trong Unity Editor
    2. **Manual UI test**: Verify Storage sell flow + Shop buy flow trong Play Mode (button interaction cần manual)
    3. **M7: Next milestone** — xác định milestone tiếp theo (ví dụ: thêm tile mới vào scene với `_isLocked=true`, set `_requiredLevel` phù hợp, wire `_lockOverlay` GO)
    4. **Logic Wiring**: Nối dây các Sprite mới vào `CropDataSO` và `AnimalDataSO` thông qua Registry.
    5. **Generate missing sprites** — sử dụng bộ rework tại `docs/asset-prompts/2026-04-17-rework/` (bắt đầu với `entities-animals.md`)
    6. **Verify Refresh_Button & GemsBalance_Label** (xem NOTE-04, NOTE-05 trong bug-backlog.md)
    7. **Camera fix**: Game view toàn màu xanh — cần điều chỉnh Main Camera position/orthographic size để nhìn thấy scene
    8. **LockOverlay asset**: Tạo/gán sprite overlay cho `_lockOverlay` GO trên các CropTileView locked (hiện đang dùng placeholder)

## 🗂 Kiro Specs đang active

### Spec 1: `scn-main-ui-rebuild` ✅ DONE
- **Path**: `.kiro/specs/scn-main-ui-rebuild/`
- **Status**: **TẤT CẢ 11 TASKS HOÀN THÀNH** (16/04/2026)
- **Kết quả**:
    - SCN_Main tại `Assets/_Project/Scenes/SCN_Main.unity`
    - 4 canvas: HUD(10), Popup(20), System(30) + WorldRoot
    - TopHUDBar (HUDTopBarController wired), BottomNav (5 buttons, BottomNavController wired)
    - PopupManager wired: `_modalParent`, `_hudParent`, `_mainOverlayCanvas`
    - Prefabs: ShopPopup, StoragePopup, AnimalDetailPopup, ContextActionPanel → `Resources/UI/Default/`
    - Integration test: 0 errors, data binding hoạt động

### Spec 2: `scn-main-world-setup` ✅ DONE
- **Path**: `.kiro/specs/scn-main-world-setup/`
- **Status**: **TẤT CẢ 9 TASKS HOÀN THÀNH** (17/04/2026)
- **Kết quả**:
    - 6 CropTile (2×3 grid) trong CropArea, tên `"tile_r{r}_c{c}"`, layer=Interactable, CropTileView wired
    - WorldObjectPicker + PlayerInput trên cùng GO trong `[SYSTEMS]`
    - FarmCameraController trên Main Camera với bounds (-5,5) / (-3,3)
    - TimeManager (`_tickRate=1.0`) + QuestManager trong `[SYSTEMS]`
    - GameDataRegistrySO: 7 crops valid, postRipeLifeMin > 0 trên tất cả
    - Integration test: 0 errors, GameManager boot confirmed

### Spec 3: `m3a-crop-care-harvest` ✅ DONE
- **Path**: `.kiro/specs/m3a-crop-care-harvest/`
- **Status**: **TẤT CẢ 6 TASKS HOÀN THÀNH** (17/04/2026)
- **Kết quả**:
    - 9 bugs fixed trong `CropTileView.cs` (A1, A2, A3, A5, A6, A9, A10) và `CropActionPanelController.cs` (A4, A7)
    - `ContextActionPanel.prefab`: `_registry` wired với `GameDataRegistry.asset`
    - Full cycle: plant → grow → ailment → care → ripe → harvest → StorageSystem
    - Integration test: 0 errors, save/load cycle hoạt động

### Spec 4: `m3b-storage-sell-flow` ✅ DONE
- **Path**: `.kiro/specs/m3b-storage-sell-flow/`
- **Status**: **TẤT CẢ 5 TASKS HOÀN THÀNH** (17/04/2026)
- **Kết quả**:
    - BUG-B1 fixed: `StoragePanelController.OnSellAllClick()` — filter `price <= 0`, guard `totalGold == 0`
    - BUG-B2 fixed: `ShopPanelController.TryBuySeed()` — `CanAddItem()` check trước `CanAfford()`
    - `FindObjectsOfTypeAll<GameDataRegistrySO>()` xóa khỏi `StoragePanelController` và `ShopPanelController`
    - `StoragePopup.prefab`: `_registry` → `GameDataRegistry.asset` wired
    - `ShopPopup.prefab`: `_registry` → `GameDataRegistry.asset` wired
    - Integration test: 0 errors, systems running

### Spec 5: `m4-animal-care` ✅ DONE
- **Status**: **TẤT CẢ 9 TASKS HOÀN THÀNH** (20/04/2026)
- **Kết quả**:
    - 7 bugs fixed: BUG-01 (production guard), BUG-02 (RemoveAnimal), BUG-03 (Feed null-safe), BUG-04 (Feed warning), BUG-05 (sell CloseContextAction), BUG-06 (buy autosave), BUG-07 (QuestEvent)
    - `FindObjectsOfTypeAll<GameDataRegistrySO>()` xóa khỏi `AnimalPenView`
    - `AnimalPen.prefab` tạo mới: `_registry` + `_animalPrefab` + `_animalType` (Gà/animal_01) wired
    - Save/Load per-animal: `AnimalSaveData`, `GetSaveData()`, `RestoreState()`, `SpawnAndRestore()`, `GameManager` collect/restore
    - Auto-collect: `HandleTick()` tự gọi `CollectProduct()`, `_collectButton` ẩn
    - Integration test: 0 errors, Buy→Sell cycle verified, TriggerSave confirmed

### Spec 6: `m5-quest-flow` ✅ DONE
- **Path**: `.kiro/specs/m5-quest-flow/`
- **Status**: **TẤT CẢ 5 TASKS HOÀN THÀNH** (20/04/2026)
- **Kết quả**:
    - BUG-Q1 fixed: `QuestPanelController` refresh — verified đúng, event-driven architecture intact
    - BUG-Q2 fixed: `QuestManager.AcceptQuest()` — prerequisite guard thêm sau level check, trước `_activeQuests.Add()`
    - BUG-Q3 fixed: `QuestManager.HandleUnlock()` — switch statement với `case None: return` + `default: LogWarning`
    - BUG-Q4 fixed: Feedback clarity — logs đầy đủ tại accept/progress/claim/unlock
    - Audit v1: 3 quest assets (q_harvest_wheat, q_buy_chicken, q_reach_level_2), tất cả dùng `unlockType=None`
    - Integration test: 0 errors, boot sequence complete

### Spec 7: `m6a-player-feedback` ✅ DONE
- **Path**: `.kiro/specs/m6a-player-feedback/`
- **Status**: **TẤT CẢ 8 TASKS HOÀN THÀNH** (21/04/2026)
- **Kết quả**:
    - FEAT-01: `LevelUpToastController.cs` tạo mới — subscribe `LevelSystem.OnLevelUp`, fade 2s qua `CanvasGroup.alpha`, `ShowMessage(string)` public API
    - FEAT-01: GO `LevelUpToast` tại `[HUD_CANVAS]/LevelUpToast` (SetActive=false), child `Toast_Text` (TMP, size 24, trắng)
    - FEAT-07: `PlayerSaveData.gems` field + `gems = 25` default trong constructor
    - FEAT-07: `GameManager` — `SetGems(data.gems)` trong `InitializeCoreSystems`, `data.gems = CurrentGems` trong `CaptureCurrentState`
    - FEAT-07: `ShopPanelController` — `_refreshCostGems = 50`, `TryRefreshItems()` dùng `CanAffordGems` + `AddGems`
    - FEAT-07: `ShopPopup.prefab` — `GemsBalance_Label` active, `Refresh_Label` text "Làm mới (50💎)"
    - Integration test: 0 errors, gems persist sau save/load verified

### Spec 8: `m6b-world-progression` ✅ DONE
- **Path**: `.kiro/specs/m6b-world-progression/`
- **Status**: **TẤT CẢ 9 TASKS HOÀN THÀNH** (21/04/2026)
- **Kết quả**:
    - FEAT-05: `GameManager.BootSequence()` — `LastSaveTime` set từ `saveData.lastSaveTimestamp` trước `RestoreWorldState()`
    - FEAT-05: Welcome toast hiện nếu offline > 60s — format "Xg Yph" / "Y phút" qua `LevelUpToastController.ShowMessage()`
    - FEAT-06: `PlayerSaveData.unlockedTileIds` — `List<string>` field mới trong `SaveData.cs`
    - FEAT-06: `CropTileView` — lock fields (`_isLocked`, `_requiredLevel`, `_lockOverlay`), `Unlock()` method, `HandleTick()` guard, `RefreshVisuals()` sync
    - FEAT-06: `WorldObjectPicker.OnTileSelected()` — locked tile guard → `ShowLockInfo()` + return
    - FEAT-06: `LockInfoPopupController.cs` tạo mới + `LockInfoPopup.prefab` tại `Resources/UI/Default/`
    - FEAT-06: `PopupManager.ShowLockInfo(int)` — load + instantiate + `ctrl.Setup(requiredLevel)`
    - FEAT-06: `GameManager.OnPlayerLevelUp()` — auto-unlock tiles + `TriggerSave()`
    - FEAT-06: `GameManager.CaptureCurrentState()` + `RestoreWorldState()` — save/load `unlockedTileIds`
    - Integration test: 0 errors, 0 NullReferenceException, tất cả checks PASSED

---

> [!TIP]
> **Dành cho AI**: "Chào người bạn AI mới! Hệ thống UIStyleApplier/UIStyleDataSO đã bị XÓA hoàn toàn. Styling hiện tại làm **thủ công qua MCP** — gán Sprite/Color trực tiếp vào component. Kiểm tra `Assets/_Project/Art/Sprites/UI/` để thấy bộ 'Lego' Assets. **M2 World Setup đã DONE** — SCN_Main có đầy đủ UI + World layer: 6 CropTile, WorldObjectPicker, FarmCameraController, TimeManager, QuestManager. **M3a Crop Care + Harvest đã DONE** — 9 bugs fixed trong CropTileView + CropActionPanelController, full cycle plant→care→harvest→save/load hoạt động. **M3b Storage + Sell Flow đã DONE** — 3 bugs fixed (BUG-B1 Sell All filter, BUG-B2 storage check trước gold deduct), FindObjectsOfTypeAll removed, StoragePopup + ShopPopup `_registry` wired. **M4 Animal Care đã DONE** — 7 bugs fixed (BUG-01..07), Save/Load per-animal hoạt động, auto-collect product, AnimalPen.prefab tạo mới và wired đầy đủ. **M5 Quest Flow đã DONE** — 4 bugs fixed (BUG-Q1..Q4): prerequisite enforcement, HandleUnlock switch, QuestPanelController event-driven refresh verified. **M6a Player Feedback đã DONE** — LevelUpToastController tạo mới (subscribe OnLevelUp, fade 2s, ShowMessage API), Gems save/load fix (PlayerSaveData.gems, SetGems/CaptureGems trong GameManager), Shop Refresh dùng gems 50💎 thay gold. **M6b World Progression đã DONE** — FEAT-05: offline animal growth fix (LastSaveTime set trước RestoreWorldState, welcome toast > 60s); FEAT-06: locked tile system (CropTileView lock fields, WorldObjectPicker guard, LockInfoPopup, auto-unlock on level up, save/load unlockedTileIds). Bước tiếp theo: thêm locked tiles vào scene với `_isLocked=true` + `_requiredLevel`, wire `_lockOverlay` GO, xác định M7 milestone."