# Farm Game Document Bundle - All in One

Tài liệu gộp toàn bộ nội dung từ các file DOCX hiện có của project dưới dạng Markdown.

---

# farm_game_data_foundation_v1

_Source:_ `farm_game_data_foundation_v1.docx`

NÔNG TRẠI CASUAL - DATA FOUNDATION V1

Bộ dữ liệu khởi tạo cho hạt giống, vật nuôi, trạng thái hệ thống và các rule phase đầu

| Tài liệu nội bộ - dùng làm mốc dữ liệu khởi tạo cho prototype, review balance và đối chiếu thiết kế ở các vòng phát triển sau. |
| --- |

# 1. Mục đích tài liệu

Tài liệu này tổng hợp các dữ liệu và rule quan trọng nhất cho phase đầu của project. Mục tiêu là tạo một mốc tham chiếu ổn định để nhóm thiết kế, cân bằng gameplay và prototype Unity cùng bám vào một bộ giả định thống nhất.

- Dùng làm dữ liệu khởi tạo cho prototype và các vòng test nội bộ.

- Giúp xem lại nhanh các quyết định đã chốt về hạt giống, vật nuôi, kho, mở khóa và state flow.

- Cho phép cân bằng lại sau này mà vẫn biết xuất phát điểm ban đầu.

# 2. Các giả định nền của phase đầu

- Core chính của game là trồng trọt + chăm sóc cây + chăn nuôi.

- Cỏ, sâu và thiếu nước xuất hiện ngẫu nhiên theo setup; khi có vấn đề cây sẽ hiện trạng thái 'cần chăm sóc'.

- Thanh sức khỏe cây (0-100) là biến trung tâm để tính sản lượng và mức rủi ro.

- Thu hoạch đúng khung thời gian đẹp sẽ được bonus; để quá lâu sau khi chín sẽ giảm sản lượng rồi chết.

- Vật nuôi phase đầu chỉ nuôi để bán thẳng con vật; chưa có hệ sản phẩm trứng, sữa, thịt.

- Kho là kho chung, có bộ lọc; khi kho đầy người chơi bị chặn thu hoạch cho tới khi bán hoặc dùng bớt vật phẩm.

- Mini game / event là lớp phụ, đồng thời là cơ chế cứu hộ khi người chơi thiếu vốn hoặc thiếu hạt giống để tái khởi động vòng chơi.

# 3. Data hạt giống khởi tạo

Nguyên tắc cân bằng: Bảng dưới đây là bộ 8 loại hạt giống dùng cho phase đầu. Chúng được sắp theo nhịp mở khóa tăng dần để vừa dạy người chơi core loop, vừa đảm bảo có đủ mục tiêu trong 7 ngày đầu.

| ID | Hạt giống | Lv | Giá hạt | TG lớn | SL cơ bản | Giá bán / đơn vị | Doanh thu chuẩn | Lợi nhuận chuẩn | XP | Khung thu đẹp | Tuổi thọ sau chín |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| crop_01 | Cà rốt | 1 | 8 | 2 phút | 3 | 4 | 12 | 4 | 4 | 1 phút | 3 phút |
| crop_02 | Khoai tây | 1 | 12 | 4 phút | 3 | 5 | 15 | 3 | 5 | 1 phút | 4 phút |
| crop_03 | Ngô | 2 | 18 | 5 phút | 4 | 6 | 24 | 6 | 7 | 2 phút | 5 phút |
| crop_04 | Cải xanh | 3 | 25 | 8 phút | 5 | 7 | 35 | 10 | 9 | 2 phút | 6 phút |
| crop_05 | Bí đỏ | 5 | 40 | 15 phút | 4 | 14 | 56 | 16 | 14 | 3 phút | 8 phút |
| crop_06 | Dưa hấu | 7 | 65 | 25 phút | 3 | 28 | 84 | 19 | 20 | 4 phút | 10 phút |
| crop_07 | Hoa hướng dương | 9 | 90 | 40 phút | 3 | 40 | 120 | 30 | 28 | 5 phút | 12 phút |
| crop_08 | Dâu vàng hiếm | 11 | 140 | 60 phút | 2 | 90 | 180 | 40 | 40 | 6 phút | 15 phút |

# 4. Rule chăm sóc cây và các yếu tố ảnh hưởng

Trong phase đầu, mỗi loại cây chỉ cần hiện một cảnh báo tổng quát là 'cần chăm sóc'. Bên trong cảnh báo này có thể chứa một hoặc nhiều vấn đề gồm cỏ, sâu và thiếu nước.

| Nhóm cây | Tỉ lệ cỏ | Tỉ lệ sâu | Tỉ lệ thiếu nước | Số sự kiện tối đa |
| --- | --- | --- | --- | --- |
| Ngắn (2-5 phút) | 10-14% | 8-12% | 10-14% | 1 |
| Trung (8-15 phút) | 16-18% | 14-16% | 16-18% | 2 |
| Dài (25-60 phút) | 22-28% | 18-24% | 22-28% | 3 |

- Cỏ: trừ HP theo thời gian; khi cắt xong nhận cỏ làm thức ăn cho thú.

- Sâu: trừ HP nhanh hơn cỏ; khi bắt xong nhận sâu làm thức ăn cho thú.

- Thiếu nước: trừ HP vừa phải; tưới nước để xóa trạng thái, không tạo item.

# 5. Thanh sức khỏe cây và cách quy đổi sang sản lượng

Giai đoạn đầu không cần công thức quá phức tạp. Sản lượng nên phụ thuộc chủ yếu vào HP tại thời điểm thu hoạch, thời điểm thu có còn trong khung đẹp hay không, và một mức random nhẹ để tạo cảm giác tự nhiên.

| HP khi thu | Health factor | Ý nghĩa gameplay |
| --- | --- | --- |
| 90-100 | 1.00 | Gần như tối đa; hợp với thu đúng lúc và chăm đều |
| 70-89 | 0.90 | Tốt; vẫn cho lợi nhuận ổn |
| 50-69 | 0.78 | Trung bình; lời thấp hơn rõ |
| 30-49 | 0.65 | Yếu; chỉ nên xem là hòa vốn / lời thấp |
| 1-29 | 0.50 | Còn sống nhưng chất lượng kém |
| 0 | - | Cây chết, mất trắng |

| Thời điểm thu | Time factor | Ghi chú |
| --- | --- | --- |
| Trong khung thu đẹp | 1.10 | Có thưởng thêm sản lượng |
| Vừa qua khung đẹp | 1.00 | Thu bình thường |
| Trễ mức 1 | 0.92 | Nửa đầu thời gian sau chín |
| Trễ mức 2 | 0.80 | Nửa sau thời gian sau chín |
| Quá tuổi thọ | - | Cây chết, không thu được |

Gợi ý an toàn: Safety floor được đề xuất cho prototype: nếu cây còn sống và người chơi vẫn thu hoạch được, tổng giá trị bán cuối cùng không nên thấp hơn khoảng 85% giá hạt. Mục tiêu là tránh cảm giác lỗ quá nặng ở phase đầu, trừ khi cây đã chết.

# 6. Các trạng thái của cây

State flow của cây cần đơn giản, dễ nhìn và có thể mapping trực tiếp sang art / animation ở prototype.

| State | Mô tả | Điều kiện vào | Điều kiện thoát / chuyển state |
| --- | --- | --- | --- |
| Empty | Ô đất trống | Mới mở hoặc đã dọn xong vòng trước | Người chơi gieo hạt |
| Seeded | Đã gieo, chuẩn bị bước vào tăng trưởng | Chọn hạt giống và xác nhận gieo | Bắt đầu đếm thời gian Phase 1 |
| Growing Phase 1 | Giai đoạn đầu, cây mới mọc | Hết Seeded | Sang Phase 2 hoặc phát sinh Needs Care |
| Growing Phase 2 | Giai đoạn giữa, có thể phát sinh thêm sự cố | Hoàn tất Phase 1 | Sang Phase 3 hoặc phát sinh Needs Care |
| Growing Phase 3 | Giai đoạn cuối trước khi chín | Hoàn tất Phase 2 | Sang Ripe hoặc Needs Care |
| Needs Care | Có cỏ, sâu hoặc thiếu nước | Ít nhất 1 sự cố xuất hiện | Xử lý xong hết sự cố để quay lại phase trước đó |
| Ripe / Perfect Window | Đã chín và đang trong khung thu đẹp | Hoàn tất tăng trưởng | Thu hoạch hoặc chuyển Late Harvest |
| Late Harvest | Đã quá khung đẹp, sản lượng giảm dần | Hết perfect window | Thu hoạch hoặc Dead |
| Dead | Cây chết | HP = 0 hoặc quá tuổi thọ sau chín | Dọn ô đất để trồng lại |

# 7. Data vật nuôi khởi tạo

Định hướng: Vật nuôi được đẩy sang nhịp dài ngày để trồng trọt thật sự là nguồn vốn chính ở đầu game. Người chơi chỉ bắt đầu chăn nuôi sau khi đủ level, đủ vàng và đã mở chuồng tương ứng.

| ID | Vật nuôi | Lv | Chuồng | Giá mở chuồng | Giá con giống | Thức ăn | Nhịp đói | GĐ1 | Bán GĐ1 | GĐ2 | Bán GĐ2 | GĐ3 | Bán GĐ3 | Sống thêm sau trưởng thành |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| animal_01 | Gà | 4 | Chuồng Gà | 180 | 220 | Sâu | 24h | 1 ngày | 180 | 2 ngày | 280 | 3 ngày | 420 | 1 ngày |
| animal_02 | Vịt | 7 | Chuồng Vịt | 280 | 320 | Sâu + cỏ | 24h | 1 ngày | 250 | 3 ngày | 420 | 4 ngày | 620 | 1 ngày |
| animal_03 | Heo | 10 | Chuồng Heo | 650 | 700 | Cỏ | 36h | 2 ngày | 560 | 4 ngày | 920 | 6 ngày | 1380 | 2 ngày |
| animal_04 | Bò | 13 | Chuồng Bò | 1100 | 1200 | Cỏ | 48h | 3 ngày | 950 | 5 ngày | 1550 | 7 ngày | 2300 | 2 ngày |

- Gà là thú mở đầu; phù hợp để dạy người chơi cách giữ sâu lại để nuôi thú.

- Vịt tạo liên kết rõ hơn giữa cỏ và sâu vì dùng thức ăn hỗn hợp.

- Heo và Bò là đầu tư lớn hơn, phù hợp vai trò mục tiêu tuần 2 thay vì cho chạm quá sớm.

