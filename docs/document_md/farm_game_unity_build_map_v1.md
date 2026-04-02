# farm_game_unity_build_map_v1

_Source:_ `farm_game_unity_build_map_v1.docx`

# Unity Build Map

> Tài liệu triển khai production cho phase đầu của game nông trại casual

> Phạm vi: scene architecture, hierarchy tree, prefab catalog, script map, data binding, build order, asset checklist, QA checklist.

| Architecture | Visual Direction | Interaction Rule |
| --- | --- | --- |
| 1 gameplay scene chính<br>HUD + popup canvas ở trên<br>Không tách kho/chuồng/shop thành scene riêng | World 2.5D isometric giả 3D<br>UI 2D hoàn toàn<br>Landscape-first 1920x1080 | Pan camera để đi giữa các khu<br>Tap từng object để mở panel đúng ngữ cảnh<br>Quick tools để phase sau |

Phiên bản tài liệu: v1 • Ngôn ngữ: tiếng Việt • Mục tiêu: chuyển bộ design/UI hiện tại sang cấu trúc triển khai thực tế trong Unity

# 1. Mục tiêu tài liệu

Tài liệu này gom toàn bộ các quyết định đã chốt của project và chuyển chúng thành bản đồ triển khai dành cho Unity production. Mục tiêu là để team có thể bắt đầu dựng scene, prefab, UI, data binding và gameplay module theo đúng thứ tự, đúng phạm vi phase đầu, không bị lệch khỏi design.

| Nguyên tắc khóa cho phase đầu |
| --- |
| Một scene gameplay chính cho toàn bộ nông trại; các chức năng như chọn hạt giống, chăm cây, kho, bán, shop thức ăn và chi tiết vật nuôi đều mở bằng popup hoặc panel. |
| World dùng phong cách 2.5D isometric giả 3D bằng asset 2D; UI là 2D hoàn toàn; orientation landscape-first với resolution gốc 1920x1080. |
| Interaction phase đầu là tap vào từng object trong world để mở panel theo ngữ cảnh; quick tools hoặc thao tác hàng loạt chỉ là phần mở rộng ở giai đoạn sau. |

# 2. Kiến trúc tổng thể trong Unity

Kiến trúc nên được tổ chức theo mô hình một world scene duy nhất, một bộ manager rõ ràng và nhiều prefab có thể tái sử dụng. Cấu trúc này giúp prototype đi nhanh nhưng vẫn đủ sạch để mở rộng sang phase sau mà không phải đập lại.

## 2.1 Scene map

| Scene | Mục đích | Dùng ở phase đầu | Ghi chú |
| --- | --- | --- | --- |
| BootScene | Khởi tạo app, preload data cơ bản, load save local, chuyển sang MainFarmScene. | Có | Scene nhẹ, vào rất nhanh. |
| MainFarmScene | Scene gameplay chính chứa toàn bộ world farm, HUD, popup và managers. | Bắt buộc | Đây là scene người chơi gắn bó nhiều nhất. |
| MiniGameScene | Scene riêng cho mini game phức tạp hoặc event lớn. | Chưa cần | Phase đầu ưu tiên popup mini game ngay trong MainFarmScene. |

## 2.2 Lý do chọn one-scene architecture

• Giữ cảm giác 'đây là nông trại của mình'. Ruộng, chuồng, kho và các công trình cùng tồn tại trong một không gian thay vì bị cắt thành nhiều scene chức năng.

• Giữ flow casual. Người chơi có thể đang chăm cây rồi kéo camera qua cho thú ăn, rồi mở kho bán mà không bị load scene liên tục.

• Dễ tối ưu UX cho phase đầu. Các panel thao tác chỉ nổi lên trên cùng, không làm đứt nhịp chơi chính.

• Dễ mở rộng hơn. Sau này có thể thêm khu mới, event zone hoặc additive scene mà không phá kiến trúc gốc.

# 3. Hierarchy đề xuất cho MainFarmScene

