# Atomic HUD Prompt Library

This document serves as the official repository for high-fidelity UI asset generation prompts used in the NTVV project. It ensures that any AI-generated graphics maintain strict adherence to the project's **Atomic HUD Rebirth** standards.

## 1. Structural Prompting Formula (The NTVV Standard)
All asset generation MUST strictly adhere to this structure to ensure consistency across different models and generations:

> `[asset type] for [use case], [aspect ratio], [view], [style anchors], [material], [background], [must avoid], [quality target]`

---

## 2. Core HUD Assets (Atomic Layer)

### A. Background Layers (Frontal Orthographic)
*Purpose: These assets provide the base shapes for buttons and panels. Must be flat-facing to support 9-slicing.*

- **Green Button (Farm Action)**:
  *Target Filename: `bg_Button_Green_Atomic.png`*
  `Blank capsule-shaped UI button background, 2:1 width-to-height aspect ratio, Straight Front View (Orthographic), Kawaii aesthetic 3D soft-touch style, Vibrant lime green satin plastic, Rich internal volume with soft ambient shadows and subtle diffused highlights, Minimal subtle outer stroke, Smooth rounded depth, Transparent background, No icons or text / no perspective tilt, High resolution, --ar 2:1`

- **Blue Button (Energy/Gold Info)**:
  *Target Filename: `bg_Button_Blue_Atomic.png`*
  `Blank capsule-shaped UI button background, 2:1 width-to-height aspect ratio, Straight Front View (Orthographic), Kawaii professional game UI, Bright sky blue satin finish plastic, Sophisticated internal bevel with very soft diffused lighting, Thin secondary outer outline, Transparent background, No icons or text, Sharp quality with rich matte-like color depth, --ar 2:1`

- **Parchment Banner (Section Header)**:
  *Target Filename: `bg_Banner_Parchment_Atomic.png`*
  `Blank horizontal scroll UI banner for section headers, 4:1 width-to-height aspect ratio, Straight Front View (Eye-level), Cartoonish Kawaii paper style, Aged yellow parchment without thick outlines, Clean borderless cutout, Transparent background, No text or icons / no slanted alignment, --ar 4:1`

- **Main Panel Background (Popup Frame)**:
  *Target Filename: `bg_Panel_Main_Atomic.png`*
  `Blank rounded rectangular UI panel background, 2:1 width-to-height aspect ratio, Straight Front View (Orthographic), Kawaii professional game UI, Satin cream soft-touch plastic, Gentle internal ambient occlusion shadows, Very thin consistent outer border, Transparent background, No text or icons / no perspective tilt, Clean 9-sliceable edges, --ar 2:1`

### B. Content Layers (Data Displays)
*Purpose: These assets provide neutral backgrounds for resource counters and informational chips. Must be flat and non-interactive.*

- **Resource Chip (Gold/Energy Display)**:
  *Target Filename: `bg_Chip_Resource_Atomic.png`*
  `Minimalist blank horizontal capsule UI container, 3:1 width-to-height aspect ratio, Straight Front View (Orthographic), Modern Kawaii game UI, Slim and flat cream satin plastic, Subtle inner glow and light diffused shadow for depth, Micro-thin outer stroke, Transparent background, No thick borders / no heavy bevels, --ar 3:1`

### C. Icon Layers (3D Isometric)
*Purpose: Functional icons that sit on top of background layers. Must have depth and volume.*

- **Gold Coin**:
  *Target Filename: `icon_Gold_Atomic.png`*
  `Pure 3D gold coin icon for currency display, 3D Isometric View (30-degree angle), Charming Kawaii game style, Vibrant yellow-gold metallic finish with bright glints, Transparent background, No frames or buttons / no flat surfaces, High detail volume and depth, --ar 1:1`

- **Energy Bolt**:
  *Target Filename: `icon_Energy_Atomic.png`*
  `Pure 3D lightning bolt icon for energy resource, 3D Isometric View, Kawaii electric aesthetic, Glossy yellow thick plastic with white highlights, Transparent background, No backgrounds or borders / no text, Vibrant 3D volume, --ar 1:1`

- **Plant Sprout**:
  *Target Filename: `icon_Sprout_Atomic.png`*
  `Pure 3D green sprout icon for farm crops, 3D Isometric View, Kawaii natural aesthetic, Vibrant green soft plastic with dew drop highlights, Transparent background, No pots or soil / no flat lighting, Sharp 4K texture, --ar 1:1`

- **Red Apple**:
  *Target Filename: `icon_Apple_Atomic.png`*
  `Pure 3D red apple icon for farm crops, 1:1 aspect ratio, 3D Isometric View (30-degree angle), Charming Kawaii game style, Vibrant ruby red satin finish with a bright green leaf glint, Transparent background, No backgrounds or borders / no flat surfaces, High detail volume and depth, --ar 1:1`

- **Golden Wheat**:
  *Target Filename: `icon_Wheat_Atomic.png`*
  `Pure 3D golden wheat stalk icon for farm crops, 1:1 aspect ratio, 3D Isometric View, Charming Kawaii game style, Warm golden-yellow thick plastic with soft highlights, Transparent background, No soil or pots / no flat lighting, Vibrant 3D volume and sharp texture, --ar 1:1`

---

## 3. Mandatory Production Guidelines

> [!IMPORTANT]
> **Transparency Strategy**: 
> Primarily request **Transparent background**. If the AI model has difficulty with clean alpha edges, fallback to a **Solid Cyan** or **Magenta** background (Chroma Key) to be removed during post-processing. Ensure the background color is distinct from the asset's highlights.

> [!CAUTION]
> **Subtle Borders & Diffused Lighting**: 
> For all background assets, avoid "thick" outer strokes and "hard" specular highlights. Use "Satin finish", "Soft-touch plastic", and "Subtle diffused highlights" to maintain a modern, elegant 3D look that isn't overly glossy. This ensures the UI feels premium and easy on the eyes.

> [!IMPORTANT]
> **PPU Multiplier Implementation Standards**:
> - **Buttons & Resource Chips**: Set to `5` in Unity Image Component.
> - **Header Banners (Horizontal)**: Set to `2.5`.
> - **Main Panels (Large Popups)**: Set to `1.5`.
> - **Icons (Standalone Props)**: Set to `1`.

---

## 4. Quality Control Checklist
- [ ] **Perspective**: Is it 0-degree frontal (panels) or 45-degree isometric (icons)?
- [ ] **Material**: Does it have the signature "Glossy/Candy" highlights?
- [ ] **Background**: Is the chroma key color clearly separated from the asset color?
- [ ] **Completeness**: Are the edges clean and not cut off by the frame?
