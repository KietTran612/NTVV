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

---

## 🔴 HIGH — Phát hiện ngày 16/04/2026 (scn-main-world-setup review)

### BUG-08: `CropData.postRipeLifeMin = 0` → cây chết ngay khi chín
- **File:** `Assets/_Project/Scripts/Data/CropData.cs`, `Assets/_Project/Scripts/World/Views/CropTileView.cs`
- **Vấn đề:** `LifeAfterRipeInSeconds = postRipeLifeMin * 60f`. Nếu `postRipeLifeMin = 0` thì `LifeAfterRipeInSeconds = 0`. Trong `HandleTick()`:
  ```csharp
  if (_timeSinceRipe > _currentCropData.LifeAfterRipeInSeconds) _currentState = TileState.Dead;
  // → _timeSinceRipe > 0 → true ngay frame đầu tiên sau khi chín
  ```
  Cây chuyển sang `Dead` ngay lập tức, không thu hoạch được.
- **Reproduce:** Tạo CropDataSO với `postRipeLifeMin = 0`, trồng và chờ cây chín → chết ngay.
- **Fix:** Enforce `postRipeLifeMin > 0` trong Inspector (Range attribute) hoặc validate trong `GameDataRegistrySO.Initialize()`.
- **Workaround hiện tại:** Set `postRipeLifeMin = 5` trong CropDataSO (Task 7 của scn-main-world-setup).

### BUG-09: `CropTileView._tileId` private, không có setter → SaveLoad dùng fallback
- **File:** `Assets/_Project/Scripts/World/Views/CropTileView.cs`
- **Vấn đề:** `_tileId` là `[SerializeField] private string _tileId` với chỉ getter `TileId => _tileId`. Không thể set từ code bên ngoài mà không dùng reflection. `CropGridSpawner` không thể wire trực tiếp.
- **Behavior hiện tại:** `GameManager.CaptureCurrentState()` dùng fallback:
  ```csharp
  tileId = string.IsNullOrEmpty(tile.TileId) ? tile.gameObject.name : tile.TileId
  ```
  `RestoreWorldState()` match bằng `gameObject.name`. → SaveLoad hoạt động nếu GO tên đúng `"tile_r{r}_c{c}"`.
- **Fix dài hạn:** Thêm `public void SetTileId(string id) { _tileId = id; }` hoặc dùng `SerializedObject` trong Editor script.
- **Workaround hiện tại:** `CropGridSpawner` đặt tên GO = `"tile_r{row}_c{col}"`, SaveLoad dùng tên này.

---

## ⚠️ NOTES — Kiến trúc cần verify trước khi ship

### NOTE-01: FarmCameraController dùng Old Input System
- **File:** `Assets/_Project/Scripts/World/Camera/FarmCameraController.cs`
- **Vấn đề:** Dùng `Input.GetMouseButtonDown(0)` (Old Input System). `WorldObjectPicker` dùng New Input System (`PlayerInput`). Cả hai cùng fire khi tap — camera bắt đầu drag VÀ world interaction trigger đồng thời.
- **Impact:** UX lạ ở prototype (camera drag 1 frame khi tap tile). Chấp nhận được cho prototype.
- **Fix dài hạn:** Migrate `FarmCameraController` sang New Input System, hoặc add `isInteracting` flag để camera không drag khi WorldObjectPicker đang handle.

### NOTE-02: PlayerInput phải cùng GO với WorldObjectPicker
- **File:** `Assets/_Project/Scripts/World/WorldObjectPicker.cs`
- **Vấn đề:** `OnTap(InputValue value)` nhận message từ `PlayerInput` qua "Send Messages". Send Messages chỉ dispatch đến cùng GameObject — không sang sibling hoặc object khác trong hierarchy.
- **Yêu cầu bắt buộc:** `PlayerInput` và `WorldObjectPicker` PHẢI trên cùng 1 GameObject.
- **Consequence nếu sai:** Tap không có phản ứng, không có error log → silent fail, khó debug.

