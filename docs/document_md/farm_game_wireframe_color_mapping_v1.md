# farm_game_wireframe_color_mapping_v1

_Source:_ `farm_game_wireframe_color_mapping_v1.docx`

# Farm Game - UI Wireframe Text & Color Mapping

Bản v1 • Phase đầu • UI 2D + World 2.5D Isometric giả 3D

| Mục đích tài liệu<br>Khóa wireframe text cực chi tiết cho từng màn hình phase đầu, đồng thời gắn mapping màu cụ thể cho từng khu vực UI để dùng cho UI artist, AI UI tool và Unity handoff. |
| --- |
| Phạm vi<br>Bao gồm Home Farm, Seed Shop, Crop Action, Storage, Sell, Barn, Animal Detail, Basic Food Shop, Mini Game/Event và các popup hệ thống cốt lõi. |
| Rule khóa<br>UI là 2D hoàn toàn. World dùng phong cách 2.5D isometric giả 3D bằng asset 2D. Phase đầu thao tác trực tiếp trên từng object và panel theo ngữ cảnh. |

Tài liệu này ưu tiên mức độ rõ ràng, tính khả thi khi làm prototype và sự nhất quán để về sau có thể mở rộng sang quick tools, batch actions và live-ops.

# 1. Quy ước chung

• World của game dùng góc nhìn 2.5D isometric giả 3D bằng asset 2D. Nền farm, ô đất, cây trồng, vật nuôi, chuồng trại và decor giữ cùng góc nhìn; UI nổi trên cùng như một lớp 2D sạch, dễ đọc.

• Phase đầu không sử dụng tool mode toàn cục cho tưới nước, bắt sâu hay thu hoạch. Người chơi chạm trực tiếp vào từng object để mở panel theo ngữ cảnh.

• Mọi mapping màu bên dưới là mức ưu tiên hiển thị chính thức. Có thể tinh chỉnh shade nhẹ, nhưng không đổi vai trò màu.

## 2. Palette chính thức

| Token | HEX | Swatch | Ứng dụng |
| --- | --- | --- | --- |
| Primary / Farm Green | #69C34D |  | CTA chính, selected state, hành động tích cực |
| Deep Leaf Green | #4FA63A |  | Hover/pressed, viền selected, heading nhấn |
| Sky Blue | #8ED8FF |  | EXP, info state, nền sạch |
| Sun Yellow | #FFD75E |  | Vàng, reward, harvest value |
| Warm Soil Brown | #B97A4A |  | Đất, panel nông trại, nhấn world |
| Cream White | #FFF7E8 |  | Panel, popup, card nền chính |
| Soft Orange | #FFA94D |  | Warning nhẹ, CTA bán |
| Berry Pink | #FF6FAE |  | Event, reward hiếm, popup vui |
| Aqua Mint | #67DCC8 |  | Nước, hồi phục, hỗ trợ |

## 3. Màu trạng thái và text

| Token | HEX | Swatch | Ứng dụng |
| --- | --- | --- | --- |
| Healthy | #5BCB4B |  | HP tốt, hành động thành công |
| Warning | #FFB547 |  | Cần chú ý, cây cần chăm sóc, kho gần đầy |
| Danger | #FF6B5E |  | Sắp chết, kho đầy, blocked state |
| Info | #5BB8FF |  | EXP, trạng thái thông tin |
| Disabled | #CFC7B8 |  | Button khóa, CTA không dùng được |
| Text Primary | #5B4636 |  | Text chính |
| Text Secondary | #7A6858 |  | Text phụ, mô tả |

## 4. Mapping màu theo chức năng

| Khu chức năng | Màu chính | Màu nhấn phụ | Ghi chú sử dụng |
| --- | --- | --- | --- |
| Farm world | #69C34D + #B97A4A | #8ED8FF | Cỏ xanh, đất nâu ấm, trời xanh nhẹ. Không dùng neon trong world. |
| Kho | #FFF7E8 + #B97A4A | #FFD75E | Panel kem sáng, nâu gỗ và vàng nhạt gợi chất kho chứa. |
| Shop | #FFF7E8 + #FFD75E | #69C34D | Tươi, thân thiện, cảm giác giao dịch nhẹ nhàng. |
| Chuồng | #FFF7E8 + #B97A4A | #69C34D + #FFB547 | Mái chuồng, gỗ, feed resource và badge trạng thái. |
| Event/Mini game | #FF6FAE + #FFD75E | #67DCC8 | Nổi hơn màn thường nhưng vẫn cùng gia đình màu. |

# 5. Screen 01 - Home Farm Screen

## Mục tiêu màn hình

Màn hình trung tâm của game. Đây là nơi người chơi nhìn thấy thế giới 2.5D isometric, ruộng đất, đồ trang trí cơ bản, trạng thái cây trồng và đường vào tất cả hệ thống lõi như Kho, Shop, Chuồng và Event.

## Ngữ cảnh visual

World chiếm phần lớn chiều cao màn hình. Nền farm là 2.5D isometric giả 3D bằng sprite 2D; HUD, tab bar, popup và panel nổi hoàn toàn là 2D. Camera cố định, không xoay. Có thể kéo nhẹ để xem phần farm mở rộng về sau nhưng phase đầu giữ trong khung dễ kiểm soát.

