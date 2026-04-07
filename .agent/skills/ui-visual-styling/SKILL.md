---
name: ui-visual-styling
description: Use when adding visual decoration to an existing NTVV UI prefab — colors, sprites, backgrounds, shadows, fonts. Applies AFTER ui-standardization has wired the controller. Never breaks existing controller links.
---

# UI Visual Styling for NTVV

## Overview

This skill handles the **Visual / Decorator layer** of UI prefabs. It is always applied **after** `ui-standardization` has built the hierarchy and wired the controller.

**Core Principle:** Two systems own two separate namespaces. They never touch each other's objects.

```
ui-standardization  →  _Suffix names    (Controller territory)
ui-visual-styling   →  prefix_ names    (StyleApplier territory)
```

**Trigger:** Use this skill when the user says things like:
- "Làm đẹp ShopEntry"
- "Thêm background, shadow cho button"
- "Áp màu sắc / sprite theo mockup này"
- "Tạo giao diện theo theme Cartoon"

---

## Step 0: Pre-flight Check

Before doing anything, verify:
1.  **Blueprint Approval**: `@ui-blueprinting` has been run and the user has approved the Blueprint MD file.
2.  **Prefab Strategy**: The target prefab already exists at `Assets/_Project/Resources/UI/Default/` and has a Controller wired (run `@ui-standardization` first).
3.  **Applier Ready**: A `UIStyleApplier.cs` component exists on the prefab root.

---

## Part 1: Full Naming Convention Reference

### Functional (Controller territory) — DO NOT TOUCH
| Suffix | Component | Example |
|--------|-----------|---------|
| `Name_Label` | TMP_Text | `Price_Label`, `Name_Label` |
| `Name_Icon` | Image (data sprite) | `Item_Icon`, `Currency_Icon` |
| `Name_Button` | Button | `Buy_Button`, `Close_Button` |
| `Name_Fill` | Image (fillAmount) | `XP_Fill`, `HP_Fill` |
| `Name_Content` | RectTransform layout | `Items_Content`, `Slots_Content` |

### Decorator (StyleApplier territory) — THIS SKILL'S DOMAIN
| Prefix | Meaning | Example |
|--------|---------|---------|
| `bg_` | Background / panel | `bg_Card`, `bg_Button`, `bg_PriceChip` |
| `shadow_` | Drop shadow | `shadow_Card`, `shadow_Button` |
| `border_` | Decorative border/frame | `border_Frame`, `border_Panel` |
| `overlay_` | Shine / highlight overlay | `overlay_Shine`, `overlay_Highlight` |
| `fx_` | Special effect placeholder | `fx_Sparkle`, `fx_Glow` |

**Safety Rule:** This skill ONLY searches for and modifies objects whose names START with `bg_`, `shadow_`, `border_`, `overlay_`, or `fx_`. Never touch `_Suffix` objects.

---

## Part 2: Input Analysis Workflow

The user may provide any combination of:
- **A — Text description**: "Button màu xanh lá, bo tròn, có bóng đổ nhẹ"
- **B — Image/mockup**: A PNG or screenshot of the desired look
- **C — Both**: Mockup for overall layout + text for specific details

### When receiving input:

**From text (A):**
- Extract: target element names, colors (hex or descriptive), sprite types, font info
- Map to decorator names: "button background" → `bg_Button`, "card shadow" → `shadow_Card`

**From image (B):**
- Analyze each visual region of the image
- Identify layered elements: background panels, shadows, overlays, icons, text
- Map regions to the prefab's expected hierarchy
- Extract dominant colors per region
- Determine if flat color or textured sprite is needed

**From both (C):**
- Use image for layout/position/visual weight
- Use text to override specific details (exact hex, font size, padding)

---

## Part 3: Decorator Scaffolding

Before applying styles, ensure the decorator objects exist in the prefab hierarchy.

### Placement Rules:
- `bg_*` objects: **ALWAYS first children** (SetAsFirstSibling) so they render behind everything
- `shadow_*` objects: Second layer, just after `bg_*`
- `overlay_*` objects: **ALWAYS last children** (SetAsLastSibling) so they render on top
- `border_*` objects: After shadow, before functional children
- `fx_*` objects: Last layer or as needed per effect

### Standard hierarchy for a Card-style prefab:
```
PrefabRoot
  ├── shadow_Card      ← FIRST (renders behind bg too, extends outside)
  ├── bg_Card          ← Second
  ├── border_Card      ← Optional: decorative frame
  │
  ├── [Functional children — DO NOT REORDER]
  │   ├── Item_Icon
  │   ├── Name_Label
  │   └── Buy_Button
  │       ├── bg_Button    ← bg inside button
  │       └── Buy_Label    ← functional, managed by Controller
  │
  └── overlay_Shine    ← LAST (renders on top of everything)
```

### Creating decorator children via UIStyleApplier:
`UIStyleApplier.FindOrCreateDecorator(name)` handles this automatically:
- If `bg_Card` exists → reuse it
- If not → create it as Image component, stretch to fill parent, correct sibling order

---

## Part 4: UIStyleDataSO — What to Fill

Create or update the ScriptableObject at:
`Assets/_Project/Data/UI/Styles/Default/[PrefabName]_StyleData.asset`

