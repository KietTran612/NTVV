# Design Document: m4-animal-care

**Date:** 2026-04-17
**Spec:** m4-animal-care
**Status:** approved
**Kiro Spec Path:** `.kiro/specs/m4-animal-care/`

---

## Overview

M4 hoàn thiện **vòng lặp chăm sóc và bán động vật** trong SCN_Main. Logic animal đã tồn tại đầy đủ trong `AnimalView` và `AnimalPenView` nhưng có 7 bugs thực sự (2 critical, 4 high, 1 low) khiến: pen bị stuck full mãi mãi, autosave không chạy sau sell/buy, crash khi feed, và quest không trigger khi bán. Spec này fix tất cả bugs, thêm Save/Load per-animal, tự động collect product trong HandleTick, và loại bỏ `FindObjectsOfTypeAll` pattern.

**Không thuộc scope M4:**
- Badge/indicator UI trên pen khi animal cần chăm sóc (backlog)
- Collect product UI button (backlog — auto-collect đủ cho v1)
- Animal detail panel riêng biệt (AnimalDetailPanelController — backlog)
- Offline HP drain simulation (NOTE-A8 pattern — over-engineering cho v1)
- Shop rotate / gem currency (v2)

---

## Bugs Cần Fix

### BUG-01 (HIGH): Natural death không guard production block
**File:** `Assets/_Project/Scripts/World/Views/AnimalView.cs` HandleTick ~line 67-70

Khi animal chết vì quá tuổi (`timeSinceRipe > lifeAfterRipe`), code set `_currentStage = GrowthStage.Dead` rồi `return` — nhưng production block vẫn chạy trong cùng tick trước khi reach `return`. Minor nhưng cần guard để tránh item drop khi dead.

**Fix:**
```csharp
if (_timeSinceRipe > _currentData.lifeAfterRipeInSeconds)
{
    _currentStage = GrowthStage.Dead;
    RefreshVisuals();
    _pen?.RemoveAnimal(this); // BUG-02 fix chung
    return;
}
```

---

### BUG-02 (CRITICAL): Sell() và natural death không gọi RemoveAnimal() — IsFull stuck true mãi mãi
**File:** `Assets/_Project/Scripts/World/Views/AnimalView.cs` Sell() ~line 214-241, HandleTick ~line 67-70

`Sell()` gọi `Destroy(gameObject)` ở mọi trường hợp (Baby sell, Mature sell) nhưng **không bao giờ** gọi `AnimalPenView.RemoveAnimal(this)`. Kết quả: `_currentAnimals` list không shrink → `IsFull` trả về `true` mãi mãi → player không thể mua thêm animal dù pen đã trống về mặt visual. Natural death cũng không gọi `RemoveAnimal(this)` — cùng vấn đề.

**Fix:** Gọi `_pen?.RemoveAnimal(this)` TRƯỚC `Destroy(gameObject)` trong tất cả exit paths:
```csharp
public void Sell()
{
    // ... existing XP + economy logic ...
    QuestEvents.InvokeActionPerformed(QuestActionType.SellAnimal, _data.animalId, 1); // BUG-07
    Managers.GameManager.Instance?.TriggerSave();
    _pen?.RemoveAnimal(this); // ADD: trước Destroy
    Destroy(gameObject);
}
```

---

### BUG-03 (HIGH): Feed() không null-check StorageSystem.Instance
**File:** `Assets/_Project/Scripts/World/Views/AnimalView.cs` Feed() ~line 175-194

`StorageSystem.Instance.GetItemCount(...)` gọi trực tiếp — nếu StorageSystem chưa boot → NullReferenceException crash toàn game.

