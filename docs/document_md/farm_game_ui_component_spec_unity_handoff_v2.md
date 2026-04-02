# farm_game_ui_component_spec_unity_handoff_v2

_Source:_ `farm_game_ui_component_spec_unity_handoff_v2.docx`

Farm Game UI Component Spec / Unity Handoff v2

This update aligns all UI deliverables with the current project locks: landscape-first, base resolution 1920×1080, one gameplay scene, world rendered as 2.5D isometric using 2D assets, and 2D UI popup/panel overlays for phase-one interaction.

| Orientation<br>Landscape | Base frame<br>1920×1080 | World architecture<br>One scene, pan camera | Interaction pattern<br>Contextual popup / panel |
| --- | --- | --- | --- |

Production architecture update

• One gameplay scene only for phase one. Do not create separate gameplay scenes for storage, crop action, animal detail or feed shop.

• World interaction lives in a 2.5D isometric map built from 2D assets. UI lives in 2D canvases layered above the world.

• Recommended runtime split: WorldRoot, HUDCanvas, PopupCanvas, SystemCanvas.

• Base design frame is landscape 1920×1080 and every prefab must be authored with landscape anchoring in mind.

Canvas and prefab structure

• HUDCanvas: top HUD bars, persistent counters, event shortcut, optional bottom helper chips.

• PopupCanvas: modal popups, side panels, contextual action panels, reward popups.

• SystemCanvas: loading, fades, blockers, save indicators, tutorial dim overlays later if needed.

• WorldRoot: crop tiles, plant visuals, pen objects, animals, buildings and world click colliders.

Key reusable components

| Component | Purpose | Major states | Color rules | Data binding notes |
| --- | --- | --- | --- | --- |
| TopHUDBar | Persistent resource and progression display | default, nearFullStorage | Cream cards, Sky Blue XP, Sun Yellow gold, Warning badge on storage only | Binds player level, XP, gold, storage usage |
| CropTileInteractable | Tap target in world for each crop tile | empty, growing, needsCare, ripe, dying | World object; state overlays use Warning / Danger only as needed | Binds crop tile state, plant data, timer, HP |
| ContextActionPanel | Popup opened from tapped crop or object | cropCare, harvest, emptyTilePlant, buildingOpen | Cream base; primary action Farm Green; Warning/Danger for state callouts | Receives selected world object context |
| StoragePopup | Inventory management overlay | default, nearFull, full | Cream base; Warning on near full; Danger on blocked collection state only | Binds item list, stack counts, tabs, capacity |
| SellPopup | Commerce confirmation overlay | default, disabledNoSelection | Cream base; Soft Orange confirm; Sun Yellow total value | Binds selected items, unit values, total payout |
| PenWorldState | In-world pen state badge / marker | locked, empty, hungry, mature, nearDeath | Disabled, Farm Green, Warning, Danger based on state | Binds animal stage and condition |
| AnimalDetailPopup | Animal management overlay | fed, hungry, mature, nearDeath | Cream base; feed Farm Green; sell Soft Orange; hunger Warning; death-risk Danger | Binds selected animal data |
| FeedShopPopup | Safety-net feed purchase panel | default | Cream base with Aqua Mint support accent | Binds shop packs and prices |
| EventPopup | Mini game or rescue event overlay | standard, rescue, rewardResult | Cream base; Berry Pink event accent; Sun Yellow reward highlight | Binds event metadata and reward preview |

State and scene rules

• A tap on a world object should never force a scene load in phase one. It should open a popup, side panel or direct action within the same scene.

• Crop state changes should update both the world visual and the contextual panel if open.

• Animal state changes should update in-world pen markers and the animal detail popup if open.

• Storage-full blocking should route the player toward StoragePopup or SellPopup, not to a separate management scene.

Anchoring and landscape layout rules for Unity

• Anchor HUD to top-safe bands and corners. Avoid center anchors for persistent HUD blocks.

• Center modals should cap width so they do not become over-wide on landscape tablets. Side panels may use right-anchor with fixed comfortable width.

• PopupCanvas should allow world dimming without destroying readability of the farm scene underneath.

• All prefabs should be tested at 1920×1080 first, then widened with margin growth rather than arbitrary scale inflation.

Files superseded by this update

• Any earlier UI spec that implies feature-by-feature scene separation is superseded by this file.

• Any earlier color note that uses generic orange for warnings is superseded by the palette lock defined here and in UI Style Guide v2.