# 8. Shop thức ăn cơ bản

Shop thức ăn là lớp cứu hộ, không phải nguồn tối ưu. Nguồn tốt nhất vẫn là cắt cỏ và bắt sâu trong loop chăm cây.

| Item shop | Gói | Giá bán | Giá / đơn vị | Vai trò |
| --- | --- | --- | --- | --- |
| Grass Bundle | 5 cỏ | 30 | 6 | Cứu hộ cho vịt / heo / bò khi drop cỏ không đủ |
| Worm Bundle | 5 sâu | 40 | 8 | Cứu hộ cho gà / vịt khi thiếu sâu |

# 9. Các trạng thái của vật nuôi

Phase đầu chưa cần thanh sức khỏe riêng cho vật nuôi. Rule đơn giản là: đói thì ngừng lớn; quá tuổi thọ sau trưởng thành mà không bán thì chết.

| State | Mô tả | Điều kiện vào | Điều kiện thoát / chuyển state |
| --- | --- | --- | --- |
| Empty Pen | Chuồng trống | Mới xây hoặc vừa bán xong | Mua con giống |
| Baby | Mới mua, bắt đầu vòng đời | Đưa con giống vào chuồng | Tới Growing hoặc Hungry |
| Growing | Đang lớn lên theo timer | Đã cho ăn đúng và timer đang chạy | Tới Hungry hoặc Stage Up Ready |
| Hungry | Đến mốc cần cho ăn | Hết khoảng Hunger Interval | Cho ăn đúng để quay lại Growing |
| Stage Up Ready | Vừa chạm mốc giai đoạn | Đủ thời gian của giai đoạn | Tiếp tục nuôi hoặc bán ngay |
| Mature / Sellable | Đã đạt giá trị cao nhất của phase đầu | Đủ GĐ3 | Bán hoặc chờ quá hạn để chết |
| Dead | Vật nuôi chết do quá tuổi thọ sau trưởng thành | Hết Lifetime After Mature | Dọn chuồng / mua con mới |

# 10. Hệ kho và quy tắc chứa đồ

Kho là hệ áp lực quản lý tài nguyên, không chỉ là nơi cất đồ. Khi kho đầy, game chủ động chặn người chơi nhận thêm sản lượng để buộc họ bán, dùng hoặc dọn bớt.

| Nhóm dữ liệu | Quy tắc phase đầu |
| --- | --- |
| Loại kho | Một kho chung, dùng bộ lọc để xem nhanh từng nhóm item |
| Chứa | Hạt giống, nông sản, cỏ, sâu, item mini game / event |
| Không chứa | Vật nuôi; vật nuôi tồn tại trực tiếp trong chuồng |
| Sức chứa ban đầu | 50 slots |
| Stack | 1 loại item = 1 stack; tối đa 99 / stack; 1 stack chiếm 1 slot |
| Khi kho đầy | Không cho thu hoạch cây, không cho nhận cỏ / sâu / phần thưởng mới |
| Thông báo | Kho đã đầy. Hãy bán hoặc sử dụng bớt vật phẩm. |
| Nâng cấp kho | Lv8: 70 slots (150 vàng); Lv12: 95 slots (350 vàng); Lv16: 125 slots (700 vàng) |

# 11. Mở khóa level, mở rộng ruộng và các mốc quan trọng

Các mốc dưới đây là bản chuẩn v1 để triển khai prototype. Sau khi test thực tế có thể điều chỉnh lại nhịp mở ô đất, kho hoặc vật nuôi.

| Level | Mở khóa / thay đổi hệ thống |
| --- | --- |
| 1 | 6 ô đất ban đầu, Cà rốt, Khoai tây |
| 2 | Ngô |
| 3 | Mở thêm 2 ô đất (tổng 8), Cải xanh |
| 4 | Chuồng Gà, mở shop thức ăn cơ bản |
| 5 | Mở thêm 2 ô đất (tổng 10), Bí đỏ |
| 7 | Chuồng Vịt, Dưa hấu |
| 8 | Nâng cấp kho lần 1, mở thêm 4 ô đất (tổng 14) |
| 9 | Hoa hướng dương |
| 10 | Chuồng Heo |
| 11 | Dâu vàng hiếm |
| 12 | Mở thêm 4 ô đất (tổng 18) |
| 13 | Chuồng Bò |

# 12. Item nền của phase đầu

Bộ item phase đầu nên giữ gọn. Mỗi item đều có nguồn sinh ra rõ ràng và có vai trò ngay trong vòng chơi, tránh tạo ra nhiều item trang trí dữ liệu mà chưa có gameplay đi kèm.

| Item | Nguồn sinh ra | Công dụng phase đầu | Ghi chú |
| --- | --- | --- | --- |
| Cỏ | Cắt cỏ trên ruộng, mini game / event | Cho vịt / heo / bò ăn | 20% cơ hội nhận x2 khi cắt cỏ |
| Sâu | Bắt sâu trên ruộng, mini game / event | Cho gà / vịt ăn | 15% cơ hội nhận x2 khi bắt sâu |
| Hạt giống thường | Shop, mini game | Trồng trọt và tái đầu tư | Mini game đóng vai trò cứu hộ khi cạn vốn |
| Hạt giống hiếm | Mở khóa level cao hơn, mini game / event | Phần thưởng có giá trị cao, tăng cảm giác tiến triển | Chưa phải trọng tâm ở tuần đầu |
| Speed-up 5 phút | Mini game / event | Hỗ trợ comeback / đẩy nhanh tiến trình | Phần thưởng phụ, không phá kinh tế |

# 13. Ghi chú triển khai prototype

- CropData, AnimalData, ItemData, LevelData nên tách riêng để dễ cân bằng.

- Cần log lại: thời điểm người chơi mua gà đầu tiên, tần suất kho đầy, tỷ lệ dùng shop thức ăn, và ngày người chơi chạm Heo.

- Mini game / event nên luôn có một nhánh phần thưởng có thể giúp người chơi dựng lại vòng chơi khi cạn vốn.

- Bản v1 này là mốc tham chiếu; sau mỗi đợt test nên cập nhật v2 / v3 thay vì ghi đè không dấu vết.

---

# farm_game_data_foundation_v2

_Source:_ `farm_game_data_foundation_v2.docx`

Farm Game Data Foundation v2

Tài liệu dữ liệu nền tảng cho prototype và cân bằng phase đầu

| Ngôn ngữ: Tiếng Việt |
| --- |
| Phạm vi: Phase đầu - core trồng trọt, chăn nuôi, kho, event cứu hộ và interaction theo từng object |
| World direction: 2.5D isometric giả 3D bằng asset 2D • UI 2D hoàn toàn |

| Mục đích tài liệu<br>Khóa lại các quyết định thiết kế đã được thống nhất để tránh lệch hướng giữa design, balance và prototype.<br>Làm nguồn dữ liệu nền tảng để dev, game designer và người review dự án dùng xuyên suốt phase đầu.<br>Tạo nền để sau này chỉnh balance mà vẫn giữ logic tổng thể của phase đầu. |
| --- |

# 1. Snapshot các quyết định đã khóa

- Core phase đầu gồm 2 hệ chính: trồng trọt và chăn nuôi; mini game/event là hệ phụ có vai trò tạo hứng thú và cứu hộ progression.

- Cây trồng có thanh sức khỏe 0-100; sức khỏe bị ảnh hưởng bởi cỏ, sâu và thiếu nước.

- Khi cây chín sẽ có khung thu hoạch đẹp; sau đó sản lượng giảm dần; quá tuổi thọ thì chết.

- Người chơi tương tác trực tiếp trên từng object: ô đất, cây trồng, vật nuôi; panel hành động hiển thị theo đúng trạng thái của object đó.

- Kho là kho chung có bộ lọc; kho đầy thì chặn thu hoạch/nhận vật phẩm cho tới khi người chơi bán hoặc dọn chỗ.

- Vật nuôi phase đầu nuôi để bán trực tiếp con vật; chưa mở hệ sản phẩm như trứng, sữa, thịt.

- Vật nuôi mở bằng level + mở chuồng; không mua ngay từ đầu.

- Thức ăn cơ bản có bán trong shop để chống bí; đây là safety net chứ không phải nguồn tối ưu.

- Cỏ và sâu là nguyên liệu phụ do hành động chăm cây sinh ra; không có cây 'Cỏ non thức ăn'.

- Toàn bộ không gian nông trại đi theo hướng 2.5D isometric giả 3D bằng asset 2D; UI giữ 2D hoàn toàn.

# 2. Mô hình hệ thống phase đầu

- Trồng trọt = nguồn quay vòng vốn chính, đồng thời sinh ra nguyên liệu phụ cho chăn nuôi.

- Chăn nuôi = lớp đầu tư trung hạn/dài hạn mở sau khi người chơi đã có nền kinh tế từ trồng trọt.

- Mini game / event = lớp cứu hộ và quà phụ cho progression; ở phase đầu có thể trả ra hạt giống, vàng nhỏ, cỏ, sâu hoặc item hỗ trợ.

# 3. Dữ liệu cây trồng (CropData)

Quy ước: Base Gross = doanh thu chuẩn khi cây có sức khỏe tốt và thu hoạch đúng thời điểm. Base Profit = lợi nhuận chuẩn trước khi áp dụng biến động do sức khỏe, thời điểm thu và random nhẹ.

| ID | Cây | Lv | Giá hạt | Thời gian lớn | Base Yield | Giá bán/đv | Base Gross | Base Profit | XP | Cỏ | Sâu | Thiếu nước | Sự cố tối đa | Perfect Window | Post-Ripe Life |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| crop_01 | Cà rốt | 1 | 8 | 2 phút | 3 | 4 | 12 | 4 | 4 | 10% | 8% | 10% | 1 | 1 phút | 3 phút |
| crop_02 | Khoai tây | 1 | 12 | 4 phút | 3 | 5 | 15 | 3 | 5 | 12% | 10% | 12% | 1 | 1 phút | 4 phút |
| crop_03 | Ngô | 2 | 18 | 5 phút | 4 | 6 | 24 | 6 | 7 | 14% | 12% | 14% | 2 | 2 phút | 5 phút |
| crop_04 | Cải xanh | 3 | 25 | 8 phút | 5 | 7 | 35 | 10 | 9 | 16% | 14% | 16% | 2 | 2 phút | 6 phút |
| crop_05 | Bí đỏ | 5 | 40 | 15 phút | 4 | 14 | 56 | 16 | 14 | 18% | 16% | 18% | 2 | 3 phút | 8 phút |
| crop_06 | Dưa hấu | 7 | 65 | 25 phút | 3 | 28 | 84 | 19 | 20 | 22% | 18% | 22% | 3 | 4 phút | 10 phút |
| crop_07 | Hoa hướng dương | 9 | 90 | 40 phút | 3 | 40 | 120 | 30 | 28 | 24% | 20% | 24% | 3 | 5 phút | 12 phút |
| crop_08 | Dâu vàng hiếm | 11 | 140 | 60 phút | 2 | 90 | 180 | 40 | 40 | 28% | 24% | 28% | 3 | 6 phút | 15 phút |

