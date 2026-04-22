# Requirements: m8-scene-polish

## Overview

"Scene hygiene pass" — fix các bug/cleanup còn lại trong `SCN_Main` để sẵn sàng demo và testing. Phạm vi tập trung vào 3 issues player nhìn thấy trực tiếp + 2 code/doc cleanup tight-coupled:

1. **BUG-10**: Main Camera không nhìn vào world — game view render solid color khi play
2. **NOTE-08 (m6b)**: `CropTileView._lockOverlay` chưa assign trên 6 tiles trong scene
3. **Stray CropTile verification**: HANDOVER (17/04) note về stray GO ở root — verify và cleanup nếu còn
4. **BUG-08**: `CropData` timing fields không có `[Min]` validation → cây chết ngay khi chín nếu `postRipeLifeMin = 0`
5. **bug-backlog.md cleanup**: Duplicate NOTE numbers, resolved items chưa đánh dấu, event hookup outdated

**Prerequisite:** `m7b-sprite-wireup` DONE (sprites wired vào SO/Prefab — camera fix verify cần thấy sprites). Spec `m7c-font-wireup` đang active nhưng không conflict scope.

**Design doc:** `.kiro/specs/m8-scene-polish/design.md`
**Tasks:** `.kiro/specs/m8-scene-polish/tasks.md`

**Non-goals (cố ý excluded):**
- NOTE-01 FarmCamera Old/New Input conflict (prototype acceptable)
- BUG-04 `plantTimestamp` float drift (chưa gây bug thực tế)
- BUG-09 `_tileId` setter (workaround đang OK)
- NOTE-04, 05 Refresh_Button/GemsBalance UX polish (chờ UX decision)
- NOTE-10 m6b Singleton OnDestroy refactor (code quality, không gây bug)

---

## Global Rules

> ⚠️ ĐỌC TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO

- `script-read` trước khi sửa bất kỳ file nào
- `assets-refresh` sau mỗi script change, CHỜ compile xong trước khi tiếp tục
- `scene-save` sau mỗi thay đổi scene
- Quick Test FAIL → fix trong task hiện tại, KHÔNG sang task tiếp theo (tối đa 2 retry)
- Không refactor code ngoài scope đã design
- Không thêm feature gameplay mới trong spec này
- Khi tạo GameObject con trong scene: luôn dùng `transform-set-parent` sau `gameobject-create` để verify hierarchy đúng (theo NOTE-07 m6a)

---

## Functional Requirements

### Req 1 — Stray GameObject Verification (Task 0 pre-flight)
- 1.1 Verify không có GameObject tên `CropTile` ở root scene `SCN_Main`
- 1.2 Nếu tìm thấy stray GO → delete + `scene-save`
- 1.3 Nếu không tìm thấy → log + skip (HANDOVER 17/04 note outdated)
- 1.4 Acceptance: 0 GameObject tên `CropTile` ngoài 6 `tile_r{r}_c{c}` dưới `CropArea`

### Req 2 — CropData Validation Attributes (BUG-08)
- 2.1 `CropData.cs`: thêm `[Min(0.1f)]` cho `growTimeMin`
- 2.2 `CropData.cs`: thêm `[Min(0.1f)]` cho `perfectWindowMin`
- 2.3 `CropData.cs`: thêm `[Min(0.1f)]` cho `postRipeLifeMin`
- 2.4 0 compilation errors sau `assets-refresh`
- 2.5 Inspector của 7 `crop_0X.asset` hiển value hiện tại không bị reset (attribute chỉ clamp giá trị < 0.1)

### Req 3 — Main Camera Fix (BUG-10)
- 3.1 Main Camera transform: `position = (1.2, 0.35, -10)`, `rotation = (0, 0, 0)`, `scale = (1, 1, 1)`
- 3.2 Camera component giữ nguyên: `orthographic = true`, `orthographicSize = 5`, `clearFlags = SolidColor`, `cullingMask = Everything`
- 3.3 (Optional) `backgroundColor` đổi sang `#87CEEB` (sky blue) — farm vibe. Nếu không đổi, giữ navy hiện tại.
- 3.4 Acceptance: Play mode → `screenshot-game-view` → game view hiện 6 tile soil (brown), không còn solid color
- 3.5 Camera position `(1.2, 0.35)` nằm trong `FarmCameraController._boundX = (-5, 5)` và `_boundY = (-3, 3)` → drag không reset ngoài bounds

