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
    - Sử dụng `UIStyleApplier` để đồng bộ bộ font **Dosis** và bảng màu.
- **Kiến trúc UI 3 Bước (3-Step Skill Pipeline)**:
    Dự án áp dụng quy trình 3 giai đoạn được Agent thực thi 100% qua tập lệnh MCP:
    1.  **Giai đoạn 1: Blueprinting (`@ui-blueprinting`)**: 
        - **Hành động**: Khảo sát `.pen` qua `pencil.batch_get`.
        - **Đầu ra**: Xác định chính xác Layout (Grid/Horizontal/Vertical), thông số Padding, Spacing và ID linh kiện.
    2.  **Giai đoạn 2: Standardization (`@ui-standardization`)**: 
        - **Hành động (Pure MCP)**: Agent tự tay xây dựng Hierarchy trong Unity bằng `gameobject-create`. Tự thực hiện gán Reference (Manual Wiring) qua `object-modify`.
        - **Đầu ra**: Prefab "sạch", linh hoạt và không có các script tự động hoá dư thừa.
    3.  **Giai đoạn 3. **Trang trí & Styling (`@ui-visual-styling`)**:
   - **Mục tiêu**: Áp dụng thẩm mỹ (Skins) lên Prefab mà không làm hỏng logic.
   - **Dữ liệu**: Sử dụng `UIStyleDataSO.asset` làm nguồn Palette và Font duy nhất.
   - **Thực thi**: Agent gán nhãn `UIStyleApplier` và thực hiện lệnh "Bake" (gán Material Dosis, Sprite, Color) trực tiếp vào Prefab thông qua các lệnh MCP.

---

## 4. Quy trình Chế tác Asset "Nguyên tử" (Atomic Asset Production)

Để HUD có chất lượng AAA nhưng vẫn linh hoạt, chúng ta không dùng một tấm ảnh bẹt (flat). Thay vào đó, AI sẽ sản xuất các thành phần rời rạc (Atomic Layers).

### Bước 4.1: Tiêu chuẩn Structural Prompting (Template V1)
Mọi yêu cầu tạo ảnh cho HUD NTVV PHẢI tuân thủ cấu trúc sau:
`[asset type] for [use case], [view], [style anchors], [material], [background], [must avoid], [quality target]`

-   **View**: Quan trọng nhất để giữ tính đồng nhất (Consistency).
    -   *Nền (Background/Panel)*: `Straight Front View (Orthographic)` để hỗ trợ 9-slicing.
    -   *Vật phẩm (Icon/Prop)*: `3D Isometric View (45-degree angle)` để tạo độ nổi khối.
-   **Background**: Ưu tiên `TRANSPARENT BACKGROUND`. Nếu AI không đáp ứng tốt, dùng `Solid Cyan/Magenta` (Chroma Key) để tách nền sạch mà không mất Highlights (quầng sáng trắng).

### Bước 4.2: Luồng xử lý Asset AI
1.  **Generate**: Dùng Prompt Structure để tạo ảnh thô.
2.  **Alpha Processing**: Viết/Dùng Script C# (hoặc tool ngoài) để tẩy trắng hoặc khử màu Chroma Key sang Alpha 0.
3.  **Import & Metadata**: Nạp vào Unity, cài đặt `Sprite Mode: Single`, gán `Mesh Type: Full Rect` để tối ưu hóa 9-slicing.

- **Tiêu chuẩn Đấu nối (Auto-Wiring Suffixes)**:
    - Dự án áp dụng skill **`ui-standardization`** để đảm bảo liên kết bền vững và tự động hóa.
    - **Các chức năng chính của Skill**:
        1. **Ma trận Quyết định**: Tự động đánh giá nhu cầu sử dụng Controller cho từng Prefab.
        2. **Chuẩn Hậu tố (Suffixes)**: Ép buộc đặt tên `_Label`, `_Icon`, `_Button`, `_Fill`, `_Content`.
        3. **Recursive Auto-Wiring**: Dò tìm linh kiện đệ quy, chống đứt gãy link khi thay đổi hierarchy.
        4. **Luồng Wiring Chuẩn**: Quy trình 6 bước từ Hierarchy đến Verification.
        5. **Event Binding qua Code**: Đăng ký `AddListener` cho Button thay vì dùng Inspector.
        6. **Verification Checklist**: Bảng kiểm tra cuối cùng để loại bỏ 100% lỗi Null Reference.

