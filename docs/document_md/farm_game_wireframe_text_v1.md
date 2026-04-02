# farm_game_wireframe_text_v1

_Source:_ `farm_game_wireframe_text_v1.docx`

Farm Game Wireframe Text Spec v1

Wireframe text cực chi tiết cho phase đầu - world 2.5D isometric + UI 2D

| Mục đích: làm cầu nối giữa game design, UI design, AI UI tool và prototype Unity |
| --- |
| Phạm vi: các màn hình core phase đầu, popup chính, state và interaction theo từng object |

| Nguyên tắc đọc tài liệu<br>Tài liệu này chưa phải mockup hi-fi; nó mô tả cấu trúc màn hình, nội dung hiển thị, trạng thái và luồng tương tác.<br>Tất cả mô tả world đều theo hướng 2.5D isometric bằng asset 2D; toàn bộ HUD, panel và popup là UI 2D.<br>Phase đầu dùng interaction theo từng object; quick tool hàng loạt là hướng mở rộng về sau. |
| --- |

# 1. Nguyên tắc thiết kế chung cho mọi màn

- HUD phải luôn dễ đọc trên mobile: level, EXP, vàng, kho là thông tin ưu tiên.

- Nền game là world 2.5D isometric; panel tương tác nổi trên world bằng layer UI 2D.

- Một object được chạm vào phải cho phản hồi rõ: highlight, bật panel, hoặc action ngay.

- Các popup không dùng quá nhiều chữ; hành động chính phải nổi bật bằng button màu xanh lá hoặc cam tùy ngữ cảnh.

- Cảnh báo quan trọng không chỉ dựa vào màu; phải có icon và trạng thái chữ ngắn.

# 2. Sơ đồ điều hướng phase đầu

1. Người chơi vào game và luôn hạ cánh ở Farm Screen.

1. Từ Farm, người chơi đi sang Kho, Shop hạt giống, Khu chuồng và Event bằng bottom nav hoặc trigger theo ngữ cảnh.

1. Các panel chi tiết mở nổi lên trên Farm/Barn thay vì chuyển scene toàn màn hình khi không cần thiết.

1. Các flow bán hàng, cho ăn, chăm cây và thu hoạch phải ngắn để giữ cảm giác casual.

# 3. Screen 01 - Splash / Loading

Đây là màn vào game nhanh, thời gian xuất hiện ngắn. Dùng để tải save local/session và đẩy người chơi về Farm.

| Khu vực | Vị trí / kích thước tương đối | Nội dung hiển thị | Ghi chú UX |
| --- | --- | --- | --- |
| Logo game | Giữa trên, chiếm khoảng 20-25% chiều cao đầu màn | Tên game, subtitle ngắn nếu cần | Logo vui tươi, sáng, phù hợp farm casual. |
| Background art | Full screen | Khung cảnh nông trại tổng quát với trời xanh, cỏ, ruộng, chuồng nhỏ theo góc isometric | Art chỉ mang tính giới thiệu, không cần tương tác. |
| Loading area | Giữa dưới | Loading bar hoặc icon loading + tip ngắn | Tip nên là câu hữu ích như 'Chăm cây tốt để giữ sản lượng cao'. |

| Đối tượng / trigger | Điều kiện | Phản hồi UI | Kết quả |
| --- | --- | --- | --- |
| Hoàn tất tải | Save và data đã sẵn sàng | Ẩn loading, chuyển thẳng sang Farm | Không giữ người chơi ở màn này lâu. |

# 4. Screen 02 - Home Farm Screen

Màn trung tâm của phase đầu. Đây là nơi người chơi dành phần lớn thời gian: trồng cây, chăm cây, thu hoạch, nhận cảnh báo, đi sang Kho hoặc Chuồng.

