# Project Handover - NTVV (Nông Trại Vui Vẻ)

Tài liệu này dùng để đồng bộ nhanh "suy nghĩ" của AI Agent khi bạn chuyển sang máy tính mới hoặc bắt đầu một phiên làm việc mới.

## 🧠 Bối cảnh Phiên làm việc (Session Context)
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
- **Cần làm ngay**: 
    1. **~~Execute SCN_Main UI Spec~~** ✅ DONE
    2. **Execute `scn-main-world-setup`**: Mở `.kiro/specs/scn-main-world-setup/tasks.md` và execute. Setup CropArea, BarnArea, WorldObjectPicker, PlayerInput.
    3. **Logic Wiring**: Nối dây các Sprite mới vào `CropDataSO` và `AnimalDataSO` thông qua Registry.
    4. **Verify Refresh_Button & GemsBalance_Label** (xem NOTE-04, NOTE-05 trong bug-backlog.md).

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

### Spec 2: `scn-main-world-setup` ← EXECUTE TIẾP THEO
- **Path**: `.kiro/specs/scn-main-world-setup/`
- **Status**: Chưa execute — Task 0-9 đều `[ ]`
- **Approach**: Pure MCP + 1 Editor Script mới (`CropGridSpawner.cs`)
- **Prerequisite**: scn-main-ui-rebuild Task 1-5 phải xong trước Integration Test
- **Key design decisions**:
    - 6 CropTile (2×3 grid) trong CropArea, tên `"tile_r{r}_c{c}"` cho SaveLoad
    - WorldObjectPicker + PlayerInput trên **cùng GO** (Send Messages requirement)
    - BoxCollider 3D (không phải 2D) — WorldObjectPicker dùng Physics.Raycast 3D
    - Main Camera cần tag `"MainCamera"` cho Camera.main
    - `postRipeLifeMin > 0` là CRITICAL — xem BUG-08 trong bug-backlog.md

---

> [!TIP]
> **Dành cho AI**: "Chào người bạn AI mới! Hệ thống UIStyleApplier/UIStyleDataSO đã bị XÓA hoàn toàn. Styling hiện tại làm **thủ công qua MCP** — gán Sprite/Color trực tiếp vào component. Kiểm tra `Assets/_Project/Art/Sprites/UI/` để thấy bộ 'Lego' Assets. Spec UI mới đã sẵn sàng tại `.kiro/specs/scn-main-ui-rebuild/` — đọc `design.md` để hiểu architecture trước khi execute tasks."
