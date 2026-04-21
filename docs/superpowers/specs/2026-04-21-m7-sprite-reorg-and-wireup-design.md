# Design: m7a-sprite-reorg + m7b-sprite-wireup

**Ngày**: 2026-04-21
**Trạng thái**: Approved
**Liên quan**: `docs/asset-prompts/2026-04-17-asset-inventory-all.md`, `docs/asset-prompts/2026-04-17-rework/master-index.md`

---

## Context

Sau đợt thêm sprite (Potato, Corn, Wheat, Tomato, Strawberry, Pumpkin — 30 sprite; Duck — 4 sprite; Carrot_Dead, Chicken_Dead — 2 sprite; Grass, Nav Shop/Event — 3 sprite; World overlay rework — 4 sprite), codebase có 2 vấn đề:

1. **Mixed naming convention** — song song 2 chuẩn: old `icon_*_Atomic.png` vs new `World_[Cat]_[Entity]_[Variant]_[State].png`. Rối khi tìm/maintain.
2. **Chưa wire vào SO** — `crop_01..07.asset` và `animal_01..02.asset` hầu hết còn `{fileID: 0}` cho `growthStageSprites`, `deadSprite`, `stageSprites`, `readyToCollectIcon`. Chicken SO chưa wire gì cả.

Chia thành 2 sub-spec theo thứ tự phụ thuộc:

- **m7a-sprite-reorg**: rename + tổ chức folder theo chuẩn rework duy nhất (prerequisite)
- **m7b-sprite-wireup**: wire sprite vào SO/Prefab + cleanup duplicates (sau m7a)

---

## Naming Convention (unified)

**Format**: `[Domain]_[Category]_[Entity]_[Variant]_[State].png`

| Part | Values | Ví dụ |
|------|--------|-------|
| Domain | `World` \| `UI` | World, UI |
| Category | `Crop`, `Animal`, `Product`, `Overlay`, `Tile`, `Background`, `Icon`, `Button` | Crop, Icon |
| Entity | Tên object | Carrot, Chicken, Gold, Farm |
| Variant | Sub-type/role | Body, Seed, Nav, Common, Action, Tab |
| State | Trạng thái visual | Stage00, Dead, Default, On, Ready |

**Quy tắc:**
- Stage index 0-based: `Stage00`, `Stage01`, `Stage02`, `Stage03` (crops 4 stage, animals 3 stage)
- Single-state icons dùng suffix `Default`
- KHÔNG dùng `_Atomic`, `_v01`, PascalCase/snake_case lẫn lộn
- Không dùng số đuôi `_1`, `_2` cho duplicate — xóa duplicate

---

## Folder Structure (new)

```
Assets/_Project/Art/Sprites/
├── World/
│   ├── Crops/
│   │   ├── Carrot/       (World_Crop_Carrot_Body_Stage00-03.png + Dead.png)
│   │   ├── Corn/
│   │   ├── Potato/
│   │   ├── Pumpkin/
│   │   ├── Strawberry/
│   │   ├── Tomato/
│   │   └── Wheat/
│   ├── Animals/
│   │   ├── Chicken/      (World_Animal_Chicken_Body_Stage00-02.png + Dead.png)
│   │   └── Duck/         (World_Animal_Duck_Body_Stage00-02.png + Dead.png)
│   ├── Products/         (World_Product_Egg_Collect_Ready.png + DuckEgg)
│   ├── Overlays/         (World_Overlay_Tile_Pest/Weed/WaterNeed_On.png)
│   └── Tiles/            (World_Tile_Soil_Base_Default.png)
└── UI/
    ├── Backgrounds/      (UI_Background_Panel_Main_Default.png, Button_Blue/Green/Purple/Red, Banner_Parchment, Chip_Resource, Plaque_Wooden)
    ├── Icons/
    │   ├── Common/       (UI_Icon_Common_Gold/XP/Gem/Storage/Energy/Grass/Worm_Default.png)
    │   ├── Seed/         (UI_Icon_Seed_Carrot/Potato/Corn/Wheat/Tomato/Strawberry/Pumpkin_Default.png)
    │   ├── Nav/          (UI_Icon_Nav_Farm/Shop/Storage/Barn/Event_Default.png)
    │   ├── Tab/          (UI_Icon_Tab_Leaf/Star_Default.png)
    │   ├── Header/       (UI_Icon_Header_Sprout_Default.png)
    │   └── Action/       (UI_Icon_Action_Water/Refresh_Default.png)
    └── Buttons/          (UI_Button_Close_Circle_Default.png)
```

**Loại bỏ:**
- `UI/Legacy/` (4 sprite không dùng — Apple, Carrot-flat, Sprout-flat, Wheat-flat)
- `UI/Icons/Animals/` (chuyển sang `World/Animals/`)
- Duplicates: `icon_Feed_Worm_Atomic_1.png`
- Old world sprites lowercase sau khi repoint: `soil_empty.png`, `weed_overlay.png`, `bug_overlay.png`, `water_needed.png`

---

## Migration Strategy (m7a)

**Unity GUID preservation:** Dùng `assets-move` (Unity API) — `.meta` đi theo file, GUID không đổi, tất cả reference trong SO/Prefab/Scene giữ nguyên.

