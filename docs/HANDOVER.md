# Project Handover - NTVV (Nông Trại Vui Vẻ)

Tài liệu này dùng để đồng bộ nhanh "suy nghĩ" của AI Agent khi bạn chuyển sang máy tính mới hoặc bắt đầu một phiên làm việc mới.

## 🧠 Bối cảnh Phiên làm việc (Session Context)
- **Conversation ID**: `c562547a-dba6-474c-8d7a-0a5a61cb6538`
- **Phiên gần nhất (03/04/2026)**: Hoàn tất quy hoạch lại cấu trúc dữ liệu (**Centralized Data Architecture**). Triển khai cơ chế **Self-Healing** giúp hệ thống tự động kết nối dữ liệu cấu hình.

## 🗺 Bản đồ Hệ thống (System Map)
Nếu bạn mở dự án ở máy tính khác, AI hãy chú ý các file "đầu não" mới:
1.  **Dữ liệu Tập trung (Single Source of Truth)**: 
    - `Assets/_Project/Data/Registry/GameDataRegistry.asset`: Đăng ký toàn bộ Cây trồng, Thú nuôi, Nhiệm vụ và các Tiers cấu hình.
2.  **Cấu hình hệ thống (Global Configs)**: 
    - `Assets/_Project/Data/Configs/`: Chứa `StorageUpgradeConfig`, `AnimalPenUpgradeConfig` và `PlayerLevelData`. 
3.  **Tự động hóa & Sửa lỗi**: 
    - `GameSceneInitializer.cs`: Công cụ tạo nhanh Scene và tính năng **Repair Registry** để tự động liên kết lại các Config bị mất.
4.  **Hệ thống UI**:
    - `ResourcesUIProvider.cs`: Hệ thống nạp UI đa theme.
    - `UIStyleDataSO.cs`: Định nghĩa style cho từng theme.

## 🎯 Trạng thái hiện tại & Bước tiếp theo
- **Đã xong**: Quy hoạch lại thư mục `Data/` (Registry, Configs, Crops, Animals, Quests). Cập nhật toàn bộ Editor Tools tương thích với đường dẫn mới. Sửa lỗi biên dịch do thiếu namespace.
- **Cần làm ngay**: 
    1. Chạy Tool **Setup Full Game Scene** để kiểm tra việc tự động nạp Registry vào GameManager có ổn định không.
    2. Kiểm tra `StorageSystem` và `LevelSystem` xem log "Automatically linked" có xuất hiện trong console khi chạy game không.
    3. Tiếp tục thiết kế giao diện chi tiết cho các Modal (Shop, Storage) dựa trên Style Data đã có.

---

> [!TIP]
> **Dành cho AI**: "Chào người bạn AI mới! Hãy bắt đầu bằng cách đọc `docs/guides/System_Full_Guide.md` và `docs/Project_Master_Status.md` để nắm được kiến trúc dữ liệu tập trung vừa được triển khai."