| Lưu ý balance cho cây trồng<br>Cây rẻ không nên vừa lời cao vừa ít rủi ro; nhiệm vụ của nhóm cây đầu là giúp người chơi học loop và quay vòng vốn.<br>Cây đắt nên có sản lượng ít hơn nhưng giá trị trên mỗi đơn vị cao hơn.<br>Phase đầu ưu tiên cảm giác 'còn sống là gần như không lỗ nặng'; thất bại lớn chỉ xuất hiện khi cây chết.<br>Không có cây 'Cỏ non thức ăn'; cỏ là tài nguyên phụ do cắt cỏ mọc trên ruộng. |
| --- |

# 4. Quy tắc sinh trưởng và sản lượng cây

1. Tất cả cây đi theo 3 pha tăng trưởng + 1 trạng thái chín để art, code và UI đồng nhất hơn trong prototype.

1. Tỉ lệ chia pha gợi ý: Phase 1 = 25% tổng thời gian, Phase 2 = 35%, Phase 3 = 40%.

1. Khi cây có cỏ, sâu hoặc thiếu nước, cây hiện trạng thái Needs Care; người chơi chạm vào cây để mở panel chăm sóc theo ngữ cảnh.

1. Khi cây vào trạng thái chín, người chơi có một khoảng thời gian harvest đẹp; sau đó sản lượng giảm nhẹ dần theo tuổi thọ sau khi chín.

## 4.1 Hệ số theo HP khi thu hoạch

| HP lúc thu | HealthFactor | Diễn giải |
| --- | --- | --- |
| 90-100 | 1.00 | Cây rất khỏe, sản lượng tối đa theo data. |
| 70-89 | 0.90 | Chăm sóc tốt, mất ít sản lượng. |
| 50-69 | 0.78 | Cây đã bị ảnh hưởng nhưng vẫn còn hiệu quả. |
| 30-49 | 0.65 | Cây yếu rõ rệt, sản lượng giảm mạnh. |
| 1-29 | 0.50 | Cây gần chết, chỉ thu mức thấp. |
| 0 | Dead | Mất trắng. |

## 4.2 Hệ số theo thời điểm thu

| Thời điểm thu | TimeFactor | Diễn giải |
| --- | --- | --- |
| Trong perfect window | 1.10 | Được thưởng vì quay lại đúng nhịp. |
| Sau perfect window, chưa già | 1.00 | Thu bình thường, không thưởng thêm. |
| Trễ mức 1 | 0.92 | Nửa đầu của thời gian sống sau khi chín. |
| Trễ mức 2 | 0.80 | Nửa sau của thời gian sống sau khi chín. |
| Quá tuổi thọ | Dead | Cây chết, mất trắng. |

## 4.3 HP drain và hành động chăm sóc

| Tác nhân | HP drain gợi ý | Hành động xử lý | Kết quả |
| --- | --- | --- | --- |
| Cỏ | -2 HP / 10 giây | Cắt cỏ | Xóa trạng thái cỏ, nhận Grass x1 (20% cơ hội x2). |
| Sâu | -3 HP / 10 giây | Bắt sâu | Xóa trạng thái sâu, nhận Worm x1 (15% cơ hội x2). |
| Thiếu nước | -2 HP / 10 giây | Tưới nước | Xóa trạng thái thiếu nước, không cho item. |

| Safety floor được khuyến nghị cho prototype<br>Nếu cây còn sống và người chơi thu hoạch được, tổng giá trị bán cuối cùng nên không thấp hơn khoảng 85% giá hạt.<br>Rule này giúp game có phạt nhưng không quá cay nghiệt ở phase đầu. |
| --- |

# 5. Dữ liệu vật nuôi (AnimalData)

| ID | Vật nuôi | Lv | Chuồng | Giá mở chuồng | Giá con giống | Thức ăn | Nhịp đói | Mỗi bữa | Đến GĐ1 | Bán GĐ1 | Đến GĐ2 | Bán GĐ2 | Đến GĐ3 | Bán GĐ3 | Tuổi thọ sau trưởng thành | XP bán GĐ1/2/3 |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| animal_01 | Gà | 4 | Chuồng Gà | 180 | 220 | Worm | 24h | 1 | 1 ngày | 180 | 2 ngày | 280 | 3 ngày | 420 | 1 ngày | 18 / 28 / 42 |
| animal_02 | Vịt | 7 | Chuồng Vịt | 280 | 320 | Worm + Grass | 24h | 1 + 1 | 1 ngày | 250 | 3 ngày | 420 | 4 ngày | 620 | 1 ngày | 25 / 40 / 62 |
| animal_03 | Heo | 10 | Chuồng Heo | 650 | 700 | Grass | 36h | 2 | 2 ngày | 560 | 4 ngày | 920 | 6 ngày | 1380 | 2 ngày | 56 / 92 / 138 |
| animal_04 | Bò | 13 | Chuồng Bò | 1100 | 1200 | Grass | 48h | 3 | 3 ngày | 950 | 5 ngày | 1550 | 7 ngày | 2300 | 2 ngày | 95 / 155 / 230 |

| Lưu ý balance cho chăn nuôi<br>Vật nuôi phase đầu là lớp đầu tư trung hạn/dài hạn; người chơi không mua ngay từ đầu mà mở dần theo level + chuồng.<br>Khi vật nuôi đói, tăng trưởng dừng lại cho tới khi được cho ăn đúng loại; phase đầu chưa cần thanh sức khỏe riêng cho vật nuôi.<br>Vật nuôi chỉ chết khi quá tuổi thọ sau trưởng thành mà người chơi vẫn không bán. |
| --- |

# 6. Item nền và thức ăn shop

| Item | Nhóm | Nguồn chính | Giá mua | Ghi chú |
| --- | --- | --- | --- | --- |
| Grass | Nguyên liệu nuôi | Cắt cỏ trên ruộng; thưởng event | Bundle 5 = 30 vàng | Nguồn tối ưu là từ chăm cây; shop chỉ chống bí. |
| Worm | Nguyên liệu nuôi | Bắt sâu trên cây; thưởng event | Bundle 5 = 40 vàng | Nguồn tối ưu là từ chăm cây; shop chỉ chống bí. |
| Hạt giống thường | Hạt giống | Shop, event cứu hộ | Tùy từng loại cây | Event có thể trả ra hạt giống để người chơi comeback. |
| Hạt giống hiếm | Hạt giống | Event / unlock cao cấp | Tùy setup sau | Để mở rộng về phase sau. |

# 7. Level unlock và mở rộng ruộng

| Level | XP tích lũy | Mở khóa chính |
| --- | --- | --- |
| 1 | 0 | Cà rốt, Khoai tây, 6 ô đất đầu. |
| 2 | 40 | Ngô. |
| 3 | 100 | Cải xanh, thêm 2 ô đất. |
| 4 | 180 | Chuồng Gà, shop thức ăn cơ bản. |
| 5 | 280 | Bí đỏ, thêm 2 ô đất. |
| 7 | 550 | Chuồng Vịt, Dưa hấu. |
| 8 | 730 | Nâng cấp kho lần 1, thêm 4 ô đất. |
| 9 | 940 | Hoa hướng dương. |
| 10 | 1180 | Chuồng Heo. |
| 11 | 1450 | Dâu vàng hiếm. |
| 12 | 1760 | Thêm 4 ô đất. |
| 13 | 2110 | Chuồng Bò. |

| Mốc mở rộng ruộng | Thêm ô đất | Tổng ô đất | Ghi chú |
| --- | --- | --- | --- |
| Level 1 | 6 ô ban đầu | 6 | Đủ cho tutorial và nhịp trồng cơ bản. |
| Level 3 | +2 | 8 | Người chơi bắt đầu cảm nhận farm đang lớn dần. |
| Level 5 | +2 | 10 | Hỗ trợ tích vốn trước khi vào chăn nuôi. |
| Level 8 | +4 | 14 | Bắt đầu mở rộng rõ rệt. |
| Level 12 | +4 | 18 | Chuẩn bị cho phase có nhiều cây trung/dài hạn hơn. |

# 8. State model - Cây trồng

| State | Tên hiển thị | Điều kiện vào | Hành động chính của user |
| --- | --- | --- | --- |
| Empty | Ô đất trống | Chưa gieo hoặc vừa thu xong | Chọn hạt giống để gieo. |
| Seeded / Growing | Đang lớn | Đã gieo và đang chạy timer | Xem thông tin; nếu có sự cố thì xử lý. |
| Needs Care | Cần chăm sóc | Có cỏ, sâu hoặc thiếu nước | Mở panel và xử lý theo ngữ cảnh. |
| Ripe | Đã chín | Hết timer tăng trưởng | Thu hoạch. |
| Perfect Harvest Window | Khung thu đẹp | Ngay sau khi chín | Thu hoạch để nhận bonus. |
| Late Harvest | Trễ thu hoạch | Sau khung đẹp nhưng chưa chết | Thu hoạch với hệ số giảm. |
| Dead | Đã chết | HP về 0 hoặc quá tuổi thọ sau chín | Dọn trạng thái / reset ô; mất trắng. |

# 9. State model - Vật nuôi

| State | Tên hiển thị | Điều kiện vào | Hành động chính của user |
| --- | --- | --- | --- |
| Locked Pen | Chuồng bị khóa | Chưa đủ level hoặc chưa mở chuồng | Xem yêu cầu; mở chuồng khi đủ điều kiện. |
| Empty Pen | Chuồng trống | Đã mở chuồng nhưng chưa có vật nuôi | Mua con giống. |
| Growing | Đang lớn | Đã có con giống và chưa đến giai đoạn mới | Xem timer; chuẩn bị thức ăn. |
| Hungry | Đói | Đến mốc đói | Cho ăn đúng loại. |
| Stage Up Ready | Qua giai đoạn mới | Vừa đạt mốc trưởng thành kế tiếp | Xem giá bán mới; quyết định giữ hay bán. |
| Mature / Sellable | Đã trưởng thành / bán được | Đến giai đoạn cuối hoặc đang ở giai đoạn có thể bán | Bán ngay hoặc chờ thêm trong thời gian an toàn. |
| Dead | Đã chết | Quá tuổi thọ sau trưởng thành | Mất khoản đầu tư của con vật đó. |