Hierarchy dưới đây là khung tổ chức scene để dev không bị rối từ đầu. Tên object có thể điều chỉnh theo convention của team, nhưng nên giữ tinh thần chia root theo chức năng lớn.

## 3.1 Hierarchy tree (logic level)

| Node | Loại | Mô tả |
| --- | --- | --- |
| MainFarmScene | Root scene | Scene gameplay chính. |
| - EnvironmentRoot | World visuals | Ground, grass, paths, water, shadow planes, ambience. |
| - CropAreaRoot | World gameplay area | Các ô đất, cây trồng, indicator trong khu ruộng. |
| - AnimalAreaRoot | World gameplay area | Chuồng, vật nuôi, ground marker, bubble cảnh báo. |
| - BuildingRoot | World interactive building | Kho, nhà chính, bảng event, công trình khác. |
| - DecorRoot | World decoration | Hàng rào, cây trang trí, giếng nước, props không gameplay. |
| - CameraRoot | Camera system | Main camera, pan controller, bounds, optional focus targets. |
| - InteractionRoot | Input and hit testing | Raycast, selection, object highlight, contextual open panel. |
| - HUDCanvas | UI 2D overlay | Level, gold, EXP, storage, quick navigation, event badge. |
| - PopupCanvas | UI 2D overlay | Seed shop, crop action panel, animal detail, sell popup, warnings. |
| - SystemCanvas | UI 2D top-most | Fade, loading, reward flyout, tutorial pointer, system notifications. |
| - Managers | Systems | Game managers, data providers, save/load, audio, VFX pool, analytics hooks. |

## 3.2 Focus targets trong world

Camera pan tự do là tương tác chính, nhưng nên chuẩn bị sẵn focus target để sau này hỗ trợ nút nhảy nhanh hoặc tutorial.

• CropFocusTarget - điểm camera ưu tiên cho khu ô đất.

• AnimalFocusTarget - điểm camera ưu tiên cho khu chuồng vật nuôi.

• StorageFocusTarget - điểm camera ưu tiên cho nhà kho.

• EventFocusTarget - điểm camera ưu tiên cho khu event hoặc bảng sự kiện sau này.

# 4. Prefab catalog

Prefab nên được chuẩn hóa sớm để tiết kiệm công refactor. Bảng dưới chia thành prefab world, prefab UI và prefab tiện ích.

## 4.1 World prefabs

| Prefab | Nhóm | Vai trò | State/variant chính |
| --- | --- | --- | --- |
| CropTile | World | Ô đất cơ bản, nhận input tap, chứa crop state và visual root. | Empty, Seeded, Growing, NeedsCare, Ripe, Dead |
| CropVisual | World | Sprite bundle cho cây theo phase; có thể thay đổi theo CropData. | Phase1, Phase2, Phase3, Ripe |
| CropCareIndicator | World overlay | Icon/badge cho weed, bug, water-needed hoặc generic needs care. | Warning, danger, hidden |
| AnimalPen | World | Chuồng cho một loại vật nuôi; có slot animal, state locked/unlocked/occupied. | Locked, Empty, Occupied, Warning |
| AnimalUnit | World | Sprite/animation nhẹ của vật nuôi trong chuồng. | Stage1, Stage2, Stage3, Hungry, Sellable |
| StorageBuilding | World | Công trình kho trong farm; click mở panel kho. | Default, Highlight |
| EventBoard | World | Bảng hoặc công trình event; click mở popup event. | Idle, Available, Highlight |
| DecorItem | World | Vật trang trí không gameplay. | Default |

## 4.2 UI prefabs

