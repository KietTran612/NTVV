# Crop Entities Prompt Spec
Date: 2026-04-17

## Group Rules
- Domain: `World`
- Category: `Crop`
- Size: 512x512
- Pivot target: bottom-center at ground contact
- One PNG for one crop state
- Do not combine multiple stages into one spritesheet image
- English-only naming/prompt/setup

## Naming Pattern
- `World_Crop_[Entity]_Body_Stage00`
- `World_Crop_[Entity]_Body_Stage01`
- `World_Crop_[Entity]_Body_Stage02`
- `World_Crop_[Entity]_Body_Stage03`
- `World_Crop_[Entity]_Body_Dead`

## Missing Crop Assets (31)

### Carrot (1)

<details>
<summary>World_Crop_Carrot_Body_Dead — carrot dead state</summary>

- File: `World_Crop_Carrot_Body_Dead.png`
- Target: `Assets/_Project/Art/Sprites/UI/Icons/Crops/Carrot/`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon dead carrot crop state, wilted leaves, non-graphic, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric dead carrot crop, wilted leaves, cute cartoon, transparent background"
Negative: "realistic gore, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Trạng thái cà rốt chết.

</details>

### Potato (5)

<details>
<summary>World_Crop_Potato_Body_Stage00 — potato seedling</summary>

- File: `World_Crop_Potato_Body_Stage00.png`
- Target: `Assets/_Project/Art/Sprites/UI/Icons/Crops/Potato/`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon potato seedling, very early growth stage, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric potato seedling, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Khoai tây mầm non.

</details>

<details>
<summary>World_Crop_Potato_Body_Stage01 — potato growing</summary>

- File: `World_Crop_Potato_Body_Stage01.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon potato growing stage, medium leafy plant, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric potato growing stage, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Khoai tây giai đoạn phát triển.

</details>

<details>
<summary>World_Crop_Potato_Body_Stage02 — potato near ripe</summary>

- File: `World_Crop_Potato_Body_Stage02.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon potato near ripe stage, fuller plant mass, hint of tuber form, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric potato near ripe stage, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Khoai tây gần chín.

</details>

<details>
<summary>World_Crop_Potato_Body_Stage03 — potato ripe</summary>

- File: `World_Crop_Potato_Body_Stage03.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon ripe potato crop, harvest-ready plant, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric ripe potato crop, harvest-ready, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Khoai tây thu hoạch.

</details>

<details>
<summary>World_Crop_Potato_Body_Dead — potato dead state</summary>

- File: `World_Crop_Potato_Body_Dead.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon dead potato crop, wilted dry leaves, non-graphic, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric dead potato crop, wilted leaves, cute cartoon, transparent background"
Negative: "realistic gore, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Khoai tây chết.

</details>

### Corn (5)

<details><summary>World_Crop_Corn_Body_Stage00 — corn seedling</summary>

- File: `World_Crop_Corn_Body_Stage00.png`
- Target: `Assets/_Project/Art/Sprites/UI/Icons/Crops/Corn/`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon corn seedling, tiny early sprout, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric corn seedling, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Ngô mầm nhỏ.

</details>

<details><summary>World_Crop_Corn_Body_Stage01 — corn growing</summary>

- File: `World_Crop_Corn_Body_Stage01.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon corn growing stage, medium green stalks, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric corn growing stage, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Ngô đang phát triển.

</details>

<details><summary>World_Crop_Corn_Body_Stage02 — corn near ripe</summary>

- File: `World_Crop_Corn_Body_Stage02.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon corn near ripe stage, taller stalk with emerging cob shape, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric corn near ripe stage, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Ngô gần chín.

</details>

<details><summary>World_Crop_Corn_Body_Stage03 — corn ripe</summary>

- File: `World_Crop_Corn_Body_Stage03.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon ripe corn crop, visible yellow cobs, harvest-ready, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric ripe corn crop with yellow cobs, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Ngô chín thu hoạch.

</details>

<details><summary>World_Crop_Corn_Body_Dead — corn dead state</summary>

- File: `World_Crop_Corn_Body_Dead.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon dead corn crop, dry bent stalk, non-graphic, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric dead corn crop, dry bent stalk, cute cartoon, transparent background"
Negative: "realistic gore, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Ngô chết.

</details>

### Wheat (5)

<details><summary>World_Crop_Wheat_Body_Stage00 — wheat seedling</summary>

- File: `World_Crop_Wheat_Body_Stage00.png`
- Target: `Assets/_Project/Art/Sprites/UI/Icons/Crops/Wheat/`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon wheat seedling, early green shoots, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric wheat seedling, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Lúa mì mầm non.

</details>

<details><summary>World_Crop_Wheat_Body_Stage01 — wheat growing</summary>

- File: `World_Crop_Wheat_Body_Stage01.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon wheat growing stage, thicker green stalks, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric wheat growing stage, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Lúa mì phát triển.

</details>

<details><summary>World_Crop_Wheat_Body_Stage02 — wheat near ripe</summary>

- File: `World_Crop_Wheat_Body_Stage02.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon wheat near ripe stage, visible forming wheat heads, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric wheat near ripe stage, visible wheat heads, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Lúa mì gần chín.

</details>

<details><summary>World_Crop_Wheat_Body_Stage03 — wheat ripe</summary>