# 10. State model - Kho

| State | Tên hiển thị | Điều kiện | Ứng xử hệ thống |
| --- | --- | --- | --- |
| Available | Còn chỗ | Số slot đã dùng thấp hơn sức chứa | Cho nhận item, thu hoạch và nhận quà bình thường. |
| Near Full | Gần đầy | Kho gần chạm ngưỡng tối đa | Hiện cảnh báo nhẹ để người chơi bán/dọn sớm. |
| Full | Đã đầy | Không còn slot trống | Chặn thu hoạch, nhận item event, nhận cỏ/sâu; mở popup hướng tới kho/bán. |
| Filtered View | Đang lọc | Người chơi chọn tab lọc trong kho | Chỉ đổi cách hiển thị, không ảnh hưởng logic chứa. |

| Rule kho được khóa cho phase đầu<br>Kho là kho chung cho hạt giống, nông sản, cỏ, sâu và vật phẩm event.<br>Vật nuôi không đi vào kho; vật nuôi tồn tại trực tiếp trong khu chuồng.<br>1 loại item = 1 stack, tối đa 99, mỗi stack chiếm 1 slot.<br>Dung lượng khởi đầu gợi ý: 50 slot; có thể nâng cấp dần ở các level sau. |
| --- |

# 11. Interaction rule phase đầu

- User action trực tiếp trên từng object: ô đất, cây trồng, vật nuôi.

- Khi chạm vào object, game hiển thị panel hành động theo đúng trạng thái hiện tại của object đó.

- Phase đầu chưa dùng quick tool hay thao tác hàng loạt; các công cụ này chỉ là hướng mở rộng cho phase sau khi nội dung tăng nhiều.

# 12. Art & scene direction khóa cho prototype

- UI: 2D hoàn toàn, bo tròn, tươi sáng, dễ đọc trên mobile.

- World: 2.5D isometric giả 3D bằng asset 2D, không dùng 3D thật.

- Áp dụng cho toàn bộ nền farm, ô đất, cây trồng, vật nuôi, công trình và vật trang trí.

- Mục tiêu là giữ production nhẹ, dễ mở rộng content và đồng bộ style cho game farm casual.

# 13. Mini game / event trong phase đầu

- Mini game không thay core farm; nó là hệ phụ tăng hứng thú và đóng vai trò cứu hộ progression.

- Khi người chơi chơi lỗi, thiếu vàng hoặc thiếu hạt giống, mini game / event có thể trả ra hạt giống cơ bản, vàng nhỏ, cỏ, sâu hoặc item hỗ trợ để họ xây lại vòng chơi.

- Ở phase sau, mini game và event sẽ được nâng tầm thành hệ nội dung khó hơn, phần thưởng hiếm hơn và gắn mạnh hơn với live-ops.

# 14. Dữ liệu cần tiếp tục theo dõi khi test prototype

- Ngày mà người chơi casual mua được gà đầu tiên.

- Tỉ lệ phải mua thức ăn từ shop so với tự tích từ ruộng.

- Tần suất kho đầy và mức độ khó chịu của kho đối với flow chơi.

- Tỉ lệ cây chết do bỏ quên và cảm giác người chơi về mức phạt.

- Tỉ lệ người chơi chạm mốc Heo ở ngày 6-7 của progression.

---

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

---

# farm_game_overview_7day_v2

_Source:_ `farm_game_overview_7day_v2.docx`

Farm Game Overview & 7-Day Progression v2

Tài liệu tổng quan sản phẩm, core loop, nhịp 7 ngày đầu và định hướng mở rộng

| Mục tiêu: dùng cho thuyết trình, review nội bộ và làm nền cho thiết kế/production |
| --- |
| Bản này đã cập nhật đầy đủ các chốt mới về interaction, mini game, art direction và hệ chăn nuôi dài ngày |

| Tóm tắt một câu<br>Đây là game nông trại casual, lấy trồng trọt làm nền kinh tế chính và chăn nuôi làm lớp đầu tư mở sau; mini game/event đóng vai trò tăng hứng thú và cứu hộ progression khi người chơi gặp khó khăn. |
| --- |

# 1. Tầm nhìn sản phẩm

Game hướng tới cảm giác của một nông trại vui vẻ, tươi sáng, dễ vào, dễ hiểu và dễ quay lại mỗi ngày. Người chơi không chỉ trồng rồi chờ, mà phải thật sự chăm cây, quản lý kho, tích nguyên liệu phụ và nâng cấp từ trồng trọt sang chăn nuôi.

- Tinh thần sản phẩm: vui vẻ, hoài niệm nhẹ, sinh động, casual mobile.

- Core phase đầu: trồng trọt và chăn nuôi; mini game/event là lớp cứu hộ và quà phụ.

- Gameplay yêu cầu người chơi tương tác thật, nhưng không được quá nặng để mất chất casual.

# 2. Trụ cột gameplay

## 2.1 Trồng trọt là nền kinh tế chính

- Mua hạt giống, gieo trồng, quan sát các giai đoạn phát triển.

- Cây có thể phát sinh cỏ, sâu và thiếu nước theo tỷ lệ setup.

- Người chơi xử lý bằng cách cắt cỏ, bắt sâu và tưới nước để giữ HP cho cây.

- Sức khỏe cây ảnh hưởng trực tiếp tới sản lượng khi thu hoạch.

- Thu đúng khung đẹp được bonus; để quá lâu sau khi chín sẽ giảm sản lượng rồi chết.

## 2.2 Chăn nuôi là lớp đầu tư mở sau

- Người chơi không mua vật nuôi ngay đầu game; phải đạt đủ level và mở chuồng tương ứng.

- Vật nuôi phase đầu nuôi để bán trực tiếp con vật; chưa có sản phẩm như trứng, sữa, thịt.

- Nhịp nuôi bắt đầu từ 1 ngày trở lên để chăn nuôi trở thành lớp đầu tư trung/dài hạn.

- Khi đói, vật nuôi dừng lớn cho tới khi được cho ăn đúng loại.

- Quá tuổi thọ sau trưởng thành mà không bán thì vật nuôi chết.

## 2.3 Mini game / event là lớp cứu hộ và giữ nhịp cảm xúc

- Mini game không thay core farm; nó hỗ trợ người chơi khi vòng chơi bị đứt vì thiếu vàng hoặc thiếu hạt giống.

- Phase đầu, reward ưu tiên là hạt giống cơ bản, vàng nhỏ, cỏ, sâu hoặc item hỗ trợ.

- Về sau, mini game/event sẽ được nâng tầm thành hệ nội dung lớn hơn với phần thưởng hiếm hơn và độ khó cao hơn.

# 3. Cách chơi và cảm giác người chơi nên nhận được

- Trồng trọt không phải chỉ đặt timer rồi đợi; người chơi phải quay lại để chăm sóc.

- Nếu chăm tốt, người chơi vừa có sản lượng tốt vừa có cỏ/sâu làm thức ăn cho vật nuôi.

- Nếu chơi lỗi, người chơi vẫn có đường comeback nhờ shop cơ bản và mini game/event.

- Game phải tạo cảm giác luôn có việc để làm, nhưng không biến thành game quản lý căng thẳng.

| Cảm giác mục tiêu<br>Đầu game: học và hiểu loop trồng trọt, thấy farm bắt đầu sống.<br>Giữa tuần đầu: thấy farm lớn lên, có đủ vốn để mở gà/vịt và bắt đầu bước sang hệ chăn nuôi.<br>Cuối tuần đầu: có nền kinh tế rõ ràng, có mục tiêu sang tuần 2 như heo, bò, hạt giống hiếm và nâng cấp kho/ruộng. |
| --- |

# 4. Core loop phase đầu

1. Vào farm và kiểm tra cây trồng đang lớn, cây nào cần chăm sóc, cây nào đã chín.

1. Xử lý cỏ, sâu, thiếu nước nếu có; giữ HP cây để bảo toàn sản lượng.

1. Thu hoạch cây đã chín và chuyển vật phẩm vào kho.

1. Bán nông sản để lấy vàng tái đầu tư vào hạt giống mới.

1. Tích dần cỏ và sâu - đây là nguyên liệu phụ cho chăn nuôi.

1. Khi đạt đủ level và đã mở chuồng, bắt đầu mua vật nuôi và cho ăn.

1. Nếu thiếu tài nguyên hoặc lỡ tay làm vòng chơi hụt, dùng mini game/event để có cơ hội comeback.

# 5. Hành vi người chơi trong 7 ngày đầu

Phần này mô tả hành trình điển hình của một người chơi casual đăng nhập nhiều lần ngắn trong ngày, không phải người chơi hardcore cày liên tục.

| Ngày | Trọng tâm | Level mục tiêu | Tình trạng kinh tế & tài nguyên | Cảm giác người chơi nên có |
| --- | --- | --- | --- | --- |
| Ngày 1 | Học trồng, chăm cây, bán nông sản | 1 → 2 / chạm 3 | Bắt đầu có vàng quay vòng; chưa dư nhiều; kho chưa quá áp lực | Hiểu game này không chỉ trồng rồi chờ mà phải chăm cây thật. |
| Ngày 2 | Ổn định vòng trồng trọt; bắt đầu hiểu giá trị của cỏ/sâu | 3 → 4 | Tích đủ hoặc gần đủ điều kiện để nghĩ đến Chuồng Gà | Cảm thấy mình sắp mở một lớp gameplay mới. |
| Ngày 3 | Mở Chuồng Gà và mua gà đầu tiên | 4 → 5 / 6 | Vàng có thể tụt sau đầu tư; vẫn sống chủ yếu bằng trồng trọt | Thấy rõ trồng trọt đang nuôi chăn nuôi. |
| Ngày 4 | Vận hành farm hai lớp: trồng + nuôi gà | 6 → 7 | Cân giữa mở rộng cây, nuôi thú và giữ tài nguyên nuôi | Farm bắt đầu có chiều sâu, không còn chỉ là ruộng. |
| Ngày 5 | Tối ưu quản lý kho, cây ngắn/dài và vòng thức ăn | 7 → 8 / 9 | Bắt đầu va chạm giới hạn kho và quyết định chiến lược | Hiểu mình cần quản lý chứ không chỉ bấm thu. |
| Ngày 6 | Heo trở thành mục tiêu lớn | 9 → 10 | Bắt đầu gom vốn cho chuồng lớn hơn; vịt có thể đã xuất hiện | Thấy game còn đường phát triển, chưa hết content. |
| Ngày 7 | Hoàn thành tuần đầu với nền kinh tế rõ | 10 → 11 / 12 | Có 1-2 hệ nuôi đầu tiên và đủ vốn để nhìn sang tuần 2 | Farm đã vận hành thật; tuần sau còn nhiều mục tiêu hơn. |

