# farm_game_visual_prompts_v1

_Source:_ `farm_game_visual_prompts_v1.docx`

FARM GAME - SCREEN-BY-SCREEN VISUAL PROMPTS

Bộ prompt dựng màn hình cho AI UI tool · Version 1

Mục tiêu của tài liệu này là chuyển toàn bộ định hướng gameplay, phong cách UI 2D và world 2.5D isometric giả 3D thành bộ visual prompt có thể copy dùng ngay cho AI UI tool hoặc làm brief cực chi tiết cho UI artist.

# 1. Khóa định hướng chung

| Hạng mục | Quyết định khóa |
| --- | --- |
| UI | 2D hoàn toàn; panel, button, popup, icon, HUD đều là flat-to-soft casual UI. |
| World | 2.5D isometric giả 3D bằng asset 2D; không dùng 3D thật. |
| Tone | Tươi sáng, sinh động, dễ thương, đọc nhanh, casual mobile, hơi hoài niệm farm game cũ. |
| Interaction | Phase đầu click trực tiếp từng object; panel hiển thị theo ngữ cảnh trạng thái hiện tại. |
| Mini game | Là lớp cứu hộ/comeback và phần thưởng tích cực; không thay thế core farm. |

# 2. Bộ màu khóa dùng xuyên suốt

| Tên màu | Hex | Vai trò chính | Ghi chú visual |
| --- | --- | --- | --- |
| Farm Green | #69C34D | Primary CTA, selected tab, positive action | Nút Gieo, Thu hoạch, Mở chuồng |
| Deep Leaf Green | #4FA63A | Heading accent, hover, selected border | Dùng để neo nhận diện chính |
| Sky Blue | #8ED8FF | Top bar info, XP, info state | Làm UI thoáng và sạch |
| Sun Yellow | #FFD75E | Currency, reward, highlight | Coin, bonus, event glow nhẹ |
| Warm Soil Brown | #B97A4A | Earth tone, table accent, farm texture | Hợp theme nông trại |
| Cream White | #FFF7E8 | Panel background chính | Giữ giao diện sáng, mềm |
| Soft Orange | #FFA94D | Warning nhẹ, sell action, active reward | Không dùng thay đỏ quá nhiều |
| Berry Pink | #FF6FAE | Event, mini game, special reward | Giữ mức dùng vừa phải |
| Aqua Mint | #67DCC8 | Water, healing, support item | Rất hợp thao tác tưới nước |

# 3. Cách dùng tài liệu prompt này

- Mỗi màn hình bên dưới có 5 phần: mục tiêu màn, những gì phải nhìn thấy, color mapping, prompt chính, negative prompt.

- Prompt chính nên được copy nguyên khối vào AI UI tool; nếu tool không chịu prompt dài, dùng phần Must-show + Color mapping + Prompt rút gọn.

- Tất cả prompt đều phải giữ đúng rule: world 2.5D isometric giả 3D bằng asset 2D, còn UI là 2D bo tròn mềm.

- Không thêm 3D thật, không dùng dark UI, không dùng sci-fi/fantasy UI, không làm quá neon.

# 4. Screen 01 - Home Farm Screen (màn trung tâm)

## Mục tiêu màn

Màn hình quan trọng nhất; cho người chơi nhìn thấy nông trại, thao tác lên từng ô đất/cây trồng, thấy vàng/EXP/kho/event, và di chuyển sang các hệ khác.

## Những gì bắt buộc phải thấy

- Góc nhìn world là 2.5D isometric giả 3D; nền cỏ sáng, đường đi đất ấm, ruộng ô đất xếp rõ, có chiều sâu nhẹ.

- Top bar 2D hiển thị Avatar nhỏ, Level, thanh EXP, Gold, dung lượng Kho, Settings.

- Bottom navigation 2D gồm Farm, Kho, Shop, Chuồng, Event.

- Ruộng cây thể hiện nhiều trạng thái: ô trống, đang lớn, cần chăm sóc, chín, gần chết.

- Icon hoặc bubble trạng thái phải dễ đọc: sâu, cỏ, thiếu nước, ready harvest, storage full, event available.

