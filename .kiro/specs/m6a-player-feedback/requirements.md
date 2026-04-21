# Requirements: m6a-player-feedback

## Overview

Hoàn thiện vòng feedback của player với 2 features: level-up toast notification và gems system cho Shop Refresh. Cả hai tập trung vào layer UI/economy mà chưa có ai subscribe event hoặc persist.

**Prerequisite:** `m5-quest-flow` hoàn thành. `LevelSystem`, `EconomySystem`, `ShopPanelController` đã hoạt động ổn định.

**Design doc:** `docs/superpowers/specs/2026-04-20-m6-gameplay-features-design.md`

---

## Global Rules

> ⚠️ ĐỌC TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO

- `script-read` trước khi sửa bất kỳ file nào
- `assets-refresh` sau mỗi script change, CHỜ compile xong trước khi tiếp tục
- `scene-save` sau mỗi thay đổi scene/prefab
- Quick Test FAIL → fix trong task hiện tại, KHÔNG sang task tiếp theo (tối đa 2 retry)
- Không refactor code ngoài scope đã design
- KHÔNG dùng `UIAnimationHelper.PopIn()` cho toast — `StopAllCoroutines()` gây conflict

---

## Functional Requirements

### Req 1 — Level-up Toast (FEAT-01)
- 1.1 Khi player lên cấp, hiện toast "⬆ Lên cấp {X}!" trên HUD
- 1.2 Toast fade out sau 2 giây và ẩn (SetActive false)
- 1.3 Toast subscribe `LevelSystem.OnLevelUp` trong `OnEnable`, unsubscribe trong `OnDisable`
- 1.4 Toast hỗ trợ `ShowMessage(string msg)` để tái sử dụng cho các thông báo khác
- 1.5 Fade dùng `CanvasGroup.alpha` trực tiếp, không qua `UIAnimationHelper`
- 1.6 Không cần queue — nếu toast đang hiện thì show lại từ đầu

### Req 2 — Gems Save/Load (FEAT-07 — foundation)
- 2.1 `PlayerSaveData` có field `gems` lưu số gems hiện tại
- 2.2 Default khi không có save: `gems = 25`
- 2.3 `GameManager.InitializeCoreSystems()` gọi `EconomySystem.Instance.SetGems(data.gems)`
- 2.4 `GameManager.CaptureCurrentState()` lưu `data.gems = EconomySystem.Instance.CurrentGems`
- 2.5 Sau save + restart, gems giữ nguyên

### Req 3 — Gems → Shop Refresh (FEAT-07 — logic)
- 3.1 `Refresh_Button` trong ShopPopup dùng gems, không dùng gold
- 3.2 Giá refresh = 50 gems (configurable qua `_refreshCostGems`)
- 3.3 Nếu đủ gems → trừ gems, gọi `PopulateShop()`
- 3.4 Nếu không đủ gems → `Debug.LogWarning`, không làm gì
- 3.5 `GemsBalance_Label` hiển thị số gems hiện tại đúng lúc
- 3.6 `Refresh_Button` có label text rõ ràng (ví dụ: "Làm mới (50💎)")

### Req 4 — Integration
- 4.1 AddXP đủ lên cấp → toast xuất hiện trong HUD
- 4.2 Mở Shop → tap Refresh → gems giảm, shop list reload
- 4.3 Save + restart → gems giữ nguyên, không reset về 25
- 4.4 Console: 0 NullReferenceException liên quan toast / gems
