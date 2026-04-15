# Task Format Steering

## Purpose
Hướng dẫn Kiro cách generate `tasks.md` cho Unity specs trong project NTVV.
Áp dụng khi: tạo mới hoặc regenerate tasks.md từ requirements/design.

---

## Activate When
- Kiro được yêu cầu generate hoặc update `tasks.md`
- Kiro đang viết implementation plan cho một Unity spec

---

## Task Structure Template

Mỗi top-level task PHẢI có cấu trúc sau:

```markdown
- [ ] {N}. {Tên task}
  - [ ] {N}.0 · Resource Check
    - Scan: {liệt kê tên sprites/prefabs được reference trong task này}
    - Check từng asset trong Assets/ project
    - Nếu thiếu → append vào docs/asset-prompts/{date}-{spec}-missing-assets.md
    - NON-BLOCKING: luôn proceed dù có asset thiếu
  - [ ] {N}.1 {Bước implement đầu tiên}
    ...
  - [ ] {N}.2 {Bước implement tiếp theo}
    ...
  - [ ] {N}.X {Bước wire / attach script}  ← bước cuối implement
    ...
  - [ ] {N}.✓ · Quick Test
    - editor_read_log filter=Error → 0 errors liên quan task {N}
    - component_get {controllers vừa wire} → key references không null
    - screenshot_scene → lưu docs/screenshots/{date}-{spec}-task{N}.png
    - Nếu FAIL → fix trong task {N}, KHÔNG sang task {N+1}
```

---

## Rules khi generate

### 1. Resource Check (X.0)
- LUÔN là sub-task đầu tiên của mỗi top-level task implement
- Liệt kê CỤ THỂ tên assets được reference trong task đó (không viết chung chung)
- Nếu task không có sprite/asset mới → ghi "Không có sprite asset mới — SKIP scan"
- Không áp dụng cho task test, task extract prefab (task cuối), task update docs

### 2. Implement steps (X.1 → X.n)
- Giữ nguyên level of detail theo requirements/design
- Mỗi step kết thúc bằng `editor_save_scene` nếu có thay đổi scene
- Ghi `_Requirements: X.X` reference ở cuối mỗi step
- Step wire/attach script LUÔN là bước cuối cùng trước Quick Test

### 3. Quick Test (X.✓)
- LUÔN là sub-task CUỐI CÙNG của mỗi top-level task implement
- Kiểm tra CỤ THỂ components/references vừa tạo trong task đó
- Luôn có `screenshot_scene` → lưu file với pattern `task{N}.png`
- Ghi rõ: "Nếu FAIL → fix trong task {N}, KHÔNG sang task {N+1}"
- Không cần Quick Test cho: task 1b (Resource Check standalone), task docs update

### 4. Integration Test (task cuối)
- Task cuối của mỗi spec PHẢI là Integration Smoke Test
- Dùng `sim_play` → `editor_wait_ready` → verify toàn bộ → `sim_stop`
- Output full test report

---

## Ngoại lệ — KHÔNG thêm X.0 và X.✓ cho:
- Task đã done [x]
- Task "Resource Check standalone" (task 1b)
- Task "Extract Prefabs" → chỉ thêm X.✓, không cần X.0
- Task "Integration Smoke Test" → không thêm gì, đây là test tổng thể
- Task "Update HANDOVER.md" hoặc task docs

---

## Format conventions
- Checkbox: `- [ ]` cho pending, `- [x]` cho done
- Sub-task indent: 2 spaces
- Emoji phase markers: dùng `·` (middle dot) cho phase labels
- Requirements references: `_Requirements: X.X, X.Y_` ở cuối step
- Tên file screenshots: `{YYYY-MM-DD}-{spec-name}-task{N}.png`
- Tên file asset prompts: `{YYYY-MM-DD}-{spec-name}-missing-assets.md`