- Một khu nhỏ phía xa hoặc cạnh trên có thể nhìn thấy chuồng/nông trại phụ để gợi nhắc progression, nhưng không lấn core ruộng.

## Color mapping cho màn này

| Khu vực / thành phần | Màu và cách áp dụng |
| --- | --- |
| Nền farm / cỏ | Gradient xanh sáng pha xanh lá nhẹ; ưu tiên tươi, thoáng. |
| Ô đất | Nâu đất ấm #B97A4A, highlight nhẹ viền trên; khi gieo xong màu đất đậm hơn một chút. |
| Top bar | Cream White #FFF7E8 + accent Sky Blue #8ED8FF và Sun Yellow #FFD75E. |
| CTA tích cực | Farm Green #69C34D. |
| Cảnh báo chăm sóc | Soft Orange #FFA94D; sắp chết dùng Danger coral-red nhẹ. |
| EXP / info | Sky Blue #8ED8FF và Aqua Mint #67DCC8. |

## Prompt chính để copy vào AI UI tool

Design a mobile farm game Home Farm screen in bright casual style. The world view must be 2.5D isometric fake-3D made with 2D assets, while the interface is fully 2D. Show a cheerful farm field with neat crop plots, soft rounded casual UI, warm sunlight feeling, bright grass, warm soil, and readable mobile HUD. The top bar should show player avatar, level, EXP bar, gold, storage capacity, and settings in compact rounded capsules. The main center area must focus on crop plots with different states: empty plot, growing crop, crop needing care, ripe crop, and crop near death. Use very clear visual state signals without making the screen noisy. Add small state bubbles or icons for weeds, bugs, lack of water, and ready-to-harvest. The bottom navigation should contain Farm, Storage, Shop, Barn, and Event. Keep all buttons soft rounded, readable, colorful but controlled. Use Farm Green as primary action color, Cream White for panels, Sky Blue for information, Sun Yellow for rewards, Warm Soil Brown for earth surfaces, and Soft Orange for warning. The overall feeling should be lively, warm, accessible, and slightly nostalgic like a classic social farm game modernized for mobile. Do not make it hyper-detailed or realistic; keep it stylized, cute, clear, and production-friendly.

## Negative prompt / điều không được làm sai

No true 3D models, no dark mode, no neon cyber colors, no realistic farming simulator look, no tiny unreadable icons, no metallic fantasy UI, no cluttered overlays, no aggressive glow, no heavy black shadows, no camera rotation feel, no square hard-edged UI.

# 5. Screen 02 - Seed Shop Panel

## Mục tiêu màn

Panel chọn hạt giống từ ô đất trống hoặc từ tab Shop; phải dễ so sánh, dễ mua, dễ hiểu level unlock.

## Những gì bắt buộc phải thấy

- Panel 2D dạng sheet hoặc popup lớn phủ một phần màn hình; nền kem sáng, bo góc lớn.

- Danh sách card hạt giống có icon cây, tên, level mở, giá hạt, thời gian lớn, sản lượng cơ bản, tag ngắn hạn/trung hạn/dài hạn/hiếm.

- Card hạt giống hiếm có viền rarity rõ nhưng tinh tế; chưa đủ level thì card mờ + khóa.

- Nút Mua/Gieo dùng màu xanh chủ đạo, trạng thái thiếu vàng phải disabled rõ.

## Color mapping cho màn này

| Khu vực / thành phần | Màu và cách áp dụng |
| --- | --- |
| Nền panel | Cream White #FFF7E8 |
| Header panel | Sun Yellow #FFD75E + text nâu ấm |
| Card thường | Kem nhạt + viền xanh lá rất nhẹ |
| Card hiếm | Viền Rare/Epic tùy rarity, glow nhẹ không quá chói |
| Button mua | Farm Green #69C34D |
| Locked state | Neutral beige/gray + overlay nhẹ |

## Prompt chính để copy vào AI UI tool

