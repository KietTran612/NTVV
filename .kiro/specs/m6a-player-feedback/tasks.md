# Implementation Plan: m6a-player-feedback

## Overview

2 features: Level-up toast (FEAT-01) và Gems system cho Shop Refresh (FEAT-07). Tất cả logic tập trung vào subscribe event có sẵn, sửa SaveData, và wire scene.

**Design doc:** `.kiro/specs/m6a-player-feedback/design.md`
**Requirements:** `.kiro/specs/m6a-player-feedback/requirements.md`

> ⚠️ ĐỌC GLOBAL RULES trong requirements.md TRƯỜC KHI BẮT ĐẦU BẤT KỲ TASK NÀO.

---

## Tasks

- [x] 1. Tạo LevelUpToastController.cs (FEAT-01)
  - [x] 1.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [x] 1.4 · Script Agent — Tạo LevelUpToastController.cs
    - `script-read` → `Assets/_Project/Scripts/UI/HUD/HUDTopBarController.cs` — hiểu pattern, namespace
    - `script-read` → `Assets/_Project/Scripts/Gameplay/Progression/LevelSystem.cs` — confirm `OnLevelUp` signature: `Action<int>`
    - Tạo file mới `Assets/_Project/Scripts/UI/HUD/LevelUpToastController.cs`:
      ```csharp
      namespace NTVV.UI.HUD
      {
          using UnityEngine;
          using UnityEngine.UI;
          using TMPro;
          using System.Collections;
          using NTVV.Gameplay.Progression;

          public class LevelUpToastController : MonoBehaviour
          {
              [SerializeField] private TMP_Text _message_Label;
              [SerializeField] private CanvasGroup _canvasGroup;
              [SerializeField] private float _displayDuration = 2f;

              private Coroutine _fadeCoroutine;

              private void OnEnable()
              {
                  LevelSystem.OnLevelUp += OnLevelUp;
              }

              private void OnDisable()
              {
                  LevelSystem.OnLevelUp -= OnLevelUp;
              }

              private void OnLevelUp(int newLevel)
              {
                  ShowMessage($"⬆ Lên cấp {newLevel}!");
              }

              public void ShowMessage(string msg)
              {
                  if (_message_Label != null) _message_Label.text = msg;
                  gameObject.SetActive(true);
                  if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
                  _fadeCoroutine = StartCoroutine(FadeOut());
              }

              private IEnumerator FadeOut()
              {
                  if (_canvasGroup != null) _canvasGroup.alpha = 1f;
                  yield return new WaitForSeconds(_displayDuration);
                  float elapsed = 0f;
                  float fadeDuration = 0.5f;
                  while (elapsed < fadeDuration)
                  {
                      elapsed += Time.unscaledDeltaTime;
                      if (_canvasGroup != null)
                          _canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                      yield return null;
                  }
                  gameObject.SetActive(false);
              }
          }
      }
      ```
    - `assets-refresh` → chờ compile xong
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 1.6_
  - [x] 1.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors liên quan LevelUpToastController
    - Verify: có `ShowMessage(string)` public method
    - Verify: fade dùng `CanvasGroup.alpha`, không gọi `UIAnimationHelper`
    - Nếu FAIL → fix trong task 1, KHÔNG sang task 2

- [x] 2. Wire LevelUpToast trong SCN_Main (FEAT-01)
  - [x] 2.0 · Resource Check
    - Không có sprite asset — SKIP scan
    - NON-BLOCKING
  - [x] 2.4 · Scene Agent — Tạo GO LevelUpToast trong HUD Canvas
    - `scene-open` → `Assets/_Project/Scenes/SCN_Main.unity`
    - `gameobject-find` name=`[HUD_CANVAS]` — lấy ID
    - `gameobject-create` name=`LevelUpToast`, parent=`[HUD_CANVAS]`
    - `gameobject-component-add` → `CanvasGroup` vào `LevelUpToast`
    - Tạo child GO `Toast_Text` với `TMP_Text` component, text mẫu: "⬆ Lên cấp 5!", font size 24, color trắng, alignment center
    - `gameobject-component-add` → `LevelUpToastController` vào `LevelUpToast`
    - `gameobject-component-modify` → wire `_message_Label` = `Toast_Text/TMP_Text`, `_canvasGroup` = `LevelUpToast/CanvasGroup`
    - `gameobject-modify` `LevelUpToast` → `SetActive(false)`
    - `scene-save`
    - _Requirements: 1.1, 1.2, 1.3_
  - [x] 2.✓ · Quick Test
    - `gameobject-find` name=`LevelUpToast` → tồn tại trong `[HUD_CANVAS]`
    - Verify: `LevelUpToastController` attached, `_message_Label` và `_canvasGroup` không null
    - Verify: GO `SetActive=false`
    - Nếu FAIL → fix trong task 2, KHÔNG sang task 3

