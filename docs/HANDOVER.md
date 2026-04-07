# Project Handover - NTVV (Nông Trại Vui Vẻ)

Tài liệu này dùng để đồng bộ nhanh "suy nghĩ" của AI Agent khi bạn chuyển sang máy tính mới hoặc bắt đầu một phiên làm việc mới.

## 🧠 Bối cảnh Phiên làm việc (Session Context)
- **Phiên 07/04/2026 (Hiện tại)**:
    - Nâng cấp **Toàn diện Hệ thống Tự Động Hóa UI bằng MCP**: Ép buộc hệ thống sử dụng thư viện MCP `unity-skill-create` để sinh ra Script hoạt động ngầm. Chấm dứt kỷ nguyên "AI bảo lập trình viên bấm tay".
    - Ra mắt **Phương pháp luận tích hợp mới**: Công bố cẩm nang định vị chéo dữ liệu UI đồ họa khổng lồ thông qua `pencil.batch_get` regex và tham chiếu doc gốc.
- **Phiên 06/04/2026**: 
    - Hoàn thiện **Automated Semantic Labeling** trên PrefabAssembler.
    - Ra mắt quy trình UI 1-Click thủ công và Kiến trúc UI 2 lớp (Functional/Decorator).
- **Phiên 03/04/2026**: Hoàn tất quy hoạch lại cấu trúc dữ liệu (**Centralized Data Architecture**). Triển khai cơ chế **Self-Healing** giúp hệ thống tự động kết nối dữ liệu cấu hình.

## 🗺 Bản đồ Hệ thống (System Map)
Nếu bạn mở dự án ở máy tính khác, AI hãy chú ý các file "đầu não" mới:
1.  **Dữ liệu Tập trung & Cấu hình**: `GameDataRegistry.asset` và thư mục `Assets/_Project/Data/Configs/`.
2.  **Tiêu chuẩn Tự động hóa UI (Architect, Builder, Stylist) qua MCP**:
    - `docs/guides/AI_UI_Integration_Methodology.md`: Kim chỉ nam bắt buộc về chiến lược tìm kiếm và tích hợp UI.
    - `.agent/skills/ui-blueprinting/SKILL.md`: Kiến trúc sư - Dùng `mcp_pencil_batch_get` thu thập dữ liệu và quét Prefab.
    - `.agent/skills/ui-standardization/SKILL.md`: Thợ xây - Gọi `unity-skill-create` bọc PrefabAssembler tạo Cấu trúc & Nối dây hoàn toàn AI.
    - `.agent/skills/ui-visual-styling/SKILL.md`: Thợ sơn - Dùng script-execute đổ Visual qua mã nguồn (Bake tự động).
3.  **Hướng dẫn cài đặt máy mới**: `docs/guides/New_Machine_Setup.md` (Chú ý MCP `ai-game-developer` và `pencil`).

## 🎯 Trạng thái hiện tại & Bước tiếp theo
- **Đã xong**: Quy hoạch dữ liệu, Self-Healing, và Kiến trúc UI Tự động hóa 100% bằng Unity MCP. Hoàn tất cẩm nang tích hợp UI chéo.
- **Cần làm ngay**: 
    1. Yêu cầu Agent thực thi tác vụ UI tự động mới để kiểm tra dòng lệnh `unity-skill-create`.
    2. Đọc file `docs/guides/AI_UI_Integration_Methodology.md` để nắm quy trình truy vấn dữ liệu từ file `.pen`.
    3. Thử nghiệm đổi Theme từ `Default` sang `Cartoon` để xem Decorator Layer hoạt động mượt mà ra sao.

---

> [!TIP]
> **Dành cho AI**: "Chào người bạn AI mới! Hãy bắt đầu bằng cách đọc `.agent/skills/ui-standardization/SKILL.md`,`.agent/skills/ui-visual-styling/SKILL.md` để nắm được quy tắc xây dựng UI. Sau đó xem mục **🛡️ Chiến lược Thiết kế Prefab theo Theme** trong `docs/guides/System_Full_Guide.md` để biết cách tạo Variant đúng chuẩn và tối ưu cho Addressables."
