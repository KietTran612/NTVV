# Design Doc: Epic 4 - Shop & Economy System

**Tóm tắt:** Xây dựng hệ thống Cửa hàng tập trung để quản lý việc mua Hạt giống và Thú nuôi, kết nối với hệ thống Kinh tế (Vàng) và Cấp độ (Level). Đồng thời bổ sung tính năng nâng cấp Chuồng thú để tăng sức chứa.

---

## 1. Mục tiêu (Goals)
- Tạo một trung tâm kinh tế duy nhất cho người chơi.
- Áp dụng các giới hạn Level và Vàng một cách trực quan.
- Tăng giá trị sử dụng cho Vàng thông qua việc mua thú nuôi và nâng cấp chuồng.

---

## 2. Các thành phần chính (Components)

### 2.1. Centralized Shop UI
- **ShopPanelController:** Refactor từ `SeedShopPanelController`.
- **Hệ thống Tabs:** Phân chia "Hạt giống" và "Thú nuôi".
- **Dữ liệu động:** Tự động lấy từ `GameDataRegistrySO`.
- **Hiển thị vật phẩm:** 
    - Thông tin: Tên, Hình ảnh, Giá.
    - Trạng thái: Mở khóa / Khóa (dựa trên Level).
    - Phản hồi: Nút "MUA" (vàng) hoặc "FULL" (đối với thú nuôi nếu chuồng đầy).

### 2.2. Animal Pen Upgrade (Approach A)
- **AnimalPenUpgradeDataSO:** ScriptableObject chứa cấu hình cho từng Tier nâng cấp (Giá nâng cấp, Sức chứa tối đa).
- **AnimalPenView Update:** 
    - Thêm chỉ số `CurrentTier`.
    - Thêm hàm `UpgradePen()` để tiêu Vàng và tăng `Capacity`.
    - Hiển thị UI Nâng cấp khi người chơi bấm vào Chuồng.

### 2.3. Logic Spawning
- **Shop-to-Pen Connection:** Shop sẽ quét Scene để tìm `AnimalPenView` phù hợp với loại thú vừa mua.
- **Trigger:** Một NPC hoặc Storefront trong Farm để người chơi bấm vào mở Shop.

---

## 3. Luồng dữ liệu (Data Flow)
1. Player clicks **Shop NPC** -> `ShopPanelController` opens.
2. `ShopPanelController` reads `GameDataRegistrySO` to populate items.
3. Player clicks **Buy Chicken** -> Checks `EconomySystem` (Gold) + `LevelSystem` (Level) + `AnimalPenView` (Space).
4. If valid -> `EconomySystem.AddGold(-cost)` -> `AnimalPenView.SpawnAnimal()`.

---

## 4. Kế hoạch xác thực (Verification Plan)
- Mở Shop thành công từ NPC.
- Kiểm tra các vật phẩm bị khóa theo Level (đúng với `CropData.unlockLevel` và `AnimalData.unlockLevel`).
- Mua thú nuôi thành công -> Thú xuất hiện trong chuồng.
- Nâng cấp chuồng thành công -> Sức chứa tăng lên -> Có thể mua thêm thú.
- Thông báo lỗi hiện lên khi hết tiền hoặc hết chỗ.
