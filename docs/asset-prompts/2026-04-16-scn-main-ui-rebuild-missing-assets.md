# Missing Assets — scn-main-ui-rebuild
Date: 2026-04-16
Spec: `.kiro/specs/scn-main-ui-rebuild/`

> Scan kết quả: 7/9 sprites có sẵn. 2 sprites thiếu cần generate.
> Prefabs: ShopEntry_Seed.prefab ✅, InventorySlot.prefab ✅

---

## icon_Farm_Atomic.png
Target path: Assets/_Project/Art/Sprites/UI/Icons/Nav/icon_Farm_Atomic.png
Size: 64×64 | Style: 2.5D isometric, cute cartoon, bright warm colors
Status: applied
Used in: BottomNav → NavBtn_Farm → NavIcon_Farm

### ChatGPT / DALL-E
Prompt: "A small cute farm field icon with green crops growing in rows, top-down isometric view, cute cartoon style, bright warm colors, rounded shapes, soft shadows, white background, stylized farm game asset, production-friendly, clean outline, no text, no humans, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: icon_Farm_Atomic.png

### Stable Diffusion
Positive: "farm field icon, small green crop rows, isometric 2.5D game asset, cute cartoon, bright warm colors, rounded shapes, soft cel shading, white background, game-ready sprite, clean lineart, vibrant, top-down view"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: icon_Farm_Atomic.png

### ComfyUI
Model node  : CheckpointLoaderSimple → dreamshaper_8.safetensors
Positive    : farm field icon, small green crop rows, isometric 2.5D game asset, cute cartoon, bright warm colors, rounded shapes, soft cel shading, white background, game-ready sprite, clean lineart, vibrant
Negative    : realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans
KSampler    : sampler=dpmpp_2m | scheduler=karras | steps=28 | cfg=7.0 | denoise=1.0
SaveImage   : filename=icon_Farm_Atomic.png
Optional LoRA: game-icon-institute | flat-2d-animerge (optional, strength 0.6-0.8)

---

## icon_Barn_Atomic.png
Target path: Assets/_Project/Art/Sprites/UI/Icons/Nav/icon_Barn_Atomic.png
Size: 64×64 | Style: 2.5D isometric, cute cartoon, bright warm colors
Status: applied
Used in: BottomNav → NavBtn_Barn → NavIcon_Barn

### ChatGPT / DALL-E
Prompt: "A small cute red barn building icon, isometric 2.5D view, cute cartoon style, bright warm colors, rounded shapes, soft shadows, white background, stylized farm game asset, production-friendly, clean outline, no text, no humans, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: icon_Barn_Atomic.png

### Stable Diffusion
Positive: "barn building icon, small red barn, isometric 2.5D game asset, cute cartoon, bright warm colors, rounded shapes, soft cel shading, white background, game-ready sprite, clean lineart, vibrant"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: icon_Barn_Atomic.png

### ComfyUI
Model node  : CheckpointLoaderSimple → dreamshaper_8.safetensors
Positive    : barn building icon, small red barn, isometric 2.5D game asset, cute cartoon, bright warm colors, rounded shapes, soft cel shading, white background, game-ready sprite, clean lineart, vibrant
Negative    : realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans
KSampler    : sampler=dpmpp_2m | scheduler=karras | steps=28 | cfg=7.0 | denoise=1.0
SaveImage   : filename=icon_Barn_Atomic.png
Optional LoRA: game-icon-institute | flat-2d-animerge (optional, strength 0.6-0.8)

---

## Hướng dẫn sau khi generate

1. Copy file `.png` vào đúng target path ở trên
2. Trong Unity: `Assets > Refresh` hoặc Ctrl+R
3. Set Texture Type = Sprite (2D and UI), Filter Mode = Bilinear, Compression = None
4. Vào scene SCN_Main, tìm `[HUD_CANVAS]/BottomNav/NavBtn_Farm/NavIcon_Farm`
5. Set Image.sprite = icon_Farm_Atomic
6. Tương tự cho NavBtn_Barn/NavIcon_Barn
7. Update Status trong file này: `pending → resolved`
