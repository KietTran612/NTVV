# Bug & Feature Backlog — NTVV

> Được ghi lại từ audit ngày 15/04/2026. Làm sau khi UI hoàn chỉnh.

---

## 🔴 HIGH — Fix ngay khi có thời gian

### BUG-01: `CropActionPanelController._registry` không được gán
- **File:** `Assets/_Project/Scripts/UI/Panels/CropActionPanelController.cs`
- **Vấn đề:** `TryAutoPlant()` return nếu `_registry == null` → player không thể trồng cây
- **Fix:** Auto-assign qua `Resources.FindObjectsOfTypeAll<GameDataRegistrySO>()` hoặc require inspector

### BUG-02: Animal chết không bị xóa khỏi `AnimalPenView`
- **File:** `Assets/_Project/Scripts/World/Views/AnimalView.cs`
- **Vấn đề:** Khi `lifeTimer >= lifeAfterMature`, animal đánh dấu Dead nhưng không bị Destroy → chiếm slot pen
- **Fix:**
  ```csharp
  _currentStage = GrowthStage.Dead;
  GetComponentInParent<AnimalPenView>()?.RemoveAnimal(this);
  Destroy(gameObject);
  ```

---

## 🟡 MEDIUM — Logic không chính xác

### BUG-03: `StoragePanelController.OnSellAllClick()` thiếu null check registry
- **File:** `Assets/_Project/Scripts/UI/Panels/StoragePanelController.cs`
- **Vấn đề:** Nếu registry null, category filter bị skip → bán nhầm item
- **Fix:** Add null check + fallback hoặc guard early return

### BUG-04: `plantTimestamp` bị drift do float→ticks conversion
- **File:** `Assets/_Project/Scripts/Managers/GameManager.cs`
- **Vấn đề:** Tính `plantTimestamp = DateTime.Now.Ticks - TimeSpan.FromSeconds(tile.ElapsedTime).Ticks`
  → floating-point rounding tích lũy qua nhiều save/load
- **Fix:** Lưu `ElapsedTime` trực tiếp vào `TileSaveData` thay vì tính ngược từ timestamp

---

## 🟢 LOW — Features chưa implement (làm sau)

### FEAT-01: `LevelSystem.OnLevelUp` không có subscriber
- Event fire nhưng không có UI nào lắng nghe → không có level-up feedback
- **Fix:** HUDTopBarController hoặc popup level-up subscribe và hiện animation/toast

### FEAT-02: `QuestEvents.OnQuestStateChanged` không có subscriber ngoài QuestManager
- Quest UI không tự refresh khi quest state thay đổi
- **Fix:** QuestPanelController subscribe và refresh list

### FEAT-03: Quest unlock system (`HandleUnlock()`) chỉ log
- **File:** `Assets/_Project/Scripts/Gameplay/Quests/QuestManager.cs`
- Rewards có `unlockType` nhưng không làm gì
- **Fix:** Implement thực tế (mở tab shop, unlock NPC, v.v.)

### FEAT-04: Prerequisite quests không được enforce
- `QuestDataSO.prerequisiteQuestId` có nhưng `QuestManager.AcceptQuest()` không check
- **Fix:** Add check `IsQuestCompleted(prerequisiteId)` trước khi accept

### FEAT-05: Offline growth (timestamp có nhưng chưa dùng)
- `PlayerSaveData.lastSaveTimestamp` tồn tại nhưng không dùng để tính ticks bị miss
- **Fix:** Tính `missedTicks = (DateTime.Now - lastSave).TotalSeconds` và apply khi load

### FEAT-06: Land expansion chưa implement
- `PlayerLevelData.LandExpansionCount` có theo từng level nhưng không có system unlock tile
- **Fix:** Implement tile unlocking dựa theo level milestone

### FEAT-07: Gems system là dead code
- `EconomySystem` có `OnGemsChanged` nhưng không có gì dùng gems
- **Fix:** Implement hoặc remove for v1

---

## 📋 Event hookup cần verify

| Event | Trạng thái |
|-------|-----------|
| `LevelSystem.OnLevelUp` | ⚠️ Không ai subscribe |
| `QuestEvents.OnQuestStateChanged` | ⚠️ Không ai subscribe |
| `TimeManager.OnTick` | ✅ OK |
| `EconomySystem.OnGoldChanged` | ✅ OK |
| `StorageSystem.OnStorageChanged` | ✅ OK |
| `LevelSystem.OnXPChanged` | ✅ OK |
