---
name: generate-image
description: Generate game asset images internal AI capabilities. No external API keys or scripts are needed.
---

# Generate Image Skill (Internal AI Version)

## Purpose
Generate game asset images (PNG/JPG) using the AI's internal `generate_image` capability and save them directly to the Unity project under `Assets/_Project/Art/AI_Generated/`.

## Prerequisites
- **None**: No external API keys or `node_modules` are required.

## Workflow
1. **Request**: Ask the agent to generate an image (e.g., "Tạo icon quả táo đỏ").
2. **Generation**: The agent uses its built-in `generate_image` tool with optimized prompt (isometric, clean edges, etc.).
3. **Download/Save**: The agent moves the resulting file into `Assets/_Project/Art/AI_Generated/`.
4. **Import**: Unity automatically detects the new asset.

## Asset Types & Optimization
The agent automatically applies technical keywords based on the asset type:
- **Vehicle**: Side/top view, wheels visible, consistent scale.
- **Prop**: 3D centered, recognizable silhouette, white background.
- **Obstacle**: Geometric, stackable, clear edges.
- **General**: Studio lighting, high detail, Kawaii/Cartoon style (Project NTVV standard).

## Inputs
- `description` (required): Text description of the asset.
- `assetType` (optional): Type of asset ('vehicle', 'obstacle', 'prop', 'general').
- `baseImage` (optional): An existing image to modify or use as a style base.

## Output
- Image file saved to `Assets/_Project/Art/AI_Generated/`.
- Notification from the agent about the final file path.
