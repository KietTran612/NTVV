# UI Prompt Spec
Date: 2026-04-17

## Group Rules
- Domain: `UI`
- Categories: `Icon`
- One sprite per file
- Center pivot
- English-only naming/prompt/setup

## Naming Pattern
- `UI_Icon_Nav_[Entity]_Default`
- `UI_Icon_Common_[Entity]_Default`

## Missing UI Assets (3)

<details>
<summary>UI_Icon_Nav_Shop_Default — bottom nav shop icon</summary>

- File: `UI_Icon_Nav_Shop_Default.png`
- Target: `Assets/_Project/Art/Sprites/UI/Icons/Nav/`
- Export size: 64x64

### ChatGPT / DALL-E (EN)
Prompt: "Cute cartoon shop icon for mobile farm game bottom nav, simple storefront silhouette, readable at 64x64, transparent background, clean outline"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "cute cartoon shop nav icon, simple storefront silhouette, readable small size, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry, overly detailed"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512 then downscale to 64

### VN note
Icon Shop cho nav dưới.

</details>

<details>
<summary>UI_Icon_Nav_Event_Default — bottom nav event icon</summary>

- File: `UI_Icon_Nav_Event_Default.png`
- Target: `Assets/_Project/Art/Sprites/UI/Icons/Nav/`
- Export size: 64x64

### ChatGPT / DALL-E (EN)
Prompt: "Cute cartoon event icon for mobile farm game bottom nav, festive star badge symbol, readable at 64x64, transparent background, clean outline"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "cute cartoon event nav icon, festive star badge, readable small size, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry, overly detailed"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512 then downscale to 64

### VN note
Icon Event cho nav dưới.

</details>

<details>
<summary>UI_Icon_Common_Grass_Default — inventory/feed grass icon</summary>

- File: `UI_Icon_Common_Grass_Default.png`
- Target: `Assets/_Project/Art/Sprites/UI/Icons/Common/`
- Export size: 128x128

### ChatGPT / DALL-E (EN)
Prompt: "Cute cartoon grass bundle icon for farm inventory and feed UI, simple readable silhouette, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "cute cartoon grass bundle icon, inventory ui, transparent background, readable"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512 then downscale to 128

### VN note
Icon cỏ cho kho và feed.

</details>
