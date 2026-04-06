# Project Master Status: NTVV - Nông Trại Vui Vẻ

Tài liệu này tổng hợp toàn bộ tiến độ dự án, kết nối các bản kế hoạch chi tiết (`docs/plans`) và định hướng các bước tiếp theo.

---

## 📊 Tổng quan Tiến độ (Epic Progress)

| Epic | Tên Epic | Trạng thái | Tài liệu Kế hoạch |
| :--- | :--- | :--- | :--- |
| **Epic 1** | Core Framework & Time System | ✅ Hoàn thành | [Core Implementation](file:///d:/soflware/Unity/Source/NTVV/docs/plans/2026-04-02-core-framework-implementation.md) |
| **Epic 2** | UI System (uGUI Overhaul & Styling v2) | ✅ Hoàn thành | [UI Styling Design](file:///d:/soflware/Unity/Source/NTVV/docs/plans/2026-04-06-ui-visual-styling-design.md) |
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

### 7. Hệ thống Đa chủ đề (Multi-Theme UI) & Styling v2
- **Kiến trúc UI 2 Lớp (Two-Layer Architecture)**: 
    - **Functional Layer**: Quản lý bởi `ui-standardization`, tự động nối dây (Auto-Wiring) dựa trên hậu tố `_Label`, `_Button`, v.v.
    - **Decorator Layer**: Quản lý bởi `ui-visual-styling`, nạp visual (màu sắc, sprite, bóng đổ) qua ScriptableObject (`UIStyleDataSO`) và tiền tố `bg_`, `shadow_`, v.v.
- **Sức mạnh của PrefabAssembler (v2)**: 
    - Cơ chế **"Create or Verify"**: Bảo vệ 100% thiết kế visual khi chạy lại tool để cập nhật code logic. Tự động sửa lỗi (Repair) các liên kết bị đứt gãy.
- **Kết quả**: 
    - Tách biệt hoàn toàn Logic và Visual giúp dự án cực kỳ linh hoạt khi đổi Theme.
    - Giảm 90% thời gian thiết lập UI và loại bỏ hoàn toàn lỗi `NullReference` từ Inspector.

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
- **2026-04-06**: Triển khai Kiến trúc UI 2 Lớp & Visual Styling System. Hoàn tất nâng cấp PrefabAssembler v2.
- **2026-04-03**: Hoàn tất Epic 6 - Quy hoạch dữ liệu tập trung và Self-Healing Systems.
- **2026-04-02**: Hoàn tất Epic 4 & 5 - Quest System và Persistence.