## Bố cục wireframe chi tiết

• Vùng A - Top HUD cao khoảng 12-14% chiều cao màn hình: avatar nhỏ bên trái, level badge, thanh EXP, số vàng, số slot kho và nút settings. Các phần tử nằm trên panel kem sáng hoặc dải mờ để không che world quá nặng.

• Vùng B - Main World chiếm khoảng 62-68% chiều cao: ruộng đất isometric đặt trung tâm; ô đất hiển thị dạng diamond/iso tile. Cây mọc theo phase rõ ràng; trạng thái cỏ, sâu, thiếu nước và HP biểu diễn bằng overlay/icon nhỏ.

• Vùng C - Floating Alerts cạnh phải hoặc trên object: chỉ hiện khi object cần tương tác. Ví dụ icon chấm than vàng cho Needs Care, glow mềm cho cây chín, bubble thức ăn cho vật nuôi đói khi có chuồng trong tầm nhìn.

• Vùng D - Bottom Navigation cao khoảng 10-12%: 5 tab gồm Farm, Kho, Shop, Chuồng, Event. Nút dạng rounded capsule hoặc card bo tròn với icon lớn và label ngắn.

• Vùng E - Event / Quick CTA cạnh phải: nút nổi nhỏ để báo event đang có hoặc mini game có thể chơi. Chỉ xuất hiện 1 điểm nhấn chính để tránh rối.

## Tương tác và hành vi

• Chạm ô đất trống để mở panel trồng cây gắn theo đúng ô đó, không bật mode trồng toàn màn.

• Chạm cây đang lớn để xem info panel. Nếu cây có cỏ, sâu hoặc thiếu nước, panel chỉ hiện các nút cần thiết cho trạng thái đó.

• Chạm cây chín để thu hoạch nhanh hoặc mở panel xác nhận rất nhẹ tùy tuning; phase đầu ưu tiên ít bước.

• Khi kho đầy, thao tác thu hoạch bị chặn và hiện popup dẫn sang Kho/Bán hàng.

• Tất cả CTA chính luôn phải đọc được trên nền world, vì vậy icon và badge phải có bóng nhẹ hoặc viền kem để tách nền.

## Trạng thái cần hỗ trợ

• Ô đất trống

• Cây đang lớn bình thường

• Cây Needs Care

• Cây chín - perfect window

• Cây trễ thu - late harvest

• Kho gần đầy / kho đầy

• Event available

## Text / label gợi ý

| Vùng / trạng thái | Text hiển thị gợi ý |
| --- | --- |
| Top HUD - level | Lv. 4 |
| Top HUD - kho | 27 / 50 |
| Tooltip cây cần chăm | Cây cần chăm sóc |
| Tooltip cây chín | Sẵn sàng thu hoạch |
| Event bubble | Event mới |
| Bottom tab Chuồng | Chuồng |

## Color mapping theo khu vực UI

| Khu vực | Màu nền / fill | Màu nhấn / state | Ghi chú triển khai |
| --- | --- | --- | --- |
| Top HUD panel | #FFF7E8 | #69C34D + #FFD75E + #8ED8FF | Panel kem sáng; vàng cho currency, xanh dương cho EXP, xanh lá cho level/action. |
| Main world grass & sky | #69C34D + #8ED8FF | #4FA63A | World sáng nhưng không lấn UI. |
| Soil / empty tile | #B97A4A | #8C5A33 hoặc shadow 10-15% | Đất ấm; tile trống cần nổi khỏi nền cỏ. |
| Needs Care alert | #FFF7E8 | #FFB547 / #FF6B5E | Icon vàng cho needs care; đỏ chỉ dùng khi cây sắp chết. |
| Bottom nav | #FFF7E8 | #69C34D selected / #CFC7B8 locked | Tab active dùng xanh lá; inactive kem. |
| Event floating CTA | #FF6FAE | #FFD75E + #67DCC8 | Nổi hơn UI thường nhưng giữ cùng hệ visual. |

## Lưu ý handoff

• HP bar của cây nên nằm ngay dưới hoặc trên cây, dài ngắn vừa phải, không che phase art.

• Nếu có nhiều object cần chăm trong cùng tầm nhìn, chỉ giữ icon ở mức nhỏ và đồng nhất.

• Khoảng đệm giữa HUD và world phải đủ để ngón tay không chạm nhầm.

# 6. Screen 02 - Seed Shop Panel

## Mục tiêu màn hình

Panel chọn hạt giống dùng khi người chơi chạm ô đất trống hoặc mở tab Shop. Panel cần giải thích được giá, thời gian lớn, sản lượng cơ bản và unlock level của từng loại cây mà không gây cảm giác nặng bảng tính.

## Ngữ cảnh visual

Panel trượt lên từ dưới hoặc nổi giữa màn trên nền dim nhẹ. World farm phía sau vẫn còn thấy mờ để giữ cảm giác đang thao tác trên một ô đất thật sự.

## Bố cục wireframe chi tiết

• Header panel: tên "Chọn hạt giống", nút đóng, có thể kèm filter nhỏ theo nhóm Ngắn hạn / Trung hạn / Dài hạn / Hiếm.