| Khu vực | Vị trí / kích thước tương đối | Nội dung hiển thị | Ghi chú UX |
| --- | --- | --- | --- |
| Top HUD | Dải ngang phía trên cùng, cao ~12-14% màn hình | Avatar nhỏ, level, thanh EXP, vàng, kho đã dùng/tối đa, settings nhỏ | Mọi số liệu phải đủ lớn để đọc nhanh. |
| World viewport | Vùng giữa, chiếm phần lớn màn hình | Nền farm 2.5D isometric: ruộng, lối đi, decor cơ bản, hướng sang khu chuồng | Đây là layer world 2.5D; các object trong world là sprite 2D đồng góc nhìn. |
| Crop grid / plot cluster | Giữa trọng tâm của viewport | Các ô đất, cây theo phase, icon Needs Care, hiệu ứng chín | Ô đất là object chạm chính ở phase đầu. |
| Right utility cluster | Góc phải giữa hoặc phải dưới | Nút sự kiện, cảnh báo nhanh, icon mini game nếu có | Không đặt quá nhiều icon; tối đa 2-3 nút nổi. |
| Bottom nav | Dải dưới cùng, cao ~12-14% màn hình | Farm, Kho, Shop, Chuồng, Event | UI 2D nổi trên world; state selected phải rất rõ. |

| State | Biểu hiện trực quan | CTA / hành động chính | Ghi chú |
| --- | --- | --- | --- |
| Ô đất trống | Ô đất sạch, không có cây, đất nâu ấm | Tap để mở panel trồng cây | Có thể thêm hiệu ứng hover/highlight khi chạm. |
| Cây đang lớn | Cây theo phase 1-3, có timer nhỏ nếu cần | Tap để xem info; nếu có vấn đề thì hiện action tương ứng | Không nên bắt người chơi mở menu riêng trước. |
| Needs Care | Icon cảnh báo chung hoặc icon cỏ/sâu/nước + cây bị ảnh hưởng nhẹ về màu | Tap để mở Crop Action Panel | State này phải dễ phát hiện bằng cả icon và animation nhẹ. |
| Đã chín | Glow nhẹ, cây đạt màu tươi nhất, có thể có icon ready | Tap để thu hoạch hoặc mở panel thu hoạch ngắn | Thu hoạch nên cực nhanh, ít bước. |
| Kho đầy | HUD kho chuyển màu cảnh báo; có thể có popup mềm | Tap thu hoạch bị chặn và mở thông báo kho đầy | Không thu lén vào hư không. |

| Đối tượng / trigger | Điều kiện | Phản hồi UI | Kết quả |
| --- | --- | --- | --- |
| Tap ô đất trống | Object = Empty plot | Mở Plant Panel / Seed Shop contextual | Người chơi chọn hạt và gieo ngay vào ô đó. |
| Tap cây có vấn đề | Object = Crop Needs Care | Mở Crop Action Panel với đúng nút cần thiết | Chỉ hiện cắt cỏ, bắt sâu, tưới nước khi thật sự cần. |
| Tap cây đã chín | Object = Ripe crop | Thu nhanh hoặc mở panel thu ngắn | Sau thu, item vào kho nếu còn chỗ. |
| Tap icon Kho | Bất kỳ lúc nào trên Farm | Mở Storage Screen | Không rời world quá lâu. |
| Tap icon Chuồng | Bất kỳ lúc nào trên Farm | Chuyển sang Barn Screen | Dùng same art direction, khác khu chức năng. |
| Tap icon Event | Khi có event hoặc có nút cố định | Mở Event / Mini Game Popup | Phase đầu event là hệ phụ, không lấn core farm. |

| Microcopy gợi ý cho Farm Screen<br>Kho đã đầy. Hãy bán hoặc sử dụng bớt vật phẩm.<br>Cây cần chăm sóc.<br>Thu hoạch đẹp! Sản lượng được thưởng.<br>Cây đang yếu. Hãy xử lý cỏ, sâu hoặc thiếu nước. |
| --- |

# 5. Screen 03 - Plant Panel / Seed Shop theo ngữ cảnh

Panel này mở từ ô đất trống hoặc từ tab Shop. Trong phase đầu, panel gieo theo ngữ cảnh là cách chơi chính; người chơi không phải vào một scene shop tách rời chỉ để trồng một hạt.

| Khu vực | Vị trí / kích thước tương đối | Nội dung hiển thị | Ghi chú UX |
| --- | --- | --- | --- |
| Panel container | Nổi ở giữa hoặc bottom sheet chiếm 55-70% chiều cao | Tiêu đề, filter, grid card hạt giống, button gieo | UI 2D; nền kem sáng, bo tròn lớn. |
| Seed card list | Phần thân panel | Icon cây, tên, level mở, giá, thời gian lớn, tag ngắn/trung/dài | Card khóa mờ nếu chưa đủ level. |
| Filter row | Đầu panel hoặc ngay dưới tiêu đề | Tất cả, Ngắn hạn, Trung hạn, Dài hạn, Hiếm | Phase đầu filter đơn giản để hỗ trợ đọc data nhanh. |
| Footer action | Cuối panel | Nút Gieo / Hủy | Nếu mở từ ô đất cụ thể thì gieo vào đúng ô đó. |

