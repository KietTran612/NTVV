# Project Design: Folder Structure, Naming Convention & Scripts

**Date:** 2026-04-01
**Topic:** Foundation for Unity Farm Game (NTVV)
**Status:** Approved

## 1. Overview
This design establishes the technical foundation for a 2.5D Isometric Casual Farm Game built in Unity. The architecture follows a **Data-Driven Managed** approach where game logic is decoupled from data using ScriptableObjects.

## 2. Technical Stack
- **Rendering:** Universal Render Pipeline (URP).
- **Input:** New Input System (Actions-based).
- **Architecture:** Managed Singleton Managers with ScriptableObject-driven entities.

## 3. Folder Structure
All custom assets are contained within `Assets/_Project/` to maintain a clean workspace.

```text
Assets/
├── _Project/
│   ├── Animations/
│   ├── Art/
│   │   ├── Sprites/
│   │   │   ├── Animals/
│   │   │   ├── Crops/
│   │   │   ├── Environment/
│   │   │   └── UI/
│   │   └── Materials/
│   ├── Prefabs/
│   │   ├── World/
│   │   └── UI/
│   ├── Scenes/
│   ├── ScriptableObjects/
│   │   ├── Crops/
│   │   ├── Animals/
│   │   ├── Items/
│   │   └── Player/
│   ├── Scripts/
│   │   ├── Core/
│   │   ├── Data/
│   │   ├── World/
│   │   ├── UI/
│   │   └── Utilities/
│   └── Settings/
```

## 4. Naming Convention

| Category | Style | Prefix/Suffix |
| :--- | :--- | :--- |
| **Folders** | PascalCase | N/A |
| **Scripts / Classes** | PascalCase | N/A |
| **Namespaces** | PascalCase | `NTVV.Core`, `NTVV.Data`, `NTVV.UI` |
| **Assets (Sprites)** | PascalCase | `Spr_` (e.g., `Spr_Crop_Carrot_Stage1`) |
| **Assets (Prefabs)** | PascalCase | `Pre_` (e.g., `Pre_World_CropTile`) |
| **Assets (SOs)** | PascalCase | `SO_` (e.g., `SO_Crop_Carrot`) |
| **Private Fields** | camelCase | `_` (e.g., `_currentGold`) |
| **UI Components** | camelCase | `btn_`, `txt_`, `img_` |

## 5. Core Script File List

### Data (ScriptableObjects)
- `CropData.cs`: Growth time, yield, HP drain, perfect window.
- `AnimalData.cs`: Growth stages, food, sale price, etc.
- `ItemData.cs`: Base SO for all items (Seeds, Crops, Animals, Food).
- `PlayerLevelData.cs`: Level milestones and unlocks.

### Core Systems
- `GameManager.cs`: Singleton, overall game state.
- `TimeManager.cs`: Handles game time calculations.
- `PlayerManager.cs`: Manages Gold, Level, XP.
- `InventoryManager.cs`: Manages Warehouse logic and limits.
- `ShopManager.cs`: Handles seed and food purchasing.

### World Entities
- `CropTile.cs`: Logic for in-world plot; growth/care state machine.
- `AnimalPen.cs`: Manages animals inside a barn/pen.
- `AnimalEntity.cs`: Individual animal behavior and growth logic.
- `WorldObjectPicker.cs`: Handles input for tapping world objects.

### UI Views
- `UIManager.cs`: Manages screens and popups.
- `HUDView.cs`: Top bar display (Gold, Level, Storage).
- `ContextActionPanel.cs`: Context-sensitive popup (Plant/Care/Harvest).
- `StoragePopup.cs`: Inventory UI and filters.
- `ShopPopup.cs`: UI for buying seeds/food.
- `AnimalDetailPopup.cs`: UI for managing specific animals.