**Fix:** Dùng null-conditional + early return:
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
    // ... rest of Feed logic ...
}
```

---

### BUG-04 (MEDIUM): Feed() fail im lặng khi không đủ food
**File:** `Assets/_Project/Scripts/World/Views/AnimalView.cs` Feed() ~line 175-194

Khi `grassCount < feedQty || wormCount < feedQty`, hàm return mà không log gì. Player không biết lý do feed thất bại — trải nghiệm người dùng kém.

**Fix:**
```csharp
if (grassCount < _data.feedQtyGrass || wormCount < _data.feedQtyWorm)
{
    Debug.LogWarning($"[Animal] Not enough food to feed {_data.animalName}. Need {_data.feedQtyGrass}x grass + {_data.feedQtyWorm}x worm.");
    return;
}
```

---

### BUG-05 (HIGH): _sellButton dùng SetActive(false) — không autosave sau khi bán animal
**File:** `Assets/_Project/Scripts/UI/Panels/CropActionPanelController.cs` _sellButton listener

`_targetAnimal?.Sell(); gameObject.SetActive(false);` — bypass `PopupManager.CloseContextAction()` → `GameManager.TriggerSave()` không được gọi. Nếu game crash sau khi bán, animal biến mất nhưng gold không được save.

**Fix:** Thay `gameObject.SetActive(false)` → `PopupManager.Instance?.CloseContextAction()`:
```csharp
_sellButton.onClick.AddListener(() =>
{
    _targetAnimal?.Sell();
    PopupManager.Instance?.CloseContextAction(); // thay SetActive(false)
});
```

**Lưu ý:** `Sell()` đã gọi `TriggerSave()` trực tiếp (sau BUG-02 fix). `CloseContextAction()` gọi `TriggerSave()` lần thứ 2 — double save acceptable, không gây lỗi.

---

### BUG-06 (HIGH): _buyButton không trigger save sau PurchaseAnimal()
**File:** `Assets/_Project/Scripts/UI/Panels/CropActionPanelController.cs` _buyButton listener

`_targetPen?.PurchaseAnimal(); RefreshUI();` — không có `GameManager.TriggerSave()`. Nếu game crash sau khi mua, gold bị trừ nhưng animal không tồn tại sau load.

**Fix:**
```csharp
_buyButton.onClick.AddListener(() =>
{
    _targetPen?.PurchaseAnimal();
    Managers.GameManager.Instance?.TriggerSave(); // ADD
    RefreshUI();
});
```

---

### BUG-07 (LOW): Sell() không gọi QuestEvent
**File:** `Assets/_Project/Scripts/World/Views/AnimalView.cs` Sell()

`Feed()` gọi `QuestEvents.InvokeActionPerformed(FeedAnimal, ...)` nhưng `Sell()` không có event tương ứng → quest "Sell N animals" không bao giờ trigger.

**Fix:** Thêm vào Sell() trước Destroy:
```csharp
NTVV.Gameplay.Quests.QuestEvents.InvokeActionPerformed(Data.QuestActionType.SellAnimal, _data.animalId, 1);
```

---

### PERF-PEN (MEDIUM): AnimalPenView.Awake() dùng FindObjectsOfTypeAll
**File:** `Assets/_Project/Scripts/World/Views/AnimalPenView.cs` Awake()

`Resources.FindObjectsOfTypeAll<GameDataRegistrySO>().FirstOrDefault()` — expensive scan mỗi lần pen awake. Pattern giống M3b — fix bằng SerializeField.

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

## Save/Load Architecture

### AnimalSaveData

Thêm nested class trong `PlayerSaveData` (không tạo file mới):
```csharp
[Serializable]
public class AnimalSaveData
{
    public string animalId;                  // lookup SO từ registry
    public int    stage;                     // GrowthStage enum as int
    public float  hp;
    public float  timeInCurrentStage;        // resume growth tự nhiên
    public float  timeSinceLastProduction;   // resume production timer
}

// Trong PlayerSaveData:
public List<AnimalSaveData> animals = new();
```

### Save Flow

`GameManager.TriggerSave()` → `SaveLoadManager.Save()` → collect:
```csharp
// Trong GameManager.CollectSaveData():
saveData.animals = FindObjectsOfType<AnimalView>()
    .Select(a => a.GetSaveData())
    .ToList();
