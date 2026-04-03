# NTVV - UI Design Doc: Phase 1 (Managed & Data-driven)

This document outlines the architecture and design for the Phase 1 UI implementation, focusing on a managed loading pattern, dynamic theming, and world-space contextual interactions.

## 🏗 Architecture: Managed UI Provider

To ensure the project is ready for **Addressables** in the future while staying lightweight for now using **Resources**, we implement a provider-based pattern.

### 1. IUIAssetProvider (Interface)
- `GameObject LoadPrefab(string key)`: Retrieves a UI prefab by its logical name (e.g., "Shop", "Storage").
- `UIStyleData GetCurrentStyle()`: Returns the active theme data.

### 2. ResourcesUIProvider (Implementation)
- Maps keys to paths in `Assets/_Project/Resources/UI/`.
- Holds a reference to the active `UIStyleData` ScriptableObject.

### 3. PopupManager (Singleton)
- **Role**: Manages the screen stack and layer assignment.
- **Workflow**:
  1. Requests a prefab from the `IUIAssetProvider`.
  2. Spawns it under the correct `Canvas` layer (`HUD`, `Popup`, or `System`).
  3. Handles "Close" logic by destroying the instance (managed cleanup).

---

## 🎨 Data-driven Theming

Instead of hardcoding colors in the inspector of every button, we centralize visuals.

### 1. UIStyleData (ScriptableObject)
- **PrimaryActionColor**: Orange (#FFA500) for progress-oriented actions (Plant, Harvest, Buy).
- **SecondaryActionColor**: Green (#4CAF50) for care-oriented actions (Water, Feed, Cure).
- **Text Styles**: Centralized TMP_FontAssets and sizes.
- **Sprites**: Grayscale placeholders (Rounded Rects) that are tinted via `Image.color`.

### 2. UIStyleApplier Component
- A small helper script attached to UI elements.
- Subscribes to the theme and applies settings automatically, ensuring consistency across all screens.

---

## 🛠 Interaction: World-Space Context Action

The `ContextActionPanel` provides intuitive interaction with world objects (Crops, Animals).

- **Positioning**: Calculated via `Camera.main.WorldToScreenPoint(target.position)`.
- **Offset**: Dynamic offset to ensure the panel appears "near" the object without overlapping it.
- **Responsive Layout**: Uses `Horizontal/Vertical Layout Groups` to adapt to the number of available actions (e.g., just "Buy" vs "Feed & Sell").

---

## 🎬 Animation & Feel

- **Transitions**: Native `Color Tint` for hover/click states.
- **Micro-animations**: Leverages `UIAnimationHelper.cs` for scale-based "Bounce" effects on click to provide "Juicy" feedback even with placeholder assets.

---

## 🔒 Error Handling
- **Missing Asset**: Logs a `Warning` but fails gracefully (doesn't hang the game).
- **Layering Check**: Ensures popups always spawn on top of HUD elements.
- **Input Blocker**: Automatic background dimming via `UI_Modal_Base` to prevent clicking through modals.
