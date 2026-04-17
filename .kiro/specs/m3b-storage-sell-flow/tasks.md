# Implementation Plan: m3b-storage-sell-flow

## Overview

Fix 3 bugs trong Storage/Shop flow + wire 2 prefabs. Không viết script mới — chỉ sửa 2 files + wire 2 prefabs.

**Design doc:** `.kiro/specs/m3b-storage-sell-flow/design.md`
**Requirements:** `.kiro/specs/m3b-storage-sell-flow/requirements.md`

> ⚠️ ĐỌC GLOBAL RULES trong requirements.md TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO.

---

## Tasks

- [ ] 0. Fix StoragePanelController — Registry + Sell All Filter (BUG-B1)
  - [ ] 0.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [ ] 0.4 · Script Agent — Sửa StoragePanelController.cs
    - `script-read` → `Assets/_Project/Scripts/UI/Panels/StoragePanelController.cs`
    - **Thêm `_registry` field** (sau `_itemCardPrefab`):
      ```csharp
      [Header("Data")]
      [SerializeField] private GameDataRegistrySO _registry;
      ```
    - **Thêm `Awake()` null-check**:
      ```csharp
      private void Awake()
      {
          if (_registry == null)
              Debug.LogError("[StoragePanelController] _registry is null — assign GameDataRegistry.asset in prefab Inspector.");
      }
      ```
    - **Thay `FindObjectsOfTypeAll` trong `PopulateGrid()`** (khoảng line 105):
      ```csharp
      // BEFORE:
      var registry = Resources.FindObjectsOfTypeAll<GameDataRegistrySO>().FirstOrDefault();
      // AFTER:
      var registry = _registry;
      ```
    - **Thay `FindObjectsOfTypeAll` trong `OnSellAllClick()`** (khoảng line 196):
      ```csharp
      // BEFORE:
      var registry = Resources.FindObjectsOfTypeAll<GameDataRegistrySO>().FirstOrDefault();
      // AFTER:
      var registry = _registry;
      ```
    - **Fix BUG-B1:** Trong `OnSellAllClick()`, sau block tính `price` (khoảng line 210-211):
      ```csharp
      if (cropSO != null) price = cropSO.data.sellPriceGold;
      }

      if (price <= 0) continue; // ADD: skip items with no sell value

      totalGold += item.Value * price;
      ```
    - **Fix Req 2.3:** Sau vòng lặp foreach, trước `EconomySystem.Instance.AddGold(totalGold)`, thêm guard:
      ```csharp
      if (totalGold == 0)
      {
          Debug.LogWarning("[Storage] Sell All: no items with sell value > 0.");
          return;
      }
      EconomySystem.Instance.AddGold(totalGold);
      ```
    - **GIỮ `using System.Linq;`** — file vẫn dùng `.Any()`, `.FirstOrDefault()`, `.ToList()` để lookup `registry.crops` và snapshot. **KHÔNG xóa.**
    - `assets-refresh` → đợi compile xong
    - _Requirements: 1.1, 1.3, 2.1, 2.2, 2.3, 2.4_
  - [ ] 0.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors liên quan StoragePanelController
    - Verify: không còn `FindObjectsOfTypeAll` trong file
    - Verify: `OnSellAllClick()` có `if (price <= 0) continue;`
    - Verify: `OnSellAllClick()` có `if (totalGold == 0)` guard trước `AddGold`
    - Verify: `_registry` field tồn tại với `[SerializeField]`
    - Verify: `using System.Linq;` vẫn còn trong file
    - Nếu FAIL → fix trong task 0, KHÔNG sang task 1

