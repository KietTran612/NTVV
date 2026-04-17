# Implementation Plan: m5-quest-flow

## Overview

Fix 4 bugs trong Quest runtime flow. Không viết script mới — chỉ sửa 2 files chính + verify 3 files read-only.

**Design doc:** `.kiro/specs/m5-quest-flow/design.md`
**Requirements:** `.kiro/specs/m5-quest-flow/requirements.md`

> ⚠️ ĐỌC GLOBAL RULES trong requirements.md TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO.

---

## Tasks

- [ ] 0. Audit quest data usage và unlock types thực tế
  - [ ] 0.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [ ] 0.4 · Research Agent — Đọc QuestData.cs + QuestDataSO.cs
    - `script-read` → `Assets/_Project/Scripts/Data/QuestData.cs`
    - Ghi nhận: `QuestUnlockType` enum có những giá trị nào
    - `script-read` → `Assets/_Project/Scripts/Data/ScriptableObjects/QuestDataSO.cs`
    - Ghi nhận: `prerequisiteQuestId`, `unlockType`, `unlockId` fields
    - `script-read` → `Assets/_Project/Scripts/Gameplay/Quests/QuestManager.cs`
    - Ghi nhận: `AcceptQuest()` line range, `HandleUnlock()` line range, `ClaimReward()` line range
    - `script-read` → `Assets/_Project/Scripts/UI/Panels/QuestPanelController.cs`
    - Ghi nhận: subscribe/unsubscribe pattern, `RefreshUI()` signature
    - `assets-find` pattern `*.asset` filter `Quest` → liệt kê QuestDataSO assets trong project
    - Mở từng asset (hoặc dùng `assets-inspect`) → ghi lại `unlockType` đang thực sự được dùng
    - Output audit report ngắn gọn:
      ```
      AUDIT: QuestUnlockType values in use:
        - [giá trị]: [số lượng quest dùng]
        - ...
      QuestUnlockType values NOT in use:
        - [giá trị]: Future
      AcceptQuest() prerequisite check: MISSING / EXISTS
      HandleUnlock() real behavior: MISSING / EXISTS
      QuestPanelController OnQuestStateChanged: SUBSCRIBED / NOT
      ```
    - _Requirements: thông tin nền cho Task 1-3_
  - [ ] 0.✓ · Quick Test
    - Verify: audit report hoàn chỉnh, không thiếu thông tin
    - Nếu không tìm thấy QuestDataSO assets → report + NON-BLOCKING (Task 3 sẽ implement với warning fallback)

- [ ] 1. Fix QuestManager — AcceptQuest() prerequisite enforcement (BUG-Q2)
  - [ ] 1.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [ ] 1.4 · Script Agent — Sửa QuestManager.cs AcceptQuest()
    - `script-read` → `Assets/_Project/Scripts/Gameplay/Quests/QuestManager.cs`
    - Tìm `AcceptQuest()` method (khoảng line 46-64)
    - Tìm đoạn check `minLevelRequired` — prerequisite guard phải đặt SAU level check, TRƯỚC `_activeQuests.Add(quest)`
    - **Fix BUG-Q2:** Thêm prerequisite guard:
      ```csharp
      if (!string.IsNullOrEmpty(quest.prerequisiteQuestId) &&
          !_completedQuestIds.Contains(quest.prerequisiteQuestId))
      {
          Debug.LogWarning($"[Quest] Prerequisite not completed: {quest.prerequisiteQuestId}");
          return;
      }
      ```
    - Đảm bảo thứ tự trong AcceptQuest(): null check → already-active check → already-completed check → level check → **prerequisite check** → `_activeQuests.Add(quest)` → init progress → fire event
    - `assets-refresh` → đợi compile xong
    - _Requirements: 1.1, 1.2, 1.3, 1.4_
  - [ ] 1.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors liên quan QuestManager
    - Verify: `AcceptQuest()` có prerequisite guard TRƯỚC `_activeQuests.Add`
    - Verify: guard dùng `_completedQuestIds.Contains(quest.prerequisiteQuestId)`
    - Nếu FAIL → fix trong task 1, KHÔNG sang task 2