| Prefab | Vai trò | Màn dùng lại | State/variant chính |
| --- | --- | --- | --- |
| PrimaryButton | CTA chính: gieo, thu hoạch, cho ăn, mở chuồng. | Hầu hết panel | Normal, Pressed, Disabled |
| SecondaryButton | Đóng, hủy, xem thêm. | Hầu hết panel | Normal, Pressed, Disabled |
| WarningButton | Bán, dọn kho, xác nhận thao tác quan trọng. | Sell popup, warning popup | Normal, Pressed |
| ResourceChip | Hiển thị gold, EXP, cỏ, sâu, capacity. | HUD, panel | Default, Warning |
| ItemCard | Card item cho hạt giống, vật phẩm kho, food shop. | Seed shop, storage, food shop | Normal, Selected, Locked |
| PanelBase | Khung popup chuẩn có header/body/footer. | Toàn bộ popup | Small, Medium, Large |
| StorageSlotRow | Dòng item trong kho/bán hàng. | Storage, Sell | Default, Selected, Disabled |
| AnimalPenCard | Card UI nếu có overview chuồng trong panel. | Barn overlay nếu dùng | Locked, Ready, Occupied |
| RewardPopup | Popup nhận thưởng sau mini game hoặc level up. | System | Common, Rare, Event |
| ToastBanner | Thông báo ngắn: kho đầy, cần chăm sóc, thiếu thức ăn. | System | Info, Warning, Danger |

## 4.3 Prefab naming convention

• Dùng tiền tố rõ ràng: PF_World_, PF_UI_, PF_System_, PF_VFX_.

• Ví dụ: PF_World_CropTile, PF_UI_PrimaryButton, PF_System_RewardPopup.

• Variant nên đặt hậu tố Variant hoặc trạng thái nếu cần: PF_UI_ItemCard_RareVariant.

# 5. Script và system map

Script list dưới đây ưu tiên đủ cho phase đầu, không đi quá xa vào tương lai. Nên tách 'system' và 'view/controller' để logic gameplay không dính chặt với UI.

## 5.1 Gameplay systems

| Script/System | Vai trò | Dữ liệu chính đọc/ghi |
| --- | --- | --- |
| CropSystem | Quản lý trồng cây, timer lớn lên, HP, care events, ripe/dead. | CropInstance, CropData |
| AnimalSystem | Quản lý vật nuôi, hunger, stage growth, sellable/dead. | AnimalInstance, AnimalData |
| StorageSystem | Quản lý kho chung, stack, full-capacity rule, add/remove item. | ItemStack, StorageState |
| EconomySystem | Gold, sell flow, seed/food purchase, reward payout. | GoldBalance, Price rules |
| LevelSystem | EXP, level up, unlock crop/pen/land/storage. | LevelData, XP value |
| MiniGameEventSystem | Trigger mini game/event popup, reward comeback flow. | RewardPool, Trigger rules |
| SaveLoadSystem | Save/load local state phase đầu. | Profile save models |

## 5.2 UI & world controllers

| Controller | Vai trò | Lắng nghe từ đâu |
| --- | --- | --- |
| FarmCameraController | Pan camera, clamp bounds, optional focus target jump. | Input system, focus requests |
| WorldInteractionController | Hit test object world, open đúng panel theo ngữ cảnh. | Input + selectable world objects |
| HUDController | Cập nhật gold, EXP, storage, event badge, quick nav. | EconomySystem, LevelSystem, StorageSystem |
| PopupController | Mở/đóng PanelBase và các popup cụ thể, quản lý stack popup. | WorldInteractionController, systems |
| CropActionPanelController | Hiển thị info cây, nút cắt cỏ/bắt sâu/tưới/thu hoạch. | CropSystem, selected CropTile |
| SeedShopPanelController | Hiển thị danh sách hạt giống, lock by level, mua và gieo. | LevelSystem, CropData, EconomySystem |
| StoragePanelController | Hiển thị kho, filter, capacity, chọn item. | StorageSystem |
| SellPanelController | Tính tổng gold nhận, xác nhận bán, update kho. | StorageSystem, EconomySystem |
| AnimalDetailPanelController | Hiển thị trạng thái vật nuôi, feed/sell, thiếu thức ăn. | AnimalSystem, StorageSystem |
| FoodShopPanelController | Mua cỏ/sâu cơ bản để chống bí. | EconomySystem, StorageSystem |

# 6. Data binding map

Phần này là cầu nối từ file data/excel hiện có sang runtime UI và world. Mục tiêu là tránh tình trạng data đổi nhưng UI không biết cập nhật gì.

