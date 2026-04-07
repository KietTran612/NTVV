# UI Atomic Components Implementation Plan

> **For Antigravity:** REQUIRED WORKFLOW: Use `.agent/workflows/execute-plan.md` to execute this plan in single-flow mode.

**Goal:** Assemble and visually style the `InventorySlot` and `ShopEntry_Seed` atomic prefabs utilizing full MCP automated Tools (Architect, Builder, Stylist).

**Architecture:** 
1. Use `unity-skill-create` to build an MCP bridge to the existing `PrefabAssembler` and `UIStyleApplier`.
2. Construct the exact functional hierarchy (Decorator background, Functional contents).
3. Execute the MCP tools to auto-wire the controllers and bake the visual Styles.

**Tech Stack:** Unity Editor API, C# MCP Plugins, Unity uGUI.

---

### Task 1: Build AI-Resident Pipeline (Builder & Stylist Tools)

**Files:**
- Create: `Assets/_Project/Scripts/Editor/MCP/Tool_UIBuilder.cs`
- Create: `Assets/_Project/Scripts/Editor/MCP/Tool_UIStyling.cs`

**Step 1: Write Builder MCP Tool**
Call `unity-skill-create` to create `Tool_UIBuilder` plugin with a method `[McpPluginTool("ui-prefab-assemble")]` which takes a `GameObject` instance ID or name and executes `PrefabAssembler.AssembleAll()` or targets the object.

**Step 2: Write Stylist MCP Tool**
Call `unity-skill-create` to create `Tool_UIStyling` plugin with a method `[McpPluginTool("ui-prefab-style")]` that delegates visual styling. 

**Step 3: Commit**
```bash
git add .
git commit -m "feat: Add MCP bridge tools for UI Builder and Stylist"
```

### Task 2: Standardize InventorySlot

**Files:**
- Modify: `Assets/_Project/Prefabs/UI/Components/InventorySlot.prefab`

**Step 1: Construct Hierarchy**
Using `assets-prefab-open`, modify `InventorySlot`:
- Add `bg_slot_frame` (if missing).
- Add `Item_Icon` (Image).
- Add `Quantity_Label` (TMP_Text, Dosis).

**Step 2: Auto-Wire Component**
Use the newly generated `ui-prefab-assemble` tool to wire the components internally.

**Step 3: Visual Bake**
Use `ui-prefab-style` to bake the colors, fonts, and renderer settings. Use `assets-prefab-close` (save: true).

**Step 4: Commit**
```bash
git add Assets/_Project/Prefabs/UI/Components/InventorySlot.prefab
git commit -m "ui: Standardize and style InventorySlot component"
```

### Task 3: Standardize ShopEntry_Seed

**Files:**
- Modify: `Assets/_Project/Prefabs/UI/Components/ShopEntry_Seed.prefab`

**Step 1: Construct Hierarchy**
Using `assets-prefab-open`, modify `ShopEntry_Seed`:
- Add `bg_entry_card`.
- Setup `Seed_Icon`, `Name_Label`, `PriceContainer` (with `Price_Icon` and `Price_Label`), and `Buy_Button`.

**Step 2: Auto-Wire Component**
Use `ui-prefab-assemble` on the ShopEntry.

**Step 3: Visual Bake**
Use `ui-prefab-style` to bake aesthetics. Save prefab.

**Step 4: Commit**
```bash
git add Assets/_Project/Prefabs/UI/Components/ShopEntry_Seed.prefab
git commit -m "ui: Standardize and style ShopEntry_Seed component"
```
