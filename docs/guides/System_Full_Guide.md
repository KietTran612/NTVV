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

### 3. Kiến trúc Dữ liệu Tập trung (Centralized Data Registry)
- **Mô tả**: Toàn bộ dữ liệu tĩnh của Game (Cây, Thú, Nhiệm vụ, Cấu hình nâng cấp) được quản lý tập trung trong một file Registry duy nhất. 
- **Lợi ích**: Giúp tránh việc phải kéo thả liên kết thủ công trong từng Scene, đảm bảo tính nhất quán của dữ liệu toàn dự án.
- **File**: `Assets/_Project/Data/Registry/GameDataRegistry.asset`

### 4. Mô hình Tự khắc phục (Self-Healing Pattern)
- **Mô tả**: Các hệ thống quan trọng (`LevelSystem`, `StorageSystem`) được lập trình để tự động tìm kiếm cấu hình cần thiết từ Registry nếu chúng bị thiếu liên kết khi khởi tạo.
- **Cơ chế**: Khi `OnInitialize`, nếu `config == null`, hệ thống sẽ gọi `GameManager.Instance.DataRegistry` để tự động kết nối dữ liệu.

---

## 🌱 Hệ thống Trồng trọt (Crop System)

- **Vận hành**: 
    1. Click ô đất (`CropTileView`) -> Mở bảng hành động.
    2. Chọn hạt giống -> Giảm vật phẩm trong kho -> Trồng.
    3. Cây lớn dần theo Tick. Khi chín (Ripe) -> Nhấn để thu hoạch.
- **Dữ liệu**: Định nghĩa trong `CropDataSO`.
- **Thư mục**: `Assets/_Project/Data/Crops/`
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
- **Nâng cấp**: Yêu cầu Vàng và Cấp độ người chơi (`minLevelToAccess`). Dữ liệu cấu hình được lấy từ Registry.
- **Cách Test**:
    - Thêm vật phẩm quá giới hạn -> Phải báo lỗi hoặc không cho thêm.
    - Nâng cấp Kho -> Kiểm tra Max Capacity tăng lên và trừ tiền.

---

## 🐥 Hệ thống Chăn nuôi (Animal System)

- **Vận hành**: Mua thú -> Thú xuất hiện trong chuồng (`AnimalPenView`). 
- **Chu kỳ**: Thú đói -> Hiện icon đói -> Người chơi Cho ăn (tốn cỏ/sâu) -> Thú lớn/Sản xuất sản phẩm.
- **Nâng cấp Chuồng**: Tăng sức chứa tối đa của chuồng, cấu hình được nạp tự động từ Registry.
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
- **Thư mục**: `Assets/_Project/Data/Quests/`
- **Cách Test**:
    - Nhận nhiệm vụ "Thu hoạch 5 Lúa mì" -> Thu hoạch -> Kiểm tra UI Quest Panel có nhảy số 1/5, 2/5... không.
    - Hoàn thành -> Claim -> Kiểm tra nhận đúng thưởng Vàng/XP.

---

## 🎨 Giao diện & Đa chủ đề (Managed Multi-Theme UI)

- **Cơ chế**: Dự án sử dụng kiến trúc giao diện theo hướng dữ liệu (Data-driven), cho phép thay đổi toàn bộ visual (skin) mà không làm thay đổi logic code.
- **Tập trung Dữ liệu UI**: `UIStyleDataSO` và Prefab theo Theme được quản lý trong `Resources/UI/`.
- **Cấu trúc Thư mục Theme**:
    - `Assets/_Project/Resources/UI/Default/`: Chứa các Prefab gốc.
    - `Assets/_Project/Resources/UI/[ThemeName]/`: Chứa các Prefab ghi đè (Override) riêng cho theme đó.
- **Tiêu chuẩn Thiết kế**: 
    - Toàn bộ UI thiết kế trên **Canvas uGUI**.
    - Độ phân giải chuẩn: **1920x1080**.
    - Sử dụng `UIStyleApplier` để đồng bộ Font/Color.

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
- **Chức năng**: Quản lý Crops, Animals, Quests, Themes tập trung.
- **Tự động hóa**: Nút **Sync from JSON** để cập nhật toàn bộ database từ file nguồn.

### 2. Game Scene Initializer (Setup Playtest)
- **Menu**: `NTVV > Setup Full Game Scene`.
- **Mục đích**: 1-Click để tạo Scene gameplay hoàn chỉnh. Tự động gắn Manager, Registry và UI Shell.

### 3. Repair Registry Tool (Khôi phục liên kết)
- **Menu**: `NTVV > UI > Restore Core Assets & Repair Registry`.
- **Mục đích**: Tự động quét toàn bộ thư mục `Data/Configs/`, tìm kiếm và kết nối lại các file cấu hình bị mất vào Registry. Hữu ích khi bạn di chuyển folder hoặc bị lỗi liên kết.

---

## 📁 Cấu trúc Thư mục Dữ liệu (Data Structure)

Dự án tuân thủ cấu trúc thư mục phẳng và nhất quán trong `Assets/_Project/Data/`:
- `Registry/`: Chứa file `GameDataRegistry.asset` (Trái tim của dữ liệu).
- `Configs/`: Chứa cấu hình hệ thống (Level, Storage Tier, Animal Pen Tier).
- `Crops/`: Danh sách các loại cây.
- `Animals/`: Danh sách các vật nuôi.
- `Quests/`: Danh sách các nhiệm vụ.

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
