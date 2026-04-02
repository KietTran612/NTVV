# UI System Implementation Design (Hybrid Approach)

## Context
The goal is to implement the UI system for the NTVV farm game based on the `pencil-new.pen` design file. We will use a hybrid approach combining uGUI (for gameplay-related popups) and UI Toolkit (for standardized system screens and HUD).

## Architecture
We will maintain two distinct UI layers:

### 1. uGUI Layer (Gameplay UI)
- **Component**: Unity Canvas (Screen Space - Camera or World Space).
- **Usage**: 
  - `ContextActionPanel`: Popups triggered by interacting with farm tiles.
  - `In-World Labels`: Floating text or indicators above animals/crops.
- **Rationale**: uGUI is better integrated with the 3D/2D world space and existing gameplay scripts.

### 2. UI Toolkit Layer (System UI)
- **Component**: `UIDocument`.
- **Usage**:
  - `TopHUD`: Permanent resource display.
  - `BottomNav`: Navigation tabs.
  - `Overlay Screens`: Storage, Shop, System Alerts, Animal Details.
- **Rationale**: UI Toolkit's CSS-like styling (USS) and efficient layout engine are ideal for complex, state-driven system menus.

## Implementation Details

### Design Tokens (`farm-theme.uss`)
We will extract the following tokens from the "Design System Atoms" frame in Pencil:
- **Colors**: `$farm-green`, `$deep-leaf-green`, `$cream-white`, `$berry-pink`, `$sky-blue`, `$warning`, `$danger`.
- **Typography**: Inter (primary font).
- **Radius**: `24px` for modals, `20px` for chips.

### Phase 1: Foundation & HUD
1. **Asset Export**: Export sprites for HUD chips, icons, and buttons from the Design System Atoms frame.
2. **Global Styling**: Create `farm-theme.uss` with variables for colors and common styles.
3. **TopHUD**:
   - Create `TopHUD.uxml`.
   - Implement `HUDController.cs` (replacing/refactoring `HUDView.cs`) to bind with `PlayerManager` and `InventoryManager`.
4. **BottomNav**:
   - Create `BottomNav.uxml`.
   - Implement navigation controller to switch between screen states.
5. **UIManager Bridge**:
   - Update `UIManager.cs` to handle opening/closing both uGUI panels and UI Toolkit screens.

### Phase 2: Screens & Popups
1. **Storage & Shop**: Full-screen modals built with UI Toolkit.
2. **System Alerts**: Overlay notification system.
3. **Animal Details**: Information panels for livestock.

## Verification Plan
- **Mockup Comparison**: Screenshots of the implemented UI in Unity compared with Pencil screenshots.
- **Data Binding**: Verify Gold/XP/Storage updates in the HUD when global manager values change.
- **Interaction**: Verify buttons in `BottomNav` and `TopHUD` trigger the expected UI states.
