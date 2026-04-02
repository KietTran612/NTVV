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
