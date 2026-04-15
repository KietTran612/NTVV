# Project Handover - NTVV (Nông Trại Vui Vẻ)

Tài liệu này dùng để đồng bộ nhanh "suy nghĩ" của AI Agent khi bạn chuyển sang máy tính mới hoặc bắt đầu một phiên làm việc mới.

## 🧠 Bối cảnh Phiên làm việc (Session Context)
- **Phiên 15/04/2026 (Hiện tại)**:
    - **UI/Theme System Removal**: Xóa toàn bộ hệ thống styling tự động (`UIStyleApplier`, `UIStyleProcessor`, `UIStyleDataSO`, `UIInitializer`). Lý do: hệ thống quá phức tạp, không cần thiết cho workflow Pure MCP hiện tại.
    - **Code Logic Review & Cleanup**: Refactor 6 files (`PopupManager`, `CropActionPanelController`, `IUIAssetProvider`, `ResourcesUIProvider`, `GameSceneInitializer`, `GameDataManagerWindow`) để loại bỏ mọi dependency vào styling system.
    - **Simplified Infrastructure**: `ResourcesUIProvider` giờ load thẳng từ `UI/Default/{key}` không qua theme fallback. `PopupManager` không còn inject style vào popup khi spawn.
    - **Data Manager Cleanup**: Tab "UI/Themes" đã bị xóa khỏi `GameDataManagerWindow`. Chỉ còn: Crops, Animals, Quests, Settings.
    - **MCP Unity Connect**: Kết nối UnityMCP cho project NTVV qua `.mcp.json` (bundled Python tại `Assets/StreamingAssets/realvirtual-MCP/`).
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
- **Đã xong**: Sản xuất 100% Asset bộ khởi động (Carrot, Chicken, UI Backgrounds, Resource Icons). Quy hoạch thư mục Art khoa học.
- **Cần làm ngay**: 
    1. **Logic Wiring**: Nối dây các Sprite mới vào `CropDataSO` và `AnimalDataSO` thông qua Registry.
    2. **UI Prototype Assembly**: Lắp ráp màn hình Gameplay HUD thực tế sử dụng Asset chuẩn — gán sprite trực tiếp qua MCP, không qua UIStyleApplier.
    3. **Prefab Wiring**: Kiểm tra các prefab trong `Resources/UI/Default/` còn reference nào đến UIStyleApplier component không, nếu có thì xóa.

---

> [!TIP]
> **Dành cho AI**: "Chào người bạn AI mới! Hệ thống UIStyleApplier/UIStyleDataSO đã bị XÓA hoàn toàn. Styling hiện tại làm **thủ công qua MCP** — gán Sprite/Color trực tiếp vào component. Kiểm tra `Assets/_Project/Art/Sprites/UI/` để thấy bộ 'Lego' Assets. Đọc `docs/guides/Atomic_HUD_Prompt_Library.md` cho quy trình sản xuất asset."
