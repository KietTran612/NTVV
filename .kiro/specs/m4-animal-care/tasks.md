# Implementation Plan: m4-animal-care

## Overview

Fix 7 bugs trong Animal care flow + thêm Save/Load + auto-collect. Không viết script mới — chỉ sửa 5 files + wire 1 prefab.

**Design doc:** `.kiro/specs/m4-animal-care/design.md`
**Requirements:** `.kiro/specs/m4-animal-care/requirements.md`

> ⚠️ ĐỌC GLOBAL RULES trong requirements.md TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO.

---

## Tasks

- [x] 0. Fix AnimalView — Sell() (BUG-02 + BUG-07)
  - [x] 0.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [x] 0.4 · Script Agent — Sửa AnimalView.cs Sell()
    - `script-read` → `Assets/_Project/Scripts/World/Views/AnimalView.cs`
    - Tìm `Sell()` method (khoảng line 214-241)
    - **Fix BUG-07:** Thêm QuestEvent trước Destroy (trong Sell, sau economy logic):
      ```csharp
      NTVV.Gameplay.Quests.QuestEvents.InvokeActionPerformed(Data.QuestActionType.SellAnimal, _data.animalId, 1);
      ```
    - **Fix BUG-02:** Thêm sau QuestEvent, trước Destroy:
      ```csharp
      Managers.GameManager.Instance?.TriggerSave();
      _pen?.RemoveAnimal(this);
      // Destroy(gameObject); ← dòng này đã tồn tại, giữ nguyên
      ```
    - Đảm bảo thứ tự trong Sell(): ... economy/XP logic ... → QuestEvent → TriggerSave → RemoveAnimal → Destroy
    - `assets-refresh` → đợi compile xong
    - _Requirements: 1.1, 1.3, 1.4, 5.2, 7.1, 7.2_
  - [x] 0.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors liên quan AnimalView
    - Verify: `Sell()` có `_pen?.RemoveAnimal(this)` trước `Destroy(gameObject)`
    - Verify: `Sell()` có `QuestEvents.InvokeActionPerformed(... SellAnimal ...)`
    - Verify: `Sell()` có `Managers.GameManager.Instance?.TriggerSave()`
    - Nếu FAIL → fix trong task 0, KHÔNG sang task 1

- [x] 1. Fix AnimalView — HandleTick natural death (BUG-01 + BUG-02)
  - [x] 1.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [x] 1.4 · Script Agent — Sửa HandleTick natural death
    - `script-read` → `Assets/_Project/Scripts/World/Views/AnimalView.cs`
    - Tìm `HandleTick()` (khoảng line 57-92), section kiểm tra thời gian quá hạn
    - **Fix BUG-01 + BUG-02:** Đảm bảo death path có `RemoveAnimal` và `return` TRƯỚC production block:
      ```csharp
      if (_timeSinceRipe > _currentData.lifeAfterRipeInSeconds)
      {
          _currentStage = GrowthStage.Dead;
          RefreshVisuals();
          _pen?.RemoveAnimal(this); // ADD: BUG-02
          return;                   // đảm bảo production không chạy: BUG-01
      }
      ```
    - ⚠️ Kiểm tra `return` phải nằm TRƯỚC production block (lines 82-90)
    - `assets-refresh` → đợi compile xong
    - _Requirements: 1.2, 2.1, 2.2_
  - [x] 1.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: HandleTick natural death section có `_pen?.RemoveAnimal(this)` trước `return`
    - Verify: `return` nằm trước production block
    - Nếu FAIL → fix trong task 1, KHÔNG sang task 2

