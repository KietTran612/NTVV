# Project Handover - NTVV (Nông Trại Vui Vẻ)

Tài liệu này dùng để đồng bộ nhanh "suy nghĩ" của AI Agent khi bạn chuyển sang máy tính mới hoặc bắt đầu một phiên làm việc mới.

## 🧠 Bối cảnh Phiên làm việc (Session Context)
- **Phiên 06/04/2026**: 
    - Ra mắt kĩ năng **`ui-blueprinting`** (Kiến trúc sư) giúp bóc tách visual từ Mockup/Ảnh.
    - Kiến trúc UI 2 Lớp: Tách biệt **Functional Layer** (Auto-Wiring) và **Decorator Layer** (Styling).
    - Triển khai skill **`ui-standardization`** (xây cấu trúc) và **`ui-visual-styling`** (trang trí).
    - Cập nhật `PrefabAssembler` với logic **"Create or Verify"** để bảo vệ thiết kế visual khi chạy lại tool.
    - Tạo các controller nguyên tử (`UIResourceChip`, `UINavButton`, `UIProgressBar`).
- **Phiên 03/04/2026**: Hoàn tất quy hoạch lại cấu trúc dữ liệu (**Centralized Data Architecture**). Triển khai cơ chế **Self-Healing** giúp hệ thống tự động kết nối dữ liệu cấu hình.

## 🗺 Bản đồ Hệ thống (System Map)
Nếu bạn mở dự án ở máy tính khác, AI hãy chú ý các file "đầu não" mới:
1.  **Dữ liệu Tập trung & Cấu hình**: `GameDataRegistry.asset` và thư mục `Assets/_Project/Data/Configs/`.
2.  **Tiêu chuẩn UI (Architect, Builder, Stylist)**:
    - `.agent/skills/ui-blueprinting/SKILL.md`: Phân tích Mockup/Ảnh/Mô tả & Gợi ý **Component Layout Responsive**.
    - `.agent/skills/ui-standardization/SKILL.md`: Xây dựng cấu trúc & nối dây (Logic).
    - `.agent/skills/ui-visual-styling/SKILL.md`: Trang trí, màu sắc, bóng đổ (Visual).
    - `PrefabAssembler.cs`: Công cụ "Create or Verify" (không ghi đè visual nếu đã tồn tại).
    - `UIStyleApplier.cs` & `UIStyleDataSO.cs`: Bộ đôi quản lý theme và nạp visual khi game chạy.
3.  **Hướng dẫn cài đặt máy mới**: `docs/guides/New_Machine_Setup.md`.

## 🎯 Trạng thái hiện tại & Bước tiếp theo
- **Đã xong**: Quy hoạch dữ liệu tập trung (Registry). Thiết lập xong Skill chuẩn hóa UI và chiến lược 3 tầng cho AI. Cập nhật tài liệu hướng dẫn chuyên sâu về **Theme thiết kế (Prefab Variant)** và **Quy hoạch Addressables (Grouping per Theme)**.
- **Cần làm ngay**: 
    1. Khi nhận được yêu cầu UI mới, hãy gọi **`@ui-blueprinting`** trước để phân tích Mockup/Ảnh và xuất bản Blueprint chi tiết.
    2. Một khi người dùng đã duyệt Blueprint, hãy dùng **`@ui-standardization`** để xây khung Prefab.
    3. Cuối cùng, dùng **`@ui-visual-styling`** để nạp visual chuẩn 100% theo Blueprint.
    3. Luôn nhấn **Apply Style to Prefab NOW** để xem trực diện kết quả trong Unity Editor.
    4. Thử nghiệm đổi Theme từ `Default` sang `Cartoon` bằng cách swap `StyleData` Asset trên `UIStyleApplier`.

---

> [!TIP]
> **Dành cho AI**: "Chào người bạn AI mới! Hãy bắt đầu bằng cách đọc `.agent/skills/ui-standardization/SKILL.md`,`.agent/skills/ui-visual-styling/SKILL.md` để nắm được quy tắc xây dựng UI. Sau đó xem mục **🛡️ Chiến lược Thiết kế Prefab theo Theme** trong `docs/guides/System_Full_Guide.md` để biết cách tạo Variant đúng chuẩn và tối ưu cho Addressables."
