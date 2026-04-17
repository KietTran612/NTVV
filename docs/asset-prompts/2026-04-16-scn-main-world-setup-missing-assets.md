# Missing Assets — scn-main-world-setup
Date: 2026-04-16
Spec: scn-main-world-setup

---

## soil_empty.png
Target path: Assets/_Project/Art/Sprites/World/soil_empty.png
Size: 512x512 | Style: 2.5D isometric, cute cartoon, bright warm colors
Status: pending

### ChatGPT / DALL-E
Prompt: "Isometric farm soil tile, empty tilled dirt plot, top-down 2.5D view, cute cartoon style, bright warm brown earth tones, rounded edges, soft shadows, white background, stylized farm game asset, production-friendly, clean outline, no text, no humans, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: soil_empty.png

### Stable Diffusion
Positive: "isometric farm soil tile, empty tilled dirt, top-down 2.5D game asset, cute cartoon, bright warm brown colors, rounded shapes, soft cel shading, white background, game-ready sprite, clean lineart, vibrant"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: soil_empty.png

### ComfyUI
Model node  : CheckpointLoaderSimple → dreamshaper_8.safetensors
Positive    : [same as Stable Diffusion positive]
Negative    : [same as Stable Diffusion negative]
KSampler    : sampler=dpmpp_2m | scheduler=karras | steps=28 | cfg=7.0 | denoise=1.0
SaveImage   : filename=soil_empty.png

---

## weed_overlay.png
Target path: Assets/_Project/Art/Sprites/World/weed_overlay.png
Size: 512x512 | Style: 2.5D isometric, cute cartoon, bright warm colors
Status: pending

### ChatGPT / DALL-E
Prompt: "Cartoon weed plant overlay sprite for farm game, small green weeds growing on soil, top-down 2.5D view, cute cartoon style, bright green colors, transparent background, stylized farm game asset, production-friendly, clean outline, no text, no humans, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: weed_overlay.png

### Stable Diffusion
Positive: "cartoon weed overlay sprite, small green weeds, farm game asset, 2.5D isometric, cute cartoon, bright green colors, transparent background, game-ready sprite, clean lineart"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: weed_overlay.png

---

## bug_overlay.png
Target path: Assets/_Project/Art/Sprites/World/bug_overlay.png
Size: 512x512 | Style: 2.5D isometric, cute cartoon, bright warm colors
Status: pending

### ChatGPT / DALL-E
Prompt: "Cartoon bug pest overlay sprite for farm game, small cute bug/insect on soil, top-down 2.5D view, cute cartoon style, warm brown colors, transparent background, stylized farm game asset, production-friendly, clean outline, no text, no humans, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: bug_overlay.png

### Stable Diffusion
Positive: "cartoon bug pest overlay sprite, small cute insect, farm game asset, 2.5D isometric, cute cartoon, warm brown colors, transparent background, game-ready sprite, clean lineart"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: bug_overlay.png

---

## water_needed.png
Target path: Assets/_Project/Art/Sprites/World/water_needed.png
Size: 512x512 | Style: 2.5D isometric, cute cartoon, bright warm colors
Status: pending

### ChatGPT / DALL-E
Prompt: "Cartoon water droplet indicator overlay sprite for farm game, small blue water drop icon on dry soil, top-down 2.5D view, cute cartoon style, light blue colors, transparent background, stylized farm game asset, production-friendly, clean outline, no text, no humans, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: water_needed.png

### Stable Diffusion
Positive: "cartoon water droplet indicator, farm game asset, 2.5D isometric, cute cartoon, light blue colors, transparent background, game-ready sprite, clean lineart"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: water_needed.png

---

## crop_carrot_phase1.png
Target path: Assets/_Project/Art/Sprites/Crops/crop_carrot_phase1.png
Size: 512x512 | Style: 2.5D isometric, cute cartoon, bright warm colors
Status: pending

### ChatGPT / DALL-E
Prompt: "Cartoon carrot seedling phase 1 sprite for farm game, tiny carrot sprout just emerging from soil, top-down 2.5D view, cute cartoon style, bright green and orange colors, transparent background, stylized farm game asset, production-friendly, clean outline, no text, no humans, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: crop_carrot_phase1.png

### Stable Diffusion
Positive: "cartoon carrot seedling phase 1, tiny sprout, farm game asset, 2.5D isometric, cute cartoon, bright green orange colors, transparent background, game-ready sprite, clean lineart"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: crop_carrot_phase1.png

