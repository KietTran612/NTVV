# farm_game_overview_7day_v1

_Source:_ `farm_game_overview_7day_v1.docx`

NÔNG TRẠI CASUAL - TỔNG QUAN THIẾT KẾ & 7 NGÀY ĐẦU

Tài liệu mô tả concept game, cách chơi, hành trình người chơi và mục tiêu cân bằng cho tuần đầu

| Tài liệu nội bộ - dùng làm mốc dữ liệu khởi tạo cho prototype, review balance và đối chiếu thiết kế ở các vòng phát triển sau. |
| --- |

# 1. Tóm tắt định hướng sản phẩm

Đây là một game nông trại casual theo tinh thần hoài niệm, trong đó trồng trọt và chăn nuôi là hai hệ cốt lõi liên kết chặt với nhau. Điểm khác biệt quan trọng là cây trồng không chỉ gieo rồi chờ, mà còn có cỏ, sâu và thiếu nước làm ảnh hưởng trực tiếp tới thanh sức khỏe, sản lượng và giá trị thu hoạch.

- Trồng trọt là nguồn vốn chính ở đầu game.

- Chăm cây tốt sẽ sinh thêm cỏ và sâu - đây là tài nguyên phụ để nuôi thú.

- Chăn nuôi là bước đầu tư mở sau, nhịp dài ngày hơn để người chơi cảm nhận rõ sự phát triển của farm.

- Mini game / event là cơ chế tăng hứng thú và cũng là đường comeback khi người chơi thiếu vốn hoặc thiếu hạt giống.

# 2. Core gameplay và mối liên kết giữa các hệ

Vòng chơi cơ bản của game được xây theo hướng 'trồng trọt nuôi chăn nuôi'. Người chơi bắt đầu bằng việc gieo hạt, bán nông sản và dần tích lũy vàng. Khi đã quen loop chăm cây và có lượng cỏ / sâu đủ tốt, họ mới bắt đầu bước sang chăn nuôi.

| Mốc hành trình | Người chơi đang ở đâu | Ý nghĩa |
| --- | --- | --- |
| Bắt đầu | 6 ô đất, hạt giống rẻ, chưa có chuồng | Học vòng lặp cơ bản của trồng trọt |
| Giữa tuần đầu | Đã có 8-14 ô đất, bắt đầu nuôi gà / vịt | Farm chuyển từ 1 lớp sang 2 lớp |
| Cuối tuần đầu | Có vốn, có kho rõ ràng, có ít nhất 1 thú đầu tiên | Người chơi hiểu cách quản lý tài nguyên và có mục tiêu cho tuần 2 |

Ý nghĩa loop: Điểm mấu chốt của thiết kế là trồng trọt không bị tách khỏi chăn nuôi. Nếu chăm cây tốt, người chơi vừa tối ưu sản lượng vừa có thêm thức ăn cho thú. Nếu chăm tệ, họ mất cả lợi nhuận lẫn nguồn thức ăn.

# 3. Cách chơi phase đầu

- Gieo hạt trên các ô đất đang mở khóa.

- Quan sát trạng thái cây; khi có cảnh báo 'cần chăm sóc' thì xử lý cỏ, sâu hoặc thiếu nước.

- Giữ HP cây ở mức tốt để tối ưu sản lượng và doanh thu khi thu hoạch.

- Đưa sản lượng vào kho và bán để lấy vàng mua hạt giống mới.

- Từ level 4 trở đi, mở chuồng và bắt đầu chăn nuôi nếu đã có vốn dự phòng.

- Dùng cỏ và sâu kiếm được từ chăm cây để cho vật nuôi ăn; chỉ dùng shop thức ăn khi bị bí.

- Khi thiếu vốn, mini game / event có thể trả ra hạt giống, vàng nhỏ hoặc tài nguyên cứu hộ để người chơi gầy dựng lại vòng chơi.

# 4. Hành trình 7 ngày đầu

Bảng dưới đây là mục tiêu cân bằng cho prototype. Các con số vàng và XP là khoảng tham chiếu để team design, economy và QA có cùng một chuẩn đánh giá khi test tuần đầu.

