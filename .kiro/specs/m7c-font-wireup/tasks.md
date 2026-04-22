# Implementation Plan: m7c-font-wireup

## Overview

Đổi 13 TextMeshPro từ LiberationSans SDF → Dosis-Bold SDF bằng direct YAML edit. Scope nhỏ, ~8 file edit + verify.

**Design doc:** `.kiro/specs/m7c-font-wireup/design.md`
**Requirements:** `.kiro/specs/m7c-font-wireup/requirements.md`

> ⚠️ ĐỌC GLOBAL RULES trong requirements.md TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO.

---

## Constants

| Tên | Value |
|-----|-------|
| OLD_FONT_GUID (LiberationSans SDF) | `8f586378b4e144a9851e7b34d9b748ee` |
| OLD_MAT_FILEID (LiberationSans material subasset) | `2180264` |
| NEW_FONT_GUID (Dosis-Bold SDF) | `452c63e094a208745841fe3a5aeb7642` |
| NEW_MAT_FILEID (Dosis-Bold material subasset) | `-3612796072522039072` |

## Replace Pattern

**Old block (2 dòng liền kề):**
```
  m_fontAsset: {fileID: 11400000, guid: 8f586378b4e144a9851e7b34d9b748ee, type: 2}
  m_sharedMaterial: {fileID: 2180264, guid: 8f586378b4e144a9851e7b34d9b748ee, type: 2}
```

**New block:**
```
  m_fontAsset: {fileID: 11400000, guid: 452c63e094a208745841fe3a5aeb7642, type: 2}
  m_sharedMaterial: {fileID: -3612796072522039072, guid: 452c63e094a208745841fe3a5aeb7642, type: 2}
```

---

## Tasks

- [x] 0. Prerequisite — verify 13 locations (baseline)
  - [x] 0.1 · Baseline grep
    - `Bash grep -rn "guid: 8f586378b4e144a9851e7b34d9b748ee" Assets/_Project/ --include="*.prefab" --include="*.unity" | wc -l` → expected **26** (13 m_fontAsset + 13 m_sharedMaterial)
    - `Bash grep -rn "fileID: 2180264.*guid: 8f586378" Assets/_Project/ | wc -l` → expected **13** (material references)
    - `Bash grep -rn "m_fontAsset: .*guid: 452c63e0" Assets/_Project/ | wc -l` → expected **45** (Dosis-Bold đã đúng)
    - BLOCKING nếu số liệu khác → điều tra trước khi fix (có thể project state đã thay đổi)
    - _Requirements: 5.1, 5.3_

- [x] 1. Fix SCN_Main.unity (1 TMP tại line ~4230)
  - [x] 1.0 · Resource Check
    - Verify `Assets/_Project/Scenes/SCN_Main.unity` tồn tại
    - `Bash grep -c "guid: 8f586378b4e144a9851e7b34d9b748ee" Assets/_Project/Scenes/SCN_Main.unity` → **2** (m_fontAsset + m_sharedMaterial của 1 TMP)
    - BLOCKING nếu ≠ 2
  - [x] 1.4 · Edit Agent — SCN_Main
    - Dùng `Edit` tool với `replace_all: false`:
      - file_path: `Assets/_Project/Scenes/SCN_Main.unity`
      - old_string (3 dòng để unique — font asset + material):
        ```
          m_fontAsset: {fileID: 11400000, guid: 8f586378b4e144a9851e7b34d9b748ee, type: 2}
          m_sharedMaterial: {fileID: 2180264, guid: 8f586378b4e144a9851e7b34d9b748ee, type: 2}
        ```
      - new_string:
        ```
          m_fontAsset: {fileID: 11400000, guid: 452c63e094a208745841fe3a5aeb7642, type: 2}
          m_sharedMaterial: {fileID: -3612796072522039072, guid: 452c63e094a208745841fe3a5aeb7642, type: 2}
        ```
    - _Requirements: 2.1_
  - [x] 1.✓ · Quick Test
    - `Bash grep -c "guid: 8f586378b4e144a9851e7b34d9b748ee" Assets/_Project/Scenes/SCN_Main.unity` → **0**
    - `Bash grep -c "fileID: -3612796072522039072" Assets/_Project/Scenes/SCN_Main.unity` → **1** (material mới)
    - Nếu FAIL → fix task 1, KHÔNG sang task 2