• Danh sách card hạt giống dạng vertical list hoặc 2 cột vừa phải. Mỗi card gồm icon cây, tên, level mở, giá hạt, grow time, base yield, tag độ dài vòng đời.

• Nếu panel được mở từ một ô đất cụ thể, phía trên cùng có dòng context như "Ô đất A03 - chọn hạt giống để gieo".

• CTA chính trên card là "Gieo" hoặc "Mua & Gieo". Với hạt đủ điều kiện nhưng thiếu vàng, CTA disable rõ ràng.

• Card hiếm giữ viền rarity riêng nhưng vẫn không phá palette chung.

## Tương tác và hành vi

• Nếu mở từ ô trống, chọn hạt giống xong phải gieo trực tiếp vào đúng ô và đóng panel.

• Nếu mở từ tab Shop tổng, có thể chỉ mua hạt về kho mà chưa gieo ngay; phase đầu có thể bỏ cách mua lẻ này nếu muốn giảm complexity.

• Pressed state của card hoặc CTA cần rõ, vì panel này sẽ dùng rất nhiều ở đầu game.

• Card khóa bởi level hiển thị mờ và có text "Mở ở Lv.X", không dùng đỏ.

## Trạng thái cần hỗ trợ

• Card mở khóa bình thường

• Card thiếu vàng

• Card bị khóa bởi level

• Card hiếm / high-tier

• Filter selected

## Text / label gợi ý

| Vùng / trạng thái | Text hiển thị gợi ý |
| --- | --- |
| Header | Chọn hạt giống |
| Context line | Ô đất 03 • Chọn hạt giống để gieo |
| Card tag | Ngắn hạn |
| Locked card | Mở ở Lv. 7 |
| CTA disabled | Thiếu vàng |

## Color mapping theo khu vực UI

| Khu vực | Màu nền / fill | Màu nhấn / state | Ghi chú triển khai |
| --- | --- | --- | --- |
| Panel nền | #FFF7E8 | #B97A4A border nhẹ | Bo tròn lớn, shadow mềm, đọc tốt trên nền world dim. |
| Header band | #EAF8E4 | #69C34D | Header nhấn xanh lá, giữ cảm giác tươi sáng. |
| Seed card default | #FFFFFF hoặc #FFF7E8 | #D9D2C7 border / #69C34D selected | Card thường sáng, selected viền xanh lá. |
| Time / yield info | #FFF7E8 | #8ED8FF + #FFD75E | Time có thể nhấn xanh dương; yield hoặc sell value nhấn vàng nhẹ. |
| Locked card | #F3EEE3 | #CFC7B8 + overlay 25-35% | Không dùng màu xám lạnh. |
| Rare card | #FFF7E8 | #5BB8FF hoặc #B07CFF badge | Rarity chỉ nằm ở viền/badge. |

## Lưu ý handoff

• Giá, thời gian và sản lượng nên bố trí thành các badge nhỏ hoặc dòng phụ.

• Mỗi card phải bấm được dễ dàng bằng ngón tay; ưu tiên card cao hơn một chút thay vì chật chữ.

# 7. Screen 03 - Crop Action Panel

## Mục tiêu màn hình

Panel xuất hiện khi người chơi chạm vào một ô cây đang lớn hoặc cần chăm sóc. Đây là panel theo ngữ cảnh quan trọng nhất trong phase đầu vì nó quyết định cảm giác game có dễ hiểu hay không.

## Ngữ cảnh visual

Panel có thể là card nổi cạnh object hoặc bottom sheet nhỏ. World vẫn hiển thị phía sau để người chơi hiểu panel này gắn với cây nào.

## Bố cục wireframe chi tiết

• Phần đầu panel: tên cây, phase hiện tại, thumbnail cây hoặc icon cùng phase, timer còn lại.

• Khối trạng thái: HP bar, tình trạng hiện tại (Bình thường / Có cỏ / Có sâu / Thiếu nước / Chín / Trễ thu / Sắp chết).

• Khối action buttons: chỉ hiện nút đúng với state. Ví dụ nếu có cỏ và sâu thì panel hiện 2 nút "Cắt cỏ" và "Bắt sâu"; không hiển thị nút thừa.

• Khối chất lượng dự kiến: mô tả ngắn kiểu "Sản lượng dự kiến: Tốt" hoặc "Sản lượng đang giảm".

• Nếu cây chín, CTA chính là "Thu hoạch". Nếu trong perfect window, có badge riêng để kích thích hành động.

## Tương tác và hành vi

• Bấm vào action xong panel có thể giữ mở nếu cây còn vấn đề khác, hoặc tự thu gọn lại sau hành động.

• Nếu cây có 3 vấn đề cùng lúc, thứ tự ưu tiên hiển thị nên theo mức ảnh hưởng hoặc theo icon/hành động dễ đọc từ trái sang phải.

• Khi cây sắp chết, panel phải chuyển ưu tiên thị giác sang cảnh báo thay vì giữ cân bằng nhẹ như bình thường.

• Không bắt người chơi vào sub-menu phụ để xử lý sâu/cỏ/nước; tất cả nằm ngay ở panel đầu tiên.

## Trạng thái cần hỗ trợ

• Bình thường

• Needs Care - một vấn đề

• Needs Care - nhiều vấn đề

• Perfect harvest

• Late harvest

• Near death

## Text / label gợi ý

