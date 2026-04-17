# Design Document: m5-quest-flow

**Date:** 2026-04-17
**Spec:** m5-quest-flow
**Status:** approved
**Kiro Spec Path:** `.kiro/specs/m5-quest-flow/`

---

## Overview

M5 hoàn thiện **quest runtime flow** trong SCN_Main. Quest system đã tồn tại đầy đủ trong `QuestManager`, `QuestPanelController`, `QuestEvents`, và các gameplay call sites, nhưng còn 4 điểm cần xử lý/verify: prerequisite chưa được enforce, unlock/reward kiểu feature chỉ log chứ không chạy thật, quest UI refresh cần được verify/fix để phản ánh đúng mọi state quan trọng trong runtime, và feedback state change hiện chưa đủ rõ cho player. Spec này tập trung vào **quest UX + logic cốt lõi**, không thêm quest content mới.

**Không thuộc scope M5:**
- Viết thêm quest data/content mới
- Dialogue system hoặc NPC narrative
- Popup/cinematic level-up hoặc quest animation phức tạp
- Refactor lớn toàn bộ quest architecture
- Hệ thống toast framework mới nếu codebase chưa có sẵn

---

## Bugs Cần Fix

### BUG-Q1 (MEDIUM): Quest UI refresh cần được verify/fix theo toàn bộ runtime flow
**Files:** `Assets/_Project/Scripts/UI/Panels/QuestPanelController.cs`, `Assets/_Project/Scripts/Gameplay/Quests/QuestManager.cs`

`QuestPanelController` **đã subscribe** `QuestEvents.OnQuestStateChanged`, nên panel có cơ chế auto refresh cơ bản. Tuy nhiên M5 cần verify kỹ và fix nếu cần để đảm bảo UI phản ánh đúng ở tất cả các thời điểm quan trọng:
- accept quest
- progress update
- claim reward / complete quest
- panel mở sau khi state đã thay đổi từ trước

**Fix direction:** giữ kiến trúc event-driven hiện có, chỉ vá các điểm refresh chưa đủ nếu phát hiện trong review/implementation; không chuyển sang polling hay refresh thủ công ở nhiều call sites.

---

### BUG-Q2 (HIGH): Prerequisite quest không được enforce trong AcceptQuest()
**File:** `Assets/_Project/Scripts/Gameplay/Quests/QuestManager.cs`

`QuestDataSO.prerequisiteQuestId` có trong data, nhưng `QuestManager.AcceptQuest()` hiện chỉ check `minLevelRequired`, không check prerequisite quest completion. Kết quả: player có thể nhận quest sai thứ tự progression.

**Fix:** trước khi accept quest:
```csharp
if (!string.IsNullOrEmpty(quest.prerequisiteQuestId) && !_completedQuestIds.Contains(quest.prerequisiteQuestId))
{
    Debug.LogWarning($"[Quest] Prerequisite not completed: {quest.prerequisiteQuestId}");
    return;
}
```

---

### BUG-Q3 (HIGH): HandleUnlock() chỉ log — unlock/reward không chạy thật
**File:** `Assets/_Project/Scripts/Gameplay/Quests/QuestManager.cs`

`HandleUnlock()` hiện chỉ `Debug.Log`, nghĩa là quest complete không tạo ra thay đổi runtime thật cho feature unlock. Người chơi complete quest nhưng không thấy hệ thống/tab/flag nào được mở.

**Fix direction:** implement unlock runtime thật cho các `unlockType` đang có trong data v1. Nếu gặp `unlockType` chưa support, log warning rõ ràng và skip — không crash.

---

### BUG-Q4 (MEDIUM): Thiếu feedback rõ ràng khi quest state đổi
**Files:** `QuestPanelController.cs`, có thể thêm ở `QuestManager.cs`

Khi quest progress/completion thay đổi, nếu UI list/item state không đổi rõ hoặc panel đang đóng, player khó hiểu chuyện gì vừa xảy ra. M5 không cần popup lớn, nhưng cần đảm bảo feedback runtime đủ rõ:
- panel đang mở → refresh ngay
- panel mở sau đó → thấy state mới đúng
- log state changes đủ rõ để test/verify

**Fix direction:** ưu tiên refresh UI đúng state; chỉ thêm log/toast nhẹ nếu codebase đã có pattern phù hợp.

---

## Architecture

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

### Thành phần chính

#### 1. `QuestManager`
Vẫn là trung tâm quest runtime:
- `AcceptQuest()`
- `HandleActionPerformed()`
- `IsQuestComplete()` / `GetQuestTotalProgress()`
- `ClaimReward()`
- `HandleUnlock()`
- persistence `LoadData()` / `SaveData()`

#### 2. `QuestPanelController`
Là subscriber UI chính:
- subscribe `QuestEvents.OnQuestStateChanged`
- `RefreshUI(string questId)` rebuild quest list theo state hiện tại
- unsubscribe đúng lifecycle để tránh duplicate listeners

#### 3. `QuestDataSO` / reward-unlock data
M5 không thêm data mới; chỉ dùng những gì đã có:
- `minLevelRequired`
- `prerequisiteQuestId`
- `rewards.goldReward`
- `rewards.xpReward`
- `rewards.unlockType`
- `rewards.unlockId`

