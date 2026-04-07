# Project Master Status: NTVV - Nông Trại Vui Vẻ

Tài liệu này tổng hợp toàn bộ tiến độ dự án, kết nối các bản kế hoạch chi tiết (`docs/plans`) và định hướng các bước tiếp theo.

---

## 📊 Tổng quan Tiến độ (Epic Progress)

| Epic | Tên Epic | Trạng thái | Tài liệu Kế hoạch |
| :--- | :--- | :--- | :--- |
| **Epic 1** | Core Framework & Time System | ✅ Hoàn thành | [Core Implementation](file:///d:/soflware/Unity/Source/NTVV/docs/plans/2026-04-02-core-framework-implementation.md) |
| **Epic 2** | UI System (Architect, Builder, Stylist) | ✅ Hoàn thành | [UI Styling Design](file:///d:/soflware/Unity/Source/NTVV/docs/plans/2026-04-06-ui-visual-styling-design.md) |
| **Epic 3** | Shop, Economy & Animal Pen | ✅ Hoàn thành | [Economic Implementation](file:///d:/soflware/Unity/Source/NTVV/docs/plans/2026-04-02-Epic4-Shop-Economy-Implementation.md) |
| **Epic 4** | Quest System & Editor Tools | ✅ Hoàn thành | [Quest Task Tracker](file:///d:/soflware/Unity/Source/NTVV/docs/plans/task_quest_system.md) |
| **Epic 5** | Persistence & Sync | ✅ Hoàn thành | [Persistence Design](file:///d:/soflware/Unity/Source/NTVV/docs/plans/2026-04-02-enhanced-persistence-test-design.md) |
| Epic 6 | Centralized Data & Scene Automation | ✅ Hoàn thành | [Data Architecture Plan](file:///C:/Users/Hoang.H/.gemini/antigravity/brain/c562547a-dba6-474c-8d7a-0a5a61cb6538/implementation_plan.md) |
| **Epic 7** | Refined UI & Content Exp. | ⏳ Sắp tới | - |

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
- **Repair Tool**: Tự động sửa lỗi liên kết các config bị thiếu/mất.
- **Data Manager**: Chỉnh sửa tập trung cho Crops, Animals, Quests, Themes.

### 7. Kiến trúc UI 3 Bước Tự Hành AI (Architect, Builder, Stylist)
- **Quy trình 3 Giai đoạn MCP Tự Động Hóa 100%**:
    1.  **Stage 1: Architect (`ui-blueprinting`)**: AI tự quét file .pen qua `batch_get`, tìm kiếm Prefab qua mã nguồn Unity để vẽ Blueprint.
    2.  **Stage 2: Builder (`ui-standardization`)**: AI tận dụng `unity-skill-create` tạo Script C# tự lắp ráp khung sương Functional, đánh `Semantic Labeling` tự động mà không bắt con người Click chuột.
    3.  **Stage 3: Stylist (`ui-visual-styling`)**: AI tự động tạo mã Bake thông qua Tool C# nạp ScriptableObject, đổ dữ liệu tự động thẳng qua API `Object.ApplyStyle`.
- **Sức mạnh của PrefabAssembler (v2.1)**: 
    - Cơ chế **"Create or Verify"**: Bảo vệ 100% thiết kế visual khi chạy lại tool để cập nhật code logic. Tự động sửa lỗi (Repair) các liên kết bị đứt gãy.
    - **Self-Repair**: Tự động thêm `CanvasRenderer` cho UI.
- **Tiêu chuẩn Typography**: Sử dụng bộ font **Dosis** (Bold/ExtraBold). Asset Font Type 3.
- **Kết quả**: Hệ thống tách biệt Decorator và Logic, quy trình từ Mockup vào Game được AI thao tác kín vòng lặp (Closed-Loop).

### 8. Tự động hóa Scene (Scene Automation)
- Công cụ **Setup Full Game Scene** 1-Click tạo scene gameplay hoàn chỉnh.
- Tự động nạp Registry vào GameManager khi khởi tạo.

### 9. Kiến trúc Dữ liệu Tập trung (Data Architecture)
- Chuyển đổi từ "Kéo thả thủ công" sang **Registry-Driven**.
- Cấu trúc thư mục dữ liệu phẳng, nhất quán và khoa học.

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
- **2026-04-07**: Nâng cấp Toàn quyền AI tự hành bộ hệ thống 3 Tác vụ UI (Architect, Builder, Stylist). Agent bắt buộc tự code ngầm Tool MCP C# sinh ra Editor action. Tạo tài liệu Giao thức bóc tách `pencil` kết hợp chéo với `document_md`.
- **2026-04-06**: Hoàn thiện Tự động hóa UI v2. Triển khai **Semantic Labeling** và cơ chế **Self-Repair Renderer**. Khắc phục triệt để lỗi mất Font TMPro (Fix metadata type: 3). Thiết lập phong cách "Dark Chocolate" cho Resource Chip.
- **2026-04-03**: Hoàn tất Epic 6 - Quy hoạch dữ liệu tập trung và Self-Healing Systems.
- **2026-04-02**: Hoàn tất Epic 4 & 5 - Quest System và Persistence.
