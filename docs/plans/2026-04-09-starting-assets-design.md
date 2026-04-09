# [Design] Starting Assets: Carrot & Chicken (Atomic Style)

## 1. Overview
This document defines the visual requirements and prompts for the first set of functional assets in NTVV. These assets are critical for testing the end-to-end farming and animal husbandry loops.

- **Style**: Glossy 3D Kawaii (Atomic HUD Rebirth)
- **Primary Anchors**: Satin plastic, soft-touch, 3D Isometric View, subtle diffused highlights.

---

## 2. Crop Set: Carrot (crop_01)
*Goal: Visual progression from seed to harvest.*

| Stage | Name | Visual Description |
|-------|------|--------------------|
| Seed | `icon_Seeds_Carrot_Atomic` | A cute, small paper seed packet with a cartoon carrot illustration on it. |
| Stage 1 | `icon_Carrot_Stage1_Atomic` | A small green sprout with two tiny leaves poking out of the ground. |
| Stage 2 | `icon_Carrot_Stage2_Atomic` | A medium-sized green bush with more leaves, hint of orange near the base. |
| Stage 3 | `icon_Carrot_Stage3_Atomic` | Fully grown vibrant orange carrot with bright green lush leaves. |

---

## 3. Animal Set: Chicken (animal_01)
*Goal: Life cycle of the starter animal.*

| Entity | Name | Visual Description |
|--------|------|--------------------|
| Baby | `icon_Chick_Atomic` | A round, fluffy yellow chick with small orange beak and tiny feet. |
| Adult | `icon_Chicken_Atomic` | A mature white/light yellow hen with a small red comb and soft rounded volume. |
| Product | `icon_Egg_Atomic` | A smooth, oval white egg with a gentle glossy sheen. |
| Feed | `icon_Feed_Worm_Atomic` | A small, cute green stylized worm wiggling slightly. |

---

## 4. Technical Specifications
- **Perspective**: All icons use **3D Isometric View (30-degree angle)** for consistent depth within the UI and inventory.
- **Material**: Non-metallic glossy plastic for organic items (Carrot, Chick) to maintain "Soft-touch" feel.
- **Background**: **Transparent** (or Magenta Chroma Key fallback).
- **Scale**: Target high resolution (at least 512x512) for sharp downscaling in Unity.

---

## 5. Next Steps
1. Append these prompts to `docs/guides/Atomic_HUD_Prompt_Library.md`.
2. Generate these prompts for user review.
3. Map these assets to `CropDataSO` and `AnimalDataSO` in Unity.
