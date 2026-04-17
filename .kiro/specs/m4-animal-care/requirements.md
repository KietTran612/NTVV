# Requirements: m4-animal-care

## Overview

Fix bugs trong Animal care flow. Logic animal đã tồn tại đầy đủ trong AnimalView và AnimalPenView — spec này fix 7 bugs (2 critical, 4 high, 1 low), thêm Save/Load per-animal, auto-collect product, và loại bỏ `FindObjectsOfTypeAll` pattern.

**Prerequisite:** `m3b-storage-sell-flow` phải hoàn thành — `item_grass` và `item_worm` phải có trong StorageSystem để feed animals.

**Design doc:** `docs/superpowers/specs/2026-04-17-m4-animal-care-design.md`

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

### Req 1 — RemoveAnimal Cleanup (BUG-02)
- 1.1 `AnimalView.Sell()` gọi `_pen?.RemoveAnimal(this)` TRƯỚC `Destroy(gameObject)` trong tất cả paths
- 1.2 Natural death trong `HandleTick()` gọi `_pen?.RemoveAnimal(this)` trước `return`
- 1.3 Sau khi sell hoặc death, `AnimalPenView._currentAnimals` shrink đúng → `IsFull` trả về đúng
- 1.4 Pen không bao giờ bị stuck "full" sau khi animal đã bị destroy

### Req 2 — Natural Death Production Guard (BUG-01)
- 2.1 Khi animal chết vì quá tuổi trong `HandleTick()`, production block không chạy trong cùng tick
- 2.2 `_pen?.RemoveAnimal(this)` được gọi trong death path của HandleTick (kết hợp BUG-02)

### Req 3 — Feed Null Safety (BUG-03)
- 3.1 `Feed()` kiểm tra `StorageSystem.Instance != null` trước khi gọi `GetItemCount()`
- 3.2 Nếu StorageSystem null → `Debug.LogWarning("[AnimalView] StorageSystem not ready.")` + return, không crash

### Req 4 — Feed Warning (BUG-04)
- 4.1 Khi không đủ food, `Feed()` log `Debug.LogWarning` với số lượng cần thiết
- 4.2 HP không thay đổi khi thiếu food

### Req 5 — Sell Autosave (BUG-05)
- 5.1 `_sellButton` listener dùng `PopupManager.Instance?.CloseContextAction()` thay `gameObject.SetActive(false)`
- 5.2 `GameManager.TriggerSave()` được gọi sau khi bán animal (qua CloseContextAction hoặc trực tiếp trong Sell)

### Req 6 — Buy Autosave (BUG-06)
- 6.1 `_buyButton` listener gọi `Managers.GameManager.Instance?.TriggerSave()` sau `PurchaseAnimal()`
- 6.2 Sau khi mua animal và game crash, animal tồn tại sau load

### Req 7 — Sell QuestEvent (BUG-07)
- 7.1 `Sell()` gọi `QuestEvents.InvokeActionPerformed(QuestActionType.SellAnimal, animalId, 1)` trước Destroy
- 7.2 Quest "Sell N animals" có thể trigger khi bán animal

### Req 8 — Registry SerializeField (PERF-PEN)
- 8.1 `AnimalPenView._registry` là `[SerializeField] private GameDataRegistrySO _registry`
- 8.2 `Resources.FindObjectsOfTypeAll<GameDataRegistrySO>()` bị xóa khỏi `AnimalPenView.Awake()`
- 8.3 `AnimalPen.prefab` được assign `GameDataRegistry.asset` trực tiếp

### Req 9 — Save/Load Per-Animal
- 9.1 `AnimalSaveData` class (Serializable) tồn tại trong `PlayerSaveData.cs` với fields: animalId, stage, hp, timeInCurrentStage, timeSinceLastProduction
- 9.2 `PlayerSaveData.animals` là `List<AnimalSaveData>` với default empty list
- 9.3 `AnimalView.GetSaveData()` trả về đúng AnimalSaveData từ state hiện tại
- 9.4 `AnimalView.RestoreState(AnimalSaveData)` phục hồi đúng state + gọi `RefreshVisuals()`
- 9.5 `AnimalPenView.SpawnAndRestore(animalSO, data)` spawn animal không trừ gold
- 9.6 `GameManager.CollectSaveData()` collect animals từ tất cả AnimalView instances
- 9.7 `GameManager.RestoreWorldState()` spawn + restore animals từ save data
- 9.8 Offline time được cộng vào `timeInCurrentStage` và `timeSinceLastProduction` khi restore

### Req 10 — Auto-collect Product
- 10.1 `AnimalView.HandleTick()` tự động gọi `CollectProduct()` khi production timer đủ
- 10.2 `_collectButton` bị hide trong `CropActionPanelController.RefreshUI()` — không hiển thị với player
- 10.3 Product xuất hiện trong StorageSystem mà không cần player tap

### Req 11 — Integration
- 11.1 Mua animal → TriggerSave log → load lại → animal tồn tại ở đúng stage
- 11.2 Bán animal → `_currentAnimals` shrink → có thể mua animal mới ngay
- 11.3 Feed animal đủ food → HP tăng, food bị trừ khỏi Storage
- 11.4 Sau production timer → item xuất hiện trong Storage tự động
- 11.5 Console: 0 NullReferenceException liên quan AnimalView hoặc StorageSystem