---

## Runtime Rules

### Accept Flow
- Nếu quest null / đã active / đã complete → return như hiện tại
- Nếu player level chưa đủ → warning + return như hiện tại
- Nếu `prerequisiteQuestId` rỗng → accept bình thường
- Nếu có `prerequisiteQuestId` → chỉ accept khi quest đó đã complete
- Sau khi accept thành công → init runtime progress + fire `OnQuestStateChanged`

### Progress Flow
- `HandleActionPerformed()` tiếp tục là nơi duy nhất update progress runtime
- Progress phải clamp theo `requiredAmount` như hiện tại
- Khi có bất kỳ thay đổi nào → fire `OnQuestStateChanged`

### Complete / Claim Flow
- Khi player claim reward:
  - verify quest còn active + complete
  - add gold/xp
  - apply unlock runtime thật
  - remove khỏi active, add vào completed
  - xóa runtime progress entry
  - fire `OnQuestStateChanged`

### UI Refresh Rule
- Không dùng polling
- Không thêm nhiều manual refresh call sites ngoài event hiện có trừ khi thật cần
- Panel mở → refresh theo event
- Panel mở sau khi state đã đổi → `OnEnable()` refresh initial state đúng

---

## Unlock Strategy

M5 implement unlock theo hướng **nhỏ nhưng thật**:

- `HandleUnlock()` không còn chỉ log
- Với mỗi `unlockType` đang có trong data v1:
  - nếu đã có runtime/system flag tương ứng → set thật
  - nếu đã có UI visibility/runtime state tương ứng → bật thật
  - nếu chưa support → `Debug.LogWarning` rõ ràng, skip unlock đó

Mục tiêu: **quest complete phải gây ra thay đổi quan sát được**.

### Nguyên tắc triển khai
- Reuse system đang có, không invent framework unlock mới nếu chưa cần
- Không đoán ý nghĩa các `unlockType`; phải map theo enum/data thực tế trong codebase
- Nếu enum có nhiều loại nhưng v1 chỉ dùng 1-2 loại trong data hiện tại, chỉ support những loại đang thực sự được dùng

---

## Error Handling

Giữ ở mức gọn và đúng boundary:

- prerequisite quest id không tồn tại trong registry/save state → warning, không crash
- unlock type không hợp lệ hoặc chưa support → warning, skip unlock đó
- UI panel chưa mở mà event bắn ra → chấp nhận; panel sẽ refresh đúng ở lần mở tiếp theo
- Không thêm fallback phức tạp hay persistence format mới nếu không cần

---

## Critical Design Decisions

| Decision | Lý do |
|----------|-------|
| Giữ event-driven refresh | Đúng với kiến trúc quest hiện có, ít blast radius hơn polling/manual refresh |
| Enforce prerequisite tại `AcceptQuest()` | Đây là gate tự nhiên nhất của progression |
| Implement unlock ở `HandleUnlock()` thay vì rải logic ra nhiều nơi | Giữ một điểm xử lý reward side effects rõ ràng |
| Không thêm quest content mới | Giữ M5 tập trung vào runtime correctness, không làm scope phình |
| Chỉ support unlock types đang thực sự dùng ở v1 | YAGNI, tránh over-engineering |

---

## Task Summary

| Task | Mô tả | Loại |
|------|-------|------|
| 0 | Audit quest data usage và unlock types thực tế | Research |
| 1 | Fix `AcceptQuest()` prerequisite enforcement | Script fix |
| 2 | Verify/fix `QuestPanelController` refresh behavior | Script fix |
| 3 | Implement `HandleUnlock()` runtime behavior cho unlock types v1 | Script fix |
| 4 | Integration smoke test quest flow | Test |
| 5 | Update HANDOVER.md | Docs |

---

## Test Thành Công Khi

1. Quest có prerequisite không accept được nếu quest prerequisite chưa complete
2. Sau khi complete prerequisite, quest kế tiếp accept được bình thường
3. Gameplay event làm quest progress tăng đúng
4. Claim reward làm quest chuyển từ active sang completed
5. Reward gold/xp được cộng đúng
6. Unlock tạo ra thay đổi runtime thật, không chỉ log
7. Quest panel đang mở tự refresh khi state đổi
8. Mở lại quest panel sau đó vẫn thấy state đúng
9. Console: 0 NullReferenceException liên quan QuestManager / QuestPanelController

---

## References

- `Assets/_Project/Scripts/Gameplay/Quests/QuestManager.cs` — logic trung tâm
- `Assets/_Project/Scripts/Gameplay/Quests/QuestEvents.cs` — event bus
- `Assets/_Project/Scripts/UI/Panels/QuestPanelController.cs` — quest UI panel
- `Assets/_Project/Scripts/UI/Items/QuestListItem.cs` — item UI từng quest
- `Assets/_Project/Scripts/Data/ScriptableObjects/QuestDataSO.cs` — quest template data
- `Assets/_Project/Scripts/Data/QuestData.cs` — reward/unlock enums & data structs
- `docs/backlog/bug-backlog.md` — backlog quest issues
