# Implementation Plan: scn-main-world-setup

## Overview

Setup world gameplay layer cho SCN_Main: physics layer, input system, CropTile prefab, camera pan, data verification. Không viết code gameplay mới — chỉ tạo `CropGridSpawner.cs` (Editor tool) và wire/configure các scripts đã có.

**Design doc:** `.kiro/specs/scn-main-world-setup/design.md`
**Requirements:** `.kiro/specs/scn-main-world-setup/requirements.md`

> ⚠️ ĐỌC GLOBAL RULES trong requirements.md TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO.

---

## Tasks

- [x] 0. Setup "Interactable" Layer + Camera Tag
  - [x] 0.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [x] 0.1 Thêm "Interactable" layer vào Project Settings
    - Mở Project Settings → Tags and Layers
    - Thêm layer mới tên `"Interactable"` vào một slot trống (Layer 8 hoặc slot trống tiếp theo)
    - Ghi lại layer index để dùng cho task 2 và task 4
    - `editor_save_scene`
    - _Requirements: 1.1_
  - [x] 0.2 Set tag "MainCamera" cho Main Camera
    - Tìm Main Camera trong `[WORLD_ROOT]`
    - Set Tag = `"MainCamera"`
    - Verify: `Camera.main` sẽ không null sau khi set
    - `editor_save_scene`
    - _Requirements: 1.2_
  - [x] 0.✓ · Quick Test
    - `editor_read_log` filter=Error → 0 errors
    - `component_get` Main Camera → tag = "MainCamera"
    - Verify layer "Interactable" tồn tại trong Project Settings
    - Nếu FAIL → fix trong task 0, KHÔNG sang task 1

- [x] 1. Viết CropGridSpawner.cs
  - [x] 1.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [x] 1.4 · Script Agent — Tạo CropGridSpawner.cs
    - Check: `Assets/_Project/Scripts/World/CropGridSpawner.cs` đã tồn tại chưa?
    - Nếu chưa → tạo mới với nội dung sau:
      ```
      namespace NTVV.World
      [ExecuteInEditMode] MonoBehaviour
      
      Fields:
        [SerializeField] private GameObject _cropTilePrefab
        [SerializeField] private int _rows = 2
        [SerializeField] private int _cols = 3
        [SerializeField] private float _cellWidth = 1.2f
        [SerializeField] private float _cellHeight = 0.7f
        [SerializeField] private GameDataRegistrySO _registry (using NTVV.Data.ScriptableObjects)
      
      [ContextMenu("Generate Grid")]
      public void GenerateGrid():
        - Destroy tất cả children hiện có của transform
        - for row = 0 to _rows-1:
            for col = 0 to _cols-1:
              GO = Instantiate(_cropTilePrefab, transform)
              GO.name = $"tile_r{row}_c{col}"
              GO.transform.localPosition = new Vector3(col * _cellWidth, row * _cellHeight, 0)
              CropTileView view = GO.GetComponent<CropTileView>()
              if view != null && _registry != null:
                SerializedObject so = new SerializedObject(view)
                so.FindProperty("_registry").objectReferenceValue = _registry
                so.ApplyModifiedProperties()
      
      [ContextMenu("Clear Grid")]
      public void ClearGrid():
        - while transform.childCount > 0: DestroyImmediate(transform.GetChild(0).gameObject)
      ```
    - `assets_refresh` → đợi compile xong
    - _Requirements: 3.1, 3.2_
  - [x] 1.✓ · Quick Test
    - `editor_read_log` filter=Error → 0 compile errors liên quan CropGridSpawner
    - `assets_find` CropGridSpawner.cs → tồn tại tại `Assets/_Project/Scripts/World/`
    - Nếu FAIL → fix trong task 1, KHÔNG sang task 2

