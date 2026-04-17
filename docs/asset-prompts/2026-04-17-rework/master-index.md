# Asset Prompt Master Index (Rework)
Date: 2026-04-17
Scope: Full rework prompt system for assets listed in `../2026-04-17-asset-inventory-all.md`

## Folder
`docs/asset-prompts/2026-04-17-rework/`

## Files
- [world-overlays.md](world-overlays.md) — Soil + weed/pest/water overlays
- [entities-crops.md](entities-crops.md) — Crop stages/dead sets
- [entities-animals.md](entities-animals.md) — Animal stages/dead/product sets
- [ui.md](ui.md) — UI icons (nav/inventory/feed/common)

## Global Rules
- All file names, prompts, and tool setup must be English only.
- Vietnamese is allowed only for short explanation notes.
- Naming convention:
  - `[Domain]_[Category]_[Entity]_[Variant]_[State]`
  - Stage format: `Stage00`, `Stage01`, `Stage02`, `Stage03`
  - No `_v01` suffix.
- Runtime interaction standard:
  - Pivot: bottom-center at ground contact.
  - Click: footprint collider (not alpha-pixel click).
- One exported PNG = one asset only.
- Do not pack multiple gameplay states into one single combined sprite.

## Per-Asset Template (collapsible)
```md
<details>
<summary>World_Crop_Carrot_Base_Stage00 — seedling stage</summary>

### File
- Name: `World_Crop_Carrot_Base_Stage00.png`
- Target: `Assets/_Project/Art/Sprites/...`
- Size: `512x512` (or as specified by group)
- Pivot rule: bottom-center at ground contact
- Click rule: footprint collider (Unity-side)

### ChatGPT / DALL-E (EN)
- Prompt: "..."
- Setup: DALL-E 3, 1024x1024, style vivid, quality hd

### ComfyUI (EN)
- Positive: "..."
- Negative: "realistic, photo, 3d render, dark, gritty, pixel art, text, watermark, blurry"
- Checkpoint: dreamshaper_8 / anything-v5
- Sampler: DPM++ 2M Karras
- Steps: 28
- CFG: 7
- Size: 512x512
- Seed: fixed per batch for consistency

### VN note
- Ghi chú ngắn cho bạn: sprite này dùng cho ...

</details>
```
