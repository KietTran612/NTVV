---
name: ui-standardization
description: Use when creating or updating Unity uGUI prefabs in the NTVV project. Applies when building UI hierarchy from scratch, fixing broken prefab links, wiring controllers to child components, or encountering missing references in Inspector.
---

# UI Standardization for NTVV

## Overview

Prevent "Dead Prefabs" and broken Inspector links. This skill standardizes how Unity uGUI prefabs are built, wired, and styled in the NTVV farm game project.

**Core Principle:** Naming conventions enable recursive auto-wiring. Without predictable names, linking is fragile.

### Key Functions & Capabilities:
1.  **Controller Decision Matrix**: Evaluates if a prefab needs a C# controller based on dynamic data, interaction, or reusability.
2.  **Mandatory Naming Suffixes**: Enforces `_Label`, `_Icon`, `_Button`, `_Fill`, and `_Content` for reliable identification.
3.  **Recursive Auto-Wiring**: Provides robust patterns to find components at any depth, avoiding fragile hard-coded paths.
4.  **Standardized Wiring Workflow**: A 6-step sequence for guaranteed prefab integrity (Hierarchy -> Controller -> SerializeField -> Wiring -> Events -> Verify).
5.  **Quality & Styling Enforcement**: Ensures consistent fonts (no default TMP fonts), optimized raycasts, and project-standard palettes.
6.  **Code-based Event Binding**: Mandates `AddListener` in code for traceability and to prevent Inspector link breakage.
7.  **Verification Checklist**: A built-in "Final Check" to eliminate null references before completion.

## Decision Matrix: Do I Need a Controller?

