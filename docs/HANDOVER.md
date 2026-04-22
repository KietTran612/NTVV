# Project Handover — NTVV (Nông Trại Vui Vẻ)

Tài liệu sync context AI Agent khi chuyển máy tính mới hoặc bắt đầu session mới.

> **⚠️ Convention update (22/04/2026)**: Spec mới chỉ dùng **3-file Kiro** (`.kiro/specs/<name>/{requirements,design,tasks}.md`). **KHÔNG** tạo `docs/superpowers/specs/` doc nữa. Rule chi tiết: xem `CLAUDE.md` ở root + `.kiro/steering/spec-format.md`.

---

## 🧠 Session gần đây

### Phiên 22/04/2026 (Hiện tại) — m8-scene-polish spec + Kiro conventions
- **`m8-scene-polish` spec WRITTEN** (3 files, 734 lines, 9 tasks) — **chưa execute**.
  - Scope: BUG-10 Main Camera fix, NOTE-08 m6b LockOverlay wire, BUG-08 CropData validation, stray GO verification, bug-backlog.md renumber + resolved items, HANDOVER update.
  - Non-goals: NOTE-01 FarmCamera Input conflict, BUG-04 float drift, BUG-09 tileId setter, NOTE-04/05 UX polish, NOTE-10 m6b Singleton refactor.
- **Kiro spec convention established**:
  - `CLAUDE.md` (root) — Claude Code session guide, auto-load
  - `.kiro/steering/spec-format.md` — detailed spec structure rule
  - Memory file ở `.claude/projects/.../memory/` → thin pointer tới repo files (source of truth trong repo)
- **Git**: 2 commits mới (`744a7ad` conventions + `4dbbfae` m8 spec). Chưa push.
- **Sẵn sàng execute** m8 khi bạn approve.

### Phiên 22/04/2026 — m7c-font-wireup DONE
- 13 TMP component đổi `LiberationSans SDF` → `Dosis-Bold SDF`.
- Files: `SCN_Main.unity` + 7 prefabs (InventorySlot, QuestListItem, UI_Nav_Button, UI_XP_ProgressBar, LockInfoPopup, QuestPopup, ShopPopup).
- Post-verify: 0 LiberationSans match, 58 Dosis-Bold total, 0 missing font warnings.
- Commit: `e037065`.

### Phiên 21/04/2026 — M7 Sprites DONE (a + b)
- **m7a-sprite-reorg**: Mass rename ~82 sprites sang `[Domain]_[Category]_[Entity]_[Variant]_[State].png`, folder structure `World/` (Crops/Animals/Products/Overlays/Tiles) + `UI/` (Backgrounds/Icons/Buttons). GUID preserved qua `.meta` di chuyển cùng.
- **m7b-sprite-wireup**: Wire sprites vào 7 CropDataSO + 2 AnimalDataSO + `CropTile.prefab` (4 SpriteRenderer) + `SCN_Main` BottomNav. Cleanup Legacy folder + duplicate worm icon + old lowercase world sprites. Smoke test 0 errors.

### Phiên 21/04/2026 — M6 DONE (a + b)
- **m6a-player-feedback**: `LevelUpToastController.cs` tạo mới (fade 2s qua `CanvasGroup.alpha`, `ShowMessage(string)` public API). `PlayerSaveData.gems = 25` default, save/load gems verified. `ShopPanelController._refreshCostGems = 50` (thay gold).
- **m6b-world-progression**: FEAT-05 offline animal growth fix (`GameManager.LastSaveTime` set từ save data trước `RestoreWorldState`, welcome toast nếu offline > 60s). FEAT-06 locked tile system (`CropTileView._isLocked/_requiredLevel/_lockOverlay`, `WorldObjectPicker` guard, `LockInfoPopup.prefab`, `GameManager.OnPlayerLevelUp` auto-unlock, `PlayerSaveData.unlockedTileIds` save/load).

