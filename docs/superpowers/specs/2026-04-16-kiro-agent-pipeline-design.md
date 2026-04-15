# Design Document: Kiro Agent Pipeline

**Date:** 2026-04-16
**Spec:** kiro-agent-pipeline
**Status:** approved

---

## Overview

Thiết lập hệ thống Steering files cho Kiro IDE để:

1. **Resource Checker** — tự động scan assets thiếu, generate prompts cho DALL-E / Stable Diffusion / ComfyUI, track lifecycle qua file markdown
2. **Unity Agent + Script Agent** — cung cấp operating procedure ổn định, giảm lặp lại convention trong mỗi task
3. **Test Agent** — verify sau mỗi task lớn (Quick Test) + verify toàn bộ spec ở cuối (Integration Test)
4. **Task Format** — dạy Kiro cách tự động thêm bước Resource Check và Quick Test vào tasks.md mỗi khi generate spec mới

Không tạo pipeline agent riêng biệt. Tất cả chạy trong **1 Kiro AI session duy nhất**, đóng vai lần lượt theo steering context.

---

## Architecture

```
Kiro IDE (1 AI session)
  │
  ├── reads .kiro/steering/ (luôn load khi bắt đầu)
  │     ├── task-format.md       ← hướng dẫn generate tasks.md
  │     ├── resource-checker.md  ← X.0: scan + prompt gen
  │     ├── unity-agent.md       ← X.1-X.3: Unity implementation
  │     ├── script-agent.md      ← X.4: script/compile/wire
  │     └── test-agent.md        ← X.✓: quick test + integration test
  │
  └── reads .kiro/specs/{spec}/
        ├── requirements.md
        ├── design.md
        └── tasks.md  ← generated theo format từ task-format.md
```

**Task structure chuẩn cho mỗi task lớn:**
```
Task X
  ├── X.0  · Resource Check    → resource-checker.md
  ├── X.1   ... implement ...  → unity-agent.md
  ├── X.2   ... implement ...  → unity-agent.md
  ├── X.3   ... implement ...  → unity-agent.md
  ├── X.4   Wire / Script      → script-agent.md
  └── X.✓  · Quick Test       → test-agent.md (verify task X only)

Task N (cuối). Integration Test  → test-agent.md (verify toàn spec)
```

**Files không bị thay đổi:**
- `.kiro/settings/mcp.json`
- `.kiro/specs/*/requirements.md` và `design.md` hiện có
- `scn-main-ui-rebuild/tasks.md` (đang dang dở, giữ nguyên)

---

## Scope

**Trong scope:**
- Tạo 5 steering files (thêm `test-agent.md`)
- Áp dụng format mới cho specs MỚI từ đây về sau
- Thêm task `1b. Resource Check` vào `scn-main-ui-rebuild/tasks.md` (spec đang dang dở)

**Ngoài scope:**
- Retrofit toàn bộ `scn-main-ui-rebuild/tasks.md`
- Tạo MCP server mới
- Kiro hooks / automation pipeline
- Parallel agents

---

## Components

### 1. `.kiro/steering/task-format.md`

Dạy Kiro cách generate tasks.md cho Unity specs. Khi Kiro tạo task mới từ requirements/design, nó tự thêm bước `X.0 · Resource Check` vào đầu mỗi top-level task.

**Output mẫu khi generate tasks:**
```markdown
- [ ] 2. Build TopHUDBar
  - [ ] 2.0 · Resource Check
    - Scan sprites/assets được reference trong task này
    - Nếu thiếu → generate prompts → lưu docs/asset-prompts/
    - NON-BLOCKING
  - [ ] 2.1 Tạo TopHUDBar container
    ...
  - [ ] 2.4 Wire HUDTopBarController
    ...
```

---

### 2. `.kiro/steering/resource-checker.md`

**Scope:** Scan assets trước khi implement, generate prompts nếu thiếu.

