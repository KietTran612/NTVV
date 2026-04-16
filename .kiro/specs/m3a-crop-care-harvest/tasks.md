# Implementation Plan: m3a-crop-care-harvest

## Overview

Fix 9 bugs trong crop care và harvest flow. Không viết script mới — chỉ sửa 2 files + wire 1 prefab.

**Design doc:** `.kiro/specs/m3a-crop-care-harvest/design.md`
**Requirements:** `.kiro/specs/m3a-crop-care-harvest/requirements.md`

> ⚠️ ĐỌC GLOBAL RULES trong requirements.md TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO.

---

## Tasks

- [ ] 0. Fix CropTileView — Care Actions + Death Visual (BUG-A1, A2, A10)
  - [ ] 0.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [ ] 0.4 · Script Agent — Sửa CropTileView.cs
    - `script-read` → `Assets/_Project/Scripts/World/Views/CropTileView.cs`
    - **Fix BUG-A1 + BUG-A2:** Thay 3 care methods:
      ```csharp
      public void ClearWeeds() { _hasWeeds = false; StorageSystem.Instance?.AddItem("item_grass", 1); RefreshVisuals(); }
      public void ClearPests() { _hasPests = false; StorageSystem.Instance?.AddItem("item_worm", 1);  RefreshVisuals(); }
      public void WaterPlant()  { _needsWater = false; RefreshVisuals(); }
      ```
    - **Fix BUG-A10:** Trong `HandleTick()`, tìm section Ripe (khoảng line 258-263):
      ```csharp
      if (_timeSinceRipe > _currentCropData.LifeAfterRipeInSeconds)
      {
          _currentState = TileState.Dead;
          RefreshVisuals(); // ADD LINE NÀY
      }
      ```
    - `assets-refresh` → đợi compile xong
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 3.1_
  - [ ] 0.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors liên quan CropTileView
    - Verify: 3 care methods đều có `RefreshVisuals()` ở cuối
    - Verify: `StorageSystem.Instance?` dùng null-conditional
    - Verify: HandleTick Ripe section có `RefreshVisuals()` sau `TileState.Dead`
    - Nếu FAIL → fix trong task 0, KHÔNG sang task 1

- [ ] 1. Fix CropTileView — HandleTick State Management (BUG-A3)
  - [ ] 1.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [ ] 1.4 · Script Agent — Thêm branch NeedsCare → Growing
    - `script-read` → `Assets/_Project/Scripts/World/Views/CropTileView.cs`
    - Tìm HandleTick(), section HP drain (khoảng line 282-302)
    - Thêm branch mới vào cuối else-if chain:
      ```csharp
      else if (_growthProgress >= 1f && _currentState != TileState.Ripe)
      {
          _currentState = TileState.Ripe;
          _currentStage = GrowthStage.Ripe;
          RefreshVisuals();
      }
      else if (_currentState == TileState.NeedsCare) // ADD BRANCH NÀY
      {
          _currentState = TileState.Growing;
      }
      ```
    - ⚠️ QUAN TRỌNG: Branch `NeedsCare → Growing` PHẢI nằm SAU branch Ripe trong else-if chain
    - `assets-refresh` → đợi compile xong
    - _Requirements: 2.1, 2.2_
  - [ ] 1.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: else-if chain đúng thứ tự: `drainRate > 0` → `growthProgress >= 1f (Ripe)` → `NeedsCare → Growing`
    - Nếu FAIL → fix trong task 1, KHÔNG sang task 2

- [ ] 2. Fix CropTileView — RestoreState (BUG-A6, A9)
  - [ ] 2.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [ ] 2.4 · Script Agent — Sửa RestoreState()
    - `script-read` → `Assets/_Project/Scripts/World/Views/CropTileView.cs`
    - Tìm `RestoreState()` (khoảng line 76-107)
    - Thêm sau `UpdateStage()` (line 105), trước closing brace của method:
      ```csharp
      UpdateStage();
      // BUG-A9 fix: UpdateStage() không set Ripe stage
      if (_currentState == TileState.Ripe)
          _currentStage = GrowthStage.Ripe;
      // BUG-A6 fix: cập nhật visuals ngay sau restore
      RefreshVisuals();
      ```
    - `assets-refresh` → đợi compile xong
    - _Requirements: 6.1, 6.2, 6.3_
  - [ ] 2.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: `RestoreState()` gọi `RefreshVisuals()` ở cuối
    - Verify: `if (_currentState == TileState.Ripe) _currentStage = GrowthStage.Ripe;` tồn tại
    - Nếu FAIL → fix trong task 2, KHÔNG sang task 3