- File: `World_Crop_Wheat_Body_Stage03.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon ripe wheat crop, golden wheat heads, harvest-ready, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric ripe wheat crop, golden heads, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Lúa mì chín vàng.

</details>

<details><summary>World_Crop_Wheat_Body_Dead — wheat dead state</summary>

- File: `World_Crop_Wheat_Body_Dead.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon dead wheat crop, dry bent stalks, non-graphic, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric dead wheat crop, dry bent stalks, cute cartoon, transparent background"
Negative: "realistic gore, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Lúa mì chết.

</details>

### Tomato (5)

<details><summary>World_Crop_Tomato_Body_Stage00 — tomato seedling</summary>

- File: `World_Crop_Tomato_Body_Stage00.png`
- Target: `Assets/_Project/Art/Sprites/UI/Icons/Crops/Tomato/`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon tomato seedling, tiny green sprout, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric tomato seedling, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Cà chua mầm non.

</details>

<details><summary>World_Crop_Tomato_Body_Stage01 — tomato growing</summary>

- File: `World_Crop_Tomato_Body_Stage01.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon tomato growing stage, medium green leaves, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric tomato growing stage, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Cà chua phát triển.

</details>

<details><summary>World_Crop_Tomato_Body_Stage02 — tomato flowering</summary>

- File: `World_Crop_Tomato_Body_Stage02.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon tomato near ripe stage, small yellow flowers visible, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric tomato stage with small yellow flowers, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Cà chua giai đoạn có hoa.

</details>

<details><summary>World_Crop_Tomato_Body_Stage03 — tomato ripe</summary>

- File: `World_Crop_Tomato_Body_Stage03.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon ripe tomato crop, red tomatoes visible, harvest-ready, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric ripe tomato crop with red tomatoes, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Cà chua chín thu hoạch.

</details>

<details><summary>World_Crop_Tomato_Body_Dead — tomato dead state</summary>

- File: `World_Crop_Tomato_Body_Dead.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon dead tomato crop, wilted leaves and stems, non-graphic, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric dead tomato crop, wilted leaves, cute cartoon, transparent background"
Negative: "realistic gore, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Cà chua chết.

</details>

### Strawberry (5)

<details><summary>World_Crop_Strawberry_Body_Stage00 — strawberry seedling</summary>

- File: `World_Crop_Strawberry_Body_Stage00.png`
- Target: `Assets/_Project/Art/Sprites/UI/Icons/Crops/Strawberry/`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon strawberry seedling, tiny green sprout, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric strawberry seedling, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Dâu tây mầm non.

</details>

<details><summary>World_Crop_Strawberry_Body_Stage01 — strawberry growing</summary>

- File: `World_Crop_Strawberry_Body_Stage01.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon strawberry growing stage, green leaf cluster, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric strawberry growing stage, green leaves, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Dâu tây phát triển.

</details>

<details><summary>World_Crop_Strawberry_Body_Stage02 — strawberry flowering</summary>

- File: `World_Crop_Strawberry_Body_Stage02.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon strawberry near ripe stage, white flowers visible, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric strawberry stage with white flowers, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Dâu tây giai đoạn có hoa.

</details>

<details><summary>World_Crop_Strawberry_Body_Stage03 — strawberry ripe</summary>

- File: `World_Crop_Strawberry_Body_Stage03.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon ripe strawberry crop, red strawberries visible, harvest-ready, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric ripe strawberry crop with red berries, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Dâu tây chín thu hoạch.

</details>

<details><summary>World_Crop_Strawberry_Body_Dead — strawberry dead state</summary>

- File: `World_Crop_Strawberry_Body_Dead.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon dead strawberry crop, wilted leaves, non-graphic, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric dead strawberry crop, wilted leaves, cute cartoon, transparent background"
Negative: "realistic gore, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Dâu tây chết.

</details>

### Pumpkin (5)

<details><summary>World_Crop_Pumpkin_Body_Stage00 — pumpkin seedling</summary>

- File: `World_Crop_Pumpkin_Body_Stage00.png`
- Target: `Assets/_Project/Art/Sprites/UI/Icons/Crops/Pumpkin/`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon pumpkin seedling, tiny sprout with first leaves, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric pumpkin seedling, tiny sprout, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Bí đỏ mầm non.

</details>

<details><summary>World_Crop_Pumpkin_Body_Stage01 — pumpkin vine growing</summary>

- File: `World_Crop_Pumpkin_Body_Stage01.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon pumpkin growing stage, short vine and larger leaves, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric pumpkin vine growing stage, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Bí đỏ dây phát triển.

</details>

<details><summary>World_Crop_Pumpkin_Body_Stage02 — pumpkin near ripe</summary>

- File: `World_Crop_Pumpkin_Body_Stage02.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon pumpkin near ripe stage, larger vine with small green pumpkin fruit, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric pumpkin near ripe stage, small green pumpkin fruit, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Bí đỏ gần chín.

</details>

<details><summary>World_Crop_Pumpkin_Body_Stage03 — pumpkin ripe</summary>

- File: `World_Crop_Pumpkin_Body_Stage03.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon ripe pumpkin crop, large orange pumpkin fruit, harvest-ready, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric ripe pumpkin crop, large orange fruit, cute cartoon, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Bí đỏ chín thu hoạch.

</details>

<details><summary>World_Crop_Pumpkin_Body_Dead — pumpkin dead state</summary>

- File: `World_Crop_Pumpkin_Body_Dead.png`

### ChatGPT / DALL-E (EN)
Prompt: "Isometric 2.5D cute cartoon dead pumpkin crop, wilted vine and leaves, non-graphic, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "isometric dead pumpkin crop, wilted vine, cute cartoon, transparent background"
Negative: "realistic gore, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Bí đỏ chết.

</details>
