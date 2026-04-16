# Design Document: m3a-crop-care-harvest

**Date:** 2026-04-16
**Spec:** m3a-crop-care-harvest
**Status:** approved
**Kiro Spec Path:** `.kiro/specs/m3a-crop-care-harvest/`

---

## Overview

M3a hoàn thiện **vòng lặp chăm sóc và thu hoạch cây trồng** trong SCN_Main. Logic ailment và harvest đã tồn tại trong `CropTileView` nhưng có 4 bugs thực sự và 1 wiring thiếu khiến gameplay loop bị broken. Spec này fix các bugs đó và verify full cycle: plant → grow → ailment → care → ripe → harvest → items vào StorageSystem.

**Không thuộc scope M3a:**
- Badge/indicator UI trên tile khi cần chăm sóc (backlog)
- Storage/Sell flow (M3b)
- Animal care (separate spec)

---

## Bugs Cần Fix

### BUG-A1 (HIGH): Care actions không update visuals ngay lập tức
**File:** `Assets/_Project/Scripts/World/Views/CropTileView.cs:137-139`

`ClearWeeds()`, `ClearPests()`, `WaterPlant()` set flag về false nhưng không gọi `RefreshVisuals()`. Ailment visual (`_weedVisual`, `_bugVisual`, `_waterVisual`) tiếp tục hiển thị trên tile cho đến tick tiếp theo.

**Fix:**
```csharp
public void ClearWeeds()  { _hasWeeds = false;   StorageSystem.Instance?.AddItem("item_grass", 1); RefreshVisuals(); }
public void ClearPests()  { _hasPests = false;   StorageSystem.Instance?.AddItem("item_worm", 1);  RefreshVisuals(); }
public void WaterPlant()  { _needsWater = false;  RefreshVisuals(); }
```

---

### BUG-A2 (HIGH): NullRef khi StorageSystem chưa boot
**File:** `Assets/_Project/Scripts/World/Views/CropTileView.cs:137-138`

`StorageSystem.Instance.AddItem(...)` không có null check. Nếu StorageSystem chưa khởi động → NullReferenceException.

**Fix:** Dùng null-conditional operator `?.` như trên (fix chung với BUG-A1).

---

### BUG-A3 (MEDIUM): State kẹt NeedsCare sau khi hết ailments
**File:** `Assets/_Project/Scripts/World/Views/CropTileView.cs:282-302`

`HandleTick()` chỉ reset state khi `growthProgress >= 1f` (Ripe) hoặc `HP <= 0` (Dead). Khi player clear hết ailments nhưng cây chưa Ripe, state kẹt ở `NeedsCare` mãi. Ảnh hưởng save/load (state restore sai).

**Fix:** Thêm branch trong HandleTick:
```csharp
else if (_currentState == TileState.NeedsCare)
{
    _currentState = TileState.Growing; // Hết ailments → resume growing
}
```
Branch này nằm sau kiểm tra drainRate == 0, trước kiểm tra growthProgress >= 1f.

---

### BUG-A4 (MEDIUM): Autosave không trigger sau care/harvest
**File:** `Assets/_Project/Scripts/UI/Panels/CropActionPanelController.cs:48-58`

Close button và harvest button gọi `gameObject.SetActive(false)` trực tiếp, bỏ qua `PopupManager.CloseContextAction()` — nơi duy nhất gọi `GameManager.TriggerSave()`.

**Fix:** Thay `gameObject.SetActive(false)` bằng `PopupManager.Instance?.CloseContextAction()` trong:
- `_closeButton` listener
- `_harvestButton` listener  
- `_resetButton` listener (ClearDead)

Care buttons (water/cure/weed) không đóng panel (player có thể care nhiều ailment liên tiếp). Save chỉ xảy ra khi panel đóng (close/harvest/reset). Mất state care nếu game crash giữa chừng là acceptable cho v1.

---

### ISSUE-A5 (LOW): Harvest fail im lặng khi Storage đầy
**File:** `Assets/_Project/Scripts/World/Views/CropTileView.cs:117-127`

Khi `StorageSystem.CanAddItem()` trả về false, harvest bị skip hoàn toàn — không có log, không có UI feedback. Player không biết tại sao.

**Fix:** Thêm log warning:
```csharp
if (!StorageSystem.Instance.CanAddItem(_currentCropData.cropId, finalYield))
{
    Debug.LogWarning($"[Harvest] Storage full! Cannot add {finalYield}x {_currentCropData.cropName}.");
    return;
}
```
UI feedback đầy đủ (toast/alert) là backlog — log là đủ cho v1.

---