| Đối tượng / trigger | Điều kiện | Phản hồi UI | Kết quả |
| --- | --- | --- | --- |
| Chọn seed card | Card đã mở khóa và đủ vàng | Card được selected, hiện preview giá / thời gian | Gieo chưa diễn ra cho tới khi nhấn xác nhận hoặc tap card theo flow chọn nhanh. |
| Tap Gieo | Có card được chọn | Đóng panel, ô đất chuyển sang state Seeded | Animation gieo hạt ngắn, vui. |
| Tap card bị khóa | Chưa đủ level | Hiện tooltip level yêu cầu | Không nên cho panel lỗi dài. |
| Tap card thiếu vàng | Đã mở khóa nhưng không đủ tiền | Button gieo disabled hoặc tooltip thiếu vàng | Ngắn gọn, không chặn trải nghiệm quá lâu. |

# 6. Screen 04 - Crop Action Panel

Đây là panel quan trọng nhất để giữ rule interaction theo từng object. Nó phải hiển thị đúng việc cây đang cần, không thừa nút và không ép người chơi học mode thao tác toàn cục.

| Khu vực | Vị trí / kích thước tương đối | Nội dung hiển thị | Ghi chú UX |
| --- | --- | --- | --- |
| Header | Phần trên của panel | Tên cây, icon cây, phase hiện tại | Cho người chơi biết mình đang chạm vào cây nào. |
| Status block | Ngay dưới header | HP, timer còn lại, trạng thái hiện tại, khung thu đẹp nếu có | Thông tin cốt lõi phải đọc trong 1-2 giây. |
| Action buttons | Giữa hoặc cuối panel | Cắt cỏ, Bắt sâu, Tưới nước, Thu hoạch - chỉ hiện nút phù hợp | Primary action phải nổi bật nhất. |
| Helper note | Dòng nhỏ cuối panel | Ví dụ: Thu trong 2 phút tới để nhận bonus. | Dùng microcopy ngắn, không dài dòng. |

| State | Biểu hiện trực quan | CTA / hành động chính | Ghi chú |
| --- | --- | --- | --- |
| Cây bình thường | HP hiển thị xanh, không có icon vấn đề | Đóng panel hoặc chỉ xem thông tin | Không cần ép thao tác. |
| Có cỏ | Icon cỏ / cảnh báo Needs Care | Nút Cắt cỏ | Sau khi xử lý, panel cập nhật hoặc đóng. |
| Có sâu | Icon sâu / cảnh báo Needs Care | Nút Bắt sâu | Thường quan trọng hơn cỏ vì drain HP mạnh hơn. |
| Thiếu nước | Icon giọt nước có dấu chấm than | Nút Tưới nước | Không tạo item; chỉ gỡ trạng thái. |
| Đã chín | Timer tăng trưởng hoàn tất, trạng thái Ready | Nút Thu hoạch | Nếu đang trong perfect window, hiển thị benefit rõ. |
| Trễ thu hoạch | Cảnh báo nhẹ màu vàng/cam | Nút Thu hoạch | Hiển thị rằng sản lượng đã giảm nhưng vẫn còn thu được. |
| Sắp chết | HP rất thấp hoặc gần hết post-ripe life | CTA xử lý hoặc thu hoạch ngay | Cần visual và microcopy rõ hơn bình thường. |

| Đối tượng / trigger | Điều kiện | Phản hồi UI | Kết quả |
| --- | --- | --- | --- |
| Tap Cắt cỏ | Cây đang có cỏ | Hiệu ứng cắt nhanh, nhận Grass | HP ngừng bị trừ bởi cỏ. |
| Tap Bắt sâu | Cây đang có sâu | Hiệu ứng pop, nhận Worm | HP ngừng bị trừ bởi sâu. |
| Tap Tưới nước | Cây đang thiếu nước | Hiệu ứng nước nhẹ | Gỡ trạng thái thiếu nước. |
| Tap Thu hoạch | Cây đã chín / trễ | Mở Harvest Result Popup hoặc thu ngay + reward flyout | Nếu kho đầy thì chặn và báo kho đầy. |