- [ ] 3. Fix CropActionPanelController (BUG-A4, A5, A7)
  - [ ] 3.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [ ] 3.4 · Script Agent — Sửa CropActionPanelController.cs
    - `script-read` → `Assets/_Project/Scripts/UI/Panels/CropActionPanelController.cs`
    - **Fix BUG-A7:** Trong `RefreshUI()`, tìm dòng `_headerText.text = ...` (khoảng line 102):
      ```csharp
      // BEFORE:
      _headerText.text = _targetTile.CurrentState == CropTileView.TileState.Empty ? "Mảnh Đất" : _targetTile.CurrentCropData.cropName;
      // AFTER:
      string cropName = _targetTile.CurrentCropData?.cropName ?? "[Unknown]";
      _headerText.text = _targetTile.CurrentState == CropTileView.TileState.Empty ? "Mảnh Đất" : cropName;
      ```
    - **Fix BUG-A4:** Trong `Start()`, thay `gameObject.SetActive(false)` trong 3 listeners:
      - `_closeButton` listener:
        ```csharp
        _closeButton?.onClick.AddListener(() => {
            PopupManager.Instance?.CloseContextAction();
            if (PopupManager.Instance == null) { gameObject.SetActive(false); Managers.GameManager.Instance?.TriggerSave(); }
        });
        ```
      - `_harvestButton` listener:
        ```csharp
        _harvestButton?.onClick.AddListener(() => {
            _targetTile?.Harvest();
            PopupManager.Instance?.CloseContextAction();
            if (PopupManager.Instance == null) { gameObject.SetActive(false); Managers.GameManager.Instance?.TriggerSave(); }
        });
        ```
      - `_resetButton` listener:
        ```csharp
        _resetButton?.onClick.AddListener(() => {
            _targetTile?.ClearDead();
            PopupManager.Instance?.CloseContextAction();
            if (PopupManager.Instance == null) { gameObject.SetActive(false); Managers.GameManager.Instance?.TriggerSave(); }
        });
        ```
    - **Fix BUG-A5:** Trong `CropTileView.Harvest()` — `script-read` CropTileView trước, tìm đoạn check `CanAddItem` (khoảng line 117):
      ```csharp
      if (StorageSystem.Instance != null && StorageSystem.Instance.CanAddItem(_currentCropData.cropId, finalYield))
      {
          // ... existing harvest code
      }
      // Thêm else:
      else if (StorageSystem.Instance != null)
      {
          Debug.LogWarning($"[Harvest] Storage full! Cannot add {finalYield}x {_currentCropData.cropName}.");
      }
      ```
    - `assets-refresh` → đợi compile xong
    - _Requirements: 4.1, 4.2, 4.3, 4.4, 5.1, 5.2, 7.1, 7.2_
  - [ ] 3.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: `_closeButton`, `_harvestButton`, `_resetButton` listeners gọi `CloseContextAction()`
    - Verify: `cropName` null-safe trong RefreshUI
    - Nếu FAIL → fix trong task 3, KHÔNG sang task 4

