# Spec Format Steering

## Purpose
Hướng dẫn cách generate Kiro specs cho Unity project NTVV.
Áp dụng khi: tạo spec mới (brainstorm output → design) hoặc regenerate spec từ requirements mới.

Companion của `task-format.md` (chỉ cover `tasks.md`). File này cover overall spec structure + `requirements.md` + `design.md`.

---

## Activate When
- Agent được yêu cầu viết spec mới cho NTVV
- Agent chuyển từ brainstorming sang design doc
- Agent update existing spec (adjust scope, add requirements)

---

## Spec Structure — 3 files Kiro, KHÔNG tạo superpowers doc

Mỗi spec ở `.kiro/specs/<spec-name>/` gồm 3 file:

```
.kiro/specs/<spec-name>/
├── requirements.md   ← Overview + Global Rules + Functional Requirements
├── design.md         ← Context + Design + Decision Log (self-contained)
└── tasks.md          ← Implementation plan (theo task-format.md)
```

### ❌ KHÔNG tạo

- `docs/superpowers/specs/YYYY-MM-DD-*-design.md` — pattern cũ trước 2026-04-22, đã deprecated cho project NTVV
- Separate brainstorm doc, alternative-analysis doc, context doc — tất cả gộp vào `design.md`

### ✅ Lý do chọn pattern 3-file

- Kiro agent chỉ auto-load files trong `.kiro/specs/<name>/`. Superpowers doc phải follow link thủ công → risk miss Risks/Testing sections mid-task
- Empirical check qua m3a-m7c: superpowers doc thường duplicate ~70% nội dung Kiro design → drift risk khi maintain 2 files
- Single source of truth, execution agent load 1 lần có đủ context

---

## requirements.md — Format

```markdown
# Requirements: <spec-name>

## Overview
<2-4 câu mô tả scope, why now, milestone position>

**Prerequisite:** <specs đã DONE cần thiết>
**Design doc:** `.kiro/specs/<spec-name>/design.md`
**Tasks:** `.kiro/specs/<spec-name>/tasks.md`

**Non-goals (cố ý excluded):**
- <items đã cân nhắc và loại bỏ khỏi scope>

---

## Global Rules
> ⚠️ ĐỌC TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO
- `script-read` trước khi sửa bất kỳ file nào
- `assets-refresh` sau mỗi script change, CHỜ compile xong trước khi tiếp tục
- `scene-save` sau mỗi thay đổi scene
- Quick Test FAIL → fix trong task hiện tại, KHÔNG sang task tiếp theo (tối đa 2 retry)
- Không refactor code ngoài scope đã design
- <các rule riêng cho spec này>

---

## Functional Requirements

### Req 1 — <Tên requirement> (<bug/feat ID nếu có>)
- 1.1 <yêu cầu cụ thể>
- 1.2 <yêu cầu cụ thể>
- 1.X Acceptance: <verify method cụ thể>

### Req 2 — <...>
...
```

**Rules:**
- Mỗi Req có số Acceptance bullet cụ thể (verify qua MCP tool nào → expected result)
- Req cuối cùng thường là Integration Test + HANDOVER update
- Không có Req dạng "nice to have" — scope strict, defer bằng Non-goals

---

## design.md — Format

**Target length:** 150-300 lines tùy complexity spec. Không có hard limit.

Sections theo thứ tự:

```markdown
# Design Document: <spec-name>

**Date:** YYYY-MM-DD
**Spec:** <spec-name>
**Status:** approved

---

## Context
<~20-40 lines>
- Milestone position (sau spec nào, trước milestone nào)
- Why now — vấn đề gì đang block, constraint nào
- Scope rationale — tại sao gộp/tách feature này

---

## Overview
<1-3 câu + file inventory table>

**Files cần sửa/tạo:**

| File | Thay đổi |
|------|----------|
| `<path>` | <tóm tắt> |

---

## Verification (nếu có)
<Nếu brainstorm có inspect scene/code/assets, ghi findings ở đây. Agent sẽ tham khảo khi execute.>

---

## <Per-Feature Design>
<1 section cho mỗi bug/feat trong scope. Bao gồm:>
- Root cause
- Fix strategy
- Code snippets (nếu cần)
- File paths cụ thể

---

## Risks & Mitigations

| Risk | Mitigation |
|------|-----------|
| <known failure mode> | <concrete fallback> |

---

## Testing Plan
<Per-task quick test + integration smoke test. Agent sẽ follow khi execute.>

---

## Decision Log
<Các quyết định trong brainstorm, format: decision + alternatives + why.>

### N. <Decision name>
- **Chosen**: <option>
- **Alternatives rejected**: <list + lý do>
- **Why**: <rationale>
```

**Rules:**
- Context + Decision Log bắt buộc — capture "why" cho human review sau
- Verification section optional, nhưng LUÔN có nếu brainstorm có inspect scene/asset state
- Risks & Mitigations phải có — agent cần khi gặp edge case mid-task
- Code snippets ở Per-Feature Design, không dup sang tasks.md

---

## tasks.md — Format

Theo `task-format.md` (file riêng, đã có sẵn). Spec này KHÔNG override task-format.md.

---

## Workflow để tạo spec mới

1. **Brainstorm** (usually bằng `superpowers:brainstorming` skill trong Claude Code) — ask clarifying questions, propose approaches, present design sections
2. **Verify** — inspect scene/asset/code state thực tế trước khi commit to spec (save to Verification section)
3. **Write 3 files** — requirements, design, tasks (theo format này + task-format.md)
4. **Self-review** — check placeholder, internal consistency, scope, ambiguity; fix inline
5. **User review gate** — yêu cầu user review trước khi execute Task 1
6. **Execute** — Task 0 → Task N sequentially (Kiro agent hoặc manual)
7. **HANDOVER update** — task cuối của spec luôn là update `docs/HANDOVER.md`

---

## Ngoại lệ — KHI NÀO tạo superpowers doc

Default: KHÔNG tạo. Chỉ tạo trong 2 trường hợp:

1. **User explicit yêu cầu** "tạo lại superpowers doc" hoặc "viết theo pattern cũ"
2. **Brainstorm cover nhiều specs cùng lúc** (ví dụ: 1 brainstorm → m6a + m6b) và user muốn archive combined design ở `docs/superpowers/specs/`

Trong 2 trường hợp này: tạo superpowers doc + giữ Kiro design.md ngắn (references superpowers). Khớp pattern m6/m7a-b cũ.

---

## Ví dụ tham khảo

- **1-file pattern** (recommended): `.kiro/specs/m8-scene-polish/design.md` (~290 lines, self-contained)
- **Pattern cũ 2-file** (legacy): `.kiro/specs/m6b-world-progression/design.md` + `docs/superpowers/specs/2026-04-20-m6-gameplay-features-design.md`
- **Pattern cũ đã converge 1-file**: `.kiro/specs/m7c-font-wireup/design.md` (Kiro đã dài hơn superpowers)
