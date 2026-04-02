# Project Master Status: NTVV - Nông Trại Vui Vẻ

Tài liệu này tổng hợp toàn bộ tiến độ dự án, kết nối các bản kế hoạch chi tiết (`docs/plans`) và định hướng các bước tiếp theo.

---

## 📊 Tổng quan Tiến độ (Epic Progress)

| Epic | Tên Epic | Trạng thái | Tài liệu Kế hoạch |
| :--- | :--- | :--- | :--- |
| **Epic 1** | Core Framework & Time System | ✅ Hoàn thành | [Core Implementation](file:///d:/soflware/Unity/Source/NTVV/docs/plans/2026-04-02-core-framework-implementation.md) |
| **Epic 2** | UI System (uGUI Overhaul) | ✅ Hoàn thành | [UI Implementation](file:///d:/soflware/Unity/Source/NTVV/docs/plans/2026-04-01-ui-system-implementation.md) |
| **Epic 3** | Shop, Economy & Animal Pen | ✅ Hoàn thành | [Economic Implementation](file:///d:/soflware/Unity/Source/NTVV/docs/plans/2026-04-02-Epic4-Shop-Economy-Implementation.md) |
| **Epic 4** | Quest System & Editor Tools | ✅ Hoàn thành | [Quest Task Tracker](file:///d:/soflware/Unity/Source/NTVV/docs/plans/task_quest_system.md) |
| **Epic 5** | Persistence & Sync | ✅ Hoàn thành | [Persistence Design](file:///d:/soflware/Unity/Source/NTVV/docs/plans/2026-04-02-enhanced-persistence-test-design.md) |

---

## 🛠 Chi tiết các Hệ thống đã làm được

### 1. Trồng trọt (Crops)
- Logic trồng, lớn theo thời gian (Tick), gặt hái.
- Tự động hóa việc tạo dữ liệu qua Editor.

### 2. Kinh nghiệm & Cấp độ (Leveling)
- Hệ thống XP, Milestone, tự động Level Up.
- Rào cản Level cho Cửa hàng và Nâng cấp Kho.

### 3. Kinh tế (Economy) & Cửa hàng (Shop)
- Quản lý Vàng (Gold), mua hạt giống/vật nuôi.
- Phân chia Tab Shop (Hạt giống, Thú nuôi).

### 4. Chăn nuôi (Animal & Pens)
- Vòng đời thú, Cho ăn, Thu hoạch sản phẩm.
- Nâng cấp sức chứa chuồng trại.

### 5. Nhiệm vụ (Quest)
- Nhận nhiệm vụ từ NPC, theo dõi tiến độ qua sự kiện (Events).
- Thưởng Vàng, XP và Mở khóa tính năng.

### 6. Quản trị Dữ liệu (Editor Tools)
- **Data Manager Window**: Chỉnh sửa tập trung cho toàn bộ project.
- **Sample Generator**: Tạo nhanh dữ liệu chuẩn để Playtest.
- **Auto-Sync**: Tự động đăng ký Quest vào Registry.

---

## 🚀 Định hướng tiếp theo (Roadmap & Backlog)

Dựa trên những gì đã có, dự án có thể mở rộng theo các hướng sau:

### 🌟 Tính năng mới (Backlog)
- **Hệ thống Chế tạo (Crafting)**: Dùng sản phẩm thu hoạch (Lúa mì, Trứng) để làm bánh/thức ăn cao cấp.
- **Hệ thống Bạn bè (Social)**: Ghé thăm nông trại, giúp tưới nước.
- **Thời tiết (Weather)**: Ảnh hưởng đến tốc độ mọc của cây.
- **Nghề nghiệp (Skills)**: Chuyên môn hóa về Trồng trọt hoặc Chăn nuôi.

### 🔧 Cải tiến Kỹ thuật (Technical Debt)
- **Hệ thống Hội thoại (Dialogue System)**: Hiện tại NPC mới chỉ giao Quest trực tiếp, cần thêm hội thoại dẫn dắt.
- **Âm thanh (Audio)**: Tích hợp âm thanh cho các hành động (Click, Harvest, Feed).
- **Hiệu ứng (VFX)**: Thêm Particle khi cây lớn hoặc khi thu hoạch.

---

## 📝 Nhật ký Quản trị
Mỗi khi bắt đầu một Epic mới, hãy tạo thêm một file `task_*.md` trong `docs/plans` và cập nhật vào bảng **Epic Progress** ở trên.
