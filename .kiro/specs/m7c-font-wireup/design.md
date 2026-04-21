# Design Document: m7c-font-wireup

**Date:** 2026-04-21
**Spec:** m7c-font-wireup
**Status:** approved
**Full design:** `docs/superpowers/specs/2026-04-21-m7c-font-wireup-design.md`

---

## Overview

13 TextMeshPro (TMP) component dùng LiberationSans SDF (TMP default) thay vì Dosis-Bold SDF chuẩn. Fix bằng direct YAML edit — thay 2 dòng `m_fontAsset` + `m_sharedMaterial` của mỗi TMP.

**Phạm vi:**
- 1 scene (`SCN_Main.unity`)
- 7 prefab (InventorySlot, QuestListItem, UI_Nav_Button, UI_XP_ProgressBar, LockInfoPopup, QuestPopup, ShopPopup)
- Tổng 13 TMP component

---

## YAML Replace Pattern

**Cũ (LiberationSans SDF):**
```yaml
  m_fontAsset: {fileID: 11400000, guid: 8f586378b4e144a9851e7b34d9b748ee, type: 2}
  m_sharedMaterial: {fileID: 2180264, guid: 8f586378b4e144a9851e7b34d9b748ee, type: 2}
```

**Mới (Dosis-Bold SDF):**
```yaml
  m_fontAsset: {fileID: 11400000, guid: 452c63e094a208745841fe3a5aeb7642, type: 2}
  m_sharedMaterial: {fileID: -3612796072522039072, guid: 452c63e094a208745841fe3a5aeb7642, type: 2}
```

**Lưu ý:**
- 2 dòng này xuất hiện LIỀN KỀ trong YAML của mỗi TMP component
- Indentation 2 space (giữ nguyên)
- `m_fontAsset.fileID`: `11400000` giữ nguyên (standard cho TMP font asset)
- `m_sharedMaterial.fileID`: đổi `2180264` (LiberationSans material subasset) → `-3612796072522039072` (Dosis-Bold material subasset)
- GUIDs đổi cả 2 field

---

## Tool Choice: `Edit.replace_all`

**Lý do không dùng Unity MCP:**
- Mỗi file có số lượng TMP khác nhau (1-4 component)
- Pattern YAML cố định → direct edit an toàn
- Không cần Unity Editor mở → faster execution
- Git diff clean, dễ review

**Lý do `replace_all: true`:**
- Trong 1 prefab có 2-4 TMP cùng LiberationSans → cần thay tất cả trong 1 lần Edit call
- Pattern exact match (2 dòng liền kề + GUID cố định) → an toàn không false positive

---

## File-by-File Edit Plan

### File 1: `SCN_Main.unity` (1 TMP)
Edit 1 block tại/gần line 4230. `replace_all: false` (chỉ 1 occurrence).

### File 2: `InventorySlot.prefab` (1 TMP)
Line 228. `replace_all: false`.

### File 3: `QuestListItem.prefab` (4 TMP)
Lines 167, 305, 523, 680. `replace_all: true` — đổi tất cả.

### File 4: `UI_Nav_Button.prefab` (1 TMP)
Line 305. `replace_all: false`.

### File 5: `UI_XP_ProgressBar.prefab` (1 TMP)
Line 271. `replace_all: false`.

### File 6: `LockInfoPopup.prefab` (2 TMP)
Lines 70, 207. `replace_all: true`.

### File 7: `QuestPopup.prefab` (1 TMP)
Line 124. `replace_all: false`.

### File 8: `ShopPopup.prefab` (2 TMP)
Lines 1544, 2622. `replace_all: true`.

---

## Edge Cases

### 1. Mixed-font prefabs (ShopPopup, InventorySlot)
Các prefab này đã có TMP dùng Dosis-Bold SDF (đúng) lẫn TMP dùng LiberationSans (sai). Vì pattern replace_all chỉ match block LiberationSans → các TMP Dosis-Bold không bị ảnh hưởng (GUID khác).

**Example:** `ShopPopup.prefab` có 5 TMP Dosis + 2 TMP LiberationSans. Edit sẽ chỉ thay 2 block LiberationSans.

### 2. GUID LiberationSans xuất hiện ở chỗ khác ngoài TMP?
Grep verified: GUID `8f586378...` chỉ xuất hiện trong `m_fontAsset` + `m_sharedMaterial` context. Không có `m_Font` (legacy Text) dùng GUID này. An toàn replace.

### 3. TMP với material override (custom material)
Không gặp trong 13 locations. Tất cả đều dùng default material của font asset. Nếu có case này trong tương lai, `m_sharedMaterial` sẽ trỏ file .mat riêng, không phải subasset → sẽ bị miss. Cần kiểm riêng.

---

## Critical Design Decisions

| Decision | Lý do |
|----------|--------|
| Direct YAML edit thay vì Unity MCP | Pattern cố định, không cần Editor mở, git diff clean |
| `replace_all: true` cho multi-TMP prefabs | Exact 2-dòng block, an toàn không false positive |
| Không touch Dosis-ExtraBold (2 TMP ShopEntry_Seed) | Có thể intentional (bold hơn cho item name) |
| Fix material fileID (-3612796072522039072) | Subasset khác nhau giữa font — không fix → render sai |
| 0 touch shader reference | Shader nằm inside material subasset → tự động |

---

## Execution Order

1. Task 0: Prerequisite — verify đủ 13 vị trí (grep count)
2. Task 1: Fix SCN_Main.unity
3. Task 2: Fix 4 prefab UI/Components (InventorySlot, QuestListItem, UI_Nav_Button, UI_XP_ProgressBar)
4. Task 3: Fix 3 prefab Resources/UI/Default (LockInfoPopup, QuestPopup, ShopPopup)
5. Task 4: Post-verify — grep GUID LiberationSans = 0, grep GUID Dosis-Bold = 58
6. Task 5: Integration smoke test Play mode
7. Task 6: HANDOVER + commit