Add a Controller script (C# MonoBehaviour at root) **only if**:
- Contains **dynamic data** (text/icon changes at runtime, e.g. Price, Quantity)
- Contains **user interaction** (Button that triggers game logic)
- Acts as a **reusable list template** (e.g. ShopEntry, InventorySlot, QuestRow)

**Skip Controller** if the prefab is purely decorative (static background, separator, frame border).

## Part 1: Naming Conventions (MANDATORY)

All child objects serving as data or interaction points MUST use these suffixes:

| Suffix | Unity Component | Example name |
|--------|----------------|--------------|
| `_Label` | TMP_Text | Price_Label, Name_Label |
| `_Icon` | Image (sprite display) | Item_Icon, Avatar_Icon |
| `_Button` | Button | Buy_Button, Close_Button |
| `_Fill` | Image (fillAmount) | XP_Fill, HP_Fill |
| `_Content` | RectTransform (layout container) | Items_Content, Slots_Content |

**Critical:** Use **full descriptive names** (e.g. `Price_Label`, `Name_Label`), NOT just the suffix alone. This prevents ambiguity when multiple similar children exist.

## Part 2: Recursive Auto-Wiring Pattern

When writing a Controller or Editor Assembly tool, always search by **exact full name**, not just suffix:

```csharp
// CORRECT: Search by exact child name
private T FindNamed<T>(string exactName) where T : Component
{
    foreach (Transform t in GetComponentsInChildren<Transform>(true))
        if (t.name == exactName)
            return t.GetComponent<T>();
    return null;
}

// In Awake/Initialize - use full descriptive names
void Awake()
{
    _priceLabel = FindNamed<TMP_Text>("Price_Label");
    _nameLabel  = FindNamed<TMP_Text>("Name_Label");
    _icon       = FindNamed<Image>("Item_Icon");
    _buyButton  = FindNamed<Button>("Buy_Button");
    _buyButton?.onClick.AddListener(OnBuyClicked);
}

// WRONG: Hard-coded paths break when hierarchy changes
// _priceLabel = transform.Find("Panel/Row/Price").GetComponent<TMP_Text>();

// WRONG: Suffix-only search - ambiguous with multiple same-type children
// _priceLabel = FindBySuffix<TMP_Text>("_Label"); // finds FIRST _Label, not Price_Label
```

In Editor Tools (PrefabAssembler pattern):

```csharp
private static T FindInHierarchy<T>(Transform root, string exactName) where T : Component
{
    foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
        if (child.name == exactName)
            return child.GetComponent<T>();
    return null;
}

// Usage inside UpgradePrefab<T>()
so.FindProperty("_priceLabel").objectReferenceValue =
    FindInHierarchy<TMP_Text>(instance.transform, "Price_Label");
so.ApplyModifiedProperties(); // NEVER forget this line
```


## Part 2b: Component Resolution Strategy (3-Tier)

When wiring components, apply tiers in order. Stop at the first tier that works.

### Tier 1 — Standard (prefabs we control)
Use **exact full name** search. This is the default for all NTVV-owned prefabs.

```csharp
// Use when: YOU created the prefab and it follows naming convention
_priceLabel = FindNamed<TMP_Text>("Price_Label");
```

### Tier 2 — Single-type Fallback (no naming control, but only 1 of that type)
Use when a prefab has only one Text or one Image and renaming is not possible.

```csharp
// Use when: Only ONE component of this type exists in the hierarchy
// Example: A simple button prefab from a legacy system
_label = GetComponentInChildren<TMP_Text>(true);
_icon  = GetComponentInChildren<Image>(true);
```

> [!WARNING] This breaks if the prefab later adds a second Text or Image. Re-evaluate when refactoring.

### Tier 3 — Hard-coded Path (third-party / Unity packages — no control at all)
Use only for external assets you cannot modify (Unity Store, packages).
MUST include an explicit comment explaining why.

```csharp
// LEGACY EXCEPTION: [PackageName] prefab - cannot rename, structure fixed by vendor
// Revisit if package is updated or replaced.
_label = transform.Find("Content/Inner/Text").GetComponent<TMP_Text>();
```

### Decision Flow

```
Can I rename the child object?
    YES → Use Tier 1 (FindNamed with full name)
    NO  → Is there only ONE component of this type?
              YES → Use Tier 2 (GetComponentInChildren)
              NO  → Use Tier 3 (hard-coded path + LEGACY comment)
```
## Part 3: Controller Wiring Workflow

Follow this sequence for every new prefab:

1. Build hierarchy - Create all child GameObjects with full descriptive suffix names (e.g. `Price_Label`)
2. Add Controller to root (if needed per Decision Matrix)
3. Add [SerializeField] for each linkable child in Controller
4. Wire in code - Use FindNamed<T>() in Awake() OR use PrefabAssembler for Editor-time wiring
5. Register events - Use button.onClick.AddListener() in code, NOT Inspector drag-drop
6. Verify - Open prefab in Unity, check Inspector for null references (None fields = fail)

## Part 4: Styling Requirements

- TMP_Text MUST NOT use "Liberation Sans" (Unity default) - verify in Inspector font asset field
- Project font assets are located in: `Assets/_Project/Fonts/`
- Image components for icons should have raycastTarget = false unless they are buttons
- Colors must use the project palette defined in `UIStyleApplier` (Assets/_Project/Scripts/UI/Common/)
- Do NOT hardcode pure white (#FFFFFF) or pure black (#000000) unless explicitly required

## Part 5: Event Binding Rules

Register button events in code only:

```csharp
// CORRECT: Register in code (traceable, refactorable)
_closeButton.onClick.AddListener(OnCloseClicked);

// WRONG: Unity Inspector event (breaks on rename, hard to find)
```

## Quick Verification Checklist

Before calling any prefab "done":

- [ ] Root has correct Controller (or none if decorative)
- [ ] All data/interaction children use full descriptive suffix naming (e.g. Price_Label)
- [ ] No None references in Inspector
- [ ] onClick listeners registered in code, not Inspector
- [ ] Font is NOT Liberation Sans (check Assets/_Project/Fonts/ for correct asset)
- [ ] ApplyModifiedProperties() called after SerializedObject changes in Editor code
- [ ] PrefabUtility.SaveAsPrefabAsset() called after editing in Editor tool

## Common Mistakes

| Mistake | Fix |
|---------|-----|
| NullReference on icon at runtime | Name mismatch - verify exact name matches FindNamed() call |
| Suffix-only search returns wrong child | Use exact full name (Price_Label) not suffix (_Label) |
| SerializedField still shows None after assembler | Forgot so.ApplyModifiedProperties() |
| Button OnClick not firing | Listener added in Inspector - move to Awake() |
| Multiple _Label children, wrong text shown | Use full names: Price_Label vs Name_Label |
| Prefab changes lost | Forgot PrefabUtility.SaveAsPrefabAsset() |