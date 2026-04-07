# UI Kit Generation (Hybrid B+C) Implementation Plan

> **For Antigravity:** REQUIRED WORKFLOW: Use `.agent/workflows/execute-plan.md` to execute this plan in single-flow mode.

**Goal:** Tạo bộ Sprite UI cơ bản (Panel, Button, Icon) theo phong cách Hybrid đã duyệt và lưu vào dự án.

**Architecture:** Sử dụng công cụ `generate_image` với Prompt đã tối ưu để tạo ra các Sprite đơn lẻ (2D), tách nền và lưu tại thư mục `Art/Sprites/UI/`.

**Tech Stack:** `generate_image` (AI), Unity AssetDatabase.

---

### Task 1: Generate UI Panels (Lớp Nền tảng)

**Files:**
- Create: `Assets/_Project/Art/Sprites/UI/bg_Panel_Main.png`
- Create: `Assets/_Project/Art/Sprites/UI/bg_Panel_Main.meta`
- Create: `Assets/_Project/Art/Sprites/UI/bg_Panel_Sub.png`

**Step 1: Generate Main Panel**
Run: `generate_image` with prompt "Kawaii farm game main panel, butter cream color #FFF9E5, caramel wood texture border #8B5A2B, thick white sticker outline, bo tròn mạnh, game UI asset, 2D sprite, white background"
Save to: `Assets/_Project/Art/Sprites/UI/bg_Panel_Main.png`

**Step 2: Generate Sub Panel**
Run: `generate_image` with prompt "Kawaii farm game item slot panel, pale parchment color #F5F5DC, subtle rounded corners, thick white sticker outline, game UI asset, 2D sprite, white background"
Save to: `Assets/_Project/Art/Sprites/UI/bg_Panel_Sub.png`

**Step 3: Commit Assets**
```bash
git add Assets/_Project/Art/Sprites/UI/bg_Panel_Main.png Assets/_Project/Art/Sprites/UI/bg_Panel_Sub.png
git commit -m "feat: add main and sub UI panels"
```

### Task 2: Generate Interactive Buttons (Lớp Tương tác)

**Files:**
- Create: `Assets/_Project/Art/Sprites/UI/bg_Button_Green.png`
- Create: `Assets/_Project/Art/Sprites/UI/bg_Button_Yellow.png`
- Create: `Assets/_Project/Art/Sprites/UI/bg_Button_Blue.png`
- Create: `Assets/_Project/Art/Sprites/UI/bg_Button_Red.png`

**Step 1: Generate Green Button (Confirm)**
Run: `generate_image` with prompt "Kawaii farm game button, lush meadow green #4CAF50, glossy squishy finish, thick white sticker outline, game UI asset, 2D sprite, white background"
Save to: `Assets/_Project/Art/Sprites/UI/bg_Button_Green.png`

**Step 2: Generate Yellow Button (Upgrade)**
Run: `generate_image` with prompt "Kawaii farm game button, honey glaze yellow #FFC107, glossy squishy finish, thick white sticker outline, game UI asset, 2D sprite, white background"
Save to: `Assets/_Project/Art/Sprites/UI/bg_Button_Yellow.png`

**Step 3: Generate Blue and Red Buttons**
(Repeat generation for Sky Splash #00BCD4 and Berry Smash #E91E63)

**Step 4: Commit Buttons**
```bash
git add Assets/_Project/Art/Sprites/UI/bg_Button_*.png
git commit -m "feat: add interactive UI buttons"
```

### Task 3: Generate Resource Icons (HUD Icons)

**Files:**
- Create: `Assets/_Project/Art/Sprites/UI/icon_Gold.png`
- Create: `Assets/_Project/Art/Sprites/UI/icon_Diamond.png`

**Step 1: Generate Gold Icon**
Run: `generate_image` with prompt "Kawaii farm game gold coin, honey glow #FFD700, shiny cartoon style, thick white outline, game asset, 2D sprite, white background"
Save to: `Assets/_Project/Art/Sprites/UI/icon_Gold.png`

**Step 2: Generate Diamond Icon**
Run: `generate_image` with prompt "Kawaii farm game diamond, cyan crystal, shiny cartoon style, thick white outline, game asset, 2D sprite, white background"
Save to: `Assets/_Project/Art/Sprites/UI/icon_Diamond.png`

**Step 3: Commit Icons**
```bash
git add Assets/_Project/Art/Sprites/UI/icon_*.png
git commit -m "feat: add gold and diamond HUD icons"
```

### Task 4: Asset Refresh & Verification

**Files:**
- Modify: `Assets/_Project/Art/Sprites/UI.meta`

**Step 1: Force Asset Refresh**
Invoke `@assets-refresh` to ensure Unity imports all PNGs as Sprites.

**Step 2: Final Verification**
Use `list_dir` to confirm all files are in place with their .meta files.

**Step 3: Commit Final State**
```bash
git add Assets/_Project/Art/Sprites/UI/
git commit -m "chore: finalize UI Kit assets"
```
