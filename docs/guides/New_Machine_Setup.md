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
> 5. **Kỹ năng AI Image Generation**: Kích hoạt khả năng `generate_image` để phục vụ quy trình Structural Prompting cho HUD.

### 2.2 Đồng bộ Skills & Quy trình Pure MCP
Vì thư mục `.agent/` và `.antigravity/` đã được push lên GitHub, AI sẽ tự động đọc được các hướng dẫn sau khi bạn `git pull`.
- **Kiểm tra (Summoning)**: Bạn có thể gõ chat `@ui-blueprinting`, `@ui-standardization` hoặc `@ui-visual-styling`. Nếu AI hiển thị mô tả kỹ năng, nghĩa là hệ thống đã nhận diện được folder `.agent/skills/`.
- **Dữ liệu**: Mọi thông số thẩm mỹ rút ra từ `Assets/_Project/Data/UI/Styles/UIStyleDataSO.asset`. 
- **Verify PPU**: Kiểm tra rằng các thông số **Pixels Per Unit Multiplier** trong UIStyleData (hoặc gán tay vào Image con) tuân thủ: Buttons=5, Banners=2.5, Panels=1.5, Icons=1.

---

## 3. Kiểm tra Tính Năng
Sau khi setup xong, hãy yêu cầu AI thực hiện một lệnh kiểm tra đơn giản:
> "Kiểm tra tình trạng 3 Skill UI chính và mô tả ngắn gọn quy trình làm việc của mỗi skill"

Nếu AI trả lời chi tiết, bạn đã hoàn tất quá trình setup!

---

## 4. Troubleshooting (Xử lý sự cố)
- **AI không tìm thấy Skill**: Chắc chắn rằng các file `.agent/skills/[skill-name]/SKILL.md` tồn tại trong thư mục dự án.
- **Lỗi Pencil MCP**: Nếu AI không thể đọc file `.pen`, hãy kiểm tra lại cài đặt MCP Server trong phần Settings.
- **Lỗi Font**: Đồ hoạ yêu cầu Font Dosis. Hãy đảm bảo TMPro Font Asset `Dosis-ExtraBold SDF` đã được cài đặt đúng.