```

`AnimalView.GetSaveData()`:
```csharp
public AnimalSaveData GetSaveData() => new AnimalSaveData
{
    animalId              = _data.animalId,
    stage                 = (int)_currentStage,
    hp                    = _hp,
    timeInCurrentStage    = _timeInCurrentStage,
    timeSinceLastProduction = _timeSinceLastProduction,
};
```

### Load Flow

`GameManager.RestoreWorldState()` — sau khi restore crops:
```csharp
foreach (var saved in saveData.animals)
{
    var animalSO = _registry.animals.FirstOrDefault(a => a.data.animalId == saved.animalId);
    if (animalSO == null) continue; // stale save — bỏ qua

    var pen = FindPenWithSpace();
    pen?.SpawnAndRestore(animalSO, saved); // không trừ gold
}
```

`AnimalView.RestoreState(AnimalSaveData data)`:
```csharp
public void RestoreState(AnimalSaveData data)
{
    _currentStage             = (GrowthStage)data.stage;
    _hp                       = data.hp;
    _timeInCurrentStage       = data.timeInCurrentStage;
    _timeSinceLastProduction  = data.timeSinceLastProduction;

    // Offline growth: advance timers
    float offline = (float)(DateTime.UtcNow - GameManager.LastSaveTime).TotalSeconds;
    _timeInCurrentStage += offline; // HandleTick sẽ advance stage nếu đủ
    _timeSinceLastProduction += offline;

    RefreshVisuals();
}
```

**Offline HP drain:** Không simulate (acceptable for v1 — như NOTE-A8 trong M3a).

---

## CollectProduct — Auto-collect trong HandleTick

**Decision:** Hide collect UI button, logic chạy tự động.

`AnimalView.HandleTick()` — production timer:
```csharp
if (_currentStage == GrowthStage.Mature)
{
    _timeSinceLastProduction += deltaTime;
    if (_timeSinceLastProduction >= _data.produceTimeMin * 60f)
    {
        CollectProduct(); // auto-collect
        _timeSinceLastProduction = 0f;
    }
}
```

`CropActionPanelController.RefreshUI()` — ẩn collect button:
```csharp
if (_collectButton != null) _collectButton.gameObject.SetActive(false);
```

Không xóa button khỏi prefab — chỉ hide trong code để dễ enable sau.

---

## Wiring Cần Làm

### WIRE-01: AnimalPen.prefab → _registry
Assign `GameDataRegistry.asset` vào `AnimalPenView._registry` trong prefab Inspector.

---

## Architecture

```
Player tap pen
    ↓
WorldObjectPicker.OnTap() → AnimalPenView hit
    ↓
PopupManager.ShowContextAction(pen)
    → CropActionPanelController.Setup(pen)
    → RefreshUI(): hiển thị Buy button nếu pen có chỗ, hide Collect
    ↓
Player tap Buy
    → AnimalPenView.PurchaseAnimal()  ← trừ gold + spawn AnimalView
    → GameManager.TriggerSave()       ← BUG-06 fix
    → RefreshUI()

Player tap animal
    ↓
PopupManager.ShowContextAction(animal)
    → CropActionPanelController.Setup(animal)
    → RefreshUI(): Feed button + Sell button
    ↓
Player tap Feed
    → AnimalView.Feed()   ← null-safe (BUG-03), log warning (BUG-04)
    → RefreshUI()

Player tap Sell
    → AnimalView.Sell()                      ← QuestEvent (BUG-07)
    → _pen.RemoveAnimal(this)                ← BUG-02 fix
    → GameManager.TriggerSave()              ← bên trong Sell()
    → Destroy(gameObject)
    → PopupManager.CloseContextAction()      ← BUG-05 fix (2nd save OK)