- [x] 2. Tạo CropTile Prefab
  - [x] 2.0 · Resource Check
    - Scan sprites được reference trong task này:
      - `soil_empty` hoặc tương đương tại `Assets/_Project/Art/Sprites/World/`
      - `weed_overlay`, `bug_overlay`, `water_needed` tại `Assets/_Project/Art/Sprites/World/`
      - Crop phase sprites (`crop_carrot_phase1`, v.v.)
    - Với mỗi sprite thiếu → append vào `docs/asset-prompts/2026-04-16-scn-main-world-setup-missing-assets.md`
    - NON-BLOCKING: proceed regardless
  - [x] 2.1 Tạo CropTile root GameObject
    - Tạo GO tên `"CropTile"` (empty, ở root scene tạm thời)
    - Add `BoxCollider` (3D, KHÔNG phải BoxCollider2D): size=(1, 1, 0.1), center=(0,0,0)
    - Set layer = `"Interactable"`
    - `editor_save_scene`
    - _Requirements: 4.1_
  - [x] 2.2 Add SoilRenderer (root level)
    - Add `SpriteRenderer` component vào CropTile root
    - Name: để trống (component trên root GO)
    - Sprite: `soil_empty` nếu có, nếu không → Unity default white square + ghi `// TODO: replace with soil_empty`
    - `editor_save_scene`
    - _Requirements: 4.2_
  - [x] 2.3 Add child visual GameObjects
    - Tạo child `"CropRenderer"`: add SpriteRenderer, SetActive=false
    - Tạo child `"WeedVisual"`: add SpriteRenderer, color=#7CFC00 (placeholder xanh lá), SetActive=false
    - Tạo child `"BugVisual"`: add SpriteRenderer, color=#8B4513 (placeholder nâu), SetActive=false
    - Tạo child `"WaterVisual"`: add SpriteRenderer, color=#87CEEB (placeholder xanh nước), SetActive=false
    - Sprite cho từng child: dùng sprite tương ứng nếu có, nếu không dùng color placeholder
    - `editor_save_scene`
    - _Requirements: 4.3, 4.4_
  - [x] 2.4 · Script Agent — Attach CropTileView + Wire + Save Prefab
    - `component_get` CropTile → kiểm tra CropTileView đã có chưa
    - Nếu chưa → `gameobject_component_add` CropTileView vào CropTile root
    - Wire serialized fields:
      - `_soilRenderer` → SpriteRenderer trên root CropTile
      - `_cropRenderer` → child "CropRenderer" SpriteRenderer
      - `_weedVisual` → child "WeedVisual" GameObject
      - `_bugVisual` → child "BugVisual" GameObject
      - `_waterVisual` → child "WaterVisual" GameObject
      - `_registry` → `GameDataRegistry.asset`
    - `assets_prefab_create`: CropTile → `Assets/_Project/Prefabs/World/CropTile.prefab`
    - `editor_refresh_assets`
    - `editor_save_scene`
    - _Requirements: 4.5, 4.6_
  - [x] 2.✓ · Quick Test
    - `editor_read_log` filter=Error → 0 errors liên quan CropTile
    - `component_get` CropTileView trên CropTile prefab → `_soilRenderer`, `_cropRenderer`, `_weedVisual`, `_bugVisual`, `_waterVisual`, `_registry` không null
    - `assets_find` CropTile.prefab tại `Assets/_Project/Prefabs/World/` → tồn tại
    - `screenshot_scene` → lưu `docs/screenshots/2026-04-16-scn-main-world-setup-task2.png`
    - Nếu FAIL → fix trong task 2, KHÔNG sang task 3

- [x] 3. Generate 6 CropTiles trong CropArea
  - [x] 3.0 · Resource Check
    - Không có sprite asset mới trong task này — SKIP scan
    - NON-BLOCKING
  - [x] 3.1 Run CropGridSpawner
    - Tìm GO `CropArea` trong `[WORLD_ROOT]`
    - Add component `CropGridSpawner` vào `CropArea`
    - Assign fields:
      - `_cropTilePrefab` → `Assets/_Project/Prefabs/World/CropTile.prefab`
      - `_rows` = 2, `_cols` = 3
      - `_cellWidth` = 1.2, `_cellHeight` = 0.7
      - `_registry` → `GameDataRegistry.asset`
    - Run `[ContextMenu] Generate Grid`
    - `editor_save_scene`
    - _Requirements: 5.1, 5.2_
  - [x] 3.2 Verify và cleanup
    - Verify `CropArea` có đúng 6 children
    - Verify tên các GO: `tile_r0_c0`, `tile_r0_c1`, `tile_r0_c2`, `tile_r1_c0`, `tile_r1_c1`, `tile_r1_c2`
    - Verify mỗi tile có layer = Interactable
    - Xóa component `CropGridSpawner` khỏi `CropArea` (script file giữ nguyên)
    - `editor_save_scene`
    - _Requirements: 3.3, 5.3_
  - [x] 3.✓ · Quick Test
    - `editor_read_log` filter=Error → 0 errors
    - Count children của CropArea = 6
    - `component_get` từng tile → CropTileView._registry không null
    - `screenshot_scene` → lưu `docs/screenshots/2026-04-16-scn-main-world-setup-task3.png`
    - Nếu FAIL → fix trong task 3, KHÔNG sang task 4

