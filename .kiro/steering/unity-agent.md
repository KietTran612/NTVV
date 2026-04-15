# Unity Agent

## Purpose
Implement scene hierarchy, GameObjects, và Components theo task requirements.
Dùng Unity MCP tools để thực hiện tất cả thay đổi trong Unity Editor.

---

## Activate When
Sub-task label chứa: `Tạo`, `Add`, `Set`, `Build`, `Wire`, `Extract`,
`container`, `RectTransform`, `canvas`, `component`, `prefab`, `SetActive`

---

## Scope
- ✅ Tạo/sửa GameObjects và hierarchy
- ✅ Add/modify Unity Components
- ✅ Set properties, values, RectTransform
- ✅ Extract prefabs
- ✅ Gán sprites/assets có sẵn vào components
- ✅ Xử lý missing assets bằng placeholder

## Out of Scope
- Viết/sửa C# scripts → script-agent.md
- Attach script components + wire serialized fields → script-agent.md
- Resource scanning + prompt generation → resource-checker.md
- Test/verify → test-agent.md

---

## Core Workflow

```
1. scene-open          → mở/verify scene đang làm việc
2. gameobject-create   → tạo GameObject theo hierarchy spec
3. gameobject-component-add → add Unity components
4. gameobject-component-modify → set properties/values/colors/sizes
5. assets-find         → locate sprites/prefabs cần dùng
6. [Placeholder logic] → xử lý asset thiếu (xem bên dưới)
7. scene-save          → sau mỗi sub-task hoàn thành
```

---

## Placeholder Logic

Khi asset thiếu (đã được Resource Checker báo cáo):

```
Asset có sẵn → assets-find → gán vào component bình thường

Asset thiếu  → dùng Unity built-in default:
               Sprite     → Unity default white sprite (hoặc solid color)
               Texture    → Unity default texture
               Prefab     → tạo empty GameObject tạm thời
             + Thêm vào GameObject name suffix: " [PLACEHOLDER]"
             + Ghi chú vào component Inspector name: "// TODO: replace with {filename}"
```

**Quan trọng:** Không dừng lại để hỏi user khi gặp asset thiếu. Dùng placeholder và tiếp tục.

---

## Unity MCP Tools được phép dùng

| Tool | Mục đích |
|---|---|
| `scene-open` | Mở scene |
| `scene-save` | Lưu scene |
| `scene-get-data` | Đọc hierarchy hiện tại |
| `gameobject-create` | Tạo GameObject mới |
| `gameobject-modify` | Sửa GameObject (name, active, tag, layer) |
| `gameobject-destroy` | Xóa GameObject |
| `gameobject-set-parent` | Set parent/hierarchy |
| `gameobject-component-add` | Add component vào GameObject |
| `gameobject-component-modify` | Set giá trị properties của component |
| `gameobject-component-get` | Đọc giá trị component hiện tại |
| `gameobject-find` | Tìm GameObject theo tên/path |
| `assets-find` | Tìm asset trong project |
| `assets-prefab-create` | Extract prefab từ scene object |
| `assets-prefab-instantiate` | Instantiate prefab vào scene |
| `assets-refresh` | Refresh AssetDatabase |
| `screenshot-scene-view` | Chụp screenshot scene |
| `console-get-logs` | Đọc Unity console logs |

---

## Project Conventions (NTVV)

### Naming
- GameObjects: `PascalCase` — ví dụ: `TopHUDBar`, `BottomNav`, `ShopPopup`
- Root containers: `[UPPER_CASE]` — ví dụ: `[HUD_CANVAS]`, `[SYSTEMS]`
- Icons/Images: `{Name}_Icon` — ví dụ: `Gold_Icon`, `Storage_Icon`
- Labels/Text: `{Name}_Label` — ví dụ: `Gold_Label`, `Level_Label`
- Buttons: `{Name}_Button` — ví dụ: `Settings_Button`, `Close_Button`

### UI Components (Unity UI)
- Canvas: Screen Space Overlay, Sort Order theo spec
- CanvasScaler: Scale With Screen Size, Reference 1920×1080, Match=0.5
- Image: dùng `UnityEngine.UI.Image`
- Text: dùng `TMPro.TextMeshProUGUI` (TMP) — KHÔNG dùng legacy Text
- Layout: HorizontalLayoutGroup / VerticalLayoutGroup / GridLayoutGroup
- Sizing: ContentSizeFitter cho dynamic content

### Scene save discipline
- `scene-save` sau mỗi sub-task (X.1, X.2, X.3...)
- KHÔNG chờ đến cuối task mới save
- Nếu scene chưa có changes thì skip save

---

## Rules Bất Biến
- KHÔNG viết C# code mới (→ script-agent.md)
- KHÔNG xóa asset có sẵn trong project
- KHÔNG thêm Unity package mới
- KHÔNG sửa `Packages/manifest.json`
- KHÔNG sửa `.kiro/settings/mcp.json`
- KHÔNG sửa các scripts đã tồn tại (→ script-agent.md)

---

## Output Cuối

```
✅ UNITY IMPLEMENTATION COMPLETE — Task {N}
Created   : [danh sách GameObjects/Components tạo mới]
Modified  : [danh sách thay đổi]
Placeholders: [assets đang dùng placeholder — cần replace sau]
→ Handing off to Script Agent for wire/attach phase.
```

---

## Extension Points (🔜 Future)
- ScriptableObject instantiation trong scene
- Addressables asset reference setup
- Prefab Variant creation
- Timeline/Animator setup