**Batch theo thứ tự:**
1. Tạo folder structure mới (empty)
2. Move + rename World/Crops (Carrot rename 6 file, 6 loại khác move folder path)
3. Move + rename World/Animals (Chicken rename 4, Duck move từ UI/Icons/Animals/)
4. Move + rename World/Products (Egg rename, DuckEgg move)
5. World/Overlays + Tiles (new files đã đúng naming, chỉ move)
6. UI/Backgrounds (8 rename)
7. UI/Icons/Common + Seed + Nav + Tab + Header + Action (~22 rename)
8. UI/Buttons (1 rename)
9. `assets-refresh` sau mỗi batch
10. **Không xóa** old duplicates ở bước này — để m7b xử lý sau khi wire xong

**Rủi ro & mitigation:**
- Unity import race → `assets-refresh` giữa batches
- Reference mất → GUID preservation đảm bảo; verify bằng console 0 "missing sprite" warning
- Rollback: git tracked, có thể `git checkout` toàn bộ Art/Sprites/ nếu vỡ

---

## Wire-up Strategy (m7b)

**Bước 1: CropDataSO (7 file `crop_01..07.asset`)**

Mỗi SO cần wire:
- `growthStageSprites[0..3]` → 4 sprite Stage00-03
- `deadSprite` → 1 sprite Dead
- `seedIcon` → icon seed riêng
- `cropIcon` → reuse Stage03 (assign cùng GUID)

Dùng `assets-modify` với cấu trúc:
```yaml
growthStageSprites:
  - {fileID: 21300000, guid: <Stage00_GUID>, type: 3}
  - {fileID: 21300000, guid: <Stage01_GUID>, type: 3}
  - {fileID: 21300000, guid: <Stage02_GUID>, type: 3}
  - {fileID: 21300000, guid: <Stage03_GUID>, type: 3}
deadSprite: {fileID: 21300000, guid: <Dead_GUID>, type: 3}
```

**Bước 2: AnimalDataSO (2 file `animal_01..02.asset`)**

Mỗi SO cần wire:
- `stageSprites[0..2]` → 3 sprite Stage00-02
- `deadSprite` → 1 sprite Dead
- `readyToCollectIcon` → product sprite (Egg / DuckEgg)

**Bước 3: CropTile prefab — overlay sprite references**

Prefab `CropTile` có 4 SpriteRenderer children cần swap old → new:
- `_soilRenderer.sprite` → `World_Tile_Soil_Base_Default.png`
- `_weedVisual.sprite` → `World_Overlay_Tile_Weed_On.png`
- `_bugVisual.sprite` → `World_Overlay_Tile_Pest_On.png`
- `_waterVisual.sprite` → `World_Overlay_Tile_WaterNeed_On.png`

Dùng `gameobject-component-modify` sau khi `assets-prefab-open`.

**Bước 4: BottomNav button sprites**

Trong `SCN_Main.unity`, 2 nút Nav đang placeholder — wire sprite mới:
- `NavBtn_Shop.Image.sprite` → `UI_Icon_Nav_Shop_Default.png`
- `NavBtn_Event.Image.sprite` → `UI_Icon_Nav_Event_Default.png`

**Bước 5: Cleanup**
- Delete `Art/Sprites/UI/Legacy/` (4 file)
- Delete old lowercase World sprites sau khi CropTile prefab đã repoint
- Delete duplicate `icon_Feed_Worm_Atomic_1.png`

**Bước 6: Smoke test**
- Play mode → tap 7 crop tile khác nhau → verify growth sprite hiện đúng
- Plant seed → verify seedling sprite hiện trên tile
- Force weed/pest/water → verify overlay hiện đúng
- AnimalPen: verify 3 stage + dead sprite render đúng
- Console: 0 "missing sprite" warning

---

## Files ảnh hưởng

**Renamed/Moved:** ~75 sprite file + 75 meta file
**Modified:** 9 SO asset + 1 prefab (CropTile) + 1 scene (SCN_Main)
**Deleted:** ~10 file (Legacy + duplicates)

---

## Verification

**m7a-sprite-reorg:**
1. `Art/Sprites/` chỉ có 2 folder top-level: `World/` và `UI/`
2. 0 file naming cũ `icon_*_Atomic.png` ngoài seed/common icons
3. 0 file lowercase trong `World/` (trừ old duplicates chờ m7b xóa)
4. Console khi mở Unity: 0 "missing sprite" warning
5. Git diff: ~75 rename entries (không phải delete+create)

**m7b-sprite-wireup:**
1. Tất cả 7 `CropDataSO` có `growthStageSprites` đủ 4 GUID, `deadSprite` != 0, `seedIcon` != 0, `cropIcon` != 0
2. Cả 2 `AnimalDataSO` có `stageSprites` đủ 3 GUID, `deadSprite` != 0, `readyToCollectIcon` != 0
3. `CropTile` prefab: 4 overlay SpriteRenderer có sprite mới
4. `SCN_Main` scene: `NavBtn_Shop` và `NavBtn_Event` có sprite mới
5. Play mode smoke test: 7 crop lifecycle OK, chicken/duck render OK, 0 pink sprite, 0 console warning
