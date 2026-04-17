# Requirements: m5-quest-flow

## Overview

Fix quest runtime flow trong SCN_Main. Quest system đã tồn tại đầy đủ trong `QuestManager`, `QuestPanelController`, `QuestEvents`, và các gameplay call sites — spec này hoàn thiện 4 điểm cần xử lý/verify: prerequisite enforcement, unlock runtime behavior, quest UI refresh correctness, và feedback state change đủ rõ cho runtime.

**Prerequisite:** `m4-animal-care` nên hoàn thành trước — animal, crop, shop flows phải đã phát `QuestEvents.InvokeActionPerformed(...)` ổn định để quest progress có dữ liệu thật.

**Design doc:** `docs/superpowers/specs/2026-04-17-m5-quest-flow-design.md`

---

## Global Rules

> ⚠️ ĐỌC TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO

- Chỉ sửa đúng những gì bug yêu cầu — không refactor thêm
- `script-read` trước khi sửa bất kỳ file nào
- `assets-refresh` sau mỗi script change, CHỜ compile xong trước khi tiếp tục
- `editor_save_scene` sau mỗi sub-task có Unity change
- Quick Test FAIL → fix trong task hiện tại, KHÔNG sang task tiếp theo (tối đa 2 retry, sau đó escalate)
- **Không viết system mới lớn** — chỉ reuse kiến trúc quest hiện có
- Không thêm quest content/data mới trong M5

---

## Functional Requirements

### Req 1 — Prerequisite Enforcement (BUG-Q2)
- 1.1 `QuestManager.AcceptQuest()` kiểm tra `quest.prerequisiteQuestId` trước khi accept quest
- 1.2 Nếu `prerequisiteQuestId` rỗng → flow accept giữ nguyên
- 1.3 Nếu `prerequisiteQuestId` không rỗng và quest đó chưa completed → `Debug.LogWarning` rõ ràng + không accept
- 1.4 Nếu prerequisite đã completed → quest accept bình thường

### Req 2 — Quest UI Refresh Correctness (BUG-Q1)
- 2.1 `QuestPanelController` tiếp tục dùng `QuestEvents.OnQuestStateChanged` làm nguồn refresh chính
- 2.2 Accept quest → panel đang mở refresh đúng ngay lập tức
- 2.3 Progress update → panel đang mở refresh đúng progress
- 2.4 Claim reward / complete quest → panel đang mở refresh đúng active/completed state
- 2.5 Panel mở sau khi state đã thay đổi từ trước → `OnEnable()` refresh initial state đúng

### Req 3 — Unlock Runtime Behavior (BUG-Q3)
- 3.1 `QuestManager.HandleUnlock()` không còn chỉ log
- 3.2 Với mỗi `QuestUnlockType` đang thực sự được dùng trong data v1, unlock phải tạo ra runtime effect thật
- 3.3 Nếu gặp `QuestUnlockType.None` → return như hiện tại
- 3.4 Nếu gặp `unlockType` chưa support trong v1 → `Debug.LogWarning` rõ ràng, không crash

### Req 4 — Runtime Feedback Clarity (BUG-Q4)
- 4.1 Khi quest accept thành công → có log rõ ràng như hiện tại
- 4.2 Khi quest progress thay đổi → log update rõ ràng đủ để verify trong integration test
- 4.3 Khi claim reward thành công → log reward claimed như hiện tại
- 4.4 Khi unlock được apply → phải có thay đổi runtime quan sát được hoặc log rõ rằng unlock đã apply

### Req 5 — Event-driven Architecture Preservation
- 5.1 `QuestManager.HandleActionPerformed()` vẫn là nơi duy nhất update runtime progress
- 5.2 `QuestEvents.InvokeQuestStateChanged(...)` vẫn là event chính để quest UI refresh
- 5.3 Không chuyển sang polling hoặc manual refresh rải rác ở nhiều call sites nếu không thật sự cần

### Req 6 — Integration
- 6.1 Quest có prerequisite không accept được khi prerequisite chưa complete
- 6.2 Sau khi complete prerequisite, quest kế tiếp accept được bình thường
- 6.3 Gameplay event từ crop/shop/animal làm quest progress tăng đúng
- 6.4 Claim reward làm quest chuyển từ active sang completed
- 6.5 Reward gold/xp được cộng đúng
- 6.6 Unlock tạo ra thay đổi runtime thật, không chỉ log
- 6.7 Quest panel đang mở tự refresh khi state đổi
- 6.8 Mở lại quest panel sau đó vẫn thấy state đúng
- 6.9 Console: 0 NullReferenceException liên quan QuestManager / QuestPanelController / QuestListItem