### NOTE-03: `EconomySystem.CanAfford()` cần gold > 0 để plant
- **File:** `Assets/_Project/Scripts/World/Views/CropTileView.cs` line ~61
- **Vấn đề:** `Plant()` chỉ chạy nếu `EconomySystem.Instance.CanAfford(data.seedCostGold)`. Player bắt đầu với 0 gold → không thể plant ngay. `CropActionPanelController.TryAutoPlant()` sẽ fallback mở Shop.
- **Cần verify:** Integration test plant flow cần set gold ban đầu > 0 (hoặc set `seedCostGold = 0` cho test).
- **Không phải bug** — đây là intended design, nhưng cần nhớ khi test.


---

## 🟡 MEDIUM — Phát hiện ngày 16/04/2026 (scn-main-ui-rebuild Task 6)

### NOTE-04: `ShopPopup.Refresh_Button` được thêm ngoài spec
- **File:** Scene `SCN_Main`, GameObject `[POPUP_CANVAS]/ModalParent/ShopPopup/Footer/Refresh_Button`
- **Vấn đề:** `ShopPanelController` có field `_btnRefresh` nhưng task spec 6.3 không yêu cầu tạo button này. Button được thêm thủ công để tránh null reference.
- **Cần verify:**
  - UI/UX: Refresh_Button có nên hiển thị trong Footer không? Hay ẩn đi cho v1?
  - Cost display: Button cần label hiển thị "Làm mới (50g)" để player biết giá
  - Hiện tại button chỉ có Image màu xanh #69C34D, không có label text
- **Fix đề xuất:** Thêm TMP label "Làm mới" + cost label vào Refresh_Button, hoặc SetActive=false nếu feature chưa cần cho v1.

### NOTE-05: `ShopPopup.GemsBalance_Label` được thêm ngoài spec — Gems system là dead code
- **File:** Scene `SCN_Main`, GameObject `[POPUP_CANVAS]/ModalParent/ShopPopup/Footer/GemsBalance_Label`
- **Vấn đề:** `ShopPanelController` có field `_gemsBalanceLabel` nhưng task spec 6.3 không yêu cầu. Label được thêm để tránh null reference. Liên quan đến **FEAT-07** (Gems system là dead code).
- **Cần verify:**
  - Gems có được dùng trong v1 không? Nếu không → có thể SetActive=false hoặc xóa label
  - Hiện tại label hiển thị "0" màu tím (#9966FF) trong Footer, không có icon gem đi kèm
- **Fix đề xuất:** Nếu Gems không dùng cho v1, SetActive=false trên GemsBalance_Label. Nếu dùng, thêm GemIcon đi kèm tương tự GoldChip.

---

## 🟡 MEDIUM — Phát hiện ngày 20/04/2026 (m4-animal-care Task 7 — WIRE-01 blocked)

### NOTE-06: `AnimalPen.prefab` chưa tồn tại — WIRE-01 đã được unblock ✅
- **Spec:** `m4-animal-care`, Task 7 (WIRE-01)
- **Vấn đề gốc:** Task 7 yêu cầu assign `_registry` vào `AnimalPen.prefab` nhưng prefab chưa tồn tại.
- **Kiến trúc đã quyết định (20/04/2026):**
  - 1 `AnimalPen` GameObject trong scene, đặt con của `BarnArea`
  - `AnimalPenView._animalType` fill inline từ data của `animal_01.asset`
  - Extract thành `Assets/_Project/Prefabs/World/AnimalPen.prefab`
- **Assets đã xác nhận có sẵn:**
  - `Animal_Placeholder.prefab` → `Assets/_Project/Prefabs/World/Animal_Placeholder.prefab`
  - `GameDataRegistry.asset` → `Assets/_Project/Data/Registry/GameDataRegistry.asset`
  - `animal_01.asset` → `Assets/_Project/Data/Animals/animal_01.asset`
  - `BarnArea` → có trong SCN_Main (chỉ có Transform, sẵn sàng nhận child)
- **Trạng thái:** Task 7 đã cập nhật với 4 sub-task rõ ràng (7.1–7.4) — **chờ thực thi**.
- **Priority:** Cần giải quyết trước khi test Buy/Sell animal flow (Task 8).