- [x] 2. Fix AnimalView — Feed() (BUG-03 + BUG-04)
  - [x] 2.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [x] 2.4 · Script Agent — Sửa Feed()
    - `script-read` → `Assets/_Project/Scripts/World/Views/AnimalView.cs`
    - Tìm `Feed()` method (khoảng line 175-194)
    - **Fix BUG-03:** Thêm null check cho StorageSystem ở đầu Feed():
      ```csharp
      if (StorageSystem.Instance == null)
      {
          Debug.LogWarning("[AnimalView] StorageSystem not ready.");
          return;
      }
      ```
    - **Fix BUG-04:** Tìm đoạn check food (grassCount/wormCount), thêm warning trước return:
      ```csharp
      if (grassCount < _data.feedQtyGrass || wormCount < _data.feedQtyWorm)
      {
          Debug.LogWarning($"[Animal] Not enough food to feed {_data.animalName}. Need {_data.feedQtyGrass}x grass + {_data.feedQtyWorm}x worm.");
          return;
      }
      ```
    - `assets-refresh` → đợi compile xong
    - _Requirements: 3.1, 3.2, 4.1, 4.2_
  - [x] 2.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: `Feed()` có `if (StorageSystem.Instance == null)` ở đầu method
    - Verify: `Feed()` có `Debug.LogWarning` khi grassCount hoặc wormCount không đủ
    - Nếu FAIL → fix trong task 2, KHÔNG sang task 3

- [x] 3. Fix CropActionPanelController (BUG-05 + BUG-06)
  - [x] 3.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [x] 3.4 · Script Agent — Sửa CropActionPanelController.cs
    - `script-read` → `Assets/_Project/Scripts/UI/Panels/CropActionPanelController.cs`
    - **Fix BUG-05:** Tìm `_sellButton` listener, thay `gameObject.SetActive(false)` bằng:
      ```csharp
      _sellButton?.onClick.AddListener(() =>
      {
          _targetAnimal?.Sell();
          PopupManager.Instance?.CloseContextAction(); // thay gameObject.SetActive(false)
      });
      ```
    - **Fix BUG-06:** Tìm `_buyButton` listener, thêm TriggerSave sau PurchaseAnimal:
      ```csharp
      _buyButton?.onClick.AddListener(() =>
      {
          _targetPen?.PurchaseAnimal();
          Managers.GameManager.Instance?.TriggerSave(); // ADD
          RefreshUI();
      });
      ```
    - `assets-refresh` → đợi compile xong
    - _Requirements: 5.1, 5.2, 6.1, 6.2_
  - [x] 3.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: `_sellButton` listener gọi `CloseContextAction()`, không có `SetActive(false)` cho sell flow
    - Verify: `_buyButton` listener có `GameManager.Instance?.TriggerSave()`
    - Nếu FAIL → fix trong task 3, KHÔNG sang task 4

- [x] 4. Fix AnimalPenView — PERF-PEN (_registry SerializeField)
  - [x] 4.0 · Resource Check
    - Verify `GameDataRegistry.asset` tồn tại tại `Assets/_Project/Data/GameDataRegistry.asset`
    - Nếu không tìm thấy → `assets-find` với pattern `GameDataRegistry` → report path
    - NON-BLOCKING
  - [x] 4.4 · Script Agent — Sửa AnimalPenView.cs
    - `script-read` → `Assets/_Project/Scripts/World/Views/AnimalPenView.cs`
    - **Thêm `_registry` field** (sau các fields hiện có, trước Awake):
      ```csharp
      [Header("Data")]
      [SerializeField] private GameDataRegistrySO _registry;
      ```
    - **Thêm null-check vào Awake()** (hoặc thêm vào Awake đã có):
      ```csharp
      if (_registry == null)
          Debug.LogError("[AnimalPenView] _registry is null — assign GameDataRegistry.asset in prefab Inspector.");
      ```
    - **Xóa** `Resources.FindObjectsOfTypeAll<GameDataRegistrySO>().FirstOrDefault()` trong Awake()
    - **Thay bằng** `_registry` ở những chỗ có dùng registry
    - `assets-refresh` → đợi compile xong
    - _Requirements: 8.1, 8.2_
  - [x] 4.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: không còn `FindObjectsOfTypeAll` trong AnimalPenView.cs
    - Verify: `_registry` field tồn tại với `[SerializeField]`
    - Nếu FAIL → fix trong task 4, KHÔNG sang task 5

