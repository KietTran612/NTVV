# farm_game_wireframe_color_mapping_v2

_Source:_ `farm_game_wireframe_color_mapping_v2.docx`

Farm Game Wireframe Text + Color Mapping v2

This update aligns all UI deliverables with the current project locks: landscape-first, base resolution 1920×1080, one gameplay scene, world rendered as 2.5D isometric using 2D assets, and 2D UI popup/panel overlays for phase-one interaction.

| Orientation<br>Landscape | Base frame<br>1920×1080 | World architecture<br>One scene, pan camera | Interaction pattern<br>Contextual popup / panel |
| --- | --- | --- | --- |

Architecture assumptions used below

• One scene only: the farm world includes crop land, animal pens and the storage building in the same isometric map.

• Camera movement is horizontal and diagonal panning inside world bounds, initiated by drag.

• Every screen definition below is an overlay or panel on top of the same gameplay scene unless noted as HUD-only.

• Base frame is landscape 1920×1080.

Screen 1 - Home Farm

| Area | Placement | Content | Color mapping | Notes / state rules |
| --- | --- | --- | --- | --- |
| Top HUD | Top full-width safe band | Avatar shortcut, level chip, XP bar, gold, storage occupancy, settings/event badges | HUD base uses Cream White cards with Deep Leaf Green titles, Sky Blue XP fill, Sun Yellow coin accent, Warning when storage nears full | Keep the HUD shallow. It must never cover the primary crop interaction band. |
| World focus zone | Center and mid-lower frame | Visible crop tiles, plant states, pen silhouettes, storage building entry point, decorative landmarks | World colors follow natural farm palette. State overlays use Warning and Danger only where needed | This is the tappable zone. Maintain clean negative space around interactable objects. |
| Bottom helper band | Bottom edge, light footprint | Optional navigation chips, focus shortcuts to crop area, animal area, storage if later needed | Use Cream White chip bases, Deep Leaf Green selected states | Do not overbuild this band in phase one. |
| Event marker | Upper-right floating zone | Mini game/event badge, not a full menu strip | Berry Pink accent with Sun Yellow reward spark only for event-specific emphasis | Should read as optional, not mandatory. |
| State callouts | Near tapped objects or edge notifications | Needs care marker, ripe marker, full storage warning, near-death warning | Warning for care and near full, Danger for near-death, Farm Green for ready/healthy | Keep callouts small and context-tied so the world remains readable. |

Screen 2 - Seed Shop Panel

| Area | Placement | Content | Color mapping | Notes / state rules |
| --- | --- | --- | --- | --- |
| Panel shell | Centered popup or right-side panel | Seed list, category filter, close button | Cream White base, Deep Leaf Green title, Warm Soil Brown section labels | Use popup form when opened from world tile; use right panel when opened from HUD shortcut. |
| Filter row | Top inside panel | All, short, medium, long, rare | Selected filter uses Farm Green fill; unselected uses Cream White with Warm Soil Brown text | Do not overcrowd with too many tabs in phase one. |
| Seed cards | Main scroll area | Seed icon, name, unlock level, price, grow time, base yield, buy/grow action | Common cards stay cream; rare accent may use Sky Blue or Berry Pink badge only where relevant | Locked seeds should use Disabled overlay and level note, not red danger. |
| Primary action | Bottom of each seed card | Plant / Buy and plant | Farm Green primary button | If the player lacks gold, disable the button and keep the card readable. |

Screen 3 - Crop Action Panel

| Area | Placement | Content | Color mapping | Notes / state rules |
| --- | --- | --- | --- | --- |
| Header block | Top of popup | Crop name, phase, timer, HP state | Cream White base, Deep Leaf Green title, HP bar uses Green/Warning/Danger progression | Panel is contextual and only shows valid actions. |
| Status row | Below header | Weed, bug, water, ripe, perfect harvest timing | Warning for active care needs; Farm Green for healthy / ready; Danger only for near-death | Avoid showing inactive icons just for symmetry. |
| Action buttons | Bottom of panel | Remove weed, catch bug, water, harvest | Farm Green for main valid action, Aqua Mint allowed on water support action, Soft Orange only on sell-like outcomes if any future extension | One object, one contextual panel. No global mode switching here. |
| Yield hint | Small helper text | Expected outcome quality or perfect window note | Warm Soil Brown helper text, Sun Yellow used for perfect bonus note only | Should inform without becoming another large card. |

Screen 4 - Storage / Warehouse

| Area | Placement | Content | Color mapping | Notes / state rules |
| --- | --- | --- | --- | --- |
| Panel shell | Centered large popup | Storage title, occupancy, tabs, item grid/list | Cream White base, Warm Soil Brown category accents, Warning on near full, Danger when full | May also be opened by tapping the storage building in world or by HUD shortcut. |
| Capacity strip | Top panel row | Used slots / total slots and full warning | Cream White strip with Warning or Danger state fill on the badge only | Do not flood the entire panel background with red. |
| Tabs | Below capacity | All, Seeds, Crops, Feed, Event items | Selected tab uses Farm Green; unselected tab uses Cream White with Warm Soil Brown text | Tabs should fit in one row at 1920×1080 base. |
| Item cards | Main grid/list area | Icon, quantity, stack, optional quick sell or use action | Cream White cards with subtle borders; rarity badge optional | Text must remain readable and never hug borders. |
| Footer actions | Bottom row | Close, Sell selected, maybe Use item | Sell uses Soft Orange, close stays secondary cream | Storage action hierarchy should feel calm, not alarm-heavy. |