- [x] 2. Fix 4 prefab UI/Components (7 TMP tổng)
  - [x] 2.0 · Resource Check
    - Verify 4 prefab tồn tại:
      - `Prefabs/UI/Components/InventorySlot.prefab` (1 TMP)
      - `Prefabs/UI/Components/QuestListItem.prefab` (4 TMP)
      - `Prefabs/UI/Components/UI_Nav_Button.prefab` (1 TMP)
      - `Prefabs/UI/Components/UI_XP_ProgressBar.prefab` (1 TMP)
    - BLOCKING nếu thiếu
  - [x] 2.4 · Edit Agent — Batch 4 prefab
    - **InventorySlot.prefab** — `Edit replace_all: false` (1 block)
      - old_string = 2 dòng LiberationSans
      - new_string = 2 dòng Dosis-Bold
    - **QuestListItem.prefab** — `Edit replace_all: true` (4 blocks cùng pattern)
      - Cùng pattern replace, nhưng `replace_all: true` để đổi cả 4 TMP
    - **UI_Nav_Button.prefab** — `Edit replace_all: false` (1 block)
    - **UI_XP_ProgressBar.prefab** — `Edit replace_all: false` (1 block)
    - _Requirements: 3.1, 3.2, 3.3, 3.4_
  - [x] 2.✓ · Quick Test
    - `Bash grep -c "guid: 8f586378b4e144a9851e7b34d9b748ee" Assets/_Project/Prefabs/UI/Components/*.prefab` → **0** trong mỗi file
    - `Bash grep -rc "fileID: -3612796072522039072" Assets/_Project/Prefabs/UI/Components/` → tổng **7** (1+4+1+1)
    - Nếu FAIL → fix task 2, KHÔNG sang task 3

- [ ] 3. Fix 3 prefab Resources/UI/Default (5 TMP tổng)
  - [x] 3.0 · Resource Check
    - Verify 3 prefab tồn tại:
      - `Resources/UI/Default/LockInfoPopup.prefab` (2 TMP)
      - `Resources/UI/Default/QuestPopup.prefab` (1 TMP)
      - `Resources/UI/Default/ShopPopup.prefab` (2 TMP)
    - BLOCKING nếu thiếu
  - [x] 3.4 · Edit Agent — Batch 3 prefab
    - **LockInfoPopup.prefab** — `Edit replace_all: true` (2 blocks)
    - **QuestPopup.prefab** — `Edit replace_all: false` (1 block)
    - **ShopPopup.prefab** — `Edit replace_all: true` (2 blocks; chú ý: file còn có 5 TMP khác dùng Dosis-Bold đúng rồi → pattern LiberationSans không match)
    - _Requirements: 4.1, 4.2, 4.3_
  - [x] 3.✓ · Quick Test
    - `Bash grep -c "guid: 8f586378b4e144a9851e7b34d9b748ee" Assets/_Project/Resources/UI/Default/*.prefab` → 0 mỗi file
    - `Bash grep -c "m_fontAsset: .*guid: 452c63e0" Assets/_Project/Resources/UI/Default/ShopPopup.prefab` → **7** (5 cũ đúng + 2 mới fix)
    - Nếu FAIL → fix task 3, KHÔNG sang task 4

