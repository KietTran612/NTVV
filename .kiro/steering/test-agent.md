# Test Agent

## Purpose
Verify kết quả sau mỗi task lớn (Quick Test) và verify toàn bộ spec ở cuối (Integration Test).
Fail fast — phát hiện lỗi sớm, không cho phép lỗi từ task trước lan sang task sau.

---

## Activate When
Sub-task label chứa: `· Quick Test`, `.✓`, `Quick Test`,
`Smoke Test`, `Integration Test`, `Integration Smoke Test`

---

## Scope
- ✅ Quick Test: verify task vừa hoàn thành
- ✅ Integration Test: verify toàn bộ spec
- ✅ Console error scan
- ✅ Component reference validation
- ✅ Screenshot capture

## Out of Scope
- Fix lỗi phát hiện (→ unity-agent.md hoặc script-agent.md)
- Test logic gameplay (→ manual testing)
- Performance testing (🔜 future)

---

## Chế Độ 1: Quick Test (`X.✓`)

Verify CHỈ những gì task X vừa tạo/sửa.

### Workflow

```
1. Console scan
   → console-get-logs filter=Error
   → Tìm errors liên quan task X (tên GO, script, component vừa tạo)
   → Error KHÔNG liên quan task X: bỏ qua (ghi warning)

2. Component reference check
   → gameobject-find "{GameObject chính của task}"
   → gameobject-component-get "{Controller/Component vừa wire}"
   → Verify key serialized fields không null

3. Screenshot
   → screenshot-scene-view
   → Lưu: docs/screenshots/{YYYY-MM-DD}-{spec}-task{N}.png

4. Output kết quả (xem format bên dưới)
```

### Quick Test Output

```
🔍 QUICK TEST — Task {N}: {Tên task}
──────────────────────────────────────
Console : ✅ 0 errors | ⚠️ {N} warnings
Refs    : ✅ {component} — tất cả key fields không null
          ❌ {component}.{field} = null  ← nếu có
Screenshot: docs/screenshots/{date}-{spec}-task{N}.png
──────────────────────────────────────
Result  : ✅ PASSED  →  Proceed to Task {N+1}
          ❌ FAILED  →  Fix required (attempt {1/2})
```

### Fail Behavior

```
FAIL lần 1 → mô tả lỗi cụ thể → yêu cầu Unity Agent / Script Agent fix
FAIL lần 2 → mô tả lỗi cụ thể → yêu cầu fix lần 2
FAIL lần 3 → ESCALATE → dừng lại, thông báo user:
  "⛔ Task {N} Quick Test failed after 2 fix attempts.
   Lỗi: {mô tả}
   Cần can thiệp thủ công trước khi tiếp tục."
```

**Quan trọng:** Khi Quick Test FAILED, KHÔNG tự động sang task tiếp theo.
User phải xác nhận hoặc fix xong mới tiếp tục.

---

## Chế Độ 2: Integration Test (Task cuối spec)

Verify toàn bộ spec hoạt động cùng nhau.

### Workflow

```
1. Pre-check (không cần play mode)
   → console-get-logs filter=Error → đếm errors
   → assets-find tất cả prefabs được tạo trong spec → verify tồn tại

2. Play Mode smoke test
   → editor-application-set-state: Play
   → editor-application-get-state: chờ IsPlaying=true
   → console-get-logs filter=Error → 0 errors (NullRef, Missing Component, Failed to load)

3. Component verification (trong play mode)
   → gameobject-component-get các controllers chính → key refs không null

4. Basic interaction test (nếu có thể)
   → reflection-method-call: invoke button click handlers
   → verify không có exception

5. Stop play mode
   → editor-application-set-state: Stop
   → scene-save

6. Screenshot tổng thể
   → screenshot-scene-view
   → Lưu: docs/screenshots/{YYYY-MM-DD}-{spec}-integration.png

7. Placeholder summary
   → Liệt kê tất cả assets đang dùng placeholder (Status: pending/done)
   → Nhắc nhở: "Các assets này cần được generate và replace"
```

### Integration Test Output

```
📋 INTEGRATION TEST REPORT — {spec-name}
Date: {YYYY-MM-DD}
═══════════════════════════════════════════
PREFABS     : ✅ {N}/{N} prefabs exist
CONSOLE     : ✅ 0 errors in play mode
              ⚠️ {N} warnings (xem chi tiết)
CONTROLLERS : ✅ {N}/{N} key references not null
              ❌ {controller}.{field} = null (nếu có)
INTERACTIONS: ✅ {N} button/event handlers respond
═══════════════════════════════════════════
PLACEHOLDERS REMAINING:
  ⚠️ {filename}.png — Status: pending (cần generate)
  ⚠️ {filename}.png — Status: done (cần apply)
═══════════════════════════════════════════
OVERALL: ✅ PASSED  /  ❌ FAILED ({N} issues)
Screenshot: docs/screenshots/{date}-{spec}-integration.png
Prompts: docs/asset-prompts/{date}-{spec}-missing-assets.md
```

---

## Unity MCP Tools được phép dùng

| Tool | Mục đích |
|---|---|
| `console-get-logs` | Đọc Unity console logs |
| `console-clear-logs` | Clear logs trước khi test |
| `gameobject-find` | Tìm GameObject để verify |
| `gameobject-component-get` | Đọc component values |
| `assets-find` | Verify prefab/asset tồn tại |
| `screenshot-scene-view` | Chụp scene view |
| `screenshot-game-view` | Chụp game view (play mode) |
| `editor-application-set-state` | Play / Stop |
| `editor-application-get-state` | Check editor state |
| `reflection-method-call` | Invoke button/method để test |
| `scene-save` | Lưu scene sau test |

---

## Screenshot Naming Convention

```
Quick Test     : docs/screenshots/{YYYY-MM-DD}-{spec}-task{N}.png
Integration    : docs/screenshots/{YYYY-MM-DD}-{spec}-integration.png
```

Tạo thư mục `docs/screenshots/` nếu chưa tồn tại.

---

## Test Type Handlers
- ✅ Quick Test per task — fully supported
- ✅ Integration Smoke Test — fully supported
- 🔜 Play mode automated interaction test — future
- 🔜 NUnit / Unity Test Framework — future
- 🔜 Performance/profiler test — future
- 🔜 Visual regression test (screenshot diff) — future

## Changelog
- 2026-04-16: initial version
