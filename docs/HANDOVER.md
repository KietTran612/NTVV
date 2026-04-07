# Project Handover - NTVV (Nông Trại Vui Vẻ)

Tài liệu này dùng để đồng bộ nhanh "suy nghĩ" của AI Agent khi bạn chuyển sang máy tính mới hoặc bắt đầu một phiên làm việc mới.

## 🧠 Bối cảnh Phiên làm việc (Session Context)
- **Phiên 07/04/2026 (Hiện tại)**:
    - **Xác lập Quy trình UI Pure MCP**: Chấm dứt kỷ nguyên tự động hoá "đoán mò" (`PrefabAssembler`). Agent chuyển sang tự tay xây dựng GameObjects và nối dây Reference qua MCP.
    - **Atomic HUD Rebirth**: Khởi động quy trình tái thiết kế HUD theo lớp "Nguyên tử" (Atomic Layers). Sử dụng AI để tự sản xuất Asset cực phẩm (Glossy 3D Kawaii).
    - **Structural Prompting**: Thiết lập tiêu chuẩn Prompting `[asset type] for [use case]...` để kiểm soát chất lượng và góc nhìn (Perspective) đồng nhất.
- **Phiên 06/04/2026**: Hoàn thiện quy trình UI 1-Click cũ (Legacy).
- **Phiên 03/04/2026**: Hoàn tất quy hoạch lại cấu trúc dữ liệu (**Centralized Data Architecture**). Triển khai cơ chế **Self-Healing** giúp hệ thống tự động kết nối dữ liệu cấu hình.

## 🗺 Bản đồ Hệ thống (System Map)
Nếu bạn mở dự án ở máy tính khác, AI hãy chú ý các file "đầu não" mới:
1.  **Dữ liệu Tập trung & Cấu hình**: `GameDataRegistry.asset` và thư mục `Assets/_Project/Data/Configs/`.
2.  **Quy chuẩn UI Pure MCP (Manual AI Execution)**:
    - `docs/guides/AI_UI_Integration_Methodology.md`: Kim chỉ nam mới về chiến lược gán nhãn và styling thủ công.
    - `Assets/_Project/Data/UI/Styles/UIStyleDataSO.asset`: Nguồn sự thật duy nhất về màu sắc/font.
3.  **Hướng dẫn cài đặt máy mới**: `docs/guides/New_Machine_Setup.md` (Chú ý MCP `ai-game-developer` và `pencil`).

## 🎯 Trạng thái hiện tại & Bước tiếp theo
- **Đã xong**: Quy hoạch dữ liệu, Self-Healing, và Kiến trúc UI Tự động hóa 100% bằng Unity MCP. Hoàn tất cẩm nang tích hợp UI chéo.
- **Cần làm ngay**: 
    1. Tiếp tục đúc bộ Asset HUD (Blue Button, Yellow Circle, Scroll, Icons) theo tiêu chuẩn **Structural Prompting**.
    2. Viết Script `RemoveWhiteBackground.cs` (hoặc Chroma Key) để xử lý Asset thô sang Sprite trong suốt.
    3. Đọc `docs/guides/AI_UI_Integration_Methodology.md` để nắm quy trình sản xuất Asset Atomic.

---

> [!TIP]
> **Dành cho AI**: "Chào người bạn AI mới! Hãy bắt đầu bằng cách đọc `.agent/skills/ui-standardization/SKILL.md`,`.agent/skills/ui-visual-styling/SKILL.md` để nắm được quy tắc xây dựng UI. Sau đó xem mục **🛡️ Chiến lược Thiết kế Prefab theo Theme** trong `docs/guides/System_Full_Guide.md` để biết cách tạo Variant đúng chuẩn và tối ưu cho Addressables."
