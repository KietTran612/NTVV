# Design: Atomic UI Components (InventorySlot & ShopEntry_Seed)

## Overview
This design outlines the Functional and Decorator Layers for two generic UI components: `InventorySlot` and `ShopEntry_Seed`. These components serve as the atomic building blocks containing no underlying logic but relying heavily on UI Prefab auto-wiring and MCP styling.

## 1. InventorySlot
### Architecture
- **Root**: `InventorySlot` (CanvasGroup, LayoutElement, RectTransform).
- **Decorator Layer**:
  - `bg_slot_frame`: A rounded rectangle (squircle) background acting as the container.
- **Functional Layer**:
  - `Item_Icon` (Image): The visual representation of the resource. Anchored center.
  - `Quantity_Label` (TMPro): The numeric amount. Anchored bottom-right. Dosis-Bold.

### Visual Styling (Placeholder)
- Background Color: Semi-transparent dark gray/brown (`#30201580`).
- Text Style: White with dark outline map (`Outline-Green` if full, default otherwise).

## 2. ShopEntry_Seed
### Architecture
- **Root**: `ShopEntry_Seed` (CanvasGroup, LayoutElement, RectTransform, LayoutGroup).
- **Decorator Layer**:
  - `bg_entry_card`: A solid rounded rectangle serving as the card back. 
- **Functional Layer**:
  - `Seed_Icon` (Image): Left-aligned item preview.
  - `Name_Label` (TMPro): Top-center aligned item name with `Dosis-ExtraBold`.
  - `PriceContainer`:
    - `Price_Icon` (Image): Small gold/currency icon.
    - `Price_Label` (TMPro): The numeric cost.
  - `Buy_Button`: A standardized `Button` component instance anchored to the right side containing its own internal `bg_` and `_Label` structure.

### Visual Styling (Placeholder)
- Background Color: Milk Chocolate (`#8D6E63`).
- Buy Button: Leaf Green (`#4CAF50`).
- Typography: Dosis SDF with drop shadows for readability.

## Scalability
These are designed to be instanced dynamically by `InventoryPanelController` and `ShopPanelController`. The strict naming convention (e.g., `_Icon`, `_Label`, `_Button`) ensures auto-wiring during their instantiation or initialization phase via central PrefabAssembler scripts.