**Workflow:**
```
1. Đọc task requirements → xác định danh sách assets cần thiết
2. Check từng asset trong Assets/ project
3. Với mỗi asset thiếu:
   a. Tạo entry trong docs/asset-prompts/{date}-{spec}-missing-assets.md
   b. Generate 3 loại prompt (xem format bên dưới)
4. Output summary: X found, Y missing
5. NON-BLOCKING: luôn proceed dù có asset thiếu
```

**Asset naming convention:** `snake_case`, pattern `{object}_{state}.png`
Ví dụ: `barn_idle.png`, `chicken_walk.png`, `coin_icon.png`

**Base art style (dùng cho tất cả prompts):**
> "Bright casual farm, 2.5D isometric fake-3D using 2D assets; rounded 2D UI; bright, warm, easy-to-read colors; stylized, cute, production-friendly, pixel-free, not realistic, not true 3D."

**Format file `docs/asset-prompts/{YYYY-MM-DD}-{spec}-missing-assets.md`:**
```markdown
# Missing Asset Prompts
Spec: {spec-name}
Date: {YYYY-MM-DD}
Status: pending generation

---

## {filename}.png
Target path: Assets/{path}/{filename}.png
Size: {WxH} | Style: 2.5D isometric, cute cartoon, bright warm

### ChatGPT / DALL-E
Prompt: "..."
Setup: DALL-E 3, 1024x1024, style: vivid, quality: hd
Output filename: {filename}.png

### Stable Diffusion
Positive: "..."
Negative: "realistic, photo, 3D render, dark, gritty, pixel art,
           text, watermark, blurry, humans"
Setup: Checkpoint: dreamshaper_8 / anything-v5
       Sampler: DPM++ 2M Karras, Steps: 28, CFG: 7, Size: 512x512
Output filename: {filename}.png

### ComfyUI
Model: dreamshaper_8.safetensors
Positive: [same as SD]
Negative: [same as SD]
KSampler: dpmpp_2m, karras, steps=28, cfg=7.0
SaveImage filename: {filename}.png
Optional LoRA: game-icon-institute / flat-2d-animerge
```

**Asset lifecycle:**
```
pending   → chưa generate
done      → đã gen + copy vào Assets/ (user update thủ công)
resolved  → Phase 1 detect file đã tồn tại trong Assets/
applied   → Phase 2 đã replace placeholder + xóa TODO comment
```

**Behavior khi re-run:**
- Đọc prompts file, tìm entries `done`
- Check file tồn tại trong Assets/ → update `done → resolved`
- Signal Phase 2 replace placeholder tương ứng
- Không tạo lại prompt cho assets đã `resolved` hoặc `applied`

**Extension points (🔜 future):**
- Audio asset scan (`.wav`, `.mp3`)
- Animation clip scan
- Font asset scan
- Batch prompt generation cho toàn spec

---

### 3. `.kiro/steering/unity-agent.md`

**Scope:** Implement scene hierarchy, GameObjects, Components theo task requirements.

**Out of scope:** Viết/sửa C# scripts (→ script-agent.md)

**Core workflow:**
```
1. scene-open          → mở scene cần làm
2. gameobject-create   → tạo hierarchy
3. gameobject-component-add → attach components
4. gameobject-component-modify → set properties/values
5. assets-find         → locate assets cần dùng
6. Placeholder logic:
   - Asset có sẵn → dùng bình thường
   - Asset thiếu  → dùng Unity default / solid color sprite
                    + ghi comment: // TODO: replace with {filename}
7. scene-save          → sau mỗi sub-task
```

**Rules:**
- KHÔNG viết C# code mới
- KHÔNG xóa assets có sẵn
- KHÔNG thêm Unity packages mới
- Dùng đúng naming convention của project
- `editor_save_scene` sau mỗi sub-task

**Output cuối:**
```
✅ UNITY IMPLEMENTATION COMPLETE
- Created: [danh sách GameObjects]
- Modified: [danh sách thay đổi]
- Placeholders: [assets dùng placeholder]
```