| Ngày | Mục tiêu chính | Level mục tiêu | Vàng cuối ngày (ước tính) | XP tích lũy (ước tính) | Điểm nhấn gameplay |
| --- | --- | --- | --- | --- | --- |
| Ngày 1 | Học gieo trồng, bán nông sản, hiểu kho và chăm cây | Lv2-3 | 120-220 | 50-80 | Biết cỏ / sâu / thiếu nước làm cây tụt HP; chưa chạm chăn nuôi |
| Ngày 2 | Ổn định vòng quay vốn, mở thêm cây trung bình | Lv3-4 | 280-420 | 170-230 | Chuẩn bị điều kiện mở Chuồng Gà và shop thức ăn |
| Ngày 3 | Mở Chuồng Gà, mua gà đầu tiên nếu quản lý tốt | Lv5-6 | 140-280* | 310-390 | Bắt đầu giữ sâu lại để nuôi gà; *vàng giảm nếu đã đầu tư chuồng + gà |
| Ngày 4 | Farm có 2 lớp: trồng trọt + gà; chạm Chuồng Vịt | Lv7-8 | 250-420 | 520-620 | Người chơi hiểu cỏ / sâu là tài nguyên cho chăn nuôi |
| Ngày 5 | Tối ưu chọn cây online / offline, quản lý kho | Lv8-9 | 420-650 | 760-900 | Có thể nâng kho lần 1 và cân nhắc vịt |
| Ngày 6 | Heo trở thành mục tiêu lớn của tuần đầu | Lv10 | 650-950 | 1050-1250 | Trồng trọt phải đủ khỏe mới nuôi thú lớn ổn định |
| Ngày 7 | Kết thúc tuần đầu với farm vận hành rõ ràng | Lv10-11 | 900-1300** | 1380-1600 | **nếu chưa đầu tư Heo; nếu đã đầu tư mạnh, vàng giữ thấp hơn nhưng có tài sản sống |

# 5. Diễn giải chi tiết 7 ngày đầu

Phần này chuyển bảng cân bằng ở trên thành câu chuyện trải nghiệm. Mục tiêu là giúp người đọc hình dung người chơi thật sự sẽ đi từ đâu tới đâu trong tuần đầu, thay vì chỉ đọc các mốc số liệu.

### Ngày 1

Người chơi chỉ cần học loop trồng trọt cơ bản: mua hạt, gieo, chăm cây, thu hoạch, bán hàng và nhìn thấy kho có giới hạn. Trải nghiệm quan trọng nhất là hiểu rằng cỏ / sâu / thiếu nước không phải hình thức trang trí, mà là yếu tố ảnh hưởng trực tiếp tới HP và sản lượng.

### Ngày 2

Người chơi bắt đầu có vòng quay vốn ổn hơn, mở thêm cây trung bình và dần hiểu rằng chăm tốt thì lời hơn. Đây cũng là lúc game chuẩn bị cho việc mở Chuồng Gà, nhưng chưa buộc người chơi phải bước sang hệ nuôi ngay.

### Ngày 3

Nếu quản lý ổn, người chơi có thể mở Chuồng Gà và mua con gà đầu tiên. Đây là khoảnh khắc chuyển game từ farm một lớp sang farm hai lớp. Người chơi bắt đầu có lý do giữ sâu lại để nuôi thú thay vì chỉ xem sâu là một sự cố cần xóa.

### Ngày 4

Trồng trọt vẫn là nguồn vàng chính, nhưng chăn nuôi bắt đầu tạo cảm giác đầu tư. Người chơi làm quen với việc duy trì tài nguyên cho thú song song với việc canh thu hoạch cây. Chuồng Vịt trở thành mục tiêu kế tiếp.

### Ngày 5

Bản sắc quản lý của game bắt đầu rõ hơn: cây nào hợp khi online ngắn, cây nào hợp khi offline lâu, lúc nào nên bán, lúc nào nên giữ tài nguyên cho thú. Người chơi cũng bắt đầu cảm nhận áp lực kho rõ ràng hơn.

### Ngày 6

Heo trở thành mục tiêu lớn đầu tiên của tuần đầu. Đây là một phép thử cho nền kinh tế của farm: nếu trồng trọt chưa ổn, người chơi sẽ khó nuôi thú lớn một cách thoải mái. Mốc này giúp game giữ được mục tiêu dài hơn cho người chơi tích cực.

### Ngày 7

