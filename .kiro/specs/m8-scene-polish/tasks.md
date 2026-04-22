# Implementation Plan: m8-scene-polish

## Overview

"Scene hygiene pass" — 9 tasks (Task 0 pre-flight → Task 8 HANDOVER). Fix BUG-08 (CropData validation), BUG-10 (Main Camera), NOTE-08 m6b (LockOverlay wiring), verify stray GO, cleanup bug-backlog.

**Design doc:** `.kiro/specs/m8-scene-polish/design.md`
**Requirements:** `.kiro/specs/m8-scene-polish/requirements.md`

> ⚠️ ĐỌC GLOBAL RULES trong requirements.md TRƯỚC KHI BẮT ĐẦU BẤT KỲ TASK NÀO.

**Prerequisite check trước khi start:**
- `m7b-sprite-wireup` DONE (sprites wired — cần để camera verify thấy renderers)
- Scene `SCN_Main.unity` là scene active

---

## Tasks

- [x] 0. Pre-flight — Verify stray CropTile GO cleanup
  - [x] 0.0 · Resource Check
    - Không có asset cần check — SKIP
    - NON-BLOCKING
  - [x] 0.1 · Scene Agent — Verify + cleanup nếu cần
    - `scene-open` → `Assets/_Project/Scenes/SCN_Main.unity`
    - `gameobject-find(name="CropTile")` — expected: không tìm thấy (HANDOVER 17/04 note có thể outdated, verify đã xác nhận 22/04 không có)
    - Nếu tìm thấy GO tên `CropTile` ở root scene (không phải 6 `tile_r{r}_c{c}` dưới CropArea) → `gameobject-destroy(name="CropTile")`
    - Nếu không tìm thấy → log "No stray CropTile — HANDOVER note outdated, skip cleanup"
    - `scene-save`
    - _Requirements: 1.1, 1.2, 1.3_
  - [x] 0.✓ · Quick Test
    - `gameobject-find(name="CropTile")` lần 2 → 0 hit
    - `gameobject-find(name="tile_r0_c0")` → confirm 6 tiles vẫn còn dưới CropArea
    - Nếu FAIL → fix trong task 0, KHÔNG sang task 1

- [x] 1. Fix BUG-08 — CropData validation attributes
  - [x] 1.0 · Resource Check
    - Không có sprite asset — SKIP
    - NON-BLOCKING
  - [x] 1.4 · Script Agent — Sửa CropData.cs
    - `script-read` → `Assets/_Project/Scripts/Data/CropData.cs`
    - Thêm `[Min(0.1f)]` trước 3 fields:
      - Line 17 (cũ: `public float growTimeMin;`):
        ```csharp
        [Min(0.1f)] public float growTimeMin;
        ```
      - Line 39 (cũ: `public float perfectWindowMin;`):
        ```csharp
        [Min(0.1f)] public float perfectWindowMin;
        ```
      - Line 40 (cũ: `public float postRipeLifeMin;`):
        ```csharp
        [Min(0.1f)] public float postRipeLifeMin;
        ```
    - `assets-refresh` → chờ compile xong
    - _Requirements: 2.1, 2.2, 2.3_
  - [x] 1.✓ · Quick Test
    - `console-get-logs` filter=Error → 0 compile errors
    - `script-read` lại CropData.cs → verify 3 attribute `[Min(0.1f)]` có mặt
    - `assets-get-data` cho `crop_01.asset` → verify `growTimeMin` value unchanged (không bị reset)
    - Nếu FAIL → fix trong task 1, KHÔNG sang task 2