Create a Seed Shop panel for a casual mobile farm game. The UI is fully 2D, layered over a 2.5D isometric farm background. The panel should feel soft, bright, and organized, like a clean market catalog inside a farm game. Use large rounded corners, cream-white background, warm brown text, and green call-to-action buttons. Each seed card must show crop icon, crop name, unlock level, seed cost, grow time, base yield, and a simple tag like short, medium, long, or rare. Make comparison easy at a glance. Rare seeds should have a distinct but tasteful border color, while locked seeds are faded with a level requirement label. Buying or planting buttons must be visually obvious and friendly. The panel should feel efficient and easy to scan, with roomy spacing and readable numbers. Keep the visual language consistent with a bright cheerful farm game, not a generic e-commerce layout.

## Negative prompt / điều không được làm sai

No overly dense table layout, no tiny text, no dark marketplace theme, no realistic seed packaging, no sci-fi tags, no huge paragraphs, no glossy casino look.

# 6. Screen 03 - Crop Action Panel

## Mục tiêu màn

Panel thao tác theo ngữ cảnh cho một ô cây; phải đổi đúng theo trạng thái hiện tại của cây.

## Những gì bắt buộc phải thấy

- Hiển thị tên cây, phase, thời gian còn lại, thanh HP, trạng thái hiện tại.

- Chỉ hiện các nút đúng với context: Cắt cỏ, Bắt sâu, Tưới nước, Thu hoạch, hoặc xem thông tin.

- Nếu cây chín phải nhìn thấy khung thu hoạch đẹp và mức sản lượng ước tính.

- Nếu cây gần chết phải có màu cảnh báo nhưng vẫn giữ cảm giác casual, không quá đáng sợ.

## Color mapping cho màn này

| Khu vực / thành phần | Màu và cách áp dụng |
| --- | --- |
| Nền panel | Cream White |
| Thanh HP | Green → Yellow → Coral-red |
| Action chăm sóc | Aqua Mint cho tưới, Soft Orange cho cắt cỏ, Sun Yellow/Green cho thu hoạch |
| Thông tin timer | Sky Blue |
| Cảnh báo nguy hiểm | Coral-red nhẹ, không dùng đỏ gắt |

## Prompt chính để copy vào AI UI tool

Design a contextual Crop Action Panel for a mobile farm game. This is a compact 2D popup panel that appears when the player taps a crop in the isometric farm world. The panel must clearly show crop name, current growth stage, remaining time, health bar, and current care problems. It should dynamically adapt so that only relevant actions appear: remove weeds, catch bugs, water plant, harvest, or simply view status. Keep the layout very readable and action-oriented. The health bar should use green, yellow, and soft coral-red to indicate crop condition. If the crop is ripe, show a highlighted perfect harvest window and estimated harvest quality in a friendly way. If the crop is near death, use warning emphasis without making the game feel harsh. The panel should feel like a smart farm management helper: compact, colorful, soft-cornered, easy to understand, and clearly tied to the player tapping one object.

## Negative prompt / điều không được làm sai

No giant modal with too much text, no technical dashboard look, no multiple unnecessary buttons, no hard red alarms, no realistic agriculture interface.

# 7. Screen 04 - Storage / Warehouse

## Mục tiêu màn

Màn quản lý kho; phải đọc nhanh, lọc nhanh, và xử lý tình trạng kho đầy thuận tiện.

## Những gì bắt buộc phải thấy

- Tiêu đề Kho, dung lượng sử dụng dạng 27/50, trạng thái gần đầy/đầy.

- Tab lọc: All, Seeds, Crops, Feed, Event Items.

- Danh sách item theo card/row rõ icon, tên, số lượng, giá bán nếu có, thao tác bán nhanh.

- Khi kho gần đầy màu cảnh báo phải rõ nhưng không quá chói.

## Color mapping cho màn này

| Khu vực / thành phần | Màu và cách áp dụng |
| --- | --- |
| Panel chính | Cream White + Warm Soil Brown accent |
| Storage capacity bình thường | Sky Blue/Green nhẹ |
| Near full | Soft Orange |
| Full | Coral-red nhẹ |
| Tab selected | Farm Green hoặc Sun Yellow tùy loại tab |

## Prompt chính để copy vào AI UI tool