- [x] 4. Setup WorldObjectPicker + PlayerInput
  - [x] 4.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [x] 4.1 Tạo Input Action Asset
    - Tạo file `Assets/_Project/Input/PlayerInputActions.inputactions`
    - Tạo thư mục `Assets/_Project/Input/` nếu chưa có
    - Cấu hình:
      - Action Map: `"Player"`
      - Action: `"Tap"`, Type: Value, Control Type: Vector2
      - Binding: `<Pointer>/position`
      - Interaction: Press (fires on button press)
    - `editor_refresh_assets`
    - _Requirements: 2.1, 2.2_
  - [x] 4.2 Tạo WorldObjectPicker GameObject
    - Trong `[SYSTEMS]`, tạo GO tên `"WorldObjectPicker"` (empty)
    - `editor_save_scene`
    - _Requirements: 6.1_
  - [x] 4.4 · Script Agent — Attach WorldObjectPicker + PlayerInput (CÙNG GO)
    - ⚠️ QUAN TRỌNG: Cả 2 components phải trên CÙNG GameObject
      PlayerInput "Send Messages" chỉ dispatch đến cùng GO — không đến sibling
    - Add `WorldObjectPicker` vào GO "WorldObjectPicker"
    - Wire `WorldObjectPicker`:
      - `_mainCamera` → Main Camera trong `[WORLD_ROOT]`
      - `_interactableLayer` → LayerMask "Interactable"
    - Add `PlayerInput` vào CÙNG GO "WorldObjectPicker"
    - Wire `PlayerInput`:
      - `Actions` → `Assets/_Project/Input/PlayerInputActions.inputactions`
      - `Behavior` → Send Messages
    - `editor_save_scene`
    - _Requirements: 2.3, 2.4, 6.2_
  - [x] 4.✓ · Quick Test
    - `editor_read_log` filter=Error → 0 errors
    - `component_get` WorldObjectPicker → `_mainCamera`, `_interactableLayer` không null
    - `component_get` PlayerInput → `actions` không null, behavior = SendMessages
    - Verify cả 2 components trên cùng 1 GO
    - Nếu FAIL → fix trong task 4, KHÔNG sang task 5

- [x] 5. Setup FarmCameraController
  - [x] 5.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [x] 5.4 · Script Agent — Attach FarmCameraController lên Main Camera
    - `component_get` Main Camera → kiểm tra FarmCameraController đã có chưa
    - Nếu chưa → add `FarmCameraController` vào Main Camera
    - Set values:
      - `_panSpeed` = 1.0
      - `_boundX` = (-5, 5)
      - `_boundY` = (-3, 3)
      - `_zoomSpeed` = 4
      - `_minOrtho` = 3
      - `_maxOrtho` = 8
    - `editor_save_scene`
    - _Requirements: 7.1, 7.2, 7.3_
  - [x] 5.✓ · Quick Test
    - `editor_read_log` filter=Error → 0 errors liên quan FarmCameraController
    - `component_get` FarmCameraController trên Main Camera → không null
    - Verify _boundX = (-5, 5), _boundY = (-3, 3)
    - Nếu FAIL → fix trong task 5, KHÔNG sang task 6

- [x] 6. Verify [SYSTEMS] đầy đủ
  - [x] 6.0 · Resource Check
    - Không có sprite asset trong task này — SKIP scan
    - NON-BLOCKING
  - [x] 6.1 Verify/Add TimeManager
    - Tìm GO "TimeManager" trong `[SYSTEMS]`
    - Nếu tồn tại → verify `_tickRate` = 1.0
    - Nếu KHÔNG tồn tại:
      - Tạo GO `"TimeManager"` trong `[SYSTEMS]`
      - Add `TimeManager` component
      - Set `_tickRate` = 1.0
    - `editor_save_scene`
    - _Requirements: 8.1_
  - [x] 6.2 Verify/Add QuestManager
    - Tìm GO "QuestManager" trong `[SYSTEMS]`
    - Nếu KHÔNG tồn tại:
      - Tạo GO `"QuestManager"` trong `[SYSTEMS]`
      - Add `QuestManager` component
    - `editor_save_scene`
    - _Requirements: 8.2_
  - [x] 6.✓ · Quick Test
    - `sim_play` → `editor_wait_ready`
    - `editor_read_log` filter=Error → 0 NullRef errors liên quan TimeManager hoặc QuestManager
    - `component_get` TimeManager → `_tickRate` = 1.0
    - `sim_stop`
    - Nếu FAIL → fix trong task 6, KHÔNG sang task 7