# 7. Screen 05 - Storage / Warehouse

Kho là trung tâm quản lý tài nguyên của phase đầu. Vì kho chung và có giới hạn, màn này phải rất rõ, thao tác bán phải nhanh, và người chơi phải đọc được ngay kho đang đầy ở đâu.

| Khu vực | Vị trí / kích thước tương đối | Nội dung hiển thị | Ghi chú UX |
| --- | --- | --- | --- |
| Header | Phần trên | Tên kho, capacity 27/50, trạng thái near full/full | Phải nổi rõ hơn các màn phụ khác vì đây là màn quản lý tài nguyên. |
| Filter tabs | Ngay dưới header | Tất cả, Hạt giống, Nông sản, Nguyên liệu nuôi, Event | Mỗi tab nên có số lượng stack nếu muốn hỗ trợ. |
| Inventory list/grid | Phần thân chính | Item card gồm icon, tên, stack, giá bán (nếu có), nút dùng hoặc bán | Scroll dọc; card đủ to cho mobile. |
| Footer quick actions | Phần dưới | Bán nhanh, Bán theo lọc, Đóng | Chỉ cần 1-2 CTA mạnh, đừng nhồi quá nhiều. |

| State | Biểu hiện trực quan | CTA / hành động chính | Ghi chú |
| --- | --- | --- | --- |
| Kho bình thường | Capacity hiển thị màu trung tính | Duyệt item, bán hoặc giữ | Không gây áp lực. |
| Kho gần đầy | Capacity vàng/cam + note ngắn | Khuyến khích người chơi dọn kho | Không chặn thao tác. |
| Kho đầy | Capacity đỏ/cam mạnh, note rõ | Bán hoặc dùng item để giải phóng chỗ | Các action nhận item từ game sẽ bị chặn cho tới khi còn chỗ. |

| Đối tượng / trigger | Điều kiện | Phản hồi UI | Kết quả |
| --- | --- | --- | --- |
| Tap item card | Bất kỳ item nào | Mở item detail ngắn hoặc chọn item | Phù hợp cho bán số lượng cụ thể. |
| Tap tab lọc | Bất kỳ lúc nào | Lọc lại danh sách item | Không thay logic chứa, chỉ đổi view. |
| Tap Bán nhanh | Item bán được đang được chọn | Mở Sell Popup | Giữ thao tác giải phóng kho thật nhanh. |

# 8. Screen 06 - Sell Popup / Sell Screen

Đây là cầu nối từ kho sang vàng. Vì kho đầy là vấn đề có thể chặn gameplay, popup bán phải giải quyết rất nhanh và đủ minh bạch về số vàng nhận.

| Khu vực | Vị trí / kích thước tương đối | Nội dung hiển thị | Ghi chú UX |
| --- | --- | --- | --- |
| Header | Trên popup | Tiêu đề Bán vật phẩm, icon vàng | Rõ mục đích, không cần quá nhiều lời. |
| Sell list | Giữa popup | Tên item, số lượng hiện có, giá mỗi đơn vị, stepper +/- hoặc slider | Có thể cho bán nhiều item cùng lúc nếu vẫn rõ. |
| Summary area | Cuối popup | Tổng vàng nhận, tổng số item đang bán | Tổng tiền phải nổi bật. |
| Action footer | Cuối cùng | Xác nhận bán, Hủy | CTA bán dùng màu cam hoặc xanh tùy style. |

| Đối tượng / trigger | Điều kiện | Phản hồi UI | Kết quả |
| --- | --- | --- | --- |
| Tăng / giảm số lượng | Item đang chọn để bán | Cập nhật tổng vàng ngay | Tương tác phải mượt. |
| Tap Bán | Có ít nhất một item được chọn | Xác nhận ngắn, nhận vàng, trừ item trong kho | Nếu là flow từ kho đầy, nên quay lại action trước đó hoặc kho. |
| Tap Hủy | Bất kỳ lúc nào | Đóng popup, không thay đổi dữ liệu | Không gây mất mát. |

# 9. Screen 07 - Barn / Animal Area

Barn là nơi thể hiện lớp gameplay thứ hai của phase đầu. Màn này vẫn theo world 2.5D isometric, nhưng trọng tâm là khu chuồng và trạng thái từng loại vật nuôi.

