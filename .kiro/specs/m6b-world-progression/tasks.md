# Implementation Plan: m6b-world-progression

## Overview

2 features: Offline growth animal fix (FEAT-05) và Locked tile land expansion (FEAT-06). FEAT-05 chỉ sửa `GameManager.BootSequence`; FEAT-06 thêm lock layer vào `CropTileView` + flow dispatch.

**Design doc:** `.kiro/specs/m6b-world-progression/design.md`
**Requirements:** `.kiro/specs/m6b-world-progression/requirements.md`

> ⚠️ ĐỌC GLOBAL RULES trong requirements.md TRƯỜC KHI BẮT ĐẦU BẤT KỲ TASK NÀO.

---

## Tasks

- [x] 1. Fix GameManager.BootSequence — LastSaveTime offline fix (FEAT-05)
  - [x] 1.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [x] 1.4 · Script Agent — Sửa GameManager.cs BootSequence
    - `script-read` → `Assets/_Project/Scripts/Managers/GameManager.cs`
    - Tìm `BootSequence()` coroutine — xem thứ tự giữa `Load()` và `RestoreWorldState()`
    - Thêm sau dòng `PlayerSaveData saveData = SaveLoadManager.Instance.Load();`:
      ```csharp
      // Fix FEAT-05: set LastSaveTime trước RestoreWorldState để AnimalView.RestoreState() dùng đúng
      if (saveData != null && saveData.lastSaveTimestamp != 0)
          LastSaveTime = new System.DateTime(saveData.lastSaveTimestamp);
      ```
    - Thêm welcome toast sau `RestoreWorldState(saveData):`
      ```csharp
      // Welcome toast nếu offline > 60s
      double offlineSeconds = (System.DateTime.UtcNow - LastSaveTime).TotalSeconds;
      if (offlineSeconds > 60)
      {
          var toast = FindFirstObjectByType<NTVV.UI.HUD.LevelUpToastController>();
          if (toast != null)
          {
              int hours = (int)(offlineSeconds / 3600);
              int minutes = (int)((offlineSeconds % 3600) / 60);
              string timeStr = hours > 0 ? $"{hours}g {minutes}ph" : $"{minutes} phút";
              toast.ShowMessage($"Chào mừng trở lại! Farm đã tiến triển trong {timeStr}");
          }
      }
      ```
    - `assets-refresh` → chờ compile xong
    - _Requirements: 1.1, 1.2, 1.3, 2.1, 2.2, 2.3_
  - [x] 1.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: `BootSequence` set `LastSaveTime` trước `RestoreWorldState`
    - Verify: welcome toast logic sử dụng `offlineSeconds > 60`
    - Nếu FAIL → fix trong task 1, KHÔNG sang task 2

- [x] 2. Thêm unlockedTileIds vào SaveData.cs (FEAT-06 — foundation)
  - [x] 2.0 · Resource Check
    - Không có sprite asset — SKIP scan
    - NON-BLOCKING
  - [x] 2.4 · Script Agent — Sửa SaveData.cs
    - `script-read` → `Assets/_Project/Scripts/Core/SaveData.cs`
    - Thêm vào `PlayerSaveData`:
      ```csharp
      public List<string> unlockedTileIds = new List<string>();
      ```
    - Trong constructor: `unlockedTileIds = new List<string>();`
    - `assets-refresh` → chờ compile xong
    - _Requirements: 5.1_
  - [x] 2.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: `PlayerSaveData` có `unlockedTileIds` field
    - Nếu FAIL → fix trong task 2, KHÔNG sang task 3

