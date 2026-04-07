# HUD Rebirth: Kawaii Pure Canvas Implementation Plan

> **For Antigravity:** Use `.agent/workflows/execute-plan.md` to execute this plan in single-flow mode.

**Goal:** Loại bỏ hoàn toàn sự phụ thuộc vào Prefab cũ đang bị lỗi hiển thị. Xây dựng lại HUD từ các mảnh ghép Atomic (PNG -> Sprite) trực tiếp trên Scene để đảm bảo tính mỹ thuật "Kawaii" và kết nối logic điều khiển.

**Architecture:**
- **Canvas Root**: Create a fresh `[UI_NEW_CANVAS]` with `CanvasScaler` (1920x1080).
- **Atomic Layers**: Xây dựng UI theo từng cấp độ: Background -> Icon -> Text.
- **Controller Re-wiring**: Gán thủ công `HUDTopBarController` và `UIResourceChip` vào các Object mới.

---

### Task 1: Dọn dẹp Scene (Cleanup)
- **Hành động**: Xóa GameObject `[UI_CANVAS]` hiện tại trong Scene `SCN_Gameplay`.
- **Lý do**: Triệt tiêu các Prefab bị đè dữ liệu (broken overrides) và các Script Style cũ gây lỗi màu hồng.

### Task 2: Xây dựng HUD Top Bar (Top Level)
- **Hành động**:
    1. Tạo `HUDTopBar` (Empty + HorizontalLayoutGroup).
    2. Tạo `Gold_Chip`:
        - Nền: `bg_Panel_Sub` (Sliced).
        - Con 1: `Resource_Icon` (Image: `icon_Gold`).
        - Con 2: `Amount_Label` (TextMeshPro).
    3. Gán Component `UIResourceChip` vào `Gold_Chip` và kéo thả Reference.
    4. Lặp lại cho `Storage_Chip` (Dùng `icon_Energy`).

### Task 3: Xây dựng Bottom Navigation (Bottom Level)
- **Hành động**:
    1. Tạo `BottomNav` (Empty + HorizontalLayoutGroup).
    2. Tạo các nút: `Btn_Farm`, `Btn_Shop`, `Btn_Storage`.
    3. Gán các Sprite tương ứng: `bg_Button_Green`, `bg_Button_Blue`, `bg_Button_Yellow`.
    4. Căn chỉnh khoảng cách để tạo cảm giác "bồng bềnh" (Floating).

### Task 4: Kết nối Logic Hệ thống (System Wiring)
- **Hành động**:
    1. Gán `HUDTopBarController` vào `HUDTopBar`.
    2. Gán các nhãn Text của Gold, Storage vào Controller.
    3. Kiểm tra logic nhảy số (EconomySystem) để đảm bảo khi nhận Vàng, Text vẫn cập nhật.

### Task 5: Kiểm chứng & Lưu trữ
- **Hành động**:
    1. Chụp ảnh màn hình Canvas mới.
    2. Lưu Scene.
