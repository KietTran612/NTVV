# Bug & Feature Backlog — NTVV

> Được ghi lại từ audit ngày 15/04/2026. Làm sau khi UI hoàn chỉnh.

---

## 🔴 HIGH — Fix ngay khi có thời gian

### BUG-10: Main Camera không nhìn vào world — game view trống khi play ⚠️ QUAN TRỌNG
- **File:** Scene `SCN_Main`, GameObject `[WORLD_ROOT]/Main Camera`
- **Vấn đề:** Khi vào play mode, game view render màu xanh đơn sắc (#2E4A6B — background color của camera). Scene view cho thấy HUD (TopHUDBar + BottomNav) render đúng, nhưng toàn bộ world (farm tiles, CropArea, AnimalPen, BarnArea) không hiển thị trong game view.
- **Nguyên nhân xác định:** Main Camera đang ở vị trí/rotation không hướng vào `[WORLD_ROOT]`. Camera nhìn vào vùng không có renderer nào → chỉ thấy background color.
- **Reproduce:** Vào play mode → `screenshot_game` → màn hình xanh đơn sắc.
- **Impact:**
  - Player không thấy farm khi chơi
  - Không thể test bất kỳ world interaction nào (tap tile, animal, shop trigger)
  - Mọi integration test visual đều fail
  - **Blocker cho demo và QA**
- **Fix đề xuất:**
  1. Kiểm tra `Main Camera` transform: `position` phải nhìn vào `CropArea` (khoảng `(0, 5, -10)` với orthographic hoặc perspective phù hợp)
  2. Kiểm tra `Orthographic Size` — nếu quá nhỏ sẽ không thấy tiles
  3. Kiểm tra `Culling Mask` — phải include layer của `[WORLD_ROOT]` objects (Default hoặc layer tùy chỉnh)
  4. Kiểm tra `Clear Flags` — nên là `Skybox` hoặc `Solid Color` với màu phù hợp
- **Priority:** 🔴 HIGH — Blocker cho mọi visual test và demo

### BUG-01: `CropActionPanelController._registry` không được gán
- ✅ Fixed in m3a-crop-care-harvest (WIRE-01)
- **File:** `Assets/_Project/Scripts/UI/Panels/CropActionPanelController.cs`
- **Vấn đề:** `TryAutoPlant()` return nếu `_registry == null` → player không thể trồng cây
- **Fix:** Auto-assign qua `Resources.FindObjectsOfTypeAll<GameDataRegistrySO>()` hoặc require inspector

### BUG-02: Animal chết không bị xóa khỏi `AnimalPenView`
- ✅ Fixed in m4-animal-care (BUG-02)
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
- ✅ Fixed in m3b-storage-sell-flow (BUG-B1)
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
- ✅ Fixed in m6a-player-feedback + m6b-world-progression
- Event fire nhưng không có UI nào lắng nghe → không có level-up feedback
- **Fix:** HUDTopBarController hoặc popup level-up subscribe và hiện animation/toast

### FEAT-02: `QuestEvents.OnQuestStateChanged` không có subscriber ngoài QuestManager
- ✅ Fixed in m5-quest-flow (BUG-Q1)
- Quest UI không tự refresh khi quest state thay đổi
- **Fix:** QuestPanelController subscribe và refresh list

### FEAT-03: Quest unlock system (`HandleUnlock()`) chỉ log
- ✅ Fixed in m5-quest-flow (BUG-Q3)
- **File:** `Assets/_Project/Scripts/Gameplay/Quests/QuestManager.cs`
- Rewards có `unlockType` nhưng không làm gì
- **Fix:** Implement thực tế (mở tab shop, unlock NPC, v.v.)

### FEAT-04: Prerequisite quests không được enforce
- ✅ Fixed in m5-quest-flow (BUG-Q2)
- `QuestDataSO.prerequisiteQuestId` có nhưng `QuestManager.AcceptQuest()` không check
- **Fix:** Add check `IsQuestCompleted(prerequisiteId)` trước khi accept

### FEAT-05: Offline growth (timestamp có nhưng chưa dùng)
- `PlayerSaveData.lastSaveTimestamp` tồn tại nhưng không dùng để tính ticks bị miss
- **Fix:** Tính `missedTicks = (DateTime.Now - lastSave).TotalSeconds` và apply khi load

### FEAT-06: Land expansion chưa implement
- `PlayerLevelData.LandExpansionCount` có theo từng level nhưng không có system unlock tile
- **Fix:** Implement tile unlocking dựa theo level milestone

### FEAT-07: Gems system là dead code
- ✅ Fixed in m6a-player-feedback
- `EconomySystem` có `OnGemsChanged` nhưng không có gì dùng gems
- **Fix:** Implement hoặc remove for v1

---

## 📋 Event hookup cần verify

| Event | Trạng thái |
|-------|-----------|
| `LevelSystem.OnLevelUp` | ✅ 2 subscribers: LevelUpToastController (m6a), GameManager.OnPlayerLevelUp (m6b) |
| `QuestEvents.OnQuestStateChanged` | ✅ QuestPanelController subscribe (m5-quest-flow) |
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
- ✅ Fixed in m4-animal-care Task 7
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

---

## 🟡 MEDIUM — Phát hiện ngày 21/04/2026 (m6a-player-feedback execution)

### NOTE-07: `game_object_create` không set parent đúng — cần dùng `transform_set_parent` sau khi tạo
- **Phát hiện:** Task 2 (Wire LevelUpToast) — khi gọi `game_object_create` với `parentPath`, GO bị tạo ở root scene thay vì trong parent chỉ định.
- **Reproduce:** `game_object_create(name="LevelUpToast", parentPath="[HUD_CANVAS]")` → GO xuất hiện ở root, không phải con của `[HUD_CANVAS]`.
- **Workaround đã áp dụng:** Tạo GO trước, sau đó gọi `transform_set_parent(name="LevelUpToast", parent="[HUD_CANVAS]")` → path trở thành `[HUD_CANVAS]/LevelUpToast` đúng.
- **Impact:** Nếu không phát hiện, `LevelUpToastController.OnEnable()` vẫn subscribe event nhưng GO không nằm trong Canvas → toast không render trên HUD.
- **Fix cho agent:** Luôn dùng `transform_set_parent` sau `game_object_create` để đảm bảo hierarchy đúng. Không tin vào `parentPath` parameter.

### NOTE-08: `LevelUpToast` hierarchy bị sai trong Task 2 — đã fix trong Task 7
- **Spec:** `m6a-player-feedback`, Task 2 → phát hiện và fix trong Task 7
- **Vấn đề:** `LevelUpToast` và `Toast_Text` bị đặt ở root scene thay vì `[HUD_CANVAS]/LevelUpToast/Toast_Text`. Nguyên nhân: xem NOTE-07.
- **Triệu chứng phát hiện:** `scene_hierarchy` trong Task 7 (play mode) cho thấy `LevelUpToast` và `Toast_Text` là root objects, không phải con của `[HUD_CANVAS]`.
- **Fix đã thực hiện (Task 7):**
  1. Destroy `LevelUpToast` và `Toast_Text` ở root
  2. Tạo lại `LevelUpToast` → `transform_set_parent` → `[HUD_CANVAS]`
  3. Tạo lại `Toast_Text` → `transform_set_parent` → `[HUD_CANVAS]/LevelUpToast`
  4. Re-add components + re-wire refs + SetActive=false + scene-save
- **Trạng thái:** ✅ Đã fix. `[HUD_CANVAS]/LevelUpToast/Toast_Text` đúng hierarchy.

### NOTE-09: `component_invoke` chỉ hỗ trợ public parameterless methods
- **Phát hiện:** Task 7 — không thể gọi `LevelSystem.AddXP(9999)` qua `component_invoke`.
- **Vấn đề:** `component_invoke` tool chỉ invoke được method không có parameter. `AddXP(int amount)` có param → error "Public parameterless method 'AddXP' not found".
- **Workaround:** Dùng `component_set` để set `_currentXP` trực tiếp, sau đó gọi `RefreshUI()` (parameterless). Tuy nhiên `RefreshUI()` chỉ fire `OnXPChanged`, không fire `OnLevelUp` → không test được toast trigger qua MCP.
- **Impact:** Level-up toast visual test phải làm thủ công trong Editor. Logic subscribe đúng nhưng không verify được qua automation.
- **Fix dài hạn:** Thêm debug method parameterless vào `LevelSystem` (ví dụ `DebugForceLevel()`) để test qua MCP, hoặc dùng `editor_invoke_method` với static wrapper.

### NOTE-10: `ShopPopup.prefab` — `Refresh_Label` text không được set qua MCP trong prefab stage
- **Phát hiện:** Task 6 — sau khi `component_set` text = "Làm mới (50💎)" trong prefab stage và `prefab_save`, verify lại thấy `m_text: ` rỗng trong file.
- **Nguyên nhân:** MCP `component_set` trong prefab stage không persist text value vào file `.prefab` đúng cách — có thể do prefab stage chưa fully loaded khi set.
- **Workaround đã áp dụng:** Sửa trực tiếp `m_text` trong file `.prefab` bằng `strReplace` với Unicode escape (`\u00e0m m\u1edbi (50\U0001F48E)`), sau đó `editor_refresh_assets`.
- **Fix cho agent:** Khi set TMP text trong prefab stage qua MCP không work, fallback sang edit file `.prefab` trực tiếp với Unicode-escaped string.

### NOTE-11: Game view render xanh đơn sắc — camera background issue (pre-existing)
- **Phát hiện:** Task 7 integration test — `screenshot_game` trả về màn hình xanh đơn sắc (#2E4A6B).
- **Nguyên nhân:** Main Camera background color là solid blue, không có background sprite/skybox. Camera đang nhìn vào vùng không có renderer nào.
- **Impact:** Không thể verify visual qua `screenshot_game` trong automation. Logic test vẫn pass qua `component_get`.
- **Trạng thái:** Pre-existing issue (đã ghi trong m4-animal-care). Non-blocking cho logic tests.
- **Fix đề xuất:** Điều chỉnh Main Camera orthographic size + position để nhìn thấy scene, hoặc thêm background sprite vào world.

---

## 🟡 MEDIUM — Phát hiện ngày 21/04/2026 (m6b-world-progression review)

### NOTE-12: `DontDestroyOnLoad` warning trên `TimeManager`
- **File:** `Assets/_Project/Scripts/Core/TimeManager.cs` line ~30
- **Vấn đề:** `DontDestroyOnLoad` được gọi trên một GameObject không phải root (có parent trong hierarchy). Unity log warning: "DontDestroyOnLoad only works for root GameObjects or components on root GameObjects."
- **Impact:** Warning không ảnh hưởng chức năng hiện tại (TimeManager vẫn hoạt động), nhưng có thể gây vấn đề khi load scene mới.
- **Fix đề xuất:** Trong `TimeManager.Awake()`, gọi `transform.SetParent(null)` trước `DontDestroyOnLoad(gameObject)`, tương tự pattern đã có trong `Singleton<T>.Awake()`.

### NOTE-13: `CropTileView._lockOverlay` chưa được assign trong scene tiles
- **File:** `Assets/_Project/Scripts/World/Views/CropTileView.cs`
- **Vấn đề:** Lock system (FEAT-06) đã implement `_lockOverlay` field nhưng các tile hiện có trong scene (`tile_r0_c0` đến `tile_r1_c2`) chưa có `_lockOverlay` GameObject được assign. Khi `Unlock()` được gọi, `_lockOverlay?.SetActive(false)` skip (null-safe) — không crash nhưng không có visual feedback.
- **Impact:** Tile locked không hiển thị overlay sprite → player không biết tile đang locked.
- **Fix đề xuất:**
  1. Tạo `LockOverlay` child GameObject cho mỗi tile (SpriteRenderer với lock icon sprite)
  2. Assign vào `_lockOverlay` field trong Inspector
  3. Cần asset: `lock_overlay.png` hoặc dùng placeholder màu đỏ bán trong suốt
- **Priority:** Cần trước khi test locked tile flow với player thực.

### NOTE-14: `LevelSystem.OnLevelUp` event table cần cập nhật
- **File:** `docs/backlog/bug-backlog.md` — Event hookup table (FEAT-01)
- **Vấn đề:** FEAT-01 ghi "Không ai subscribe" nhưng sau m6b đã có 2 subscribers:
  1. `LevelUpToastController` (từ m6a)
  2. `GameManager.OnPlayerLevelUp` (từ m6b — auto-unlock tiles)
- **Trạng thái:** FEAT-01 có thể đóng lại — event đã có subscriber.
- **Fix:** Cập nhật event table, đánh dấu FEAT-01 = DONE.

### NOTE-15: `GameManager.OnDestroy()` có thể conflict với Singleton pattern
- **File:** `Assets/_Project/Scripts/Managers/GameManager.cs`
- **Vấn đề:** `Singleton<T>` base class không có virtual `OnDestroy()`. `GameManager` thêm `private void OnDestroy()` để unsubscribe `LevelSystem.OnLevelUp`. Nếu `Singleton<T>` sau này thêm `OnDestroy` logic (ví dụ clear `_instance`), sẽ không được gọi vì `private` không override.
- **Impact:** Hiện tại không có vấn đề. Risk khi refactor Singleton.
- **Fix đề xuất:** Thêm `protected virtual void OnDestroy() {}` vào `Singleton<T>` base class, rồi `GameManager` dùng `protected override void OnDestroy()`.

### NOTE-16: Game view trống khi play — Main Camera không nhìn vào world
- **File:** Scene `SCN_Main`, GameObject `Main Camera`
- **Vấn đề:** Khi play mode, game view render màu xanh đơn sắc (background color). Scene view cho thấy HUD render đúng nhưng world (farm tiles, animals) không hiển thị trong game view.
- **Nguyên nhân có thể:** Camera position/rotation không hướng vào `[WORLD_ROOT]`, hoặc camera culling mask không include world layer.
- **Impact:** Không ảnh hưởng logic gameplay, nhưng cần fix trước khi demo/test thực tế.
- **Fix đề xuất:** Kiểm tra `Main Camera` transform position và rotation, đảm bảo nhìn vào `CropArea`. Kiểm tra `Culling Mask` include layer của world objects.
- **Priority:** Medium — cần fix trước integration test thực tế.

---

## ✅ RESOLVED — m6b-world-progression (21/04/2026)

### FEAT-05: Offline growth fix — DONE ✅
- `GameManager.BootSequence()` set `LastSaveTime` từ `saveData.lastSaveTimestamp` trước `RestoreWorldState()`
- Welcome toast hiện nếu offline > 60s (dùng lại `LevelUpToastController.ShowMessage()`)

### FEAT-06: Land expansion / Locked tile system — DONE ✅
- `CropTileView`: thêm `_isLocked`, `_requiredLevel`, `_lockOverlay`, `Unlock()`, guard trong `HandleTick()`
- `WorldObjectPicker`: guard `tile.IsLocked` → `PopupManager.ShowLockInfo()`
- `LockInfoPopupController.cs` + `LockInfoPopup.prefab` tạo mới
- `PopupManager.ShowLockInfo(int)` thêm mới
- `PlayerSaveData.unlockedTileIds` thêm mới
- `GameManager`: `OnPlayerLevelUp` auto-unlock, `CaptureCurrentState` lưu, `RestoreWorldState` restore

## ✅ RESOLVED — m8-scene-polish (2026-04-22)

### BUG-08: postRipeLifeMin = 0 cây chết ngay — DONE ✅
- `CropData.cs`: `[Min(0.1f)]` cho `growTimeMin`, `perfectWindowMin`, `postRipeLifeMin`

### BUG-10: Main Camera không nhìn vào world — DONE ✅
- Main Camera transform reset: `position = (1.2, 0.35, -10)`, rotation/scale identity
- Game view render đúng world sau fix

### NOTE-13 (renumbered từ NOTE-08 m6b): `_lockOverlay` chưa assign — DONE ✅
- 6 tiles có child `LockOverlay` + SpriteRenderer với `World_Overlay_Tile_Lock_On` sprite
- `CropTileView._lockOverlay` field wired trên cả 6 instance

### NOTE-14 (renumbered từ NOTE-09 m6b): Event table outdated — DONE ✅
- Event hookup table updated: `QuestEvents.OnQuestStateChanged` ✅ QuestPanelController subscribe
