# 🚀 Giao thức Khởi động Phiên làm việc: Hệ thống UI NTVV

**Dành cho AI Agent**: "Chào bạn đồng nghiệp! Đây là hướng dẫn nhanh để bạn hiểu ngay bộ máy chúng ta đang vận hành trước khi bắt đầu thiết kế UI."

---

## 1. 📂 Các file "Đầu não" bạn CẦN đọc ngay (Theo thứ tự)

Hãy dùng công cụ `view_file` để đọc các file này theo đúng thứ tự để nắm bắt kiến trúc:

1.  **`docs/guides/AI_UI_Integration_Methodology.md`**: Cẩm nang thiết yếu về quy trình Manual MCP Styling.
2.  **`Assets/_Project/Data/UI/Styles/UIStyleDataSO.asset`**: Nguồn sự thật duy nhất về Theme (Màu, Font, Sprite).
3.  **`docs/HANDOVER.md`**: Để biết trạng thái chính xác của các Epic và Context của dự án.

---

## 2. 🏗 Tóm tắt Kiến trúc UI 2 Lớp (Two-Layer UI)

Chúng ta đang vận hành hệ thống UI theo mô hình tách biệt hoàn toàn Logic và Visual:

| **Architect** (Analysis) | Phân tích Mockup/Ảnh/Mô tả & Gợi ý **Unity Layout Component** (Responsive) | Xuất **Blueprint** MD | `@ui-blueprinting` |
| **Functional** (Logic) | Controller, Button, Text hiển thị data | Hậu tố: `_Label`, `_Button` | `@ui-standardization` |
| **Decorator** (Visual) | Nền, Bóng đổ, Viền, Hiệu ứng | Tiền tố: `bg_`, `shadow_` | `@ui-visual-styling` |

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

### 4. Art & Artistic Assets (Tài nguyên Hình ảnh)
- **Mockups (Pencil Files)**: `Design/*.pen` (Sử dụng công cụ `pencil` để đọc các file này).
- **UI Sprites (Icons/Panels)**:
  - `Assets/_Project/Art/Sprites/`
  - `Assets/_Project/Textures/UI/`
  - `Assets/_Project/UI/System/` (Các icon hệ thống).
- **UI Textures**: `Assets/_Project/Textures/`

### 5. Typography (Phông chữ)
- **Master Font**: **Dosis** (Bold/ExtraBold).
- **Material Presets**: Sử dụng các bản Outline màu (Green, Blue, Yellow, Red) để phù hợp ngữ cảnh.
- **Asset Path**: `Assets/_Project/Fonts/Dosis/`
- *Tất cả text phải sử dụng TextMeshPro với Dosis SDF.*

---

## 3. 🎯 Mục tiêu của Phiên làm việc mới

Dự án đã có khung xương và hệ thống dán nhãn tự động cực mạnh. Phiên sau sẽ tập trung vào:
1.  **Thay thế Asset Thật**: Cập nhật các Sprite thực tế từ họa sĩ vào `DefaultFarmStyle.asset`.
2.  **Mở rộng Popup**: Hoàn thiện chi tiết Visual cho `QuestDetailPanel` và `AnimalDetailPanel`.
3.  **Hệ thống Icon động**: Tự động đổi Icon Resource (Gold, Gem) dựa trên dữ liệu thật thay vì Placeholder.

**Luồng làm việc (Workflow) chuẩn Pure MCP:**
1.  **Bước 1**: Nhận mockup, dùng `mcp_pencil_batch_get` dò mã định danh UI.
2.  **Bước 2**: Tự tay xây dựng Hierarchy qua MCP (`gameobject-create`).
3.  **Bước 3**: Gán nhãn `UIStyleApplier` và thực hiện Bake từ `UIStyleDataSO` thông qua các lệnh `component-modify`.

---

## 💡 Lời nhắn cuối cho AI:
"Hãy luôn dùng công cụ `pencil` để kiểm tra các file `.pen` nếu có thiết kế UI sẵn có trong đó. Dự án này rất khắt khe về **Naming Convention** — tuyệt đối không sáng tạo tên khác ngoài các hậu tố/tiền tố đã quy định."
