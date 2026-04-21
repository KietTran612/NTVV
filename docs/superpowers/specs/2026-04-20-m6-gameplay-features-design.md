# Design: m6a-player-feedback + m6b-world-progression

**Ngày**: 2026-04-20  
**Trạng thái**: Approved  
**Liên quan**: FEAT-01, FEAT-05, FEAT-06, FEAT-07 từ `docs/backlog/bug-backlog.md`

---

## Context

Game NTVV đã hoàn thành 5 milestones (UI, World, Crop, Storage/Shop, Animals, Quests). Bước tiếp theo là hoàn thiện gameplay loop với 4 features còn thiếu trong Group B:

- **FEAT-01**: Level-up UI feedback — `LevelSystem.OnLevelUp` không ai subscribe
- **FEAT-05**: Offline growth — animal không tiến triển khi tắt game (bug `GameManager.LastSaveTime`)
- **FEAT-06**: Land expansion — unlock tile theo level milestone
- **FEAT-07**: Gems system — implement gems cho Shop Refresh Button

Chia thành 2 specs nhỏ theo layer: UI feedback (m6a) và gameplay logic (m6b).

---

## Spec 1: `m6a-player-feedback`

### FEAT-01 — Level-up Toast

**File mới**: `Assets/_Project/Scripts/UI/HUD/LevelUpToastController.cs`

- `OnEnable()` subscribe `LevelSystem.OnLevelUp += ShowToast`
- `OnDisable()` unsubscribe
- `ShowToast(int level)`: set text "⬆ Lên cấp {level}!", `gameObject.SetActive(true)`, `StartCoroutine(FadeOut())`
- `FadeOut()`: fade `CanvasGroup.alpha` 1→0 trong 2 giây, sau đó `gameObject.SetActive(false)`
- **Không dùng** `UIAnimationHelper.PopIn()` vì `StopAllCoroutines()` sẽ xung đột với animation khác trên singleton đó

**Scene wiring**:
- Thêm GO `LevelUpToast` con của `[HUD_CANVAS]`
- Attach: `LevelUpToastController`, `CanvasGroup`, `TMP_Text` (text hiển thị)
- `SetActive(false)` ban đầu

---

### FEAT-07 — Gems → Shop Refresh

**Phân tích**: `EconomySystem` đã có `AddGems()`, `CanAffordGems()`, `SetGems()`. Nhưng `PlayerSaveData` chưa có field `gems` → gems reset về 25 mỗi lần restart. `ShopPanelController.TryRefreshItems()` hiện dùng gold.

**File sửa 1**: `Assets/_Project/Scripts/Core/SaveData.cs`
```csharp
public class PlayerSaveData
{
    // ... existing fields ...
    public int gems;  // ADD
    
    public PlayerSaveData()
    {
        // ... existing ...
        gems = 25;  // ADD: default starting gems
    }
}
```

**File sửa 2**: `Assets/_Project/Scripts/Managers/GameManager.cs`
- `InitializeCoreSystems()`: thêm `EconomySystem.Instance.SetGems(data.gems);`
- `CaptureCurrentState()`: thêm `data.gems = EconomySystem.Instance.CurrentGems;`

**File sửa 3**: `Assets/_Project/Scripts/UI/Panels/ShopPanelController.cs`
- Đổi `[SerializeField] private int _refreshCostGold = 50;` → `_refreshCostGems = 50`
- `TryRefreshItems()`: đổi `CanAfford(_refreshCostGold)` → `CanAffordGems(_refreshCostGems)` và `AddGold(-_refreshCostGold)` → `AddGems(-_refreshCostGems)`

**Scene wiring**:
- `GemsBalance_Label` trong ShopPopup: `SetActive(true)`
- `Refresh_Button`: thêm TMP_Text child với text "Làm mới (50💎)"

---

## Spec 2: `m6b-world-progression`

### FEAT-05 — Offline Growth (Animal fix)

**Root cause**:
- `CropTileView.RestoreState()` đã tự tính elapsed time từ `plantTimestamp` → crops xử lý offline đúng, không cần sửa
- `AnimalView.RestoreState()` dùng `GameManager.LastSaveTime` để tính offline time. Nhưng `LastSaveTime` được set = `DateTime.UtcNow` lúc class load → offline time luôn = 0

**Fix**: `GameManager.BootSequence()` — set `LastSaveTime` từ save data **trước** khi gọi `RestoreWorldState()`:

```csharp
PlayerSaveData saveData = SaveLoadManager.Instance.Load();

// ADD: fix offline time cho AnimalView.RestoreState()
if (saveData != null && saveData.lastSaveTimestamp != 0)
    LastSaveTime = new System.DateTime(saveData.lastSaveTimestamp);

InitializeCoreSystems(saveData);
RestoreWorldState(saveData);
```

**Welcome toast** (nếu offline > 60s):
- Tính `offlineSeconds = (DateTime.UtcNow - LastSaveTime).TotalSeconds`
- Nếu > 60: tìm `LevelUpToastController` trong scene, gọi `ShowMessage("Chào mừng trở lại! Farm đã tiến triển {X} giờ")` 
- Mở rộng `LevelUpToastController` thêm method `ShowMessage(string msg)` để tái sử dụng

