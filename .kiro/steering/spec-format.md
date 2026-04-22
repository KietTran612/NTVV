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

---

## 🎯 Quality Rules — Viết spec chi tiết, cụ thể, chính xác

Phần này quan trọng: spec viết vague → agent execute sai/miss; spec viết chính xác → agent chạy smooth, ít retry. Đọc kỹ trước khi viết spec mới.

### Nguyên lý cốt lõi

1. **Verification-first**: Inspect code/scene/assets **TRƯỚC** khi viết spec. Không đoán — `script-read`, `Grep`, `Read` scene file, `assets-find` trước. Ghi findings vào Verification section.
2. **Every claim is testable**: Mỗi requirement/step phải có cách verify cụ thể qua MCP tool. Nếu không verify được → rephrase cho đến khi có thể.
3. **Atomic + idempotent steps**: Mỗi sub-task = 1 MCP tool call hoặc sequence ngắn. Chạy lại không làm hỏng state.
4. **Evidence over assumption**: "Verified 22/04: position = (7.37, 8.93, 0)" tốt hơn "position có vẻ sai".
5. **Specify failure modes**: Với mỗi fragile operation → fallback path explicit. Không để agent tự quyết định khi error.

---

### requirements.md — Quality checklist

**MỖI requirement PHẢI có:**

- [ ] **Testable acceptance** — cụ thể MCP tool nào check, expected result nào
- [ ] **Scope đơn concern** — 1 Req = 1 bug/feat. Không gộp 2 concerns vào 1 Req.
- [ ] **Bug/Feat ID reference** — `(BUG-XX)`, `(NOTE-XX m6a)`, `(FEAT-XX)` để trace back bug-backlog
- [ ] **Pre-condition explicit nếu có** — "Prerequisite: m7b DONE" ở overview, không giấu trong Req
- [ ] **Non-goals riêng biệt** — gom vào 1 bulleted list ở đầu file, không rải rác

**Example — Testable acceptance:**

❌ **Bad (vague)**:
```
2.1 CropData validation hoạt động đúng
```

✅ **Good (specific + testable)**:
```
2.1 `CropData.cs`: thêm `[Min(0.1f)]` cho `growTimeMin` (line 17), `perfectWindowMin` (line 39), `postRipeLifeMin` (line 40)
2.2 Acceptance: `script-read` → grep `\[Min\(0\.1f\)\]` → 3 matches
2.3 Acceptance: `assets-get-data crop_01.asset` → `growTimeMin` unchanged (không bị clamp vì current value > 0.1)
```

**Anti-pattern danh sách:**

- ❌ "Nice to have" Requirement — nếu không là must, không viết vào. Defer qua Non-goals.
- ❌ "ensure X works" / "make sure Y is correct" — không testable. Dùng verb + object + expected verify.
- ❌ Req conflict ngầm — 2 Req có yêu cầu trái nhau nhưng không nói priority. Phải rank.
- ❌ Missing `_Requirements: X.Y_` reference trong tasks.md.

---

### design.md — Quality checklist

**Context section:**
- [ ] **Milestone position** — "Sau spec X, trước milestone Y"
- [ ] **Why now** — constraint/blocker/deadline nào drive spec này
- [ ] **Scope rationale** — tại sao gộp/tách các features như vậy

**Verification section (bắt buộc nếu có inspect):**
- [ ] **Actual values, not guesses** — "Main Camera.position = (7.37, 8.93, 0) verified 22/04 từ scene file L5751"
- [ ] **Line number reference** — khi reference code, luôn kèm file:line
- [ ] **Grep/Read commands used** — ghi cách lấy info để reproduce

**Per-feature design:**
- [ ] **Root cause identified** (cho bug) — không chỉ symptom
- [ ] **Fix strategy có code snippet** — với context "chèn vào file X sau dòng Y"
- [ ] **Alternative considered** — "Tại sao chọn A thay vì B?"
- [ ] **File paths absolute từ project root** — `Assets/_Project/Scripts/...`, không chỉ `CropData.cs`

