# 🚀 Giao thức Khởi động Phiên làm việc: Hệ thống UI NTVV

**Dành cho AI Agent**: "Chào bạn đồng nghiệp! Đây là hướng dẫn nhanh để bạn hiểu ngay bộ máy chúng ta đang vận hành trước khi bắt đầu thiết kế UI."

---

## 1. 📂 Các file "Đầu não" bạn CẦN đọc ngay (Theo thứ tự)

Hãy dùng công cụ `view_file` để đọc các file này theo đúng thứ tự để nắm bắt kiến trúc:

1.  **`docs/HANDOVER.md`**: Trạng thái chính xác của các Epic và Context của dự án.
2.  **`docs/guides/AI_UI_Integration_Methodology.md`**: Cẩm nang về quy trình Manual MCP Styling.
3.  **`Assets/_Project/Data/Registry/GameDataRegistry.asset`**: Nguồn sự thật duy nhất về toàn bộ game data.

> ⚠️ **`UIStyleDataSO` đã bị xóa** — Không còn theme asset. Styling làm thủ công qua MCP.

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
- **High-level Panels**: `Assets/_Project/Scripts/UI/Panels/` (Shop, Inventory, Storage, CropAction)
- **Common**: `Assets/_Project/Scripts/UI/Common/` (PopupManager)
- **Infrastructure**: `Assets/_Project/Scripts/UI/Infrastructure/` (IUIAssetProvider, ResourcesUIProvider — load từ `UI/Default/`)

### 3. Data (Dữ liệu game)
- **Hệ thống Registry**: `Assets/_Project/Data/Registry/` (Trái tim dữ liệu tĩnh — Crops, Animals, Quests, Configs).
- **Cấu hình Hệ thống**: `Assets/_Project/Data/Configs/` (Level, Storage, AnimalPen).
- ~~Style Data (SO)~~ — **Đã xóa.** Không còn UIStyleDataSO asset.

### 4. Art & Artistic Assets (Tài nguyên Hình ảnh)
- **Mockups (Pencil Files)**: `Design/*.pen` (Sử dụng công cụ `pencil` để đọc các file này).
- **Kho Art chuẩn Atomic (SPRITES)**: `Assets/_Project/Art/Sprites/UI/`
  - `Backgrounds/`: Các tấm nền Panel, Banner, Button (Orthographic).
  - `Icons/Common/`: Icon tài nguyên hệ thống (Gold, Gem, XP...).
  - `Icons/Crops/`: Icon giai đoạn mọc cây (Ví dụ: `Carrot/Stage0-3`).
  - `Icons/Animals/`: Icon vòng đời thú (Ví dụ: `Chicken/Stage1-3`).
- **Texture Resources**: `Assets/_Project/Textures/`

### 5. Typography (Phông chữ)
- **Master Font**: **Dosis** (Bold/ExtraBold).
- **Material Presets**: Sử dụng các bản Outline màu (Green, Blue, Yellow, Red) để phù hợp ngữ cảnh.
- **Asset Path**: `Assets/_Project/Fonts/Dosis/`
- *Tất cả text phải sử dụng TextMeshPro với Dosis SDF.*

---

## 3. 🎯 Mục tiêu của Phiên làm việc mới

Chúng ta đã có 28 Atomic Assets và codebase đã được dọn sạch styling system. Phiên tới tập trung:
1.  **Logic Wiring (Đấu nối dữ liệu)**:
    - Gán Atomic Sprites vào `CropDataSO` và `AnimalDataSO` thông qua `GameDataRegistry`.
    - Thay thế toàn bộ placeholder cũ bằng bộ Stage 0-3 (Cây) và Stage 1-3 (Thú).
2.  **UI Prototype Assembly**:
    - Lắp ráp HUD Gameplay chính sử dụng Atomic Assets — gán Sprite/Color **trực tiếp** qua MCP.
    - Kiểm tra prefab trong `Resources/UI/Default/` còn UIStyleApplier component không → xóa nếu có.
3.  **Visual Validation**:
    - Thẩm định độ sắc nét (Visual Audit) của 9-slicing trên các tấm nền lớn.
    - Screenshot scene qua `screenshot-game-view` để kiểm tra kết quả thực tế.

**Luồng làm việc (Workflow) chuẩn Pure MCP (cập nhật):**
1.  **Bước 1**: Nhận mockup, dùng `pencil.batch_get` dò mã định danh UI.
2.  **Bước 2**: Xây dựng Hierarchy qua MCP (`gameobject-create`, `component-add`).
3.  **Bước 3**: Gán trực tiếp Sprite, Color, Font vào component qua `component-modify` — **không dùng UIStyleApplier**.

---

## 💡 Lời nhắn cuối cho AI:
"Hãy luôn dùng công cụ `pencil` để kiểm tra các file `.pen` nếu có thiết kế UI sẵn có trong đó. Dự án này rất khắt khe về **Naming Convention** — tuyệt đối không sáng tạo tên khác ngoài các hậu tố/tiền tố đã quy định.

**⚠️ Lưu ý cực quan trọng**: `UIStyleApplier`, `UIStyleDataSO`, `UIStyleProcessor` đã bị **XÓA HOÀN TOÀN** khỏi project (15/04/2026). Nếu AI cũ nào đề xuất dùng các class này — BỎ QUA. Styling hiện tại làm thủ công qua MCP tools."