Create a Storage or Warehouse screen for a casual farm game. The screen should be fully 2D UI, clean, bright, and management-friendly, sitting on top of the game style established by the cheerful farm. It must show storage title, used capacity like 27/50, and a very readable warning state for near-full or full. Include filter tabs for All, Seeds, Crops, Feed, and Event Items. The item list should use roomy rounded cards or rows with icon, item name, quantity, and quick sell interaction if sellable. Make inventory management feel smooth, not stressful. Use cream white as the main panel background, warm brown for structure, green for positive actions, and orange/coral only where storage pressure needs emphasis. The overall mood should remain soft and lively, not spreadsheet-like or industrial.

## Negative prompt / điều không được làm sai

No spreadsheet UI, no tiny table rows, no warehouse realism, no gray sterile inventory, no overly complex RPG inventory framing.

# 8. Screen 05 - Sell Screen / Sell Popup

## Mục tiêu màn

Màn bán nông sản/vật phẩm; phải thao tác nhanh, giải phóng kho dễ, nhìn thấy tổng vàng nhận được.

## Những gì bắt buộc phải thấy

- Danh sách item bán được, số lượng hiện có, giá bán/đơn vị, điều chỉnh số lượng, tổng vàng nhận được.

- Nút Bán phải nổi bật nhưng vẫn thân thiện.

- Cần hỗ trợ flow từ popup kho đầy chuyển sang đây ngay.

## Color mapping cho màn này

| Khu vực / thành phần | Màu và cách áp dụng |
| --- | --- |
| Header | Sun Yellow + text nâu ấm |
| Sell CTA | Soft Orange hoặc Sun Yellow-Orange blend |
| Tổng vàng nhận | Sun Yellow + outline nhẹ |
| Counter / stepper | Cream + green accent |

## Prompt chính để copy vào AI UI tool

Design a Sell screen or Sell popup for a mobile farm management game. The layout should let players quickly select item quantity and see the total gold they will receive. This is a functional but cheerful 2D interface, with rounded controls, soft spacing, and clear numbers. The selling action should feel rewarding but not aggressive; use a bright warm orange-yellow call-to-action. Show item icon, item name, owned quantity, sell price per unit, quantity stepper, and a large total gold summary area. The overall UI should be lightweight, pleasant, and easy to use repeatedly when the storage becomes full. Keep the visual style aligned with a bright farm game, not a cash register or finance app.

## Negative prompt / điều không được làm sai

No casino flashy sale screen, no realistic market stall clutter, no dark money theme, no tiny numeric controls.

# 9. Screen 06 - Barn Screen / Khu chăn nuôi

## Mục tiêu màn

Màn quản lý chuồng trại; cho thấy chuồng nào khóa, chuồng nào mở, con nào đang đói/sẵn sàng bán.

## Những gì bắt buộc phải thấy

- World vẫn 2.5D isometric asset 2D; khu chuồng tách rõ với khu ruộng nhưng cùng art direction.

- Pen cards hoặc pen zones cho Gà, Vịt, Heo, Bò.

- Locked by level, available to unlock, unlocked empty, occupied, hungry, sellable đều phải có state rõ.

- Hiển thị nhanh lượng cỏ và sâu đang có để người chơi nuôi thú thuận tiện.

## Color mapping cho màn này

| Khu vực / thành phần | Màu và cách áp dụng |
| --- | --- |
| Nền khu chuồng | Nâu ấm + xanh lá + mái đỏ gạch nhẹ |
| Locked chuồng | Beige muted + overlay nhẹ |
| Hungry state | Soft Orange |
| Sellable state | Farm Green / Sun Yellow phối nhẹ |
| Feed resource quick display | Aqua Mint + Sun Yellow accent |

## Prompt chính để copy vào AI UI tool

Design a Barn or Animal Area screen for a casual mobile farm game. The world should still use a 2.5D isometric fake-3D look built from 2D assets, but this area focuses on barns, pens, and animals instead of crop plots. Show separate pen sections for Chicken, Duck, Pig, and Cow, with clear status variations: locked by level, available to unlock, unlocked but empty, occupied by an animal, hungry, ready to sell. Keep the environment warm and friendly, with wood, grass, and subtle red-roof farm accents. Add a compact quick resource display for grass and worms because those matter for feeding. The interface around the world remains fully 2D and rounded. The screen should feel like a natural expansion of the farm, not a different game mode.