- [x] 2. Fix BUG-10 — Main Camera reset transform
  - [x] 2.0 · Resource Check
    - Không có sprite asset — SKIP
    - NON-BLOCKING
  - [x] 2.1 · Scene Agent — Reset Main Camera transform
    - `gameobject-find(name="Main Camera")` → get current state
    - Log current transform (để so sánh sau): expected position `(7.37, 8.93, 0)` (verified 22/04)
    - `gameobject-modify` Main Camera:
      - `localPosition = (1.2, 0.35, -10)`
      - `localRotation = (0, 0, 0)` (Euler)
      - `localScale = (1, 1, 1)`
    - `component-get` Camera → verify các param đã đúng (không sửa):
      - `orthographic = 1`
      - `orthographicSize = 5`
      - `clearFlags = 1` (SolidColor)
      - `cullingMask = -1` (Everything)
    - (Optional) `component-modify` Camera: `backgroundColor = (0.529, 0.808, 0.922, 1.0)` = `#87CEEB`. Nếu skip, giữ navy hiện tại.
    - `scene-save`
    - _Requirements: 3.1, 3.2, 3.3_
  - [x] 2.✓ · Quick Test — Play mode verify
    - `editor-application-set-state` → Play
    - `console-get-logs` filter=Error → 0 runtime errors (đặc biệt 0 NullReferenceException trong camera setup)
    - `screenshot-game-view` → save tạm `docs/screenshots/2026-04-22-m8-camera-fix.png`
    - Visual check: thấy 6 tile soil (brown sprite), KHÔNG phải solid color
    - `editor-application-set-state` → Stop
    - Nếu game view vẫn solid color → inspect `screenshot-scene-view` để xem tile position vs camera position, adjust ortho size hoặc position. Retry tối đa 2 lần, fix trong task 2, KHÔNG sang task 3.
    - _Requirements: 3.4, 3.5_

- [x] 3. Tạo LockOverlay placeholder asset
  - [x] 3.0 · Resource Check
    - Verify folder `Assets/_Project/Art/Sprites/World/Overlays/` tồn tại (đã verify 22/04 — có Weed/Pest/WaterNeed)
    - Verify `World_Overlay_Tile_Lock_On.png` CHƯA tồn tại (nếu đã có, skip create, chỉ re-import)
  - [x] 3.4 · Script Agent — Execute Editor script tạo texture
    - `script-execute` với code:
      ```csharp
      using UnityEditor;
      using UnityEngine;
      using System.IO;

      public class CreateLockOverlayPlaceholder
      {
          public static void Run()
          {
              string path = "Assets/_Project/Art/Sprites/World/Overlays/World_Overlay_Tile_Lock_On.png";
              Texture2D tex = new Texture2D(512, 512, TextureFormat.RGBA32, false);
              Color32[] pixels = new Color32[512 * 512];
              for (int i = 0; i < pixels.Length; i++) pixels[i] = new Color32(0, 0, 0, 150);
              tex.SetPixels32(pixels);
              tex.Apply();
              File.WriteAllBytes(path, tex.EncodeToPNG());
              AssetDatabase.ImportAsset(path);
              TextureImporter imp = (TextureImporter)AssetImporter.GetAtPath(path);
              imp.textureType = TextureImporterType.Sprite;
              imp.spriteImportMode = SpriteImportMode.Single;
              imp.spritePixelsPerUnit = 100;
              imp.SaveAndReimport();
              Debug.Log("[m8] LockOverlay placeholder created at " + path);
          }
      }
      ```
    - Entry method: `CreateLockOverlayPlaceholder.Run`
    - `assets-refresh`
    - _Requirements: 4.1, 4.2, 4.3_
  - [x] 3.✓ · Quick Test
    - `assets-find(filter="World_Overlay_Tile_Lock_On t:Sprite")` → 1 hit
    - `assets-get-data` cho sprite → verify `textureType=Sprite`, `spriteMode=Single`
    - Nếu FAIL (file không tạo, import sai) → fix trong task 3, KHÔNG sang task 4

- [x] 4. Wire LockOverlay vào 6 CropTile
  - [x] 4.0 · Resource Check
    - `assets-find("World_Overlay_Tile_Lock_On")` → confirm sprite tồn tại
    - `scene-open` SCN_Main (đảm bảo không ở prefab edit mode)
  - [x] 4.1 · Scene Agent — Add LockOverlay child to each tile (standalone GOs — prefab path skipped)
    - Tiles confirmed standalone (m_CorrespondingSourceObject: {fileID: 0}) → dùng scene override
    - Editor script `Assets/Editor/SetupLockOverlays.cs` tạo + chạy qua `Tools > Setup Lock Overlays`
    - Cho mỗi tile: tạo child `LockOverlay`, add `SpriteRenderer` (sprite=World_Overlay_Tile_Lock_On GUID cc3abb6e, sortingOrder=10, sortingLayerName=Default), SetActive(false)
    - `scene-save`
    - _Requirements: 5.1, 5.2, 5.3_
  - [x] 4.2 · Script Agent — Wire `_lockOverlay` field cho 6 tile instances
    - Editor script dùng `SerializedObject` để set `_lockOverlay` field trên CropTileView mỗi tile
    - 6/6 tiles wired: `_lockOverlay` → tương ứng child LockOverlay GO
    - `scene-save`
    - _Requirements: 5.4_
  - [x] 4.✓ · Quick Test
    - 6/6 `tile_r{r}_c{c}/LockOverlay` found (active=false, components=[Transform, SpriteRenderer])
    - `component-get` CropTileView mỗi tile → `_lockOverlay` not null (verified tile_r0_c0, tile_r0_c1, tile_r1_c2)
    - Scene file: 6x GUID cc3abb6e25f62b14bb37edf653cb8d8c + 6x m_SortingOrder: 10 confirmed
    - Revert: `_isLocked = false`, `scene-save` ✅
    - Note: Prefab path skipped (tiles are standalone GOs) — dùng scene override per task 4.⚠️ fallback

