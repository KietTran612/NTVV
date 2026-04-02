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