- [x] 3. Sửa SaveData.cs — thêm gems field (FEAT-07)
  - [x] 3.0 · Resource Check
    - Không có sprite asset — SKIP scan
    - NON-BLOCKING
  - [x] 3.4 · Script Agent — Sửa SaveData.cs
    - `script-read` → `Assets/_Project/Scripts/Core/SaveData.cs`
    - Thêm `public int gems;` vào `PlayerSaveData` (sau field `storageTier`)
    - Trong constructor `PlayerSaveData()`: thêm `gems = 25;`
    - `assets-refresh` → chờ compile xong
    - _Requirements: 2.1, 2.2_
  - [x] 3.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: `PlayerSaveData` có `gems` field
    - Nếu FAIL → fix trong task 3, KHÔNG sang task 4

- [x] 4. Sửa GameManager.cs — wire gems save/load (FEAT-07)
  - [x] 4.0 · Resource Check
    - Không có sprite asset — SKIP scan
    - NON-BLOCKING
  - [x] 4.4 · Script Agent — Sửa GameManager.cs
    - `script-read` → `Assets/_Project/Scripts/Managers/GameManager.cs`
    - Tìm `InitializeCoreSystems(PlayerSaveData data)` — vị trí gọi `EconomySystem.Instance.SetGold(data.gold)`
    - Thêm dòng ngay sau: `EconomySystem.Instance.SetGems(data.gems);`
    - Tìm `CaptureCurrentState()` — vị trí `data.gold = EconomySystem.Instance.CurrentGold`
    - Thêm dòng ngay sau: `data.gems = EconomySystem.Instance.CurrentGems;`
    - `assets-refresh` → chờ compile xong
    - _Requirements: 2.3, 2.4, 2.5_
  - [x] 4.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: `InitializeCoreSystems` có `SetGems` call
    - Verify: `CaptureCurrentState` có `data.gems` assignment
    - Nếu FAIL → fix trong task 4, KHÔNG sang task 5

- [x] 5. Sửa ShopPanelController.cs — gems refresh logic (FEAT-07)
  - [x] 5.0 · Resource Check
    - Không có sprite asset — SKIP scan
    - NON-BLOCKING
  - [x] 5.4 · Script Agent — Sửa ShopPanelController.cs
    - `script-read` → `Assets/_Project/Scripts/UI/Panels/ShopPanelController.cs`
    - Đổi field: `private int _refreshCostGold = 50;` → `private int _refreshCostGems = 50;`
    - Tìm `TryRefreshItems()` method:
      - Đổi `EconomySystem.Instance.CanAfford(_refreshCostGold)` → `EconomySystem.Instance.CanAffordGems(_refreshCostGems)`
      - Đổi `EconomySystem.Instance.AddGold(-_refreshCostGold)` → `EconomySystem.Instance.AddGems(-_refreshCostGems)`
      - Cập nhật Debug.Log: `$"<color=green>[Shop]</color> Refreshed list for {_refreshCostGems} gems"`
      - Cập nhật Debug.LogWarning: `"[Shop] Not enough gems to refresh!"`
    - `assets-refresh` → chờ compile xong
    - _Requirements: 3.1, 3.2, 3.3, 3.4_
  - [x] 5.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: không còn dùng `_refreshCostGold`
    - Verify: `TryRefreshItems()` gọi `CanAffordGems` và `AddGems`
    - Nếu FAIL → fix trong task 5, KHÔNG sang task 6