| Vùng / trạng thái | Text hiển thị gợi ý |
| --- | --- |
| HP label | Sức khỏe: 76 / 100 |
| Needs Care | Cây cần chăm sóc |
| Quality | Sản lượng dự kiến: Tốt |
| Perfect window | Thu hoạch đẹp • +10% |
| Danger | Cây sắp chết |

## Color mapping theo khu vực UI

| Khu vực | Màu nền / fill | Màu nhấn / state | Ghi chú triển khai |
| --- | --- | --- | --- |
| Panel nền | #FFF7E8 | #B97A4A border nhẹ | Card nền sáng, tách rõ khỏi world. |
| HP bar fill | #5BCB4B / #FFD24D / #FF7B5A | #5B4636 text | Đổi màu theo mốc HP. |
| Action button - care | #69C34D | #4FA63A pressed | Cho cắt cỏ, bắt sâu, tưới nước. |
| Action button - harvest | #FFD75E | #FFA94D hoặc glow nhẹ | Harvest là action đặc biệt nên dùng vàng ấm. |
| State badge - perfect | #FFF1CC | #FFD75E + #C98D1A | Perfect window cần tạo cảm giác khẩn trương tích cực. |
| State badge - danger | #FFE3DD | #FF6B5E | Sắp chết dùng đỏ cam mềm. |

## Lưu ý handoff

• Không làm panel quá cao; nếu state ít thì panel ngắn gọn, nếu state nhiều thì giãn cao thêm.

• Chữ trong panel phải rất thực dụng, ưu tiên hành động hơn mô tả dài.

# 8. Screen 04 - Storage / Warehouse

## Mục tiêu màn hình

Màn quản lý toàn bộ item trong kho chung: hạt giống, nông sản, cỏ, sâu, vật phẩm event. Đây là màn phục vụ quản lý tài nguyên và giải quyết tắc nghẽn khi kho đầy.

## Ngữ cảnh visual

UI thuần 2D, không cần đưa world lên làm trọng tâm. Có thể giữ background là blur/dim nhẹ của farm hoặc nền panel riêng sáng hơn để việc quản lý item dễ nhìn.

## Bố cục wireframe chi tiết

• Header trên cùng: tiêu đề "Nhà kho", capacity label kiểu "27 / 50", nút đóng hoặc back.

• Thanh filter ngay dưới header: Tất cả, Hạt giống, Nông sản, Nguyên liệu nuôi, Event. Dùng tab pill bo tròn lớn.

• Danh sách item card dạng lưới 2-3 cột hoặc list; mỗi card có icon, tên, số lượng, stack, giá bán nếu có.

• Nếu capacity gần đầy, đặt một dải cảnh báo mềm ngay dưới header hoặc ngay sát capacity label.

• Nút đi nhanh sang "Bán hàng" nằm cuối header hoặc sticky ở bottom để giải phóng kho nhanh.

## Tương tác và hành vi

• Bộ lọc phải chuyển nhanh, không có animation nặng. Người chơi vào màn này thường để xử lý nhanh.

• Bấm card item có thể mở menu nhỏ: xem chi tiết, bán, dùng. Phase đầu có thể bỏ menu chi tiết với item đơn giản.

• Khi kho đầy do vừa bị chặn thu hoạch, màn này cần ưu tiên show item có thể bán ngay hoặc filter gợi ý.

• Hạt giống nên hiển thị rõ vì mini game/event có thể dùng nó như cơ chế comeback.

## Trạng thái cần hỗ trợ

• Kho bình thường

• Kho gần đầy

• Kho đầy

• Filter selected

• Item stack max

## Text / label gợi ý

| Vùng / trạng thái | Text hiển thị gợi ý |
| --- | --- |
| Header | Nhà kho |
| Capacity | 27 / 50 |
| Near full | Kho gần đầy • Hãy dọn bớt vật phẩm |
| Full | Kho đã đầy |
| Sticky CTA | Đi đến bán |

## Color mapping theo khu vực UI

| Khu vực | Màu nền / fill | Màu nhấn / state | Ghi chú triển khai |
| --- | --- | --- | --- |
| Background / panel | #FFF7E8 | #B97A4A | Kho dùng nền kem và nâu gỗ để gợi chất lưu trữ. |
| Capacity default | #FFF7E8 | #5B4636 + #8ED8FF | Số slot bình thường đọc nhẹ, sạch. |
| Capacity warning | #FFF1CC | #FFB547 | Khi gần đầy chuyển vàng cam. |
| Capacity full | #FFE3DD | #FF6B5E | Khi đầy dùng đỏ cam mềm và CTA rõ. |
| Filter tabs | #F5EEDC | #69C34D selected / #CFC7B8 off | Selected phải đọc rõ. |
| Item card | #FFFFFF | #D9D2C7 / rarity badge | Card sáng, icon rõ, text nâu đậm. |

## Lưu ý handoff

• Không dùng nền tối cho kho; kho cần cảm giác ngăn nắp và sáng.

• Nếu dùng grid card, mỗi card phải giữ nhịp spacing đều để tránh giống spreadsheet.

# 9. Screen 05 - Sell Popup / Sell Screen

## Mục tiêu màn hình

