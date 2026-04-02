# Design Doc: Core Operational Framework (Milestone 1-2)

| Property | Detail |
| --- | --- |
| **Topic** | Core Framework (GameManager, Save/Load, Camera) |
| **Status** | Approved by User (Drafting) |
| **Created** | 2026-04-02 |

## 1. GameManager (The Brain)
Consistent singleton for overall game state coordination.

### Responsibilities
- Bootstrapping core systems.
- Handling Scene transitions (Boot -> Gameplay).
- Managing global GameState (`Loading`, `Playing`, `Paused`).
- Coordinating between `SaveLoadManager` and the gameplay systems.

### Implementation
- Inherits from `Singleton<GameManager>`.
- Uses a simple state machine or Enum for GameState.

## 2. Persistence (Save/Load System)
Hybrid persistence model using JSON for data integrity and flexibility.

### SaveData Schema
- `PlayerData`: Gold, Current XP, Current Level.
- `InventoryData`: List of `(itemID, quantity)`.
- `WorldState`: 
    - List of `TileData`: `(tileID, cropID, health, growthTimestamp, isWatered, hasWeeds, hasBugs)`.
    - List of `AnimalData`: `(penID, animalID, ageTimestamp, hungerStatus)`.

### Checkpoints (Hybrid Strategy)
- **UI Interaction**: Save when a main UI Panel (Shop/Storage) is closed.
- **Milestones**: Save on Level-Up or major tutorial completion.
- **Engine Events**: Save on `OnApplicationPause` (Mobile reliability) and `OnApplicationQuit`.

## 3. World Camera (FarmCameraController)
Direct-control camera for 2D/Isometric farm navigation.

### Pan logic
- **Method**: Direct Drag.
- **Behavior**: Mouse Delta added to position. No momentum (v1 requirement).

### Zoom logic
- **Method**: Orthographic Size manipulation.
- **Constraints**: Min/Max zoom values to preserve sprite pixel density.

### Constraints
- **Clamping**: Bounding box calculated based on the tile grid size.

## 4. Integration
1. `GameManager` starts and requests `SaveLoadManager.Load()`.
2. `SaveLoadManager` reads `save.json` and populates a data object.
3. `GameManager` distributes this object's fields to `EconomySystem`, `StorageSystem`, and `LevelSystem`.
4. The Scene views populate based on the loaded `WorldState`.