**Extension points (🔜 future):**
- ScriptableObject instantiation
- Addressables asset loading
- Prefab variant creation

---

### 4. `.kiro/steering/script-agent.md`

**Scope:** Check/create/modify C# scripts, compile, attach components, wire serialized field references.

**Out of scope:** Scene hierarchy setup (→ unity-agent.md)

**Check-first logic:**
```
Script đã tồn tại?
├── CÓ, không cần sửa  → chỉ attach + wire
├── CÓ, cần update     → sửa tối thiểu → compile → attach → wire
└── KHÔNG              → tạo mới → compile → attach → wire
```

**Không bao giờ overwrite** script có sẵn trừ khi task yêu cầu rõ ràng.

**Core workflow:**
```
1. script-read                 → đọc script nếu tồn tại
2. So sánh với yêu cầu        → reuse / modify / create?
3. script-update-or-create     → (nếu cần)
4. assets-refresh              → trigger recompile, CHỜ xong
5. gameobject-component-add    → attach vào đúng GameObject
6. gameobject-component-modify → set serialized fields + cross-references
7. scene-save
```

**Compile discipline:** Bước 4 PHẢI hoàn tất trước bước 5. Nếu compile chưa xong → wait, không proceed.

**Output cuối:**
```
✅ SCRIPT AGENT COMPLETE
- Reused:   {script} (attach + wire only)
- Modified: {script} (changes made)
- Created:  {script} (new)
- Attached: {N} components
- Wired:    {N} serialized references
```

**Script Type Handlers:**
- ✅ MonoBehaviour — fully supported
- 🔜 ScriptableObject — future
- 🔜 Editor Scripts — future
- 🔜 Interface / Abstract base — future

**Wire & Reference Types:**
- ✅ Serialized fields (Inspector references) — fully supported
- 🔜 UnityEvents wiring — future
- 🔜 Addressables references — future

**Compile Strategy:**
- ✅ Basic `assets-refresh` — fully supported
- 🔜 Assembly Definition targeting — future

**Changelog:**
- 2026-04-16: initial version

---

### 5. `.kiro/steering/test-agent.md`

**Scope:** Verify sau mỗi task lớn (Quick Test) và verify toàn bộ spec ở cuối (Integration Test).

**Activate When:**
- Sub-task label chứa: `· Quick Test`, `X.✓`, `Smoke Test`, `Integration Test`

**Hai chế độ hoạt động:**

#### Quick Test (sau mỗi task lớn — `X.✓`)
Verify chỉ những gì task X vừa tạo/sửa:
```
1. editor_read_log filter=Error
   → Nếu có Error liên quan task X → FAIL, yêu cầu fix trước khi tiếp tục
   → Warning → log lại, không block

2. component_get các GameObjects vừa tạo
   → Verify references không null
   → Verify properties đúng giá trị

3. screenshot_scene → lưu docs/screenshots/{date}-{spec}-task{X}.png

4. Output:
   ✅ QUICK TEST PASSED: Task X
   hoặc
   ❌ QUICK TEST FAILED: Task X — [mô tả lỗi]
   → Nếu FAIL: fix trong task X, KHÔNG sang task X+1
```

**Fail behavior:**
- FAIL → loop lại yêu cầu Unity Agent / Script Agent fix — tối đa **2 lần**
- Sau 2 lần vẫn fail → escalate lên user, dừng lại

#### Integration Test (cuối spec)
Verify toàn bộ spec hoạt động cùng nhau:
```
1. sim_play → editor_wait_ready
2. editor_read_log filter=Error → 0 errors
3. Verify tất cả controllers có references không null
4. Verify prefabs load được
5. Smoke test các user flow chính
6. sim_stop → editor_save_scene
7. Output full test report
```