### BUG-A6 (HIGH): RestoreState không gọi RefreshVisuals() — visuals sai sau load
**File:** `Assets/_Project/Scripts/World/Views/CropTileView.cs:76-107`

`GameManager.RestoreWorldState()` gọi `RestoreState()` để phục hồi tile từ save. `RestoreState()` set `_hasWeeds`, `_hasPests`, `_needsWater` đúng từ save data nhưng không gọi `RefreshVisuals()`. Kết quả: player mở game lại, tile có ailments nhưng visual (`_weedVisual`, `_bugVisual`, `_waterVisual`) vẫn tắt — player không biết tile đang cần chăm sóc.

**Fix:** Thêm `RefreshVisuals()` ở cuối `RestoreState()`:
```csharp
// Cuối RestoreState(), sau UpdateStage():
RefreshVisuals();
```

---

### BUG-A7 (MEDIUM): NPE khi cropData null nhưng state không phải Empty
**File:** `Assets/_Project/Scripts/UI/Panels/CropActionPanelController.cs:102`

`GameManager.RestoreWorldState()` truyền `cropSO?.data` — có thể là `null` nếu cropId không còn trong registry (data thay đổi hoặc save cũ). Khi đó tile có `CurrentState = Growing/NeedsCare/Ripe` nhưng `CurrentCropData = null`. Player tap tile → `RefreshUI()` → NPE:
```csharp
_headerText.text = ... : _targetTile.CurrentCropData.cropName; // NPE!
```

**Fix:**
```csharp
string cropName = _targetTile.CurrentCropData?.cropName ?? "[Unknown]";
_headerText.text = _targetTile.CurrentState == CropTileView.TileState.Empty ? "Mảnh Đất" : cropName;
```

---

### BUG-A9 (LOW): RestoreState không set GrowthStage.Ripe — sprite sai khi load tile Ripe
**File:** `Assets/_Project/Scripts/World/Views/CropTileView.cs:95-105`

`RestoreState()` set `_currentState = TileState.Ripe` rồi gọi `UpdateStage()`. Nhưng `UpdateStage()` chỉ set Phase1/2/3, không bao giờ set `GrowthStage.Ripe`. Kết quả: tile Ripe sau load hiển thị sprite Phase3 (xanh đậm) thay vì Ripe (vàng) — player không nhận ra tile đã chín nhìn từ bên ngoài.

So sánh: trong `HandleTick`, khi tile chín tự nhiên, `_currentStage = GrowthStage.Ripe` được set đúng (line 300).

**Fix:** Thêm `_currentStage = GrowthStage.Ripe;` ngay sau `_currentState = TileState.Ripe;` trong `RestoreState()`.

---

### BUG-A10 (MEDIUM): Tile chết vì quá hạn (Ripe→Dead) không gọi RefreshVisuals
**File:** `Assets/_Project/Scripts/World/Views/CropTileView.cs:258-263`

Khi `timeSinceRipe > LifeAfterRipeInSeconds`, code set `_currentState = TileState.Dead` rồi `return` ngay — không gọi `RefreshVisuals()`. Tick tiếp theo cũng exit sớm (Dead state). Kết quả: dead sprite không bao giờ hiển thị khi tile chết theo thời gian — tile nhìn vẫn như đang Ripe cho đến khi player tap vào.

So sánh: HP-based death (line 287-290) gọi `RefreshVisuals()` đúng.

**Fix:**
```csharp
if (_timeSinceRipe > _currentCropData.LifeAfterRipeInSeconds)
{
    _currentState = TileState.Dead;
    RefreshVisuals(); // ADD
}
```

---

### NOTE-A8 (Design — không fix trong M3a): Offline HP drain không simulate
`RestoreState()` tính offline elapsed time để grow crop nhưng không simulate offline HP drain. Tile có ailments khi player thoát → HP đóng băng ở giá trị lúc thoát, không tiếp tục drain offline. Chấp nhận được cho v1 — simulate offline tick history là over-engineering.

---

## Wiring Cần Làm

### WIRE-01: Assign `_registry` trong ContextActionPanel prefab
**File:** `Assets/_Project/Resources/UI/Default/ContextActionPanel.prefab`

`CropActionPanelController._registry` là `[SerializeField]` — cần assign `GameDataRegistry.asset` trực tiếp trong prefab Inspector. Khi PopupManager instantiate prefab, reference được preserve tự động.

- `_registry` → `Assets/_Project/Data/GameDataRegistry.asset`

---

## Architecture