# 6. Ước tính vàng và XP trong 7 ngày đầu

Đây là ước tính định hướng, dùng để review nhịp progression chứ chưa phải cam kết cân bằng cuối. Số liệu giả định người chơi vào game 3-5 lần/ngày, chủ yếu trồng cây ngắn/trung, và không tối ưu hoàn hảo mọi thao tác.

| Ngày | Vàng khả dụng cuối ngày (ước tính) | XP / level ước tính | Diễn giải |
| --- | --- | --- | --- |
| Ngày 1 | 120-220 vàng | Level 2, có thể chạm 3 | Học loop, lời nhỏ từ cây đầu. |
| Ngày 2 | 280-420 vàng | Level 4 | Đủ hoặc gần đủ để mở Chuồng Gà + chuẩn bị mua gà. |
| Ngày 3 | 100-260 vàng sau đầu tư | Level 5-6 | Vàng giảm vì đã đầu tư vào gà; kinh tế vẫn do cây gánh chính. |
| Ngày 4 | 220-420 vàng | Level 7 | Bắt đầu có nền hai lớp; cân giữa mở rộng và nuôi. |
| Ngày 5 | 320-600 vàng | Level 8-9 | Kho bắt đầu có áp lực; người chơi hiểu rõ vòng bán. |
| Ngày 6 | 450-800 vàng | Level 10 | Bắt đầu đủ lực nghĩ tới Heo hoặc đầu tư tiếp vào vịt/ruộng. |
| Ngày 7 | 650-1100 vàng | Level 11-12 | Kết thúc tuần đầu với nền tài nguyên rõ ràng; có mục tiêu bước sang tuần 2. |

| Cách đọc bảng vàng / XP<br>Vàng khả dụng cuối ngày đã giả định người chơi có bán nông sản định kỳ và có thể đã chi một phần cho hạt giống, mở rộng hoặc chăn nuôi.<br>XP và level có thể dao động theo mức chăm cây tốt hay không, số lần người chơi online và tần suất dùng cây trung/dài hạn.<br>Mục tiêu quan trọng không phải đúng từng số, mà là giữ được nhịp: ngày 2-3 có gà, ngày 4-5 có vịt, ngày 6-7 chạm Heo hoặc chuẩn bị cho Heo. |
| --- |

# 7. Các hệ an toàn để tránh soft-lock

- Shop có bán thức ăn cơ bản giá rẻ để chống bí; không phải nguồn tối ưu bằng việc chăm cây tốt.

- Mini game/event có thể trả ra hạt giống cơ bản và tài nguyên để người chơi xây lại vòng chơi nếu thiếu vốn.

- Safety floor của cây giúp người chơi còn sống thì thường không lỗ quá nặng; mất trắng chỉ xảy ra khi cây chết.

# 8. Interaction rule đã khóa

- Phase đầu: user action trực tiếp trên từng object - ô đất, cây trồng, vật nuôi.

- Game hiển thị panel hành động theo đúng trạng thái hiện tại của object đó.

- Phase đầu chưa dùng quick tool hay thao tác hàng loạt; các tiện ích tăng tốc chỉ mở rộng về sau sau khi gameplay đã ổn và lượng content đủ lớn.

# 9. Art direction và visual direction đã khóa

- UI: 2D hoàn toàn, sáng, bo tròn, vui vẻ, dễ đọc trên mobile.

- Farm world: 2.5D isometric giả 3D bằng asset 2D; không dùng 3D thật.

- Toàn bộ ô đất, cây trồng, vật nuôi, công trình và vật trang trí phải đồng bộ một góc nhìn isometric cố định.

- Màu sắc chủ đạo: xanh lá tươi, vàng ấm, kem sáng, nâu đất, xanh trời; cảnh báo dùng cam/đỏ mềm.

# 10. UI style guide rút gọn để dùng cho thiết kế

| Hạng mục | Định hướng đã khóa |
| --- | --- |
| Phong cách chung | Bright casual farm, warm and lively, slightly nostalgic but modernized. |
| UI shape language | Bo tròn lớn, card/panel mềm, shadow nhẹ. |
| Màu chủ đạo | Farm Green #69C34D, Cream White #FFF7E8, Sun Yellow #FFD75E, Warm Soil Brown #B97A4A, Sky Blue #8ED8FF. |
| Màu trạng thái | Healthy xanh, Warning cam, Danger đỏ cam, Disabled xám kem. |
| World visual | 2.5D isometric bằng sprite 2D; không dùng 3D thật. |
| Typography | Rounded sans dễ đọc, số liệu rõ, không dùng font quá trẻ con hoặc quá mảnh. |

# 11. Mini game / event theo lộ trình dài hơn

- Phase đầu: mini game là hệ cứu hộ mềm, giúp người chơi lấy lại hạt giống, vàng nhỏ hoặc tài nguyên cơ bản.

- Phase giữa: mini game trở thành nơi cho item tối ưu hóa farm như speed-up, vật phẩm chăm cây, hạt giống trung cấp.

- Phase sau: mini game/event là hệ nội dung lớn hơn với phần thưởng hiếm, item giới hạn, vật nuôi hiếm, decor và live-ops theo mùa.

# 12. Vì sao cấu trúc này phù hợp cho dự án

- Không quá idle: người chơi phải thật sự chăm cây và quay lại đúng nhịp.

- Không quá nặng: không ép người chơi phải canh quá sát hoặc tính toán phức tạp.

- Có progression rõ: trồng trọt nuôi chăn nuôi; chăn nuôi lại mở ra mục tiêu ngày dài hơn.

- Có đường gỡ: shop cơ bản và mini game/event giúp người chơi không bị kẹt cứng.

- Dễ mở rộng: phase đầu vẫn đủ gọn để prototype nhanh, nhưng có nhiều đường mở sang phase sau.

# 13. Mục tiêu review sau khi có prototype

- Người chơi casual có mua được gà vào khoảng ngày 2-3 không.

- Có quá nhiều người phải sống nhờ shop thức ăn không.

- Kho có gây bực quá sớm không.

- Người chơi có còn mục tiêu rõ sau ngày 7 không.

- Mini game/event có thật sự đóng vai trò cứu hộ thay vì chỉ là quà phụ không.

---

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

---

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

---

# farm_game_ui_style_guide_v1

_Source:_ `farm_game_ui_style_guide_v1.docx`

FARM GAME UI STYLE GUIDE

Bản v1 - palette, component rules, 2.5D isometric world direction

Mục tiêu tài liệu

- Khóa hướng UI cho phase đầu theo đúng tinh thần game nông trại casual: tươi sáng, sinh động, dễ hiểu, dễ thao tác.

- Xác định palette chính thức để designer, AI UI tool và dev bám cùng một chuẩn.

- Đồng bộ giữa UI 2D và thế giới nông trại 2.5D isometric giả 3D bằng asset 2D.

# 1. Hướng visual chính thức

UI: 2D hoàn toàn. Dùng panel bo tròn, icon thân thiện, bóng mềm, chữ rõ, ưu tiên thao tác nhanh trên mobile.

Thế giới nông trại: 2.5D isometric giả 3D bằng asset 2D. Không dùng 3D thật ở phase đầu.

Cảm xúc chính: Vui tươi, ấm áp, sáng sủa, hơi hoài niệm game farm cũ nhưng sạch và hiện đại hơn.

Từ khóa nên bám: bright casual farm, rounded friendly UI, lively but readable, soft natural materials, mobile-first.

# 2. Bộ màu chính thức

Nguyên tắc chọn màu: màu phải vui và sống động nhưng không neon; cảnh báo rõ nhưng không gây căng; panel sáng để đọc tốt; thế giới farm ưu tiên xanh lá, vàng ấm, kem sáng và nâu đất.

| Nhóm màu | Hex | Mẫu | Cách dùng |
| --- | --- | --- | --- |
| Primary / Farm Green | #69C34D |  | Màu hành động tích cực, nút chính, selected state, cảm giác phát triển. |
| Deep Leaf Green | #4FA63A |  | Nhấn mạnh, hover, viền selected, accent secondary. |
| Sky Blue | #8ED8FF |  | Nền trời, thông tin sạch, yếu tố nước, tăng độ thoáng của UI. |
| Sun Yellow | #FFD75E |  | Reward, XP, điểm nhấn tích cực, badge nổi bật. |
| Warm Soil Brown | #B97A4A |  | Đất, vật liệu nông trại, panel gỗ sơn nhẹ, frame ấm. |
| Cream White | #FFF7E8 |  | Nền popup, panel, card, vùng đọc nội dung. |
| Soft Orange | #FFA94D |  | CTA phụ, bán hàng, nhấn cảnh báo mềm. |
| Berry Pink | #FF6FAE |  | Event, mini game, limited reward, điểm nhấn vui vẻ. |
| Aqua Mint | #67DCC8 |  | Buff nhẹ, chữa lành, support, nước sạch, event phụ. |

## 2.1 Màu trạng thái và rarity

| Loại | Hex | Cách dùng | Ghi chú |
| --- | --- | --- | --- |
| Healthy / Success | #5BCB4B | HP cao, xác nhận thành công, trạng thái tốt. | Dùng nhiều nhất cho feedback tích cực. |
| Warning | #FFB547 | Cần chăm sóc, kho gần đầy, hành động đáng chú ý. | Không lạm dụng màu đỏ quá sớm. |
| Danger | #FF6B5E | Cây sắp chết, kho đầy, cảnh báo chặn thao tác. | Chỉ dùng cho case thật sự khẩn. |
| Info | #5BB8FF | Thông tin, gợi ý, helper state. | Giữ cảm giác sạch và hiện đại. |
| Common | #CFC7B8 | Item thường, viền nhẹ. | Không cần glow. |
| Rare | #5BB8FF | Item hiếm vừa. | Kết hợp badge hoặc viền sáng. |
| Epic | #B07CFF | Item hiếm cao. | Không dùng quá nhiều trên màn chính. |
| Legendary | #FFBE3D | Item hiếm nhất / reward lớn. | Chỉ dùng ở điểm nhấn lớn. |

