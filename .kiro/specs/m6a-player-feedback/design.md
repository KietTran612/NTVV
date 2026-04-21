# Design Document: m6a-player-feedback

**Date:** 2026-04-20  
**Spec:** m6a-player-feedback  
**Status:** approved  
**Full design:** `docs/superpowers/specs/2026-04-20-m6-gameplay-features-design.md`

---

## Overview

2 features: level-up toast (FEAT-01) và gems system cho Shop Refresh (FEAT-07). Không viết system mới lớn — chỉ thêm subscriber cho event có sẵn và hoàn thiện gems economy.

**Files cần sửa/tạo:**

| File | Thay đổi |
|------|----------|
| `Assets/_Project/Scripts/UI/HUD/LevelUpToastController.cs` | Tạo mới |
| `Assets/_Project/Scripts/Core/SaveData.cs` | Thêm `gems` field |
| `Assets/_Project/Scripts/Managers/GameManager.cs` | SetGems + CaptureGems |
| `Assets/_Project/Scripts/UI/Panels/ShopPanelController.cs` | Gems refresh logic |

---

## FEAT-01 — LevelUpToastController

`LevelSystem.OnLevelUp` đã fire với `int newLevel` nhưng không ai subscribe. `HUDTopBarController` chỉ lắng nghe `OnXPChanged`, không làm gì với `OnLevelUp`.

**Solution:** Tạo `LevelUpToastController.cs` trên 1 GO `LevelUpToast` trong `[HUD_CANVAS]`.

```
LevelUpToast (GameObject, SetActive=false)
  └ LevelUpToastController (script)
  └ CanvasGroup (alpha fade)
  └ TMP_Text ("⬆ Lên cấp 5!")
```

**Tại sao không dùng `UIAnimationHelper.PopIn()`:** `UIAnimationHelper` là singleton MonoBehaviour, `PopIn()` gọi `StopAllCoroutines()` trên chính nó — sẽ kill bất kỳ animation nào đang chạy. Toast tự fade bằng coroutine trên MonoBehaviour của chính nó là an toàn hơn.

**API mở rộng:** `ShowMessage(string msg)` để `GameManager` có thể gọi welcome toast (FEAT-05 trong m6b).

---

## FEAT-07 — Gems Save/Load + Shop Refresh

**Root cause:** `PlayerSaveData` không có `gems` field → mỗi lần restart `EconomySystem` reset về hardcoded `_currentGems = 25`. `ShopPanelController.TryRefreshItems()` hiện dùng `CanAfford` (gold) thay vì gems.

**SaveData fix:**
```csharp
public int gems;  // thêm vào PlayerSaveData
// constructor: gems = 25;
```

**GameManager fix:** `InitializeCoreSystems()` gọi `SetGems(data.gems)`, `CaptureCurrentState()` lưu `data.gems`.

**ShopPanelController fix:**
```csharp
// Đổi field:
[SerializeField] private int _refreshCostGems = 50;

// TryRefreshItems():
if (EconomySystem.Instance.CanAffordGems(_refreshCostGems))
{
    EconomySystem.Instance.AddGems(-_refreshCostGems);
    PopulateShop(_currentCategory);
    ...
}
```

---

## Critical Design Decisions

| Decision | Lý do |
|----------|--------|
| Toast dùng `CanvasGroup` alpha fade | Đơn giản, không conflict UIAnimationHelper |
| `ShowMessage(string)` thay vì chỉ `ShowToast(int)` | Tái sử dụng cho welcome toast offline (m6b) |
| Không queue toast | 1 toast một lần đủ cho v1 |
| `gems = 25` trong constructor | Giữ đúng với giá trị hardcoded hiện có |
| `_refreshCostGems` configurable | Dễ điều chỉnh trong Inspector |
