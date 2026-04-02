# farm_game_visual_prompts_v3_landscape_update

_Source:_ `farm_game_visual_prompts_v3_landscape_update.docx`

Farm Game Visual Prompts v3 - Landscape / One-Scene Update

This update aligns all UI deliverables with the current project locks: landscape-first, base resolution 1920×1080, one gameplay scene, world rendered as 2.5D isometric using 2D assets, and 2D UI popup/panel overlays for phase-one interaction.

| Orientation<br>Landscape | Base frame<br>1920×1080 | World architecture<br>One scene, pan camera | Interaction pattern<br>Contextual popup / panel |
| --- | --- | --- | --- |

How to use this document

• Every prompt below is written for landscape 1920×1080 screen generation.

• Every prompt assumes one gameplay scene. Crop area, animal area and storage are part of the same isometric farm world; panels and popups open above it.

• Use the palette from UI Style Guide v2 as the source of truth. Warning is #FFB547 and Danger is #FF6B5E. Do not substitute them with generic orange or red wording.

• The world must read as 2.5D isometric fake 3D using 2D assets. The UI must remain fully 2D.

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

Shared positive prompt core for every screen

Bright casual farm game, landscape 1920×1080, one continuous 2.5D isometric farm world built with 2D assets, clean 2D mobile UI, rounded cards, cream popup surfaces, Farm Green primary actions (#69C34D), Warning orange (#FFB547) for care states, Danger coral (#FF6B5E) for urgent death-risk, Sun Yellow (#FFD75E) for coins and rewards, warm readable typography, cheerful but not noisy composition, balanced spacing, no portrait stacking inside landscape frame.

Shared negative prompt core for every screen

Do not depict separate gameplay scenes for storage, animal management, shop or crop care in phase one. Do not use 3D models, free camera rotation or portrait-first layouts. Do not replace warning orange with soft orange. Do not overfill the screen with glowing effects, thick fantasy metal frames, dark UI or neon palette.

Prompt 1 - Home Farm

Positive prompt

Design a landscape 1920×1080 home farm screen for a mobile casual farming game. Show one continuous 2.5D isometric farm world using only 2D assets. The crop area is centered, pens and a storage building are visible as secondary landmarks within the same scene, and the player can clearly imagine panning across the farm. Place a shallow 2D HUD at the top with avatar, level chip, XP bar, gold and storage count. Keep the center clear for crop interaction. Add small event and state markers without turning the screen into a menu wall. The UI should be cheerful, warm and very readable.

Negative prompt

Do not make the crop area a separate screen from the pen area. Do not show a portrait HUD stretched sideways. Do not use dark fantasy materials or generic orange warning states.

Color focus

HUD cards use Cream White; headings and selected tokens use Deep Leaf Green; XP bar uses Sky Blue; gold uses Sun Yellow; care alerts use Warning; urgent crop death signals use Danger.

Prompt 2 - Seed Shop Panel

Positive prompt

Design a seed shop panel that opens above the same farm scene in landscape 1920×1080. It may appear as a centered popup or a right-side panel. Show category filters, seed cards, unlock levels, prices, grow times and a clear plant action. The panel should feel lightweight and contextual, not like a separate game mode. Keep the world faintly visible behind the overlay.

Negative prompt

Do not design this as a full standalone store scene. Do not bury the plant action in excessive decoration.

Color focus

Panel body uses Cream White; selected tabs and primary plant buttons use Farm Green; labels use Warm Soil Brown; rare accents may use Sky Blue or Berry Pink sparingly; locked states use Disabled overlay.

Prompt 3 - Crop Action Panel

Positive prompt

Design a compact contextual crop action panel for a tapped plant inside the main farm scene. The panel shows crop name, growth phase, timer, HP bar, active care needs and only the actions valid right now: remove weed, catch bug, water or harvest. It should feel fast, readable and closely tied to the selected crop tile.

Negative prompt

Do not show every possible action if the crop state does not need it. Do not convert the whole screen into a management dashboard.

Color focus

Panel base uses Cream White; healthy states and valid primary actions use Farm Green; water support may use Aqua Mint accent; active care issues use Warning; near-death state uses Danger; helper labels use Warm Soil Brown.

Prompt 4 - Storage and Sell

Positive prompt

Design a storage popup and a sell popup for the same game. Both open above the one-scene farm world. Storage shows tabs, item cards, occupancy and quick routes to selling. Sell shows selected item rows, quantities and a clear total gold summary. The UI should feel calm and managerial rather than dramatic.

Negative prompt

Do not design storage as a detached warehouse screen. Do not use danger red as the default commerce color.

Color focus

Storage cards and popup shell use Cream White; occupancy warning uses Warning only when needed; total value and coin chips use Sun Yellow; sell confirmation button uses Soft Orange; text and section labels use Warm Soil Brown and Text Dark.

Prompt 5 - Barn and Animal Detail

Positive prompt

Design a landscape 1920×1080 barn area and animal detail overlay for the same farm. The barn remains part of the one continuous isometric farm world. Pens sit in-world, with labels for locked, hungry, ready to sell or mature states. Tapping a pen opens an animal detail panel showing stage, growth timer, feed requirement and sell values.

Negative prompt

Do not isolate the barn into a separate map or separate scene. Do not make the animal detail panel visually heavier than the farm itself.

Color focus

In-world states use Farm Green for healthy, Warning for hungry, Danger for near death. Animal detail popup uses Cream White body, Deep Leaf Green title, Sun Yellow value accents, Soft Orange sell action and Farm Green feed action.

Prompt 6 - Food Shop and Event Popup

Positive prompt

Design a small feed shop panel and a mini game/event popup that both sit above the same farm scene. The feed shop is a safety-net helper with grass and worm bundles. The event popup feels cheerful and comeback-friendly, offering seed bundles, feed bundles or small support rewards.

Negative prompt

Do not make the feed shop look like a premium monetization page. Do not make the event popup so loud that it visually breaks the calm farm identity.

Color focus

Feed shop uses Cream White with Aqua Mint support accents and Sun Yellow price markers. Event popup uses Cream White body, Berry Pink event accent, Sun Yellow reward highlight, Farm Green play button, Warning only if the event is framed as rescue or shortage support.