- **Hệ thống Trang trí Visual (UI Visual Styling)**:
    - Dự án tách biệt tầng **Logic/Cấu trúc** và tầng **Trang trí/Visual** để đảm bảo an toàn khi bảo trì.
    - **Công nghệ**: Sử dụng bộ đôi `UIStyleApplier` và `UIStyleDataSO` (ScriptableObject).
    - **Quy tắc Naming Prefix**:
        - `bg_`: Background/Panel.
        - `shadow_`: Bóng đổ.
        - `border_`: Viền trang trí.
        - `overlay_`: Hiệu ứng phủ/Highlight.
        - `fx_`: Hiệu ứng Glow/Sparkle.
    - **Workflow**: 
        1. AI dùng `batch_get` để trích xuất file Layout Mockup, đối chiếu tên bằng `docs\document_md`.
        2. Sinh các Object tiền tố trang trí (không chạm Object Logic). 
        3. Tạo file `StyleData.asset` chứa hệ điều hành Sprite/Color/Font.
        4. Gọi API Unity bằng Script C# sinh bằng `unity-skill-create` để nạp Visual.
    - **Lợi ích**: Workflow Tự Động Kín. Rủi ro lỗi hỏng Reference biến mất hoàn toàn. Cho phép đổi Theme chớp nhoáng (Swap ScriptableObject).
    - **Lợi ích**: Giúp code Editor có thể tự động "dò dây" và gán biến vào Inspector mà không cần kéo thả thủ công.

### 🛡️ Chiến lược Thiết kế Prefab theo Theme (Variant Strategy)

Khi tạo một bản hiển thị (Visual) mới cho Theme:
1. **Dùng Prefab Variant**: Luôn tạo **Prefab Variant** từ Prefab gốc trong thư mục `Default/`.
2. **Kế thừa Logic**: Tuyệt đối không xóa hoặc thay thế Script Controller của bản gốc trên bản Variant.
3. **Bảo vệ Cấu trúc (Structure Integrity)**: 
    - Có thể thay đổi vị trí (Transform) hoặc Sprite/Font/Color.
    - **KHÔNG** được đổi tên hoặc xóa các Object con có hậu tố chuẩn (`_Label`, `_Icon`) vì code Controller dựa vào các tên này để gán dữ liệu.
4. **Quản lý Decorator**: Luôn sử dụng `UIStyleApplier` để quản lý các lớp `bg_`, `shadow_`. Nếu cần thêm lớp trang trí mới, hãy dùng đúng tiền tố quy định.
5. **Đường dẫn Lưu trữ**: Lưu bản Variant vào thư mục `Assets/_Project/Resources/UI/[ThemeName]/` với cùng tên file như bản gốc.

### 🔠 Typography (Phong cách Chữ)
- **Font Chủ đạo**: **Dosis** (Bộ font chính cho game).
    - `Dosis-Bold SDF`: Dùng cho tiêu đề, nội dung quan trọng.
    - `Dosis-ExtraBold SDF`: Dùng cho các con số, nhấn mạnh, UI Button.
- **Dosis Material Presets**: Tận dụng bộ sưu tập Outline có sẵn để phân loại trạng thái:
    - `Outline-Green/Lime`: Thành công, tăng trưởng, thu hoạch.
    - `Outline-Blue/Cyan`: Thông tin, cấp độ, hệ thống.
    - `Outline-Yellow`: Tiền vàng, phần thưởng, VIP.
    - `Outline-Red/Berry`: Cảnh báo, lỗi, tiêu hao.
- **Quy tắc**: Toàn bộ UI Text phải sử dụng **TextMeshPro** và ưu tiên dùng các Preset Material này để đồng nhất visual.

