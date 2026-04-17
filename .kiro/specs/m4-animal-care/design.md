# Design Document: m4-animal-care

**Date:** 2026-04-17
**Spec:** m4-animal-care
**Status:** approved
**Full design:** `docs/superpowers/specs/2026-04-17-m4-animal-care-design.md`

---

## Overview

Fix 7 bugs + thêm Save/Load + auto-collect. Không viết script mới.

**Files cần sửa:**
| File | Bugs/Task |
|---|---|
| `Assets/_Project/Scripts/World/Views/AnimalView.cs` | BUG-01, BUG-02, BUG-03, BUG-04, BUG-07, Save/Load, Auto-collect |
| `Assets/_Project/Scripts/World/Views/AnimalPenView.cs` | PERF-PEN (_registry), SpawnAndRestore |
| `Assets/_Project/Scripts/UI/Panels/CropActionPanelController.cs` | BUG-05, BUG-06, hide _collectButton |
| `Assets/_Project/Data/PlayerSaveData.cs` | Thêm AnimalSaveData class + animals List |
| `Assets/_Project/Scripts/Managers/GameManager.cs` | CollectSaveData + RestoreWorldState |
| `Assets/_Project/Resources/UI/Default/AnimalPen.prefab` | WIRE-01 |

---

## Bug Fix Summary

### BUG-02 (CRITICAL) — RemoveAnimal không được gọi trong Sell() và natural death

`Sell()` gọi `Destroy(gameObject)` nhưng không gọi `AnimalPenView.RemoveAnimal(this)` → `_currentAnimals` không shrink → `IsFull` stuck true mãi mãi.

**Fix:** Gọi `_pen?.RemoveAnimal(this)` TRƯỚC `Destroy(gameObject)` trong tất cả exit paths:
```csharp
public void Sell()
{
    // ... existing XP + economy logic ...
    NTVV.Gameplay.Quests.QuestEvents.InvokeActionPerformed(Data.QuestActionType.SellAnimal, _data.animalId, 1); // BUG-07
    Managers.GameManager.Instance?.TriggerSave();
    _pen?.RemoveAnimal(this); // ADD: trước Destroy
    Destroy(gameObject);
}
```

---

### BUG-01 (HIGH) — Production block chạy trong cùng tick với natural death

Khi animal chết vì quá tuổi, production block vẫn chạy trước `return`.

**Fix:**
```csharp
if (_timeSinceRipe > _currentData.lifeAfterRipeInSeconds)
{
    _currentStage = GrowthStage.Dead;
    RefreshVisuals();
    _pen?.RemoveAnimal(this); // BUG-02
    return;                   // production block không chạy
}
```

---

### BUG-03 + BUG-04 (HIGH + MEDIUM) — Feed() crash + fail im lặng

`StorageSystem.Instance.GetItemCount()` không có null check → NPE. Khi thiếu food → return mà không log.

**Fix:**
```csharp
public void Feed()
{
    if (StorageSystem.Instance == null)
    {
        Debug.LogWarning("[AnimalView] StorageSystem not ready.");
        return;
    }
    int grassCount = StorageSystem.Instance.GetItemCount("item_grass");
    int wormCount  = StorageSystem.Instance.GetItemCount("item_worm");
    if (grassCount < _data.feedQtyGrass || wormCount < _data.feedQtyWorm)
    {
        Debug.LogWarning($"[Animal] Not enough food to feed {_data.animalName}. Need {_data.feedQtyGrass}x grass + {_data.feedQtyWorm}x worm.");
        return;
    }
    // ... existing feed logic ...
}
```

---

### BUG-05 (HIGH) — _sellButton dùng SetActive(false) → không autosave

**Fix:** `gameObject.SetActive(false)` → `PopupManager.Instance?.CloseContextAction()`

---

### BUG-06 (HIGH) — _buyButton không trigger save

**Fix:** Thêm `Managers.GameManager.Instance?.TriggerSave()` sau `PurchaseAnimal()`.

---

### BUG-07 (LOW) — Sell() không gọi QuestEvent

**Fix:** Thêm `QuestEvents.InvokeActionPerformed(QuestActionType.SellAnimal, _data.animalId, 1)` trong `Sell()` trước Destroy. (Xem code snippet BUG-02)

---

### PERF-PEN (MEDIUM) — FindObjectsOfTypeAll trong AnimalPenView.Awake()

**Fix:**
```csharp
[Header("Data")]
[SerializeField] private GameDataRegistrySO _registry;

private void Awake()
{
    if (_registry == null)
        Debug.LogError("[AnimalPenView] _registry is null — assign GameDataRegistry.asset in prefab Inspector.");
    // ... rest of Awake ...
}
```
Xóa `Resources.FindObjectsOfTypeAll<GameDataRegistrySO>().FirstOrDefault()`. Wire trong `AnimalPen.prefab`.

---

## Save/Load

**AnimalSaveData** (thêm vào `PlayerSaveData.cs`):
```csharp
[Serializable]
public class AnimalSaveData
{
    public string animalId;
    public int    stage;
    public float  hp;
    public float  timeInCurrentStage;
    public float  timeSinceLastProduction;
}
// Trong PlayerSaveData:
public List<AnimalSaveData> animals = new();
```

**Save:** `GameManager.CollectSaveData()` → `FindObjectsOfType<AnimalView>().Select(a => a.GetSaveData()).ToList()`

**Load:** `GameManager.RestoreWorldState()` → lookup animalSO từ registry → `AnimalPenView.SpawnAndRestore(animalSO, data)` (không trừ gold).

**RestoreState():** set stage/hp/timers + cộng offline time + `RefreshVisuals()`.

---

## Auto-collect

`HandleTick()` tự gọi `CollectProduct()` khi production timer đủ. `_collectButton` bị hide trong `CropActionPanelController.RefreshUI()`.

---

## Critical Design Decisions

| Decision | Lý do |
|----------|-------|
| `RemoveAnimal()` trước `Destroy()` trong mọi exit path | IsFull phải reflect thực tế |
| Double save khi sell | Acceptable — không gây lỗi |
| Auto-collect thay collect button | Scope nhỏ gọn cho v1 |
| AnimalSaveData inline trong PlayerSaveData | Đồng nhất với CropTileData pattern |