Kết thúc tuần đầu, người chơi nên thấy farm của mình đã thật sự vận hành: có ruộng, có kho, có ít nhất một loại thú đầu tiên, có đủ kinh nghiệm để hiểu giá trị của chăm sóc cây và vẫn còn mục tiêu cho tuần 2 như Heo, Bò, nâng kho hoặc hạt giống hiếm.

# 6. Ước tính vàng và XP của tuần đầu

Các con số dưới đây không phải cam kết tuyệt đối; chúng là vùng mục tiêu để đánh giá xem progression có chạy quá nhanh, quá chậm hay đủ mượt cho người chơi casual vào game 3-5 lần mỗi ngày.

- XP tích lũy mục tiêu sau ngày 7: khoảng 1380-1600, tương ứng Level 10-11.

- Mốc đẹp để mua Gà đầu tiên là cuối ngày 2 hoặc ngày 3; Heo nên trở thành mục tiêu rõ ràng ở ngày 6-7.

- Vàng cuối ngày 7 có thể ở khoảng 900-1300 nếu chưa đầu tư mạnh vào Heo; nếu đã đầu tư, vàng cầm tay thấp hơn nhưng có tài sản sống đang chạy lợi nhuận.

# 7. Vai trò của mini game / event

Mini game không chỉ là phần thưởng vui mắt. Ở project này, chúng còn đóng vai trò như một cơ chế cứu vãn khi người chơi chăm cây kém, thiếu vàng hoặc không còn đủ hạt giống để tái khởi động vòng chơi. Đây là lớp an toàn thứ ba sau trồng trọt và shop cơ bản.

| Giai đoạn | Vai trò của mini game / event | Loại thưởng chính | Tác dụng lên progression |
| --- | --- | --- | --- |
| Phase đầu | Cứu hộ + tạo niềm vui | Hạt giống cơ bản, vàng nhỏ, cỏ, sâu, speed-up nhẹ | Giúp người chơi dựng lại vòng chơi khi thiếu vốn |
| Phase giữa | Hỗ trợ phát triển | Hạt giống trung cấp, item chăm sóc, item bảo vệ cây | Tăng hiệu suất và bù sai lầm |
| Phase sau | Event nội dung lớn | Hạt giống hiếm, con giống hiếm, vật phẩm giới hạn, decor, trophy | Retention dài hạn và progression phụ |

# 8. Rủi ro cần theo dõi khi test prototype

Các rủi ro dưới đây là những điểm nên được quan sát ngay từ bản build đầu để tránh lệch hướng quá xa rồi mới sửa.

| Rủi ro thiết kế | Dấu hiệu khi test | Hướng chỉnh sau này |
| --- | --- | --- |
| Thiếu đất quá sớm | Người chơi lên level nhưng không đủ vàng sang hệ thú | Tăng sớm thêm ô đất hoặc tăng nhẹ lợi nhuận cây trung |
| Thiếu cỏ / sâu kéo dài | Người chơi có tiền mua thú nhưng không đủ thức ăn | Tăng drop, giảm nhịp đói hoặc tăng vai trò shop cứu hộ |
| Kho đầy gây bực | Bị chặn thu hoạch liên tục ở tần suất khó chịu | Tăng slot đầu game hoặc tăng tốc dùng / bán item |
| Heo mở quá sớm / quá muộn | Ngày 6-7 mở quá nhiều hoặc quá ít | Điều chỉnh XP, giá mở chuồng, giá con giống |

# 9. Các giai đoạn mở rộng sau phase đầu

- Bổ sung hệ sản phẩm vật nuôi như trứng, sữa, thịt khi core nuôi để bán đã ổn định.

- Mở rộng mini game / event thành hệ nội dung lớn hơn với độ khó cao hơn và phần thưởng hiếm hơn.

- Bổ sung các lớp tiêu thụ sâu hơn như chế biến, nhiệm vụ hoặc đơn hàng khi kinh tế nền đã chắc.

# 10. Kết luận dùng cho thuyết trình nội bộ

Project này đang được định hình thành một game nông trại casual nơi người chơi bắt đầu bằng trồng trọt, rồi dần mở sang chăn nuôi như một lớp đầu tư dài ngày. Tuần đầu đủ để hình thành một farm có vòng quay vốn, kho có ý nghĩa quản lý và ít nhất một loại thú đầu tiên; mini game / event vừa tăng hứng thú vừa đóng vai trò comeback khi người chơi gặp khó khăn.
