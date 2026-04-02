# Core Operational Framework Implementation Plan (Final Review)

> **For Antigravity:** REQUIRED WORKFLOW: Use `.agent/workflows/execute-plan.md` to execute this plan in single-flow mode.

**Goal:** Implement the `GameManager`, `SaveLoadManager`, and `FarmCameraController` to enable persistent gameplay and world navigation, while standardizing all manager classes and handling offline time growth.

**Architecture:** 
- Centralized `GameManager` (Singleton) for state orchestration and **Time Advancement** on load.
- `SaveLoadManager` (Singleton) for JSON-based local file persistence.
- `FarmCameraController` for direct user interaction with the world.
- Refactor all managers to use `Singleton<T>`.

**Tech Stack:** Unity, C#, JsonUtility, System.IO.

---

### Task 1: Data Models & Time Utility

**Files:**
- Create: `Assets/_Project/Scripts/Core/SaveData.cs`

**Step 1: Define SaveData structure**
Include `lastSaveTimestamp` in `PlayerSaveData` for offline growth calculation.

**Step 2: Commit**
```bash
git add Assets/_Project/Scripts/Core/SaveData.cs
git commit -m "feat: add save data models with timestamp support"
```

---

### Task 2: Standardize Managers with Singleton Base

**Files:**
- Modify: `Assets/_Project/Scripts/Gameplay/Economy/EconomySystem.cs`
- Modify: `Assets/_Project/Scripts/Gameplay/Storage/StorageSystem.cs`
- Modify: `Assets/_Project/Scripts/Gameplay/Progression/LevelSystem.cs`
- Modify: `Assets/_Project/Scripts/UI/Common/PopupManager.cs`

**Step 1: Apply Singleton Base**
Refactor the 4 core managers to inherit from `Singleton<T>`.

**Step 2: Commit**
```bash
git commit -am "refactor: standardize all managers with Singleton base"
```

---

### Task 3: Implement SaveLoadManager

**Files:**
- Create: `Assets/_Project/Scripts/Managers/SaveLoadManager.cs`

**Step 1: Implement Persistence Logic**
Handle file I/O and JSON conversion. Add a helper to get the Current Timestamp in Ticks.

**Step 2: Commit**
```bash
git add Assets/_Project/Scripts/Managers/SaveLoadManager.cs
git commit -m "feat: implement SaveLoadManager"
```

---

### Task 4: Implement GameManager & Data Injection

**Files:**
- Create: `Assets/_Project/Scripts/Managers/GameManager.cs`

**Step 1: Initialize Systems**
On Load, push data to `Economy`, `Storage`, and `Level` systems.

**Step 2: World State Restoration**
Find all `CropTileView`s in the scene and map them to saved `TileData`.

**Step 3: Offline Growth Logic**
Calculate `ElapsedSeconds = Now - savedTimestamp`. Tell `CropSystem` or tiles to advance their growth by this amount.

**Step 4: Commit**
```bash
git add Assets/_Project/Scripts/Managers/GameManager.cs
git commit -m "feat: implement GameManager with offline growth support"
```

---

### Task 5: Implement FarmCameraController

**Files:**
- Create: `Assets/_Project/Scripts/World/Camera/FarmCameraController.cs`

**Step 1: Pan/Zoom/Clamp Logic**
Implement straightforward drag and ortho scaling.

**Step 2: Commit**
```bash
git add Assets/_Project/Scripts/World/Camera/FarmCameraController.cs
git commit -m "feat: implement FarmCameraController"
```

---

### Task 6: Final Integration & Action Saves

**Files:**
- Modify: `Assets/_Project/Scripts/UI/Common/PopupManager.cs`
- Modify: `Assets/_Project/Scripts/Gameplay/Progression/LevelSystem.cs`

**Step 1: Wire Triggers**
Ensure Every Popup close and Level Up triggers a save.

**Step 2: Verification**
Verify persistent gameplay after a restart.

**Step 3: Commit**
```bash
git commit -am "feat: finish framework integration"
```