- [x] 5. Save/Load — AnimalSaveData + GetSaveData + RestoreState + GameManager + SpawnAndRestore
  - [x] 5.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [x] 5.4 · Script Agent — Step 1: PlayerSaveData.cs
    - `script-read` → `Assets/_Project/Data/PlayerSaveData.cs`
    - **Thêm `AnimalSaveData` class** (sau class `CropTileSaveData` hoặc ở cuối file, trong namespace cũ):
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
      ```
    - **Thêm `animals` list** vào `PlayerSaveData` class:
      ```csharp
      public List<AnimalSaveData> animals = new();
      ```
    - `assets-refresh` → đợi compile xong
  - [x] 5.4b · Script Agent — Step 2: AnimalView.cs GetSaveData + RestoreState
    - `script-read` → `Assets/_Project/Scripts/World/Views/AnimalView.cs`
    - **Thêm `GetSaveData()`** method:
      ```csharp
      public AnimalSaveData GetSaveData() => new AnimalSaveData
      {
          animalId                = _data.animalId,
          stage                   = (int)_currentStage,
          hp                      = _hp,
          timeInCurrentStage      = _timeInCurrentStage,
          timeSinceLastProduction = _timeSinceLastProduction,
      };
      ```
    - **Thêm `RestoreState(AnimalSaveData data)`** method:
      ```csharp
      public void RestoreState(AnimalSaveData data)
      {
          _currentStage            = (GrowthStage)data.stage;
          _hp                      = data.hp;
          _timeInCurrentStage      = data.timeInCurrentStage;
          _timeSinceLastProduction = data.timeSinceLastProduction;

          float offline = (float)(System.DateTime.UtcNow - Managers.GameManager.LastSaveTime).TotalSeconds;
          _timeInCurrentStage      += offline;
          _timeSinceLastProduction += offline;

          RefreshVisuals();
      }
      ```
    - `assets-refresh` → đợi compile xong
  - [x] 5.4c · Script Agent — Step 3: AnimalPenView.cs SpawnAndRestore
    - `script-read` → `Assets/_Project/Scripts/World/Views/AnimalPenView.cs`
    - **Thêm `SpawnAndRestore(AnimalDataSO animalSO, AnimalSaveData data)`** method:
      ```csharp
      public void SpawnAndRestore(AnimalDataSO animalSO, AnimalSaveData data)
      {
          if (IsFull) return;
          var animalGO = Instantiate(_animalPrefab, transform);
          var animalView = animalGO.GetComponent<AnimalView>();
          animalView.Initialize(animalSO, this);
          animalView.RestoreState(data);
          _currentAnimals.Add(animalView);
      }
      ```
    - `assets-refresh` → đợi compile xong
  - [x] 5.4d · Script Agent — Step 4: GameManager.cs
    - `script-read` → `Assets/_Project/Scripts/Managers/GameManager.cs`
    - **Cập nhật `CollectSaveData()`** — thêm sau crop collect:
      ```csharp
      saveData.animals = FindObjectsOfType<AnimalView>()
          .Select(a => a.GetSaveData())
          .ToList();
      ```
    - **Cập nhật `RestoreWorldState()`** — thêm sau crop restore:
      ```csharp
      foreach (var saved in saveData.animals)
      {
          var animalSO = _registry.animals.FirstOrDefault(a => a.data.animalId == saved.animalId);
          if (animalSO == null) continue; // stale save — bỏ qua
          var pen = FindObjectsOfType<AnimalPenView>().FirstOrDefault(p => !p.IsFull);
          pen?.SpawnAndRestore(animalSO, saved);
      }
      ```
    - **Thêm `LastSaveTime` property** nếu chưa tồn tại:
      ```csharp
      public static System.DateTime LastSaveTime { get; private set; } = System.DateTime.UtcNow;
      // Cập nhật trong TriggerSave(): LastSaveTime = System.DateTime.UtcNow;
      ```
    - `assets-refresh` → đợi compile xong
    - _Requirements: 9.1, 9.2, 9.3, 9.4, 9.5, 9.6, 9.7, 9.8_
  - [x] 5.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: `PlayerSaveData` có class `AnimalSaveData` và field `animals`
    - Verify: `AnimalView` có `GetSaveData()` và `RestoreState()`
    - Verify: `AnimalPenView` có `SpawnAndRestore()`
    - Verify: `GameManager` có animals collect và restore
    - Nếu FAIL → fix trong task 5, KHÔNG sang task 6

- [x] 6. Auto-collect + Hide _collectButton
  - [x] 6.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [x] 6.4 · Script Agent — Sửa HandleTick + CropActionPanelController
    - `script-read` → `Assets/_Project/Scripts/World/Views/AnimalView.cs`
    - Tìm `HandleTick()`, phần production timer (khoảng line 82-90)
    - **Thêm auto-collect logic** vào production section:
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
    - `assets-refresh` → đợi compile xong
    - `script-read` → `Assets/_Project/Scripts/UI/Panels/CropActionPanelController.cs`
    - Tìm `RefreshUI()` method, thêm ẩn collect button:
      ```csharp
      if (_collectButton != null) _collectButton.gameObject.SetActive(false);
      ```
    - `assets-refresh` → đợi compile xong
    - _Requirements: 10.1, 10.2, 10.3_
  - [x] 6.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: `HandleTick()` có `CollectProduct()` trong production timer section
    - Verify: `RefreshUI()` có `_collectButton.gameObject.SetActive(false)`
    - Nếu FAIL → fix trong task 6, KHÔNG sang task 7

- [x] 7. Create + Wire AnimalPen Prefab (WIRE-01)
  - [] 7.0 · Resource Check _(DONE — prefab chưa tồn tại, các sub-task bên dưới tạo mới)_
    - ✅ `Animal_Placeholder.prefab` → `Assets/_Project/Prefabs/World/Animal_Placeholder.prefab`
    - ✅ `GameDataRegistry.asset` → `Assets/_Project/Data/Registry/GameDataRegistry.asset`
    - ✅ `animal_01.asset` → `Assets/_Project/Data/Animals/animal_01.asset`
    - ✅ `BarnArea` → có trong SCN_Main nhưng chỉ có Transform (chưa có AnimalPenView)
    - ❌ `AnimalPen.prefab` → **chưa tồn tại** — tạo mới trong task này
    - NON-BLOCKING
  - [x] 7.1 · Unity Agent — Tạo AnimalPen GameObject trong scene
    - `scene-open` → `Assets/_Project/Scenes/SCN_Main.unity` (nếu chưa mở)
    - `gameobject-find` → tìm GameObject tên `"BarnArea"` trong scene
    - `gameobject-create` → tạo GameObject mới:
      - Name: `"AnimalPen"`
      - Parent: `BarnArea`
      - Position: `(0, 0, 0)` local
    - `gameobject-component-add` → thêm component `AnimalPenView` vào `AnimalPen`
    - _Verify: GameObject tồn tại trong hierarchy, AnimalPenView đã gắn_
  - [x] 7.2 · Unity Agent — Đọc animal_01.asset để lấy AnimalData
    - `assets-get-data` → `Assets/_Project/Data/Animals/animal_01.asset`
    - Ghi lại các fields trong block `data`: `animalId`, `animalName`, `unlockLevel`,
      `penType`, `buyCostGold`, `feedQtyGrass`, `feedQtyWorm`, `produceItemId`, `produceTimeMin`
    - _(Bước đọc — không thay đổi scene)_
  - [x] 7.3 · Unity Agent — Configure AnimalPenView fields
    - `gameobject-component-modify` → AnimalPen / AnimalPenView:
      - `_animalPrefab`: reference đến `Assets/_Project/Prefabs/World/Animal_Placeholder.prefab`
      - `_registry`: reference đến `Assets/_Project/Data/Registry/GameDataRegistry.asset`
      - `_capacity`: `4`
      - `_currentTier`: `0`
      - `_animalType.animalId`: giá trị `animalId` từ 7.2
      - `_animalType.animalName`: giá trị `animalName` từ 7.2
      - `_animalType.unlockLevel`: giá trị `unlockLevel` từ 7.2
      - `_animalType.buyCostGold`: giá trị `buyCostGold` từ 7.2
      - `_animalType.feedQtyGrass`: giá trị `feedQtyGrass` từ 7.2
      - `_animalType.feedQtyWorm`: giá trị `feedQtyWorm` từ 7.2
      - `_animalType.produceItemId`: giá trị `produceItemId` từ 7.2
      - `_animalType.produceTimeMin`: giá trị `produceTimeMin` từ 7.2
    - `scene-save` → lưu scene
  - [x] 7.4 · Unity Agent — Extract prefab
    - `assets-prefab-create` → từ GameObject `AnimalPen` trong scene:
      - Output path: `Assets/_Project/Prefabs/World/AnimalPen.prefab`
    - Verify: file `AnimalPen.prefab` tồn tại tại path trên
    - _(Instance trong scene vẫn giữ nguyên — linked tới prefab vừa tạo)_
    - _Requirements: 8.3_
  - [x] 7.✓ · Quick Test
    - `gameobject-find` → `AnimalPen` trong scene
    - `gameobject-component-get` → AnimalPenView
    - Verify: `_registry` không null → trỏ đến `GameDataRegistry.asset`
    - Verify: `_animalPrefab` không null → trỏ đến `Animal_Placeholder.prefab`
    - Verify: `_animalType.animalId` không empty
    - `assets-find` → filter `AnimalPen t:Prefab` → Verify prefab tồn tại tại `Assets/_Project/Prefabs/World/`
    - Nếu FAIL → fix trong task 7, KHÔNG sang task 8

- [x] 8. Integration Smoke Test
  - ⚠️ **Prerequisite:** `m3b-storage-sell-flow` hoàn thành (item_grass và item_worm có thể vào Storage qua ClearWeeds/ClearPests)
  - [x] 8.1 Buy + Save/Load cycle
    - `editor-application-set-state` → Play mode
    - Verify: `console-get-logs` filter=Error → 0 errors
    - Mở pen → tap Buy → verify animal spawn trong pen
    - Verify: `console-get-logs` → GameManager save triggered (BUG-06 verify)
    - `editor-application-set-state` → Stop → Play (simulate reload)
    - Verify: animal vẫn tồn tại sau reload ở đúng stage
    - _Requirements: 9.6, 9.7, 11.1_
  - [x] 8.2 Feed cycle
    - Tap animal → Feed button visible
    - Tap Feed khi đủ food → verify HP tăng, food bị trừ khỏi Storage
    - Tap Feed khi thiếu food → verify `console-get-logs` → `"[Animal] Not enough food"`
    - _Requirements: 3.1, 4.1, 11.3_
  - [x] 8.3 Sell + IsFull cycle
    - Tap animal → Sell button visible
    - Tap Sell → verify animal biến mất
    - Verify: console có TriggerSave log
    - Verify: có thể mua animal mới ngay (pen không stuck full)
    - _Requirements: 1.3, 1.4, 5.2, 11.2_
  - [x] 8.4 Auto-collect verify
    - Đặt `_data.produceTimeMin` nhỏ tạm (0.01f) để test nhanh
    - Chờ timer đủ → verify item xuất hiện trong StorageSystem tự động
    - Verify: collect button không hiển thị trong CropActionPanel
    - Revert `produceTimeMin`
    - _Requirements: 10.1, 10.2, 10.3, 11.4_
  - [x] 8.✓ · Integration Test Report
    - `screenshot-game-view` → lưu `docs/screenshots/2026-04-17-m4-animal-care-integration.png`
    - Output:
      ```
      📋 INTEGRATION TEST REPORT: m4-animal-care
      ✅ PASSED: [N] checks
      ⚠️ WARNINGS: [danh sách]
      ❌ FAILED: [N] — [mô tả]
      Bugs fixed: BUG-01, BUG-02, BUG-03, BUG-04, BUG-05, BUG-06, BUG-07
      PERF: FindObjectsOfTypeAll removed: AnimalPenView ✅
      Wiring: AnimalPen._registry ✅
      Save/Load: AnimalSaveData ✅
      ```
    - Nếu FAIL critical → fix, KHÔNG chuyển sang task 9

- [x] 9. Update HANDOVER.md
  - Mở `docs/HANDOVER.md`
  - Cập nhật section "Phiên 17/04/2026":
    - Spec `m4-animal-care` execute xong
    - 7 bugs fixed: BUG-01 production guard, BUG-02 RemoveAnimal, BUG-03 Feed null-safe,
      BUG-04 Feed warning, BUG-05 sell autosave, BUG-06 buy autosave, BUG-07 QuestEvent
    - `FindObjectsOfTypeAll<GameDataRegistrySO>()` đã xóa khỏi AnimalPenView
    - AnimalPen.prefab: `_registry` wired
    - Save/Load per-animal hoạt động: mua/bán/reload đúng
    - Auto-collect product đang hoạt động, collect button ẩn
  - Cập nhật Kiro Specs: thêm Spec 5 `m4-animal-care` = DONE
  - Cập nhật "Bước tiếp theo" → M5 milestones
  - _Requirements: tất cả_
