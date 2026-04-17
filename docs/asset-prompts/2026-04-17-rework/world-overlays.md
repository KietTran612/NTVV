# World Overlays Prompt Spec
Date: 2026-04-17

## Group Rules
- Domain: `World`
- Category: `Overlay` or `Tile`
- Size: 512x512
- Transparent background required
- Keep visual center aligned to tile footprint
- One PNG per state

## Assets

<details>
<summary>World_Tile_Soil_Base_Default — empty soil tile base</summary>

### File
- Name: `World_Tile_Soil_Base_Default.png`
- Target: `Assets/_Project/Art/Sprites/World/`

### ChatGPT / DALL-E (EN)
- Prompt: "Isometric 2.5D farm soil tile base, empty tilled dirt, cute cartoon style, warm brown tones, clean outline, transparent background, centered composition, no text, no humans, game-ready sprite"
- Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
- Positive: "isometric 2.5d farm soil tile, empty tilled dirt, cute cartoon, warm brown, clean outline, transparent background, game sprite"
- Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
- Checkpoint: dreamshaper_8 / anything-v5
- Sampler: DPM++ 2M Karras
- Steps: 28
- CFG: 7
- Size: 512x512

### VN note
- Nền ô đất trống, luôn tách riêng để stack với cây/cỏ/sâu.

</details>

<details>
<summary>World_Overlay_Tile_Weed_On — weed status overlay</summary>

### File
- Name: `World_Overlay_Tile_Weed_On.png`
- Target: `Assets/_Project/Art/Sprites/World/`

### ChatGPT / DALL-E (EN)
- Prompt: "Isometric 2.5D weed overlay for farm tile, small green weeds clustered near ground contact area, cute cartoon style, transparent background, no text"
- Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
- Positive: "isometric weed overlay, small green weeds, farm tile status indicator, cute cartoon, transparent background"
- Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
- Checkpoint: dreamshaper_8 / anything-v5
- Sampler: DPM++ 2M Karras
- Steps: 28
- CFG: 7
- Size: 512x512

### VN note
- Layer cỏ dại bật/tắt độc lập.

</details>

<details>
<summary>World_Overlay_Tile_Pest_On — pest status overlay</summary>

### File
- Name: `World_Overlay_Tile_Pest_On.png`
- Target: `Assets/_Project/Art/Sprites/World/`

### ChatGPT / DALL-E (EN)
- Prompt: "Isometric 2.5D pest overlay for farm tile, tiny cute insects near crop base area, cartoon style, transparent background, clean outline"
- Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
- Positive: "isometric pest overlay, tiny insects, farm tile status icon, cute cartoon, transparent background"
- Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
- Checkpoint: dreamshaper_8 / anything-v5
- Sampler: DPM++ 2M Karras
- Steps: 28
- CFG: 7
- Size: 512x512

### VN note
- Layer sâu hại tách riêng để logic hiển thị sạch.

</details>

<details>
<summary>World_Overlay_Tile_WaterNeed_On — need-water status overlay</summary>

### File
- Name: `World_Overlay_Tile_WaterNeed_On.png`
- Target: `Assets/_Project/Art/Sprites/World/`

### ChatGPT / DALL-E (EN)
- Prompt: "Isometric 2.5D water-needed overlay for farm tile, blue droplet indicator near tile center, cute cartoon style, transparent background"
- Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
- Positive: "isometric water-needed overlay, blue droplet indicator, farm tile status, cute cartoon, transparent background"
- Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
- Checkpoint: dreamshaper_8 / anything-v5
- Sampler: DPM++ 2M Karras
- Steps: 28
- CFG: 7
- Size: 512x512

### VN note
- Layer thiếu nước, chỉ hiện trạng thái chăm sóc.

</details>