## 6.1 Data model -> View/Controller

| Data model | View/Controller nhận | Dùng để hiển thị / điều khiển |
| --- | --- | --- |
| CropData | SeedShopPanel, CropTileView, CropActionPanel | Tên cây, giá hạt, grow time, art theo phase, giá bán/unit. |
| CropInstance | CropTileView, CropActionPanel | HP hiện tại, timer, state needs care, ripe, dead. |
| AnimalData | AnimalPenView, AnimalDetailPanel | Tên con, giá mua, stage duration, feed type, sell value. |
| AnimalInstance | AnimalPenView, AnimalDetailPanel | Stage hiện tại, hunger, mature lifetime, sellable state. |
| StorageState / ItemStack | HUD, StoragePanel, SellPanel, FoodShopPanel | Capacity, item quantity, có đủ feed không. |
| EconomyState | HUD, SeedShopPanel, SellPanel, FoodShopPanel | Gold hiện tại, có đủ tiền hay không. |
| LevelData + PlayerXP | HUD, SeedShopPanel, Barn-related UI, unlock popup | Level hiện tại, item nào còn khóa, pen nào mở được. |
| MiniGameRewardPool | EventPopup, RewardPopup | Danh sách quà và rarity. |

## 6.2 Binding rule quan trọng

• HUD phải lắng nghe event thay đổi gold, EXP và storage thay vì polling mỗi frame.

• CropTileView chỉ nên giữ visual state; logic HP/timer nằm ở CropSystem hoặc data instance.

• Popup mở theo selected object. Không để panel tự giữ object reference quá lâu mà không revalidate.

• Storage full là rule gameplay, nên quyết định chặn thao tác phải đến từ StorageSystem trước khi UI chạy animation thu hoạch.

# 7. Build order / Milestone thực thi

Thứ tự build dưới đây ưu tiên ra gameplay sớm, test được sớm, và giảm rủi ro đập lại. Mỗi milestone nên có bản playable tối thiểu trước khi sang bước tiếp.

## 7.1 Milestone roadmap

| Milestone | Mục tiêu | Deliverable tối thiểu |
| --- | --- | --- |
| M1 - World shell | Dựng MainFarmScene, camera pan, world placeholder, HUD khung. | Có thể pan farm và click object placeholder. |
| M2 - Crop core | Ô đất, gieo hạt, grow timer, ripe, harvest cơ bản. | Loop trồng -> chờ -> thu chạy hoàn chỉnh. |
| M3 - Crop care + storage | HP, cỏ/sâu/thiếu nước, kho chung, bán hàng. | Loop trồng + chăm + thu + bán + kho đầy hoạt động. |
| M4 - Animal core | Chuồng, vật nuôi, feed, stage growth, sell flow. | Có thể mua gà, cho ăn, đợi lớn, bán. |
| M5 - Unlock + progression | Level/EXP, mở ô đất, mở chuồng, food shop. | 7 ngày đầu test được progression tương đối. |
| M6 - Event/minigame layer | Popup event, reward comeback, warning polish. | Mini game popup xuất hiện và trả thưởng về kho. |
| M7 - Visual polish | Art, animation nhẹ, warning icon, reward flyout, QA pass. | Build prototype sạch, nhìn được, đủ để review nội bộ. |

## 7.2 Build order chi tiết cho dev

• Bắt đầu từ world interaction và crop loop trước; không nên làm animal UI quá sớm khi crop chưa chạy.

• Storage phải có trước sell flow thật, vì rất nhiều rule gameplay dựa vào kho đầy.

• PopupController nên được dựng sớm ngay từ milestone đầu để các panel sau dùng chung một cơ chế mở/đóng.

• Food shop chỉ nên kích hoạt sau khi animal feed flow đã có, để tránh làm UI thừa khi gameplay chưa cần.

# 8. Asset checklist cho prototype

## 8.1 World art tối thiểu