| Khu vực | Vị trí / kích thước tương đối | Nội dung hiển thị | Ghi chú UX |
| --- | --- | --- | --- |
| Top HUD | Giống Farm HUD | Level, EXP, vàng, kho, có thể thêm Grass/Worm quick count | Giữ thống nhất toàn game. |
| Barn world viewport | Phần giữa | Khu chuồng gà, vịt, heo, bò; lối đi, decor nhẹ, trạng thái chuồng | World là 2.5D isometric bằng asset 2D. |
| Pen cards / pen world objects | Trong viewport | Chuồng khóa, chuồng trống, chuồng có vật nuôi, icon đói / bán được | Mỗi chuồng vừa là object world vừa có thể hiện status overlay. |
| Bottom nav | Giống Farm | Farm, Kho, Shop, Chuồng, Event | Giúp chuyển màn nhanh. |

| State | Biểu hiện trực quan | CTA / hành động chính | Ghi chú |
| --- | --- | --- | --- |
| Chuồng bị khóa | Mờ màu, icon khóa, hiển thị level hoặc chi phí mở | Xem yêu cầu / Mở chuồng khi đủ | Không cho mua con giống. |
| Chuồng trống | Sạch, có nút hoặc badge 'Mua con giống' | Mua vật nuôi | State này xuất hiện sau khi mở chuồng thành công. |
| Vật nuôi đang lớn | Có con vật trong chuồng, timer nhỏ | Tap để xem detail | Không cần nhiều icon nếu chưa đói. |
| Vật nuôi đói | Bubble thức ăn nổi bật | Tap để mở panel cho ăn | Phải dễ thấy, vì đói làm dừng tăng trưởng. |
| Bán được / trưởng thành | Glow nhẹ hoặc badge bán | Tap để mở panel bán | Không nên quá rực để tránh gây áp lực liên tục. |
| Sắp chết | Warning rõ | Tap mở detail và ưu tiên bán | Cảnh báo này quan trọng như cây sắp chết. |

| Đối tượng / trigger | Điều kiện | Phản hồi UI | Kết quả |
| --- | --- | --- | --- |
| Tap chuồng khóa | Chưa đủ điều kiện | Tooltip level yêu cầu hoặc popup mở chuồng | Không chuyển scene riêng. |
| Tap chuồng trống | Đã mở chuồng | Mở panel mua con giống | Flow phải ngắn gọn. |
| Tap vật nuôi | Chuồng đang có con vật | Mở Animal Detail Panel | Panel theo ngữ cảnh như cây. |
| Tap icon shop thức ăn | Thiếu Grass/Worm | Mở Food Shop | Đây là safety net, không phải core loop. |

# 10. Screen 08 - Animal Detail / Feed Panel

Panel này là bản tương đương Crop Action Panel nhưng cho vật nuôi. Nó phải giúp người chơi quyết định rất nhanh: cho ăn, giữ tiếp hay bán.

| Khu vực | Vị trí / kích thước tương đối | Nội dung hiển thị | Ghi chú UX |
| --- | --- | --- | --- |
| Header | Trên panel | Tên vật nuôi, icon/góc nhìn của con vật, giai đoạn hiện tại | Cho người chơi nhận diện trạng thái ngay. |
| Status summary | Phần trên giữa | Timer đến giai đoạn tiếp theo, hunger status, tuổi thọ còn lại nếu đã trưởng thành | Thông tin quyết định hành động chính. |
| Value block | Phần giữa | Giá bán hiện tại, giá bán ở giai đoạn kế tiếp, loại thức ăn cần | Hỗ trợ quyết định bán sớm hay giữ lâu. |
| Action footer | Cuối panel | Cho ăn, Bán ngay, Mua thức ăn | CTA chính thay đổi theo trạng thái. |

| State | Biểu hiện trực quan | CTA / hành động chính | Ghi chú |
| --- | --- | --- | --- |
| Đang lớn bình thường | Không đói, chưa bán đẹp | Xem info, đóng panel | Người chơi chỉ theo dõi. |
| Đói | Bubble thức ăn + note thiếu resource nếu có | Cho ăn hoặc mở Food Shop | Đây là state được ưu tiên nhất. |
| Qua giai đoạn mới | Hiển thị giá bán mới, animation nhỏ | Giữ tiếp hoặc bán | Giúp người chơi thấy phần thưởng của việc chờ. |
| Trưởng thành | Giá bán tốt nhất hoặc gần tốt nhất | Bán hoặc tiếp tục giữ trong thời gian an toàn | Cần cảnh báo tuổi thọ còn lại. |
| Sắp chết | Warning mạnh | Bán ngay | Tránh để người chơi quên và mất trắng. |

