# farm_game_ui_component_spec_unity_handoff_v1

_Source:_ `farm_game_ui_component_spec_unity_handoff_v1.docx`

Farm Game UI Component Spec
and Unity Handoff v1

Production-facing component catalogue, prefab strategy, state rules, data binding, and screen assembly guide
for a 2.5D isometric farm world with 2D UI.

| World Direction<br>2.5D isometric with 2D assets | UI Direction<br>100% 2D HUD, panels, popups | Interaction Rule<br>Tap object -> contextual panel | Phase Scope<br>Farm + storage + barn + events |
| --- | --- | --- | --- |

# 1. Purpose and scope

This document converts the approved UI direction into implementation-ready guidance for UI artists, technical designers, and Unity developers. It is the bridge between visual prompts and real production. It defines reusable UI components, state logic, prefab notes, screen assembly rules, data binding, and handoff expectations for the phase-one build.

- Reference visual direction: bright, lively, rounded farm UI using the locked palette from UI Style Guide v1.

- Reference world direction: 2.5D isometric farm rendered with 2D assets. No true 3D production is assumed.

- Reference interaction rule: the player taps each crop tile, land tile, or animal object directly and receives contextual actions.

# 2. Production principles that all screens must follow

| Principle | What it means in production | Why it matters |
| --- | --- | --- |
| Contextual interactions first | Do not force mode switching for basic crop care or feeding. A tapped object opens only the actions valid for its current state. | Keeps the game readable and casual in phase one. |
| Fast repeated actions | Harvest, sell, and confirm actions should use the fewest possible taps without creating accidental destructive actions. | Core loops repeat many times per day. |
| Readable state over decorative noise | HP, hunger, storage usage, rarity, and warning states must be legible at a glance. | Management gameplay depends on quick scanning. |
| One source of truth for colors | All screens and components must use the locked palette. Warning and danger colors must stay consistent across the app. | Prevents drift between mockups and final production. |
| Reusable prefab library | Every repeating UI object should be assembled from reusable prefabs or nested prefabs. | Speeds production and reduces inconsistency. |

# 3. Design tokens and locked visual references

## 3.1 Core palette tokens

| Token | Hex | Primary use | Notes |
| --- | --- | --- | --- |
| Primary / Farm Green | #69C34D | primary CTA, positive actions, selected state | Default action color |
| Deep Leaf Green | #4FA63A | strong emphasis, active tab, secondary positive accent | Use for pressed/active states |
| Sky Blue | #8ED8FF | top-bar info accents, cool support surfaces, XP support | Use sparingly to keep the UI airy |
| Sun Yellow | #FFD75E | coin, reward, attention without alarm | Main currency accent |
| Warm Soil Brown | #B97A4A | farm-themed secondary accent, earthy framing | Pairs well with cream panels |
| Cream White | #FFF7E8 | panel backgrounds, card surfaces | Default neutral surface |
| Soft Orange | #FFA94D | sell CTA, reward-active accent | Not the main warning token |
| Berry Pink | #FF6FAE | event panels, limited content, fun accent | Use for event energy |
| Aqua Mint | #67DCC8 | recovery/support, water-linked accents | Good for helpful or fresh states |
| Warning | #FFB547 | needs care, near-full capacity, caution | Locked warning color |
| Danger | #FF6B5E | critical HP, imminent death, hard stop | Locked danger color |

## 3.2 World and UI separation

- The farm world is not built as UI. It lives in the 2.5D isometric scene space using 2D sprites, sorting groups, and interaction colliders.

- All menus, HUD, panels, and system messages live on the UI canvas stack as 2D interface layers.

- Scene objects may show lightweight in-world indicators such as HP strips, warning bubbles, or harvest sparkles, but these indicators must still follow the locked UI colors.

## 3.3 Shape language

- Primary panels, buttons, tabs, and cards use rounded rectangles with generous radius and soft shadows.

- Avoid sharp corners, thin hard outlines, or metallic fantasy treatment.

