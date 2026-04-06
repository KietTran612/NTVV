# Project Handover - NTVV (Nông Trại Vui Vẻ)

Tài liệu này dùng để đồng bộ nhanh "suy nghĩ" của AI Agent khi bạn chuyển sang máy tính mới hoặc bắt đầu một phiên làm việc mới.

## 🧠 Bối cảnh Phiên làm việc (Session Context)
- **Conversation ID**: `c562547a-dba6-474c-8d7a-0a5a61cb6538`
- **Phiên 06/04/2026**: Thiết lập hệ thống **UI Standardization**. Triển khai skill **`ui-standardization`** giúp tự động hóa lắp ráp UI (Auto-Wiring). Tạo các controller nguyên tử (`UIResourceChip`, `UINavButton`, `UIProgressBar`).
- **Phiên 03/04/2026**: Hoàn tất quy hoạch lại cấu trúc dữ liệu (**Centralized Data Architecture**). Triển khai cơ chế **Self-Healing** giúp hệ thống tự động kết nối dữ liệu cấu hình.

## 🗺 Bản đồ Hệ thống (System Map)
Nếu bạn mở dự án ở máy tính khác, AI hãy chú ý các file "đầu não" mới:
1.  **Dữ liệu Tập trung & Cấu hình**: `GameDataRegistry.asset` và thư mục `Assets/_Project/Data/Configs/`.
2.  **Tiêu chuẩn UI (Standardization)**:
    - `.agent/skills/ui-standardization/SKILL.md`: "Kinh nang" bắt buộc AI phải đọc để xây dựng prefab.
    - `PrefabAssembler.cs`: Công cụ tự động gán biến (Auto-Wiring) trong Editor.
    - `Assets/_Project/Scripts/UI/Common/`: Chứa các Script Controller chuẩn (`UIResourceChip`, v.v.).
3.  **Hướng dẫn cài đặt máy mới**: `docs/guides/New_Machine_Setup.md`.

## 🎯 Trạng thái hiện tại & Bước tiếp theo
- **Đã xong**: Quy hoạch dữ liệu tập trung (Registry). Thiết lập xong Skill chuẩn hóa UI và chiến lược 3 tầng cho AI. Cập nhật tài liệu hướng dẫn chuyên sâu về **Theme thiết kế (Prefab Variant)** và **Quy hoạch Addressables (Grouping per Theme)**.
- **Cần làm ngay**: 
    1. Sử dụng skill **`@ui-standardization`** để xây dựng các Prefab thực tế cho `ShopEntry` và `InventorySlot`.
    2. Chạy Tool **NTVV > Setup > Assemble All Placeholders** để kiểm tra việc "Dò dây đệ quy" (Recursive Wiring) của PrefabAssembler.
    3. Hoàn thiện Logic cho `ShopPopup` và `StoragePopup` bằng cách kết nối với các Entry Prefab mới tạo.

---

> [!TIP]
> **Dành cho AI**: "Chào người bạn AI mới! Hãy bắt đầu bằng cách đọc `.agent/skills/ui-standardization/SKILL.md` để nắm được quy tắc xây dựng UI. Sau đó xem mục **🛡️ Chiến lược Thiết kế Prefab theo Theme** trong `docs/guides/System_Full_Guide.md` để biết cách tạo Variant đúng chuẩn và tối ưu cho Addressables."