- [ ] 2. Verify/fix QuestPanelController refresh behavior (BUG-Q1 + BUG-Q4)
  - [ ] 2.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [ ] 2.4 · Script Agent — Verify và fix QuestPanelController.cs
    - `script-read` → `Assets/_Project/Scripts/UI/Panels/QuestPanelController.cs`
    - Verify 4 điểm:
      - **A** — `OnEnable()` subscribe `QuestEvents.OnQuestStateChanged` và gọi `RefreshUI()` ngay để load initial state
      - **B** — `OnDisable()` unsubscribe đúng cách (tránh duplicate listeners)
      - **C** — `RefreshUI(string questId)` cập nhật cả active và completed quest list
      - **D** — QuestListItem trong list phản ánh đúng progress khi được update
    - Nếu **A** thiếu call `RefreshUI()` trong `OnEnable()` → thêm:
      ```csharp
      private void OnEnable()
      {
          QuestEvents.OnQuestStateChanged += RefreshUI;
          RefreshUI(""); // load initial state khi panel mở
      }
      ```
    - Nếu **B** đúng → giữ nguyên
    - Nếu **C** thiếu tab nào → thêm rebuild cho tab còn thiếu
    - `assets-refresh` → đợi compile xong nếu có sửa
    - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.5, 4.1, 4.2, 4.3_
  - [ ] 2.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: `OnEnable()` có cả subscribe VÀ `RefreshUI("")` call
    - Verify: `OnDisable()` unsubscribe
    - Nếu FAIL → fix trong task 2, KHÔNG sang task 3

- [ ] 3. Implement HandleUnlock() runtime behavior cho unlock types v1 (BUG-Q3)
  - [ ] 3.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - Verify audit từ Task 0 đã xác định `unlockType` đang được dùng trong data
    - NON-BLOCKING
  - [ ] 3.4 · Script Agent — Implement HandleUnlock() trong QuestManager.cs
    - `script-read` → `Assets/_Project/Scripts/Gameplay/Quests/QuestManager.cs`
    - Tìm `HandleUnlock()` method (khoảng line 144-149)
    - `script-read` → `Assets/_Project/Scripts/Data/QuestData.cs` — đọc lại QuestUnlockType enum
    - Dựa trên audit từ Task 0, implement theo từng case thực sự đang dùng trong data v1:

    **Case ShopTab_Animals (nếu đang được dùng):**
    ```csharp
    case QuestUnlockType.ShopTab_Animals:
        // Unlock animal tab trong shop UI
        var shopPanel = PopupManager.Instance?.GetPanel<ShopPanelController>();
        if (shopPanel != null)
            shopPanel.UnlockAnimalTab();
        else
            Debug.LogWarning($"[Quest] HandleUnlock: ShopPanelController not found for unlock {quest.rewards.unlockId}");
        Debug.Log($"[Quest] Unlock applied: {quest.rewards.unlockType} id={quest.rewards.unlockId}");
        break;
    ```
    *(Nếu ShopPanelController không có `UnlockAnimalTab()` → kiểm tra method thực tế trong file, không đoán)*

    **Case Building_NewNPC (nếu đang được dùng):**
    - Tìm system/flag tương ứng trong codebase trước khi implement
    - Nếu chưa có runtime hook → `Debug.LogWarning` + skip, KHÔNG crash

    **Case System_Crafting và các type chưa support:**
    ```csharp
    default:
        Debug.LogWarning($"[Quest] HandleUnlock: unlockType {quest.rewards.unlockType} not supported in v1. Skipping.");
        break;
    ```

    **Case None:**
    ```csharp
    case QuestUnlockType.None:
        return;
    ```

    - ⚠️ **Quan trọng:** KHÔNG implement case nào mà Task 0 audit xác nhận KHÔNG có trong data v1 — chỉ `Debug.LogWarning` + skip
    - `assets-refresh` → đợi compile xong
    - _Requirements: 3.1, 3.2, 3.3, 3.4, 4.4_
  - [ ] 3.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - Verify: `HandleUnlock()` không còn chỉ có `Debug.Log`
    - Verify: có `case QuestUnlockType.None: return;`
    - Verify: có `default: Debug.LogWarning(...)` cho unsupported types
    - Nếu FAIL → fix trong task 3, KHÔNG sang task 4