- [ ] 1. Fix ShopPanelController — Registry + Buy Storage Check (BUG-B2)
  - [ ] 1.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [ ] 1.4 · Script Agent — Sửa ShopPanelController.cs
    - `script-read` → `Assets/_Project/Scripts/UI/Panels/ShopPanelController.cs`
    - **Thêm `_registry` field** (sau `_shopItemPrefab`):
      ```csharp
      [Header("Data")]
      [SerializeField] private GameDataRegistrySO _registry;
      ```
    - **Thêm `Awake()` null-check**:
      ```csharp
      private void Awake()
      {
          if (_registry == null)
              Debug.LogError("[ShopPanelController] _registry is null — assign GameDataRegistry.asset in prefab Inspector.");
      }
      ```
    - **Thay `FindObjectsOfTypeAll` trong `PopulateShop()`** (khoảng line 85):
      ```csharp
      // BEFORE:
      var registry = Resources.FindObjectsOfTypeAll<GameDataRegistrySO>().FirstOrDefault();
      // AFTER:
      var registry = _registry;
      ```
    - **Fix BUG-B2:** Trong `TryBuySeed()`, thêm storage check TRƯỚC gold check (khoảng line 149):
      ```csharp
      private void TryBuySeed(string cropId, int unitCost, int qty)
      {
          int totalCost = unitCost * qty;

          // BUG-B2 fix: check storage space BEFORE deducting gold
          if (StorageSystem.Instance != null && !StorageSystem.Instance.CanAddItem(cropId, qty))
          {
              Debug.LogWarning($"[Shop] Storage full! Cannot buy {qty}x {cropId}.");
              return;
          }

          if (EconomySystem.Instance != null && EconomySystem.Instance.CanAfford(totalCost))
          {
              EconomySystem.Instance.AddGold(-totalCost);
              StorageSystem.Instance.AddItem(cropId, qty);
              NTVV.Gameplay.Quests.QuestEvents.InvokeActionPerformed(Data.QuestActionType.BuyItem, cropId, qty);
              Managers.GameManager.Instance?.TriggerSave();
              Debug.Log($"<color=cyan>[Shop]</color> Bought Seed: {cropId} ×{qty} for {totalCost}g");
          }
          else
          {
              Debug.LogWarning("[Shop] Not enough gold!");
          }
      }
      ```
    - **Xóa `using System.Linq;`** nếu `FirstOrDefault()` không còn dùng
    - `assets-refresh` → đợi compile xong
    - _Requirements: 1.2, 1.3, 3.1, 3.2, 3.3_
  - [ ] 1.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors liên quan ShopPanelController
    - Verify: không còn `FindObjectsOfTypeAll` trong file
    - Verify: `TryBuySeed()` có `CanAddItem()` check trước `CanAfford()`
    - Verify: `_registry` field tồn tại với `[SerializeField]`
    - Nếu FAIL → fix trong task 1, KHÔNG sang task 2

- [ ] 2. Wire StoragePopup Prefab (WIRE-01)
  - [ ] 2.0 · Resource Check
    - Verify `GameDataRegistry.asset` tồn tại tại `Assets/_Project/Data/`
    - Verify `StoragePopup.prefab` tồn tại tại `Assets/_Project/Resources/UI/Default/`
    - NON-BLOCKING
  - [ ] 2.1 · Unity Agent — Assign _registry trong StoragePopup prefab
    - `assets-prefab-open` → `Assets/_Project/Resources/UI/Default/StoragePopup.prefab`
    - `gameobject-component-get` StoragePanelController → kiểm tra `_registry` field
    - Nếu `_registry` null → `gameobject-component-modify`:
      - Component: `StoragePanelController`
      - Field: `_registry`
      - Value: reference đến `GameDataRegistry.asset`
    - `assets-prefab-save`
    - `assets-prefab-close`
    - _Requirements: 1.1, 1.4_
  - [ ] 2.✓ · Quick Test
    - `assets-prefab-open` StoragePopup → `gameobject-component-get` StoragePanelController
    - Verify: `_registry` không null
    - `assets-prefab-close`
    - Nếu FAIL → fix trong task 2, KHÔNG sang task 3