---

## crop_carrot_phase2.png
Target path: Assets/_Project/Art/Sprites/Crops/crop_carrot_phase2.png
Size: 512x512 | Style: 2.5D isometric, cute cartoon, bright warm colors
Status: pending

### ChatGPT / DALL-E
Prompt: "Cartoon carrot growing phase 2 sprite for farm game, medium-sized carrot plant with green leaves, top-down 2.5D view, cute cartoon style, bright green and orange colors, transparent background, stylized farm game asset, production-friendly, clean outline, no text, no humans, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: crop_carrot_phase2.png

### Stable Diffusion
Positive: "cartoon carrot growing phase 2, medium plant with leaves, farm game asset, 2.5D isometric, cute cartoon, bright green orange colors, transparent background, game-ready sprite, clean lineart"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: crop_carrot_phase2.png

---

## crop_carrot_phase3.png
Target path: Assets/_Project/Art/Sprites/Crops/crop_carrot_phase3.png
Size: 512x512 | Style: 2.5D isometric, cute cartoon, bright warm colors
Status: pending

### ChatGPT / DALL-E
Prompt: "Cartoon carrot ripe/harvest phase 3 sprite for farm game, fully grown carrot ready to harvest with orange carrot visible, top-down 2.5D view, cute cartoon style, bright orange and green colors, transparent background, stylized farm game asset, production-friendly, clean outline, no text, no humans, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: crop_carrot_phase3.png

### Stable Diffusion
Positive: "cartoon carrot ripe harvest phase 3, fully grown carrot, farm game asset, 2.5D isometric, cute cartoon, bright orange green colors, transparent background, game-ready sprite, clean lineart"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: crop_carrot_phase3.png

---

## Summary
- soil_empty: MISSING → using Unity default white sprite placeholder
- weed_overlay: MISSING → using color placeholder (#7CFC00 green)
- bug_overlay: MISSING → using color placeholder (#8B4513 brown)
- water_needed: MISSING → using color placeholder (#87CEEB light blue)
- crop_carrot_phase1: MISSING → using color placeholder
- crop_carrot_phase2: MISSING → using color placeholder
- crop_carrot_phase3: MISSING → using color placeholder

---

## Task 7.0 — Data Check: GameDataRegistrySO.crops

Date: 2026-04-16 (Task 7 execution)

### Kết quả kiểm tra

| Asset | cropId | growTimeMin | baseYieldUnits | postRipeLifeMin | Status |
|-------|--------|-------------|----------------|-----------------|--------|
| crop_01 (Cà rốt) | crop_01 ✅ | 2 ✅ | 3 ✅ | 3 ✅ | **VALID** |
| crop_02 (Khoai tây) | crop_02 ✅ | 4 ✅ | 3 ✅ | 4 ✅ | **VALID** |
| crop_03 (Ngô) | crop_03 ✅ | 5 ✅ | 4 ✅ | 5 ✅ | **VALID** |
| crop_04 (Lúa mì) | crop_04 ✅ | 2 ✅ | **0 ❌** | **0 ❌** | **INVALID** |
| crop_05 (Cà chua) | crop_05 ✅ | 4 ✅ | **0 ❌** | **0 ❌** | **INVALID** |
| crop_06 (Dâu tây) | crop_06 ✅ | 6 ✅ | **0 ❌** | **0 ❌** | **INVALID** |
| crop_07 (Bí đỏ) | crop_07 ✅ | 9 ✅ | **0 ❌** | **0 ❌** | **INVALID** |

### Issues cần fix trong Task 7.1

- **crop_04 (Lúa mì):** `baseYieldUnits = 0` → cây không cho thu hoạch; `postRipeLifeMin = 0` → BUG-08 cây chết ngay khi chín
- **crop_05 (Cà chua):** `baseYieldUnits = 0` → cây không cho thu hoạch; `postRipeLifeMin = 0` → BUG-08
- **crop_06 (Dâu tây):** `baseYieldUnits = 0` → cây không cho thu hoạch; `postRipeLifeMin = 0` → BUG-08
- **crop_07 (Bí đỏ):** `baseYieldUnits = 0` → cây không cho thu hoạch; `postRipeLifeMin = 0` → BUG-08

### Action: Fix trong Task 7.1 (NON-BLOCKING — crops_01..03 đã valid)