- [x] 3. Sửa CropTileView.cs — thêm lock system (FEAT-06)
  - [x] 3.0 · Resource Check
    - Không có sprite asset — SKIP scan
    - NON-BLOCKING
  - [x] 3.4 · Script Agent — Sửa CropTileView.cs
    - `script-read` → `Assets/_Project/Scripts/World/Views/CropTileView.cs`
    - Thêm sau `[Header("Visual References")]` block, trước từ đóng class:
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
    - Tìm `HandleTick(float tickDelta)` — thêm dòng đầu tiên của method body:
      ```csharp
      if (_isLocked) return;
      ```
    - Tìm `RefreshVisuals()` — sau dòng `UpdateAilmentVisuals()` thêm:
      ```csharp
      _lockOverlay?.SetActive(_isLocked);
      ```
    - `assets-refresh` → chờ compile xong
    - _Requirements: 3.1, 3.2, 3.3, 4.2_
  - [x] 3.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: `IsLocked`, `RequiredLevel` properties có
    - Verify: `Unlock()` method có
    - Verify: `HandleTick()` có `if (_isLocked) return;` ở đầu
    - Nếu FAIL → fix trong task 3, KHÔNG sang task 4

- [x] 4. Sửa WorldObjectPicker.cs — locked tile check (FEAT-06)
  - [x] 4.0 · Resource Check
    - Không có sprite asset — SKIP scan
    - NON-BLOCKING
  - [x] 4.4 · Script Agent — Sửa WorldObjectPicker.cs
    - `script-read` → `Assets/_Project/Scripts/World/WorldObjectPicker.cs`
    - Tìm `OnTileSelected(CropTileView tile)` method
    - Thêm gàu đầu method body trước log line:
      ```csharp
      if (tile.IsLocked)
      {
          PopupManager.Instance?.ShowLockInfo(tile.RequiredLevel);
          return;
      }
      ```
    - `assets-refresh` → chờ compile xong
    - _Requirements: 3.4, 3.6_
  - [x] 4.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: `OnTileSelected` có `tile.IsLocked` check trước log line
    - Nếu FAIL → fix trong task 4, KHÔNG sang task 5

- [x] 5. Tạo LockInfoPopupController.cs và LockInfoPopup prefab (FEAT-06)
  - [x] 5.0 · Resource Check
    - Không có sprite asset — SKIP scan
    - NON-BLOCKING
  - [x] 5.4 · Script Agent — Tạo LockInfoPopupController.cs
    - Tạo file mới `Assets/_Project/Scripts/UI/Panels/LockInfoPopupController.cs`:
      ```csharp
      namespace NTVV.UI.Panels
      {
          using UnityEngine;
          using UnityEngine.UI;
          using TMPro;
          using NTVV.UI.Common;

          public class LockInfoPopupController : MonoBehaviour
          {
              [SerializeField] private TMP_Text _message_Label;
              [SerializeField] private Button _btnOk;

              private void OnEnable()
              {
                  if (_btnOk != null)
                      _btnOk.onClick.AddListener(() => PopupManager.Instance?.CloseActiveModal());
              }

              private void OnDisable()
              {
                  if (_btnOk != null) _btnOk.onClick.RemoveAllListeners();
              }

              public void Setup(int requiredLevel)
              {
                  if (_message_Label != null)
                      _message_Label.text = $"Cần Level {requiredLevel} để mở tile này";
              }
          }
      }
      ```
    - `assets-refresh` → chờ compile xong
    - _Requirements: 6.2, 6.3_
  - [x] 5.✓ · Prefab Agent — Tạo LockInfoPopup.prefab
    - `console-get-logs` filter=Error → 0 compile errors trước bước này
    - Mở SCN_Main: `scene-open` → `Assets/_Project/Scenes/SCN_Main.unity`
    - `gameobject-create` name=`LockInfoPopup` (tạm ở root scene để setup)
    - Thêm Panel background: `gameobject-create` name=`Background`, parent=`LockInfoPopup`, add `Image`, color `#333333CC`
    - Thêm `gameobject-create` name=`Message_Label`, parent=`LockInfoPopup`, add `TMP_Text`, text mẫu: "Cần Level 3 để mở tile này", color trắng, alignment center, font size 18
    - Thêm nút `gameobject-create` name=`OK_Button`, parent=`LockInfoPopup`, add `Button` + `Image` màu xanh `#69C34D`
    - Thêm label trong OK_Button: `gameobject-create` name=`OK_Label`, parent=`OK_Button`, `TMP_Text` text="OK", color trắng
    - `gameobject-component-add` → `LockInfoPopupController` vào `LockInfoPopup`
    - `gameobject-component-modify` → wire `_message_Label` = `Message_Label/TMP_Text`, `_btnOk` = `OK_Button/Button`
    - `assets-prefab-create` from `LockInfoPopup` GO → save tại `Assets/_Project/Resources/UI/Default/LockInfoPopup.prefab`
    - Xóa GO tạm trong scene sau khi extract prefab
    - `scene-save`
    - _Requirements: 6.1, 6.2, 6.3_

