# Missing Assets — Animal Sprites
Date: 2026-04-17
Spec: m4-animal-care

> 6 sprites: Chicken Dead (1) + Duck all stages (5).
> Duck sprites are potentially BLOCKING for M4 integration test (AnimalView needs sprites to render).

---

## icon_Chicken_Dead_Atomic.png
Target path: Assets/_Project/Art/Sprites/UI/Icons/Animals/Chicken/icon_Chicken_Dead_Atomic.png
Size: 256x256 | Style: Cute cartoon 2D, transparent background
Status: pending
Used in: AnimalView — deadSprite slot (AnimalDataSO.deadSprite)

### ChatGPT / DALL-E
Prompt: "A cute cartoon dead chicken lying on its side, x-marks for eyes, fluffy white feathers, red comb still visible, peaceful/comedic expression, transparent background, game icon style, clean outline, no text, no humans, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: icon_Chicken_Dead_Atomic.png

### Stable Diffusion
Positive: "cute cartoon dead chicken, lying on side, x eyes, white feathers, red comb, farm game asset, transparent background, game-ready icon, clean lineart, soft shading, chibi style"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, scary"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: icon_Chicken_Dead_Atomic.png

### ComfyUI
Model node  : CheckpointLoaderSimple -> dreamshaper_8.safetensors
Positive    : cute cartoon dead chicken, lying on side, x eyes, white feathers, red comb, farm game asset, transparent background, game-ready icon, clean lineart, soft shading, chibi style
Negative    : realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, scary
KSampler    : sampler=dpmpp_2m | scheduler=karras | steps=28 | cfg=7.0 | denoise=1.0
SaveImage   : filename=icon_Chicken_Dead_Atomic.png

---

## icon_Duck_Stage1_Atomic.png
Target path: Assets/_Project/Art/Sprites/UI/Icons/Animals/Duck/icon_Duck_Stage1_Atomic.png
Size: 256x256 | Style: Cute cartoon 2D, transparent background
Status: pending
Used in: AnimalView — stageSprites[0] (Baby stage)

### ChatGPT / DALL-E
Prompt: "A tiny cute baby duck chick, fluffy bright yellow feathers, small orange beak, big sparkly eyes, round pudgy body, sitting pose, transparent background, cute cartoon game icon style, clean outline, no text, no humans, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: icon_Duck_Stage1_Atomic.png

### Stable Diffusion
Positive: "cute baby duckling, fluffy yellow feathers, tiny orange beak, big eyes, round body, farm game asset, transparent background, game-ready icon, clean lineart, chibi style, kawaii"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, adult duck"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: icon_Duck_Stage1_Atomic.png

### ComfyUI
Model node  : CheckpointLoaderSimple -> dreamshaper_8.safetensors
Positive    : cute baby duckling, fluffy yellow feathers, tiny orange beak, big eyes, round body, farm game asset, transparent background, game-ready icon, clean lineart, chibi style, kawaii
Negative    : realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, adult duck
KSampler    : sampler=dpmpp_2m | scheduler=karras | steps=28 | cfg=7.0 | denoise=1.0
SaveImage   : filename=icon_Duck_Stage1_Atomic.png

---

## icon_Duck_Stage2_Atomic.png
Target path: Assets/_Project/Art/Sprites/UI/Icons/Animals/Duck/icon_Duck_Stage2_Atomic.png
Size: 256x256 | Style: Cute cartoon 2D, transparent background
Status: pending
Used in: AnimalView — stageSprites[1] (Stage2 / adolescent)

### ChatGPT / DALL-E
Prompt: "A cute juvenile duck, in-between stage — partly white feathers replacing yellow, medium body size, orange beak, alert expression, standing pose, transparent background, cute cartoon farm game icon, clean outline, no text, no humans, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: icon_Duck_Stage2_Atomic.png

### Stable Diffusion
Positive: "cute juvenile duck, half-white half-yellow feathers, medium size, orange beak, farm game asset, transparent background, game-ready icon, clean lineart, chibi style"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, fully yellow duckling, fully adult white duck"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: icon_Duck_Stage2_Atomic.png

### ComfyUI
Model node  : CheckpointLoaderSimple -> dreamshaper_8.safetensors
Positive    : cute juvenile duck, half-white half-yellow feathers, medium size, orange beak, farm game asset, transparent background, game-ready icon, clean lineart, chibi style
Negative    : realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, fully yellow duckling, fully adult white duck
KSampler    : sampler=dpmpp_2m | scheduler=karras | steps=28 | cfg=7.0 | denoise=1.0
SaveImage   : filename=icon_Duck_Stage2_Atomic.png

---

## icon_Duck_Stage3_Atomic.png
Target path: Assets/_Project/Art/Sprites/UI/Icons/Animals/Duck/icon_Duck_Stage3_Atomic.png
Size: 256x256 | Style: Cute cartoon 2D, transparent background
Status: pending
Used in: AnimalView — stageSprites[2] (Mature / adult)

### ChatGPT / DALL-E
Prompt: "A cute plump adult duck, all-white fluffy feathers, bright orange beak and feet, happy expression with big shiny eyes, standing proudly, transparent background, cute cartoon farm game icon, clean outline, no text, no humans, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: icon_Duck_Stage3_Atomic.png