### Phiên 20/04/2026 — M4 + M5 DONE
- **m4-animal-care**: 7 bugs (BUG-01..07), save/load per-animal (`AnimalSaveData`, `GetSaveData()`, `RestoreState()`, `SpawnAndRestore()`), auto-collect product, `AnimalPen.prefab` tạo mới + wired.
- **m5-quest-flow**: 4 quest bugs (Q1 refresh, Q2 prerequisite guard, Q3 HandleUnlock switch, Q4 feedback logs).

---

## 📜 Timeline compact (phiên cũ)

| Ngày | Deliverable |
|------|-------------|
| 17/04 | m3a-crop-care-harvest DONE (9 bugs, full crop cycle) + m3b-storage-sell-flow DONE (BUG-B1, B2) + scn-main-world-setup DONE (6 CropTile grid) + M5 spec written + Asset inventory 100 sprites + Prompt rework (`docs/asset-prompts/2026-04-17-rework/`) |
| 16/04 | SCN_Main UI rebuild DONE (4 canvas, TopHUDBar, BottomNav, 4 popups, PopupManager wired, prefabs extracted) + world-setup spec written + 6 bugs phát hiện (BUG-08, BUG-09, NOTE-01..03) |
| 15/04 | UI/Theme system removal (xóa UIStyleApplier/Processor/DataSO/Initializer) + simplify `ResourcesUIProvider` + MCP Unity connect + scn-main-ui-rebuild spec written |
| 09/04 | 100% atomic asset success (21 icons: Carrot, Chicken, Resources, Buttons; 28 assets đạt chuẩn) + folder Art UI chuyên sâu |
| 08/04 | Visual standardization (PPU multipliers: Buttons/Chips=5, Banners=2.5, Panels=1.5, Icons=1) + `[UI_CAMERA]` + `[UI_ATOMIC_STAGE]/[SAFE_AREA]` root hierarchy |

---

## 🗺 System Map

Các file "đầu não" cần nhớ khi onboard máy mới:

1. **Data & Config**
   - `Assets/_Project/Data/Registry/GameDataRegistry.asset` — 7 crops + 2 animals + quests
   - `Assets/_Project/Data/Configs/` — BalanceRules, GameConfig
2. **Art Sprites** (tổ chức từ 21/04 m7a):
   - `Assets/_Project/Art/Sprites/World/` — Crops, Animals, Products, Overlays, Tiles
   - `Assets/_Project/Art/Sprites/UI/` — Backgrounds, Icons, Buttons
3. **UI Infrastructure** (đơn giản hóa từ 15/04):
   - `IUIAssetProvider.cs` — interface load prefab
   - `ResourcesUIProvider.cs` — load từ `Resources/UI/Default/`
   - `PopupManager.cs` — screen stack
4. **Convention docs** (mới 22/04):
   - `CLAUDE.md` (root) — Claude Code session guide
   - `.kiro/steering/spec-format.md` — spec structure rule (3-file Kiro only)
   - `.kiro/steering/task-format.md` — tasks.md rule
   - `.kiro/steering/resource-checker.md` — asset scan pattern
5. **AI Production guides**:
   - `docs/guides/AI_UI_Integration_Methodology.md` — labeling, styling, PPU standards
   - `docs/guides/Atomic_HUD_Prompt_Library.md` — prompt library
   - `docs/asset-prompts/2026-04-17-rework/` — English-only naming, `Stage00..Stage03`, no `_v01`
6. **Backlog**: `docs/backlog/bug-backlog.md` — BUG-XX + NOTE-XX numbering

---

## 🎯 Trạng thái & Bước tiếp theo

### ✅ 11 specs DONE

| # | Spec | Date | Tasks |
|---|------|------|-------|
| 1 | scn-main-ui-rebuild | 16/04 | 11 |
| 2 | scn-main-world-setup | 17/04 | 9 |
| 3 | m3a-crop-care-harvest | 17/04 | 6 |
| 4 | m3b-storage-sell-flow | 17/04 | 5 |
| 5 | m4-animal-care | 20/04 | 9 |
| 6 | m5-quest-flow | 20/04 | 5 |
| 7 | m6a-player-feedback | 21/04 | 8 |
| 8 | m6b-world-progression | 21/04 | 9 |
| 9 | m7a-sprite-reorg | 21/04 | 11 |
| 10 | m7b-sprite-wireup | 21/04 | 9 |
| 11 | m7c-font-wireup | 22/04 | 6 |