- [x] 5. Append asset prompt vào world-overlays.md
  - [x] 5.0 · Resource Check
    - File `docs/asset-prompts/2026-04-17-rework/world-overlays.md` tồn tại (đã verify 22/04)
  - [x] 5.1 · Docs Agent — Append block
    - `Read` file đầu + cuối để xác định vị trí append (sau block `World_Overlay_Tile_Pest_On` hoặc cuối file)
    - `Edit` (hoặc append) thêm block:
      ````markdown

      <details>
      <summary>World_Overlay_Tile_Lock_On — locked tile overlay</summary>

      ### File
      - Name: `World_Overlay_Tile_Lock_On.png`
      - Target: `Assets/_Project/Art/Sprites/World/Overlays/`

      ### ChatGPT / DALL-E (EN)
      - Prompt: "Isometric 2.5D overlay for a locked farm tile, semi-transparent dark gradient with a centered golden padlock icon, cute cartoon style, clean outline, transparent background, centered composition, no text, no humans, game-ready sprite"
      - Setup: DALL-E 3 | 1024x1024 | vivid | hd

      ### ComfyUI (EN)
      - Positive: "isometric 2.5d locked tile overlay, semi-transparent dark tint, centered golden padlock, cute cartoon, clean outline, transparent background, game sprite"
      - Negative: "realistic, photo, 3d render, gritty, pixel art, text, watermark, blurry, opaque"
      - Checkpoint: dreamshaper_8 / anything-v5
      - Sampler: DPM++ 2M Karras
      - Steps: 28
      - CFG: 7
      - Size: 512x512

      ### VN note
      - Overlay cho tile đang locked. Khi `CropTileView._isLocked = true` → SetActive(true). Khi player đạt level → auto-unlock → SetActive(false).
      - **Placeholder hiện tại (m8-scene-polish)**: texture 512×512 đen alpha 150, không có icon padlock. Swap asset khi có sprite thật — GUID preserved trong `.meta`, không cần sửa scene.

      </details>
      ````
    - _Requirements: 6.1, 6.2, 6.3_
  - [x] 5.✓ · Quick Test
    - Grep `World_Overlay_Tile_Lock_On` trong file → 1 block match
    - Verify format khớp các block hiện có (có summary, File, ChatGPT, ComfyUI, VN note)
    - Nếu FAIL → fix trong task 5, KHÔNG sang task 6

