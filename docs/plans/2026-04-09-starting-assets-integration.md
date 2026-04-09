# Starting Farm Assets Integration Plan

> **For Antigravity:** REQUIRED WORKFLOW: Use `.agent/workflows/execute-plan.md` to execute this plan in single-flow mode.

**Goal:** Configure the game registry and UI styling system to support the full life cycle of the Carrot (crop_01) and Chicken (animal_01) using the new Atomic assets.

**Architecture:**
- **Data Layer**: Update `CropDataSO` and `AnimalDataSO` to point to specific sprite names defined in the design doc.
- **Visual Layer**: Update `UIStyleDataSO` (DefaultFarmStyle) with new `SpriteStyleEntry` mappings.
- **Auto-wiring**: Use `UIStyleProcessor` to bake dependencies into UI prefabs.

**Tech Stack:** Unity ScriptableObjects, NTVV UI Styling System.

---

## User Review Required

> [!IMPORTANT]
> **Placeholder Sprites**: Vì chúng ta chưa có các file ảnh thực tế trên đĩa (bạn sẽ tạo sau), tôi sẽ cấu hình bằng cách nhập tên Sprite vào các trường dữ liệu. Điều này có thể gây ra cảnh báo (Warning) trong Console cho đến khi bạn thực sự tạo ảnh.

---

## Proposed Changes

### [Component] Game Data Registry

#### [MODIFY] [crop_01.asset](file:///d:/soflware/Unity/Source/NTVV/Assets/_Project/Data/Crops/crop_01.asset)
- Cập nhật `seedIcon` thành `icon_Seeds_Carrot_Atomic`.
- Cập nhật `cropIcon` thành `icon_Carrot_Stage3_Atomic` (Harvest stage).
- Cập nhật danh sách `growthStageSprites` theo trình tự:
  1. `icon_Seeds_Carrot_Atomic` (Stage 0)
  2. `icon_Carrot_Stage1_Atomic` (Sprout)
  3. `icon_Carrot_Stage2_Atomic` (Growing)
  4. `icon_Carrot_Stage3_Atomic` (Harvest)

#### [MODIFY] [animal_01.asset](file:///d:/soflware/Unity/Source/NTVV/Assets/_Project/Data/Animals/animal_01.asset)
- Cập nhật `icon` thành `icon_Chicken_Atomic`.
- Cập nhật `produceItemId` và `produceIcon` thành `icon_Egg_Atomic`.
- Cập nhật `stageSprites`:
  1. `icon_Chick_Atomic` (Baby)
  2. `icon_Chicken_Atomic` (Adult)

---

### [Component] UI Visual Styling

#### [MODIFY] [DefaultFarmStyle.asset](file:///d:/soflware/Unity/Source/NTVV/Assets/_Project/Settings/UI/DefaultFarmStyle.asset)
- Thêm các `SpriteStyleEntry` mới cho tất cả các asset đã định nghĩa trong Prompt Library (XP, Storage, Gem, Carrot stages, Chicken stages).
- Cấu hình `Pixels Per Unit Multiplier` cho các icon là **1** và các button nền là **5**.

---

## Verification Plan

### Automated Tests
- Chạy `unity-mcp-cli` hoặc script check rỗng để đảm bảo không có ScriptableObject nào bị Reference Null (sau khi bạn đã đưa ảnh vào).

### Manual Verification
- Mở `TopBar_Prefab` và `Inventory_Prefab` trong Unity.
- Kiểm tra bảng `UIStyleApplier` xem các Sprite đã được tự động map đúng tên chưa.
- Sau khi có ảnh, nhấn **[Apply Styles]** trên Controller và kiểm tra hiển thị.