- [x] 4. Post-verify — grep toàn project
  - [x] 4.0 · Global sanity check
    - `Bash grep -rn "guid: 8f586378b4e144a9851e7b34d9b748ee" Assets/_Project/ --include="*.prefab" --include="*.unity"` → **0 match** (không còn LiberationSans)
    - `Bash grep -rn "fileID: 2180264" Assets/_Project/ --include="*.prefab" --include="*.unity"` → **0 match** (không còn material LiberationSans)
    - `Bash grep -rn "m_fontAsset: .*guid: 452c63e0" Assets/_Project/ --include="*.prefab" --include="*.unity" | wc -l` → **58** (45 + 13)
    - `Bash grep -rn "m_sharedMaterial: .*fileID: -3612796072522039072" Assets/_Project/ | wc -l` → **58** (material tương ứng)
    - _Requirements: 5.1, 5.2, 5.3_
  - [x] 4.1 · Unity console check
    - `assets-refresh` (nếu Unity đang mở) — chờ re-import
    - `console-get-logs` filter=Warning → 0 "Missing font", "Could not find material"
    - Nếu FAIL → điều tra file nào vỡ, rollback bằng `git checkout`
    - _Requirements: 5.4_

- [x] 5. Integration smoke test — Play mode (visual confirm)
  - [x] 5.0 · Prerequisite
    - Task 4 PASS (0 LiberationSans, 0 missing material warning)
    - BLOCKING nếu fail
  - [x] 5.1 Boot + HUD visual
    - `editor-application-set-state` → Play mode
    - `screenshot-game-view` → verify HUD (XP bar, resource chips, Nav buttons) chữ là Dosis-Bold
    - _Requirements: 5.5_
  - [x] 5.2 Shop + Storage visual
    - Mở Shop popup (`PopupManager.Instance.ShowScreen("Shop")` qua reflection nếu cần)
    - `screenshot-game-view` → verify Shop text, entry rows là Dosis-Bold
    - Mở Storage tương tự → verify
    - _Requirements: 5.5_
  - [x] 5.3 Quest + Lock visual
    - Mở QuestPopup → verify quest title/desc Dosis-Bold
    - Tap locked tile (nếu có trong scene) → LockInfoPopup hiện với Dosis-Bold
    - _Requirements: 5.5_
  - [x] 5.✓ · Integration Test Report
    - `editor-application-set-state` → Stop
    - `screenshot-game-view` lưu `docs/screenshots/2026-04-21-m7c-font-wireup-integration.png`
    - Output:
      ```
      📋 INTEGRATION TEST REPORT: m7c-font-wireup
      ✅ PASSED: [N] checks
      ⚠️ WARNINGS: [danh sách]
      ❌ FAILED: [N] — [mô tả]
      LiberationSans removed: ✅ 0 match
      Dosis-Bold count: ✅ 58
      HUD/Shop/Storage/Quest/Lock visual: ✅
      Missing font warnings: 0 ✅
      ```
    - Nếu FAIL critical → fix, KHÔNG sang task 6

- [x] 6. Cập nhật HANDOVER.md + commit
  - Mở `docs/HANDOVER.md`
  - Thêm section phiên 21/04/2026 — m7c-font-wireup DONE:
    - Fix 13 TextMeshPro component đang dùng LiberationSans SDF (TMP default) → Dosis-Bold SDF
    - Đồng bộ m_fontAsset (guid) + m_sharedMaterial (fileID + guid) cho 13 TMP
    - Files modified: 1 scene (SCN_Main) + 7 prefab (InventorySlot, QuestListItem, UI_Nav_Button, UI_XP_ProgressBar, LockInfoPopup, QuestPopup, ShopPopup)
    - Không touch 2 TMP Dosis-ExtraBold ở ShopEntry_Seed (intentional)
  - Cập nhật Kiro Specs: thêm Spec 11 `m7c-font-wireup` = DONE
  - Commit:
    - `git add Assets/_Project/Scenes/SCN_Main.unity Assets/_Project/Prefabs/UI/Components/ Assets/_Project/Resources/UI/Default/ docs/HANDOVER.md docs/screenshots/`
    - Commit message: `fix(ui): m7c — wire 13 TextMeshPro components to Dosis-Bold SDF`
  - _Requirements: tất cả_