# 3. Typography, shape và component

Typography: Dùng rounded sans hoặc clean sans thân thiện; tiêu đề đậm, số liệu rõ; text chính ưu tiên màu nâu đậm ấm #5B4636 thay vì đen tuyệt đối.

Shape language: UI dùng bo tròn lớn, card mềm, button dạng capsule hoặc rounded rectangle; tránh góc vuông cứng.

Shadow: Bóng mềm, blur vừa, nghiêng ấm; không dùng shadow đen nặng hoặc glow fantasy.

Texture: Panel có thể gợi chất liệu gỗ sơn nhẹ hoặc kem mịn; không dùng texture quá nhiều làm bẩn giao diện.

## 3.1 Quy chuẩn component

| Component | Visual chính | Màu chính | Ghi chú UX |
| --- | --- | --- | --- |
| Primary Button | Bo tròn lớn, mặt nút sáng hơn đáy một chút, có shadow mềm. | #69C34D / #4FA63A | Dùng cho gieo, thu hoạch, mở chuồng, xác nhận. |
| Secondary Button | Nền kem sáng, viền nhẹ, chữ nâu đậm. | #FFF7E8 / #B97A4A | Dùng cho đóng, hủy, xem thêm. |
| Warning Button | Cam ấm, rõ nhưng không quá gắt. | #FFA94D / #FFB547 | Dùng cho bán, dọn kho, hành động quan trọng. |
| Popup Base | Nền kem sáng, header accent nhẹ, bo góc 20-28 px. | #FFF7E8 + accent chức năng | Panel phải sạch và dễ đọc. |
| Tab / Filter | Dạng pill, selected nổi rõ, unselected nhạt. | #69C34D hoặc #FFD75E | Nhãn ngắn và dễ bấm. |
| Item Card | Icon lớn rõ, badge rarity nhỏ, viền nhẹ. | Nền sáng + viền theo rarity | Tập trung readability, không quá nhiều hiệu ứng. |
| Status Bar | HP / EXP dùng fill màu chuyển cấp. | HP: green-yellow-red, EXP: blue | Thanh ngắn, luôn đọc được ở mobile. |

# 4. Màu theo từng khu chức năng

- Farm Screen: Xanh lá tươi + nâu đất + trời xanh. Giữ cảm giác sống động, tự nhiên, dễ đọc trạng thái ruộng.

- Storage / Warehouse: Kem sáng + nâu gỗ + vàng nhạt. Tạo cảm giác quản lý tài nguyên và vật liệu.

- Seed / Item Shop: Vàng ấm + xanh lá + nhấn cam. Mang cảm giác mua sắm tích cực, không quá thương mại.

- Barn / Animal Area: Nâu ấm + xanh lá + đỏ mái chuồng nhẹ. Tách rõ khỏi ruộng nhưng vẫn thuộc cùng thế giới farm.

- Event / Mini Game: Berry pink + vàng + aqua mint. Cho cảm giác vui và nổi bật hơn mà không phá tổng thể.

# 5. Art direction cho thế giới 2.5D isometric

Góc nhìn: Isometric giả 3D bằng asset 2D, góc nhìn cố định, không xoay camera.

Ô đất: Sprite 2D nhiều trạng thái: đất trống, đã gieo, có cây, cần chăm sóc, chín. Đất dùng nâu ấm, có texture nhẹ.

Cây trồng: Mỗi loại cây có 3-4 phase rõ ràng; phase ripe phải là phase đẹp nhất và bão hòa hơn nhẹ.

Vật nuôi: Sprite tròn trịa, thân thiện, dễ nhận silhouette; bubble icon cho hungry, ready to sell, warning.

Decor và công trình: Đều là asset 2D theo cùng góc nhìn; không trộn asset top-down với asset nghiêng.

Hiệu ứng: Dùng shadow, scale và particle nhỏ để tạo cảm giác chiều sâu; không dùng lighting 3D thật.

# 6. Áp màu vào gameplay state

| Đối tượng | State | Biểu hiện màu | Gợi ý UI/FX |
| --- | --- | --- | --- |
| Cây | Khỏe | HP xanh #5BCB4B | Icon ẩn, cây tươi, không nhấp nháy. |
| Cây | Cần chăm sóc | Cam #FFB547 | Hiện icon needs care + nhẹ rung / bounce. |
| Cây | Sắp chết | Đỏ #FF6B5E | HP đỏ, pulse nhẹ, trạng thái khẩn. |
| Cây | Chín / perfect window | Vàng #FFD75E | Glow mềm hoặc sparkle rất nhẹ. |
| Vật nuôi | Bình thường | Tông base tự nhiên | Không có bubble trạng thái. |
| Vật nuôi | Đói | Cam #FFB547 | Bubble thức ăn nổi rõ. |
| Vật nuôi | Sẵn sàng bán đẹp | Vàng #FFD75E | Viền sáng hoặc badge nhỏ. |
| Kho | Gần đầy | Cam #FFB547 | Label capacity đổi màu. |
| Kho | Đầy | Đỏ #FF6B5E | Chặn thao tác + popup rõ. |

# 7. Do / Don't để giữ UI hợp lý

| Nên làm | Không nên làm |
| --- | --- |
| • Giữ nền panel sáng để nội dung đọc tốt. | • Không dùng neon xanh/hồng quá gắt. |
| • Dùng màu để phân cấp chức năng, không chỉ để trang trí. | • Không phủ glow lên quá nhiều item cùng lúc. |
| • Ưu tiên silhouette rõ cho cây, thú và icon trạng thái. | • Không trộn nhiều phong cách icon khác nhau. |
| • Giữ selected state và locked state khác nhau rõ rệt. | • Không dùng nền quá tối hoặc quá nhiều texture bẩn. |
| • Dùng highlight tiết chế để reward và ripe state thật sự nổi. | • Không dùng đỏ cho quá nhiều cảnh báo nhỏ khiến người chơi mệt. |

# 8. Checklist triển khai cho designer / dev

- Khóa palette chính thức trong file design system và asset guideline.

- Tạo component set tối thiểu: primary button, secondary button, popup base, item card, tab, HP bar, EXP bar, reward popup.

- Giữ toàn bộ scene farm theo hướng 2.5D isometric giả 3D bằng asset 2D; UI là 2D hoàn toàn.

- Khi làm mockup, ưu tiên Farm, Storage, Seed Shop, Barn, Animal Detail trước.

- Mọi screen mới đều phải bám quy tắc màu theo khu chức năng đã chốt.

Kết luận: UI của game nên đi theo hướng bright casual farm với palette xanh lá - vàng - kem - nâu đất làm trục chính, event dùng nhấn hồng/teal có kiểm soát, và toàn bộ scene nông trại dùng 2.5D isometric giả 3D bằng asset 2D để vừa đẹp, vừa nhẹ production, vừa hợp gameplay quản lý nông trại.

---

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

---

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

---

# farm_game_unity_folder_structure_naming_scripts_v1

_Source:_ `farm_game_unity_folder_structure_naming_scripts_v1.docx`

Farm Game - Unity Folder Structure, Naming Convention & Script File List

Production reference v1 • phase đầu • landscape 1920×1080 • one-scene gameplay architecture

| Mục tiêu tài liệu<br>Thống nhất cấu trúc project Unity, quy ước đặt tên và danh sách script khởi đầu để team dev vào project sạch, ít lệch, dễ mở rộng.<br>Tài liệu này bám theo các chốt hiện tại của dự án: 1 gameplay scene chính, world 2.5D isometric giả 3D bằng asset 2D, UI 2D, interaction theo từng object + popup/panel theo ngữ cảnh. |
| --- |

1. Scope & nguyên tắc chung

- Game được triển khai theo hướng landscape-first với base resolution 1920×1080.

- Phase đầu chỉ có một gameplay scene chính: MainFarmScene. Các thao tác như chọn hạt giống, chăm cây, xem vật nuôi, kho, bán hàng và event đều mở bằng popup/panel UI 2D.

- Tên folder, prefab, scene, ScriptableObject và script phải đủ rõ để dev nhìn tên là đoán đúng vai trò. Ưu tiên nhất quán hơn là ngắn quá mức.

- Không dùng tên mơ hồ như Temp, Manager2, TestFinal, NewScript hoặc UIThing.

2. Folder structure đề xuất

Cấu trúc dưới đây ưu tiên: dễ tìm file, dễ handoff, tách rõ world / UI / data / systems, và thuận tiện cho việc tăng content về sau.

Folder tree đề xuất

| Assets/<br>├─ _Project/<br>│  ├─ Art/<br>│  │  ├─ UI/<br>│  │  │  ├─ Atlas/<br>│  │  │  ├─ Icons/<br>│  │  │  ├─ Panels/<br>│  │  │  ├─ Buttons/<br>│  │  │  ├─ HUD/<br>│  │  │  └─ Popups/<br>│  │  ├─ World/<br>│  │  │  ├─ Terrain/<br>│  │  │  ├─ Crops/<br>│  │  │  ├─ Animals/<br>│  │  │  ├─ Buildings/<br>│  │  │  ├─ Decorations/<br>│  │  │  ├─ VFX/<br>│  │  │  └─ Shadows/<br>│  │  ├─ Fonts/<br>│  │  └─ Marketing/<br>│  ├─ Audio/<br>│  │  ├─ BGM/<br>│  │  ├─ SFX/<br>│  │  └─ UI/<br>│  ├─ Data/<br>│  │  ├─ ScriptableObjects/<br>│  │  │  ├─ Crops/<br>│  │  │  ├─ Animals/<br>│  │  │  ├─ Items/<br>│  │  │  ├─ Levels/<br>│  │  │  ├─ Economy/<br>│  │  │  └─ Events/<br>│  │  ├─ Tables/<br>│  │  └─ Config/<br>│  ├─ Materials/<br>│  ├─ Prefabs/<br>│  │  ├─ World/<br>│  │  │  ├─ Crops/<br>│  │  │  ├─ Animals/<br>│  │  │  ├─ Buildings/<br>│  │  │  ├─ Decorations/<br>│  │  │  └─ Interaction/<br>│  │  ├─ UI/<br>│  │  │  ├─ Common/<br>│  │  │  ├─ HUD/<br>│  │  │  ├─ Panels/<br>│  │  │  ├─ Popups/<br>│  │  │  └─ Screens/<br>│  │  └─ FX/<br>│  ├─ Scenes/<br>│  │  ├─ Boot/<br>│  │  ├─ Gameplay/<br>│  │  └─ Sandbox/<br>│  ├─ Scripts/<br>│  │  ├─ Core/<br>│  │  ├─ Data/<br>│  │  ├─ Gameplay/<br>│  │  │  ├─ Crops/<br>│  │  │  ├─ Animals/<br>│  │  │  ├─ Storage/<br>│  │  │  ├─ Economy/<br>│  │  │  ├─ Progression/<br>│  │  │  └─ Events/<br>│  │  ├─ World/<br>│  │  │  ├─ Camera/<br>│  │  │  ├─ Interaction/<br>│  │  │  └─ Views/<br>│  │  ├─ UI/<br>│  │  │  ├─ HUD/<br>│  │  │  ├─ Panels/<br>│  │  │  ├─ Popups/<br>│  │  │  ├─ Screens/<br>│  │  │  └─ Common/<br>│  │  ├─ Managers/<br>│  │  └─ Utilities/<br>│  ├─ Settings/<br>│  ├─ ThirdParty/<br>│  └─ Tests/<br>└─ AddressableAssetsData/    (nếu dùng Addressables sau này) |
| --- |