- [x] 6. Cleanup bug-backlog.md
  - [x] 6.0 · Resource Check
    - File `docs/backlog/bug-backlog.md` tồn tại
  - [x] 6.1 · Docs Agent — Renumber duplicate NOTE
    - `Read` file full
    - Renumber m6b section:
      - `### NOTE-07: DontDestroyOnLoad` → `### NOTE-12: DontDestroyOnLoad`
      - `### NOTE-08: _lockOverlay chưa assign` → `### NOTE-13: _lockOverlay chưa assign` (sẽ mark RESOLVED ở bước tiếp theo)
      - `### NOTE-09: LevelSystem.OnLevelUp event table` → `### NOTE-14: LevelSystem.OnLevelUp event table` (cũng mark RESOLVED)
      - `### NOTE-10: GameManager.OnDestroy` → `### NOTE-15: GameManager.OnDestroy`
      - `### NOTE-11: Game view trống` → `### NOTE-16: Game view trống` (sẽ merge với BUG-10 RESOLVED)
    - _Requirements: 7.1_
  - [x] 6.2 · Docs Agent — Move resolved items xuống ✅ RESOLVED section
    - Cho mỗi entry sau, thêm line `- ✅ Fixed in {spec}` ở đầu description, sau đó move nguyên block xuống section `✅ RESOLVED`:
      - BUG-01 (`_registry` null) → m3a-crop-care-harvest (WIRE-01)
      - BUG-02 (Animal chết không xóa) → m4-animal-care (BUG-02)
      - BUG-03 (Storage null check) → m3b-storage-sell-flow (BUG-B1)
      - FEAT-01 (OnLevelUp không subscriber) → m6a + m6b
      - FEAT-02 (OnQuestStateChanged) → m5-quest-flow (BUG-Q1)
      - FEAT-03 (Quest unlock) → m5-quest-flow (BUG-Q3)
      - FEAT-04 (Prerequisite) → m5-quest-flow (BUG-Q2)
      - FEAT-07 (Gems dead code) → m6a-player-feedback
      - NOTE-06 (AnimalPen prefab) → m4-animal-care Task 7
    - _Requirements: 7.2_
  - [x] 6.3 · Docs Agent — Update Event hookup table
    - Sửa dòng `| QuestEvents.OnQuestStateChanged | ⚠️ Không ai subscribe |`:
      → `| QuestEvents.OnQuestStateChanged | ✅ QuestPanelController subscribe (m5-quest-flow) |`
    - _Requirements: 7.3_
  - [x] 6.4 · Docs Agent — Thêm ✅ RESOLVED section cho m8
    - Thêm cuối file:
      ```markdown
      ## ✅ RESOLVED — m8-scene-polish (2026-04-22)

      ### BUG-08: postRipeLifeMin = 0 cây chết ngay — DONE ✅
      - `CropData.cs`: `[Min(0.1f)]` cho `growTimeMin`, `perfectWindowMin`, `postRipeLifeMin`

      ### BUG-10: Main Camera không nhìn vào world — DONE ✅
      - Main Camera transform reset: `position = (1.2, 0.35, -10)`, rotation/scale identity
      - Game view render đúng world sau fix

      ### NOTE-13 (renumbered từ NOTE-08 m6b): `_lockOverlay` chưa assign — DONE ✅
      - 6 tiles có child `LockOverlay` + SpriteRenderer với `World_Overlay_Tile_Lock_On` sprite
      - `CropTileView._lockOverlay` field wired trên cả 6 instance

      ### NOTE-14 (renumbered từ NOTE-09 m6b): Event table outdated — DONE ✅
      - Event hookup table updated: `QuestEvents.OnQuestStateChanged` ✅ QuestPanelController subscribe
      ```
    - _Requirements: 7.4_
  - [x] 6.✓ · Quick Test
    - Grep `^### NOTE-(0[7-9]|1[01])` trong section m6b → 0 hits (đã renumber)
    - Grep `^### NOTE-1[2-6]` → 5 hits (NOTE-12..16 mới)
    - Grep `✅ RESOLVED — m8-scene-polish` → 1 match
    - Count `✅ RESOLVED` sections: should be ≥ 2 (existing m6b + new m8)
    - Nếu FAIL → fix trong task 6, KHÔNG sang task 7

