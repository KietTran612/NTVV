# Hướng dẫn Thiết lập Máy tính Mới (New Machine Setup Guide)

Dự án NTVV sử dụng hệ thống AI và Skill tùy chỉnh qua Antigravity. Khi chuyển sang máy tính mới, hãy làm theo các bước sau để đảm bảo AI hoạt động với đầy đủ "trí thông minh" của dự án.

---

## 1. Môi trường Unity
- **Phiên bản**: Đảm bảo cài đúng phiên bản Unity thông qua Unity Hub.
- **Git Clone**: `git clone https://github.com/KietTran612/NTVV.git`

---

## 2. Thiết lập AI (Antigravity/Gemini) - QUAN TRỌNG

### 2.1 Cài đặt MCP Servers
Phần lớn các công cụ thao tác của AI và kết xuất đồ họa đều chạy thông qua MCP Servers.
> [!IMPORTANT]
> **Hướng dẫn cài đặt Core MCPs:**
> 1. Mở phần **Settings** trong Antigravity/Gemini -> **MCP Servers**.
> 2. Cài đặt Server **`pencil`**: (Công cụ mổ xẻ file Mockup UI `.pen`).
> 3. Cài đặt Server **`ai-game-developer`**: (Công cụ kết nối Unity Editor API và sinh lệnh C# `unity-skill-create`).
> 4. Kiểm tra: Cả 2 Server phải hiển thị "Connected".

### 2.2 Đồng bộ Skills & Prompt Keys
Vì thư mục `.agent/` và `.antigravity/` đã được push lên GitHub, AI sẽ tự động đọc được các Skill sau khi bạn `git pull`.
- **Kiểm tra**: Gõ chat `@ui-blueprinting`, `@ui-standardization` hoặc `@ui-visual-styling`. Nếu AI hiển thị mô tả về kĩ năng tương ứng, nghĩa là AI đã nhận diện được folder `.agent/skills/`.

---

## 3. Kiểm tra Tính Năng
Sau khi setup xong, hãy yêu cầu AI thực hiện một lệnh kiểm tra đơn giản:
> "Kiểm tra skill ui-standardization và báo cáo tình trạng các file .pen đang mở"

Nếu AI trả lời chi tiết, bạn đã hoàn tất quá trình setup!

---

## 4. Troubleshooting (Xử lý sự cố)
- **AI không tìm thấy Skill**: Chắc chắn rằng bạn đã gõ đúng tên `@ui-blueprinting`, `@ui-standardization` hoặc `@ui-visual-styling` và các file `.agent/skills/[skill-name]/SKILL.md` tồn tại trong thư mục dự án.
- **Lỗi Pencil MCP**: Nếu AI không thể sửa file `.pen`, hãy kiểm tra lại cài đặt MCP Server trong phân Settings.