### Req 4 — LockOverlay Placeholder Asset
- 4.1 File `Assets/_Project/Art/Sprites/World/Overlays/World_Overlay_Tile_Lock_On.png` tồn tại
- 4.2 Spec placeholder: texture 512×512 RGBA, tất cả pixel `(0, 0, 0, 150)` (đen alpha ~59%)
- 4.3 TextureImporter: `textureType = Sprite (2D and UI)`, `spriteMode = Single`, `pixelsPerUnit = 100`
- 4.4 Acceptance: `assets-find(filter="World_Overlay_Tile_Lock_On t:Sprite")` → 1 hit, GUID generated

### Req 5 — LockOverlay Wire vào 6 Tiles
- 5.1 Mỗi tile `tile_r{r}_c{c}` có child GameObject tên `LockOverlay`
- 5.2 LockOverlay có `SpriteRenderer`: `sprite = World_Overlay_Tile_Lock_On`, `sortingLayerName = "Default"`, `sortingOrder = 10` (phủ lên sprite body)
- 5.3 LockOverlay GameObject `SetActive(false)` mặc định (v1 không có tile nào locked)
- 5.4 `CropTileView._lockOverlay` field được wire với LockOverlay GO tương ứng trên cùng tile
- 5.5 Acceptance: `component-get` CropTileView mỗi tile → `_lockOverlay` not null; Set `_isLocked=true` trên tile → overlay hiện trong Scene view

### Req 6 — Asset Prompt Documentation
- 6.1 `docs/asset-prompts/2026-04-17-rework/world-overlays.md` có thêm block `<details>` cho `World_Overlay_Tile_Lock_On`
- 6.2 Block có: File name + target path, ChatGPT/DALL-E prompt, ComfyUI positive+negative+checkpoint+sampler+steps+CFG+size, VN note
- 6.3 Format khớp với các block hiện có trong file (Weed, Pest, WaterNeed)

### Req 7 — bug-backlog.md Cleanup
- 7.1 Renumber duplicate NOTE: m6b section `NOTE-07..11` → `NOTE-12..16`
- 7.2 Move xuống `✅ RESOLVED` section với reference spec đã fix:
  - BUG-01 → m3a
  - BUG-02 → m4
  - BUG-03 → m3b
  - FEAT-01 → m6a + m6b
  - FEAT-02 → m5
  - FEAT-03 → m5 (BUG-Q3)
  - FEAT-04 → m5 (BUG-Q2)
  - FEAT-07 → m6a
  - NOTE-06 → m4
- 7.3 Update Event hookup table: `QuestEvents.OnQuestStateChanged` → ✅ `QuestPanelController` subscribe (m5)
- 7.4 Add new `✅ RESOLVED — m8-scene-polish (2026-04-22)` section cho BUG-08, BUG-10, NOTE-13 (renumbered từ NOTE-08 m6b)
- 7.5 Acceptance: Grep `^### NOTE-\d+` trong file → không còn số trùng; active HIGH/MEDIUM sections chỉ còn bugs thực sự open

### Req 8 — Integration Smoke Test
- 8.1 Play mode → boot 0 errors
- 8.2 `screenshot-game-view` → thấy 6 tile soil + UI HUD
- 8.3 Set `tile_r0_c0._isLocked=true, _requiredLevel=2` → Play → tile có overlay đen
- 8.4 Fire level up (qua `component-set` hoặc manual) → overlay biến mất
- 8.5 Stop → revert test state → `scene-save`
- 8.6 Console: 0 NullReferenceException

### Req 9 — HANDOVER Update
- 9.1 `docs/HANDOVER.md` có section session 22/04/2026 cho `m8-scene-polish` DONE
- 9.2 Kiro Specs section thêm Spec 11 (`m8-scene-polish`) = DONE
- 9.3 "Bước tiếp theo" cập nhật: BUG-10 resolved, LockOverlay placeholder wired, còn lại: swap asset thật, các non-goal bugs