| Đối tượng / trigger | Điều kiện | Phản hồi UI | Kết quả |
| --- | --- | --- | --- |
| Tap Cho ăn | Đủ Grass/Worm trong kho | Trừ tài nguyên, ẩn hunger state, tiếp tục timer | Nếu thiếu tài nguyên, gợi ý sang shop. |
| Tap Mua thức ăn | Thiếu food | Mở Food Shop dạng overlay hoặc chuyển nhanh | Sau khi mua xong quay lại panel con vật. |
| Tap Bán ngay | Bất kỳ giai đoạn bán được | Nhận vàng + XP, chuồng về trạng thái trống | Flow càng nhanh càng tốt. |

# 11. Screen 09 - Basic Food Shop

Shop thức ăn là safety net của phase đầu. Giao diện phải cho người chơi cảm giác đây là giải pháp cứu hộ, không phải nguồn chơi tối ưu.

| Khu vực | Vị trí / kích thước tương đối | Nội dung hiển thị | Ghi chú UX |
| --- | --- | --- | --- |
| Header | Trên cùng của panel / màn phụ | Tên cửa hàng thức ăn, note ngắn về tính năng hỗ trợ | Không nên làm quá hoành tráng như shop premium. |
| Food cards | Phần giữa | Grass Bundle, Worm Bundle, số lượng/pack, giá vàng | Card đơn giản, cực dễ mua. |
| Info note | Dưới card hoặc cuối màn | Ghi chú: chăm cây tốt vẫn là nguồn thức ăn hiệu quả hơn | Giúp định vị đúng vai trò của shop. |
| Action area | Cuối màn | Mua / Đóng | Mua xong nên quay lại panel đang thiếu thức ăn. |

| Đối tượng / trigger | Điều kiện | Phản hồi UI | Kết quả |
| --- | --- | --- | --- |
| Tap Mua Grass Bundle | Có đủ vàng | Trừ vàng, cộng Grass vào kho | Nếu kho đầy, chặn mua và báo rõ. |
| Tap Mua Worm Bundle | Có đủ vàng | Trừ vàng, cộng Worm vào kho | Sau mua xong quay về Animal Detail nếu vào từ đó. |

# 12. Screen 10 - Event / Mini Game Popup

Popup event/minigame trong phase đầu phải cực ngắn, vui, có cảm giác bất ngờ nhưng không phá nhịp farm. Nó còn đóng vai trò comeback mechanic khi người chơi hụt vốn hoặc hết hạt.

| Khu vực | Vị trí / kích thước tương đối | Nội dung hiển thị | Ghi chú UX |
| --- | --- | --- | --- |
| Popup shell | Giữa màn, nổi trên layer UI | Tên event, icon event, mô tả 1-2 dòng | Dùng màu event như berry pink / vàng nhẹ để phân biệt. |
| Reward preview | Giữa popup | Hạt giống, vàng nhỏ, cỏ, sâu, speed-up hoặc item hỗ trợ | Giúp người chơi hiểu tại sao nên chơi. |
| CTA buttons | Cuối popup | Chơi ngay, Bỏ qua | Không ép người chơi trong mọi trường hợp. |

| Đối tượng / trigger | Điều kiện | Phản hồi UI | Kết quả |
| --- | --- | --- | --- |
| Trigger random khi đang chơi | Cắt cỏ, bắt sâu, tưới nước, thu hoạch, cho ăn | Popup event xuất hiện | Tần suất thấp để giữ cảm giác bất ngờ. |
| Trigger từ nút Event | Người chơi chủ động mở | Mở popup event đang sẵn có | Giúp event không hoàn toàn ngẫu nhiên. |
| Hoàn thành mini game | Thắng mini game | Mở reward popup; quà vào kho nếu còn chỗ | Nếu kho đầy thì dẫn người chơi sang kho/bán. |

# 13. Popup bắt buộc cần có ở phase đầu

