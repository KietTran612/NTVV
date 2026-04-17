# Animal Entities Prompt Spec
Date: 2026-04-17

## Group Rules
- Domain: `World`
- Category: `Animal` / `Product`
- Size: 256x256
- Pivot target: bottom-center at ground contact
- One sprite per file
- English-only naming/prompt/setup

## Naming Pattern
- `World_Animal_[Entity]_Body_Stage00`
- `World_Animal_[Entity]_Body_Stage01`
- `World_Animal_[Entity]_Body_Stage02`
- `World_Animal_[Entity]_Body_Dead`
- `World_Product_[Entity]_Collect_Ready`

## Missing Animal Assets (6)

<details>
<summary>World_Animal_Chicken_Body_Dead — chicken dead state</summary>

- File: `World_Animal_Chicken_Body_Dead.png`
- Target: `Assets/_Project/Art/Sprites/UI/Icons/Animals/Chicken/`

### ChatGPT / DALL-E (EN)
Prompt: "Cute cartoon dead chicken state for farm game, non-graphic, clear readable silhouette, transparent background, clean outline"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "cute dead chicken state, non graphic, cartoon style, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry, gore"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Trạng thái gà chết, không phản cảm.

</details>

<details>
<summary>World_Animal_Duck_Body_Stage00 — duck baby stage</summary>

- File: `World_Animal_Duck_Body_Stage00.png`
- Target: `Assets/_Project/Art/Sprites/UI/Icons/Animals/Duck/`

### ChatGPT / DALL-E (EN)
Prompt: "Cute cartoon duck baby, yellow fluffy feathers, small orange beak, rounded body, transparent background, game-ready sprite"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "cute duck baby, yellow fluffy feathers, orange beak, cartoon game sprite, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Vịt con stage đầu.

</details>

<details>
<summary>World_Animal_Duck_Body_Stage01 — duck growth stage</summary>

- File: `World_Animal_Duck_Body_Stage01.png`
- Target: `Assets/_Project/Art/Sprites/UI/Icons/Animals/Duck/`

### ChatGPT / DALL-E (EN)
Prompt: "Cute cartoon duck juvenile stage, transition from yellow to white feathers, medium body, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "cute duck juvenile, mixed yellow white feathers, cartoon style, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Vịt lớn vừa stage chuyển tiếp.

</details>

<details>
<summary>World_Animal_Duck_Body_Stage02 — duck mature stage</summary>

- File: `World_Animal_Duck_Body_Stage02.png`
- Target: `Assets/_Project/Art/Sprites/UI/Icons/Animals/Duck/`

### ChatGPT / DALL-E (EN)
Prompt: "Cute cartoon mature duck, mostly white feathers, orange beak, clean silhouette, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "cute mature duck, white feathers, orange beak, cartoon farm sprite, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Vịt trưởng thành.

</details>

<details>
<summary>World_Animal_Duck_Body_Dead — duck dead state</summary>

- File: `World_Animal_Duck_Body_Dead.png`
- Target: `Assets/_Project/Art/Sprites/UI/Icons/Animals/Duck/`

### ChatGPT / DALL-E (EN)
Prompt: "Cute cartoon dead duck state for farm game, non-graphic, readable silhouette, transparent background"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "cute dead duck state, non graphic, cartoon style, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry, gore"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Trạng thái vịt chết.

</details>

<details>
<summary>World_Product_DuckEgg_Collect_Ready — duck egg collect icon</summary>

- File: `World_Product_DuckEgg_Collect_Ready.png`
- Target: `Assets/_Project/Art/Sprites/UI/Icons/Animals/Duck/`

### ChatGPT / DALL-E (EN)
Prompt: "Cute cartoon duck egg icon, light blue shell, clean outline, transparent background, game-ready"
Setup: DALL-E 3 | 1024x1024 | vivid | hd

### ComfyUI (EN)
Positive: "cute duck egg icon, light blue shell, cartoon style, transparent background"
Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
Checkpoint: dreamshaper_8 / anything-v5 | Sampler: DPM++ 2M Karras | Steps: 28 | CFG: 7 | Size: 512x512

### VN note
Icon trứng vịt để thu hoạch.

</details>
