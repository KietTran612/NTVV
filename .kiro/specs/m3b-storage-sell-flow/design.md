# Design Document: m3b-storage-sell-flow

**Date:** 2026-04-16
**Spec:** m3b-storage-sell-flow
**Status:** approved
**Full design:** `docs/superpowers/specs/2026-04-16-m3b-storage-sell-flow-design.md`

---

## Overview

Fix 3 bugs trong 2 files + wire 2 prefabs. Không viết script mới.

**Files cần sửa:**
| File | Bugs/Wire |
|---|---|
| `Assets/_Project/Scripts/UI/Panels/StoragePanelController.cs` | BUG-B1, thêm `_registry` field, xóa `FindObjectsOfTypeAll` |
| `Assets/_Project/Scripts/UI/Panels/ShopPanelController.cs` | BUG-B2, thêm `_registry` field, xóa `FindObjectsOfTypeAll` |
| `Assets/_Project/Resources/UI/Default/StoragePopup.prefab` | WIRE-01 |
| `Assets/_Project/Resources/UI/Default/ShopPopup.prefab` | WIRE-02 |

---

## Bug Fix Summary

### BUG-B1 — Sell All bán items có sellPrice = 0

**File:** `StoragePanelController.cs:192-222`

`OnSellAllClick()` bán TẤT CẢ items trong snapshot kể cả `item_grass`, `item_worm` (byproducts từ ClearWeeds/ClearPests) có `sellPrice = 0`. Player mất items nhưng không nhận gold — silent data loss.

**Fix:** Thêm filter `price > 0` trước khi bán:
```csharp
foreach (var item in snapshot)
{
    // ... existing category filter ...

    int price = 0;
    if (registry != null)
    {
        var cropSO = registry.crops.FirstOrDefault(c => c.data.cropId == item.Key);
        if (cropSO != null) price = cropSO.data.sellPriceGold;
    }

    if (price <= 0) continue; // ADD: skip items with no sell value

    totalGold += item.Value * price;
    StorageSystem.Instance.AddItem(item.Key, -item.Value);
}
```

---

### BUG-B2 — TryBuySeed trừ gold khi Storage đầy

**File:** `ShopPanelController.cs:149-165`

`TryBuySeed()` trừ gold trước rồi gọi `StorageSystem.Instance.AddItem()`. `AddItem()` có internal check reject nếu full — nhưng gold đã bị trừ. Kết quả: player mất gold mà không nhận seed.

**Fix:** Check `CanAddItem()` trước khi trừ gold:
```csharp
private void TryBuySeed(string cropId, int unitCost, int qty)
{
    int totalCost = unitCost * qty;

    if (StorageSystem.Instance != null && !StorageSystem.Instance.CanAddItem(cropId, qty))
    {
        Debug.LogWarning($"[Shop] Storage full! Cannot buy {qty}x {cropId}.");
        return; // ADD: early return, gold không bị trừ
    }

    if (EconomySystem.Instance != null && EconomySystem.Instance.CanAfford(totalCost))
    {
        // ... existing buy code ...
    }
}
```

---

### Registry Wiring — Thay FindObjectsOfTypeAll bằng SerializedField

**StoragePanelController** dùng `FindObjectsOfTypeAll` ở 2 chỗ: `PopulateGrid()` (line 105) và `OnSellAllClick()` (line 196).

**ShopPanelController** dùng ở 1 chỗ: `PopulateShop()` (line 85).

**Fix cho cả 2 files:**

1. Thêm `[SerializeField] private GameDataRegistrySO _registry;` field
2. Thêm null-check trong `Awake()`:
   ```csharp
   private void Awake()
   {
       if (_registry == null)
           Debug.LogError($"[{GetType().Name}] _registry is null — assign GameDataRegistry.asset in prefab Inspector.");
   }
   ```
3. Thay `Resources.FindObjectsOfTypeAll<GameDataRegistrySO>().FirstOrDefault()` → `_registry`
4. Wire `GameDataRegistry.asset` vào prefab

---

## Critical Design Decisions

| Decision | Lý do |
|---|---|
| Direct prefab wiring cho `_registry` | Canonical Unity SO pattern — đồng nhất với M3a |
| `price <= 0 → continue` trong Sell All | Bảo vệ byproducts khỏi bị bán mất — item_grass/item_worm cần để feed animals |
| Check `CanAddItem` TRƯỚC `CanAfford` | Gold không bị trừ nếu storage đầy — atomicity |
| Giữ nguyên `OnSellClick()` (sell individual) | Đã hoạt động đúng — không cần fix |

---

## Architecture (unchanged)

```
BottomNav tap "Storage"
  → PopupManager.ShowScreen("Storage")
  → Instantiate StoragePopup.prefab (_registry pre-wired)
  → StoragePanelController.OnEnable() → PopulateGrid()
    → _registry.crops (NO FindObjectsOfTypeAll)
    → Items hiển thị trong grid

Tap item slot → SelectItem() → Sell sub-panel mở
  → Adjust quantity → OnSellClick()
  → StorageSystem.AddItem(id, -qty) → EconomySystem.AddGold()
  → TriggerSave()

Sell All → OnSellAllClick()
  → Filter: price > 0 only [BUG-B1 fix]
  → item_grass/item_worm giữ nguyên

BottomNav tap "Shop"
  → PopupManager.ShowScreen("Shop")
  → Instantiate ShopPopup.prefab (_registry pre-wired)
  → ShopPanelController.OnEnable() → PopulateShop()

Buy seed → TryBuySeed()
  → CanAddItem() check FIRST [BUG-B2 fix]
  → CanAfford() → AddGold(-cost) → AddItem(cropId, qty)
  → QuestEvent → TriggerSave()
```
