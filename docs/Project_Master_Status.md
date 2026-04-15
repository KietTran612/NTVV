# Project Master Status: NTVV - Nông Trại Vui Vẻ

Tài liệu này tổng hợp toàn bộ tiến độ dự án, kết nối các bản kế hoạch chi tiết (`docs/plans`) và định hướng các bước tiếp theo.

---

## 📊 Tổng quan Tiến độ (Epic Progress)

| Epic | Tên Epic | Trạng thái | Tài liệu Kế hoạch |
| :--- | :--- | :--- | :--- |
| **Epic 1** | Core Framework & Time System | ✅ Hoàn thành | [Core Implementation](file:///d:/soflware/Unity/Source/NTVV/docs/plans/2026-04-02-core-framework-implementation.md) |
| **Epic 2** | UI System (Architect, Manual Builder, Stylist) | ✅ Hoàn thành | [Pure MCP Workflow](file:///C:/Users/Hoang.H/.gemini/antigravity/brain/c3f03e90-b044-4185-9073-dd5a521a0f43/implementation_plan.md) |
| **Epic 3** | Shop, Economy & Animal Pen | ✅ Hoàn thành | [Economic Implementation](file:///d:/soflware/Unity/Source/NTVV/docs/plans/2026-04-02-Epic4-Shop-Economy-Implementation.md) |
| **Epic 4** | Quest System & Editor Tools | ✅ Hoàn thành | [Quest Task Tracker](file:///d:/soflware/Unity/Source/NTVV/docs/plans/task_quest_system.md) |
| **Epic 5** | Persistence & Sync | ✅ Hoàn thành | [Persistence Design](file:///d:/soflware/Unity/Source/NTVV/docs/plans/2026-04-02-enhanced-persistence-test-design.md) |
| Epic 6 | Centralized Data & Scene Automation | ✅ Hoàn thành | [Data Architecture Plan](file:///C:/Users/Hoang.H/.gemini/antigravity/brain/c562547a-dba6-474c-8d7a-0a5a61cb6538/implementation_plan.md) |
| **Epic 7** | Refined HUD & Atomic Asset Expansion | ⏳ Giai đoạn Tích hợp | [Atomic HUD Plan](file:///C:/Users/Hoang.H/.gemini/antigravity/brain/c3f03e90-b044-4185-9073-dd5a521a0f43/artifacts/kawaii_atomic_assets_plan.md) |

---

## 🛠 Chi tiết các Hệ thống đã làm được

### 1. Trồng trọt (Crops)
- Logic trồng, lớn theo thời gian (Tick), gặt hái.
- Quy hoạch folder chuyên biệt tại `Data/Crops/`.

### 2. Kinh nghiệm & Cấp độ (Leveling)
- Hệ thống XP, Milestone, tự động Level Up.
- **Self-Healing**: Tự động nạp dữ liệu Level từ Registry.

### 3. Kinh tế (Economy) & Cửa hàng (Shop)
- Quản lý Vàng (Gold), mua hạt giống/vật nuôi.
- Danh sách vật phẩm được quản lý tập trung qua Registry.

### 4. Chăn nuôi (Animal & Pens)
- Vòng đời thú, Cho ăn, Thu hoạch sản phẩm.
- **Self-Healing**: Tự động nạp cấu hình nâng cấp chuồng.

### 5. Nhiệm vụ (Quest)
- Theo dõi tiến độ qua sự kiện (Events).
- Thư mục quản lý tập trung tại `Data/Quests/`.

### 6. Quản trị Dữ liệu (Editor Tools)
- **Data Registry**: Trung tâm lưu trữ toàn bộ config tĩnh của Game.
- **Data Manager** (`NTVV/Game Data Manager`): Chỉnh sửa tập trung cho Crops, Animals, Quests, Settings. *(Tab UI/Themes đã xóa — 15/04/2026)*
- **Scene Setup** (`NTVV/Setup Full Game Scene`): 1-Click tạo scene gameplay hoàn chỉnh, không còn phụ thuộc UIStyleDataSO.

### 7. Kiến trúc UI Pure MCP (Architect, Manual Builder)
- **Quy trình Thực thi 100% bằng Agent thông qua MCP Skills**:
    1.  **Phase 1: Architect (Blueprinting)**: AI quét thiết kế qua `pencil.batch_get`, tìm kiếm ID linh kiện và vạch ra cấu trúc Transform/Layout chuẩn.
    2.  **Phase 2: Manual Builder**: Agent trực tiếp xây dựng Hierarchy trong Unity bằng `gameobject-create`, `component-add`. Tự động nối dây Reference thông qua định danh chính xác (Manual Wiring).
    3.  **Phase 3: Direct Styling via MCP**: Gán trực tiếp Sprite, Color, Material vào component qua `component-modify` — **không dùng UIStyleApplier** *(đã xóa 15/04/2026)*.
- **Tiêu chuẩn Typography**: 
    - Sử dụng bộ font **Dosis-ExtraBold SDF.asset**.
    - Hệ thống Preset Material Outline (Blue, Green, Red, Yellow) được gán trực tiếp qua đường dẫn Art Assets.
- **⚠️ Lưu ý quan trọng**: `UIStyleApplier`, `UIStyleProcessor`, `UIStyleDataSO` đã bị **XÓA HOÀN TOÀN**. Mọi styling làm thủ công qua MCP.

### 8. Tự động hóa Scene (Scene Automation)
- Công cụ **Setup Full Game Scene** 1-Click tạo scene gameplay hoàn chỉnh.
- **Cấu trúc Scene [UI_CAMERA][UI_ATOMIC_STAGE]**: Tiêu chuẩn hóa Root Hierarchy cho UI:
    - `[UI_CAMERA]`: Camera chuyên biệt cho UI layer.
    - `[UI_ATOMIC_STAGE]`: Root Canvas chính cho toàn bộ giao diện nguyên tử.
    - `[SAFE_AREA]`: Đảm bảo UI hiển thị đúng trên các thiết bị có tai thỏ/màn hình cong.
- Tự động nạp Registry vào GameManager khi khởi tạo.

### 10. Chế tác Asset "Nguyên tử" (Atomic Asset Generation)
- **Quy trình Hybrid**: Kết hợp sức mạnh AI (`generate_image`) và kĩ năng hậu kỳ của Agent để sản xuất Resource sạch 100%.
- **100% Initial Set Success**: Đã hoàn tất 28 assets đạt chuẩn cho bộ HUD nền tảng, hệ thống Cà rốt (5 stages) và Gà (6 stages).
- **Tiêu chuẩn "Glossy 3D Kawaii"**: Thiết lập phong cách UI bóng bẩy, nổi khối, phá bỏ giới hạn của các UI phẳng (flat) truyền thống.
- **Atomic Layering**: Tách riêng Layer Nền (Background) và Layer Icon (Foreground) để tối ưu hóa tính tùy biến trong Unity. Folder chuyên biệt tại `Art/Sprites/UI/`.
- **Chế độ Dự phòng (Prompt Template)**: Xây dựng bộ [Atomic_HUD_Prompt_Library.md](file:///d:/soflware/Unity/Source/NTVV/docs/guides/Atomic_HUD_Prompt_Library.md) chuẩn `[asset type] for [use case]...` để bảo chứng tính nhất quán về góc nhìn (Perspective) và ánh sáng.

---

## 🚀 Định hướng tiếp theo (Roadmap & Backlog)

Dựa trên những gì đã có, dự án có thể mở rộng theo các hướng sau:

### 🌟 Tính năng mới (Backlog)
- **Hệ thống Chế tạo (Crafting)**: Dùng sản phẩm thu hoạch (Lúa mì, Trứng) để làm bánh/thức ăn cao cấp.
- **Hệ thống Bạn bè (Social)**: Ghé thăm nông trại, giúp tưới nước.
- **Thời tiết (Weather)**: Ảnh hưởng đến tốc độ mọc của cây.

### 🔧 Cải tiến Kỹ thuật (Technical Debt)
- **Hệ thống Hội thoại (Dialogue System)**: Hiện tại NPC cần thêm hội thoại dẫn dắt.
- **Âm thanh (Audio)**: Tích hợp âm thanh cho các hành động (Click, Harvest, Feed).
- **Addressables**: Nâng cấp hệ thống Resources lên Addressables để tối ưu memory. Phân nhóm tài nguyên (**Grouping**) theo từng Theme để tối ưu tải động.

---

## 📝 Nhật ký Quản trị
- **2026-04-15**:
    - **UI/Theme System Removal**: Xóa hoàn toàn `UIStyleApplier.cs`, `UIStyleProcessor.cs`, `UIStyleDataSO.cs`, `UIInitializer.cs`. Refactor 6 files liên quan để loại bỏ dependency.
    - **Simplified UI Infrastructure**: `ResourcesUIProvider` load thẳng `UI/Default/{key}`, bỏ 3-tier theme fallback. `PopupManager` không còn inject style khi spawn popup.
    - **Data Manager Cleanup**: Xóa tab "UI/Themes" khỏi `GameDataManagerWindow`. Xóa menu `NTVV/UI/Restore Core Assets`.
    - **MCP Unity Connect**: Kết nối UnityMCP cho NTVV qua `.mcp.json`.
- **2026-04-09**:
    - **Hoàn tất 100% Asset Sản xuất (Bộ 1)**: Đã sản xuất, làm sạch và phân loại 28 assets đạt chuẩn Atomic (Cà rốt 5 stages, Gà 6 stages, Resource Icons, UI Backgrounds).
    - **Phân cấp Thư mục Art chuyên sâu**: Chuyển đổi từ cấu trúc phẳng sang cấu trúc phân loại theo nhóm chức năng (`Backgrounds/`, `Icons/Common/`, `Crops/`, `Animals/`).
- **2026-04-08**:
    - **Visual Standardization (PPU Multiplier)**: Thiết lập bộ thông số chuẩn cho Pixels Per Unit Multiplier (Buttons: 5, Banners: 2.5, Panels: 1.5, Icons: 1).
    - **Structural Rebirth [UI_ATOMIC_STAGE]**: Triển khai Hierarchy UI chuẩn mới với `[UI_CAMERA]` và `[UI_ATOMIC_STAGE]/[SAFE_AREA]`.
    - **Prompt Library Establishment**: Chính thức đóng gói [Atomic_HUD_Prompt_Library.md](file:///d:/soflware/Unity/Source/NTVV/docs/guides/Atomic_HUD_Prompt_Library.md).
    - **Transparent Asset Success**: Sản xuất thành công bộ 5 item đầu tiên (Blue/Green Button, Gold Coin, Lightning, Parchment) đạt chuẩn Transparent & High-fidelity.
- **2026-04-07**: 
    - **Bước ngoặt "Atomic Rebirth"**: Khởi động Epic 7. Tái thiết kế HUD NTVV theo phong cách Glossy 3D. 
    - **Structural Prompting**: Chuẩn hóa ngôn ngữ tạo ảnh AI với cấu trúc Prompt logic nhằm kiểm soát Perspective (Góc nhìn chính diện cho nền, góc Isometric cho Icon).
    - **MCP Mastery**: AI tự hành 100% việc nối dây, tạo Prefab và Styling.
- **2026-04-06**: Hoàn thiện Tự động hóa UI v2. Triển khai **Semantic Labeling** và cơ chế **Self-Repair Renderer**. Khắc phục triệt để lỗi mất Font TMPro (Fix metadata type: 3). Thiết lập phong cách "Dark Chocolate" cho Resource Chip.
- **2026-04-03**: Hoàn tất Epic 6 - Quy hoạch dữ liệu tập trung và Self-Healing Systems.
- **2026-04-02**: Hoàn tất Epic 4 & 5 - Quest System và Persistence.
