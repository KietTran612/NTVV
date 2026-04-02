# Implementation Plan: Project Scaffolding & Core Foundation

> **For Antigravity:** REQUIRED WORKFLOW: Use `.agent/workflows/execute-plan.md` to execute this plan in single-flow mode.

**Goal:** Establish the complete folder structure and create the base C# scripts with correct namespaces and class definitions as per the approved design.

**Architecture:** Data-Driven Managed (Approach 1). Scripts are organized into logical namespaces (`NTVV.Core`, `NTVV.Data`, `NTVV.UI`).

**Tech Stack:** Unity URP, New Input System.

---

## Proposed Changes

### [Foundation] Project Scaffolding

#### [NEW] [Scaffold Folders]
Create the directory structure under `Assets/_Project/` as defined in the design document.

#### [NEW] [C# Script Shells]
Create all 18+ base scripts with appropriate `namespace`, `public class` definitions, and basic `[SerializeField]` placeholders for the data foundation.

---

## Task List

### Task 1: Directory Scaffolding
Create the directory structure under `Assets/_Project/`.

**Step 1: Create directories**

Run: `powershell -Command "New-Item -ItemType Directory -Force -Path 'Assets/_Project/Animations', 'Assets/_Project/Art/Sprites/Animals', 'Assets/_Project/Art/Sprites/Crops', 'Assets/_Project/Art/Sprites/Environment', 'Assets/_Project/Art/Sprites/UI', 'Assets/_Project/Art/Materials', 'Assets/_Project/Prefabs/World', 'Assets/_Project/Prefabs/UI', 'Assets/_Project/Scenes', 'Assets/_Project/ScriptableObjects/Crops', 'Assets/_Project/ScriptableObjects/Animals', 'Assets/_Project/ScriptableObjects/Items', 'Assets/_Project/ScriptableObjects/Player', 'Assets/_Project/Scripts/Core', 'Assets/_Project/Scripts/Data', 'Assets/_Project/Scripts/World', 'Assets/_Project/Scripts/UI', 'Assets/_Project/Scripts/Utilities', 'Assets/_Project/Settings'"`

**Step 2: Verify**

Run: `dir -Recurse Assets/_Project`

---

### Task 2: Data Scripts (ScriptableObjects)
**Files:**
- [NEW] `Assets/_Project/Scripts/Data/ItemData.cs`
- [NEW] `Assets/_Project/Scripts/Data/CropData.cs`
- [NEW] `Assets/_Project/Scripts/Data/AnimalData.cs`
- [NEW] `Assets/_Project/Scripts/Data/PlayerLevelData.cs`

**Step 1: Create ItemData.cs**
Implement base SO with ItemID, ItemName, Icon.

**Step 2: Create CropData.cs**
Inherit from ItemData. Add GrowthTime, BaseYield, HP thresholds.

**Step 3: Create AnimalData.cs**
Inherit from ItemData. Add FoodType, GrowthStages, SalePrice.

---

### Task 3: Core Manager Shells
**Files:**
- [NEW] `Assets/_Project/Scripts/Core/GameManager.cs`
- [NEW] `Assets/_Project/Scripts/Core/PlayerManager.cs`
- [NEW] `Assets/_Project/Scripts/Core/InventoryManager.cs`

**Step 1: Create GameManager.cs**
Implement basic Singleton and GameState enum.

**Step 2: Create PlayerManager.cs**
Fields for Gold, Level, XP.

---

## Verification Plan

### Automated Tests
- `ls -R Assets/_Project` to confirm folder structure.
- Check script content for correct namespace `NTVV.Data`, `NTVV.Core`, etc.

### Manual Verification
- Confirm directories appear in Unity Project window.
- Verify scripts compile without errors.