Screen 5 - Sell Screen

| Area | Placement | Content | Color mapping | Notes / state rules |
| --- | --- | --- | --- | --- |
| Popup shell | Centered popup over dimmed world | Selected item list, quantity steppers, total gold preview | Cream White base, Warm Soil Brown labels | This is a management popup, not a separate scene. |
| Item rows | Main content block | Item icon, quantity owned, quantity to sell, unit value | Cream White rows with subtle separators | Use horizontal alignment that favors quick reading of quantities and values. |
| Total value strip | Bottom summary zone | Total gold to receive | Sun Yellow chip or panel accent with Text Dark value | This should visually read as a reward moment. |
| Confirm action | Bottom-right primary action | Sell selected | Soft Orange primary commerce button | Keep it distinct from Farm Green harvest or planting actions. |

Screen 6 - Barn / Animal Area

| Area | Placement | Content | Color mapping | Notes / state rules |
| --- | --- | --- | --- | --- |
| World animal zone | Center world area after pan | Pens, unlocked or locked animal housing, animals in place | World uses natural farm palette; pen state callouts use Warning for hunger and Danger for near death | This remains part of the same gameplay scene, reached by camera pan. |
| Top HUD persistence | Top safe band | Gold, level, XP, storage, quick feed resources | Reuse Home Farm HUD tokens for consistency | The HUD should not restyle itself per area. |
| Pen state labels | On or above pens | Locked, unlock available, hungry, ready to sell, mature | Disabled for locked, Farm Green for active healthy, Warning for hungry, Danger for death-risk | State text should be brief and icon-supported. |
| Open detail action | On tap of pen or animal | Opens animal detail panel | No extra global toolbar in phase one | Tap target should include animal and pen frame area. |

Screen 7 - Animal Detail / Feed Panel

| Area | Placement | Content | Color mapping | Notes / state rules |
| --- | --- | --- | --- | --- |
| Header | Top of popup | Animal name, stage, growth timer, mature lifetime remaining | Cream White base, Deep Leaf Green title | Animal detail opens above the same world scene. |
| Status row | Middle upper | Hungry / fed / mature / near death plus feed requirement | Warning for hungry, Farm Green for okay, Danger for near death | Only the current need should dominate visually. |
| Value block | Mid panel | Sell value now versus next stage value | Sun Yellow for value emphasis, Warm Soil Brown labels | Makes the timing decision readable. |
| Action row | Bottom of panel | Feed, Sell, Go to food shop when needed | Feed uses Farm Green, Sell uses Soft Orange, emergency shop fallback stays cream with Aqua Mint accent | Keep the action hierarchy obvious: care first, sell second. |

Screen 8 - Basic Food Shop

| Area | Placement | Content | Color mapping | Notes / state rules |
| --- | --- | --- | --- | --- |
| Panel shell | Centered popup or side panel | Grass bundle and worm bundle | Cream White base, Aqua Mint support accent, Warm Soil Brown copy | This is a safety net panel, not a major economy store. |
| Item cards | Main content | Pack size, price, buy button | Cream White cards, price highlighted with Sun Yellow coin token | Should feel helpful, not premium. |
| Education note | Small copy block | Explains that farm-generated feed is still more efficient | Aqua Mint helper strip or note box | This keeps the safety-net role clear. |

Screen 9 - Mini Game / Event Popup

| Area | Placement | Content | Color mapping | Notes / state rules |
| --- | --- | --- | --- | --- |
| Popup shell | Centered, temporary overlay | Event title, quick description, reward promise, play button | Berry Pink event accent, Cream White body, Sun Yellow reward highlights | Keep the world visible under a dim so the player feels they remain in the farm session. |
| Reward preview | Mid content | Seed bundles, small coin reward, feed bundles, speed-up | Sun Yellow and Berry Pink for celebratory emphasis | The panel should signal comeback support without looking like a monetization wall. |
| Action buttons | Bottom | Play now, later, close | Farm Green for play, secondary cream for defer | Short and inviting. |

Screen 10 - System Popups

| Area | Placement | Content | Color mapping | Notes / state rules |
| --- | --- | --- | --- | --- |
| Level up popup | Centered modal | New level, unlocked items or pen, continue button | Cream White base, Sun Yellow celebration accent, Deep Leaf Green title | Keep concise; use celebration without blocking too long. |
| Storage full popup | Centered warning modal | Message, go to storage, go to sell | Warning dominant, Danger only if action is blocked right now | This popup must quickly route the player to recovery. |
| Near death crop/animal alert | Floating contextual modal or banner | Urgent reminder on selected object | Danger accent with Text Dark body | Use sparingly; should not become a constant red storm. |
