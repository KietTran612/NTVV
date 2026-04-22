# CLAUDE.md — NTVV Project Guide

Hướng dẫn nhanh cho Claude Code khi làm việc với project **NTVV (Nông Trại Vui Vẻ)**.

## Project identity

- **Tên**: NTVV — Nông Trại Vui Vẻ (Vietnamese "Happy Farm")
- **Engine**: Unity 2D
- **Genre**: Farm simulation prototype
- **Workflow**: Pure MCP (Unity MCP + Kiro specs) — không manual Unity Editor work trừ khi test visual
- **Source of truth cho session context**: `docs/HANDOVER.md` (đọc đầu mỗi session mới)

## Key directories

| Path | Purpose |
|------|---------|
| `Assets/_Project/` | Game code, assets, scenes, prefabs (KHÔNG đụng `Assets/StreamingAssets/realvirtual-MCP/` — MCP server bundled) |
| `.kiro/specs/<name>/` | Kiro specs (requirements/design/tasks.md) |
| `.kiro/steering/` | Rules Kiro agent đọc khi execute — **cũng áp dụng cho Claude Code khi viết spec** |
| `docs/HANDOVER.md` | Session context, milestone status, "bước tiếp theo" |
| `docs/backlog/bug-backlog.md` | Bug/feature backlog, BUG-XX + NOTE-XX numbering |
| `docs/asset-prompts/` | Prompts cho asset generation (ChatGPT + ComfyUI) |
| `docs/superpowers/specs/` | **Legacy** — pattern cũ trước 2026-04-22, không tạo mới |

## Spec conventions (quan trọng — ĐỌC TRƯỚC KHI VIẾT SPEC MỚI)

### Default: 3-file Kiro spec, KHÔNG tạo superpowers doc

Mỗi spec ở `.kiro/specs/<spec-name>/` chỉ có 3 file:
- `requirements.md`
- `design.md` (self-contained — Context + Design + Risks + Testing + Decision Log)
- `tasks.md`

**KHÔNG tạo** `docs/superpowers/specs/YYYY-MM-DD-*-design.md`. Pattern cũ đã deprecated cho project này từ 2026-04-22.

**Lý do:** Kiro agent (khi execute) chỉ auto-load files trong `.kiro/specs/<name>/`. Superpowers doc phải follow link thủ công → risk miss Risks/Testing sections mid-task. 1 source of truth tốt hơn maintain 2 files drift.

**Exception:** Chỉ tạo superpowers doc nếu:
1. User explicit yêu cầu "tạo lại superpowers doc" / "viết theo pattern cũ"
2. Brainstorm cover nhiều specs cùng lúc (legacy m6/m7 pattern)

### Chi tiết format

- **Rule tổng thể spec (requirements + design)**: xem `.kiro/steering/spec-format.md`
- **Rule tasks.md**: xem `.kiro/steering/task-format.md`
- **Resource check pattern**: xem `.kiro/steering/resource-checker.md`

### Quality principles (BẮT BUỘC đọc trước khi viết spec mới)

Chi tiết đầy đủ: `.kiro/steering/spec-format.md` section `🎯 Quality Rules`. Tóm tắt 5 nguyên lý cốt lõi:

1. **Verification-first** — Inspect code/scene/assets **TRƯỚC** khi viết. Không đoán. Dùng `Grep`, `Read`, `assets-find` để confirm state rồi mới spec values. Ghi findings vào Verification section.
2. **Every claim is testable** — Mỗi Req/step có MCP verify command + expected output. "Works correctly" → rephrase thành `verify <tool> → <specific result>`.
3. **Atomic steps** — Mỗi sub-task = 1 MCP tool call hoặc sequence ngắn. Không có step cần human judgment giữa chừng.
4. **Evidence over assumption** — "Verified 22/04: position = (7.37, 8.93, 0) from SCN_Main.unity:L5751" tốt hơn "position có vẻ sai".
5. **Specify failure modes** — Mỗi fragile operation (prefab modify, reflection, texture create) phải có fallback `X.⚠️` explicit. Không để agent tự quyết khi error.

