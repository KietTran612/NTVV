# Resource Checker Agent

## Purpose
Scan assets cần thiết trước khi implement. Nếu thiếu → generate AI prompts và track lifecycle.
Không bao giờ block pipeline — luôn proceed dù có asset thiếu.

---

## Activate When
Sub-task label chứa: `· Resource Check`, `.0 ·`, `Resource Check`, `1b.`

---

## Scope
- ✅ Sprites, textures, icons (`.png`, `.jpg`, `.psd`)
- ✅ Prefabs (`.prefab`)
- ✅ ScriptableObject assets (`.asset`)
- 🔜 Audio clips (`.wav`, `.mp3`) — future
- 🔜 Animation clips — future
- 🔜 Fonts — future

## Out of Scope
- C# scripts → script-agent.md
- Scene files → unity-agent.md

---

## Workflow

```
1. Đọc danh sách assets được liệt kê trong sub-task X.0
2. Với mỗi asset:
   a. assets-find tên asset trong Assets/ project
   b. Nếu TÌM THẤY → đánh dấu ✅, tiếp tục
   c. Nếu KHÔNG TÌM THẤY → đánh dấu ⚠️, generate prompts
3. Append kết quả vào docs/asset-prompts/{date}-{spec}-missing-assets.md
4. Output summary
5. PROCEED — không dừng lại dù có asset thiếu
```

---

## Asset Lifecycle

```
pending   → chưa generate ảnh
done      → user đã gen + copy vào Assets/ (user update thủ công)
resolved  → Resource Checker detect file tồn tại trong Assets/
applied   → Unity Agent đã replace placeholder + xóa TODO comment
```

**Khi re-run task:**
- Đọc prompts file, tìm entries có `Status: done`
- Check file thực sự tồn tại trong Assets/ → update `done → resolved`
- Báo Unity Agent biết cần replace placeholder cho asset đó
- Không tạo lại prompt cho asset đã `resolved` hoặc `applied`

---

## Asset Naming Convention

- Format: `snake_case`
- Pattern: `{object}_{state}.png`
- Ví dụ: `barn_idle.png`, `chicken_walk.png`, `coin_icon.png`, `btn_close_circle.png`
- Tên phải khớp chính xác với tên file Unity script đang reference

---

## Art Style Base (NTVV Project)

> "Bright casual farm, 2.5D isometric fake-3D using 2D assets; rounded 2D UI; bright, warm, easy-to-read colors; stylized, cute, production-friendly, pixel-free, not realistic, not true 3D."

**Negative prompt (SD/ComfyUI):**
`realistic, photo, 3D render, dark, gritty, pixel art, text, watermark, blurry, humans, nsfw`

---

## Prompt Output Format

Append vào file `docs/asset-prompts/{YYYY-MM-DD}-{spec}-missing-assets.md`:

```markdown
## {filename}.png
Target path: Assets/{path}/{filename}.png
Size: {WxH} | Style: 2.5D isometric, cute cartoon, bright warm colors
Status: pending

### ChatGPT / DALL-E
Prompt: "{mô tả asset} , 2.5D isometric view, cute cartoon style,
  bright warm colors, rounded shapes, soft shadows, white background,
  stylized farm game asset, production-friendly, clean outline,
  no text, no humans, square canvas"
Setup: DALL-E 3 | Size: 1024x1024 | style: vivid | quality: hd
Output filename: {filename}.png

### Stable Diffusion
Positive: "{mô tả asset}, isometric 2.5D game asset, cute cartoon,
  bright warm colors, rounded shapes, soft cel shading,
  white background, game-ready sprite, clean lineart, vibrant"
Negative: "realistic, photo, 3D render, dark, gritty, pixel art,
  text, watermark, blurry, humans"
Setup:
  Checkpoint : dreamshaper_8 / anything-v5
  Sampler    : DPM++ 2M Karras
  Steps      : 28
  CFG        : 7
  Size       : 512x512 (icon) | 768x768 (character/building)
Output filename: {filename}.png

### ComfyUI
Model node  : CheckpointLoaderSimple → dreamshaper_8.safetensors
Positive    : [same as Stable Diffusion positive]
Negative    : [same as Stable Diffusion negative]
KSampler    : sampler=dpmpp_2m | scheduler=karras | steps=28 | cfg=7.0 | denoise=1.0
SaveImage   : filename={filename}.png
Optional LoRA: game-icon-institute | flat-2d-animerge (optional, strength 0.6-0.8)

---
```

---

## Summary Output

Sau khi scan xong, output:

```
📋 RESOURCE CHECK SUMMARY — Task {N}
✅ Found   : {N} assets
⚠️ Missing : {N} assets → prompts generated
📄 Prompts : docs/asset-prompts/{date}-{spec}-missing-assets.md
→ Proceeding with Unity implementation. Missing assets sẽ dùng placeholder.
```

---

## Extension Points (🔜 Future)
- Audio asset scan (`.wav`, `.mp3`) với prompt template cho music/sfx
- Animation clip scan
- Font asset scan
- Batch mode: scan toàn bộ spec một lần thay vì per-task
- Auto-detect asset references từ C# script source code
