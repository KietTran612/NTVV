# Requirements: m7c-font-wireup

## Overview

Fix 13 TextMeshPro component đang dùng LiberationSans SDF (TMP default) → chuyển sang `Dosis-Bold SDF.asset` (font chuẩn dự án). Đồng bộ cả `m_fontAsset` và `m_sharedMaterial` để render đúng.

**Prerequisite:** Không phụ thuộc m7a/m7b. Chạy độc lập được.

**Design doc:** `docs/superpowers/specs/2026-04-21-m7c-font-wireup-design.md`

---

## Global Rules

> ⚠️ ĐỌC TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO

- Dùng `Edit` tool với `replace_all: true` để đổi pattern YAML (an toàn, exact match)
- PHẢI đổi cả `m_fontAsset` VÀ `m_sharedMaterial` (2 dòng liền kề) — nếu chỉ đổi 1 → render sai
- PHẢI đổi `m_sharedMaterial.fileID` từ `2180264` → `-3612796072522039072` (subasset khác nhau giữa 2 font)
- `assets-refresh` sau khi edit batch prefabs (để Unity re-import)
- Quick Test FAIL → fix trong task hiện tại, KHÔNG sang task tiếp theo
- KHÔNG đổi 2 TMP đang dùng Dosis-ExtraBold ở `ShopEntry_Seed.prefab` (intentional)
- KHÔNG thay font size/color/style

---

## Functional Requirements

### Req 1 — Font Assets
- 1.1 Project có `Assets/_Project/Fonts/Dosis/Dosis-Bold SDF.asset` (GUID `452c63e094a208745841fe3a5aeb7642`)
- 1.2 Project có `Dosis-ExtraBold SDF.asset` (GUID `7beab8d0ea0e9414e84fb1aa3eb3423f`) — không touch ở spec này
- 1.3 LiberationSans SDF (GUID `8f586378b4e144a9851e7b34d9b748ee`) là TMP default — sẽ bị loại khỏi tất cả asset NTVV

### Req 2 — Scene Fix (1 TMP)
- 2.1 `SCN_Main.unity` line 4230: `m_fontAsset` và `m_sharedMaterial` đổi LiberationSans → Dosis-Bold SDF
- 2.2 Sau fix: scene mở trong Unity không có warning "Missing font" hay "Missing material"

### Req 3 — Prefabs UI/Components (7 TMP trong 4 prefab)
- 3.1 `InventorySlot.prefab` (1 TMP): đổi font
- 3.2 `QuestListItem.prefab` (4 TMP): đổi font toàn bộ 4 component trong prefab
- 3.3 `UI_Nav_Button.prefab` (1 TMP): đổi font
- 3.4 `UI_XP_ProgressBar.prefab` (1 TMP): đổi font

### Req 4 — Prefabs Resources/UI/Default (5 TMP trong 3 prefab)
- 4.1 `LockInfoPopup.prefab` (2 TMP — Message_Label + OK_Label): đổi font
- 4.2 `QuestPopup.prefab` (1 TMP): đổi font
- 4.3 `ShopPopup.prefab` (2 TMP): đổi font

### Req 5 — Verification
- 5.1 `grep -rn "guid: 8f586378b4e144a9851e7b34d9b748ee" Assets/_Project/` → **0 match**
- 5.2 `grep -rn "fileID: 2180264" Assets/_Project/` → **0 match** (material LiberationSans)
- 5.3 `grep -rn "m_fontAsset: .*guid: 452c63e0" Assets/_Project/ | wc -l` → **58 match** (45 cũ + 13 mới)
- 5.4 Unity console: 0 "Missing font", 0 "Could not find material" warning
- 5.5 Play mode: visual confirm 13 vị trí hiển thị font Dosis-Bold đúng