| Ghi chú cấu trúc<br>Dùng một root riêng như Assets/_Project để gom toàn bộ tài nguyên dự án, tránh lẫn với package hoặc third-party.<br>Art và Prefabs tách riêng để artist tìm asset gốc nhanh, dev tìm prefab nhanh.<br>Scripts tách theo domain thay vì dồn toàn bộ vào một folder. Phase đầu tuy nhỏ nhưng nên chia ngay từ đầu để tránh project rối khi thêm crop/animal/event. |
| --- |

3. Naming convention

3.1 Folder & file chung

- Folder dùng PascalCase hoặc TitleCase không dấu, không khoảng trắng. Ví dụ: Scripts, Gameplay, CropData.

- Không dùng khoảng trắng trong tên file hoặc folder.

- Tên file script phải trùng 100% với tên class public bên trong.

- Tên asset nên có prefix chức năng khi điều đó giúp tìm nhanh hơn trong Project Search.

3.2 Scene naming

| Loại | Ví dụ | Ghi chú |
| --- | --- | --- |
| Boot Scene | SCN_Boot | Load config, preload nhẹ, điều hướng vào gameplay |
| Main Gameplay | SCN_MainFarm | Scene chính của phase đầu |
| Sandbox / Test | SCN_Sandbox_Crops | Scene test nội bộ, không ship |

3.3 Prefab naming

| Nhóm | Pattern | Ví dụ |
| --- | --- | --- |
| UI | PFB_UI_[Name] | PFB_UI_PrimaryButton |
| HUD | PFB_HUD_[Name] | PFB_HUD_TopBar |
| Popup | PFB_POP_[Name] | PFB_POP_StorageFull |
| World crop | PFB_CROP_[Name] | PFB_CROP_Tile |
| World animal | PFB_ANM_[Name] | PFB_ANM_ChickenPen |
| Building | PFB_BLD_[Name] | PFB_BLD_StorageHouse |
| FX | PFB_FX_[Name] | PFB_FX_HarvestBurst |

3.4 ScriptableObject naming

| Loại data | Pattern | Ví dụ |
| --- | --- | --- |
| Crop data | SO_Crop_[Name] | SO_Crop_Carrot |
| Animal data | SO_Animal_[Name] | SO_Animal_Chicken |
| Item data | SO_Item_[Name] | SO_Item_GrassBundle |
| Level data | SO_Level_[Name or Id] | SO_Level_04 |
| Economy config | SO_Config_[Name] | SO_Config_Economy |

3.5 Script/class naming

- System-level class dùng hậu tố rõ nghĩa: CropSystem, AnimalSystem, StorageSystem, EconomySystem.

- UI controller dùng hậu tố Controller hoặc Presenter tùy team chọn, nhưng phải nhất quán. Ví dụ: SeedShopPanelController, HUDTopBarController.

- View class cho object world/UI hiển thị dữ liệu dùng hậu tố View. Ví dụ: CropTileView, AnimalPenView, StorageSlotView.

- Popup class dùng hậu tố Popup. Ví dụ: StorageFullPopup, LevelUpPopup.

- Config/data helper dùng hậu tố Config, Data, State hoặc Model theo đúng vai trò.

4. Script file list khởi đầu

Danh sách dưới đây là bộ script tối thiểu hợp lý để dựng prototype phase đầu. Có thể chưa dùng ngay toàn bộ, nhưng nên tạo cấu trúc tên từ đầu để sau này không đổi loạn.

| Script | Folder | Vai trò | Milestone gợi ý |
| --- | --- | --- | --- |
| BootFlowController.cs | Scripts/Core | Khởi tạo app, load config, chuyển scene | M1 |
| GameManager.cs | Scripts/Managers | Điều phối state game cấp cao | M1 |
| SaveLoadManager.cs | Scripts/Managers | Lưu/đọc save local | M2 |
| FarmCameraController.cs | Scripts/World/Camera | Pan, clamp, focus camera trong farm | M1 |
| WorldInteractionController.cs | Scripts/World/Interaction | Raycast/tap object world, gửi sự kiện chọn | M1 |
| CropSystem.cs | Scripts/Gameplay/Crops | Sinh vòng đời cây, timer, HP, event care | M1-M2 |
| CropTileView.cs | Scripts/World/Views | Hiển thị một ô đất/cây trong world | M1 |
| CropActionPanelController.cs | Scripts/UI/Panels | Panel chăm cây theo trạng thái | M2 |
| SeedShopPanelController.cs | Scripts/UI/Panels | Hiển thị seed shop và mua/gieo hạt | M1 |
| HarvestResolver.cs | Scripts/Gameplay/Crops | Tính sản lượng cuối theo HP/time/random | M2 |
| AnimalSystem.cs | Scripts/Gameplay/Animals | Timer lớn lên, đói, sell window, tử vong | M3 |
| AnimalPenView.cs | Scripts/World/Views | Hiển thị chuồng và thú nuôi trong world | M3 |
| AnimalDetailPanelController.cs | Scripts/UI/Panels | Panel chi tiết thú, cho ăn, bán | M3 |
| StorageSystem.cs | Scripts/Gameplay/Storage | Stack, slot, full-block rule | M2 |
| StoragePanelController.cs | Scripts/UI/Panels | Hiển thị kho, filter, chọn item | M2 |
| SellPanelController.cs | Scripts/UI/Panels | Bán nông sản/item và cập nhật vàng | M2 |
| EconomySystem.cs | Scripts/Gameplay/Economy | Vàng, giá mua/bán, chi phí thức ăn | M1-M2 |
| LevelSystem.cs | Scripts/Gameplay/Progression | EXP, level up, unlock ruộng/chuồng | M2 |
| HUDTopBarController.cs | Scripts/UI/HUD | Gold / EXP / Storage display | M1 |
| PopupManager.cs | Scripts/UI/Common | Mở/đóng popup, stack thứ tự popup | M1 |
| LevelUpPopup.cs | Scripts/UI/Popups | Popup lên level và unlock | M2 |
| StorageFullPopup.cs | Scripts/UI/Popups | Popup kho đầy, điều hướng sang kho/bán | M2 |
| BasicFoodShopPanelController.cs | Scripts/UI/Panels | Shop cỏ/sâu chống bí | M3 |
| MiniGameEventSystem.cs | Scripts/Gameplay/Events | Trigger random mini game/event cứu hộ | M4 |
| MiniGamePopupController.cs | Scripts/UI/Popups | Entry popup cho mini game/event | M4 |

5. Recommended hierarchy - MainFarmScene

Hierarchy tree đề xuất

| SCN_MainFarm<br>├─ Managers<br>│  ├─ GameManager<br>│  ├─ SaveLoadManager<br>│  ├─ PopupManager<br>│  └─ EventBus / MessageHub (optional)<br>├─ CameraRoot<br>│  └─ MainCamera<br>├─ WorldRoot<br>│  ├─ TerrainRoot<br>│  ├─ CropAreaRoot<br>│  ├─ AnimalAreaRoot<br>│  ├─ BuildingsRoot<br>│  │  └─ StorageBuildingRoot<br>│  ├─ DecorRoot<br>│  └─ InteractionMarkersRoot<br>├─ HUDCanvas<br>│  ├─ TopBar<br>│  ├─ BottomNav (optional early build)<br>│  └─ FloatingAlerts<br>├─ PopupCanvas<br>│  ├─ PanelRoot<br>│  └─ PopupRoot<br>└─ SystemsRoot (optional if team tách systems bằng GameObject) |
| --- |

6. Folder-by-folder guideline

| Folder | Nên chứa | Không nên chứa |
| --- | --- | --- |
| Art/UI | Sprite nguồn UI, atlas, icon, panel base | Prefab UI hoặc script |
| Art/World | Sprite nền farm, crop phases, animals, buildings | Config gameplay |
| Data/ScriptableObjects | CropData, AnimalData, ItemData, LevelData | Prefab scene object |
| Prefabs/UI | Component/panel/popup prefab tái sử dụng | Texture gốc hoặc sprite source |
| Prefabs/World | Crop tile, pen, building, decor prefab | Asset chỉ để concept chưa dùng |
| Scripts/Gameplay | Logic gameplay thuần, không phụ thuộc UI trực tiếp | Layout UI hoặc sprite references thô |
| Scripts/UI | Controller/presenter cho panel, HUD, popup | Logic economy/crop loop cốt lõi |

7. Production naming rules chi tiết

- Biến private serializable trong MonoBehaviour: camelCase có [SerializeField], ví dụ: [SerializeField] private CropTileView cropTileView;

- Property / public method / public class: PascalCase, ví dụ: CurrentHealth, OpenSeedShop(), AnimalSystem.

- Const: UPPER_SNAKE_CASE chỉ dùng khi team muốn tách hẳn hằng số. Nếu không, dùng PascalCase cho readonly static cũng được; nhưng phải thống nhất.

- Bool nên đọc được như câu hỏi: isHungry, isLocked, canHarvest, hasStorageSpace.

- Event/callback nên có tiền tố On hoặc hậu tố Changed/Requested/Completed: OnCropSelected, StorageFullRequested, HarvestCompleted.

8. Recommended file list for ScriptableObjects