### SpriteStyleEntry (for bg_, shadow_, border_, overlay_, fx_)
```csharp
targetName: "bg_Card"          // exact decorator name
sprite:     [assign Sprite]    // the sprite asset
tint:       Color(1,1,1,1)     // white = no tint, use color to tint
border:     Vector4(12,12,12,12) // 9-slice border for stretchable panels
```

### ColorStyleEntry (flat color, no sprite needed)
```csharp
targetName: "bg_Button"
color:      #4CAF50FF          // Farm green
```

### ButtonStyleEntry (Button component states)
```csharp
targetName:  "Buy_Button"      // MUST match a _Button functional child
colorBlock:
  normalColor:      #4CAF50
  highlightedColor: #66BB6A
  pressedColor:     #388E3C
  disabledColor:    #BDBDBD
```

### FontStyleEntry (per text element)
```csharp
targetName: "Name_Label"       // MUST match a _Label functional child
font:       [assign TMP Font]
fontSize:   18
fontStyle:  Bold
```

> [!WARNING]
> `ButtonStyleEntry` and `FontStyleEntry` reference `_Suffix` functional children by NAME ONLY (string lookup) — the StyleApplier does NOT hold a reference to the component itself. This keeps the two systems decoupled.

### LayoutStyleEntry (NEW)
Apply aesthetic layout configurations defined in the Blueprint to existing Layout Groups.
- **`LayoutConfig.Padding`**: Set [Left, Right, Top, Bottom].
- **`LayoutConfig.Spacing`**: Set GAP between elements.
- **Target**: Horizontal/VerticalLayoutGroup on the prefab root or specific child.

---

## Part 5: Applying Styles

### Runtime (Awake)
`UIStyleApplier.ApplyStyle()` is called in `Awake()`. It:
1. Iterates `spriteEntries` → finds/creates `bg_*`, `shadow_*` etc. → sets sprite/color
2. Iterates `colorEntries` → applies flat color
3. Iterates `buttonEntries` → finds Button by child name → sets ColorBlock
4. Iterates `fontEntries` → finds TMP_Text by child name → sets font/size/style

## Part 5: Applying Styles (Pure MCP)

### Editor Bake (Manual AI Execution)

> [!IMPORTANT]
> Tuyệt đối không cần code tool C# phức tạp. AI thực hiện cấu hình và "Bake" trực tiếp!

1. **Configure Style Data**: Sử dụng `assets-get-data` và `assets-modify` để cập nhật file `_StyleData.asset` (màu sắc, sprites).
2. **Apply to Prefab**: 
   - Sử dụng tool `ui-prefab-style` (MCP) để kích hoạt quá trình Apply dữ liệu từ SO vào linh kiện.
   - Hoặc thủ công dùng `object-modify` gán trực tiếp thuộc tính vào các linh kiện `bg_`, `shadow_`.
3. **Save**: Gọi `assets-prefab-save` để lưu lại thay đổi.

**Result:** Thiết kế visual được lưu cứng (Persistent) trong Prefab.

---

## Part 6: Theme Switching

To add a new theme (e.g., Cartoon):
1. Duplicate `ShopEntry_StyleData.asset` to `Data/UI/Styles/Cartoon/`
2. Update sprite/color references for the new theme
3. Change `UIStyleApplier._styleData` reference to the Cartoon asset
4. Bake with [Apply to Prefab NOW]

> The Controller wiring is **never affected** by theme switching.

---

- [ ] All decorator children use correct prefix (`bg_`, `shadow_`, `border_`, `overlay_`, `fx_`)
- [ ] Decorator children are in correct render order (bg_ first, overlay_ last)
- [ ] `UIStyleApplier` component is on the prefab root
- [ ] `_styleData` field on UIStyleApplier is NOT null
- [ ] All `_Suffix` Controller links are STILL intact
- [ ] Bake đã được thực hiện và lưu lại (`assets-prefab-save`)
- [ ] Font là **Dosis** (Check Material Outline tương ứng)
- [ ] Button ColorBlock đã được set

---

## Common Mistakes

| Mistake | Fix |
|---------|-----|
| StyleApplier accidentally overwrites `Item_Icon` sprite | Only modify `prefix_` named children |
| `bg_Card` not visible in Editor | Run [Apply to Prefab NOW] |
| Controller links broken after styling | StyleApplier only used FindOrCreateDecorator — check for naming collision |
| Decorator disappeared after Assembler re-ran | Assembler must be in Verify mode (Create-or-Verify pattern) |
| Button stays default grey | Add ButtonStyleEntry with correct `targetName` matching `Buy_Button` |
| Multiple bg_ on same object | FindOrCreateDecorator reuses existing — check name is exact match |

---

## Quick Reference: Skill Sequence

```
1. @ui-blueprinting         → Analyzes design & creates blueprint (MANDATORY START)
2. @ui-standardization      → Verifies naming + wiring (Functional Layer - SKELETON)
3. @ui-visual-styling       → Adds decoration layer (Visual Layer - SKIN)
   a. Analyze user input (text/image/both)
   b. Apply Colors, Sprites, Fonts (Dosis + Material)
   c. Apply Layout Specs (Padding, Spacing) from Blueprint
   d. Scaffold decorator children (bg_, shadow_...)
   c. Create/update _StyleData.asset
   d. Assign to UIStyleApplier
   e. [Apply to Prefab NOW]
   f. Verify checklist
```