### Stable Diffusion
Positive: "cute adult duck, white fluffy feathers, orange beak, happy expression, plump body, farm game asset, transparent background, game-ready icon, clean lineart, chibi style, kawaii"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, yellow duckling"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: icon_Duck_Stage3_Atomic.png

### ComfyUI
Model node  : CheckpointLoaderSimple -> dreamshaper_8.safetensors
Positive    : cute adult duck, white fluffy feathers, orange beak, happy expression, plump body, farm game asset, transparent background, game-ready icon, clean lineart, chibi style, kawaii
Negative    : realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, yellow duckling
KSampler    : sampler=dpmpp_2m | scheduler=karras | steps=28 | cfg=7.0 | denoise=1.0
SaveImage   : filename=icon_Duck_Stage3_Atomic.png

---

## icon_Duck_Dead_Atomic.png
Target path: Assets/_Project/Art/Sprites/UI/Icons/Animals/Duck/icon_Duck_Dead_Atomic.png
Size: 256x256 | Style: Cute cartoon 2D, transparent background
Status: pending
Used in: AnimalView — deadSprite slot (AnimalDataSO.deadSprite)

### ChatGPT / DALL-E
Prompt: "A cute cartoon dead duck lying on its side, x-marks for eyes, white feathers, orange beak pointing sideways, peaceful/comedic expression, transparent background, farm game icon style, clean outline, no text, no humans, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: icon_Duck_Dead_Atomic.png

### Stable Diffusion
Positive: "cute cartoon dead duck, lying on side, x eyes, white feathers, orange beak, farm game asset, transparent background, game-ready icon, clean lineart, soft shading, chibi style"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, scary"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: icon_Duck_Dead_Atomic.png

### ComfyUI
Model node  : CheckpointLoaderSimple -> dreamshaper_8.safetensors
Positive    : cute cartoon dead duck, lying on side, x eyes, white feathers, orange beak, farm game asset, transparent background, game-ready icon, clean lineart, soft shading, chibi style
Negative    : realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, scary
KSampler    : sampler=dpmpp_2m | scheduler=karras | steps=28 | cfg=7.0 | denoise=1.0
SaveImage   : filename=icon_Duck_Dead_Atomic.png

---

## icon_DuckEgg_Atomic.png
Target path: Assets/_Project/Art/Sprites/UI/Icons/Animals/Duck/icon_DuckEgg_Atomic.png
Size: 256x256 | Style: Cute cartoon 2D, transparent background
Status: pending
Used in: AnimalView — readyToCollectIcon (shown when duck egg is ready to collect)

### ChatGPT / DALL-E
Prompt: "A cute cartoon duck egg, light blue-green pastel color, smooth oval shape, soft sheen highlight, slightly larger than a chicken egg, transparent background, farm game icon style, clean outline, no text, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: icon_DuckEgg_Atomic.png

### Stable Diffusion
Positive: "cute cartoon duck egg, light blue green pastel, smooth oval, shiny highlight, farm game asset, transparent background, game-ready icon, clean lineart, soft shading, kawaii"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, brown egg, white chicken egg"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: icon_DuckEgg_Atomic.png

### ComfyUI
Model node  : CheckpointLoaderSimple -> dreamshaper_8.safetensors
Positive    : cute cartoon duck egg, light blue green pastel, smooth oval, shiny highlight, farm game asset, transparent background, game-ready icon, clean lineart, soft shading, kawaii
Negative    : realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, brown egg, white chicken egg
KSampler    : sampler=dpmpp_2m | scheduler=karras | steps=28 | cfg=7.0 | denoise=1.0
SaveImage   : filename=icon_DuckEgg_Atomic.png

---

## Summary
- icon_Chicken_Dead_Atomic: pending -> Assets/_Project/Art/Sprites/UI/Icons/Animals/Chicken/
- icon_Duck_Stage1_Atomic: pending -> Assets/_Project/Art/Sprites/UI/Icons/Animals/Duck/
- icon_Duck_Stage2_Atomic: pending -> Assets/_Project/Art/Sprites/UI/Icons/Animals/Duck/
- icon_Duck_Stage3_Atomic: pending -> Assets/_Project/Art/Sprites/UI/Icons/Animals/Duck/
- icon_Duck_Dead_Atomic: pending -> Assets/_Project/Art/Sprites/UI/Icons/Animals/Duck/
- icon_DuckEgg_Atomic: pending -> Assets/_Project/Art/Sprites/UI/Icons/Animals/Duck/

## Huong dan sau khi generate
1. Copy file .png vao dung target path
2. Trong Unity: Assets > Refresh hoac Ctrl+R
3. Set Texture Type = Sprite (2D and UI), Filter Mode = Bilinear, Compression = None
4. Mo AnimalDataSO (animal_01 cho Chicken, animal_02 cho Duck)
5. Assign sprite vao dung slot: stageSprites[0/1/2], deadSprite, readyToCollectIcon
6. Update Status trong file nay: pending -> applied
