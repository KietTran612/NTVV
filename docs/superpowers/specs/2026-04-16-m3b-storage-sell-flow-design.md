# Design Document: m3b-storage-sell-flow

**Date:** 2026-04-16
**Spec:** m3b-storage-sell-flow
**Status:** approved
**Kiro Spec Path:** `.kiro/specs/m3b-storage-sell-flow/`

---

## Overview

M3b hoàn thiện **Storage và Shop flow** — fix 3 bugs thực sự và wire registry vào 2 prefabs. Logic Storage/Sell/Buy đã tồn tại đầy đủ. Spec này fix edge cases nguy hiểm (gold loss, data loss) và loại bỏ `FindObjectsOfTypeAll` pattern.

**Không thuộc scope M3b:**
- Crop care/harvest (M3a)
- Animal buy/sell (separate spec)
- Shop refresh rotation logic (v2)
- Gems persistence (backlog)

---

## Bugs Cần Fix

### BUG-B1 (MEDIUM): Sell All bán items có sellPrice = 0
**File:** `Assets/_Project/Scripts/UI/Panels/StoragePanelController.cs:192-222`

`OnSellAllClick()` bán TẤT CẢ items trong Storage snapshot — kể cả `item_grass` và `item_worm` (byproducts từ ClearWeeds/ClearPests) có `sellPrice = 0`. Player mất items nhưng không nhận gold. Đặc biệt nguy hiểm vì `item_grass`/`item_worm` cần để feed animals (AnimalView.Feed()).

**Fix:** Thêm `if (price <= 0) continue;` trước khi bán.

---

### BUG-B2 (HIGH): TryBuySeed trừ gold khi Storage đầy
**File:** `Assets/_Project/Scripts/UI/Panels/ShopPanelController.cs:149-165`

`TryBuySeed()` gọi `EconomySystem.AddGold(-totalCost)` trước, rồi `StorageSystem.AddItem()`. AddItem() có internal check reject nếu full — nhưng gold đã bị trừ. Kết quả: player mất gold mà không nhận seed.

**Fix:** Check `CanAddItem()` TRƯỚC `CanAfford()`.

---

### PERF-B3: FindObjectsOfTypeAll gọi mỗi lần mở panel
**Files:**
- `StoragePanelController.cs` — 2 calls (PopulateGrid + OnSellAllClick)
- `ShopPanelController.cs` — 1 call (PopulateShop)

`Resources.FindObjectsOfTypeAll<GameDataRegistrySO>()` scan toàn bộ loaded assets mỗi lần gọi — expensive và không cần thiết. GameDataRegistrySO là ScriptableObject, designed để reference trực tiếp trong prefab.

**Fix:** Thêm `[SerializeField] GameDataRegistrySO _registry` + assign trong prefab Inspector. Xóa `FindObjectsOfTypeAll` calls.

---

## Wiring Cần Làm

### WIRE-01: StoragePopup.prefab → _registry
Assign `GameDataRegistry.asset` vào `StoragePanelController._registry`

### WIRE-02: ShopPopup.prefab → _registry
Assign `GameDataRegistry.asset` vào `ShopPanelController._registry`

---

## Task Summary (6 tasks)

| Task | Mô tả | Loại |
|------|-------|------|
| 0 | StoragePanelController: thêm _registry field, xóa FindObjectsOfTypeAll, fix Sell All filter | Script fix |
| 1 | ShopPanelController: thêm _registry field, xóa FindObjectsOfTypeAll, fix TryBuySeed | Script fix |
| 2 | Wire StoragePopup.prefab → _registry | Wiring |
| 3 | Wire ShopPopup.prefab → _registry | Wiring |
| 4 | Integration Smoke Test | Test |
| 5 | Update HANDOVER.md | Docs |

---

## Test Thành Công Khi

1. Mở Storage → items hiển thị đúng (no FindObjectsOfTypeAll)
2. Sell individual item → gold tăng đúng
3. Sell All → item_grass/item_worm VẪN CÒN trong Storage
4. Mở Shop → mua seed khi Storage đầy → gold KHÔNG bị trừ, warning xuất hiện
5. Mua seed thành công → seed xuất hiện trong Storage
6. Console log: 0 errors liên quan registry null

---

## References

- `Assets/_Project/Scripts/UI/Panels/StoragePanelController.cs` — sell logic
- `Assets/_Project/Scripts/UI/Panels/ShopPanelController.cs` — buy logic
- `Assets/_Project/Scripts/Gameplay/Storage/StorageSystem.cs` — inventory management
- `Assets/_Project/Scripts/Gameplay/Economy/EconomySystem.cs` — gold management
- `Assets/_Project/Resources/UI/Default/StoragePopup.prefab` — prefab cần wire
- `Assets/_Project/Resources/UI/Default/ShopPopup.prefab` — prefab cần wire
- `.kiro/specs/m3a-crop-care-harvest/` — M3a prerequisite