HandleTick (mỗi game tick):
    → Grow stage nếu đủ thời gian
    → Natural death → RemoveAnimal() → RefreshVisuals() (BUG-02 fix)
    → Auto-collect product khi timer đủ

Save: GameManager.TriggerSave() → AnimalView.GetSaveData() × N animals
Load: RestoreWorldState() → AnimalView.RestoreState() + offline growth advance
```

---

## Critical Design Decisions

| Decision | Lý do |
|----------|-------|
| `RemoveAnimal()` trước `Destroy()` trong mọi exit path | Đảm bảo `_currentAnimals` list luôn accurate — IsFull phải reflect thực tế |
| Double save khi sell (Sell() + CloseContextAction()) | Acceptable — không gây lỗi, đảm bảo atomicity |
| Auto-collect thay collect button | Scope nhỏ gọn, UX đơn giản hơn cho v1 |
| Offline growth advance nhưng không drain HP offline | Cân bằng UX: cây lớn offline là reward, HP drain offline là punishment — v1 chỉ reward |
| AnimalSaveData inline trong PlayerSaveData | Đồng nhất với CropTileData pattern, không tạo file mới |
| `FindPenWithSpace()` thay vì save pen reference | Pen reference không đáng tin khi load — tìm theo capacity là safer |

---

## Task Summary (11 tasks)

| Task | Mô tả | Loại |
|------|-------|------|
| 0 | Fix BUG-02: RemoveAnimal() trong Sell() tất cả paths | Script fix |
| 1 | Fix BUG-01: Production guard trong natural death | Script fix |
| 2 | Fix BUG-03 + BUG-04: Feed() null-safe + warning | Script fix |
| 3 | Fix BUG-07: QuestEvent trong Sell() | Script fix |
| 4 | Fix BUG-05: _sellButton → CloseContextAction() | Script fix |
| 5 | Fix BUG-06: _buyButton → TriggerSave() | Script fix |
| 6 | PERF-PEN: AnimalPenView → _registry SerializeField | Script fix |
| 7 | Save/Load: AnimalSaveData + GetSaveData() + RestoreState() + FindPenWithSpace() + SpawnAndRestore() | Script fix |
| 8 | Auto-collect trong HandleTick + hide _collectButton | Script fix |
| 9 | WIRE-01: AnimalPen.prefab → _registry | Wiring |
| 10 | Integration Smoke Test + Update HANDOVER.md | Test + Docs |

---

## Test Thành Công Khi

1. Mua animal → TriggerSave() log xuất hiện → load lại → animal vẫn tồn tại
2. Feed animal khi đủ food → HP tăng, food bị trừ khỏi Storage
3. Feed animal khi thiếu food → LogWarning `"[Animal] Not enough food"` → HP không thay đổi
4. Bán animal → `_currentAnimals` shrink → có thể mua animal mới ngay
5. Bán animal → TriggerSave() → load lại → animal không còn, gold được giữ
6. Animal chết tự nhiên → `_currentAnimals` shrink → pen không stuck full
7. Sau production timer → item xuất hiện trong Storage tự động (không cần tap)
8. Load game → animal ở đúng stage → visual đúng
9. Console: 0 NullReferenceException liên quan AnimalView hoặc StorageSystem
10. QuestEvent `SellAnimal` trigger khi bán — quest progress update

---

## References

- `Assets/_Project/Scripts/World/Views/AnimalView.cs` — logic chính
- `Assets/_Project/Scripts/World/Views/AnimalPenView.cs` — pen management
- `Assets/_Project/Scripts/UI/Panels/CropActionPanelController.cs` — UI panel (dùng chung)
- `Assets/_Project/Scripts/Managers/GameManager.cs` — RestoreWorldState(), TriggerSave()
- `Assets/_Project/Scripts/Gameplay/Storage/StorageSystem.cs` — food inventory
- `Assets/_Project/Data/PlayerSaveData.cs` — save data struct
- `.kiro/specs/m3b-storage-sell-flow/` — M3b prerequisite (items phải có trong Storage)