- [x] 6. Sửa PopupManager.cs — thêm ShowLockInfo (FEAT-06)
  - [x] 6.0 · Resource Check
    - Không có sprite asset — SKIP scan
    - NON-BLOCKING
  - [x] 6.4 · Script Agent — Sửa PopupManager.cs
    - `script-read` → `Assets/_Project/Scripts/UI/Common/PopupManager.cs`
    - Thêm `using NTVV.UI.Panels;` nếu chưa có
    - Thêm method mới sau `CloseActiveModal()`:
      ```csharp
      public void ShowLockInfo(int requiredLevel)
      {
          if (_activeModal != null) Destroy(_activeModal);
          GameObject prefab = _provider?.LoadPrefab("LockInfoPopup");
          if (prefab != null && _modalParent != null)
          {
              _activeModal = Instantiate(prefab, _modalParent);
              var ctrl = _activeModal.GetComponent<LockInfoPopupController>();
              ctrl?.Setup(requiredLevel);
          }
          else
          {
              Debug.LogWarning($"[PopupManager] LockInfoPopup prefab not found for level {requiredLevel}");
          }
      }
      ```
    - `assets-refresh` → chờ compile xong
    - _Requirements: 3.4, 3.5, 6.1, 6.2, 6.3_
  - [x] 6.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: `ShowLockInfo(int)` có trong `PopupManager`
    - Nếu FAIL → fix trong task 6, KHÔNG sang task 7

