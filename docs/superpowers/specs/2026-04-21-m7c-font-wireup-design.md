# Design: m7c-font-wireup

**Ngày**: 2026-04-21
**Trạng thái**: Approved
**Liên quan**: `m7a-sprite-reorg`, `m7b-sprite-wireup` (parent specs)

---

## Context

Audit font toàn bộ scene + prefab phát hiện **13 TextMeshPro component đang dùng LiberationSans SDF** (default TMP shipped font) thay vì font chuẩn dự án `Dosis-Bold SDF.asset`. Gây visual inconsistency: 45 TMP Dosis-Bold + 2 Dosis-ExtraBold + 13 LiberationSans.

Nguyên nhân: các prefab/scene object được tạo qua Unity Editor mà quên set font → TMP dùng default.

---

## Font Assets

| Purpose | File | GUID | Material fileID |
|---------|------|------|-----------------|
| **Desired (primary)** | `Assets/_Project/Fonts/Dosis/Dosis-Bold SDF.asset` | `452c63e094a208745841fe3a5aeb7642` | `-3612796072522039072` |
| Acceptable (header) | `Assets/_Project/Fonts/Dosis/Dosis-ExtraBold SDF.asset` | `7beab8d0ea0e9414e84fb1aa3eb3423f` | (không dùng trong m7c) |
| **WRONG (default)** | `Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF.asset` | `8f586378b4e144a9851e7b34d9b748ee` | `2180264` |

**Quyết định (hướng A):** Tất cả 13 TMP sai → Dosis-Bold SDF (đơn giản, đồng nhất). Không phân biệt header/body ở spec này. Nếu sau muốn visual hierarchy, làm spec typography riêng.

---

## 13 Vị trí cần fix

### Scene (1)
- `Assets/_Project/Scenes/SCN_Main.unity:4230`

### Prefabs UI/Components (7)
- `Prefabs/UI/Components/InventorySlot.prefab:228` (1 TMP)
- `Prefabs/UI/Components/QuestListItem.prefab:167, 305, 523, 680` (4 TMP)
- `Prefabs/UI/Components/UI_Nav_Button.prefab:305` (1 TMP)
- `Prefabs/UI/Components/UI_XP_ProgressBar.prefab:271` (1 TMP)

### Prefabs Resources/UI/Default (5)
- `Resources/UI/Default/LockInfoPopup.prefab:70, 207` (2 TMP — vừa tạo ở m6b)
- `Resources/UI/Default/QuestPopup.prefab:124` (1 TMP)
- `Resources/UI/Default/ShopPopup.prefab:1544, 2622` (2 TMP)

---

## Migration Strategy — Direct YAML Edit

**Approach**: Edit tool replace_all trên từng file. An toàn vì:
- Pattern đổi là **2 dòng cố định** (m_fontAsset + m_sharedMaterial) với GUID + fileID đã verify
- Không cần Unity Editor mở
- Git diff rõ ràng, dễ review & rollback

**Pattern cũ** (cần thay):
```yaml
m_fontAsset: {fileID: 11400000, guid: 8f586378b4e144a9851e7b34d9b748ee, type: 2}
m_sharedMaterial: {fileID: 2180264, guid: 8f586378b4e144a9851e7b34d9b748ee, type: 2}
```

**Pattern mới**:
```yaml
m_fontAsset: {fileID: 11400000, guid: 452c63e094a208745841fe3a5aeb7642, type: 2}
m_sharedMaterial: {fileID: -3612796072522039072, guid: 452c63e094a208745841fe3a5aeb7642, type: 2}
```

**Chú ý quan trọng:**
- `m_sharedMaterial.fileID` **PHẢI đổi** từ `2180264` (LiberationSans material) sang `-3612796072522039072` (Dosis-Bold material). Nếu chỉ đổi GUID mà không đổi fileID → material sẽ "Missing" hoặc vẫn render sai.
- `m_fontAsset.fileID` giữ nguyên `11400000` (TMP font asset luôn dùng fileID này).
- Sau khi đổi material, TMP sẽ pick up shader đúng (`68e6db2e...` của Dosis) tự động vì shader reference nằm inside material subasset.

---

## Risk & Mitigation

| Rủi ro | Mitigation |
|--------|-----------|
| Typo GUID khi Edit → TMP "Missing Font" | Dùng `Edit.replace_all` với exact 2-dòng match để tránh nhầm vị trí |
| Miss 1 TMP (grep lọt) | Task verify cuối cùng: grep LiberationSans GUID → 0 match |
| Shader/material render khác | Dosis material subasset đã setup sẵn (test được sau Play mode) |
| File đang checkout bởi Unity → write conflict | Prerequisite: đóng Unity Editor hoặc không mở các prefab đang edit |

---

## Verification

1. `grep -rn "guid: 8f586378b4e144a9851e7b34d9b748ee" Assets/_Project/` → **0 match** (không còn LiberationSans)
2. `grep -rn "m_fontAsset: .*guid: 452c63e0" Assets/_Project/ | wc -l` → **58 match** (45 cũ + 13 mới fix)
3. `grep -rn "fileID: 2180264" Assets/_Project/` → **0 match** (không còn material LiberationSans)
4. Mở Unity → 13 TMP hiển thị đúng font Dosis-Bold
5. Play mode: HUD, Shop, Storage, QuestList, XP bar, LockInfoPopup → toàn bộ chữ là Dosis-Bold

---

## Scope KHÔNG làm

- Không đổi 2 TMP đang dùng Dosis-ExtraBold (`ShopEntry_Seed.prefab`) — có thể intentional
- Không thay font color/size/style của các TMP
- Không thêm visual hierarchy (ExtraBold cho headings) — để spec riêng sau