- UI should feel friendly and toy-like, not clinical or hyper-corporate.

# 4. Global Unity UI architecture

## 4.1 Canvas stack

| Layer | Typical objects | Interaction | Notes |
| --- | --- | --- | --- |
| World Scene | farm tiles, crops, animals, decor, scene VFX | tappable world objects | 2.5D isometric scene, not UI |
| HUD Canvas | top bar, bottom nav, event button, small warnings | always visible | screen-space overlay or camera-space |
| Panel Canvas | shops, storage, detail panels | blocks partial input behind it | used for contextual or navigation panels |
| Popup Canvas | confirmations, level-up, rewards, full-storage popups | modal | highest normal interaction priority |
| Toast / FX Canvas | lightweight reward flyups, text toasts | non-blocking | short-lived feedback only |

## 4.2 Suggested Unity folder structure

- Assets/UI/Prefabs/Common/

- Assets/UI/Prefabs/HUD/

- Assets/UI/Prefabs/Shop/

- Assets/UI/Prefabs/Storage/

- Assets/UI/Prefabs/Barn/

- Assets/UI/Prefabs/Popups/

- Assets/UI/Sprites/

- Assets/UI/Materials/

- Assets/UI/Fonts/

- Assets/Scripts/UI/

- Assets/Scripts/UI/ViewModels/

- Assets/Scripts/UI/Binders/

## 4.3 Naming conventions

| Type | Prefix example | Example name |
| --- | --- | --- |
| Prefab | PF_ | PF_PrimaryButton |
| Popup prefab | POP_ | POP_StorageFull |
| Panel prefab | PNL_ | PNL_CropAction |
| Card prefab | CARD_ | CARD_SeedItem |
| View script | View | CropActionPanelView |
| Binder script | Binder | StorageSlotBinder |
| State enum | Enum | CropTileState |

# 5. Shared component catalogue

## 5.1 PrimaryButton

| Purpose | Main positive action such as Plant, Harvest, Unlock, Confirm, and Feed. |
| --- | --- |
| Required content | Label text, optional left icon, optional numeric badge. |
| States / variants | Default, Pressed, Disabled, Loading. |
| Color rules | Default uses Primary Green #69C34D with cream text. Pressed deepens toward #4FA63A. Disabled uses muted neutral. |
| Interaction / behavior | Tap animation with slight depth press. Minimum hit area stays mobile-friendly. |
| Primary bindings | buttonLabel, interactable, loadingState, onClick callback. |
| Prefab recommendation | PF_PrimaryButton with child Text_Label and optional IMG_Icon. |

## 5.2 SecondaryButton

| Purpose | Non-destructive actions such as Close, Back, More Info, and View Details. |
| --- | --- |
| Required content | Label text and optional icon. |
| States / variants | Default, Pressed, Disabled. |
| Color rules | Cream White surface with Warm Soil Brown or Deep Leaf Green text depending on context. |
| Interaction / behavior | Should never visually compete with PrimaryButton on the same modal. |
| Primary bindings | buttonLabel, interactable, onClick. |
| Prefab recommendation | PF_SecondaryButton. |

## 5.3 WarningButton

| Purpose | Actions that change inventory, sell stock, or advance a risky action. |
| --- | --- |
| Required content | Label text, optional coin icon, optional quantity. |
| States / variants | Default, Pressed, Disabled. |
| Color rules | Soft Orange #FFA94D for sell and transactional actions. Critical warnings use Danger only for destructive confirmations. |
| Interaction / behavior | Should be visually separated from positive care actions. |
| Primary bindings | buttonLabel, numericPreview, interactable. |
| Prefab recommendation | PF_WarningButton. |

## 5.4 TopBarStatChip