## Negative prompt / điều không được làm sai

No realistic livestock farm, no mud-heavy dirty look, no harsh industrial ranch style, no 3D barns, no grim farming mood.

# 10. Screen 07 - Animal Detail / Feed Panel

## Mục tiêu màn

Panel quản lý 1 con vật; nhấn mạnh trạng thái đói, giai đoạn, giá bán hiện tại và giai đoạn tiếp theo.

## Những gì bắt buộc phải thấy

- Tên vật nuôi, stage, timer tới stage tiếp theo, giá bán hiện tại, giá trị nếu giữ thêm.

- Hiển thị rõ loại thức ăn cần và lượng đang có.

- Nếu thiếu thức ăn phải có đường dẫn rõ sang food shop.

- Nếu trưởng thành và sắp chết phải có cảnh báo ưu tiên Bán ngay.

## Color mapping cho màn này

| Khu vực / thành phần | Màu và cách áp dụng |
| --- | --- |
| Panel | Cream White |
| Feed CTA | Farm Green hoặc Aqua Mint |
| Sell CTA | Soft Orange / Sun Yellow |
| Cảnh báo sắp chết | Coral-red nhẹ |
| Thông tin tiến độ | Sky Blue |

## Prompt chính để copy vào AI UI tool

Create an Animal Detail and Feed Panel for a mobile farm game. This is a contextual 2D popup that appears when a player taps an animal or pen in the isometric barn area. The panel must clearly show animal name, current stage, time remaining to next stage, hunger status, required feed type, current sell value, and next-stage value. The most important actions are Feed and Sell. If feed is missing, the panel must guide the player to the basic food shop in a friendly way. If the animal is mature and close to its lifetime limit, show a warning with a soft but clear urgency. Keep the style cute, readable, and consistent with the rest of the farm game UI. This panel should feel practical and emotionally clear without becoming stressful.

## Negative prompt / điều không được làm sai

No veterinary simulator style, no medical UI, no overly sad death imagery, no cluttered text blocks, no hard red emergency visuals.

# 11. Screen 08 - Basic Food Shop

## Mục tiêu màn

Shop thức ăn chống bí; phải rõ đây là nguồn hỗ trợ chứ không phải nguồn tối ưu.

## Những gì bắt buộc phải thấy

- Chỉ 2 item chính giai đoạn đầu: Grass Bundle và Worm Bundle.

- Phải có note nhẹ rằng chăm cây tốt vẫn là nguồn thức ăn hiệu quả hơn.

- Giá tiền rõ, pack size rõ, nút mua thân thiện.

## Color mapping cho màn này

| Khu vực / thành phần | Màu và cách áp dụng |
| --- | --- |
| Panel nền | Cream White |
| Feed cards | Aqua Mint và xanh lá nhẹ cho Grass; vàng-cam nhẹ cho Worm nếu cần khác biệt |
| Giá | Sun Yellow |
| Thông điệp note | nâu nhạt / xanh ngọc nhạt |

## Prompt chính để copy vào AI UI tool

Design a Basic Food Shop panel for a casual farm game. This shop sells only simple emergency feed items in the early phase, such as Grass Bundle and Worm Bundle. The panel should feel supportive and practical, not like a premium monetization shop. Use a friendly 2D layout with rounded item cards, clear pack quantity, clear gold price, and easy purchase buttons. Include a gentle note that caring for crops is still the best source of animal feed. Make the visual style bright and calm, using cream panel backgrounds, mint and green tones for feed identity, and warm yellow for prices. It should feel like a helpful support shop inside a cozy farm world.

## Negative prompt / điều không được làm sai

No premium gacha shop style, no flashy monetization banners, no dark economy UI, no giant discount labels.

# 12. Screen 09 - Mini Game / Event Popup

## Mục tiêu màn