Màn dùng để đổi nông sản hoặc vật phẩm có thể bán thành vàng. Đây là màn giảm tải kho và quay vòng kinh tế nên phải rõ ràng, ít nhầm lẫn và tính toán tổng vàng dễ nhìn.

## Ngữ cảnh visual

Thuần UI 2D. Có thể là full screen nhẹ hoặc popup lớn. Vì đây là màn transactional, cần rõ số liệu hơn các màn khác nhưng vẫn giữ chất casual, thân thiện.

## Bố cục wireframe chi tiết

• Header: "Bán vật phẩm" hoặc "Bán hàng", có tổng số item đang chọn và nút đóng.

• Danh sách item bán được: icon, tên, số lượng sở hữu, giá/đơn vị, stepper tăng giảm số lượng, thành tiền.

• Summary bar ở cuối: Tổng vàng nhận được, nút Bán, nút Hủy. Summary bar nên sticky ở đáy màn.

• Nếu vào từ kho đầy, có dòng context ngắn như "Bạn cần giải phóng chỗ trong kho để tiếp tục thu hoạch".

## Tương tác và hành vi

• Thay đổi số lượng phải phản hồi tức thì vào tổng vàng.

• Không cho phép bán item bị khóa hoặc item chưa muốn cho bán ở phase đầu nếu rule game cấm.

• Nếu người chơi bán xong mà giải phóng đủ chỗ, có thể hiện toast ngắn "Đã giải phóng chỗ trong kho".

## Trạng thái cần hỗ trợ

• Không chọn item

• Đang chọn vài item

• Từ kho đầy

• Bán thành công

## Text / label gợi ý

| Vùng / trạng thái | Text hiển thị gợi ý |
| --- | --- |
| Header | Bán hàng |
| Context | Giải phóng chỗ trong kho để tiếp tục thu hoạch |
| Summary | Tổng vàng nhận được |
| CTA | Bán ngay |
| Toast | Đã bán thành công |

## Color mapping theo khu vực UI

| Khu vực | Màu nền / fill | Màu nhấn / state | Ghi chú triển khai |
| --- | --- | --- | --- |
| Popup nền | #FFF7E8 | #B97A4A border | Tạo cảm giác an toàn, thân thiện. |
| Summary bar | #FFF1CC | #FFD75E + #C98D1A | Tổng vàng và CTA bán nổi bật ấm áp. |
| Sell CTA | #FFA94D | #FF8E31 pressed | Khác màu CTA chăm sóc để hiểu đây là hành động kinh tế. |
| Cancel CTA | #F5EEDC | #7A6858 | Nền kem hoặc beige, nhẹ hơn CTA chính. |
| Danger notice nếu bán item quý | #FFE3DD | #FF6B5E | Chỉ dùng khi thật sự cần xác nhận lại. |

## Lưu ý handoff

• Vì bán hàng là hành động thường xuyên, icon coin và tổng vàng phải rất dễ đọc ở đáy màn.

• Không thiết kế màn bán như cửa hàng dark theme hoặc machine-like.

# 10. Screen 06 - Animal Area / Barn Screen

## Mục tiêu màn hình

Màn hiển thị khu chuồng trại và tình trạng các chuồng: khóa, có thể mở, đang trống, đang nuôi, đói, sẵn sàng bán hoặc sắp quá tuổi thọ. Đây là lớp gameplay thứ hai sau trồng trọt.

## Ngữ cảnh visual

World vẫn theo 2.5D isometric, nhưng phần chuồng chiếm ít đối tượng hơn ruộng nên UI overlay có thể rõ hơn. Màu ấm và nâu gỗ nên xuất hiện nhiều hơn Farm Screen để tăng cảm giác chăn nuôi.

## Bố cục wireframe chi tiết

• Header tương tự Farm: level, exp, vàng, có thể thêm quick resource cho Grass và Worm.

• Khu chính hiển thị các chuồng theo hàng hoặc cluster rõ ràng: Chuồng Gà, Chuồng Vịt, Chuồng Heo, Chuồng Bò.

• Mỗi chuồng có visual state: Locked by level, Unlockable, Empty, Occupied, Hungry, Sellable, Near death.

• CTA chính nằm ngay trên card chuồng hoặc bubble nổi: Mở chuồng, Mua con giống, Xem chi tiết, Cho ăn, Bán.

• Nếu chuồng bị khóa, card phải nói rõ điều kiện: cần level nào và chi phí mở chuồng bao nhiêu.

## Tương tác và hành vi

• Chạm chuồng có thú để vào Animal Detail. Nếu chuồng đang trống thì mở luồng mua con giống.

• Nếu có nhiều chuồng cùng có vấn đề, chỉ nên dùng icon/marker nhỏ và để người chơi tự chạm vào từng chuồng.

• Quick resource Grass/Worm phải đặt đủ gần header để người chơi hiểu nguồn thức ăn hiện có.

## Trạng thái cần hỗ trợ

• Locked

• Unlockable

• Empty

• Hungry

• Growing

• Mature / sellable

• Near death

## Text / label gợi ý

| Vùng / trạng thái | Text hiển thị gợi ý |
| --- | --- |
| Locked state | Mở ở Lv. 7 |
| Unlockable | Mở chuồng |
| Empty | Chuồng trống |
| Hungry bubble | Cần ăn |
| Sellable | Có thể bán |