| Purpose | Shows Gold, Level, EXP, and Storage usage in the persistent HUD. |
| --- | --- |
| Required content | Icon, label or value, optional progress fill. |
| States / variants | Gold chip, EXP chip, Storage chip, Level badge. |
| Color rules | Coin uses Sun Yellow. EXP uses Sky Blue. Storage switches to Warning or Danger as capacity rises. |
| Interaction / behavior | Always visible on Farm and Barn. Light animated pulse on value gain. |
| Primary bindings | currentValue, maxValue, iconSprite, statusState. |
| Prefab recommendation | PF_TopBarStatChip. |

## 5.5 BottomNavTab

| Purpose | Persistent navigation between Farm, Storage, Shop, Barn, and Event entry points. |
| --- | --- |
| Required content | Icon, short label, selected underline or fill. |
| States / variants | Selected, Unselected, Locked, AlertDot. |
| Color rules | Selected uses Primary Green or Deep Leaf Green. Unselected uses cream and brown. Alert dot may use Berry Pink. |
| Interaction / behavior | Must be large enough for thumb interaction. Avoid long labels. |
| Primary bindings | icon, label, selectedState, lockedState, alertState. |
| Prefab recommendation | PF_BottomNavTab. |

## 5.6 FilterTabPill

| Purpose | Filters items in Storage or Shop: All, Seeds, Crops, Feed, Event, and crop duration bands. |
| --- | --- |
| Required content | Short text label, optional count. |
| States / variants | Selected, Unselected, Disabled. |
| Color rules | Selected uses Deep Leaf Green on light background or inverse depending on surface. |
| Interaction / behavior | Group should wrap cleanly on smaller screens. |
| Primary bindings | label, isSelected, countOptional. |
| Prefab recommendation | PF_FilterTabPill. |

## 5.7 SeedItemCard

| Purpose | Represents one crop option inside the Seed Shop. |
| --- | --- |
| Required content | Crop art, name, unlock level, grow time, seed cost, yield, price tag, CTA. |
| States / variants | Unlocked, InsufficientGold, LockedByLevel, Rare. |
| Color rules | Base card uses Cream White. Rare crops add rarity badge and border accent. |
| Interaction / behavior | CTA changes from Plant to Locked or Insufficient Gold. Card must stay readable even with many numbers. |
| Primary bindings | CropData, playerLevel, playerGold, onPlantRequest. |
| Prefab recommendation | CARD_SeedItem. |

## 5.8 StorageItemCard

| Purpose | Displays one inventory entry in the Storage screen or Sell flow. |
| --- | --- |
| Required content | Item icon, name, category, quantity, optional sell value, optional stepper. |
| States / variants | Normal, SelectedForSell, Disabled, NewBadge. |
| Color rules | Uses Cream White with subtle category accents. SelectedForSell uses Primary Green border. |
| Interaction / behavior | Must support fast scan and batch sell workflow. |
| Primary bindings | ItemData, quantity, selectionState, sellPrice. |
| Prefab recommendation | CARD_StorageItem. |

## 5.9 QuantityStepper

| Purpose | Controls how many items the user wants to sell or use. |
| --- | --- |
| Required content | Minus button, value text, plus button, optional max shortcut. |
| States / variants | Normal, DisabledMin, DisabledMax. |
| Color rules | Neutral surface with Primary accent on active controls. |
| Interaction / behavior | Needs low-friction repeated tapping. Long-press acceleration can be added later. |
| Primary bindings | currentValue, minValue, maxValue, onValueChanged. |
| Prefab recommendation | PF_QuantityStepper. |

## 5.10 ProgressBar

| Purpose | Reusable bar for EXP, storage usage, crop HP, or event progress. |
| --- | --- |
| Required content | Background track, fill, optional icon, optional label. |
| States / variants | XP, HP, Capacity, EventProgress. |
| Color rules | XP uses Sky Blue. HP uses green/yellow/red based on thresholds. Capacity shifts from neutral to warning to danger. |
| Interaction / behavior | Smooth fill animation but no long lag. |
| Primary bindings | currentValue, maxValue, stateColorRule. |
| Prefab recommendation | PF_ProgressBar. |

## 5.11 CropStateBadge