| Popup | Khi nào xuất hiện | Nội dung cốt lõi | CTA |
| --- | --- | --- | --- |
| Level Up | Người chơi lên level | Level mới, unlock mới, có thể gợi ý đi tới Chuồng nếu vừa mở | OK / Đi tới khu vừa mở |
| Kho đầy | Người chơi thu hoặc nhận item khi kho full | Thông báo chặn thao tác và hướng giải quyết | Đi tới Kho / Đi tới Bán |
| Harvest Result | Người chơi vừa thu hoạch cây | Sản lượng nhận được, XP, bonus nếu thu đẹp | OK |
| Unlock Pen | Người chơi đủ điều kiện mở chuồng | Tên chuồng, giá mở, lợi ích | Mở chuồng / Hủy |
| Animal Stage Up | Vật nuôi đạt giai đoạn mới | Giai đoạn mới, giá bán mới | Xem / OK |
| Warning | Cây sắp chết hoặc vật nuôi sắp quá tuổi thọ | Cảnh báo rõ và lý do | Đi tới object / Đóng |

# 14. Wireframe theo flow người dùng

## 14.1 Flow trồng cây

1. Người chơi nhìn Farm và thấy ô đất trống.

1. Tap vào ô đất trống để mở Plant Panel.

1. Chọn hạt giống phù hợp và nhấn Gieo.

1. Ô đất chuyển sang state đang trồng; world cập nhật sprite cây giai đoạn đầu.

1. Trong lúc lớn, nếu phát sinh cỏ/sâu/thiếu nước, cây hiện Needs Care.

1. Tap cây để mở Crop Action Panel và xử lý đúng trạng thái.

1. Khi cây chín, tap để thu hoạch. Nếu kho đầy, flow bị chặn bởi popup Kho đầy.

## 14.2 Flow chăn nuôi

1. Người chơi đạt đủ level và vào Barn.

1. Tap chuồng khóa để xem yêu cầu hoặc mở chuồng.

1. Sau khi mở chuồng, tap chuồng trống để mua con giống.

1. Vật nuôi xuất hiện và chạy timer lớn lên.

1. Khi vật nuôi đói, bubble thức ăn hiện lên; tap con vật để mở Animal Detail.

1. Cho ăn đúng loại hoặc mở Food Shop nếu thiếu thức ăn.

1. Khi vật nuôi qua giai đoạn mới hoặc đến lúc bán được, panel hiển thị giá trị và người chơi quyết định giữ/bán.

## 14.3 Flow comeback qua mini game

1. Người chơi hụt vàng, hết hạt giống hoặc đang thiếu tài nguyên vòng chơi.

1. Event hoặc mini game xuất hiện theo trigger random hoặc qua nút Event.

1. Người chơi chơi event ngắn và nhận hạt giống cơ bản, vàng nhỏ hoặc nguyên liệu nuôi.

1. Reward vào kho và người chơi có thể dựng lại vòng trồng trọt/chăn nuôi.

# 15. Rule nội dung và microcopy cho UI

- Text trên popup và panel phải ngắn, rõ, dùng động từ trực tiếp: Gieo, Thu hoạch, Cắt cỏ, Bắt sâu, Tưới nước, Cho ăn, Bán ngay.

- Cảnh báo phải ngắn nhưng đủ nghĩa: Cây sắp chết, Kho đã đầy, Vật nuôi đang đói, Hãy thu hoạch ngay để nhận thưởng tốt hơn.

- Không viết mô tả dài trong flow chính; phần giải thích chỉ nên xuất hiện ở note nhỏ hoặc tutorial ban đầu.

# 16. Handoff note cho designer và dev

- Designer cần giữ rõ sự phân lớp giữa world 2.5D isometric và HUD/panel 2D để game không bị lẫn ngôn ngữ thị giác.

- Dev nên tổ chức panel theo logic contextual action: mỗi object trả ra đúng action hiện tại, không dùng toolbar mode ở phase đầu.

- Các popup chặn flow như Kho đầy hoặc cảnh báo sắp chết phải được ưu tiên vì chúng liên quan trực tiếp tới retention và cảm giác công bằng.

- Khi làm prototype, nên ưu tiên Farm, Plant Panel, Crop Action Panel, Storage, Sell Popup, Barn, Animal Detail trước. Event và popup phụ có thể nối sau.