Popup mời vào mini game/event; phải vui, cứu hộ, tích cực, không lấn core farm.

## Những gì bắt buộc phải thấy

- Tiêu đề event/mini game rõ, mô tả ngắn, phần thưởng có thể nhận.

- Nút Chơi ngay và Bỏ qua rõ ràng.

- Màu event nổi bật hơn UI thường nhưng vẫn cùng thế giới visual.

- Có thể gợi vai trò comeback: nhận hạt giống, vàng nhỏ, cỏ/sâu, item hỗ trợ.

## Color mapping cho màn này

| Khu vực / thành phần | Màu và cách áp dụng |
| --- | --- |
| Header event | Berry Pink + Sun Yellow |
| Reward accents | Sun Yellow, Rare colors nếu có |
| Main CTA | Farm Green hoặc Berry Pink tùy vibe event |
| Panel body | Cream White |

## Prompt chính để copy vào AI UI tool

Create a Mini Game or Event popup for a casual farm game. This is a 2D modal layered over the bright isometric farm interface. It should feel energetic, optimistic, and rewarding, but still belong to the same farm UI system. Show an event title, a one-line description, a compact reward preview, and clear buttons for Play Now and Skip. Use a more festive palette with berry pink and sun yellow accents, but keep the same soft rounded shapes and cream panel base as the rest of the UI. The popup should communicate that this event can help the player recover seeds, resources, or momentum, especially when progression is struggling. Make it fun and welcoming, not overwhelming.

## Negative prompt / điều không được làm sai

No casino event styling, no overpowered gacha look, no chaotic confetti overload, no sci-fi event theme.

# 13. Screen 10 - System Popups

## Mục tiêu màn

Bộ popup hệ thống như Level Up, Storage Full, Unlock Pen, Warning; phải nhất quán và phân tầng rõ.

## Những gì bắt buộc phải thấy

- Level Up: vui, sáng, thấy unlock mới.

- Storage Full: rõ lý do bị chặn và CTA sang Kho/Bán.

- Unlock Pen: thấy yêu cầu level, giá mở, lợi ích.

- Warning: cây sắp chết hoặc thú sắp chết, có CTA xử lý phù hợp.

## Color mapping cho màn này

| Khu vực / thành phần | Màu và cách áp dụng |
| --- | --- |
| Level Up | Sun Yellow + Farm Green + Sky Blue |
| Storage Full | Soft Orange + Cream White |
| Unlock Pen | Farm Green + Warm Soil Brown |
| Warning critical | Coral-red nhẹ + Soft Orange |

## Prompt chính để copy vào AI UI tool

Design a set of system popups for a mobile farm game: Level Up, Storage Full, Unlock Pen, and Warning popups. They should all use the same 2D rounded component system with cream backgrounds, soft shadows, warm farm colors, and clear hierarchy. Level Up should feel joyful and celebratory, Storage Full should feel practical and actionable, Unlock Pen should feel like a progression milestone, and Warning should feel urgent but not punishing. Keep typography friendly and readable, use color intentionally, and make each popup instantly understandable. These popups must feel like a polished family of components inside the same bright farm game universe.

## Negative prompt / điều không được làm sai

No inconsistent popup styles, no generic mobile app modals, no dark overlays that make the game feel gloomy, no alarmist UI.

# 14. Thứ tự dùng prompt để dựng hình

- Dựng trước 5 màn nền tảng: Home Farm, Seed Shop, Crop Action Panel, Storage, Barn.

- Khi visual đã ổn, dựng Animal Detail và bộ popup hệ thống.

- Mini game popup làm sau để giữ cùng hệ component, tránh mỗi event một style khác nhau.

- Nếu AI UI tool ra layout đúng nhưng sai world direction, luôn nhắc lại: world 2.5D isometric fake-3D by 2D assets, UI fully 2D.

# 15. Ghi chú handoff ngắn

Bộ prompt này thiên về dựng mockup và concept screen. Sau khi có màn hình ưng ý, bước kế tiếp nên là tài liệu UI component spec / Unity handoff để chuẩn hóa prefab, state, binding, và rule lắp màn.