| Purpose | In-world indicator or panel indicator for crop care state. |
| --- | --- |
| Required content | Small icon and optional pulse. |
| States / variants | NeedsCare, Weed, Bug, Dry, ReadyToHarvest, Critical. |
| Color rules | General warning uses Warning #FFB547. Critical uses Danger #FF6B5E. ReadyToHarvest can use Sun Yellow or Primary Green glow. |
| Interaction / behavior | Must remain visible but not clutter the field. Prefer one combined badge in phase one when possible. |
| Primary bindings | CropTileState, severityState. |
| Prefab recommendation | PF_CropStateBadge. |

## 5.12 PopupBase

| Purpose | Foundation for system popups such as Level Up, Full Storage, Reward, Confirm Sell, and Warning. |
| --- | --- |
| Required content | Header strip, title, body copy, optional illustration, action row. |
| States / variants | Info, Warning, Reward, Critical Confirm. |
| Color rules | Body uses Cream White. Header accent depends on context: green, yellow, pink, or orange. |
| Interaction / behavior | Modal dimmer behind popup. Title hierarchy must stay consistent across all popups. |
| Primary bindings | titleText, bodyText, iconSprite, buttonSet, accentType. |
| Prefab recommendation | POP_Base. |

## 5.13 RewardCard

| Purpose | Shows a reward from harvest, event, or popup result. |
| --- | --- |
| Required content | Icon, quantity, rarity badge, label. |
| States / variants | Common, Rare, Event, Rescue/Recovery. |
| Color rules | Surface remains light. Rarity handled via border or badge color from the locked rarity palette. |
| Interaction / behavior | Supports small bounce animation on reveal. |
| Primary bindings | ItemData, rewardQty, rarityType. |
| Prefab recommendation | CARD_Reward. |

## 5.14 PenCard

| Purpose | Represents one barn slot or pen on the Barn screen. |
| --- | --- |
| Required content | Pen name, lock state, animal art or empty art, hunger marker, next stage timer, CTA. |
| States / variants | LockedByLevel, Unlockable, EmptyUnlocked, OccupiedGrowing, Hungry, Sellable, NearDeath. |
| Color rules | Barn surface uses cream + brown, with status overlays using warning and danger tokens. |
| Interaction / behavior | Tap opens detail panel. Locked state should communicate both level gate and pen-cost gate. |
| Primary bindings | AnimalData optional, penUnlocked, playerLevel, timer, hungerState. |
| Prefab recommendation | CARD_Pen. |

## 5.15 AnimalDetailPanel

| Purpose | Contextual detail panel for one animal object. |
| --- | --- |
| Required content | Animal art, stage, time to next stage, current sell value, next sell value, hunger requirement, action buttons. |
| States / variants | Normal, Hungry, Mature, NearExpiry. |
| Color rules | Contextual state colors must never override readability of data labels. |
| Interaction / behavior | Should route to Feed or Sell quickly. If feed is missing, surface a direct path to the food shop. |
| Primary bindings | AnimalData, stageState, hungerNeed, inventoryFeedCount, actionAvailability. |
| Prefab recommendation | PNL_AnimalDetail. |

# 6. World object interaction contract

## 6.1 Land tile and crop tile behavior

| Object state | Player tap result | Panel / action | Notes |
| --- | --- | --- | --- |
| Empty land tile | Open seed selection for that tile | PNL_SeedShop contextual mode | No separate tool mode in phase one |
| Growing crop, healthy | Open crop info panel | PNL_CropAction informational mode | Shows timer, HP, and expected result |
| Crop with weed/bug/dry state | Open crop care panel with only valid actions | PNL_CropAction care mode | Only show actions relevant to current issues |
| Ready-to-harvest crop | Immediate harvest or harvest-first contextual panel | Harvest action + reward feedback | Implementation can support one-tap harvest if safe |
| Dead crop or failed tile | Open cleanup / replace state | PNL_CropAction failure mode | Can route user back to replant |