**Risks & Mitigations:**
- [ ] **Risk có trigger conditions** — "Nếu prefab có child đang override → ..."
- [ ] **Mitigation concrete** — không "handle gracefully", phải nói fallback path nào
- [ ] **Known MCP pitfalls listed** — reference NOTE-07/09/10 m6a nếu applicable

**Decision Log:**
- [ ] **Mỗi decision: Chosen + Alternatives + Why** — format chuẩn
- [ ] **Capture brainstorm trade-offs** — reviewer sau không phải đoán

**Example — Root cause specificity:**

❌ **Bad (symptom)**:
```
Camera render sai màu, cần fix transform
```

✅ **Good (root cause + evidence)**:
```
**Root cause:** Camera transform drift:
- Position: (7.37, 8.93, **0**) — Z=0 sai (2D ortho cần Z=-10)
- X=7.37 ngoài `FarmCameraController._boundX = (-5, 5)` → drift ngoài grid
- View frustum chiếu vào vùng không có renderer → solid background color
- Verified: `scene SCN_Main.unity:5751` đọc trực tiếp
```

**Anti-pattern:**

- ❌ Code snippet không có location ("thêm method này") — phải nói insert sau dòng nào
- ❌ "Similar to previous spec" — không, viết đầy đủ, đừng force reader đọc 2 file
- ❌ Magic numbers không explain — `sortingOrder = 10` phải nói tại sao (vì overlays body ở 0-5, cần phủ trên)
- ❌ Mixing description + instruction — chọn 1 mode/section

---

### tasks.md — Quality checklist (bổ sung task-format.md)

**Mỗi top-level task:**
- [ ] **Tên task = action rõ ràng** — "Fix BUG-08 — CropData validation attributes" (có BUG ID + scope)
- [ ] **Có `_Requirements: X.Y, X.Z_` reference** ở từng implement step
- [ ] **Pre-condition check nếu phụ thuộc task trước** — không giả sử, verify

**Mỗi implement sub-step:**
- [ ] **MCP tool call explicit** — `script-read`, `gameobject-modify`, không "edit file"
- [ ] **Expected result stated** — "expected: 0 errors", "expected: 1 hit"
- [ ] **Values cụ thể** — `localPosition = (1.2, 0.35, -10)`, không "center position"
- [ ] **Line numbers khi sửa code** — "tại line 17, thêm attribute trước `public float`"

**Code snippets trong task:**
- [ ] **Full context block** — include using/namespace nếu cần
- [ ] **Entry point specified** — "Entry method: `CreateLockOverlayPlaceholder.Run`"
- [ ] **Target location specified** — file:line hoặc method name

**Quick Test (`X.✓`):**
- [ ] **2-4 concrete checks** — không chỉ "verify works"
- [ ] **Expected output format** — "→ 0 errors", "→ 1 hit", "→ not null"
- [ ] **Retry limit explicit** — "tối đa 2 retry, sau đó STOP"
- [ ] **"Nếu FAIL → fix trong task N, KHÔNG sang task N+1"** ở cuối

**Fallback paths (`X.⚠️`):**
- [ ] **Có cho mỗi fragile operation** — prefab modify, texture create, reflection call
- [ ] **Trigger condition explicit** — "Nếu `assets-prefab-open` fail"
- [ ] **Alternative path có instructions đầy đủ**

**Example — Atomic sub-step:**

❌ **Bad (vague)**:
```
- [ ] 2.1 Update camera
  - Change position to center the grid
  - Make sure culling is correct
```

✅ **Good (atomic + testable)**:
```
- [ ] 2.1 · Scene Agent — Reset Main Camera transform
  - `gameobject-find(name="Main Camera")` → get current state
  - Log current transform để so sánh: expected (7.37, 8.93, 0) (verified 22/04)
  - `gameobject-modify` Main Camera:
    - `localPosition = (1.2, 0.35, -10)`
    - `localRotation = (0, 0, 0)` (Euler)
    - `localScale = (1, 1, 1)`
  - `component-get` Camera → verify đã đúng (không sửa):
    - `orthographic = 1`, `orthographicSize = 5`, `clearFlags = 1`, `cullingMask = -1`
  - `scene-save`
  - _Requirements: 3.1, 3.2, 3.3_
```

