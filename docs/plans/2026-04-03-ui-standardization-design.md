# Technical Design Doc: UI Standardization Skill

**Topic**: Standardized Assembly, Wiring, and Styling of Unity Prefabs.
**Date**: 2026-04-03
**Status**: Validated

## Overview
This design establishes a set of rules for the AI assistant and the project to ensure that UI prefabs are built with a predictable, high-quality structure that remains functional across hierarchy changes.

## 1. Decision Matrix: Controller Inclusion
- **Rule**: Add a `Controller` (C# Script) to a prefab only if:
    - It contains dynamic text (e.g., `Price`, `Quantity`).
    - It contains dynamic icons (e.g., `ItemIcon`).
    - It handles interaction (`OnClick` events).
    - It is a reusable template used in lists (`ShopEntry`, `InventorySlot`).
- **Otherwise**: Use standard uGUI components and manage from the parent.

## 2. Naming Standards (Suffix-Based)
All child objects serving as data display or interaction points MUST use the following suffixes to enable **Recursive Auto-Wiring**:
- `_Label`: For `TMP_Text` components.
- `_Icon`: For `Image` components intended for sprites.
- `_Button`: For `Button` components.
- `_Fill`: For `Image` components using `FillAmount` (Progress Bars).
- `_Content`: For `RectTransform` containers for layout (e.g., Horizontal/Vertical Layout Groups).

## 3. Deep Component Resolution
The skill provides a recursive search algorithm to find signed components:
1. Start at the root of the prefab.
2. Traverse children depth-first.
3. Match name suffixes.
4. Attempt to link to the controller's `[SerializeField]` fields using `SerializedObject` in the Editor tool.

## 4. Visual & Styling Consistency
- **Rule**: Every UI prefab root MUST include a `UIStyleApplier` component.
- **Verification**:
    - Ensure font assets are NOT the default "Liberation Sans".
    - Ensure icons are using the project's color palette (no pure white/black unless intentional).

## 5. Event Binding
- **Standard**: Register `button.onClick.AddListener()` in the Controller's `Initialize()` or `OnEnable()` methods instead of manual Unity Event wiring in the inspector.
