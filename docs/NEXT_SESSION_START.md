# 🚀 Giao thức Khởi động Phiên làm việc: Hệ thống UI NTVV

**Dành cho AI Agent**: "Chào bạn đồng nghiệp! Đây là hướng dẫn nhanh để bạn hiểu ngay bộ máy chúng ta đang vận hành trước khi bắt đầu thiết kế UI."

---

## 1. 📂 Các file "Đầu não" bạn CẦN đọc ngay (Theo thứ tự)

Hãy dùng công cụ `view_file` để đọc các file này theo đúng thứ tự để nắm bắt kiến trúc:

1.  **`.agent/skills/ui-standardization/SKILL.md`**: Quy tắc xây dựng cấu trúc (Functional Layer) và nối dây Auto-Wiring.
2.  **`.agent/skills/ui-visual-styling/SKILL.md`**: Quy tắc trang trí Visual (Decorator Layer) qua ScriptableObject.
3.  **`docs/plans/2026-04-06-ui-visual-styling-design.md`**: Bản thiết kế chi tiết về hệ thống Styling v2 và cơ chế an toàn của `PrefabAssembler`.
4.  **`docs/HANDOVER.md`**: Để biết trạng thái chính xác của các Epic và Context của dự án.

---

## 2. 🏗 Tóm tắt Kiến trúc UI 2 Lớp (Two-Layer UI)

Chúng ta đang vận hành hệ thống UI theo mô hình tách biệt hoàn toàn Logic và Visual:

| Lớp (Layer) | Đối tượng quản lý | Quy tắc đặt tên | Skill tương ứng |
| :--- | :--- | :--- | :--- |
| **Functional** (Logic) | Controller, Button, Text hiển thị data | Hậu tố: `_Label`, `_Icon`, `_Button` | `@ui-standardization` |
| **Decorator** (Visual) | Nền, Bóng đổ, Viền, Hiệu ứng | Tiền tố: `bg_`, `shadow_`, `border_`, `overlay_` | `@ui-visual-styling` |

**Quy tắc An toàn:**
- `PrefabAssembler.cs` hoạt động theo cơ chế **"Create or Verify"**: Không bao giờ ghi đè lớp Decorator nếu Prefab đã tồn tại.
- Bạn có thể thoải mái chạy lại Tool Assembler để sửa lỗi nối dây mà không lo mất thiết kế hình ảnh.

---

## 📂 Quy hoạch Folder & Tổ chức Tài nguyên

Để đảm bảo hệ thống tìm kiếm và nạp tài nguyên tự động hoạt động đúng, bạn CẦN tuân thủ cấu trúc thư mục sau:

### 1. UI Prefabs (Giao diện)
- **Gốc**: `Assets/_Project/Resources/UI/Default/`
- **Theme (Variant)**: `Assets/_Project/Resources/UI/[ThemeName]/` (Ví dụ: `.../UI/Cartoon/`)

### 2. C# Scripts (Logic)
- **High-level Panels**: `Assets/_Project/Scripts/UI/Panels/` (Shop, Inventory, Quest)
- **Atomic Components**: `Assets/_Project/Scripts/UI/Common/` (Button, ProgressBar, Chip)
- **Styling System**: `Assets/_Project/Scripts/UI/Common/` (`UIStyleApplier.cs`, `UIStyleDataSO.cs`)

### 3. Data & Styles (Dữ liệu trang trí)
- **Style Data (SO)**: `Assets/_Project/Data/UI/Styles/[ThemeName]/`
  - *Mỗi prefab nên có 1 file StyleData tương ứng (ví dụ: `ShopEntry_StyleData.asset`).*
- **Hệ thống Registry**: `Assets/_Project/Data/Registry/` (Trái tim dữ liệu tĩnh).
- **Cấu hình Hệ thống**: `Assets/_Project/Data/Configs/` (Level, Storage, AnimalPen).

---

## 3. 🎯 Mục tiêu của Phiên làm việc mới

Người dùng muốn bắt đầu **tạo các Prefab thực tế** (như ShopEntry, InventorySlot) và **thiết kế UI** cho chúng.

**Luồng làm việc (Workflow) dành cho bạn:**
1.  **Bước 1**: Dùng `@ui-standardization` để xây cấu trúc thô và nối dây logic cho prefab.
2.  **Bước 2**: Yêu cầu người dùng cung cấp mô tả UI hoặc Mockup hình ảnh.
3.  **Bước 3**: Dùng `@ui-visual-styling` để phân tích ảnh/mô tả, tạo các object `bg_`, `shadow_` và sinh file `StyleData.asset`.
4.  **Bước 4**: Gắn `UIStyleApplier` và nhấn **Apply Style to Prefab NOW** để hoàn thiện.

---

## 💡 Lời nhắn cuối cho AI:
"Hãy luôn dùng công cụ `pencil` để kiểm tra các file `.pen` nếu có thiết kế UI sẵn có trong đó. Dự án này rất khắt khe về **Naming Convention** — tuyệt đối không sáng tạo tên khác ngoài các hậu tố/tiền tố đã quy định."
