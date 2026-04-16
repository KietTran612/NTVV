# Requirements: m3a-crop-care-harvest

## Overview

Fix 9 bugs trong crop care và harvest flow. Logic đã tồn tại đầy đủ trong code — spec này chỉ sửa bugs, wire prefab, và verify end-to-end cycle: plant → grow → ailment → care → ripe → harvest → items vào StorageSystem.

**Prerequisite:** `scn-main-world-setup` (M2) phải hoàn thành — CropTile prefab và WorldObjectPicker phải có trong scene.

**Design doc:** `docs/superpowers/specs/2026-04-16-m3a-crop-care-harvest-design.md`

---

## Global Rules

> ⚠️ ĐỌC TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO

- Chỉ sửa đúng những gì bug yêu cầu — không refactor thêm
- `script-read` trước khi sửa bất kỳ file nào
- `assets-refresh` sau mỗi script change, CHỜ compile xong trước khi tiếp tục
- `editor_save_scene` sau mỗi sub-task có Unity change
- Quick Test FAIL → fix trong task hiện tại, KHÔNG sang task tiếp theo (tối đa 2 retry, sau đó escalate)
- **Không viết script mới** — tất cả bugs đều fix trong files hiện có

---

## Functional Requirements

### Req 1 — Care Visual Feedback (BUG-A1, A2)
- 1.1 Sau khi `ClearWeeds()`: `_weedVisual` SetActive(false) **ngay lập tức**, không chờ tick
- 1.2 Sau khi `ClearPests()`: `_bugVisual` SetActive(false) ngay lập tức
- 1.3 Sau khi `WaterPlant()`: `_waterVisual` SetActive(false) ngay lập tức
- 1.4 `ClearWeeds()` và `ClearPests()` không crash khi `StorageSystem.Instance` là null

### Req 2 — State Management (BUG-A3)
- 2.1 Khi tile `NeedsCare` và tất cả ailments đã được clear (drainRate = 0): state tự động reset về `Growing` trong tick tiếp theo
- 2.2 Nếu drainRate = 0 và growthProgress ≥ 1f: state vẫn chuyển sang `Ripe` đúng (không bị override bởi fix 2.1)

### Req 3 — Death Visual (BUG-A10)
- 3.1 Khi tile chuyển từ `Ripe` → `Dead` do hết `LifeAfterRipeInSeconds`: dead sprite/color hiển thị **ngay lập tức** trong cùng tick

### Req 4 — Autosave (BUG-A4)
- 4.1 Khi panel đóng qua nút Close: `GameManager.TriggerSave()` được gọi
- 4.2 Khi player Harvest thành công: `TriggerSave()` được gọi
- 4.3 Khi player Reset tile chết (ClearDead): `TriggerSave()` được gọi
- 4.4 Panel luôn đóng được dù `PopupManager.Instance` null hay không

### Req 5 — Harvest Feedback (BUG-A5)
- 5.1 Khi `StorageSystem.CanAddItem()` trả về false: `Debug.LogWarning` rõ ràng với số lượng và tên crop
- 5.2 Tile không reset nếu harvest fail

### Req 6 — Save/Load Visuals (BUG-A6, A9)
- 6.1 Sau khi `RestoreState()`: ailment visuals (`_weedVisual`, `_bugVisual`, `_waterVisual`) SetActive đúng theo save data **ngay lập tức**
- 6.2 Sau khi `RestoreState()` với tile Ripe: `_currentStage = GrowthStage.Ripe` và visual hiển thị Ripe sprite/màu vàng
- 6.3 Tile Empty sau restore: `_cropRenderer` SetActive(false)

### Req 7 — Null Safety (BUG-A7)
- 7.1 `CropActionPanelController.RefreshUI()` không crash khi `CurrentCropData` là null
- 7.2 Khi cropData null và state không phải Empty: header hiển thị `"[Unknown]"` thay vì crash

### Req 8 — Registry Wiring (WIRE-01)
- 8.1 `CropActionPanelController._registry` trong `ContextActionPanel.prefab` được assign `GameDataRegistry.asset`
- 8.2 `TryAutoPlant()` có thể tìm crop trong registry mà không cần runtime lookup

### Req 9 — Integration
- 9.1 Full cycle hoạt động: plant → grow → ailment xuất hiện → care → Ripe → Harvest → item trong Storage
- 9.2 Save sau harvest → load lại → tile Empty, item vẫn trong Storage
- 9.3 Load tile đang Ripe → visual Ripe đúng → có thể Harvest
- 9.4 Load tile đang NeedsCare → ailment visuals hiển thị đúng → care buttons visible khi tap
