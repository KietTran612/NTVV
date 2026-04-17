# Project Handover - NTVV (Nông Trại Vui Vẻ)

Tài liệu này dùng để đồng bộ nhanh "suy nghĩ" của AI Agent khi bạn chuyển sang máy tính mới hoặc bắt đầu một phiên làm việc mới.

## 🧠 Bối cảnh Phiên làm việc (Session Context)
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
- **Phiên 17/04/2026 (Hiện tại) — Asset Inventory Completed**:
    - **Full sprite inventory** tại `docs/asset-prompts/2026-04-17-asset-inventory-all.md` — 100 total sprites: 44 exist, 40 missing, 2 placeholders, 16 reuse.
    - **Prompt docs created** for missing sprites (prioritized):
        - `docs/asset-prompts/2026-04-17-animal-sprites-missing.md` — Chicken dead + Duck all stages (6 sprites)
        - `docs/asset-prompts/2026-04-17-items-nav-sprites-missing.md` — Grass, Shop, Event icons (3 sprites)
        - `docs/asset-prompts/2026-04-17-crop-sprites-missing.md` — Carrot dead + 6 crops × 5 stages (31 sprites) — NON-BLOCKING (fallback available)

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
    - **✍️ `m5-quest-flow` SPEC WRITTEN** — design, requirements, tasks completed (4 quest bugs identified)
    - **📋 Asset Inventory Completed** — full sprite list and prompt docs for missing sprites (priority: Duck/animal, items/nav, crops)
- **Cần làm ngay**: 
    1. **Cleanup thủ công**: Xóa stray `CropTile` GO ở root scene trong Unity Editor
    2. **M3b: Storage + Sell Flow** — Tạo spec mới cho storage UI integration và sell flow
    3. **Logic Wiring**: Nối dây các Sprite mới vào `CropDataSO` và `AnimalDataSO` thông qua Registry.
    4. **Execute `m5-quest-flow`** — khi cần sửa quest bugs (BUG-Q1..Q4)
    5. **Generate missing sprites** — sử dụng prompt docs tại `docs/asset-prompts/` (bắt đầu với animal sprites để hỗ trợ M4 integration test)
    6. **Verify Refresh_Button & GemsBalance_Label** (xem NOTE-04, NOTE-05 trong bug-backlog.md).

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

### Spec 4: `m5-quest-flow` ✍️ SPEC WRITTEN (Chưa execute)
- **Path**: `.kiro/specs/m5-quest-flow/`
- **Status**: **Design, Requirements, Tasks completed** (17/04/2026) — **Chưa execute**
- **Kết quả**:
    - 4 quest bugs identified: BUG-Q1 (UI refresh), BUG-Q2 (prerequisite enforcement), BUG-Q3 (HandleUnlock runtime), BUG-Q4 (feedback clarity)
    - Chuẩn bị sẵn để implement khi cần

### Spec 5: `m3b-storage-sell-flow` ← EXECUTE TIẾP THEO
- **Path**: `.kiro/specs/m3b-storage-sell-flow/`
- **Status**: Chưa execute — cần tạo/review requirements/design/tasks
- **Scope**: Storage UI integration, sell flow, economy feedback

---

> [!TIP]
> **Dành cho AI**: "Chào người bạn AI mới! Hệ thống UIStyleApplier/UIStyleDataSO đã bị XÓA hoàn toàn. Styling hiện tại làm **thủ công qua MCP** — gán Sprite/Color trực tiếp vào component. Kiểm tra `Assets/_Project/Art/Sprites/UI/` để thấy bộ 'Lego' Assets. **M2 World Setup đã DONE** — SCN_Main có đầy đủ UI + World layer: 6 CropTile, WorldObjectPicker, FarmCameraController, TimeManager, QuestManager. **M3a Crop Care + Harvest đã DONE** — 9 bugs fixed trong CropTileView + CropActionPanelController, full cycle plant→care→harvest→save/load hoạt động. **M5 Quest Flow spec đã WRITTEN** — 4 bugs identified, sẵn sàng để implement. Bước tiếp theo là M3b: Storage + Sell flow và tạo ra các sprite còn thiếu theo prompts đã có."