**Output Integration Test:**
```
📋 INTEGRATION TEST REPORT: {spec-name}
✅ PASSED: {N} checks
⚠️ WARNINGS: {danh sách}
❌ FAILED: {N} — [mô tả]
Placeholders còn lại: {danh sách assets chưa applied}
Screenshots: docs/screenshots/{date}-{spec}-integration.png
```

**Script Type Handlers:**
- ✅ Quick Test per task — fully supported
- ✅ Integration Smoke Test — fully supported
- 🔜 Automated regression test (NUnit) — future
- 🔜 Performance test — future

**Changelog:**
- 2026-04-16: initial version

---

## Data Flow

```
Task X bắt đầu
  ↓
[X.0 · Resource Checker]
  Scan assets → check Assets/ → generate prompts nếu thiếu
  Lưu: docs/asset-prompts/{date}-{spec}-missing-assets.md
  → PROCEED (non-blocking)
  ↓
[X.1-X.3 · Unity Agent]
  Implement hierarchy / GO / Components
  Asset thiếu → placeholder + // TODO comment
  → scene-save
  ↓
[X.4 · Script Agent]
  Check scripts → reuse/modify/create
  Compile → attach → wire references
  → scene-save
  ↓
[X.✓ · Quick Test]
  editor_read_log → 0 errors?
    ├── PASS → proceed sang Task X+1
    └── FAIL → fix (max 2 lần) → escalate nếu vẫn fail
  screenshot → docs/screenshots/{date}-{spec}-task{X}.png
  ↓
Task X+1 bắt đầu...

  ...

[Task N · Integration Test]
  sim_play → full smoke test toàn spec
  Full test report + screenshot
```

**Re-run (sau khi assets được thêm):**
```
[Resource Checker]
  Đọc prompts file → tìm entries "done"
  Check Assets/ → update "done → resolved"
  → signal placeholder replacement
  ↓
[Unity Agent]
  Tìm // TODO comments → replace placeholder với real asset
  Update prompts file: "resolved → applied"
  → scene-save
```

---

## File Locations

| File | Path |
|---|---|
| Task format steering | `.kiro/steering/task-format.md` |
| Resource checker steering | `.kiro/steering/resource-checker.md` |
| Unity agent steering | `.kiro/steering/unity-agent.md` |
| Script agent steering | `.kiro/steering/script-agent.md` |
| Test agent steering | `.kiro/steering/test-agent.md` |
| Missing asset prompts | `docs/asset-prompts/{date}-{spec}-missing-assets.md` |
| Quick test screenshots | `docs/screenshots/{date}-{spec}-task{X}.png` |
| Integration test screenshot | `docs/screenshots/{date}-{spec}-integration.png` |

---

## Migration: scn-main-ui-rebuild

Spec này đang dang dở (Task 1 done, Tasks 2-11 pending). **Không retrofit** toàn bộ.

Chỉ thêm 1 task mới:

```markdown
- [ ] 1b. Resource Check — scn-main-ui-rebuild
  - Scan tất cả sprites/icons được reference trong tasks 2-9
    (icon_Gold_Atomic, icon_Storage_Atomic, bg_Plaque_Wooden_Atomic,
     icon_Sprout_Header_Atomic, btn_Close_Circle_Atomic,
     icon_Tab_Leaf_Atomic, icon_Tab_Star_Atomic, v.v.)
  - Check Assets/ xem đã có chưa
  - Nếu thiếu → generate prompts → lưu docs/asset-prompts/
  - NON-BLOCKING: proceed regardless
```

---

## Art Style Reference

Dùng cho tất cả prompt generation trong project NTVV:

> "Bright casual farm, 2.5D isometric fake-3D using 2D assets; rounded 2D UI; bright, warm, easy-to-read colors; stylized, cute, production-friendly, pixel-free, not realistic, not true 3D."

**Negative (SD/ComfyUI):** `realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans`

**Recommended models:** dreamshaper_8, anything-v5
**Recommended LoRA:** game-icon-institute, flat-2d-animerge (optional)
