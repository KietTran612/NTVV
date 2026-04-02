# farm_game_ui_style_guide_v2

_Source:_ `farm_game_ui_style_guide_v2.docx`

Farm Game UI Style Guide v2

This update aligns all UI deliverables with the current project locks: landscape-first, base resolution 1920×1080, one gameplay scene, world rendered as 2.5D isometric using 2D assets, and 2D UI popup/panel overlays for phase-one interaction.

| Orientation<br>Landscape | Base frame<br>1920×1080 | World architecture<br>One scene, pan camera | Interaction pattern<br>Contextual popup / panel |
| --- | --- | --- | --- |

What is newly locked in this version

• Landscape-first is the only approved layout direction for phase one. Treat 1920×1080 as the base authoring frame for every mockup, prompt and handoff.

• The player remains inside one gameplay scene. Crop area, animal area and storage building exist in the same world, with pan movement between them.

• Feature interactions open as popup or side-panel overlays. Phase one does not split crop care, storage, selling or animal detail into separate scenes.

• World art stays 2.5D isometric fake 3D using 2D assets; the UI remains fully 2D.

Core visual direction

The visual target is a cheerful, warm and readable farm game. The world should feel like a handcrafted isometric diorama with strong silhouettes and light depth cues, while the UI should stay clean, rounded and quick to scan on a horizontal mobile screen. Farm ownership and continuity are important: the player should feel that all actions happen inside one continuous place.

Locked palette

| Token | Hex | Use | Preview |
| --- | --- | --- | --- |
| Farm Green | #69C34D | Primary CTA, healthy states |  |
| Deep Leaf Green | #4FA63A | Selected states, heading accents |  |
| Sky Blue | #8ED8FF | HUD freshness, info surfaces |  |
| Sun Yellow | #FFD75E | Coins, rewards, XP accents |  |
| Warm Soil Brown | #B97A4A | Farm material accents, subheads |  |
| Cream White | #FFF7E8 | Panels, cards, popup bases |  |
| Soft Orange | #FFA94D | Sell CTA, positive commerce accent |  |
| Berry Pink | #FF6FAE | Event highlight, limited reward |  |
| Aqua Mint | #67DCC8 | Water/support/recovery accent |  |
| Warning | #FFB547 | Needs care, near full, caution |  |
| Danger | #FF6B5E | Near death, blocked, urgent danger |  |

Landscape 1920×1080 composition rules

• Keep the top HUD within a safe band so it never hides the center interaction area.

• Reserve the center third of the frame for tap targets in the world: crop tiles, pens, animals and buildings.

• Popups open centered. Side panels may slide in from the right when the world should remain visible below.

• Do not build portrait-style stacks inside a landscape frame. Use horizontal grouping and balanced empty space.

• On wide devices, expand breathing room before increasing component scale.

World versus UI split

• World layer: terrain, crop rows, plants, pens, animals, storage building, decorative props.

• HUD layer: level, XP, gold, storage occupancy, menu shortcuts, event marker.

• Popup layer: seed selection, crop care, storage, selling, animal detail, feed shop, reward popups.

• System layer: loading, saving, dim overlays and future notification rails.

Interaction language for phase one

• The player taps an individual crop tile, plant, pen, animal or building and sees only the actions valid for that exact state.

• Global quick tools are not part of the phase-one base UX. They may be added later as convenience features after the core loop proves stable.

• Harvest and feed actions should feel immediate. Repetitive actions should minimize confirmation overhead.

• After harvest, the tile becomes reusable immediately in phase one rather than requiring a separate cleanup step.

Consistency rules

• Warning uses #FFB547 only. Soft Orange #FFA94D is reserved for sell / positive commerce emphasis and must not replace warning orange.

• Danger uses #FF6B5E only for urgent death-risk and blocked states.

• All updated UI files should cite this style guide as the palette source of truth.

• Any new prompt or layout spec must explicitly mention one gameplay scene, world panning and popup-based feature access.
