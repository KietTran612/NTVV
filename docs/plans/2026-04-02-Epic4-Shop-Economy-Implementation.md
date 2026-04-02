# [IMPLEMENTATION PLAN] Epic 4 - Shop & Economy System

> **For Antigravity:** REQUIRED WORKFLOW: Use `.agent/workflows/execute-plan.md` to execute this plan in single-flow mode.

**Goal:** Xây dựng hệ thống Cửa hàng tập trung (Hạt giống & Thú nuôi) và tính năng Nâng cấp Chuồng thú.

**Architecture:** 
- Sử dụng `GameDataRegistrySO` làm nguồn dữ liệu chính cho Shop.
- Refactor `SeedShopPanelController` thành `ShopPanelController` hỗ trợ Tab.
- Triển khai `AnimalPenUpgradeDataSO` để quản lý sức chứa chuồng theo mô hình tương tự Warehouse.

**Tech Stack:** Unity uGUI, C#, ScriptableObjects.

---

### Task 1: Data Foundation - Animal Pen Upgrades
**Goal:** Tạo cấu trúc dữ liệu cho việc nâng cấp chuồng.

**Files:**
- [NEW] `Assets/_Project/Scripts/Data/ScriptableObjects/AnimalPenUpgradeDataSO.cs`
- [MODIFY] `Assets/_Project/Scripts/Data/ScriptableObjects/GameDataRegistrySO.cs`

**Step 1: Định nghĩa AnimalPenUpgradeDataSO**
```csharp
namespace NTVV.Data.ScriptableObjects
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "AnimalPenUpgrade_", menuName = "NTVV/Data/Animal Pen Upgrade")]
    public class AnimalPenUpgradeDataSO : ScriptableObject
    {
        public int tierIndex;
        public int upgradeCostGold;
        public int maxCapacity;
    }
}
```

**Step 2: Đăng ký vào Registry**
Thêm `public List<AnimalPenUpgradeDataSO> animalPenUpgrades;` vào `GameDataRegistrySO`.

---

### Task 2: Logic Nâng cấp Chuồng (AnimalPenView Update)
**Goal:** Cho phép Chuồng thú có thể nâng cấp cấp độ và sức chứa.

**Files:**
- [MODIFY] `Assets/_Project/Scripts/World/Views/AnimalPenView.cs`

**Action:**
- Thêm `[SerializeField] private int _currentTier = 0;`
- Cập nhật `_capacity` dựa trên data từ `GameDataRegistrySO`.
- Thêm hàm `public bool UpgradePen()` để trừ tiền và tăng `_currentTier`.

---

### Task 3: Refactor Shop thành ShopPanelController
**Goal:** Biến Cửa hàng hạt giống thành Cửa hàng đa năng.

**Files:**
- [MODIFY] `Assets/_Project/Scripts/UI/Panels/SeedShopPanelController.cs` (Rename to `ShopPanelController.cs`)

**Action:**
- Thêm Tab UI (Hạt giống / Thú nuôi).
- Load dữ liệu từ `GameDataRegistrySO.crops` và `GameDataRegistrySO.animals`.
- Logic mua thú: Tìm `AnimalPenView` phù hợp trong Scene và gọi `SpawnAnimal()`.

---

### Task 4: Điểm tương tác Shop (Shop NPC/Trigger)
**Goal:** Tạo đối tượng trong thế giới để mở Shop.

**Files:**
- [NEW] `Assets/_Project/Scripts/World/Interactions/ShopTrigger.cs`

---

### Task 5: Kiểm tra & Đánh bóng (Polish & Testing)
**Goal:** Đảm bảo mọi thứ hoạt động trơn tru.