---

## 🛠 Công cụ Editor & Tự động hóa (Automation Tools)

### 1. Game Data Manager (Trung tâm Quản lý)
- **Vị trí**: Menu `NTVV > Game Data Manager`.
- **Chức năng**: Quản lý Crops, Animals, Quests, Themes tập trung.
- **Tự động hóa**: Nút **Sync from JSON** để cập nhật toàn bộ database từ file nguồn.

### 2. Game Data Manager (Trung tâm Quản lý)
- **Vị trí**: Menu `NTVV > Game Data Manager`.
- **Chức năng**: Quản lý Crops, Animals, Quests, Themes tập trung.
- **Tự động hóa**: Nút **Sync from JSON** để cập nhật toàn bộ database từ file nguồn.

### 3. Game Scene Initializer (Setup Playtest)
- **Menu**: `NTVV > Setup Full Game Scene`.
- **Mục đích**: 1-Click để tạo Scene gameplay hoàn chỉnh. Tự động gắn Manager, Registry và UI Shell.

### 4. Repair Registry Tool (Khôi phục liên kết)
- **Menu**: `NTVV > UI > Restore Core Assets & Repair Registry`.
- **Mục đích**: Tự động quét toàn bộ thư mục `Data/Configs/`, tìm kiếm và kết nối lại các file cấu hình bị mất vào Registry.

Hệ thống UI hiện tại được thiết kế theo Interface `IUIAssetProvider` để dễ dàng nâng cấp lên **Unity Addressables** trong tương lai:

1. **Chuyển đổi Provider**: 
    - Thay thế `ResourcesUIProvider` bằng `AddressableUIProvider`.
    - `PopupManager` sẽ không cần thay đổi code logic vì cả hai đều dùng chung Interface.
2. **Quản lý tài nguyên theo Theme (Grouping Strategy)**:
    - **Addressable Groups**: Mỗi Theme sẽ tương ứng với một Group riêng biệt (ví dụ: `Theme_Default`, `Theme_Cartoon`).
    - **Labels**: Toàn bộ Prefab trong một Theme sẽ được gán Label tương ứng để AI có thể thực hiện **Batch Loading**.
    - **Optimization**: Cách phân nhóm này cho phép người chơi chỉ tải Theme họ đang sử dụng, giúp giảm dung lượng cài đặt và RAM.
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

- **2026-04-07**:
    - Xác lập Giao thức Đọc File `.pen` khổng lồ bằng Regex (Pencil Server) và tra chéo ID ngầm qua tài liệu `document_md`.
    - Chuyển quyền Tự Hành Trọn Trình 100% cho AI trong cả 3 Skill UI (Architect, Builder, Stylist) thông qua sức mạnh sinh Tools Editor API của `unity-skill-create`.
- **2026-04-06**:
    - Ra mắt kĩ năng **`ui-blueprinting`** (Kiến trúc sư) giúp phân tích Mockup và Ảnh chính xác trước khi thực thi.
    - Thiết lập **Kiến trúc UI 2 Lớp**: Tách biệt Functional Layer (logic) và Decorator Layer (visual).
    - Ra mắt hệ thống **`ui-visual-styling`** quản lý màu sắc, sprite, font qua ScriptableObject.
    - Nâng cấp `PrefabAssembler` với logic **"Create or Verify"**, bảo vệ thiết kế visual khi chạy lại tool.
    - Chuẩn hóa hệ thống UI với skill **`ui-standardization`**.
    - Tạo bộ controller nguyên tử: `UIResourceChip`, `UINavButton`, `UIProgressBar`.
- **2026-04-03**:
    - Tích hợp hệ thống **Managed Multi-Theme UI** (Đa chủ đề).
    - Thêm công cụ **Game Scene Initializer** để tự động thiết lập Scene.
    - Cập nhật Tab **UI/Themes** trong Game Data Manager.
- **2026-04-02**: 
    - Khởi tạo tài liệu hướng dẫn hệ thống.
    - Cập nhật chi tiết Quest System và Editor Tools.