- [x] 6. Wire scene — GemsBalance_Label + Refresh_Button label (FEAT-07)
  - [x] 6.0 · Resource Check
    - Không có sprite asset — SKIP scan
    - NON-BLOCKING
  - [x] 6.4 · Scene Agent — Cập nhật ShopPopup prefab
    - `assets-prefab-open` → `Assets/_Project/Resources/UI/Default/ShopPopup.prefab`
    - `gameobject-find` name=`GemsBalance_Label` — set `SetActive(true)` nếu đang false
    - `gameobject-find` name=`Refresh_Button` — lấy ID
    - Kiểm tra Refresh_Button đã có TMP_Text child chưa:
      - Nếu chưa → tạo child GO `Refresh_Label` với `TMP_Text`, text = "Làm mới (50💎)", font size 14, color trắng
      - Nếu rồi → update text = "Làm mới (50💎)"
    - `assets-prefab-save`
    - _Requirements: 3.5, 3.6_
  - [x] 6.✓ · Quick Test
    - Verify: `GemsBalance_Label` `SetActive=true`
    - Verify: `Refresh_Button` có label rõ ràng
    - Nếu FAIL → fix trong task 6, KHÔNG sang task 7

- [x] 7. Integration Smoke Test
  - [x] 7.1 Level-up toast test
    - `editor-application-set-state` → Play mode
    - `console-get-logs` filter=Error → 0 errors khi load
    - `reflection-method-call` → `LevelSystem.Instance.AddXP(9999)` — đủ để lên cấp
    - `screenshot-game-view` → verify toast xuất hiện trong HUD
    - Chờ 3 giây → `screenshot-game-view` → verify toast đã biến mất
    - _Requirements: 1.1, 1.2, 4.1_
  - [x] 7.2 Gems save/load test
    - `reflection-method-call` → `EconomySystem.Instance.AddGems(-10)` (dùng bớt gems)
    - `reflection-method-call` → `GameManager.Instance.TriggerSave()`
    - `editor-application-set-state` → Stop + Play lại
    - `reflection-method-call` → `EconomySystem.Instance.CurrentGems` → verify giá trị không reset về 25
    - _Requirements: 2.5, 4.3_
  - [x] 7.3 Shop refresh gems test
    - Mở Shop popup qua `PopupManager.Instance.ShowScreen("Shop")`
    - `reflection-method-call` → `EconomySystem.Instance.CurrentGems` — ghi lại số
    - Tap Refresh_Button
    - `reflection-method-call` → `EconomySystem.Instance.CurrentGems` — verify giảm 50
    - `console-get-logs` filter=Warning → không có "Not enough gold"
    - _Requirements: 3.1, 3.3, 4.2_
  - [x] 7.✓ · Integration Test Report
    - `screenshot-game-view` → lưu `docs/screenshots/2026-04-20-m6a-integration.png`
    - Output:
      ```
      📋 INTEGRATION TEST REPORT: m6a-player-feedback
      ✅ PASSED: [N] checks
      ⚠️ WARNINGS: [danh sách]
      ❌ FAILED: [N] — [mô tả]
      Toast level-up: ✅
      Gems save/load: ✅
      Shop refresh dùng gems: ✅
      Console: 0 NullReferenceException ✅
      ```
    - Nếu FAIL critical → fix, KHÔNG sang task 8

- [x] 8. Cập nhật HANDOVER.md
  - Mở `docs/HANDOVER.md`
  - Thêm section phiên 20/04/2026 — m6a-player-feedback DONE:
    - LevelUpToastController tạo mới: subscribe `LevelSystem.OnLevelUp`, fade 2s
    - Gems save/load fix: `PlayerSaveData.gems`, `SetGems`/`CaptureGems` trong GameManager
    - Shop Refresh dùng gems (50 gems), không dùng gold
  - Cập nhật Kiro Specs: thêm Spec 7 `m6a-player-feedback` = DONE
  - _Requirements: tất cả_