## 6.2 Animal object behavior

| Animal state | Player tap result | Panel / action | Notes |
| --- | --- | --- | --- |
| Locked pen | Show unlock requirements | Unlock popup or panel route | Must show level + cost clearly |
| Empty unlocked pen | Open buy animal flow | PNL_BarnBuy | Choose available animal for that pen type |
| Growing animal, not hungry | Open detail and current valuation | PNL_AnimalDetail | Informational plus optional sell |
| Hungry animal | Open detail panel focused on feed | PNL_AnimalDetail hungry mode | Direct path to food shop if inventory is missing |
| Mature / sellable animal | Open sell-focused detail panel | PNL_AnimalDetail mature mode | Make current sell value prominent |
| Near-expiry animal | Open critical detail state | PNL_AnimalDetail critical mode | Use Danger token sparingly but clearly |

## 6.3 Storage-full contract

- When storage is full, collection actions must be blocked before inventory is silently lost.

- Blocked collection must open POP_StorageFull with two clear actions: Go to Storage and Go to Sell.

- The popup copy should explain that the player must free slots before continuing the blocked action.

# 7. Screen assembly guide

## 7.1 Home Farm Screen

| Assembly area | Required content |
| --- | --- |
| Persistent HUD | TopBarStatChip x4, event button, storage warning label, optional quick gold pulse effect |
| Scene zone | isometric world tiles, crop objects, animal/barn access point, decor, ambient world feedback |
| Bottom navigation | BottomNavTab x5 |
| Context outputs | Crop state badges above tiles, harvest feedback flyouts, mini event entry |
| Primary routes | to Seed Shop contextual panel, Crop Action Panel, Storage, Barn, Event Popup |

Screen note: The Farm screen is the main stage. The world occupies most of the view. The HUD must not visually flatten the 2.5D farm scene.

## 7.2 Seed Shop Panel

| Assembly area | Required content |
| --- | --- |
| Header zone | screen title, close button, category tabs |
| List zone | SeedItemCard repeated in scroll list |
| Footer zone | player gold visibility, optional quick hint for crop duration |
| Entry mode | full panel from bottom nav or contextual mode from empty land tile |
| Primary routes | back to Farm, plant selected crop into tapped tile |

Screen note: The panel should feel light and fast. It is a selection surface, not a dense spreadsheet.

## 7.3 Crop Action Panel

| Assembly area | Required content |
| --- | --- |
| Header zone | crop name, current phase, close button |
| Status block | ProgressBar for HP, timer, perfect window, care status summary |
| Action row | Primary or secondary buttons for valid care actions |
| Forecast block | expected yield tier and warning note if left unattended |
| Primary routes | perform care, harvest, close and return to Farm |

Screen note: Only show relevant actions. Hide unavailable care buttons instead of disabling the whole panel where possible.

## 7.4 Storage Screen

| Assembly area | Required content |
| --- | --- |
| Header zone | title, storage usage stat, close/back |
| Filter row | FilterTabPill group |
| List zone | StorageItemCard repeated grid or list |
| Action strip | sell shortcut, sort, optional use action |
| Primary routes | to Sell Screen, back to Farm or Barn |

Screen note: This screen must optimize scanning and inventory relief. Near-full storage must be obvious.

## 7.5 Sell Screen

| Assembly area | Required content |
| --- | --- |
| Header zone | title, back button, coin accent |
| Item list | StorageItemCard in sell mode with QuantityStepper |
| Summary footer | total selected value, confirm sell button, cancel |
| Primary routes | confirm sell and return to Storage or Farm |
| System link | storage full popup may deep-link here |

Screen note: Keep arithmetic transparent. The player must always understand what is being sold and for how much.

## 7.6 Barn Screen

| Assembly area | Required content |
| --- | --- |
| Persistent HUD | gold, storage, optional grass/worm quick counts |
| Barn content zone | PenCard repeated for each pen type |
| Bottom nav | same global navigation as Farm |
| Primary routes | unlock pen, buy animal, open detail, food shop |
| Context outputs | hunger markers, sell-ready glow, near-death warning |