### 📝 Spec WRITTEN (chưa execute)

| # | Spec | Date | Tasks | Status |
|---|------|------|-------|--------|
| 12 | **m8-scene-polish** | 22/04 | 9 | Chờ user approve để execute |

### 🚀 Bước tiếp theo

1. **Execute m8-scene-polish** — fix camera + LockOverlay + stray GO + BUG-08 + bug-backlog cleanup. 9 tasks, ~1-2 giờ execute.
2. **Generate missing sprites** — bộ rework `docs/asset-prompts/2026-04-17-rework/` (bắt đầu `entities-animals.md`). Sau khi có asset thật, swap `World_Overlay_Tile_Lock_On.png` để replace placeholder m8 tạo.
3. **Manual UI test** — Storage sell flow + Shop buy flow (button interaction cần manual verify trong Editor).
4. **M9 milestone** — TBD sau m8 DONE. Ý tưởng: thêm tile mới với `_isLocked=true`, gameplay content mới, hoặc build/release milestone.

### 🐛 Bug/Feat còn open (ngoài m8 scope)

Không block gameplay hiện tại. Xem `docs/backlog/bug-backlog.md` chi tiết.

- BUG-04: `plantTimestamp` float drift (chưa gây bug thực tế)
- BUG-09: `_tileId` private setter — workaround `gameObject.name` OK
- NOTE-01: FarmCamera Old/New Input conflict — prototype acceptable
- NOTE-04, 05: `Refresh_Button` + `GemsBalance_Label` UX polish — chờ UX decision
- NOTE-10 m6b: `Singleton<T>` OnDestroy pattern — code quality, không gây bug
- NOTE-07 m6a / NOTE-09 m6a / NOTE-10 m6a: MCP limitations (`gameobject-create parent`, `component-invoke` parameterless, TMP text trong prefab stage)

---

## 🗂 Kiro Specs Archive

Chi tiết mỗi spec: xem `.kiro/specs/<spec-name>/design.md`.

| Spec | Summary |
|------|---------|
| `scn-main-ui-rebuild` | 4 canvas + TopHUDBar + BottomNav + 4 popups + PopupManager + prefabs extract |
| `scn-main-world-setup` | 6 CropTile 2×3 grid + FarmCamera + TimeManager + QuestManager + PlayerInput |
| `m3a-crop-care-harvest` | 9 bugs fixed, plant → grow → ailment → care → ripe → harvest cycle |
| `m3b-storage-sell-flow` | BUG-B1 (sellAll filter), BUG-B2 (CanAddItem before CanAfford), registry wired |
| `m4-animal-care` | 7 bugs + save/load per-animal + auto-collect + `AnimalPen.prefab` |
| `m5-quest-flow` | 4 quest bugs (prerequisite, HandleUnlock switch, feedback logs) |
| `m6a-player-feedback` | `LevelUpToastController` + Gems save/load + Shop Refresh (50💎) |
| `m6b-world-progression` | Offline growth fix + locked tile system + auto-unlock + save/load |
| `m7a-sprite-reorg` | Mass rename ~82 sprites, new folder structure, GUID preserved |
| `m7b-sprite-wireup` | Wire sprites vào 7 CropSO + 2 AnimalSO + CropTile.prefab + BottomNav |
| `m7c-font-wireup` | 13 TMP LiberationSans → Dosis-Bold SDF |
| `m8-scene-polish` | BUG-10 Camera + LockOverlay wire + BUG-08 Range + bug-backlog cleanup (WRITTEN) |

---

> [!TIP]
> **AI onboarding ở máy mới**:
> 1. Start session → `CLAUDE.md` tự load → có overview
> 2. Đọc section "🚀 Bước tiếp theo" ở file này
> 3. Check `docs/backlog/bug-backlog.md` cho bugs còn open
> 4. Check `.kiro/specs/` cho spec pending execute (hiện có m8)
> 5. Spec convention: 3-file Kiro only, KHÔNG tạo `docs/superpowers/specs/` — chi tiết ở `.kiro/steering/spec-format.md`