```
Player tap tile
    ↓
WorldObjectPicker.OnTap() → Physics.Raycast → CropTileView hit
    ↓
PopupManager.ShowContextAction(tile)
    → EnsureContextPanel() → load ContextActionPanel.prefab (registry pre-wired)
    → CropActionPanelController.Setup(tile) → RefreshUI()
    ↓
Player taps care button
    → CropTileView.ClearWeeds/ClearPests/WaterPlant()
    → RefreshVisuals() [BUG-A1 fix]
    → StorageSystem.Instance?.AddItem() [BUG-A2 fix]
    → RefreshUI() (buttons update)
    ↓
HandleTick (tiếp tục):
    → drainRate = 0 → state → Growing [BUG-A3 fix]
    → growthProgress = 1f → state → Ripe → RefreshVisuals()
    ↓
Player taps harvest
    → CropTileView.Harvest()
    → StorageSystem.AddItem(cropId, finalYield)
    → LevelSystem.AddXP()
    → QuestEvents.InvokeActionPerformed(HarvestCrop)
    → ResetTile() → state → Empty
    → PopupManager.CloseContextAction() [BUG-A4 fix]
    → GameManager.TriggerSave()
```

---

## Critical Design Decisions

| Decision | Lý do |
|----------|-------|
| `RefreshVisuals()` trong care methods | Immediate feedback, không chờ tick |
| Null-conditional `?.` cho StorageSystem | Defensive — boot order không đảm bảo |
| Dùng `CloseContextAction()` thay `SetActive(false)` | Đảm bảo autosave luôn chạy khi đóng panel |
| Log warning thay UI toast cho storage full | Scope nhỏ gọn, UI feedback là M3b scope |
| Direct prefab wiring cho `_registry` | Unity canonical SO pattern — không cần runtime lookup |

---

## Task Summary (11 tasks)

| Task | Mô tả | Loại |
|------|-------|------|
| 0 | Fix BUG-A1 + BUG-A2: ClearWeeds/ClearPests/WaterPlant + RefreshVisuals + null-safe | Script fix |
| 1 | Fix BUG-A3: HandleTick reset NeedsCare → Growing khi hết ailments | Script fix |
| 2 | Fix BUG-A10: Ripe→Dead qua thời gian gọi RefreshVisuals() | Script fix |
| 3 | Fix BUG-A4: Close/Harvest/Reset dùng CloseContextAction() thay SetActive(false) | Script fix |
| 4 | Fix BUG-A5: Log warning khi harvest fail (storage đầy) | Script fix |
| 5 | Fix BUG-A6 + BUG-A9: RestoreState gọi RefreshVisuals() + set GrowthStage.Ripe | Script fix |
| 6 | Fix BUG-A7: Null-safe cropName trong CropActionPanelController.RefreshUI() | Script fix |
| 7 | WIRE-01: Assign GameDataRegistry.asset → _registry trong ContextActionPanel.prefab | Wiring |
| 8 | Smoke test: plant → ailment → care → ripe → harvest | Test |
| 9 | Save/load verify: load tile Ripe → visual đúng, load tile NeedsCare → visuals đúng | Test |
| 10 | Update HANDOVER.md | Docs |

---

## Test Thành Công Khi

1. Player care tile → ailment visual tắt **ngay lập tức** (không chờ tick)
2. State tile chuyển `NeedsCare → Growing` sau khi hết ailments (verify trong Inspector)
3. Harvest → item xuất hiện trong StorageSystem → console log `[Harvest] Success!`
4. Đóng panel sau harvest → `[GameManager] Save triggered` xuất hiện trong console
5. StorageSystem full → console log warning thay vì crash
6. Load game → tile có ailments → **visuals hiển thị đúng ngay lập tức** (BUG-A6)
7. Load game → tile Ripe → **hiển thị màu vàng/Ripe sprite**, không phải Phase3 (BUG-A9)
8. Tile chín xong → chờ hết LifeAfterRipe → **dead sprite hiển thị ngay** không cần tap (BUG-A10)
9. Tap tile có cropData null (edge case) → panel mở không crash, hiển thị "[Unknown]" (BUG-A7)

---

## References

- `Assets/_Project/Scripts/World/Views/CropTileView.cs` — logic chính
- `Assets/_Project/Scripts/UI/Panels/CropActionPanelController.cs` — UI panel
- `Assets/_Project/Scripts/UI/Common/PopupManager.cs` — panel lifecycle + CloseContextAction()
- `Assets/_Project/Scripts/Managers/GameManager.cs` — RestoreWorldState(), TriggerSave()
- `Assets/_Project/Scripts/Gameplay/Storage/StorageSystem.cs` — CanAddItem(), AddItem()
- `Assets/_Project/Resources/UI/Default/ContextActionPanel.prefab` — prefab cần wire
- `docs/backlog/bug-backlog.md` — BUG-08, BUG-09 liên quan
- `.kiro/specs/scn-main-world-setup/` — M2 prerequisite