## Color mapping theo khu vực UI

| Khu vực | Màu nền / fill | Màu nhấn / state | Ghi chú triển khai |
| --- | --- | --- | --- |
| Header / HUD | #FFF7E8 | #69C34D + #FFD75E | Giữ nhất quán với Farm nhưng thêm resource feed rõ hơn. |
| Barn world base | #EAF8E4 + #B97A4A | #8C5A33 shadow nhẹ | Nâu ấm và xanh lá hỗ trợ nhau để gợi chất chuồng trại. |
| Locked pen card | #F3EEE3 | #CFC7B8 + lock overlay | Không dùng đỏ vì đây là khóa tiến trình bình thường. |
| Unlock CTA | #69C34D | #4FA63A | Mở chuồng vẫn là action tích cực. |
| Hungry state | #FFF1CC | #FFB547 | Đói cần gây chú ý nhưng không tạo căng thẳng quá mức. |
| Sellable state | #FFF1CC | #FFD75E + #C98D1A | Tới thời điểm bán đẹp có glow vàng nhẹ. |
| Near death state | #FFE3DD | #FF6B5E | Cảnh báo mạnh hơn, ưu tiên bán. |

## Lưu ý handoff

• Chuồng phải đủ to để khi zoom mobile vẫn nhận ra loại thú và trạng thái hiện tại.

• Không nên làm khu chuồng quá dày decor ở phase đầu; ưu tiên khả năng đọc trạng thái.

# 11. Screen 07 - Animal Detail / Feed Panel

## Mục tiêu màn hình

Panel quản lý 1 con vật cụ thể. Người chơi xem giai đoạn trưởng thành, thời gian đến stage tiếp theo, trạng thái đói, giá bán hiện tại và có thể cho ăn hoặc bán.

## Ngữ cảnh visual

Panel là UI 2D nổi trên world chuồng trại. Giữ cảm giác thân thiện, không biến thành bảng dữ liệu khô cứng.

## Bố cục wireframe chi tiết

• Khối đầu: tên vật nuôi, ảnh minh họa theo giai đoạn hiện tại, stage label (Baby / Growing / Mature).

• Khối giữa: timer đến stage tiếp theo, hunger status, feed requirement, giá bán hiện tại và giá bán stage sau.

• Khối CTA: Cho ăn, Bán ngay, Mua thức ăn (nếu thiếu). Nút nào quan trọng hơn ở trạng thái hiện tại thì đặt lớn hơn.

• Khối cảnh báo phụ: thời gian còn lại sau trưởng thành trước khi chết nếu không bán.

## Tương tác và hành vi

• Nếu vật nuôi đang đói và người chơi có đủ feed, nút "Cho ăn" phải là CTA lớn nhất.

• Nếu đang thiếu feed, CTA "Mua thức ăn" phải dẫn sang đúng Food Shop với tab phù hợp.

• Nếu vật nuôi đã trưởng thành và có tuổi thọ đếm ngược, panel phải cho thấy chờ thêm được gì và rủi ro mất gì.

## Trạng thái cần hỗ trợ

• Growing bình thường

• Hungry

• Ready to sell

• Near death

• Feed thiếu

## Text / label gợi ý

| Vùng / trạng thái | Text hiển thị gợi ý |
| --- | --- |
| Name | Gà |
| Stage | Giai đoạn 2 |
| Hungry | Đang đói |
| Feed | Cần 1 Sâu |
| Sell now | Giá bán hiện tại: 280 vàng |
| Next stage | Nếu chờ tiếp: 420 vàng |

## Color mapping theo khu vực UI

| Khu vực | Màu nền / fill | Màu nhấn / state | Ghi chú triển khai |
| --- | --- | --- | --- |
| Panel nền | #FFF7E8 | #B97A4A border | Nền sáng, bo tròn lớn, shadow mềm. |
| Feed CTA | #69C34D | #4FA63A | Action chăm sóc tích cực. |
| Sell CTA | #FFA94D | #FF8E31 | Bán là action kinh tế. |
| Need feed notice | #FFF1CC | #FFB547 | Nhắc thiếu feed nhẹ nhưng rõ. |
| Near death notice | #FFE3DD | #FF6B5E | Nếu sắp chết thì vùng cảnh báo rõ. |
| Value / price badges | #FFF1CC | #FFD75E | Giá trị bán có tông vàng rõ. |

## Lưu ý handoff

• Panel này cần đọc tốt với cả gà/vịt/heo/bò, nên bố cục phải đủ co giãn theo tên và số liệu.

• Không cần thêm nhiều stats không dùng tới trong phase đầu.

# 12. Screen 08 - Basic Food Shop Panel

## Mục tiêu màn hình

Màn hỗ trợ mua cỏ hoặc sâu cơ bản để chống bí khi người chơi thiếu nguồn feed. Đây là safety net, không phải nguồn tối ưu. UI cần truyền tải đúng tinh thần đó.

## Ngữ cảnh visual

UI 2D đơn giản, mở nhanh từ tab Shop hoặc từ Animal Detail. Không nên nhìn quá "premium" để tránh người chơi hiểu lầm đây là nguồn chính.

## Bố cục wireframe chi tiết

