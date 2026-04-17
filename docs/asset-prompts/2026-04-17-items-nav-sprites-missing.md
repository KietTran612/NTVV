# Missing Assets u2014 Items & Nav Sprites
Date: 2026-04-17
Spec: m3a-crop-care-harvest, scn-main-ui-rebuild

> 3 sprites: icon_Grass (storage + feed display) + icon_Shop + icon_Event (BottomNav placeholders).
> NON-BLOCKING u2014 Storage shows without icon; Nav currently uses placeholder sprites.

---

## icon_Grass_Atomic.png
Target path: Assets/_Project/Art/Sprites/UI/Icons/Common/icon_Grass_Atomic.png
Size: 128x128 | Style: Cute cartoon 2D, transparent background
Status: pending
Used in: StoragePanelController (item_grass in inventory grid) + AnimalDetailPanel (feed display for Chicken + Duck)

### ChatGPT / DALL-E
Prompt: "A cute cartoon bundle of green grass, small tied bunch of fresh green blades, bright vivid green color, simple icon style, transparent background, farm game UI icon, clean outline, no text, no humans, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: icon_Grass_Atomic.png

### Stable Diffusion
Positive: "cute cartoon grass bundle icon, small bunch of green grass blades, bright green, farm game UI asset, transparent background, game-ready icon, clean lineart, simple flat style"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, complex background"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: icon_Grass_Atomic.png

### ComfyUI
Model node  : CheckpointLoaderSimple -> dreamshaper_8.safetensors
Positive    : cute cartoon grass bundle icon, small bunch of green grass blades, bright green, farm game UI asset, transparent background, game-ready icon, clean lineart, simple flat style
Negative    : realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, complex background
KSampler    : sampler=dpmpp_2m | scheduler=karras | steps=28 | cfg=7.0 | denoise=1.0
SaveImage   : filename=icon_Grass_Atomic.png

---

## icon_Shop_Atomic.png
Target path: Assets/_Project/Art/Sprites/UI/Icons/Nav/icon_Shop_Atomic.png
Size: 64x64 | Style: Cute cartoon 2D, transparent background
Status: pending
Used in: BottomNav NavBtn_Shop (currently using icon_Sprout_Header_Atomic as placeholder)

> Match visual style with existing Nav icons: icon_Farm_Atomic (green crop field) and icon_Barn_Atomic (red barn building).

### ChatGPT / DALL-E
Prompt: "A small cute shop storefront icon, tiny wooden market stall or shopping basket, bright warm colors, top-down 2.5D isometric view, cute cartoon style, transparent background, stylized farm game nav icon, clean outline, no text, no humans, 64x64 style, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: icon_Shop_Atomic.png

### Stable Diffusion
Positive: "cute cartoon shop icon, tiny market stall or shopping basket, bright warm colors, isometric 2.5D game asset, farm game nav icon, transparent background, game-ready icon, clean lineart, vibrant, 64x64 style"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, complex"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: icon_Shop_Atomic.png
Optional LoRA: game-icon-institute | flat-2d-animerge (strength 0.6-0.8)

### ComfyUI
Model node  : CheckpointLoaderSimple -> dreamshaper_8.safetensors
Positive    : cute cartoon shop icon, tiny market stall or shopping basket, bright warm colors, isometric 2.5D game asset, farm game nav icon, transparent background, game-ready icon, clean lineart, vibrant
Negative    : realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, complex
KSampler    : sampler=dpmpp_2m | scheduler=karras | steps=28 | cfg=7.0 | denoise=1.0
SaveImage   : filename=icon_Shop_Atomic.png

---

## icon_Event_Atomic.png
Target path: Assets/_Project/Art/Sprites/UI/Icons/Nav/icon_Event_Atomic.png
Size: 64x64 | Style: Cute cartoon 2D, transparent background
Status: pending
Used in: BottomNav NavBtn_Event (currently using icon_Tab_Star_Atomic as placeholder)

> Match visual style with existing Nav icons: icon_Farm_Atomic and icon_Barn_Atomic.

### ChatGPT / DALL-E
Prompt: "A small cute special event icon, golden star or festive flag/banner, bright gold and warm colors, cute cartoon style, transparent background, stylized farm game nav icon, clean outline, no text, no humans, 64x64 style, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: icon_Event_Atomic.png

### Stable Diffusion
Positive: "cute cartoon event icon, golden star or festive banner flag, bright gold warm colors, farm game nav icon, transparent background, game-ready icon, clean lineart, vibrant, celebration theme"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, complex"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512
Output filename: icon_Event_Atomic.png
Optional LoRA: game-icon-institute | flat-2d-animerge (strength 0.6-0.8)

### ComfyUI
Model node  : CheckpointLoaderSimple -> dreamshaper_8.safetensors
Positive    : cute cartoon event icon, golden star or festive banner flag, bright gold warm colors, farm game nav icon, transparent background, game-ready icon, clean lineart, vibrant, celebration theme
Negative    : realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, complex
KSampler    : sampler=dpmpp_2m | scheduler=karras | steps=28 | cfg=7.0 | denoise=1.0
SaveImage   : filename=icon_Event_Atomic.png

---

## Summary
- icon_Grass_Atomic: pending -> Assets/_Project/Art/Sprites/UI/Icons/Common/
- icon_Shop_Atomic: pending -> Assets/_Project/Art/Sprites/UI/Icons/Nav/
- icon_Event_Atomic: pending -> Assets/_Project/Art/Sprites/UI/Icons/Nav/

## Huong dan sau khi generate
1. Copy file .png vao dung target path
2. Trong Unity: Assets > Refresh hoac Ctrl+R
3. Set Texture Type = Sprite (2D and UI), Filter Mode = Bilinear, Compression = None
4. Nav icons: Vao scene SCN_Main, tim BottomNav/NavBtn_Shop/NavIcon_Shop, set Image.sprite = icon_Shop_Atomic
5. Tuong tu NavBtn_Event/NavIcon_Event
6. Grass: Assign vao StorageItemCard prefab hoac dung trong feed display qua StorageSystem item icon lookup
7. Update Status trong file nay: pending -> applied
