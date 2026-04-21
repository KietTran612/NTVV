# Design Document: m6b-world-progression

**Date:** 2026-04-20
**Spec:** m6b-world-progression
**Status:** approved
**Full design:** `docs/superpowers/specs/2026-04-20-m6-gameplay-features-design.md`

---

## Overview

2 features: offline growth animal fix (FEAT-05) và locked tile land expansion (FEAT-06). Không viết system mới lớn — chỉ fix 1 bug trong boot sequence và thêm tile lock layer.

**Files cần sửa/tạo:**

| File | Thay đổi |
|------|----------|
| `Assets/_Project/Scripts/Managers/GameManager.cs` | LastSaveTime fix, OnPlayerLevelUp, capture/restore tile lock |
| `Assets/_Project/Scripts/World/Views/CropTileView.cs` | IsLocked, RequiredLevel, Unlock(), lock overlay, HandleTick guard |
| `Assets/_Project/Scripts/World/WorldObjectPicker.cs` | Check `tile.IsLocked` trước ShowContextAction |
| `Assets/_Project/Scripts/UI/Common/PopupManager.cs` | `ShowLockInfo(int)` method |
| `Assets/_Project/Scripts/Core/SaveData.cs` | Thêm `unlockedTileIds` |
| `Assets/_Project/Scripts/UI/Panels/LockInfoPopupController.cs` | Tạo mới |
| `Assets/_Project/Resources/UI/Default/LockInfoPopup.prefab` | Tạo mới |

---

## FEAT-05 — Offline Growth Animal Fix

**Root cause:** `AnimalView.RestoreState()` tính offline time bằng:
```csharp
float offline = (float)(DateTime.UtcNow - Managers.GameManager.LastSaveTime).TotalSeconds;
```
Nhưng `GameManager.LastSaveTime` được khởi tạo = `DateTime.UtcNow` lúc class load → offline = 0.

**Fix đơn giản:** Trong `BootSequence`, set `LastSaveTime` từ save data **trước** `RestoreWorldState()`:
```csharp
PlayerSaveData saveData = SaveLoadManager.Instance.Load();
if (saveData != null && saveData.lastSaveTimestamp != 0)
    LastSaveTime = new System.DateTime(saveData.lastSaveTimestamp);
InitializeCoreSystems(saveData);
RestoreWorldState(saveData);
```
Ây đó: `AnimalView.RestoreState()` được gọi trong `SpawnAndRestore()` từ `RestoreWorldState()`, lúc đó `LastSaveTime` đã đúng.

**Crops:** `CropTileView.RestoreState()` đã xử lý đúng qua `plantTimestamp` → không cần sửa.

**Welcome toast:** sau `RestoreWorldState()`, tính `offlineSeconds = (DateTime.UtcNow - LastSaveTime).TotalSeconds`, nếu > 60 → find `LevelUpToastController`, gọi `ShowMessage("Chào mừng trở lại! ...")`. Dùng lại component từ m6a.

---

## FEAT-06 — Land Expansion (Locked Tiles)

**Kiến trúc:** Tile có sẵn trong scene, inspector set `_isLocked=true` + `_requiredLevel`. Tap → popup. Lên cấp đủ → `GameManager` tự unlock.

### CropTileView additions

```csharp
[Header("Lock")]
[SerializeField] private bool _isLocked;
[SerializeField] private int _requiredLevel;
[SerializeField] private GameObject _lockOverlay;

public bool IsLocked => _isLocked;
public int RequiredLevel => _requiredLevel;

public void Unlock()
{
    _isLocked = false;
    _lockOverlay?.SetActive(false);
    RefreshVisuals();
}
```
- `HandleTick()`: `if (_isLocked) return;` ở đầu
- `RefreshVisuals()`: thêm `_lockOverlay?.SetActive(_isLocked);`

### WorldObjectPicker fix

`OnTileSelected(CropTileView tile)` thêm guard:
```csharp
if (tile.IsLocked)
{
    PopupManager.Instance?.ShowLockInfo(tile.RequiredLevel);
    return;
}
```

### PopupManager — ShowLockInfo

```csharp
public void ShowLockInfo(int requiredLevel)
{
    if (_activeModal != null) Destroy(_activeModal);
    GameObject prefab = _provider.LoadPrefab("LockInfoPopup");
    if (prefab != null && _modalParent != null)
    {
        _activeModal = Instantiate(prefab, _modalParent);
        var ctrl = _activeModal.GetComponent<LockInfoPopupController>();
        ctrl?.Setup(requiredLevel);
    }
}
```

### LockInfoPopupController

```csharp
public void Setup(int requiredLevel)
{
    if (_message_Label != null)
        _message_Label.text = $"Cần Level {requiredLevel} để mở tile này";
}
// _btnOk.onClick → PopupManager.Instance.CloseActiveModal()
```

### Save/Load tile lock state

`PlayerSaveData`: thêm `public List<string> unlockedTileIds`.

`CaptureCurrentState()`: lưu ID của tile đã unlock.

`RestoreWorldState()`: tile đã unlock trong save → gọi `tile.Unlock()`.

### Auto-unlock on level up

`GameManager.OnInitialize()`: subscribe `LevelSystem.OnLevelUp += OnPlayerLevelUp`

```csharp
private void OnPlayerLevelUp(int newLevel)
{
    var tiles = FindObjectsByType<CropTileView>(FindObjectsSortMode.None);
    bool anyUnlocked = false;
    foreach (var tile in tiles)
    {
        if (tile.IsLocked && newLevel >= tile.RequiredLevel)
        {
            tile.Unlock();
            anyUnlocked = true;
        }
    }
    if (anyUnlocked) TriggerSave();
}
```

---

## Critical Design Decisions

| Decision | Lý do |
|----------|--------|
| Fix `LastSaveTime` trong BootSequence | Minimal change, không tạo API mới, `AnimalView` không thay đổi |
| Welcome toast tái sử dụng `LevelUpToastController` | Không dụ lý do tạo component mới cho mục đích tương tự |
| Lock logic trong `CropTileView` (không class ring) | Tile đã tồn tại, thêm fields đơn giản hơn wrapper mới |
| Guard trong `WorldObjectPicker` (không trong `PopupManager`) | WorldObjectPicker là điểm dispatch duy nhất — đúng chỗ nhất |
| `unlockedTileIds` lưu tile đã unlock (positive list) | Tương thích với tile mộc định `_isLocked=false` |
| `OnPlayerLevelUp` trong `GameManager` (không trong `LevelSystem`) | `GameManager` là coordinator, không trộn concern vào progression system |