| Asset | Folder | Tên ví dụ | Ghi chú |
| --- | --- | --- | --- |
| CropData assets | Data/ScriptableObjects/Crops | SO_Crop_Carrot | 1 asset / 1 crop |
| AnimalData assets | Data/ScriptableObjects/Animals | SO_Animal_Cow | 1 asset / 1 animal |
| ItemData assets | Data/ScriptableObjects/Items | SO_Item_WormBundle | gồm seed / produce / feed / event item |
| LevelData assets | Data/ScriptableObjects/Levels | SO_Level_04 | unlock ruộng, chuồng, crop |
| EconomyConfig | Data/ScriptableObjects/Economy | SO_Config_Economy | buy/sell multipliers, safety floor |
| EventConfig | Data/ScriptableObjects/Events | SO_Config_MiniGameRewards | reward pool & trigger chance |

9. File list theo milestone

| Milestone 1 - crop loop tối thiểu<br>Bắt buộc có: BootFlowController, GameManager, FarmCameraController, WorldInteractionController, CropSystem, CropTileView, SeedShopPanelController, HUDTopBarController, EconomySystem.<br>Mục tiêu: gieo trồng, timer lớn, thu hoạch, cập nhật vàng/EXP, pan camera, mở panel cơ bản. |
| --- |

| Milestone 2 - care, kho, bán<br>Bổ sung: HarvestResolver, CropActionPanelController, StorageSystem, StoragePanelController, SellPanelController, SaveLoadManager, StorageFullPopup, LevelSystem, LevelUpPopup.<br>Mục tiêu: HP cây, cỏ/sâu/thiếu nước, kho đầy chặn thu hoạch, bán item để giải phóng kho. |
| --- |

| Milestone 3 - animal loop<br>Bổ sung: AnimalSystem, AnimalPenView, AnimalDetailPanelController, BasicFoodShopPanelController.<br>Mục tiêu: mở chuồng bằng level + cost, cho ăn, timer lớn lên, bán vật nuôi. |
| --- |

| Milestone 4 - event & polish<br>Bổ sung: MiniGameEventSystem, MiniGamePopupController và các popup/warning mở rộng.<br>Mục tiêu: comeback loop, event reward, polish feedback/UI. |
| --- |

10. Nên tránh trong project structure

- Không để script test tạm trong cùng folder với script ship chính. Nếu cần test, dùng Scenes/Sandbox và Scripts/Utilities hoặc Tests.

- Không đặt script UI đọc trực tiếp logic crop/animal phức tạp rồi tự tính toán. UI chỉ nên gọi service/system hoặc đọc state đã chuẩn hóa.

- Không giữ nhiều bản prefab gần giống nhau chỉ khác một màu nhỏ. Dùng prefab variant hoặc style token.

- Không tạo nhiều singleton không kiểm soát. Chỉ manager thực sự toàn cục mới nên singleton.

11. Definition of done cho phần structure

| Hạng mục | Done khi | Người check |
| --- | --- | --- |
| Folder structure | Project tree tạo đúng root, không file lạc ngoài quy ước | Lead dev |
| Naming convention | Tên scene/script/prefab mới đều bám pattern đã chốt | Lead dev / reviewer |
| Core scripts | Danh sách script M1 tồn tại và compile sạch | Dev |
| Prefab map | Prefab UI/world cốt lõi tạo đúng folder | UI dev |
| SO assets | Crop/Animal/Item/Level asset đầu tiên tạo đúng chuẩn | Game designer / dev |

| Khuyến nghị cuối<br>Nếu team nhỏ hoặc 1 người làm, vẫn nên giữ cấu trúc này ngay từ đầu. Chi phí tạo folder và tên chuẩn rất thấp, nhưng lợi ích về sau rất lớn.<br>Khi đã bắt đầu code, nên xem tài liệu này như chuẩn tham chiếu. Nếu có đổi convention, phải đổi đồng bộ một lần và cập nhật lại tài liệu. |
| --- |

---

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

---

# farm_game_visual_prompts_v2_corrected

_Source:_ `farm_game_visual_prompts_v2_corrected.docx`

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
| Cảnh báo chăm sóc | Warning #FFB547 cho cảnh báo chăm sóc; chỉ khi cây sắp chết mới dùng Danger #FF6B5E. |
| EXP / info | Sky Blue #8ED8FF và Aqua Mint #67DCC8. |

## Prompt chính để copy vào AI UI tool

Design a mobile farm game Home Farm screen in bright casual style. The world view must be 2.5D isometric fake-3D made with 2D assets, while the interface is fully 2D. Show a cheerful farm field with neat crop plots, soft rounded casual UI, warm sunlight feeling, bright grass, warm soil, and readable mobile HUD. The top bar should show player avatar, level, EXP bar, gold, storage capacity, and settings in compact rounded capsules. The main center area must focus on crop plots with different states: empty plot, growing crop, crop needing care, ripe crop, and crop near death. Use very clear visual state signals without making the screen noisy. Add small state bubbles or icons for weeds, bugs, lack of water, and ready-to-harvest. The bottom navigation should contain Farm, Storage, Shop, Barn, and Event. Keep all buttons soft rounded, readable, colorful but controlled. Use Farm Green #69C34D as the primary action color, Cream White #FFF7E8 for panels, Sky Blue #8ED8FF for information, Sun Yellow #FFD75E for rewards and currency, Warm Soil Brown #B97A4A for earth surfaces, Warning #FFB547 for care alerts, and Danger #FF6B5E only for near-death urgency. The overall feeling should be lively, warm, accessible, and slightly nostalgic like a classic social farm game modernized for mobile. Do not make it hyper-detailed or realistic; keep it stylized, cute, clear, and production-friendly.

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
| Thanh HP | Healthy #5BCB4B → Warning #FFB547 → Danger #FF6B5E |
| Action chăm sóc | Aqua Mint #67DCC8 cho tưới, Warning #FFB547 cho cắt cỏ/bắt sâu, Farm Green #69C34D + Sun Yellow #FFD75E cho thu hoạch |
| Thông tin timer | Sky Blue |
| Cảnh báo nguy hiểm | Danger #FF6B5E, dùng tiết chế; không dùng đỏ gắt ngoài trạng thái critical |

## Prompt chính để copy vào AI UI tool

Design a contextual Crop Action Panel for a mobile farm game. This is a compact 2D popup panel that appears when the player taps a crop in the isometric farm world. The panel must clearly show crop name, current growth stage, remaining time, health bar, and current care problems. It should dynamically adapt so that only relevant actions appear: remove weeds, catch bugs, water plant, harvest, or simply view status. Keep the layout very readable and action-oriented. The health bar should use Healthy #5BCB4B, Warning #FFB547, and Danger #FF6B5E to indicate crop condition. If the crop is ripe, show a highlighted perfect harvest window and estimated harvest quality in a friendly way. If the crop is near death, use warning emphasis without making the game feel harsh. The panel should feel like a smart farm management helper: compact, colorful, soft-cornered, easy to understand, and clearly tied to the player tapping one object.

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
| Storage capacity bình thường | Warning #FFB547 |
| Near full | Danger #FF6B5E |
| Full | Coral-red nhẹ |
| Tab selected | Farm Green hoặc Sun Yellow tùy loại tab |

## Prompt chính để copy vào AI UI tool

Create a Storage or Warehouse screen for a casual farm game. The screen should be fully 2D UI, clean, bright, and management-friendly, sitting on top of the game style established by the cheerful farm. It must show storage title, used capacity like 27/50, and a very readable warning state for near-full or full. Include filter tabs for All, Seeds, Crops, Feed, and Event Items. The item list should use roomy rounded cards or rows with icon, item name, quantity, and quick sell interaction if sellable. Make inventory management feel smooth, not stressful. Use Cream White #FFF7E8 as the main panel background, Warm Soil Brown #B97A4A for structure, Farm Green #69C34D for positive actions, Warning #FFB547 for near-full pressure, and Danger #FF6B5E only when storage is completely full. The overall mood should remain soft and lively, not spreadsheet-like or industrial.

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
| Sell CTA | Soft Orange #FFA94D cho nút bán; Sun Yellow #FFD75E cho điểm nhấn reward |
| Tổng vàng nhận | Sun Yellow + outline nhẹ |
| Counter / stepper | Cream + green accent |

## Prompt chính để copy vào AI UI tool

Design a Sell screen or Sell popup for a mobile farm management game. The layout should let players quickly select item quantity and see the total gold they will receive. This is a functional but cheerful 2D interface, with rounded controls, soft spacing, and clear numbers. The selling action should feel rewarding but not aggressive; use Soft Orange #FFA94D for the main sell call-to-action and Sun Yellow #FFD75E for the gold summary. Show item icon, item name, owned quantity, sell price per unit, quantity stepper, and a large total gold summary area. The overall UI should be lightweight, pleasant, and easy to use repeatedly when the storage becomes full. Keep the visual style aligned with a bright farm game, not a cash register or finance app.

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
| Hungry state | Warning #FFB547 |
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
| Feed CTA | Farm Green #69C34D hoặc Aqua Mint #67DCC8 |
| Sell CTA | Soft Orange #FFA94D / Sun Yellow #FFD75E |
| Cảnh báo sắp chết | Warning #FFB547 trước; Danger #FF6B5E khi sắp hết tuổi thọ |
| Thông tin tiến độ | Sky Blue |

## Prompt chính để copy vào AI UI tool

Create an Animal Detail and Feed Panel for a mobile farm game. This is a contextual 2D popup that appears when a player taps an animal or pen in the isometric barn area. The panel must clearly show animal name, current stage, time remaining to next stage, hunger status, required feed type, current sell value, and next-stage value. The most important actions are Feed and Sell. If feed is missing, the panel must guide the player to the basic food shop in a friendly way. If the animal is mature and close to its lifetime limit, show a warning with Warning #FFB547 first and Danger #FF6B5E only at the final urgent state. Keep the style cute, readable, and consistent with the rest of the farm game UI. This panel should feel practical and emotionally clear without becoming stressful.

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
| Feed cards | Aqua Mint #67DCC8 và xanh lá nhẹ cho Grass; Sun Yellow #FFD75E + Soft Orange #FFA94D cho Worm nếu cần khác biệt |
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
| Header event | Berry Pink #FF6FAE + Sun Yellow #FFD75E |
| Reward accents | Sun Yellow, Rare colors nếu có |
| Main CTA | Farm Green #69C34D hoặc Berry Pink #FF6FAE tùy vibe event |
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
| Level Up | Sun Yellow #FFD75E + Farm Green #69C34D + Sky Blue #8ED8FF |
| Storage Full | Danger #FF6B5E + Cream White #FFF7E8 |
| Unlock Pen | Farm Green #69C34D + Warm Soil Brown #B97A4A |
| Warning critical | Warning #FFB547 trước; Danger #FF6B5E khi critical |

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

---

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

---

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

---

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

---

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

---