- [x] 7. Integration Smoke Test
  - [x] 7.1 · Boot smoke test
    - `scene-open` SCN_Main
    - `editor-application-set-state` → Play
    - `console-get-logs filter=Error` → 0 errors + 0 NullReferenceException
    - `screenshot-game-view` → lưu `docs/screenshots/2026-04-22-m8-integration-boot.png`
    - Visual: thấy 6 tile soil + TopHUDBar + BottomNav
    - `editor-application-set-state` → Stop
    - _Requirements: 8.1, 8.2, 8.6_
  - [x] 7.2 · Locked tile visual test
    - `gameobject-component-modify` `tile_r0_c0` → CropTileView: `_isLocked = true, _requiredLevel = 2`
    - `editor-application-set-state` → Play
    - `screenshot-game-view` → lưu `docs/screenshots/2026-04-22-m8-integration-locked.png`
    - Visual: 5 tile bình thường, `tile_r0_c0` có overlay đen
    - _Requirements: 8.3_
  - [x] 7.3 · Auto-unlock test
    - Trong play mode: `component-modify` LevelSystem `_currentLevel = 2`
    - Method call: `reflection-method-call` `GameManager.OnPlayerLevelUp(2)` (nếu có access)
    - Fallback nếu method private: `component-modify` `tile_r0_c0` → CropTileView: gọi `Unlock()` qua `component-invoke` (parameterless) — chỉ verify overlay hide, không test flow OnLevelUp
    - `screenshot-game-view` → overlay biến mất
    - `editor-application-set-state` → Stop
    - _Requirements: 8.4_
  - [x] 7.4 · Revert test state
    - `gameobject-component-modify` `tile_r0_c0` → `_isLocked = false, _requiredLevel = 0`
    - `scene-save`
    - _Requirements: 8.5_
  - [x] 7.✓ · Integration Report
    - Output format:
      ```
      📋 INTEGRATION TEST REPORT: m8-scene-polish
      ✅ PASSED: N checks
      ⚠️ WARNINGS: M (list nếu có)
      ❌ FAILED: 0 (hoặc liệt kê)
      Camera fix: ✅ game view renders 6 tiles
      LockOverlay: ✅ 6 tiles wired, overlay toggles đúng
      BUG-08 attributes: ✅ [Min] applied
      Stray GO: ✅ verified clean
      Console: 0 NullReferenceException ✅
      ```
    - Nếu FAIL critical → fix trong task 7, KHÔNG sang task 8

- [x] 8. Cập nhật HANDOVER.md
  - [x] 8.1 · Docs Agent — Thêm session entry
    - `Read` `docs/HANDOVER.md`
    - Thêm section đầu "Bối cảnh Phiên làm việc":
      ```markdown
      - **Phiên 22/04/2026 — m8-scene-polish HOÀN THÀNH**:
          - **`m8-scene-polish` spec DONE**: 9 tasks execute xong (Task 0 pre-flight → Task 8 HANDOVER).
          - **BUG-08 fixed**: `CropData.cs` — `[Min(0.1f)]` cho 3 timing fields (`growTimeMin`, `perfectWindowMin`, `postRipeLifeMin`)
          - **BUG-10 fixed**: Main Camera transform reset `(7.37, 8.93, 0)` → `(1.2, 0.35, -10)`. Game view render 6 tile soil đúng, không còn solid color.
          - **NOTE-13 (renumbered từ NOTE-08 m6b) fixed**: 6 tiles có LockOverlay child + `_lockOverlay` wired. Placeholder asset `World_Overlay_Tile_Lock_On.png` (đen alpha 150), prompt thật documented tại `world-overlays.md`.
          - **Stray GO**: verified clean (HANDOVER 17/04 note outdated — không còn stray)
          - **bug-backlog.md cleanup**: Renumber NOTE-07..11 m6b → NOTE-12..16; move 9 RESOLVED items; update event hookup table; thêm ✅ RESOLVED m8 section.
          - **Integration smoke test PASSED**: 0 errors, camera works, lock overlay toggles đúng.
      ```
    - Cập nhật "Kiro Specs đang active" — thêm:
      ```markdown
      ### Spec 11: `m8-scene-polish` ✅ DONE
      - **Path**: `.kiro/specs/m8-scene-polish/`
      - **Status**: **TẤT CẢ 9 TASKS HOÀN THÀNH** (22/04/2026)
      - **Kết quả**:
          - BUG-08 fixed: CropData [Min(0.1f)] attributes
          - BUG-10 fixed: Main Camera transform (1.2, 0.35, -10)
          - NOTE-13 fixed: LockOverlay wired 6 tiles + placeholder asset
          - Asset prompt documented cho swap asset thật
          - bug-backlog.md cleanup (renumber + resolved + event table)
      ```
    - Cập nhật "Bước tiếp theo":
      - Remove: "Camera fix" (done), "LockOverlay asset" (done)
      - Keep: "Swap LockOverlay asset thật khi có sprite đẹp", "M9 milestone" (TBD), các non-goal bugs
    - _Requirements: 9.1, 9.2, 9.3_
  - [x] 8.✓ · Final Commit Check
    - Git status → list files đã thay đổi
    - Expected: `CropData.cs`, `SCN_Main.unity`, `World_Overlay_Tile_Lock_On.png{,.meta}`, `CropTile.prefab`, `world-overlays.md`, `bug-backlog.md`, `HANDOVER.md`, `.kiro/specs/m8-scene-polish/**`
    - Không commit — chờ user approve trước khi commit manual
