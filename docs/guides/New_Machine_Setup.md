# Hướng dẫn Thiết lập Máy tính Mới (New Machine Setup Guide)

Dự án NTVV sử dụng hệ thống AI và Skill tùy chỉnh qua Antigravity. Khi chuyển sang máy tính mới, hãy làm theo các bước sau để đảm bảo AI hoạt động với đầy đủ "trí thông minh" của dự án.

---

## 1. Môi trường Unity
- **Phiên bản**: Đảm bảo cài đúng phiên bản Unity thông qua Unity Hub.
- **Git Clone**: `git clone https://github.com/KietTran612/NTVV.git`
- **Mở Dự án**: Unity sẽ tự động tải lại các Package và nội dung trong `Library/`.

---

## 2. Thiết lập AI (Antigravity/Gemini) - QUAN TRỌNG

### 2.1 Cài đặt MCP Servers
Phần lớn các công cụ thiết kế của AI (đọc file `.pen`, xử lý UI chuyên sâu) nằm trong MCP Server `pencil`. 
> [!IMPORTANT]
> **Hướng dẫn cài đặt Pencil MCP:**
> 1. Mở phần **Settings** trong Antigravity/Gemini.
> 2. Tìm đến mục **MCP Servers**.
> 3. Cài đặt Server có tên là **`pencil`**.
> 4. Kiểm tra trạng thái: Server phải hiển thị trạng thái "Connected" (Đã kết nối).

### 2.2 Đồng bộ Skills & Prompt Keys
Vì thư mục `.agent/` và `.antigravity/` đã được push lên GitHub, AI sẽ tự động đọc được các Skill sau khi bạn `git pull`.
- **Kiểm tra**: Gõ chat `@ui-standardization`. Nếu AI hiển thị mô tả về kĩ năng chuân hóa UI, nghĩa là AI đã nhận diện được folder `.agent/skills/`.

---

## 3. Kiểm tra Tính Năng
Sau khi setup xong, hãy yêu cầu AI thực hiện một lệnh kiểm tra đơn giản:
> "Kiểm tra skill ui-standardization và báo cáo tình trạng các file .pen đang mở"

Nếu AI trả lời chi tiết, bạn đã hoàn tất quá trình setup!

---

## 4. Troubleshooting (Xử lý sự cố)
- **AI không tìm thấy Skill**: Chắc chắn rằng bạn đã gõ đúng tên `@ui-standardization` và file `.agent/skills/ui-standardization/SKILL.md` tồn tại trong thư mục dự án.
- **Lỗi Pencil MCP**: Nếu AI không thể sửa file `.pen`, hãy kiểm tra lại cài đặt MCP Server trong phân Settings.