**Anti-pattern:**

- ❌ Sub-step đòi hỏi human judgment — "decide appropriate value" → agent bí
- ❌ MCP tool không tồn tại hoặc sai tên — check `.kiro/steering/` hoặc tool list trước
- ❌ Verify step thiếu expected output — "run console-get-logs" không đủ
- ❌ No Quick Test cho task có implement — luôn cần verify

---

### Anti-patterns tổng hợp (red flags khi self-review)

Trước khi commit spec, grep/scan cho các pattern sau. Nếu thấy → fix.

| Pattern | Vấn đề | Fix |
|---------|--------|-----|
| "ensure", "handle properly", "make sure", "appropriately" | Vague, không testable | Thay bằng `verify <tool> → <expected>` |
| Filename không có root (e.g. `CropData.cs`) | Ambiguous | Dùng `Assets/_Project/Scripts/Data/CropData.cs` |
| Code snippet không có location | Agent không biết chèn vào đâu | Thêm "tại line X" hoặc "sau method Y" |
| Magic number (e.g. `sortingOrder = 10`) | Không biết tại sao | Thêm comment giải thích |
| Task không có Quick Test | Không verify được | Thêm `X.✓ · Quick Test` block |
| Req dạng "works correctly" | Không testable | Rephrase thành acceptance với MCP check |
| Fallback = "handle gracefully" | Agent không biết làm gì | Spec concrete fallback steps |
| "Similar to spec X" reference | Force reader follow-link | Viết đầy đủ inline |
| Design claim về state nhưng không verify | Could be stale | Thêm Verification section với grep/read command |
| Optional steps mixed với required | Agent không biết skip nào | Mark "(Optional)" + conditional |

---

### Pre-flight checklist trước khi viết spec

Hoàn thành các bước sau **TRƯỚC KHI** gõ chữ đầu tiên vào spec files:

1. [ ] **Đọc `docs/HANDOVER.md`** — nắm session context, milestone position
2. [ ] **Đọc `docs/backlog/bug-backlog.md`** — confirm bug/feat IDs trong scope
3. [ ] **`git log --oneline -20`** — xem commits gần đây, không spec stale state
4. [ ] **`git status`** — biết working tree changes, đừng conflict với WIP
5. [ ] **Read/Grep files trong scope** — `Grep` cho symbols, `Read` cho code cần modify
6. [ ] **Inspect scene file nếu có scene changes** — đọc actual transform/component values
7. [ ] **Verify prerequisite specs DONE** — e.g. m8 assume m7b wired sprites
8. [ ] **List available MCP tools cần dùng** — confirm tồn tại qua `tool-list`

---

### Self-review checklist sau khi viết xong

Trước khi submit spec cho user review:

1. [ ] **Placeholder scan** — không còn `TBD`, `TODO`, `<fill in>`
2. [ ] **Internal consistency** — task count khớp giữa `requirements.md` + `design.md` + `tasks.md`
3. [ ] **Cross-references** — mỗi `_Requirements: X.Y_` trong tasks map về Req thực tế
4. [ ] **Path consistency** — dùng forward-slash, absolute từ project root
5. [ ] **Red-flag grep** — search file cho `ensure|make sure|handle|properly|appropriately` → fix nếu thấy
6. [ ] **Acceptance grep** — mỗi Req có acceptance bullet cụ thể
7. [ ] **Code snippets có location** — đọc lại, confirm agent biết chèn vào đâu
8. [ ] **Fallback paths có cho fragile ops** — prefab modify, texture create, reflection
9. [ ] **Non-goals list đầy đủ** — prevent scope creep
10. [ ] **Decision Log captures brainstorm** — không mất rationale