| Nhóm asset | Danh sách tối thiểu | Ưu tiên |
| --- | --- | --- |
| Ground / Terrain | Tile cỏ, đường, nền đất, vùng nước hoặc kênh nhỏ nếu cần. | Rất cao |
| Crop | 8 loại cây x 3 phase + ripe state + dead state đơn giản. | Rất cao |
| Care overlay | Weed overlay, bug overlay, water-needed icon/overlay, generic warning badge. | Rất cao |
| Animal | Gà, vịt, heo, bò x các stage chính + hungry bubble. | Cao |
| Building | Kho, chuồng gà, chuồng vịt, chuồng heo, chuồng bò, bảng event. | Cao |
| Decor | Một số props nền để farm không quá trống: hàng rào, bụi cây, thùng cỏ. | Trung bình |

## 8.2 UI art tối thiểu

| Nhóm asset | Danh sách tối thiểu | Ưu tiên |
| --- | --- | --- |
| Core buttons | Primary, Secondary, Warning, small icon button. | Rất cao |
| Panels | PanelBase size small/medium/large, header strip, close icon. | Rất cao |
| Icons | Gold, EXP, storage, weed, bug, water, feed, lock, warning, event. | Rất cao |
| Cards & slots | ItemCard, StorageRow, RewardCard, ResourceChip. | Cao |
| Feedback | Toast banner, reward popup, level up popup, full storage popup. | Cao |
| HUD | Top bar background, bottom nav if dùng, badge states. | Cao |

# 9. Screen assembly guide

Phần này tóm tắt màn nào ghép từ prefab nào, để UI artist và dev có thể nhìn screen theo lối 'lắp ghép' chứ không build từ số 0 mỗi lần.

## 9.1 Screen -> prefab assembly

| Screen/Panel | Prefab chính | Ghi chú assembly |
| --- | --- | --- |
| HUD | ResourceChip + small icon button + top bar bg | Luôn sống trong HUDCanvas, không destroy khi mở popup. |
| Seed Shop | PanelBase + ItemCard + PrimaryButton + SecondaryButton | Mở từ ô đất trống hoặc từ shop shortcut. |
| Crop Action Panel | PanelBase + state info block + contextual buttons | Nội dung đổi theo CropInstance state. |
| Storage | PanelBase + filter tabs + StorageSlotRow + capacity block | Có thể chuyển sang Sell popup mà không đóng cứng. |
| Sell Popup | PanelBase + StorageSlotRow selected mode + total summary | WarningButton cho confirm sell. |
| Animal Detail | PanelBase + stage info + ResourceChip + feed/sell buttons | Mở từ AnimalPen/AnimalUnit trong world. |
| Food Shop | PanelBase + ItemCard + buy CTA | Mở từ Animal Detail khi thiếu feed hoặc từ shop shortcut. |
| System Popups | PanelBase hoặc RewardPopup + ToastBanner | Nằm trong SystemCanvas, layer cao nhất. |

# 10. Interaction contract giữa world và UI

Đây là phần rất quan trọng vì world là 2.5D còn UI là 2D. Nếu không chốt sớm, input và panel flow sẽ dễ bị chồng chéo.

• World object là điểm bắt đầu. Tap vào CropTile, cây, chuồng, vật nuôi hoặc kho sẽ gửi một selection event sang PopupController.

• UI panel là lớp thao tác. Sau khi panel mở, mọi thao tác logic đi qua controller của panel, không xử lý trực tiếp ở object world.

• Camera pan ưu tiên khi không tap vào object tương tác. Cần phân biệt drag world với tap object để UX không khó chịu.

• Khi popup lớn đang mở, world interaction nên bị khóa tạm thời. HUD vẫn có thể sống tùy popup, nhưng world raycast cần disable hoặc mask đúng cách.

# 11. Technical notes cho Unity

## 11.1 Technical decisions gợi ý