• Header: "Thức ăn cơ bản", mô tả ngắn "Nguồn hỗ trợ khi bạn thiếu thức ăn".

• Danh sách item rất gọn: Grass Bundle và Worm Bundle, mỗi item có icon, số lượng/pack, giá vàng, nút mua.

• Có thể thêm một note mờ cuối panel: "Chăm cây tốt vẫn là cách kiếm thức ăn hiệu quả hơn".

## Tương tác và hành vi

• Nếu mở từ Animal Detail vì thiếu feed, panel nên highlight trực tiếp gói thức ăn cần mua.

• Mua xong có thể hiện toast và quay lại panel vật nuôi để người chơi cho ăn ngay.

## Trạng thái cần hỗ trợ

• Mở từ Shop thường

• Mở từ vật nuôi thiếu thức ăn

• Mua thành công

## Text / label gợi ý

| Vùng / trạng thái | Text hiển thị gợi ý |
| --- | --- |
| Header | Thức ăn cơ bản |
| Subtext | Nguồn hỗ trợ khi bạn thiếu thức ăn |
| Grass bundle | Bó cỏ cơ bản • 5 • 30 vàng |
| Worm bundle | Gói sâu cơ bản • 5 • 40 vàng |
| Footnote | Chăm cây tốt vẫn là nguồn thức ăn hiệu quả hơn |

## Color mapping theo khu vực UI

| Khu vực | Màu nền / fill | Màu nhấn / state | Ghi chú triển khai |
| --- | --- | --- | --- |
| Panel nền | #FFF7E8 | #B97A4A | Giữ sáng, không quá premium. |
| Help note band | #EAF8E4 | #67DCC8 hoặc #69C34D | Nhấn tính hỗ trợ, không phải tính hiếm. |
| Buy CTA | #69C34D | #4FA63A | Mua vẫn là action tích cực. |
| Pack price tag | #FFF1CC | #FFD75E | Giá dùng vàng nhẹ. |
| Opened from shortage | #FFF1CC | #FFB547 | Nếu mở từ thiếu thức ăn, highlight item cần mua bằng vàng cam. |

## Lưu ý handoff

• Tránh dùng màu hồng hoặc rarity glow ở màn này vì sẽ làm shop thức ăn có vẻ hấp dẫn hơn nó nên có.

# 13. Screen 09 - Mini Game / Event Popup

## Mục tiêu màn hình

Popup giới thiệu mini game hoặc event nhỏ, đóng vai trò tăng hứng thú và cũng là cơ chế comeback khi người chơi cần hạt giống, item hoặc tài nguyên để dựng lại vòng chơi.

## Ngữ cảnh visual

UI 2D nổi hơn các popup thường một chút, dùng màu event rõ hơn nhưng vẫn cùng hệ palette. World phía sau mờ nhẹ để giữ bối cảnh farm.

## Bố cục wireframe chi tiết

• Header event: tên event hoặc mini game, icon vui tươi, có thể có countdown nếu là event theo thời gian.

• Body: mô tả cực ngắn, phần thưởng có thể nhận, CTA Chơi ngay và CTA Bỏ qua.

• Khối reward preview: 3-5 ô quà hiển thị hạt giống, cỏ/sâu, vàng nhỏ, speed-up hoặc item hiếm nhẹ.

• Nếu popup là cơ chế cứu hộ, có thể thêm câu mềm như "Bạn đang cần thêm hạt giống? Hãy thử event này."

## Tương tác và hành vi

• Popup có thể hiện random sau một action, nhưng không spam. Nếu người chơi đóng, quay về flow trước đó nhanh chóng.

• Nếu nhận reward khi kho đầy, luồng phải chuyển sang kho/bán hoặc hiện cảnh báo rõ.

## Trạng thái cần hỗ trợ

• Event thường

• Event comeback / rescue

• Limited-time event

## Text / label gợi ý

| Vùng / trạng thái | Text hiển thị gợi ý |
| --- | --- |
| Header | Mini game mới |
| Body | Chơi nhanh để nhận hạt giống và vật phẩm hỗ trợ |
| CTA | Chơi ngay |
| Secondary | Để sau |
| Rescue line | Bạn đang cần thêm hạt giống? Hãy thử event này. |

## Color mapping theo khu vực UI

| Khu vực | Màu nền / fill | Màu nhấn / state | Ghi chú triển khai |
| --- | --- | --- | --- |
| Popup nền | #FFF7E8 | #FF6FAE + #FFD75E | Event dùng hồng berry và vàng để tăng cảm giác vui. |
| Reward preview cards | #FFFFFF | Rarity badge colors | Mỗi reward card sáng rõ, badge rarity nhỏ. |
| Play CTA | #FF6FAE | #E25596 hoặc glow #FFD75E | CTA event khác CTA thường. |
| Skip CTA | #F5EEDC | #7A6858 | Giữ nhẹ, không cạnh tranh với Play. |
| Rescue note band | #EAF8E4 hoặc #EAF2FF | #67DCC8 hoặc #5BB8FF | Nếu event có vai trò comeback, dùng mint/blue để gợi hỗ trợ. |

## Lưu ý handoff

• Không dùng event popup quá tối hoặc quá lấp lánh. Game farm vẫn là core; event chỉ là lớp phụ nổi bật vừa đủ.

