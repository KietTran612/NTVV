# Design Document: m5-quest-flow

**Date:** 2026-04-17
**Spec:** m5-quest-flow
**Status:** approved
**Full design:** `docs/superpowers/specs/2026-04-17-m5-quest-flow-design.md`

---

## Overview

Fix quest runtime flow trong SCN_Main. Không thêm quest content mới — chỉ hoàn thiện logic/runtime correctness cho prerequisite, unlock, và UI refresh.

**Files cần sửa:**
| File | Bugs/Task |
|---|---|
| `Assets/_Project/Scripts/Gameplay/Quests/QuestManager.cs` | BUG-Q2, BUG-Q3, BUG-Q4 |
| `Assets/_Project/Scripts/UI/Panels/QuestPanelController.cs` | BUG-Q1, BUG-Q4 |
| `Assets/_Project/Scripts/UI/Items/QuestListItem.cs` | verify/fix state display nếu cần |
| `Assets/_Project/Scripts/Data/QuestData.cs` | read-only verify unlock enum usage |
| `Assets/_Project/Scripts/Data/ScriptableObjects/QuestDataSO.cs` | read-only verify prerequisite/unlock fields |

---

## Bug Fix Summary

### BUG-Q2 — AcceptQuest() chưa enforce prerequisiteQuestId

`QuestManager.AcceptQuest()` hiện chỉ check `minLevelRequired`, không check `prerequisiteQuestId`. Player có thể nhận quest sai thứ tự progression.

**Fix:** thêm prerequisite guard trước `_activeQuests.Add(quest)`:
```csharp
if (!string.IsNullOrEmpty(quest.prerequisiteQuestId) && !_completedQuestIds.Contains(quest.prerequisiteQuestId))
{
    Debug.LogWarning($"[Quest] Prerequisite not completed: {quest.prerequisiteQuestId}");
    return;
}
```

---

### BUG-Q1 — Quest UI refresh cần verify/fix theo toàn bộ flow

`QuestPanelController` đã subscribe `QuestEvents.OnQuestStateChanged`, nên kiến trúc event-driven hiện có là đúng. Tuy nhiên cần verify refresh behavior cho 4 flow: accept, progress update, claim reward, reopen panel sau khi state đổi.

**Fix direction:** giữ nguyên event-driven refresh, chỉ vá nếu `RefreshUI()` hoặc lifecycle listeners còn thiếu case.

---

### BUG-Q3 — HandleUnlock() chỉ log

`QuestManager.HandleUnlock()` hiện chỉ `Debug.Log`, chưa tạo runtime effect thật.

**Fix direction:** implement runtime effect thật cho các `QuestUnlockType` đang thực sự được dùng trong data v1; `unlockType` chưa support thì warning + skip.

---

### BUG-Q4 — Feedback state change chưa đủ rõ

Quest progress/claim flow đã có log cơ bản, nhưng M5 cần đảm bảo state changes đủ rõ để player và integration test đều nhìn thấy được.

**Fix direction:** ưu tiên UI state đúng; chỉ thêm log rõ ràng ở các điểm cần verify runtime behavior.

---

## Architecture (giữ nguyên, hoàn thiện flow)

```text
Player action
  → QuestEvents.InvokeActionPerformed(...)
  → QuestManager.HandleActionPerformed(...)
  → update runtime progress
  → if progress changed: fire OnQuestStateChanged
  → QuestPanelController.RefreshUI(...)

Player claim reward
  → QuestManager.ClaimReward(...)
      → grant gold/xp
      → HandleUnlock()
      → move active → completed
      → fire OnQuestStateChanged
  → QuestPanelController.RefreshUI(...)
```

**M5 giữ nguyên kiến trúc event-driven hiện có.** Không chuyển sang polling hoặc framework mới.

---

## Critical Design Decisions

| Decision | Lý do |
|---|---|
| Enforce prerequisite tại `AcceptQuest()` | Gate progression đúng chỗ, ít blast radius |
| Giữ event-driven refresh | Khớp kiến trúc hiện có của QuestEvents + QuestPanelController |
| Implement unlock tại `HandleUnlock()` | Một điểm xử lý side effects rõ ràng |
| Chỉ support unlock types đang thực sự dùng ở v1 | Tránh over-engineering |
| Không thêm quest content mới | Giữ M5 tập trung vào runtime correctness |

---

## Runtime Rules

### Accept flow
- Quest null / đã active / đã completed → return như hiện tại
- Level chưa đủ → warning + return như hiện tại
- `prerequisiteQuestId` rỗng → accept bình thường
- `prerequisiteQuestId` có giá trị nhưng chưa completed → warning + return
- Accept thành công → init progress runtime + fire `OnQuestStateChanged`

### Progress flow
- `HandleActionPerformed()` là nơi duy nhất update progress
- Progress continue clamp theo `requiredAmount`
- Có thay đổi progress → fire `OnQuestStateChanged("")`

### Claim flow
- Chỉ claim khi quest active và complete
- Add gold/xp
- Apply unlock runtime thật
- Remove khỏi active, add vào completed, clear progress runtime
- Fire `OnQuestStateChanged(quest.questId)`

### UI rule
- Panel mở → refresh theo event
- Panel mở lại sau đó → `OnEnable()` refresh initial state
- Không thêm polling
