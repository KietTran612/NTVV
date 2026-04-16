# Design Document: m3a-crop-care-harvest

**Date:** 2026-04-16
**Spec:** m3a-crop-care-harvest
**Status:** approved
**Full design:** `docs/superpowers/specs/2026-04-16-m3a-crop-care-harvest-design.md`

---

## Overview

Fix 9 bugs trong 2 files + 1 prefab wire. Không viết script mới.

**Files cần sửa:**
| File | Bugs |
|---|---|
| `Assets/_Project/Scripts/World/Views/CropTileView.cs` | A1, A2, A3, A5, A6, A9, A10 |
| `Assets/_Project/Scripts/UI/Panels/CropActionPanelController.cs` | A4, A7 |
| `Assets/_Project/Resources/UI/Default/ContextActionPanel.prefab` | WIRE-01 |

---

## Bug Fix Summary

### Task 0 — CropTileView: Care Actions + Death Visual
**BUG-A1:** `ClearWeeds/ClearPests/WaterPlant` không gọi `RefreshVisuals()` → visual không tắt ngay
**BUG-A2:** `StorageSystem.Instance.AddItem()` không có null check → NullRef nếu chưa boot
**BUG-A10:** Ripe→Dead qua thời gian không gọi `RefreshVisuals()` → dead sprite không hiện

```csharp
// BEFORE:
public void ClearWeeds() { _hasWeeds = false; StorageSystem.Instance.AddItem("item_grass", 1); }
public void ClearPests() { _hasPests = false; StorageSystem.Instance.AddItem("item_worm", 1); }
public void WaterPlant()  { _needsWater = false; }

// AFTER:
public void ClearWeeds() { _hasWeeds = false; StorageSystem.Instance?.AddItem("item_grass", 1); RefreshVisuals(); }
public void ClearPests() { _hasPests = false; StorageSystem.Instance?.AddItem("item_worm", 1);  RefreshVisuals(); }
public void WaterPlant()  { _needsWater = false; RefreshVisuals(); }

// HandleTick Ripe section — AFTER:
if (_timeSinceRipe > _currentCropData.LifeAfterRipeInSeconds)
{
    _currentState = TileState.Dead;
    RefreshVisuals(); // ADD
}
```

---

### Task 1 — CropTileView: HandleTick State Management
**BUG-A3:** State kẹt `NeedsCare` sau khi hết ailments — không reset về `Growing`

```csharp
// Thêm branch mới sau else-if Ripe trong HandleTick:
else if (_currentState == TileState.NeedsCare)
{
    _currentState = TileState.Growing;
}
```

⚠️ Branch này nằm SAU `else if (_growthProgress >= 1f)` — nếu drainRate=0 và growthProgress≥1 → Ripe (đúng). Chỉ reset Growing khi growthProgress < 1f.

---

### Task 2 — CropTileView: RestoreState
**BUG-A6:** `RestoreState()` không gọi `RefreshVisuals()` → visuals sai sau load
**BUG-A9:** `RestoreState()` không set `GrowthStage.Ripe` khi tile Ripe → sprite sai

```csharp
// Trong RestoreState(), sau UpdateStage():
if (_currentState == TileState.Ripe)
    _currentStage = GrowthStage.Ripe; // BUG-A9 fix
RefreshVisuals(); // BUG-A6 fix — LUÔN gọi ở cuối RestoreState
```

---

### Task 3 — CropActionPanelController: Autosave + Null Safety
**BUG-A4:** Close/Harvest/Reset dùng `gameObject.SetActive(false)` → bypass `TriggerSave()`
**BUG-A5:** Harvest fail (storage đầy) không có warning
**BUG-A7:** `_targetTile.CurrentCropData.cropName` NPE khi cropData null

```csharp
// BUG-A4: Thay gameObject.SetActive(false) bằng:
PopupManager.Instance?.CloseContextAction();
// (Nếu PopupManager null: fallback gameObject.SetActive(false) + GameManager.Instance?.TriggerSave())

// BUG-A5: Trong CropTileView.Harvest(), trước return khi CanAddItem fail:
Debug.LogWarning($"[Harvest] Storage full! Cannot add {finalYield}x {_currentCropData.cropName}.");

// BUG-A7: Trong RefreshUI():
string cropName = _targetTile.CurrentCropData?.cropName ?? "[Unknown]";
_headerText.text = _targetTile.CurrentState == TileState.Empty ? "Mảnh Đất" : cropName;
```

---

### Task 4 — Prefab Wire
**WIRE-01:** Assign `GameDataRegistry.asset` → `_registry` field trong `ContextActionPanel.prefab`

---

## Critical Design Decisions

| Decision | Lý do |
|---|---|
| Direct prefab wiring cho `_registry` | Unity canonical SO pattern — no runtime lookup |
| `CloseContextAction()` với fallback `SetActive(false)` | Panel phải luôn đóng được dù PopupManager null |
| `RefreshVisuals()` ở cuối `RestoreState()` | Immediate visual feedback khi load |
| Chỉ fix bugs, không refactor | Giữ scope nhỏ, giảm risk regression |

---

## Architecture (unchanged)

```
Tap tile → WorldObjectPicker → PopupManager.ShowContextAction(tile)
         → CropActionPanelController.Setup(tile) → RefreshUI()
         → Player tap care button → CropTileView.ClearWeeds/Pests/Water()
           → RefreshVisuals() [A1 fix] → RefreshUI() [panel updates]
         → HandleTick: drainRate=0 → Growing [A3 fix]
         → growthProgress≥1 → Ripe + GrowthStage.Ripe [normal flow]
         → Harvest() → StorageSystem.AddItem() → CloseContextAction() [A4 fix]
           → TriggerSave()

Load game → RestoreState() → GrowthStage.Ripe nếu Ripe [A9 fix]
          → RefreshVisuals() [A6 fix] → visuals đúng ngay
```