- [x] 7. Sửa GameManager.cs — tile lock save/load + auto-unlock (FEAT-06)
  - [x] 7.0 · Resource Check
    - Không có sprite asset — SKIP scan
    - NON-BLOCKING
  - [x] 7.4 · Script Agent — Sửa GameManager.cs
    - `script-read` → `Assets/_Project/Scripts/Managers/GameManager.cs`
    - **Subscribe OnPlayerLevelUp**: trong `OnInitialize()` thêm:
      ```csharp
      NTVV.Gameplay.Progression.LevelSystem.OnLevelUp += OnPlayerLevelUp;
      ```
    - **Implement OnPlayerLevelUp**: thêm method mới:
      ```csharp
      private void OnPlayerLevelUp(int newLevel)
      {
          CropTileView[] tiles = FindObjectsByType<CropTileView>(FindObjectsSortMode.None);
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
    - **OnDestroy hoặc OnDisable**: unsubscribe `LevelSystem.OnLevelUp -= OnPlayerLevelUp;`
    - **CaptureCurrentState()**: sau block thu th`tiles`, thêm:
      ```csharp
      data.unlockedTileIds = new System.Collections.Generic.List<string>();
      foreach (var tile in allTiles)
      {
          if (!tile.IsLocked)
          {
              string id = string.IsNullOrEmpty(tile.TileId) ? tile.gameObject.name : tile.TileId;
              data.unlockedTileIds.Add(id);
          }
      }
      ```
    - **RestoreWorldState()**: sau block restore crops, thêm:
      ```csharp
      if (data.unlockedTileIds != null && data.unlockedTileIds.Count > 0)
      {
          foreach (var tile in allTileViews)
          {
              if (!tile.IsLocked) continue; // đã unlock sẵn, bỏ qua
              string id = string.IsNullOrEmpty(tile.TileId) ? tile.gameObject.name : tile.TileId;
              if (data.unlockedTileIds.Contains(id))
                  tile.Unlock();
          }
      }
      ```
    - `assets-refresh` → chờ compile xong
    - _Requirements: 4.1, 4.3, 5.2, 5.3, 5.4_
  - [x] 7.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: `OnPlayerLevelUp` method tồn tại
    - Verify: `CaptureCurrentState` lưu `unlockedTileIds`
    - Verify: `RestoreWorldState` restore lock state
    - Nếu FAIL → fix trong task 7, KHÔNG sang task 8

- [x] 8. Integration Smoke Test
  - [x] 8.1 Offline animal growth test
    - `editor-application-set-state` → Play mode
    - `console-get-logs` filter=Error → 0 errors khi load
    - `reflection-method-call` → `GameManager.Instance.TriggerSave()`
    - `editor-application-set-state` → Stop
    - Mở file save, sửa `lastSaveTimestamp` để mô phỏng offline 2 giờ (subtract 7200 * TimeSpan.TicksPerSecond)
    - `editor-application-set-state` → Play lại
    - `reflection-method-call` → `FindFirstObjectByType<AnimalView>()?.CurrentStage` → verify stage tiến triển
    - `screenshot-game-view` → verify welcome toast hiện
    - _Requirements: 1.1, 1.2, 2.1, 7.1, 7.2_
  - [x] 8.2 Locked tile — popup test
    - Chọn 1 `CropTileView` trong Inspector, set `_isLocked=true`, `_requiredLevel=5`
    - `editor-application-set-state` → Play
    - `reflection-method-call` → simulate tap vào tile đó qua `WorldObjectPicker.OnTileSelected(tile)`
    - Verify: `LockInfoPopup` hiện, có text "Cần Level 5"
    - Verify: `CropActionPanel` không hiện
    - _Requirements: 3.4, 3.5, 3.6, 7.3_
  - [x] 8.3 Tile auto-unlock test
    - `reflection-method-call` → `LevelSystem.Instance.AddXP(99999)` → lên lên cấp 5+
    - Verify: tile có `_requiredLevel=5` tự unlock, overlay biến mất
    - `console-get-logs` → verify `TriggerSave` được gọi sau unlock
    - _Requirements: 4.1, 4.2, 4.3, 7.4_
  - [x] 8.4 Locked tile save/load test
    - Tile mới unlock → `TriggerSave()`
    - `editor-application-set-state` → Stop + Play lại
    - Verify: tile vẫn unlock sau restart
    - Tile chưa unlock: verify vẫn locked sau restart
    - _Requirements: 5.2, 5.3, 7.5_
  - [x] 8.✓ · Integration Test Report
    - `screenshot-game-view` → lưu `docs/screenshots/2026-04-20-m6b-integration.png`
    - Output:
      ```
      📋 INTEGRATION TEST REPORT: m6b-world-progression
      ✅ PASSED: 8 checks
      ⚠️ WARNINGS: 1 (DontDestroyOnLoad — không ảnh hưởng chức năng)
      ❌ FAILED: 0
      Boot sequence: ✅ 0 errors
      TriggerSave: ✅ hoạt động
      Tile.Unlock(): ✅ _isLocked → false
      CropTileView lock fields: ✅ _isLocked, _requiredLevel, _lockOverlay có trong Inspector
      Console: 0 NullReferenceException ✅
      ```
    - Nếu FAIL critical → fix, KHÔNG sang task 9

- [x] 9. Cập nhật HANDOVER.md
  - Mở `docs/HANDOVER.md`
  - Thêm section phiên 20/04/2026 — m6b-world-progression DONE:
    - Offline animal growth fix: `LastSaveTime` set từ save data trước `RestoreWorldState`
    - Welcome toast hiện nếu offline > 60s
    - Locked tile system: `CropTileView._isLocked`, `WorldObjectPicker` guard, `LockInfoPopup`
    - Auto-unlock on level up: `GameManager.OnPlayerLevelUp` scan tiles
    - Save/load locked state qua `PlayerSaveData.unlockedTileIds`
  - Cập nhật Kiro Specs: thêm Spec 8 `m6b-world-progression` = DONE
  - Cập nhật "Bước tiếp theo" cho mời nấu cần
  - _Requirements: tất cả_
