# Design Document: m8-scene-polish

**Date:** 2026-04-22
**Spec:** m8-scene-polish
**Status:** approved

---

## Context

NTVV đã hoàn thành **10 specs** (M1 UI rebuild → M7 sprite/font wireup). Gameplay loop hoạt động end-to-end: plant → grow → ailment → harvest → sell → level up → unlock tiles → save/load offline. Nhưng có **3 vấn đề block demo/QA** tích lũy qua nhiều phiên:

1. **BUG-10 Main Camera** — game view render solid color (#314D79) thay vì world. Không thể visual-verify bất kỳ feature nào qua `screenshot-game-view`. Blocker cho mọi integration test sau này.
2. **NOTE-08 m6b LockOverlay** — FEAT-06 locked tile system DONE (code + save/load), nhưng 6 tile trong scene chưa có `_lockOverlay` GameObject → tile locked không hiện visual feedback. Ship land expansion feature mà player không biết tile đang locked.
3. **Stray CropTile GO** (HANDOVER 17/04) — note cũ về cleanup scene root. Verify + cleanup nếu còn.

Cộng thêm 2 technical debt tight-coupled:
- **BUG-08** `postRipeLifeMin = 0` → cây chết ngay khi chín (workaround set = 5 trong asset, nhưng validation chưa enforce)
- **bug-backlog.md** có duplicate NOTE numbers (m6a và m6b cùng dùng NOTE-07..11) + 9 resolved items chưa đánh dấu

**Milestone position:** M8 = "scene hygiene pass" trước M9 (TBD). Không phải gameplay feature mới — mục tiêu là unblock QA + đóng sổ bookkeeping trước khi open milestone lớn tiếp theo.

---

## Overview

"Scene hygiene pass" sau khi các milestone gameplay M1-M7 đã DONE. Không viết system mới — chỉ fix 4 bug/note và cleanup 1 doc. Ship fast, unblock visual QA.

**Task count:** 9 tasks (Task 0 pre-flight → Task 8 HANDOVER update).

**Files cần sửa/tạo:**

| File | Thay đổi |
|------|----------|
| `Assets/_Project/Scripts/Data/CropData.cs` | Add `[Min(0.1f)]` cho 3 timing fields (BUG-08) |
| `Assets/_Project/Scenes/SCN_Main.unity` | (a) Verify stray GO cleanup (b) Main Camera position/rotation reset (c) Add 6x `LockOverlay` child + wire `_lockOverlay` field |
| `Assets/_Project/Art/Sprites/World/Overlays/World_Overlay_Tile_Lock_On.png` | Tạo placeholder 512×512 RGBA đen alpha 150 qua `script-execute` Editor script |
| `docs/asset-prompts/2026-04-17-rework/world-overlays.md` | Append prompt block cho asset thật (swap sau) |
| `docs/backlog/bug-backlog.md` | Renumber duplicate NOTE, move resolved items, update event table |
| `docs/HANDOVER.md` | Session 22/04/2026 entry + Spec 11 m8-scene-polish |

---

## Verification trước khi viết spec (done 22/04/2026)

Đọc trực tiếp `SCN_Main.unity` + `CropData.cs` xác nhận:

### Main Camera state hiện tại (root cause của BUG-10)
- Position: **(7.37, 8.93, 0)** — Z=0 (sai: 2D ortho cần Z=-10), X=7.37 ngoài `FarmCameraController._boundX=(-5,5)`
- Position trỏ vào vùng không có renderer → game view = solid color
- Orthographic size = 5, clearFlags = 1 (SolidColor), cullingMask = Everything (−1) → các tham số này đều OK, chỉ transform sai

### CropArea/Tile grid state
- `CropArea` at `[WORLD_ROOT]/CropArea` position (0, 0, 0)
- 6 tiles:
  - `tile_r0_c0` at (0, 0, 0)
  - `tile_r0_c1` at (1.2, 0, 0)
  - `tile_r0_c2` at (2.4, 0, 0)
  - `tile_r1_c0` at (0, 0.7, 0)
  - `tile_r1_c1` at (1.2, 0.7, 0)
  - `tile_r1_c2` at (2.4, 0.7, 0)
- Grid span: X [0, 2.4], Y [0, 0.7] → center ≈ (1.2, 0.35)
- **Camera target**: (1.2, 0.35, -10) centers grid + chuẩn Z cho 2D ortho

### Stray GO audit
- Grep `m_Name: CropTile` trong scene file → **0 hits**. HANDOVER note 17/04 có thể outdated.
- Task 0 giữ lại như pre-flight check phòng trường hợp user mở Unity Editor có state khác.

### CropData.cs timing fields
- L17: `public float growTimeMin;`
- L39: `public float perfectWindowMin;`
- L40: `public float postRipeLifeMin;`
- Không có attribute → `[Min(0.1f)]` an toàn vì hiện tại asset có value > 0.1 (crop_01 growTimeMin=2, postRipeLifeMin=5 theo scn-main-world-setup spec).

---

## BUG-08 — CropData Validation

**Root cause:** Field `postRipeLifeMin` cho phép `= 0` → `LifeAfterRipeInSeconds = 0` → trong `HandleTick()` `_timeSinceRipe > 0 = true` ngay frame đầu sau Ripe → Dead ngay.

**Fix:** Thêm `[Min(0.1f)]` lên 3 timing fields. Minimum 0.1 phút = 6 giây (đủ ngắn cho test, đủ dài để không crash). Unity clamp khi set giá trị trong Inspector — existing assets có value > 0.1 không bị affect.

```csharp
[Header("Timing Rules")]
[Min(0.1f)] public float perfectWindowMin;
[Min(0.1f)] public float postRipeLifeMin;

// Và:
[Min(0.1f)] public float growTimeMin;
```

---

## BUG-10 — Main Camera Fix

**Root cause:** Camera transform đã drift: position (7.37, 8.93, 0). X=7.37 ngoài bounds (-5, 5) của FarmCameraController, Y=8.93 ngoài (-3, 3), Z=0 sai cho 2D (cần -10).

**Fix:**
1. `gameobject-find(name="Main Camera")` → get transform reference
2. `gameobject-modify` transform:
   - `localPosition = (1.2, 0.35, -10)`
   - `localRotation = (0, 0, 0, 1)`
   - `localScale = (1, 1, 1)`
3. Camera component params giữ nguyên (đã đúng):
   - `orthographic = 1`
   - `orthographicSize = 5`
   - `clearFlags = 1` (SolidColor)
   - `cullingMask = -1` (Everything)
4. Optional: `backgroundColor = (0.529, 0.808, 0.922, 1.0)` = `#87CEEB` (sky blue). Nếu agent decide skip, giữ navy hiện tại — acceptance vẫn pass.
5. `scene-save` + Play → `screenshot-game-view` verify.

**Why this works:**
- (1.2, 0.35) = tile grid center → camera nhìn thẳng vào 6 tiles
- Z=-10 = convention 2D Unity, sprites ở Z=0 nằm trong frustum
- Size 5 → view height 10 units, covers full grid với margin rộng (grid chỉ cao 0.7 units)

**FarmCameraController interaction:**
- Bounds (-5, 5) × (-3, 3) chứa (1.2, 0.35) ✓
- Pan drag trong play mode không reset camera ra ngoài bounds
- minOrtho=3, maxOrtho=8 → zoom OK

---

## LockOverlay Placeholder Asset

**Mechanism (tạo qua `script-execute`):**

```csharp
using UnityEditor;
using UnityEngine;
using System.IO;

Texture2D tex = new Texture2D(512, 512, TextureFormat.RGBA32, false);
Color32[] pixels = new Color32[512 * 512];
for (int i = 0; i < pixels.Length; i++) pixels[i] = new Color32(0, 0, 0, 150);
tex.SetPixels32(pixels);
tex.Apply();

byte[] pngData = tex.EncodeToPNG();
string path = "Assets/_Project/Art/Sprites/World/Overlays/World_Overlay_Tile_Lock_On.png";
File.WriteAllBytes(path, pngData);

AssetDatabase.ImportAsset(path);

TextureImporter imp = (TextureImporter)AssetImporter.GetAtPath(path);
imp.textureType = TextureImporterType.Sprite;
imp.spriteImportMode = SpriteImportMode.Single;
imp.spritePixelsPerUnit = 100;
imp.SaveAndReimport();
```

**Swap-later contract:** Khi user generate asset thật từ prompt, ghi đè file PNG cùng path. GUID trong `.meta` preserved → mọi wire trong scene tự update. Không cần sửa scene hay code.

---

## LockOverlay Wire vào 6 Tiles

Cho mỗi tile `CropArea/tile_r{r}_c{c}`:

1. `gameobject-create(name="LockOverlay", parentPath="CropArea/tile_r{r}_c{c}")`
2. `transform-set-parent` verify → path đúng `CropArea/tile_r{r}_c{c}/LockOverlay`
3. `gameobject-component-add(component="SpriteRenderer")`
4. `gameobject-component-modify` SpriteRenderer:
   - `sprite` = asset `World_Overlay_Tile_Lock_On`
   - `sortingLayerName = "Default"`
   - `sortingOrder = 10` (phủ lên sprite body ở order 0-5, trên WaterVisual/WeedVisual/BugVisual ở 1-3)
5. `gameobject-modify` SetActive = false (v1 mặc định không có tile locked)
6. Wire `CropTileView._lockOverlay` trên cùng tile GO = reference tới LockOverlay child

**Prefab vs scene override:** 6 tile là instance của `CropTile.prefab` hay riêng lẻ? Theo `m7b-sprite-wireup` spec: 4 SpriteRenderer (Soil, Water, Weed, Bug) wired trong `CropTile.prefab`. Nếu tile là prefab instance → LockOverlay add có thể là override scene-level HOẶC apply to prefab.

**Decision:** Thêm LockOverlay vào **prefab** (`CropTile.prefab`) thay vì 6 scene overrides.
- Ưu: Consistent, scale cho future tiles, giảm override drift
- Cần: `assets-prefab-open` → add LockOverlay child → `assets-prefab-save` → 6 tile instance tự có LockOverlay
- Sau đó wire `_lockOverlay` reference mỗi tile trong scene (reference prefab child không propagate từ prefab → scene instance nếu field `[SerializeField]` scene-local — cần verify mỗi tile)

**Fallback nếu prefab open fail:** Scene override trên 6 tile riêng lẻ. Ghi warning trong task report.

---

## bug-backlog.md Cleanup Strategy

**Renumber rule:** Duplicate NOTE-07..11 trong m6b section → NOTE-12..16 (append-only sequence, giữ m6a numbering):
- m6b NOTE-07 (DontDestroyOnLoad TimeManager) → NOTE-12
- m6b NOTE-08 (`_lockOverlay` not assigned) → NOTE-13 (sẽ được mark RESOLVED)
- m6b NOTE-09 (LevelSystem event table) → NOTE-14
- m6b NOTE-10 (Singleton OnDestroy) → NOTE-15
- m6b NOTE-11 (game view green — duplicate BUG-10) → NOTE-16 (sẽ merge with BUG-10 RESOLVED)

**Move-to-RESOLVED rule:** Thêm line `- ✅ Fixed in {spec-name}` vào mỗi entry, move xuống section `✅ RESOLVED`. Không xóa content gốc (preserve lịch sử).

**Event hookup table update:**
```diff
- | `QuestEvents.OnQuestStateChanged` | ⚠️ Không ai subscribe |
+ | `QuestEvents.OnQuestStateChanged` | ✅ QuestPanelController subscribe (m5-quest-flow) |
```

**New RESOLVED section cho m8:**
```markdown
## ✅ RESOLVED — m8-scene-polish (2026-04-22)

### BUG-08: postRipeLifeMin = 0 cây chết ngay — DONE ✅
- CropData.cs: `[Min(0.1f)]` lên 3 timing fields

### BUG-10: Main Camera không nhìn vào world — DONE ✅
- Transform reset: position (1.2, 0.35, -10)

### NOTE-13 (renumbered từ NOTE-08 m6b): `_lockOverlay` chưa assign — DONE ✅
- 6 tiles: LockOverlay child GO + SpriteRenderer + wire `_lockOverlay` field
- Placeholder asset tại `World/Overlays/World_Overlay_Tile_Lock_On.png`
```

---

## Risks & Mitigations

| Risk | Mitigation |
|------|-----------|
| `script-execute` fail trên Windows với path separator | Fallback: dùng `Path.Combine` + `Assets/...` relative, `AssetDatabase.ImportAsset` qua Unity API |
| `gameobject-create(parentPath)` tạo GO ở root (NOTE-07 m6a known bug) | Luôn follow bằng `transform-set-parent` verify, log nếu hierarchy sai |
| Camera (1.2, 0.35) vẫn không thấy tiles nếu tile sprite scale bất thường | Pre-Task 2: verify tile sprite bounds qua `component-get` SpriteRenderer → confirm sprite render ở Z=0 |
| `CropTile.prefab` modify làm vỡ `m7b-sprite-wireup` wiring | Pre-Task 4: mở prefab trong Inspector, snapshot existing component list → verify không mất sau `assets-prefab-save` |
| bug-backlog renumber làm vỡ cross-references trong HANDOVER.md | Sau cleanup: grep HANDOVER cho "NOTE-XX" → fix reference nếu cần |
| Playmode test fire OnLevelUp không dễ qua MCP (NOTE-09 m6a) | Fallback: `component-set _currentLevel=2` + `RefreshUI()` (parameterless) — visual verify bằng screenshot, không cần `OnLevelUp` fire |

---

## Testing Plan

**Per-task quick test** (fail → fix ngay, không sang task sau):
- Task 1 (BUG-08): `console-get-logs filter=Error` → 0 compile errors; Inspector show `[Min(0.1f)]`
- Task 2 (Camera): `screenshot-game-view` → non-solid-color (variance check pixel)
- Task 3 (Asset): `assets-find(filter="World_Overlay_Tile_Lock_On t:Sprite")` → 1 hit
- Task 4 (Wire): `component-get` CropTileView mỗi tile → `_lockOverlay` not null
- Task 5 (Prompt): Grep `world-overlays.md` cho `World_Overlay_Tile_Lock_On` → 1 block match
- Task 6 (Backlog): Grep `^### NOTE-(0[7-9]|1[01])` trong m6b section — không còn; grep ✅ RESOLVED section có 9 items mới

**Integration smoke test (Task 7):**
1. Set `tile_r0_c0._isLocked=true, _requiredLevel=2` qua `component-modify`
2. Play → `screenshot-game-view` → thấy 5 tile bình thường + 1 tile đen
3. `component-set _currentLevel=2` + `reflection-method-call GameManager.OnPlayerLevelUp(2)` (nếu method accessible) HOẶC manual Unlock
4. `screenshot-game-view` → overlay biến mất
5. Stop → revert `_isLocked=false` → `scene-save`

---

## Decision Log

Các quyết định đã thảo luận trong brainstorm 22/04/2026 — giữ lại để trace back khi review sau.

### 1. Scope = B (focused) thay vì A (all open bugs) hoặc C (split a/b)
- **Chosen**: Option B — 3 items user requested (stray GO + camera + LockOverlay) + BUG-08 + bug-backlog cleanup
- **Alternatives rejected**:
  - A (comprehensive): ~12-15 tasks, mix visible fixes với code quality → spec quá rộng
  - C (split m8a/m8b): overhead không cần thiết cho scope nhỏ này
- **Why**: 3 task user nêu + BUG-08 đều thuộc theme "scene hygiene". Các bug code-quality khác (BUG-04 float drift, BUG-09 tileId setter, NOTE-10 Singleton) chưa gây bug thực tế → defer sang spec sau.

### 2. LockOverlay = placeholder (option A) thay vì generate asset thật (B) hoặc built-in (C)
- **Chosen**: Option A — placeholder texture 512×512 đen alpha 150, kèm prompt documentation cho asset thật
- **Alternatives rejected**:
  - B (generate asset): block spec chờ asset generation (1-2 task thêm)
  - C (Unity built-in): không chuyên nghiệp, không swap được
- **Why**: Ship fast, unblock wiring. GUID preserved → khi có sprite thật, swap file PNG → không đụng scene/code.

### 3. Camera fix = MCP inspect+apply (option A) thay vì code defaults (B) hoặc blind fix (C)
- **Chosen**: Option A — `gameobject-modify` transform với values concrete, verify qua `screenshot-game-view`
- **Alternatives rejected**:
  - B (code defaults trong FarmCameraController): phải recompile, risk scope creep sang NOTE-01 Old/New Input conflict
  - C (blind fix không verify): không biết fix đúng hay chưa
- **Why**: Concrete values (1.2, 0.35, -10), auto-verify, không touch code.

### 4. Task organization = Approach 1 (flat 9-task list) thay vì grouped (2) hoặc priority-first (3)
- **Chosen**: Approach 1 — 9 tasks sequential (Task 0 → Task 8)
- **Alternatives rejected**:
  - 2 (grouped by code/scene/docs): overkill cho 9 task đơn giản
  - 3 (priority-first, camera trước): BUG-08 attribute fix (1 dòng code) thực ra nhanh hơn camera fix
- **Why**: Matches pattern m6b/m7b (flat, successful). Mỗi task độc lập, fail-isolated.

### 5. Include bug-backlog.md cleanup trong spec (YES)
- **Chosen**: Include — Task 6
- **Why**: Cleanup doc + cleanup scene = same "hygiene" theme. Renumber + resolved markers ~15 min, closes bookkeeping cho 10 specs trước.

### 6. LockOverlay wire = prefab-first thay vì per-tile scene override
- **Chosen**: Mở `CropTile.prefab` → add LockOverlay child → save → 6 instance tự có
- **Alternatives rejected**: Scene override 6 tile riêng lẻ (sẽ drift nếu có tile mới)
- **Why**: Consistent, scale cho future tiles. Fallback scene-override documented trong Task 4.⚠️ nếu prefab modify fail.

### 7. Document structure = 1-file Kiro design (thay vì 2-file Kiro + superpowers pattern cũ)
- **Chosen**: Toàn bộ design + context + decisions + risks + testing ở `.kiro/specs/m8-scene-polish/design.md`
- **Alternatives rejected**: Pattern cũ (short Kiro design + full superpowers doc)
- **Why**: Empirical check qua m3a-m7c cho thấy superpowers doc thường duplicate ~70% Kiro content. Agent Kiro chỉ auto-load Kiro files; superpowers doc phải follow link → dễ miss Risks/Testing plan mid-task. m7c precedent đã converge về 1-file pattern. Decision áp dụng cho toàn bộ project NTVV từ m8 trở đi.
