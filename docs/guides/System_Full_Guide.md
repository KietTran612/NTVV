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

## 🎨 Giao diện & Đa chủ đề (Managed Multi-Theme UI)

- **Cơ chế**: Dự án sử dụng kiến trúc giao diện theo hướng dữ liệu (Data-driven), cho phép thay đổi toàn bộ visual (skin) mà không làm thay đổi logic code.
- **Thanh phần cốt lõi**:
    - `UIStyleDataSO`: Chứa định nghĩa về màu sắc chính, phụ, font chữ và các icon đặc trưng cho một Theme.
    - `UIStyleApplier`: Gắn trên các Object UI (Nút, Chữ, Nền) để tự động "ép" style từ SO vào component tương ứng.
    - `IUIAssetProvider` & `ResourcesUIProvider`: Hệ thống nạp Prefab theo đường dẫn Theme. Ưu tiên tìm trong folder Theme hiện tại, nếu không có sẽ tự động lấy từ folder `Default`.
- **Cấu trúc Thư mục Theme**:
    - `Assets/_Project/Resources/UI/Default/`: Chứa các Prefab gốc.
    - `Assets/_Project/Resources/UI/[ThemeName]/`: Chứa các Prefab ghi đè (Override) riêng cho theme đó.
- **Cách sử dụng & Tạo Theme mới**:
    1. Mở `NTVV > Game Data Manager > Tab UI/Themes`.
    2. Nhấn **Create New Theme**, nhập tên (ví dụ: `Modern`).
    3. Hệ thống sẽ:
        - Tạo file `ModernStyle.asset` trong `Settings/UI/`.
        - Tạo thư mục `Assets/_Project/Resources/UI/Modern/`.
    4. Để tùy chỉnh giao diện:
        - Kéo Prefab gốc từ `Default` vào thư mục `Modern`.
        - Thay đổi Layout, Sprite hoặc Font trong Prefab mới này. Hệ thống sẽ tự động ưu tiên bản trong thư mục `Modern` khi Theme này được kích hoạt.
- **Cơ chế nạp UI (Loading Logic)**:
    - Dự án sử dụng `ResourcesUIProvider` với logic **Tìm kiếm đệ quy (Recursive Fallback)**:
        - Bước 1: Tìm tại `UI/[ActiveTheme]/[Name]`.
        - Bước 2: Nếu không thấy, tìm tại `UI/Default/[Name]`.
        - Bước 3: Nếu vẫn không thấy, tìm tại gốc `UI/[Name]`.
    - Điều này cho phép bạn chỉ cần ghi đè (Override) những màn hình thực sự cần thay đổi, các màn hình khác sẽ tự động dùng bản mặc định (Default).

---

## 🏗 Lộ trình nâng cấp Addressables (Addressables Roadmap)

Hệ thống UI hiện tại được thiết kế theo Interface `IUIAssetProvider` để dễ dàng nâng cấp lên **Unity Addressables** trong tương lai:

1. **Chuyển đổi Provider**: 
    - Thay thế `ResourcesUIProvider` bằng `AddressableUIProvider`.
    - `PopupManager` sẽ không cần thay đổi code logic vì cả hai đều dùng chung Interface.
2. **Quản lý tài nguyên**:
    - Các thư mục Theme sẽ được chuyển thành **Addressable Groups**.
    - Sử dụng **Labels** (ví dụ: `Theme_Cartoon`, `Theme_Retro`) để lọc và tải tài nguyên theo cụm.
3. **Luồng tải bất đồng bộ (Async Loading)**:
    - Phương thức `LoadPrefab` sẽ được chuyển thành `Task<GameObject>` hoặc sử dụng `AsyncOperationHandle`.
    - Giúp giảm thời gian treo máy khi chuyển Theme hoặc mở Popup lớn.

---

## 🛠 Công cụ Editor & Dữ liệu (Tools)

### 1. Game Data Manager (Trung tâm Quản lý)
- **Vị trí**: Menu `NTVV > Game Data Manager`.
- **Giao diện & Chức năng**:
    - **Toolbar (Phía trên)**: 
        - Nút **Sync from JSON**: Tự động đọc dữ liệu từ các file JSON trong `Assets/_Project/Settings/DataSources/JSON/` và cập nhật vào ScriptableObjects.
        - Các Tab (**Crops, Animals, Quests, UI/Themes, Settings**): Chuyển đổi giữa các loại dữ liệu.
    - **Tab UI/Themes**:
        - Liệt kê danh sách các Theme hiện có.
        - **Create New Theme**: Tạo theme mới và tự động tạo cấu trúc thư mục Resources tương ứng.
        - **Clone Theme**: Sao chép style từ một theme có sẵn.
        - **Set Active Theme**: Thiết lập Theme mặc định khi khởi động game.
    - **Sidebar & Detail View**: Quản lý chi tiết từng Item/Quest.

### 2. UI Initializer (Khởi tạo UI)
- **Menu**: `NTVV > UI > Setup Phase 1`.
- **Mục đích**: Tự động tạo cấu trúc thư mục chuẩn cho Resource UI và tạo các file Style ban đầu nếu chưa có.

### 3. Game Scene Initializer (Thiết lập Scene chơi game)
- **Menu**: `NTVV > Setup Full Game Scene`.
- **Mục đích**: Tự động tạo Scene `SCN_Gameplay` hoàn chỉnh, gắn sẵn tất cả các Manager, UI Canvas, HUD và các ô đất tương tác để có thể chơi ngay lập tức.

---

## 💾 Lưu trữ & Khôi phục (Persistence)

- **Cơ chế**: Dữ liệu lưu dưới dạng JSON tại `Application.persistentDataPath/ntvv_save.json`.
- **GameManager**: Khi khởi động (BootSequence), GameManager sẽ đọc file save và nạp lại trạng thái cho tất cả các hệ thống.
- **Phục hồi an toàn (Defensive Loading)**: Hệ thống tự động kiểm tra và bỏ qua các vật phẩm bị lỗi hoặc thiếu ID trong file save để tránh làm hỏng luồng chạy của trò chơi.

---

## 📝 Nhật ký Cập nhật (Change Log)

- **2026-04-03**:
    - Tích hợp hệ thống **Managed Multi-Theme UI** (Đa chủ đề).
    - Thêm công cụ **Game Scene Initializer** để tự động thiết lập Scene.
    - Cập nhật Tab **UI/Themes** trong Game Data Manager.
- **2026-04-02**: 
    - Khởi tạo tài liệu hướng dẫn hệ thống.
    - Cập nhật chi tiết Quest System và Editor Tools.