# 14. Screen 10 - Popup hệ thống lõi

## Mục tiêu màn hình

Nhóm popup nhỏ nhưng xuất hiện thường xuyên và cần giữ đồng bộ: Level Up, Unlock Pen, Storage Full, Reward Popup, Warning Popup.

## Ngữ cảnh visual

Tất cả đều là UI 2D. Cùng một bộ shape và typography, chỉ đổi accent color theo mục đích.

## Bố cục wireframe chi tiết

• Level Up Popup: tiêu đề lớn, level mới, danh sách unlock, nút OK hoặc Đi đến nội dung mới.

• Unlock Pen Popup: mô tả loại chuồng, giá mở, điều kiện đạt đủ, CTA Mở chuồng.

• Storage Full Popup: tiêu đề rõ, message ngắn, 2 CTA "Đi đến kho" và "Đi đến bán".

• Reward Popup: card phần thưởng lớn, số lượng, CTA Nhận hoặc Đóng. Có thể dùng cho harvest bonus hoặc event reward.

• Warning Popup: dùng cho cây sắp chết, vật nuôi sắp quá tuổi thọ hoặc hành động quan trọng cần xác nhận nhẹ.

## Tương tác và hành vi

• Popup hệ thống phải có cùng frame, shadow và spacing, chỉ khác accent header hoặc CTA.

• Storage Full là popup gây chặn, nên message cực ngắn và hành động tiếp theo rất rõ.

• Reward popup cần cảm giác vui nhưng không mất nhất quán với style guide chung.

## Trạng thái cần hỗ trợ

• Success / level up

• Unlock

• Blocked / storage full

• Reward

• Warning / danger

## Text / label gợi ý

| Vùng / trạng thái | Text hiển thị gợi ý |
| --- | --- |
| Level up | Lên cấp! |
| Unlock | Mở Chuồng Gà |
| Storage full | Kho đã đầy |
| Reward | Bạn nhận được |
| Warning | Cây sắp chết |

## Color mapping theo khu vực UI

| Khu vực | Màu nền / fill | Màu nhấn / state | Ghi chú triển khai |
| --- | --- | --- | --- |
| Base popup | #FFF7E8 | #B97A4A border | Frame nền dùng chung cho toàn hệ thống. |
| Level up accent | #FFF1CC | #FFD75E + #69C34D | Thành công / mở khóa tích cực. |
| Unlock pen accent | #EAF8E4 | #69C34D | Action tiến triển nên dùng xanh lá. |
| Storage full accent | #FFE3DD | #FF6B5E + #FFB547 | Blocked flow cần cảnh báo mạnh hơn. |
| Reward accent | #FFF1CC | #FFD75E + rarity colors | Quà thưởng nổi bật nhưng ấm áp. |
| Warning accent | #FFE3DD | #FF6B5E | Cảnh báo nguy cơ mất tài nguyên. |

## Lưu ý handoff

• Tất cả popup nên có cùng bán kính bo góc và cùng logic nút primary/secondary để người chơi học một lần là quen.

# 15. Quy tắc handoff cuối

• UI layer luôn là 2D và tách rõ khỏi world layer. World isometric không được kéo thẩm mỹ UI đi theo góc nghiêng.

• Mỗi màn phải có 1 trọng tâm thị giác chính. Farm lấy world làm trung tâm; Kho và Bán hàng lấy khả năng đọc dữ liệu làm trung tâm; Event dùng hồng-vàng nổi hơn nhưng vẫn trong cùng họ màu.

• Action màu xanh lá = hành động tích cực/chăm sóc/mở khóa. Action màu vàng = reward/harvest/value. Action màu cam = bán hoặc warning nhẹ. Action đỏ cam = danger/blocking.

• Không dùng màu hiếm làm nền tràn màn hình. Rarity chỉ nên nằm ở badge, viền hoặc glow vừa đủ.

• Nếu phát triển quick tools ở phase sau, chỉ thêm sau khi bộ panel theo ngữ cảnh ở phase đầu đã ổn định và người chơi hiểu core loop.

## 16. Trạng thái chốt cho phase đầu

| Màn / popup | Trạng thái | Phụ thuộc chính | Ghi chú |
| --- | --- | --- | --- |
| Home Farm | Khóa | World 2.5D + HUD 2D | Core screen, world 2.5D + UI 2D |
| Seed Shop | Khóa | Home Farm / ô đất trống | Mở từ ô đất trống hoặc tab Shop |
| Crop Action Panel | Khóa | State cây | Panel theo ngữ cảnh |
| Storage | Khóa | Kho chung | Kho chung có filter |
| Sell Screen | Khóa | Storage | Giải phóng kho, quay vòng vàng |
| Barn Screen | Khóa | Level + pen unlock | Chuồng theo level + pen unlock |
| Animal Detail | Khóa | Barn Screen | Feed / Sell / shortage flow |
| Food Shop | Khóa | Animal Detail / Shop | Safety net, không phải nguồn tối ưu |
| Mini Game / Event Popup | Khóa | Random trigger / event schedule | Tăng hứng thú + comeback |
| System Popups | Khóa | Core flow | Level up, unlock, storage full, reward, warning |

Kết thúc tài liệu - Bản này dùng cho wireframe, visual prompt và UI/Unity handoff phase đầu.
