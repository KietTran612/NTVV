# Kawaii Atomic Assets Implementation Plan

**Goal:** Chuyển đổi từ "UI đúc sẵn" sang "UI nguyên tử". Tạo ra các bộ Background trống và Icon riêng biệt để đảm bảo hiển thị hoàn hảo trên các lớp GameObject khác nhau.

---

### Task 1: Tạo Bộ Nền Trống (Empty Backgrounds)
- **Hành động**: Sử dụng `generate_image` với prompt tập trung vào "Empty UI Frame", "Blank Button", "No icons inside".
- **Assets cần tạo**:
    - `bg_Button_Green_Empty`
    - `bg_Button_Blue_Empty`
    - `bg_Button_Yellow_Empty`
    - `bg_Panel_Sub_Empty`

### Task 2: Tạo Bộ Icon Thuần (Pure Icons)
- **Hành động**: Tạo các icon riêng biệt với phong cách Kawaii 3D glossy.
- **Assets cần tạo**:
    - `icon_Gold_Pure`
    - `icon_Energy_Pure`
    - `icon_Farm_Pure`
    - `icon_Shop_Pure`

### Task 3: Xử lý Kỹ thuật (Remove Background & Slicing)
- **Hành động**: 
    - Chạy lại script "Tẩy nền" cho bộ Asset mới.
    - Thiết lập 9-slicing cho các khung nền trống để có thể co giãn thoải mái.

### Task 4: Lắp ráp & Wiring 2.0
- **Hành động**: 
    - Cập nhật hierarchy `[UI_NEW_CANVAS]`.
    - Gán Background trống vào lớp nền.
    - Gán Icon thuần vào lớp `Resource_Icon`.
    - Sử dụng `TextMeshPro` cho toàn bộ văn bản.

---

### Verification Plan
- Chụp ảnh màn hình so sánh giữa "UI đúc sẵn" và "UI lắp ghép".
- Kiểm tra tính linh hoạt: Thử thay đổi Text trong Unity xem có bị chồng chữ cũ không.