- [ ] 3. Wire ShopPopup Prefab (WIRE-02)
  - [ ] 3.0 · Resource Check
    - Verify `ShopPopup.prefab` tồn tại tại `Assets/_Project/Resources/UI/Default/`
    - NON-BLOCKING
  - [ ] 3.1 · Unity Agent — Assign _registry trong ShopPopup prefab
    - `assets-prefab-open` → `Assets/_Project/Resources/UI/Default/ShopPopup.prefab`
    - `gameobject-component-get` ShopPanelController → kiểm tra `_registry` field
    - Nếu `_registry` null → `gameobject-component-modify`:
      - Component: `ShopPanelController`
      - Field: `_registry`
      - Value: reference đến `GameDataRegistry.asset`
    - `assets-prefab-save`
    - `assets-prefab-close`
    - _Requirements: 1.2, 1.4_
  - [ ] 3.✓ · Quick Test
    - `assets-prefab-open` ShopPopup → `gameobject-component-get` ShopPanelController
    - Verify: `_registry` không null
    - `assets-prefab-close`
    - Nếu FAIL → fix trong task 3, KHÔNG sang task 4

- [ ] 4. Integration Smoke Test
  - ⚠️ **Prerequisite:** `m3a-crop-care-harvest` hoàn thành (items có thể harvest vào Storage)
  - [ ] 4.1 Storage flow
    - `editor-application-set-state` → Play mode
    - Verify: `console-get-logs` filter=Error → 0 errors
    - Harvest 1 crop → verify item vào StorageSystem
    - Mở Storage (BottomNav) → verify grid hiển thị items đúng
    - Tap item slot → sell sub-panel mở → set quantity → tap Sell
    - Verify: `console-get-logs` → `"[Storage] Sold"`
    - Verify: gold balance tăng đúng
    - _Requirements: 4.1, 4.2_
  - [ ] 4.2 Sell All filter verify
    - Đảm bảo có `item_grass` hoặc `item_worm` trong Storage (từ ClearWeeds/ClearPests trong M3a)
    - Tap Sell All
    - Verify: crops bị bán, item_grass/item_worm VẪN CÒN trong Storage
    - Verify: `console-get-logs` → `"[Storage] Sold All for Xg"` — total KHÔNG bao gồm byproducts
    - _Requirements: 2.1, 2.2, 2.3, 4.3_
  - [ ] 4.3 Buy seed storage full verify
    - Fill storage gần đầy (hoặc set `_maxCapacity` nhỏ tạm)
    - Mở Shop → tap Buy seed
    - Verify: `console-get-logs` → `"[Shop] Storage full!"` — gold KHÔNG bị trừ
    - Revert `_maxCapacity` nếu đã chỉnh
    - _Requirements: 3.1, 3.2, 4.4_
  - [ ] 4.4 Buy seed success verify
    - Đảm bảo Storage có chỗ + player có đủ gold
    - Mở Shop → tap Buy seed
    - Verify: `console-get-logs` → `"[Shop] Bought Seed:"`
    - Mở Storage → verify seed mới xuất hiện
    - `editor-application-set-state` → Stop
    - _Requirements: 3.3, 4.5_
  - [ ] 4.✓ · Integration Test Report
    - `screenshot-game-view` → lưu `docs/screenshots/2026-04-16-m3b-storage-sell-flow-integration.png`
    - Output:
      ```
      📋 INTEGRATION TEST REPORT: m3b-storage-sell-flow
      ✅ PASSED: [N] checks
      ⚠️ WARNINGS: [danh sách]
      ❌ FAILED: [N] — [mô tả]
      Bugs fixed: B1, B2
      Wiring: StoragePopup._registry ✅, ShopPopup._registry ✅
      FindObjectsOfTypeAll removed: StoragePanelController ✅, ShopPanelController ✅
      ```
    - Nếu FAIL critical → fix, KHÔNG chuyển sang task 5

- [ ] 5. Update HANDOVER.md
  - Mở `docs/HANDOVER.md`
  - Cập nhật section "Phiên 16/04/2026":
    - Spec `m3b-storage-sell-flow` execute xong
    - 3 bugs fixed: Sell All filter, TryBuySeed storage check
    - `FindObjectsOfTypeAll<GameDataRegistrySO>()` đã xóa khỏi StoragePanelController và ShopPanelController
    - StoragePopup.prefab và ShopPopup.prefab: `_registry` wired
    - Full sell + buy flow hoạt động end-to-end
  - Cập nhật Kiro Specs: thêm Spec 4 `m3b-storage-sell-flow` = DONE
  - Cập nhật "Bước tiếp theo" → M4 milestones
  - _Requirements: tất cả_
