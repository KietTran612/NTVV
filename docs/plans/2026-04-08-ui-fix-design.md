# UI Asset Distortion Fix and Alignment Design

## Overview
Addressing the visual distortion ("méo mó") of UI elements caused by large 9-slice borders on high-resolution assets scaled down to small GameObjects. Additionally, improving UI alignment for HUD and Popups.

## Proposed Design

### 1. Fix 9-Slicing Distortion
We will use a combination of Metadata adjustment and Component property adjustment:
- **Sprite Border Reduction**: Adjust `Sprite Border` in the Sprite Editor from `150px` down to `40px` for button assets. This ensures the "stretchable" area of the 9-slice is large enough even at small UI scales.
- **Pixels Per Unit (PPU) Multiplier**: Set the `pixelsPerUnitMultiplier` on the `Image` component to `2.5`. This scales the corners down visually so they match the original design's proportion without crushing the asset.

### 2. UI Alignment & Balancing
- **HUD_TopBar**:
    - Change anchors of `Gold_Chip` and `Energy_Chip` to `Top-Center`.
    - Set symmetric positions (e.g., $X = -200, 200$) for a balanced look.
- **HUD_BottomNav**:
    - Center the navigation container and ensure buttons are evenly spaced.
- **Shop_Popup**:
    - Center the `Main_Panel`.
    - Align `Header_Banner` and `Btn_Close` precisely to the panel's boundaries.

## Target Assets
- `bg_Button_Blue_Atomic`
- `bg_Button_Green_Atomic`
- `bg_Banner_Parchment_Atomic`
- `bg_Panel_Main_Atomic`

## Target GameObjects
- `Gold_Chip`
- `Energy_Chip`
- `Btn_Shop`, `Btn_Farm`, `Btn_Storage`
- `Main_Panel`, `Header_Banner`, `Btn_Close`