### Red flags khi viết spec — grep tìm và fix

- ❌ `ensure|make sure|handle properly|appropriately` → vague, không testable
- ❌ File path thiếu root (`CropData.cs` thay vì `Assets/_Project/Scripts/Data/CropData.cs`)
- ❌ Code snippet không có line number / insert location
- ❌ Magic number không giải thích (e.g. `sortingOrder = 10` cần note "vì body ở 0-5")
- ❌ Task thiếu Quick Test block (`X.✓`)
- ❌ "Similar to previous spec" → viết đầy đủ inline, đừng force follow-link
- ❌ Optional mixed với required mà không mark `(Optional)`

### Pre-flight trước khi gõ chữ đầu vào spec

1. Đọc `docs/HANDOVER.md` — session context
2. Đọc `docs/backlog/bug-backlog.md` — confirm bug/feat IDs
3. `git log --oneline -20` + `git status` — không spec stale state
4. `Grep` + `Read` files trong scope — inspect actual code
5. Inspect scene file nếu có scene changes — đọc actual transform/values
6. Verify prerequisite specs DONE (check HANDOVER table)

### Override skill defaults khi brainstorm

Nếu invoke `superpowers:brainstorming` skill → đến bước "Write design doc":
- **Skip** bước write to `docs/superpowers/specs/`
- Viết design thẳng vào `.kiro/specs/<spec>/design.md`
- Giữ đủ: Context, Verification, Per-feature design, Risks, Testing Plan, Decision Log

## Workflow standard

1. **Đầu session**: Đọc `docs/HANDOVER.md` để nắm bối cảnh (có thể kèm user hỏi "chúng ta đã làm gì?")
2. **Brainstorm spec mới**: Skill `superpowers:brainstorming` — ask clarifying questions, propose approaches, verify state trước khi commit
3. **Viết spec**: 3 files Kiro theo convention trên
4. **User review gate**: Luôn yêu cầu user approve spec trước khi execute Task 1
5. **Execute**: Theo `tasks.md`, sub-task pattern `X.0 Resource Check → X.Y Implement → X.✓ Quick Test`
6. **Task cuối luôn là HANDOVER.md update**

## Project-specific rules

- **Language**: Spec + code comments tiếng Việt OK, nhưng code identifier + file names bắt buộc **English-only** (lowerCamelCase for fields, PascalCase for classes — theo rework 17/04/2026)
- **Asset naming**: `[Domain]_[Category]_[Entity]_[Variant]_[State].png` (ví dụ `World_Overlay_Tile_Lock_On.png`)
- **Stage numbering**: `Stage00`..`Stage03`, không `_v01`
- **Date format**: Spec file names dùng `YYYY-MM-DD`, converse `DD/MM/YYYY` với user OK
- **Git**: Không commit unless user explicit yêu cầu; không `--no-verify`; không push to main/master trừ khi user chỉ định

## Common MCP pitfalls (từ các spec cũ)

- **NOTE-07 m6a**: `gameobject-create(parentPath=...)` có thể tạo GO ở root scene thay vì parent chỉ định → LUÔN follow bằng `transform-set-parent` verify
- **NOTE-09 m6a**: `component-invoke` chỉ gọi được public parameterless methods → dùng `component-set` cho methods có param
- **NOTE-10 m6a**: `component-set` TMP text trong prefab stage không persist → fallback sửa trực tiếp file `.prefab` với Unicode escape

## Reference docs quick links

- Session history + milestone status: `docs/HANDOVER.md`
- Bug tracking: `docs/backlog/bug-backlog.md`
- Asset prompts: `docs/asset-prompts/2026-04-17-rework/master-index.md`
- UI methodology: `docs/guides/AI_UI_Integration_Methodology.md`
- Atomic HUD prompts: `docs/guides/Atomic_HUD_Prompt_Library.md`
