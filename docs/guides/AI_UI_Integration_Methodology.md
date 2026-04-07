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

## 3. Hệ thống Hóa - Tự động hóa UI 3 Lớp (UI Automation Tiers)

Đó là cách AI thu thập input đầu vào. Còn về phần nhào nặn ra sản phẩm đầu ra (Unity Prefab), hệ thống NTVV tuân theo quy chuẩn tự động hóa 3 lớp (3 UI Skills cốt lõi) được cấp quyền điều khiển MCP:

1. **`@ui-blueprinting` (Khảo sát)** 
   Sử dụng lệnh `assets-prefab-open` hoặc `gameobject-find` để tàng hình quét cấu trúc gốc rễ của Prefab cũ đang ở trong Unity, kết hợp với Mockup .pen mới để vạch ra thiết kế Cấu trúc Node (Blueprint). Không đoán mò.
2. **`@ui-standardization` (Chuẩn hóa Cấu trúc xương / Controller)**
   Tuyệt đối không bắt người dùng click nối dây. AI phải gọi công cụ mã C# sẵn có (`mcp-tool: ui-prefab-assemble`) để chẩn đoán hệ thống tên, tự động Set Reference (SerializedField) qua Inspector. (Nếu tool C# chưa tồn tại, AI phải nảy số sử dụng lệnh `unity-skill-create` sinh ra tool đó luôn).
3. **`@ui-visual-styling` (Baking Visual & Decorator)**
   Mọi hiệu ứng thị giác (Sprites, Mũi tô màu, Thay đổi Dosis Font) phải được tiêm tự động. Dùng `unity-skill-create` để sinh lệnh Fake-Bake (chuyển các tham số thành `UIStyleDataSO`) và nạp nó vào `UIStyleApplier.ApplyStyle()` và bắt API Unity tự lưu lại Prefab (SaveAsPrefabAsset). 

---
*(Tài liệu này được tạo ra làm dấu mốc cho việc chuyển giao từ AI "Tư vấn" sang AI "Tự vận hành toàn trình" trong dự án NTVV. Cập nhật lần cuối: 2026-04-07).*