| Chủ đề | Khuyến nghị | Vì sao |
| --- | --- | --- |
| Canvas setup | Canvas Scaler theo Scale With Screen Size, reference 1920x1080, landscape-first. | Giữ UI nhất quán với resolution gốc đã chốt. |
| Sorting | Dùng Sorting Layer rõ cho Ground, WorldObject, OverlayWorldIcon, UI, SystemUI. | Tránh cây/overlay/UI đè sai lớp. |
| Input | WorldInteractionController dùng raycast 2D hoặc collider phù hợp với asset 2D. | Object world dễ chọn và debug. |
| Popup stack | Một PopupController trung tâm quản stack mở/đóng và modal behavior. | Giảm bug chồng popup. |
| Data loading | ScriptableObject cho data tĩnh; runtime instance riêng cho crop/animal/storage. | Dễ iterate data, không mutate asset gốc. |
| Pooling | Pool cho warning icon, reward flyout, toast nếu tần suất cao. | Giảm instantiate/destroy khi prototype lớn dần. |

# 12. QA checklist cho prototype

## 12.1 Functional QA

| Hạng mục | Câu hỏi kiểm | Pass criteria |
| --- | --- | --- |
| World interaction | Tap đúng object có mở đúng panel không? | Không mở sai panel, không miss hit bất thường. |
| Crop flow | Gieo -> grow -> needs care -> harvest có chạy đủ không? | Không kẹt state, không harvest sai item. |
| Storage rule | Kho đầy có chặn thu đúng không? | Có popup cảnh báo và không cho add item trái rule. |
| Animal flow | Hunger/feed/stage/sell có cập nhật đúng không? | Đói thì dừng growth, feed xong tiếp tục, sell đúng giá. |
| Unlock flow | Lên level có mở đúng chuồng/ô đất/hạt giống không? | UI lock state đồng bộ với data unlock. |
| Popup behavior | Popup có chồng sai, khóa sai world input hay không? | Modal hoạt động đúng, đóng mở sạch. |

## 12.2 Visual QA

| Hạng mục | Điểm cần nhìn | Pass criteria |
| --- | --- | --- |
| Landscape layout | HUD và popup ở 1920x1080 có cân, không che world quá nhiều? | Đọc tốt, vẫn thấy farm rõ. |
| World readability | Cây, chuồng, kho có phân biệt được ở camera mặc định không? | Object chính dễ nhận ra. |
| Color consistency | UI có bám palette đã khóa không? | Không lẫn màu warning/danger/soft orange sai vai trò. |
| World/UI cohesion | 2.5D world và 2D UI có hòa hợp không? | Không thấy cảm giác hai hệ tách rời quá mạnh. |

# 13. Definition of done cho phase đầu

| Phase đầu được xem là 'xong để review nội bộ' khi đáp ứng đủ các điều kiện sau |
| --- |
| Người chơi có thể vào MainFarmScene, pan camera giữa khu ruộng - chuồng - kho, và tương tác tap vào object để mở panel đúng ngữ cảnh. |
| Crop loop hoàn chỉnh: gieo, lớn, phát sinh cỏ/sâu/thiếu nước, xử lý chăm sóc, chín, thu hoạch, vào kho, bán lấy vàng. |
| Animal loop hoàn chỉnh: mở chuồng theo level, mua con, cho ăn đúng loại, lớn qua stage, bán được, quá lâu thì chết. |
| Storage chung và popup flow hoạt động đúng: kho đầy chặn thu, mở kho và bán giải phóng chỗ trống được. |
| UI bám đúng landscape 1920x1080, world 2.5D isometric và UI 2D, không có scene chức năng rời rạc trái với kiến trúc đã chốt. |

# 14. Khuyến nghị tiếp theo sau tài liệu này

• Dựng MainFarmScene skeleton ngay với root hierarchy và camera bounds placeholder trước khi làm art đẹp.

• Dùng data hiện có trong Excel/Word để tạo ScriptableObject trước, rồi mới nối panel.

• Nếu team nhỏ, ưu tiên làm M1 -> M3 thật sạch rồi mới chạm animal module để tránh quá nhiều nửa-vời.

• Giữ tất cả tài liệu UI và build map này làm nguồn sự thật chung khi có thay đổi; mọi thay đổi lớn nên quay lại cập nhật file gốc.
