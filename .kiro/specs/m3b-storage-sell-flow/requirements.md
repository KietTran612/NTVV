# Requirements: m3b-storage-sell-flow

## Overview

Fix bugs trong Storage và Shop flow. Logic đã tồn tại đầy đủ — spec này fix 3 bugs, wire 2 prefabs, và verify end-to-end: harvest → items vào Storage → mở Storage UI → sell → gold tăng.

**Prerequisite:** `m3a-crop-care-harvest` phải hoàn thành — items phải có thể vào StorageSystem qua Harvest.

**Design doc:** `docs/superpowers/specs/2026-04-16-m3b-storage-sell-flow-design.md`

---

## Global Rules

> ⚠️ ĐỌC TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO

- Chỉ sửa đúng những gì bug yêu cầu — không refactor thêm
- `script-read` trước khi sửa bất kỳ file nào
- `assets-refresh` sau mỗi script change, CHỜ compile xong trước khi tiếp tục
- `editor_save_scene` sau mỗi sub-task có Unity change
- Quick Test FAIL → fix trong task hiện tại, KHÔNG sang task tiếp theo (tối đa 2 retry, sau đó escalate)
- **Không viết script mới** — tất cả bugs đều fix trong files hiện có

---

## Functional Requirements

### Req 1 — Registry Wiring (WIRE-01, WIRE-02)
- 1.1 `StoragePanelController._registry` trong `StoragePopup.prefab` được assign `GameDataRegistry.asset` trực tiếp
- 1.2 `ShopPanelController._registry` trong `ShopPopup.prefab` được assign `GameDataRegistry.asset` trực tiếp
- 1.3 `Resources.FindObjectsOfTypeAll<GameDataRegistrySO>()` bị xóa khỏi cả 2 controllers — thay bằng `_registry` SerializedField
- 1.4 Khi PopupManager instantiate prefab dynamically, `_registry` reference vẫn được giữ nguyên

### Req 2 — Sell All Filter (BUG-B1)
- 2.1 `OnSellAllClick()` chỉ bán items có `sellPrice > 0`
- 2.2 Items có `sellPrice = 0` (item_grass, item_worm, byproducts) bị bỏ qua — không bị xóa khỏi Storage
- 2.3 Nếu không có item nào có `sellPrice > 0`: không làm gì, log warning
- 2.4 Total gold tính đúng — chỉ tổng của items được bán

### Req 3 — Buy Seed Storage Check (BUG-B2)
- 3.1 `TryBuySeed()` kiểm tra `StorageSystem.CanAddItem(cropId, qty)` TRƯỚC khi trừ gold
- 3.2 Nếu storage đầy: `Debug.LogWarning` rõ ràng, gold KHÔNG bị trừ, seed KHÔNG được thêm
- 3.3 Nếu đủ chỗ: trừ gold → thêm seed → QuestEvent → TriggerSave (flow hiện tại giữ nguyên)

### Req 4 — Integration
- 4.1 Harvest crop → mở Storage (BottomNav) → items hiển thị đúng trong grid
- 4.2 Tap item slot → Sell sub-panel mở → sell quantity → tap Sell → gold tăng → item giảm
- 4.3 Sell All → chỉ crops bị bán, item_grass/item_worm còn nguyên trong Storage
- 4.4 Mở Shop → mua seed khi Storage đầy → gold không bị trừ, warning xuất hiện
- 4.5 Mua seed thành công → seed xuất hiện trong Storage