- [ ] 4. Integration Smoke Test
  - ⚠️ **Prerequisite:** `m4-animal-care` hoàn thành (crop/shop/animal flows phát QuestEvents ổn định)
  - [ ] 4.1 Prerequisite enforcement test
    - `editor-application-set-state` → Play mode
    - Verify: `console-get-logs` filter=Error → 0 errors khi load
    - Dùng `reflection-method-call` → `QuestManager.Instance.AcceptQuest([quest có prerequisite chưa completed])`
    - Verify: console có `"[Quest] Prerequisite not completed: ..."` warning
    - Verify: quest KHÔNG xuất hiện trong active list
    - _Requirements: 1.1, 1.3, 6.1_
  - [ ] 4.2 Prerequisite → unlock chain test
    - Dùng `reflection-method-call` → complete prerequisite quest
    - Dùng `reflection-method-call` → `QuestManager.Instance.AcceptQuest([quest kế tiếp])`
    - Verify: quest xuất hiện trong active list
    - Verify: Quest panel đang mở tự refresh (nếu panel đang hiện)
    - _Requirements: 1.4, 6.2_
  - [ ] 4.3 Progress update + UI refresh test
    - `reflection-method-call` → `QuestEvents.InvokeActionPerformed(...)` với action type phù hợp
    - Verify: `console-get-logs` → log progress update rõ ràng
    - Mở Quest panel → verify progress hiển thị đúng số
    - `reflection-method-call` → `QuestEvents.InvokeActionPerformed(...)` thêm lần nữa
    - Verify: panel tự cập nhật số mới (event-driven)
    - _Requirements: 2.2, 2.3, 4.2, 6.3_
  - [ ] 4.4 Claim reward + complete quest test
    - Đưa quest về trạng thái complete (đủ progress)
    - Mở Quest panel → tap Claim
    - Verify: quest biến khỏi active list, xuất hiện trong completed list
    - Verify: gold/xp được cộng đúng
    - Verify: nếu quest có unlock → console có log unlock applied
    - _Requirements: 2.4, 4.3, 4.4, 6.4, 6.5, 6.6_
  - [ ] 4.5 Panel reopen state test
    - Đóng Quest panel → thực hiện action làm quest progress thay đổi
    - Mở lại Quest panel → verify state mới hiển thị đúng (không stale)
    - _Requirements: 2.5, 6.7, 6.8_
  - [ ] 4.✓ · Integration Test Report
    - `screenshot-game-view` → lưu `docs/screenshots/2026-04-17-m5-quest-flow-integration.png`
    - Output:
      ```
      📋 INTEGRATION TEST REPORT: m5-quest-flow
      ✅ PASSED: [N] checks
      ⚠️ WARNINGS: [danh sách]
      ❌ FAILED: [N] — [mô tả]
      Bugs fixed: BUG-Q1, BUG-Q2, BUG-Q3, BUG-Q4
      Prerequisite enforcement: ✅
      UI refresh event-driven: ✅
      HandleUnlock runtime: ✅
      Console: 0 NullReferenceException QuestManager/QuestPanelController ✅
      ```
    - Nếu FAIL critical → fix, KHÔNG chuyển sang task 5

- [ ] 5. Update HANDOVER.md
  - Mở `docs/HANDOVER.md`
  - Cập nhật section "Phiên 17/04/2026":
    - Spec `m5-quest-flow` execute xong
    - 4 bugs fixed: BUG-Q1 UI refresh, BUG-Q2 prerequisite enforcement, BUG-Q3 HandleUnlock runtime, BUG-Q4 feedback clarity
    - `QuestManager.AcceptQuest()`: prerequisite guard hoạt động
    - `HandleUnlock()`: runtime effect thật cho unlock types v1
    - `QuestPanelController`: event-driven refresh verify + OnEnable initial state load
  - Cập nhật Kiro Specs: thêm Spec 6 `m5-quest-flow` = DONE
  - Cập nhật "Bước tiếp theo" → M6 hoặc milestones tiếp theo
  - _Requirements: tất cả_