- [ ] 4. Wire ContextActionPanel Prefab (WIRE-01)
  - [ ] 4.0 · Resource Check
    - Verify `GameDataRegistry.asset` tồn tại tại `Assets/_Project/Data/GameDataRegistry.asset`
    - Nếu không tìm thấy → `assets-find` với pattern `GameDataRegistry` → report path
    - NON-BLOCKING
  - [ ] 4.1 · Unity Agent — Assign _registry trong prefab
    - `assets-prefab-open` → `Assets/_Project/Resources/UI/Default/ContextActionPanel.prefab`
    - `gameobject-component-get` CropActionPanelController → kiểm tra `_registry` field hiện tại
    - Nếu `_registry` null → `gameobject-component-modify`:
      - Component: `CropActionPanelController`
      - Field: `_registry`
      - Value: reference đến `GameDataRegistry.asset`
    - `assets-prefab-save`
    - `assets-prefab-close`
    - _Requirements: 8.1, 8.2_
  - [ ] 4.✓ · Quick Test
    - `assets-prefab-open` ContextActionPanel → `gameobject-component-get` CropActionPanelController
    - Verify: `_registry` không null, reference đến `GameDataRegistry.asset`
    - `assets-prefab-close`
    - Nếu FAIL → fix trong task 4, KHÔNG sang task 5

- [ ] 5. Integration Smoke Test
  - ⚠️ **Prerequisite:** `scn-main-world-setup` phải hoàn thành (CropTile và WorldObjectPicker trong scene)
  - ⚠️ **Known Issue:** FarmCameraController (Old Input) và WorldObjectPicker (New Input) cùng fire khi tap — non-blocking
  - [ ] 5.1 Full gameplay cycle
    - `editor-application-set-state` → Play mode
    - Verify: `console-get-logs` filter=Error → 0 NullRef/MissingComponent errors
    - Verify: `console-get-logs` filter=Log → thấy `"[GameManager] Boot sequence complete."`
    - Tap vào CropTile Empty → verify CropActionPanel mở, Gieo Hạt button visible
    - Nếu có đủ gold → tap Gieo Hạt → verify: `console-get-logs` → `"[Plot] Planted:"`
    - Đợi tick (1 giây) → verify tile đang Growing
    - _Requirements: 9.1_
  - [ ] 5.2 Care cycle
    - Chỉnh `weedChancePct = 1.0` trong CropData tạm thời để force ailment
    - Đợi tick → verify `_weedVisual` bật lên trên tile
    - Tap tile → verify `_weedButton` visible trong panel
    - Tap Dọn Cỏ → verify `_weedVisual` tắt **ngay lập tức** (BUG-A1 verify)
    - Revert `weedChancePct` về giá trị cũ
    - _Requirements: 1.1, 1.2, 1.3_
  - [ ] 5.3 Harvest + Save/Load cycle
    - Set `growTimeMin = 0.01` (60ms) để test harvest nhanh
    - Đợi crop chín → tap tile → verify Harvest button visible → tap Harvest
    - Verify: `console-get-logs` → `"[Harvest] Success!"`
    - Verify: `console-get-logs` → GameManager save triggered (BUG-A4 verify)
    - `editor-application-set-state` → Stop
    - `editor-application-set-state` → Play (simulate reload)
    - Verify: item vẫn trong StorageSystem sau reload
    - _Requirements: 9.2_
  - [ ] 5.✓ · Integration Test Report
    - `screenshot-game-view` → lưu `docs/screenshots/2026-04-16-m3a-crop-care-harvest-integration.png`
    - Output:
      ```
      📋 INTEGRATION TEST REPORT: m3a-crop-care-harvest
      ✅ PASSED: [N] checks
      ⚠️ WARNINGS: [danh sách]
      ❌ FAILED: [N] — [mô tả]
      Bugs fixed: A1, A2, A3, A4, A5, A6, A7, A9, A10
      Known Issues: FarmCamera Old/New Input conflict (non-blocking)
      ```
    - Nếu FAIL critical → fix, KHÔNG chuyển sang task 6

- [ ] 6. Update HANDOVER.md
  - Mở `docs/HANDOVER.md`
  - Cập nhật section "Phiên 16/04/2026":
    - Spec `m3a-crop-care-harvest` execute xong
    - 9 bugs fixed trong CropTileView.cs và CropActionPanelController.cs
    - ContextActionPanel.prefab: `_registry` wired với GameDataRegistry.asset
    - Full cycle hoạt động: plant → care → harvest → save/load
  - Cập nhật Kiro Specs: thêm Spec 3 `m3a-crop-care-harvest` = DONE
  - Cập nhật "Bước tiếp theo" → M3b: Storage + Sell flow
  - _Requirements: tất cả_
