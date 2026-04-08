# Project Handover - NTVV (Nông Trại Vui Vẻ)

Tài liệu này dùng để đồng bộ nhanh "suy nghĩ" của AI Agent khi bạn chuyển sang máy tính mới hoặc bắt đầu một phiên làm việc mới.

## 🧠 Bối cảnh Phiên làm việc (Session Context)
- **Phiên 08/04/2026 (Hiện tại)**:
    - **Visual Standardization (PPU Multiplier)**: Thiết lập tiêu chuẩn độ phân giải visual đồng nhất (Buttons/Chips: 5, Banners: 2.5, Panels: 1.5, Icons: 1).
    - **Prompt Library Establishment**: Chính thức đóng gói [Atomic_HUD_Prompt_Library.md](file:///d:/soflware/Unity/Source/NTVV/docs/guides/Atomic_HUD_Prompt_Library.md) làm thư viện Prompt chuẩn.
    - **Transparent Asset Success**: Sản xuất thành công bộ 5 item đầu tiên (Buttons, Coin, Lightning, Parchment).

## 🗺 Bản đồ Hệ thống (System Map)
Nếu bạn mở dự án ở máy tính khác, AI hãy chú ý các file "đầu não" mới:
1.  **Dữ liệu Tập trung & Cấu hình**: `GameDataRegistry.asset` và thư mục `Assets/_Project/Data/Configs/`.
2.  **Quy chuẩn UI Pure MCP & AI Production**:
    - `docs/guides/AI_UI_Integration_Methodology.md`: Kim chỉ nam về chiến lược gán nhãn, styling và PPU standards.
    - `docs/guides/Atomic_HUD_Prompt_Library.md`: Thư viện Prompt chuẩn cho sản xuất asset.
    - `Assets/_Project/Data/UI/Styles/UIStyleDataSO.asset`: Nguồn sự thật duy nhất về màu sắc/font.
3.  **Hướng dẫn cài đặt máy mới**: `docs/guides/New_Machine_Setup.md` (Chú ý MCP `ai-game-developer` và `pencil`).

## 🎯 Trạng thái hiện tại & Bước tiếp theo
- **Đã xong**: Quy hoạch dữ liệu, Self-Healing, và Kiến trúc UI Tự động hóa. Thiết lập tiêu chuẩn PPU Multiplier và sản xuất bộ Asset đầu tiên đạt chuẩn.
- **Cần làm ngay**: 
    1. Tiếp tục hoàn thiện bộ Icon còn thiếu (Sprout, Apple, Wheat) theo chuẩn PPU 1.
    2. Thay thế toàn bộ Placeholder bằng Asset Atomic thực tế trong `SCN_Gameplay`.
    3. Kiểm tra tính đồng bộ của `UIStyleApplier` khi chạy trên các Scale màn hình khác nhau.

---

> [!TIP]
> **Dành cho AI**: "Chào người bạn AI mới! Hãy bắt đầu bằng cách đọc `.agent/skills/ui-standardization/SKILL.md`,`.agent/skills/ui-visual-styling/SKILL.md` để nắm được quy tắc xây dựng UI. Sau đó xem mục **🛡️ Chiến lược Thiết kế Prefab theo Theme** trong `docs/guides/System_Full_Guide.md` để biết cách tạo Variant đúng chuẩn và tối ưu cho Addressables."
