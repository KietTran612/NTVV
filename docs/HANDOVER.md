# Project Handover - NTVV (Nông Trại Vui Vẻ)

Tài liệu này dùng để đồng bộ nhanh "suy nghĩ" của AI Agent khi bạn chuyển sang máy tính mới hoặc bắt đầu một phiên làm việc mới.

## 🧠 Bối cảnh Phiên làm việc (Session Context)
- **Conversation ID**: `c562547a-dba6-474c-8d7a-0a5a61cb6538`
- **Phiên gần nhất (03/04/2026)**: Hoàn tất hệ thống UI Đa Theme (Epic 6) và Công cụ Tự động hóa Scene.

## 🗺 Bản đồ Hệ thống (System Map)
Nếu bạn mở dự án ở máy tính khác, AI hãy chú ý các file "đầu não" sau:
1.  **Quản lý Chủ đề (Themes)**: 
    - `PopupManager.cs`: Logic nạp Popup/Panel theo theme.
    - `ResourcesUIProvider.cs`: Hệ thống nạp đệ quy (Recursive Fallback).
    - `UIStyleDataSO.cs` & `UIStyleApplier.cs`: Định nghĩa và áp dụng style.
2.  **Dữ liệu chính**: 
    - `GameDataRegistry.asset` (tại `Data/StaticData/`): Đăng ký toàn bộ Cây trồng, Thú nuôi, Nhiệm vụ.
3.  **Tự động hóa**: 
    - `GameSceneInitializer.cs`: Công cụ tạo nhanh Scene `SCN_Gameplay`.
    - `GameDataManagerWindow.cs`: Tab UI/Themes để quản lý Theme.

## 🎯 Trạng thái hiện tại & Bước tiếp theo
- **Đã xong**: Dọn dẹp Prefab, đưa các Popup thật vào thư mục `Default`. Scene `SCN_Gameplay` đã hoạt động tốt. Sửa lỗi load Save Data thiếu ID vật phẩm.
- **Cần làm ngay**: 
    1. Kiểm tra sự tương tác giữa `HUDTopBar` và người chơi trong Scene mới.
    2. Thiết kế thêm nội dung cho các Popup (`Shop`, `Quest`) dựa trên bộ khung đã có.
    3. Chuẩn bị cho việc chuyển đổi sang **Addressables** nếu cần thiết.

---

> [!TIP]
> **Dành cho AI**: "Chào người bạn AI mới! Hãy bắt đầu bằng cách đọc `docs/guides/System_Full_Guide.md` và `docs/Project_Master_Status.md` để nắm được kiến trúc hiện tại của dự án này."