Screen note: Barn should feel like a second operational layer, not a disconnected mode.

## 7.7 Animal Detail / Feed Panel

| Assembly area | Required content |
| --- | --- |
| Header zone | animal name, stage, close button |
| Center block | animal art, stage timer, lifetime after mature |
| Value block | current sell value, next sell value, XP on sale |
| Feed block | required feed, owned feed count, shortage warning |
| Action row | Feed, Buy Feed, Sell, Close |

Screen note: Hungry state must bias the layout toward the feed path. Mature state must bias toward sell clarity.

## 7.8 Basic Food Shop

| Assembly area | Required content |
| --- | --- |
| Header zone | title, back button, helper hint |
| Item list | simple food item cards for Grass Bundle and Worm Bundle |
| Footer zone | player gold, confirm purchase flow |
| Primary routes | buy feed and return to Animal Detail or Barn |
| Visual note | supportive but not premium; this is a safety net shop |

Screen note: The shop should not visually imply it is the optimal feed source.

## 7.9 Mini Game / Event Popup

| Assembly area | Required content |
| --- | --- |
| Popup shell | PopupBase with event accent |
| Body | short description, reward preview cards, play now CTA |
| Secondary action | skip or later |
| Result flow | reward popup and route back to previous screen |
| Storage rule | if storage full, redirect through storage full contract |

Screen note: Event visuals may use Berry Pink and Aqua Mint accents, but must still sit inside the locked palette.

## 7.10 System Popups

| Assembly area | Required content |
| --- | --- |
| Level Up | new level, unlock summary, route CTA |
| Storage Full | cause, blocked action message, route choices |
| Confirm Sell | clear totals and irreversible note if needed |
| Warning / Critical | crop near death, animal near expiry |
| Reward Result | cards, quantities, positive feedback |

Screen note: Use PopupBase to keep hierarchy stable across the game.

# 8. Data binding map

| UI element | Primary data source | Key fields | Notes |
| --- | --- | --- | --- |
| SeedItemCard | CropData | cropName, unlockLevel, seedCost, growTime, baseYield, sellPricePerUnit, rarity | Player economy and level also affect card state |
| Crop Action Panel | CropTile runtime + CropData | tileState, hp, activeIssues, timeRemaining, perfectWindowRemaining, expectedYieldTier | Runtime state has priority over static data |
| TopBar Gold chip | PlayerProfile / Wallet | gold | Animated on transaction success |
| TopBar EXP chip | PlayerProfile / LevelSystem | currentXp, xpToNext | Needs smooth progress fill |
| TopBar Storage chip | StorageRuntime | usedSlots, totalSlots, warningState | Color changes based on threshold |
| StorageItemCard | InventorySlot + ItemData | itemId, quantity, sellPrice, category | Stepper mode in Sell Screen |
| PenCard | AnimalSlotRuntime + AnimalData | slotState, penUnlockState, hungerState, stage, sellValue | May be empty or locked |
| Animal Detail Panel | AnimalRuntime + AnimalData + Inventory | stage, timeToNextStage, requiredFeed, ownedFeed, currentSellValue, nextSellValue | Routes to food shop if required feed is missing |
| Food Shop item card | ItemData + Wallet | itemName, packSize, buyPrice | No rarity treatment needed in phase one |
| RewardCard | RewardEntry + ItemData | icon, qty, rarity, label | Can appear in popup or mini-event result |

# 9. State tables for implementation

## 9.1 Crop UI state visibility

