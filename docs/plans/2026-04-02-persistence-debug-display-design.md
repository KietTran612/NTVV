# Thiết kế: Hiển thị thông tin Debug cho Hệ thống Persistence

## Mô tả
Cải tiến `PersistenceTestHarness` để hiển thị các thông tin quan trọng như đường dẫn file save, dữ liệu chuẩn bị lưu và dữ liệu vừa tải lên trực tiếp trong Unity Inspector.

## Thay đổi đề xuất

### 1. [SaveLoadManager](file:///d:/soflware/Unity/Source/NTVV/Assets/_Project/Scripts/Managers/SaveLoadManager.cs)
- Thay đổi phạm vi truy cập của `SavePath` từ `private` thành `public`.

### 2. [PersistenceTestHarness](file:///d:/soflware/Unity/Source/NTVV/Assets/_Project/Scripts/Testing/PersistenceTestHarness.cs)
- Thêm các trường dữ liệu mới:
    - `public string activeSavePath`: Hiển thị đường dẫn file.
    - `public PlayerSaveData lastSaveSnapshot`: Chụp ảnh dữ liệu hệ thống trước khi nhấn Save.
    - `public PlayerSaveData lastLoadSnapshot`: Chụp ảnh dữ liệu được load từ file.
- Cập nhật phương thức:
    - `Start()`: Gán `activeSavePath = SaveLoadManager.Instance.SavePath`.
    - `ManualSave()`: Thu thập dữ liệu hiện tại vào `lastSaveSnapshot` trước khi gọi Save.
    - `ManualLoad()`: Gán dữ liệu load được vào `lastLoadSnapshot`.

## Kết quả mong đợi
- Người dùng có thể kiểm soát hoàn toàn dữ liệu persistence mà không cần mở file JSON hay kiểm tra log phức tạp.
- Dễ dàng phát hiện lỗi sai lệch dữ liệu giữa Runtime và Local Storage.
