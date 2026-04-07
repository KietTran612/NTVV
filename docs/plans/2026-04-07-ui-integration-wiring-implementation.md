# UI Integration (The Pure MCP Bridge) Implementation Plan

> **For Antigravity:** REQUIRED WORKFLOW: Use `.agent/workflows/execute-plan.md` to execute this plan in single-flow mode.

**Goal:** Chuyển đổi các file PNG thành Sprites hoàn chỉnh (có 9-slicing) và tích hợp vào thanh HUD trong Scene.

**Architecture:** 
1. **Meta-Patching**: Chỉnh sửa trực tiếp file `.meta` của các Asset để Unity nhận diện Border và Sprite Mode.
2. **Component Wiring**: Sử dụng MCP để tìm các GameObject đích (Gold_Chip, BottomNav) và gán các Sprite mới vào.
3. **Layout Assembly**: Sắp xếp lại cấu trúc con (Icon + Text) để khớp với bộ Asset Kawaii.

---

### Task 1: Cấu hình 9-Slicing cho Panels & Buttons

**Files:**
- Modify: `Assets/_Project/Art/Sprites/UI/bg_Panel_Main.png.meta`
- Modify: `Assets/_Project/Art/Sprites/UI/bg_Panel_Sub.png.meta`
- Modify: `Assets/_Project/Art/Sprites/UI/bg_Button_*.png.meta`

**Step 1: Patch Main Panel (.meta)**
Sử dụng `writing-skills` hoặc `script-execute` để ghi đè:
- `textureType`: 8 (Sprite)
- `spriteBorder`: `{x: 60, y: 60, z: 60, w: 60}` (Để góc bo gỗ không bị méo)
- `alphaIsTransparency`: 1

**Step 2: Patch Sub Panel & Buttons (.meta)**
Thiết lập Border tương tự (khoảng 30-40px) để đảm bảo co giãn mượt mà.

**Step 3: Refresh Assets**
Run: `assets-refresh` để Unity cập nhật các thay đổi trong Inspector.

### Task 2: Lắp ráp thanh "Gold_Chip" trên HUD

**GameObjects:**
- `[UI_CANVAS]/HUDLayer/HUDTopBar/Gold_Chip` (Background)
- `[UI_CANVAS]/HUDLayer/HUDTopBar/Gold_Chip/Icon` (New Child)

**Step 1: Gán nền cho Gold_Chip**
Thay placeholder bằng `bg_Panel_Sub`. Đặt `Image.type` thành `Sliced`.

**Step 2: Gán Icon Gold**
Tìm hoặc tạo Object con tên là `Icon`. Gán Sprite `icon_Gold` vào.

**Step 3: Căn chỉnh Layout**
Chỉnh `HorizontalLayoutGroup` (Padding, Spacing) để Text và Icon nằm cân đối trên nền giấy mới.

### Task 3: Chụp ảnh kiểm chứng (Final Verification)

**Step 1: Capture Game View**
Sử dụng `screenshot-game-view` để lấy hình ảnh thực tế sau khi lắp ráp.

**Step 2: Walkthrough**
Tổng kết kết quả và gửi carousel so sánh Before/After cho User.
