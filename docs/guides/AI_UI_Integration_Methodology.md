# Định hướng Tích hợp & Xử lý UI bằng AI (AI UI Integration Methodology)

Tài liệu này đóng vai trò như một **Bản ghi nhớ Phương pháp luận (Methodology Memo)** dành cho các AI Agent (hoặc kỹ sư con người xem lại) khi nhận tác vụ liên quan đến việc bóc tách dữ liệu UI từ các bản thiết kế phức tạp (Figma/Pencil) và chuyển hóa chúng thành Unity Prefab một cách tự động, chính xác.

---

## 1. Vấn đề "Mảnh vỡ Dữ liệu" trong file đồ họa lớn
Khi xử lý một file mockup `.pen` chứa toàn bộ các màn hình và vô số Component của game, Agent sẽ không thể cập nhật nhắm mục tiêu một thành phần (vd. `ShopEntry`) nếu quét toàn bộ file gốc. Việc đọc toàn bộ file `pen` cùng lúc có thể gây tràn Response Context.

## 2. Chiến lược Định vị Dữ liệu chéo (Cross-Referencing)

Để AI có thể nhắm trúng vạch đích mà không cần lập trình viên con người chỉ tay, quy trình định vị sẽ sử dụng kết hợp 2 nguồn dữ liệu: **Docs Information** và **MCP Pencil Search**.

### Bước 2.1: Truy cập Kho tài liệu Đặc tả (`docs\document_md`)
- Thư mục `docs\document_md\` (đặc biệt là các file như `farm_game_ui_component_spec_unity_handoff...`) lưu trữ tên gọi chính thức, mô tả cấu trúc, mã màu của các UI Prefab.
- **Hành động của AI:** Trước khi làm UI, Agent bắt buộc phải dùng lệnh tìm kiếm (grep/view_file) vào tài liệu này để tra tên gốc của linh kiện (Vd: *"Resource Chip"*).

### Bước 2.2: Dò quét định vị bằng Regex (Pencil MCP)
- Sau khi có được tên gốc (hoặc cụm từ khoá), AI sử dụng tool `mcp_pencil_batch_get` từ server Pencil với tham số truy vấn mẫu `patterns: [{ "name": ".*Resource Chip.*" }]`.
- **Hành động của AI:** Điều này giúp AI bóc chính xác cái Node/ID của linh kiện đó trong file đồ hoạ khổng lồ, bỏ qua phần còn lại. Đạt được độ tự chủ 100%.

> **[THỎA THUẬN QUAN TRỌNG VỚI NGƯỜI DÙNG]**
> Xin đảm bảo rằng **Tên Frame / Component** bên trong file mockup `.pen` được đặt CHUẨN XÁC hoặc chứa từ khoá đồng nhất với tên gọi cất giữ tại `docs\document_md`. Đây là chìa khóa duy nhất để thuật toán Regex Match của AI bắt trúng mục tiêu.

---

## 3. Quy trình 3 Bước Cốt lõi (3-Step Skill Pipeline)

Hệ thống NTVV vận hành dựa trên 3 Skill chuyên biệt để đảm bảo tính nhất quán từ thiết kế đến thực tế. Mọi thành viên (AI Agent) phải tuân thủ nghiêm ngặt lộ trình này:

1. **Phân tích Blueprint (`@ui-blueprinting`)**:
   - **Mục tiêu**: Bóc tách layout, màu sắc và font chữ từ Mockup `.pen`.
   - **Thực thi**: Agent sử dụng `mcp_pencil_batch_get` để lấy thông số kỹ thuật (Hex, Padding, Spacing) và lập bản thiết kế chi tiết (Blueprint).

2. **Chuẩn hoá & Nối dây (`@ui-standardization`)**:
   - **Mục tiêu**: Xây dựng Hierarchy chuẩn và kết nối linh kiện vào Controller C#.
   - **Hành động (Pure MCP)**: Agent trực tiếp sử dụng lệnh `gameobject-create` để xây dựng cấu trúc và `object-modify` để nối dây các SerializedField trong Inspector. Tuyệt đối không dùng các script tự động "đoán mò".

3. **Trang trí & Styling (`@ui-visual-styling`)**:
   - **Mục tiêu**: Áp dụng thẩm mỹ (Skins) lên Prefab mà không làm hỏng logic.
   - **Dữ liệu**: Sử dụng `UIStyleDataSO.asset` làm nguồn Palette và Font duy nhất.
   - **Thực thi**: Agent gán nhãn `UIStyleApplier` và thực hiện lệnh "Bake" (gán Material Dosis, Sprite, Color) trực tiếp vào Prefab thông qua các lệnh MCP.

---

## 4. Cấu trúc Root UI [UI_ATOMIC_STAGE]

Để đảm bảo tính nhất quán và hỗ trợ hiển thị trên đa thiết bị, Agent phải tuân thủ cấu trúc Hierarchy Root sau khi thiết lập một Scene mới hoặc một hệ thống UI lớn:

1.  **`[UI_CAMERA]`**: Camera chuyên dụng cho lớp UI. Sử dụng Render Mode: `Screen Space - Camera` trên Canvas để đảm bảo các thành phần 3D/nổi khối hiển thị chính xác.
2.  **`[UI_ATOMIC_STAGE]`**: GameObject chứa Canvas chính. Đây là trung tâm quản lý toàn bộ giao diện của giai đoạn "Atomic Rebirth".
3.  **`[SAFE_AREA]`**: Con trực tiếp của Stage, sử dụng script để tự động điều chỉnh RectTransform tránh các vùng "tai thỏ" (notch) hoặc bo góc của thiết bị di động.

---

## 5. Tiêu chuẩn Visual (Visual Standards)

Để đảm bảo HUD hiển thị sắc nét trên mọi độ phân giải màn hình, Agent bắt buộc phải thiết lập thông số **Pixels Per Unit Multiplier** trong Image Component của Unity theo các tiêu chuẩn sau:

| UI Element Type | PPU Multiplier Standard | Ghi chú |
| :--- | :--- | :--- |
| **Buttons & Chips** | `5.0` | Đảm bảo chi tiết cho các nút tương tác nhỏ. |
| **Header Banners** | `2.5` | Cân bằng giữa độ nét và kích thước texture. |
| **Main Panels & Popups** | `1.5` | Tối ưu cho các mảng nền lớn 9-slice. |
| **Icons (Foreground)** | `1.0` | Giữ nguyên tỉ lệ pixel gốc của icon. |

---

## 6. Cấu trúc Thư mục & Đóng gói Asset (Organization)

Để hệ thống tự động hóa (Style Applier) tìm thấy linh kiện, toàn bộ Asset sản xuất bởi AI phải được phân loại nghiêm ngặt:

- **Root Art Path**: `Assets/_Project/Art/Sprites/UI/`
- **Backgrounds/**: Chứa các tấm nền Panel, Banner, Button Backgrounds (Orthographic view).
- **Icons/**: Chứa các linh kiện trang trí, icon thông tin (Isometric 3D view).
  - `Common/`: Tài nguyên dùng chung (Gold, Gem, XP, Energy...).
  - `Crops/`: Phân nhóm theo loại cây (Ví dụ: `Carrot/Stage0-3`).
  - `Animals/`: Phân nhóm theo vật nuôi (Ví dụ: `Chicken/Stage1-3`).

---
*(Tài liệu này được cập nhật làm dấu mốc cho việc hoàn thiện 100% Asset bộ khởi động NTVV. Cập nhật lần cuối: 2026-04-09).*
