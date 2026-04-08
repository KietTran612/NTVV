# Design Document: Atomic UI Asset Standardization & 9-Slicing

**Date**: 2026-04-08
**Topic**: UI Asset Validation
**Status**: APPROVED by User

## 1. Overview
This design aims to standardize 5 newly imported UI assets into the NTVV Atomic HUD workflow. The goal is to ensure visual consistency, automated identification, and perfect scaling (9-slicing) across all screen resolutions without manual intervention.

## 2. Target Assets
| Original Name | New Standardized Name | Type | Scaling Method |
| :--- | :--- | :--- | :--- |
| `Blue Button.png` | `bg_Button_Blue_Atomic.png` | Background (L0) | 9-Slicing (Capsule) |
| `Green Button.png` | `bg_Button_Green_Atomic.png` | Background (L0) | 9-Slicing (Capsule) |
| `Parchment.png` | `bg_Banner_Parchment_Atomic.png` | Header (L0) | 9-Slicing (Scroll) |
| `Gold Coin.png` | `icon_Gold_Atomic.png` | Icon (L0) | None (Fixed Ratio) |
| `Lightning.png` | `icon_Energy_Atomic.png` | Icon (L0) | None (Fixed Ratio) |
| `[NEW]` | `bg_Panel_Main_Atomic.png` | Background (L0) | 9-Slicing (Rounded Box) |

## 3. Metadata Standards
- **Folder**: `Assets/_Project/Art/Sprites/UI/`
- **Texture Type**: `Sprite (2D and UI)`
- **Mesh Type**: `Full Rect` (Crucial for AI-rounded corners)
- **Pixels Per Unit (PPU)**: `100` (Standardizing based on current import)
- **Alpha is Transparency**: `True`

## 4. 9-Slicing Logic (Approach A: Automated Calculation)
To ensure the rounded caps of buttons and handles of the parchment never stretch:

### A. Capsule Buttons (Blue/Green)
- **Logic**: A capsule is composed of two half-circles on the ends.
- **Border Calculation**: 
    - `Left` = `Height / 2`
    - `Right` = `Height / 2`
    - `Top` = `0` (or 2-5 pixels for padding)
    - `Bottom` = `0`

### B. Parchment (Scroll)
- **Logic**: Preserve the left and right "handles" where the paper rolls.
- **Border Calculation**:
    - `Left` = ~20% of Width (Targeting the roll part)
    - `Right` = ~20% of Width
    - `Top` = 10% of Height
    - `Bottom` = 10% of Height

## 5. Execution Plan
1. **Move & Rename**: Use `AssetDatabase.MoveAsset` or `assets-move` to preserve GUIDs if possible.
2. **Apply Importer Settings**: Modify `.meta` or use `TextureImporter` API.
3. **Set Borders**: Inject border values into the `SpriteMetaData`.
4. **Refresh**: Force `AssetDatabase.Refresh()` to apply changes.
