# Requirements: m6b-world-progression

## Overview

Hoàn thiện gameplay loop với 2 features: offline growth fix cho animal (FEAT-05) và land expansion với locked tile system (FEAT-06). Cả hai ảnh hưởng `GameManager`, `CropTileView`, và `WorldObjectPicker`.

**Prerequisite:** `m6a-player-feedback` hoàn thành. `LevelUpToastController.ShowMessage()` cần có sẵn cho welcome toast.

**Design doc:** `docs/superpowers/specs/2026-04-20-m6-gameplay-features-design.md`

---

## Global Rules

> ⚠️ ĐỌC TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO

- `script-read` trước khi sửa bất kỳ file nào
- `assets-refresh` sau mỗi script change, CHỜ compile xong trước khi tiếp tục
- `scene-save` sau mỗi thay đổi scene
- Quick Test FAIL → fix trong task hiện tại, KHÔNG sang task tiếp theo (tối đa 2 retry)
- Không refactor code ngoài scope đã design
- Không thêm quest content mới hay map mới trong spec này

---

## Functional Requirements

### Req 1 — Offline Growth Animal Fix (FEAT-05)
- 1.1 Sau khi restart game, animal tiến triển đúng dựa trên thời gian thực tế offline
- 1.2 `AnimalView.RestoreState()` sử dụng `GameManager.LastSaveTime` được set từ `data.lastSaveTimestamp`, không phải `DateTime.UtcNow`
- 1.3 `GameManager.BootSequence()` set `LastSaveTime = new DateTime(data.lastSaveTimestamp)` trước khi gọi `RestoreWorldState()`
- 1.4 Crops không thay đổi — `CropTileView.RestoreState()` đã xử lý đúng qua `plantTimestamp`

### Req 2 — Welcome Toast (FEAT-05)
- 2.1 Nếu offline > 60 giây, hiện toast "Chào mừng trở lại! Farm đã tiến triển trong {X} giờ {Y} phút"
- 2.2 Dùng lại `LevelUpToastController.ShowMessage()` từ m6a
- 2.3 Gọi sau `RestoreWorldState()` trong `BootSequence`
- 2.4 Nếu offline ≤ 60 giây → không hiện toast

### Req 3 — Locked Tile System (FEAT-06)
- 3.1 `CropTileView` có `_isLocked` và `_requiredLevel` configurable qua Inspector
- 3.2 Tile đang locked không xử lý `HandleTick()` (không grow, không ailment)
- 3.3 Tile đang locked hiển lock overlay sprite (nếu có assign)
- 3.4 Tap vào tile locked → hiện popup "Cần Level X để mở tile này"
- 3.5 Popup có nút OK đóng modal
- 3.6 Tile locked không mở `CropActionPanel` (không cho tương tác như bình thường)

### Req 4 — Tile Auto-unlock theo Level (FEAT-06)
- 4.1 Khi player lên cấp, tất cả tile có `_requiredLevel <= newLevel` được tự unlock
- 4.2 Sau unlock: overlay biến mất (`SetActive(false)`), tile chạy tick bình thường
- 4.3 `TriggerSave()` sau khi unlock tile

### Req 5 — Locked Tile Save/Load (FEAT-06)
- 5.1 `PlayerSaveData` lưu `unlockedTileIds: List<string>` — danh sách tile đã unlock
- 5.2 Tile đã unlock: sau restart vẫn unlock
- 5.3 Tile chưa unlock: sau restart vẫn locked
- 5.4 Tile mặc định `_isLocked = false`: lưu vào `unlockedTileIds` bình thường

### Req 6 — LockInfoPopup
- 6.1 `LockInfoPopup.prefab` tồn tại trong `Resources/UI/Default/`
- 6.2 Hiển text dựa trên `requiredLevel` được inject khi spawn
- 6.3 Nút OK → `PopupManager.Instance.CloseActiveModal()`

### Req 7 — Integration
- 7.1 Save, đồng hồ tiến thêm, restart → animal hunger/growth đúng
- 7.2 Welcome toast hiện nếu offline > 60s
- 7.3 Tile locked: tap → popup, không mở action panel
- 7.4 Lên cấp đủ → tile tự unlock, overlay biến mất
- 7.5 Save/load: locked state persist đúng
- 7.6 Console: 0 NullReferenceException liên quan offline growth / tile unlock