| Crop state | Visible marker | Main action | Color emphasis | UI note |
| --- | --- | --- | --- | --- |
| Empty | none | Open seed selection | neutral | Treat as land tile, not crop |
| Growing healthy | timer + HP | Open info | green/neutral | No warning badge |
| Needs care | combined care badge | Open care panel | warning | Prefer one combined badge in phase one |
| Ready to harvest | harvest glow + ready badge | Harvest | yellow or green | Can support one-tap harvest |
| Late harvest | late state warning | Harvest | warning | Communicate reduced reward |
| Critical / near death | critical badge + low HP | Care or harvest if allowed | danger | Use red sparingly to preserve impact |
| Dead | dead art state | Cleanup / replace | danger-muted | Not a harvest state |

## 9.2 Animal UI state visibility

| Animal state | Visible marker | Main action | Color emphasis | UI note |
| --- | --- | --- | --- | --- |
| Locked pen | lock overlay | View requirements | neutral | Show level + cost clearly |
| Empty unlocked pen | empty pen art + add CTA | Buy animal | green | Positive expansion state |
| Growing | timer + stage label | View details | neutral | Not a warning state |
| Hungry | feed bubble | Feed | warning | Should not feel like an error if recoverable |
| Mature sellable | sell-ready glow | Sell or wait | yellow/green | Prominent value display |
| Near expiry | critical alert | Sell now | danger | Use only when genuinely urgent |
| Dead / expired | failed state | Clear and reset | danger-muted | Keep copy direct and clear |

# 10. Prefab and script handoff notes

- Keep visual prefab concerns separate from data mapping. Card visuals should not fetch data directly; use a binder or view-model layer.

- Use nested prefabs for repeated button rows, stat chips, and card shells to keep updates centralized.

- Do not hardcode palette values inside many unrelated scripts. Use design tokens through a theme config or a UI style asset where possible.

- The world scene should not directly know panel layout details. World objects should dispatch standardized interaction events such as OpenCropPanel(tileId) or OpenAnimalPanel(slotId).

- Treat Farm and Barn as two main container screens sharing the same HUD and bottom navigation prefabs.

## 10.1 Suggested runtime events

| Event name | Raised by | Consumed by |
| --- | --- | --- |
| OnCropTileTapped(tileId) | world crop tile object | Farm UI controller |
| OnEmptyLandTapped(tileId) | world land tile object | Farm UI controller / Seed Shop |
| OnAnimalTapped(slotId) | world animal or pen object | Barn UI controller |
| OnStorageFull(blockedActionContext) | harvest or reward logic | Popup manager |
| OnLevelUp(newLevel, unlocks) | level system | Popup manager / HUD |
| OnRewardGranted(rewardList) | harvest, event, or mini game | Reward popup + inventory |
| OnInventoryChanged() | inventory service | HUD, Storage, Animal Detail, Sell Screen |

## 10.2 Asset and prefab checklist

- Create a single shared button pack before building complex screens.

- Build TopBarStatChip, BottomNavTab, ProgressBar, PopupBase, and common card shell first; most screens depend on them.

- Lock icon sizing rules early so Seed cards, Storage cards, Reward cards, and Barn cards feel related.

- Separate isometric scene sprite work from UI sprite work, but keep palette and shading language aligned.

- Document every reusable state in the prefab inspector or companion sheet so QA can verify state coverage.

# 11. Production QA checklist

| Check area | What to verify | Pass condition |
| --- | --- | --- |
| Color consistency | Warning and danger are not swapped; event pink does not leak into core warning states | All screens use the locked palette |
| State clarity | Hungry, near death, ready to harvest, and storage full are readable at a glance | No state depends on tiny text alone |
| Tap flow | Every core loop action requires the intended number of taps and no hidden tool mode | Phase-one interactions stay contextual |
| Prefab reuse | Repeated UI objects come from shared prefabs or nested prefabs | No duplicated one-off clones for common patterns |
| Data binding | Displayed values update correctly when inventory, XP, HP, or stage changes | No stale UI after core actions |

# 12. Recommended next step

The strongest next step after this handoff is a concrete Unity-facing build map: one screen at a time, list the prefab tree, script entry points, and required art assets for Home Farm, Storage, Barn, and Seed Shop first. That turns this document from a design-system guide into a sprint-ready implementation pack.