- [x] 7. Data Check — GameDataRegistrySO
  - [x] 7.0 · Resource Check
    - Kiểm tra `GameDataRegistrySO.crops`:
      - Count ≥ 1?
      - Mỗi CropDataSO: `cropId != ""`, `growTimeMin > 0`, `baseYieldUnits > 0`, `postRipeLifeMin > 0`
    - Nếu thiếu → ghi vào `docs/asset-prompts/2026-04-16-scn-main-world-setup-missing-assets.md`
    - NON-BLOCKING nhưng cần fix trong 7.1 nếu thiếu
  - [x] 7.1 Tạo/Verify CropDataSO Carrot placeholder
    - Nếu `crops` rỗng hoặc không có crop hợp lệ → tạo CropDataSO mới:
      - File: `Assets/_Project/Data/Crops/CropData_Carrot.asset`
      - `cropId` = `"crop_carrot"`, `cropName` = `"Cà Rốt"`
      - `unlockLevel` = 0, `seedCostGold` = 10
      - `growTimeMin` = 1 (1 phút để test nhanh)
      - `baseYieldUnits` = 3, `sellPriceGold` = 5, `xpReward` = 10
      - `weedChancePct` = 0.3, `bugChancePct` = 0.2, `waterChancePct` = 0.2
      - `perfectWindowMin` = 0.5
      - ⚠️ `postRipeLifeMin` = 5 — CRITICAL: nếu = 0 thì cây chết ngay khi chín!
    - Add vào `GameDataRegistry.asset → crops` list
    - Verify `CropData.CanAfford(seedCostGold=10)` với gold ban đầu (thường 0) — player cần gold để plant
    - `editor_refresh_assets`
    - _Requirements: 9.1, 9.2, 9.3_
  - [x] 7.✓ · Quick Test
    - `editor_read_log` filter=Warning → kiểm tra "[Data Error]" warnings từ GameDataRegistrySO.Initialize()
    - `component_get` GameDataRegistry → crops.Count ≥ 1
    - Verify crop đầu tiên: cropId != "", growTimeMin > 0, postRipeLifeMin > 0
    - Nếu FAIL → fix trong task 7, KHÔNG sang task 8

- [x] 8. Integration Smoke Test
  - ⚠️ **Prerequisite:** `scn-main-ui-rebuild` Task 1–5 phải hoàn thành (hoặc ContextActionPanel.prefab tồn tại trong `Resources/UI/Default/`)
  - ⚠️ **Known Issue:** FarmCameraController (Old Input `Input.GetMouseButtonDown`) và WorldObjectPicker (New Input `PlayerInput.OnTap`) cùng fire khi tap — acceptable ở prototype, không fix ở đây
  - [x] 8.1 Play Mode basic check
    - `sim_play` → `editor_wait_ready`
    - `editor_read_log` filter=Error → 0 errors NullRef/MissingComponent
    - Verify GameManager boot: state = Playing (TimeManager sẽ bắt đầu tick)
    - `editor_read_log` filter=Log → tìm `"[GameManager] Boot sequence complete."`
    - _Requirements: Req 8_
  - [x] 8.2 Tap interaction check
    - Tap vào một CropTile trong scene
    - Verify: `editor_read_log` → thấy log `"[Tile Selected]"` từ WorldObjectPicker
    - Verify: CropActionPanel xuất hiện (SetActive=true)
    - Verify: Nút "Gieo Hạt" visible (TileState = Empty)
    - _Requirements: Req 6_
  - [x] 8.3 Plant flow check
    - Nếu player có đủ gold (seed cost = 10g) → tap "Gieo Hạt"
    - Verify: tile state chuyển sang Growing
    - Verify: `editor_read_log` → thấy `"[Plot] Planted: Cà Rốt"`
    - Verify: TimeManager đang tick (cây đang lớn)
    - `sim_stop` → `editor_save_scene`
    - _Requirements: Req 9_
  - [x] 8.✓ · Integration Test Report
    - `screenshot_scene` → lưu `docs/screenshots/2026-04-16-scn-main-world-setup-integration.png`
    - Output:
      ```
      📋 INTEGRATION TEST REPORT: scn-main-world-setup
      ✅ PASSED: [N] checks
      ⚠️  WARNINGS: [danh sách]
      ❌ FAILED: [N] — [mô tả]
      Known Issues: FarmCamera Old/New Input conflict (non-blocking)
      Placeholders: [danh sách assets chưa applied]
      ```
    - Nếu FAIL critical → fix, KHÔNG chuyển sang task 9

- [x] 9. Update HANDOVER.md
  - Mở `docs/HANDOVER.md`
  - Cập nhật section "Phiên 16/04/2026":
    - Spec `scn-main-world-setup` đã tạo và execute xong
    - 6 CropTile placed trong CropArea (2×3 grid)
    - WorldObjectPicker + PlayerInput (New Input System) wired
    - FarmCameraController active trên Main Camera
    - TimeManager + QuestManager verified trong [SYSTEMS]
    - Gameplay loop M2 hoạt động: tap tile → panel → plant → grow → harvest
  - Cập nhật "Cần làm ngay" → "World Setup (M2)" = DONE
  - Cập nhật "Bước tiếp theo" → M3: crop care + storage + sell flow
  - _Requirements: tất cả_
