# Hybrid UI System Implementation Plan

> **For Antigravity:** REQUIRED WORKFLOW: Use `.agent/workflows/execute-plan.md` to execute this plan in single-flow mode.

**Goal:** Implement the foundational HUD and navigation using UI Toolkit, while maintaining the uGUI bridge for gameplay popups.

**Architecture:** 
- UI Toolkit for persistent system UI (HUD, BottomNav).
- uGUI for contextual gameplay UI (ContextActionPanel).
- Bridge logic in `UIManager` to manage both systems.

**Tech Stack:** Unity 2022+ UI Toolkit, C#, uGUI.

---

### Task 1: Asset Export from Pencil
**Files:**
- Create: `Assets/_Project/UI/Textures/Avatar.png`
- Create: `Assets/_Project/UI/Textures/GoldIcon.png`
- Create: `Assets/_Project/UI/Textures/XPBar_Fill.png`
- Create: `Assets/_Project/UI/Textures/Chip_BG.png`

**Step 1: Export Avatar**
- Export node `WwUdK` as `Avatar.png`.

**Step 2: Export Gold Icon**
- Export gold icon from `7xs0S` as `GoldIcon.png`.

**Step 3: Export XP Bar Fill**
- Export fill from `6Tk2h` as `XPBar_Fill.png`.

**Step 4: Commit Assets**
```bash
git add Assets/_Project/UI/Textures/*.png
git commit -m "style: export initial UI assets from Pencil"
```

### Task 2: Global Styling (USS)
**Files:**
- Create: `Assets/_Project/UI/System/USS/farm-theme.uss`

**Step 1: Define Color Variables**
```css
:root {
    --farm-green: #4CAF50;
    --deep-leaf-green: #2E7D32;
    --cream-white: #FFFBE6;
    --berry-pink: #E91E8C;
    --sky-blue: #87CEEB;
    --warning: #FF9800;
    --danger: #F44336;
    --glass-bg: rgba(0, 0, 0, 0.4);
}
```

**Step 2: Define Common Classes**
```css
.hud-chip {
    background-color: var(--glass-bg);
    border-radius: 20px;
    padding: 8px 16px;
    flex-direction: row;
    align-items: center;
}

.primary-btn {
    background-color: var(--farm-green);
    border-radius: 12px;
    color: white;
    -unity-font-style: bold;
}
```

**Step 3: Commit**
```bash
git add Assets/_Project/UI/System/USS/farm-theme.uss
git commit -m "style: add global USS theme variables"
```

### Task 3: TopHUD Implementation
**Files:**
- Create: `Assets/_Project/UI/System/UXML/HUD/TopHUD.uxml`
- Create: `Assets/_Project/UI/System/Scripts/HUDController.cs`

**Step 1: Create UXML Structure**
- Define `VisualElement` container with `TopHUD` class.
- Add avatar, XP group, Gold chip, and Storage chip.

**Step 2: Implement HUDController logic**
- Bind to `PlayerManager.OnGoldChanged` and `PlayerManager.OnXPChanged`.
- Bind to `InventoryManager.OnStorageChanged`.
- Update `Label` text and `ProgressBar` value.

**Step 3: Commit**
```bash
git add Assets/_Project/UI/System/UXML/HUD/TopHUD.uxml Assets/_Project/UI/System/Scripts/HUDController.cs
git commit -m "feat: implement TopHUD with UI Toolkit binding"
```

### Task 4: UIManager Bridge
**Files:**
- Modify: `Assets/_Project/Scripts/UI/UIManager.cs`

**Step 1: Add UI Toolkit references**
- Add `public UIDocument SystemUIDocument;`
- Add methods to show/hide specific UXML screens.

**Step 2: Commit**
```bash
git add Assets/_Project/Scripts/UI/UIManager.cs
git commit -m "refactor: extend UIManager to support UI Toolkit layers"
```
