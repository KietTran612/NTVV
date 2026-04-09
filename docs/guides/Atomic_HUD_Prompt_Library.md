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

- **Warning Button (Red)**:
  *Target Filename: `bg_Button_Red_Atomic.png`*
  `Blank capsule-shaped UI button background, 2:1 aspect ratio, Straight Front View (Orthographic), Kawaii aesthetic 3D soft-touch style, Vibrant candy red satin plastic, Rich internal volume with subtle diffused highlights and soft ambient shadows, Transparent background, No icons or text, --ar 2:1`

- **Caring/Soft Button (Purple)**:
  *Target Filename: `bg_Button_Purple_Atomic.png`*
  `Blank capsule-shaped UI button background, 2:1 aspect ratio, Straight Front View (Orthographic), Kawaii aesthetic 3D soft-touch style, Gentle lavender-purple satin plastic, Smooth internal volume with soft diffused lighting, Transparent background, No icons or text, --ar 2:1`

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

- **Experience Star (XP)**:
  *Target Filename: `icon_XP_Atomic.png`*
  `Pure 3D stylized star icon for experience points, 3D Isometric View, Kawaii magic aesthetic, Vibrant purple and magenta glossy thick plastic with internal glow, smooth polished surface, soft diffused highlights, Transparent background, --ar 1:1`

- **Storage Chest**:
  *Target Filename: `icon_Storage_Atomic.png`*
  `Pure 3D stylized wooden chest icon for inventory, 3D Isometric View, Kawaii farm aesthetic, Smooth polished wood with friendly rounded corners and golden metallic bands, satin finish lighting, Transparent background, --ar 1:1`

- **Gem (Premium Currency)**:
  *Target Filename: `icon_Gem_Atomic.png`*
  `Pure 3D faceted gem icon, 3D Isometric View, Kawaii aesthetic, Sparkling cyan glossy thick plastic material with internal refractions, soft diffused highlights, Transparent background, --ar 1:1`

- **Carrot (Crop)**:
  *Target Filename: `icon_Carrot_Atomic.png`*
  `Pure 3D stylized carrot icon, 3D Isometric View, Kawaii vegetable aesthetic, Vibrant orange soft-touch material with bright green leaves, Transparent background, --ar 1:1`

- **Watering Can (Tool)**:
  *Target Filename: `icon_WateringCan_Atomic.png`*
  `Pure 3D stylized watering can, 3D Isometric View, Kawaii garden tool aesthetic, Bright sky blue glossy plastic, soft rounded handle, Transparent background, --ar 1:1`

#### C. Crop Specialized: Carrot (crop_01) - `[ĐÃ SẢN XUẤT & KIỂM CHỨNG]`
- **Storage**: `Assets/_Project/Art/Sprites/UI/Icons/Crops/Carrot/`
- **Carrot Seed Packet**:
  *Target Filename: `icon_Seeds_Carrot_Atomic.png`*
  `Pure 3D cute paper seed packet for carrots, 3D Isometric View, Kawaii farm aesthetic, Soft-touch matte paper material with a cartoon carrot illustration, bright colors, Transparent background, --ar 1:1`

- **Carrot Stage 0 (Newly Planted)**:
  *Target Filename: `icon_Carrot_Stage0_Atomic.png`*
  `Pure 3D newly planted carrot seeds in soil, 3D Isometric View, Kawaii farm aesthetic, A small mound of soft brown soil with a tiny green tip barely visible, glossy plastic finish, Transparent background, --ar 1:1`

- **Carrot Stage 1 (Sprout)**:
  *Target Filename: `icon_Carrot_Stage1_Atomic.png`*
  `Pure 3D small green carrot sprout, 3D Isometric View, Kawaii farm aesthetic, Two tiny vibrant green leaves poking out of soft brown soil, glossy soft plastic finish, Transparent background, --ar 1:1`

- **Carrot Stage 2 (Mid-Growth)**:
  *Target Filename: `icon_Carrot_Stage2_Atomic.png`*
  `Pure 3D growing carrot plant, 3D Isometric View, Kawaii farm aesthetic, Lush green leaves, a hint of vibrant orange carrot crown visible above soft brown soil, glossy plastic finish, Transparent background, --ar 1:1`

- **Carrot Stage 3 (Harvest)**:
  *Target Filename: `icon_Carrot_Stage3_Atomic.png`*
  `Pure 3D whole vibrant orange carrot with leaves, 3D Isometric View, Kawaii vegetable aesthetic, Smooth polished surface, bright green lush tops, glossy soft-touch plastic, Transparent background, --ar 1:1`

#### D. Animal Specialized: Chicken (animal_01) - `[ĐÃ SẢN XUẤT & KIỂM CHỨNG]`
- **Storage**: `Assets/_Project/Art/Sprites/UI/Icons/Animals/Chicken/`
- **Baby Chick (Stage 1)**:
  *Target Filename: `icon_Chicken_Stage1_Atomic.png`*
  `Pure 3D round fluffy baby chick, 3D Isometric View, Kawaii animal aesthetic, Soft yellow vibrant plastic, small orange beak, tiny feet, friendly eyes, glossy finish, Transparent background, --ar 1:1`

- **Juvenile Chicken (Stage 2)**:
  *Target Filename: `icon_Chicken_Stage2_Atomic.png`*
  `Pure 3D young growing chicken, 3D Isometric View, Kawaii animal aesthetic, Half-grown size between chick and hen, soft white and yellow glossy plastic, developing small red comb, friendly rounded shape, Transparent background, --ar 1:1`

- **Adult Chicken (Stage 3)**:
  *Target Filename: `icon_Chicken_Stage3_Atomic.png`*
  `Pure 3D mature hen chicken, 3D Isometric View, Kawaii animal aesthetic, Soft white and pale yellow glossy plastic, small red comb, rounded friendly volume, Transparent background, --ar 1:1`

- **Egg (Product)**:
  *Target Filename: `icon_Egg_Atomic.png`*
  `Pure 3D smooth oval bird egg, 3D Isometric View, Kawaii aesthetic, Pale pinkish-white glossy plastic material, gentle highlights, Transparent background, --ar 1:1`

- **Worm (Feed)**:
  *Target Filename: `icon_Feed_Worm_Atomic.png`*
  `Pure 3D stylized green worm, 3D Isometric View, Kawaii aesthetic, Vibrant lime green glossy soft plastic, friendly rounded segments, Transparent background, --ar 1:1`

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
