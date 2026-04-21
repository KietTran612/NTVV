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

CropDataSO kế thừa `ItemData` → **4 sprite field cần xem xét** (grep Scripts/ xác nhận):

| Field | Location | Usage | Action |
|-------|----------|-------|--------|
| `icon` | top-level (`ItemData`) | **0 usage** trong Scripts/ | **SKIP** — giữ GUID hiện tại |
| `data.seedIcon` | nested (`CropData` struct) | `ShopPanelController:105`, `StoragePanelController:133` | **WIRE** (UI dùng field này) |
| `seedIcon` | top-level (`CropDataSO`) | 0 usage found — duplicate | **WIRE** sync cùng GUID với `data.seedIcon` |
| `cropIcon` | top-level | Storage/Inventory (theo comment) | **WIRE** = Stage03 GUID (reuse) |
| `growthStageSprites[0..3]` | top-level | `CropTileView` render | **WIRE** 4 stage |
| `deadSprite` | top-level | Dead render | **WIRE** |

Dùng `assets-modify` patch từng field. Lấy GUID qua `assets-find`.

**Bước 2: AnimalDataSO (2 file `animal_01..02.asset`)**

AnimalDataSO cũng kế thừa `ItemData` → có `icon` top-level. `AnimalData` struct không có sprite field (verified). Wire:
- `stageSprites[0..2]` — 3 stage (Stage00-02)
- `deadSprite`
- `readyToCollectIcon` — Egg/DuckEgg
- SKIP: `icon` top-level (0 usage, giữ nguyên)

**Bước 3: CropTile prefab — overlay sprite references**

**Path chính xác:** `Assets/_Project/Prefabs/World/CropTile.prefab` (trong subfolder `World/`)

**Cấu trúc** (verified từ prefab YAML):
- `_soilRenderer` kiểu `SpriteRenderer` trực tiếp (field chứa component reference)
- `_weedVisual`, `_bugVisual`, `_waterVisual` kiểu `GameObject` — mỗi GameObject có `SpriteRenderer` component **trên chính nó** (không phải child). Code dùng `GameObject.SetActive()` để show/hide, SpriteRenderer render sprite đính kèm.

4 SpriteRenderer cần swap:
- SoilRenderer's sprite → `World_Tile_Soil_Base_Default.png`
- WeedVisual's SpriteRenderer.sprite → `World_Overlay_Tile_Weed_On.png`
- BugVisual's SpriteRenderer.sprite → `World_Overlay_Tile_Pest_On.png`
- WaterVisual's SpriteRenderer.sprite → `World_Overlay_Tile_WaterNeed_On.png`

Dùng `gameobject-find` tìm từng Visual GameObject theo tên (`WaterVisual`, `WeedVisual`, `BugVisual`), rồi `gameobject-component-modify` SpriteRenderer.m_Sprite sau khi `assets-prefab-open`.

**Ref:** WaterVisual hiện trỏ GUID `b73d372af6cd515499d753df7754f2ec` (= `water_needed.png` OLD) — task 4 phải repoint TRƯỚC khi task 7 xóa file đó.

**Bước 4: BottomNav button sprites**

Trong `SCN_Main.unity`, 2 nút Nav đang placeholder — wire sprite mới:
- `NavBtn_Shop.Image.sprite` → `UI_Icon_Nav_Shop_Default.png`
- `NavBtn_Event.Image.sprite` → `UI_Icon_Nav_Event_Default.png`

**Bước 5: Cleanup**
- Delete `Art/Sprites/UI/Legacy/` (4 file)
- Delete old lowercase World sprites sau khi CropTile prefab đã repoint
- Delete duplicate `icon_Feed_Worm_Atomic_1.png`

**Bước 6: Smoke test (API-accurate)**

**Không dùng fictional API** (`TimeManager.AddTime`, `SetStage`, `ApplyAilment` — không tồn tại trong code). Thay bằng:

- **Speed up tick:** reflection set private SerializeField `TimeManager.Instance._tickRate = 0.01f` (100× faster); restore cuối test
- **Plant crop:** tap tile (hoặc `CropActionPanelController.OnPlantButton`) → chờ ~10-30s với tick speedup
- **Force ailment:** reflection set private bool `_hasWeeds`/`_hasPests`/`_needsWater` = true trên tile, gọi private `RefreshVisuals()` qua reflection
- **Animal stage:** reflection set private `_currentStage` (nếu có), hoặc spawn từ SO pattern m4
- **Verify visuals:** `screenshot-game-view` + `gameobject-component-get` SpriteRenderer.m_Sprite match GUID expected
- **Console:** 0 "missing sprite", 0 pink sprite

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
