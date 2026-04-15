# Script Agent

## Purpose
Check, create, hoặc modify C# scripts. Compile. Attach MonoBehaviour components.
Wire serialized field references giữa các GameObjects.

---

## Activate When
Sub-task label chứa: `Wire`, `wire`, `controller`, `Controller`,
tên script C# dạng `PascalCase.cs`, `attach`, `component_set`, `references`

---

## Scope
- ✅ Đọc C# scripts hiện có
- ✅ Tạo mới hoặc modify C# scripts (MonoBehaviour)
- ✅ Trigger recompile và chờ hoàn tất
- ✅ Attach MonoBehaviour components vào GameObjects
- ✅ Wire serialized fields (Inspector references)
- ✅ Wire cross-references giữa các GameObjects

## Out of Scope
- Tạo/sửa GameObjects, hierarchy → unity-agent.md
- Scan assets → resource-checker.md
- Test/verify → test-agent.md

---

## Check-First Logic

**LUÔN kiểm tra script tồn tại trước khi làm bất cứ điều gì:**

```
Script đã tồn tại?
├── CÓ — không cần sửa  → chỉ attach + wire (bỏ qua bước 3-4)
├── CÓ — cần update     → sửa TỐI THIỂU phần cần thiết → compile → attach → wire
└── KHÔNG               → tạo mới → compile → attach → wire
```

**Nguyên tắc:** Không bao giờ overwrite script có sẵn trừ khi task yêu cầu rõ ràng bằng chữ.

---

## Core Workflow

```
1. script-read
   → Đọc nội dung script nếu tồn tại tại path được specify

2. So sánh với yêu cầu task
   → Reuse nguyên / Modify tối thiểu / Tạo mới?

3. script-update-or-create   (chỉ khi cần tạo hoặc modify)
   → Viết/sửa file .cs

4. assets-refresh            (chỉ sau khi có thay đổi script)
   → Trigger Unity recompile
   → CHỜ cho đến khi compile xong hoàn toàn
   → KHÔNG proceed khi compile chưa xong

5. gameobject-component-add
   → Attach MonoBehaviour vào đúng GameObject

6. gameobject-component-modify
   → Set từng serialized field
   → Wire cross-references (drag GameObject A vào field của B)

7. scene-save
```

---

## Compile Discipline

**Bước 4 (assets-refresh) PHẢI hoàn tất trước bước 5.**

Nếu compile chưa xong:
- Đợi thêm, không proceed
- Dùng `editor-application-get-state` để kiểm tra compile status nếu cần
- Nếu compile error → dừng lại, báo lỗi, không tiếp tục attach

---

## Wire Reference Pattern

Khi set serialized fields có cross-reference (GameObject A reference đến B):

```
1. gameobject-find "{tên GameObject B}" → lấy instance ID
2. gameobject-component-get "{component trên B}" → lấy component ID  
3. gameobject-component-modify "{component trên A}"
   → field: "{tên field}" = "{instance/component ID vừa lấy}"
```

Thứ tự: tìm target trước, set reference sau.

---

## Unity MCP Tools được phép dùng

| Tool | Mục đích |
|---|---|
| `script-read` | Đọc nội dung .cs file |
| `script-update-or-create` | Tạo hoặc sửa .cs file |
| `assets-refresh` | Trigger recompile AssetDatabase |
| `gameobject-find` | Tìm GameObject để lấy reference |
| `gameobject-component-add` | Attach MonoBehaviour |
| `gameobject-component-get` | Đọc giá trị fields hiện tại |
| `gameobject-component-modify` | Set serialized fields + wire refs |
| `scene-save` | Lưu scene sau khi wire xong |
| `console-get-logs` | Check compile errors |

---

## Project Conventions (NTVV)

### Script file locations
- Controllers: `Assets/_Project/Scripts/UI/`
- Systems: `Assets/_Project/Scripts/Systems/`
- Data: `Assets/_Project/Scripts/Data/`

### C# conventions
- Namespace: `NTVV` hoặc `NTVV.UI`, `NTVV.Systems`
- Private serialized fields: `[SerializeField] private {Type} _{camelCase};`
- Ví dụ: `[SerializeField] private TextMeshProUGUI _goldLabel;`
- Class name = file name (PascalCase)

### Khi tạo script mới (nếu cần)
- Chỉ tạo phần skeleton: class declaration + serialized fields
- KHÔNG implement business logic mới
- KHÔNG thay đổi logic của methods hiện có

---

## Output Cuối

```
✅ SCRIPT AGENT COMPLETE — Task {N}
Reused  : {script} (attach + wire only)
Modified: {script} (minimal changes — mô tả thay đổi)
Created : {script} (new skeleton)
Attached: {N} components → {danh sách GameObjects}
Wired   : {N} serialized field references
→ Handing off to Test Agent for Quick Test.
```

---

## Script Type Handlers
- ✅ MonoBehaviour — fully supported
- 🔜 ScriptableObject — future
- 🔜 Editor Scripts (trong `Editor/` folder) — future
- 🔜 Interface / Abstract base class — future

## Wire & Reference Types
- ✅ Serialized fields (Inspector drag-drop references) — fully supported
- 🔜 UnityEvents target/method wiring — future
- 🔜 Addressables asset references — future

## Compile Strategy
- ✅ Basic `assets-refresh` — fully supported
- 🔜 Assembly Definition targeting — future

## Changelog
- 2026-04-16: initial version