---

### FEAT-06 — Land Expansion (Locked Tiles)

**Kiến trúc**: Tiles locked có sẵn trong scene, tap hiện popup "Cần Level X", tự unlock khi lên đủ cấp.

**File sửa 1**: `Assets/_Project/Scripts/World/Views/CropTileView.cs`
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
- `HandleTick()`: thêm `if (_isLocked) return;` ở đầu
- `RefreshVisuals()`: thêm `_lockOverlay?.SetActive(_isLocked);`

**File sửa 2**: `Assets/_Project/Scripts/World/WorldObjectPicker.cs`
- `OnTileSelected(CropTileView tile)`:
```csharp
if (tile.IsLocked)
{
    PopupManager.Instance?.ShowLockInfo(tile.RequiredLevel);
    return;
}
// existing: PopupManager.Instance.ShowContextAction(tile)
```

**File sửa 3**: `Assets/_Project/Scripts/UI/Common/PopupManager.cs`
- Thêm method:
```csharp
public void ShowLockInfo(int requiredLevel)
{
    // Spawn LockInfoPopup prefab từ Resources
    GameObject prefab = _provider.LoadPrefab("LockInfoPopup");
    if (prefab != null && _modalParent != null)
        _activeModal = Instantiate(prefab, _modalParent);
    // Controller tự inject requiredLevel qua text
}
```

**Prefab mới**: `Assets/_Project/Resources/UI/Default/LockInfoPopup.prefab`
- TMP_Text: "Cần Level {X} để mở tile này"
- Button "OK" → `PopupManager.Instance.CloseActiveModal()`
- Script `LockInfoPopupController.cs`: nhận level từ PopupManager và set text

**File sửa 4**: `Assets/_Project/Scripts/Core/SaveData.cs`
```csharp
public List<string> unlockedTileIds = new List<string>(); // ADD
```
Constructor: `unlockedTileIds = new List<string>();`

**File sửa 5**: `Assets/_Project/Scripts/Managers/GameManager.cs`
- `CaptureCurrentState()`: thu thập tile nào đã unlock
```csharp
data.unlockedTileIds = allTiles
    .Where(t => !t.IsLocked)
    .Select(t => string.IsNullOrEmpty(t.TileId) ? t.gameObject.name : t.TileId)
    .ToList();
```
- `RestoreWorldState()`: restore locked state
```csharp
foreach (var tile in allTileViews)
{
    string id = string.IsNullOrEmpty(tile.TileId) ? tile.gameObject.name : tile.TileId;
    if (tile.IsLocked && data.unlockedTileIds.Contains(id))
        tile.Unlock();
}
```
- Subscribe unlock on level up (trong `OnInitialize` hoặc `Awake`):
```csharp
LevelSystem.OnLevelUp += OnPlayerLevelUp;

private void OnPlayerLevelUp(int newLevel)
{
    var tiles = FindObjectsByType<CropTileView>(FindObjectsSortMode.None);
    foreach (var tile in tiles)
        if (tile.IsLocked && newLevel >= tile.RequiredLevel)
            tile.Unlock();
    TriggerSave();
}
```

---

## Files cần sửa (tổng hợp)

| File | Thay đổi |
|------|----------|
| `SaveData.cs` | Thêm `gems`, `unlockedTileIds` |
| `GameManager.cs` | SetGems, LastSaveTime fix, OnPlayerLevelUp, capture/restore tile lock |
| `ShopPanelController.cs` | Gems refresh logic (`_refreshCostGems`) |
| `CropTileView.cs` | `_isLocked`, `_requiredLevel`, `_lockOverlay`, `Unlock()`, HandleTick guard |
| `WorldObjectPicker.cs` | Locked tile check trước ShowContextAction |
| `PopupManager.cs` | `ShowLockInfo(int)` method |
| `LevelUpToastController.cs` | File mới + `ShowMessage(string)` method |
| `LockInfoPopupController.cs` | File mới cho popup khóa tile |
| `LockInfoPopup.prefab` | Prefab mới vào `Resources/UI/Default/` |

---

## Verification

**m6a-player-feedback**:
1. AddXP đủ để lên cấp → toast "⬆ Lên cấp X!" xuất hiện và fade out sau 2s
2. Mở Shop, tap Refresh → trừ gems (không trừ gold), gems balance update
3. Save + restart → gems giữ nguyên

**m6b-world-progression**:
1. Save, đổi `lastSaveTimestamp` về 1 giờ trước, restart → animal tiến triển đúng (hungry, grown)
2. Offline > 60s → welcome toast hiện
3. Tile có `_isLocked=true`, tap → hiện popup "Cần Level X"; tile không grow
4. Lên cấp đủ → tile tự unlock, overlay biến mất
5. Save/load: tile đã unlock vẫn unlock sau restart; tile chưa unlock vẫn locked
