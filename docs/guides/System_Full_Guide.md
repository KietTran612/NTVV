# NTVV - Nông Trại Vui Vẻ: Tài liệu Hướng dẫn Hệ thống (System Guide)

Tài liệu này cung cấp cái nhìn tổng quan về các hệ thống cốt lõi trong dự án NTVV. Dành cho lập trình viên mới hoặc QA để hiểu luồng vận hành và cách kiểm thử.

---

## 🏗 Kiến trúc Cốt lõi (Core Architecture)

### 1. Singleton Pattern
- **Mô tả**: Hầu hết các Manager đều kế thừa từ `Singleton<T>` để truy cập toàn cục (ví dụ: `EconomySystem.Instance`, `QuestManager.Instance`).
- **File**: `Assets/_Project/Scripts/Core/Singleton.cs`

### 2. Time Manager (Tick System)
- **Mô tả**: Hệ thống sử dụng sự kiện `OnTick` để đồng bộ thời gian cho cây trồng và vật nuôi thay vì dùng `Update` riêng lẻ.
- **File**: `Assets/_Project/Scripts/Core/TimeManager.cs`

---

## 🌱 Hệ thống Trồng trọt (Crop System)

- **Vận hành**: 
    1. Click ô đất (`CropTileView`) -> Mở bảng hành động.
    2. Chọn hạt giống -> Giảm vật phẩm trong kho -> Trồng.
    3. Cây lớn dần theo Tick. Khi chín (Ripe) -> Nhấn để thu hoạch.
- **Dữ liệu**: Định nghĩa trong `CropDataSO`.
- **Cách Test**:
    - Trồng cây -> Chờ lớn -> Thu hoạch -> Kiểm tra XP và Kho tăng lên.
    - Test khi kho đầy có cho thu hoạch không.

---

## 💰 Kinh tế & Cửa hàng (Economy & Shop)

- **Vàng (Gold)**: Quản lý bởi `EconomySystem.Instance`.
- **Cửa hàng**: `ShopPanelController` hiển thị danh sách từ `GameDataRegistry`.
- **Cơ chế Mở khóa**: Một số vật phẩm yêu cầu `unlockLevel` (kiểm tra từ `LevelSystem`).
- **Cách Test**:
    - Mua hạt giống/Vật nuôi -> Kiểm tra trừ tiền đúng giá.
    - Kiểm tra vật phẩm chưa đủ level có bị khóa (Locked) không.

---

## 📦 Kho bãi & Nâng cấp (Storage & Upgrade)

- **Vận hành**: `StorageSystem` quản lý số lượng vật phẩm. Hỗ trợ nâng cấp dung lượng theo Tier.
- **Nâng cấp**: Yêu cầu Vàng và Cấp độ người chơi (`minLevelToAccess`).
- **Cách Test**:
    - Thêm vật phẩm quá giới hạn -> Phải báo lỗi hoặc không cho thêm.
    - Nâng cấp Kho -> Kiểm tra Max Capacity tăng lên và trừ tiền.

---

## 🐥 Hệ thống Chăn nuôi (Animal System)

- **Vận hành**: Mua thú -> Thú xuất hiện trong chuồng (`AnimalPenView`). 
- **Chu kỳ**: Thú đói -> Hiện icon đói -> Người chơi Cho ăn (tốn cỏ/sâu) -> Thú lớn/Sản xuất sản phẩm.
- **Cách Test**:
    - Để thú đói lâu -> Thú có thể chết (Dead Stage).
    - Thu hoạch sản phẩm thú -> Kiểm tra kho.
    - Nâng cấp Chuồng (`AnimalPenView`) -> Kiểm tra sức chứa tối đa tăng.

---

## 📜 Hệ thống Nhiệm vụ (Quest System)

- **Vận hành**: 
    1. Tương tác NPC/Bảng thông báo (`QuestGiver`) -> Nhận nhiệm vụ.
    2. Thực hiện hành động (Gặt, Cho ăn, Lên cấp) -> Bắn sự kiện qua `QuestEvents`.
    3. `QuestManager` ghi nhận tiến độ -> Hiện nút **CLAIM** trong bảng Quest.
- **Cách Test**:
    - Nhận nhiệm vụ "Thu hoạch 5 Lúa mì" -> Thu hoạch -> Kiểm tra UI Quest Panel có nhảy số 1/5, 2/5... không.
    - Hoàn thành -> Claim -> Kiểm tra nhận đúng thưởng Vàng/XP.

---

## 🛠 Công cụ Editor & Dữ liệu (Tools)

### 1. Game Data Manager (Trung tâm Quản lý)
- **Vị trí**: Menu `NTVV > Game Data Manager`.
- **Giao diện & Chức năng**:
    - **Toolbar (Phía trên)**: 
        - Nút **Sync from JSON**: Tự động đọc dữ liệu từ các file JSON trong `Assets/_Project/Settings/DataSources/JSON/` và cập nhật vào ScriptableObjects.
        - Các Tab (**Crops, Animals, Quests, Settings**): Chuyển đổi giữa các loại dữ liệu.
    - **Sidebar (Cột trái)**: 
        - Liệt kê toàn bộ vật phẩm có trong `GameDataRegistry`. 
        - Riêng tab **Quests** có thêm nút **Scan & Register Quests** để tự động tìm kiếm các bộ nhiệm vụ mới tạo trong Project.
    - **Detail View (Cột phải)**: 
        - Hiển thị toàn bộ thuộc tính của vật phẩm đang chọn. 
        - Nút **Ping Asset**: Giúp bạn tìm nhanh vị trí file Asset đó trong cửa sổ Project.
    - **Tab Settings**: 
        - Nơi tạo ra các file cấu hình hệ thống (Storage Upgrade, Animal Pen Upgrade).
        - Nút **Create New Quest Asset**: Tạo một nhiệm vụ mới với tên duy nhất, tự động lưu vào folder `Quests`.

### 2. Import Data (Công cụ Nhập liệu)
- **Menu**: `NTVV > Tools > Import Static Data` (Tương đương nút Sync trong Manager).
- **Cơ chế**: Chuyển đổi dữ liệu thô từ JSON sang `CropDataSO` và `AnimalDataSO`.

### 3. Sample Generator (Công cụ Tạo mẫu)
- **Menu**: `NTVV > Tools > Generate Sample...`
- **Mục đích**: Tạo nhanh dữ liệu chuẩn (Storage, Animal Pen, Quests) để lập trình viên có thể test ngay các chức năng mà không cần nhập liệu thủ công.

---

## 💾 Lưu trữ & Khôi phục (Persistence)

- **Cơ chế**: Dữ liệu lưu dưới dạng JSON tại `Application.persistentDataPath/ntvv_save.json`.
- **GameManager**: Khi khởi động (BootSequence), GameManager sẽ đọc file save và nạp lại trạng thái cho tất cả các hệ thống (Tiền, Level, Cây trồng, Nhiệm vụ).
- **Cách Test**:
    - Chơi game -> Tắt game (Alt+F4 hoặc Stop Editor) -> Mở lại -> Các cây đã trồng và Nhiệm vụ đang làm phải còn nguyên.

---

## 📝 Nhật ký Cập nhật (Change Log)

- **2026-04-02**: 
    - Khởi tạo tài liệu hướng dẫn hệ thống.
    - Cập nhật chi tiết Quest System và Editor Tools.
