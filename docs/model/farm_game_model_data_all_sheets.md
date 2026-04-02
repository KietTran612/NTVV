# Farm Game Model Data v1 — Markdown Export

Source workbook: `farm_game_model_data_v1.xlsx`

This export preserves worksheet structure as Markdown tables. Spreadsheet formulas are kept as formula text where present.


---

# CropData

- Rows exported: 9

- Columns exported: 20


| crop_id | ten_cay | unlock_level | seed_cost_gold | grow_time_min | phase1_pct | phase2_pct | phase3_pct | base_yield_units | sell_price_per_unit_gold | base_gross_gold | base_profit_gold | xp_reward | weed_chance_pct | bug_chance_pct | water_chance_pct | max_care_events | perfect_window_min | post_ripe_life_min | ghi_chu |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| crop_01 | Cà rốt | 1 | 8 | 2 | 0.25 | 0.35 | 0.4 | 3 | 4 | =I2*J2 | =K2-D2 | 4 | 0.1 | 0.08 | 0.1 | 1 | 1 | 3 | Cây mở đầu, quay vòng vốn nhanh. |
| crop_02 | Khoai tây | 1 | 12 | 4 | 0.25 | 0.35 | 0.4 | 3 | 5 | =I3*J3 | =K3-D3 | 5 | 0.12 | 0.1 | 0.12 | 1 | 1 | 4 | Ổn định, phù hợp giai đoạn đầu. |
| crop_03 | Ngô | 2 | 18 | 5 | 0.25 | 0.35 | 0.4 | 4 | 6 | =I4*J4 | =K4-D4 | 7 | 0.14 | 0.12 | 0.14 | 2 | 2 | 5 | Bắt đầu có lời tốt hơn cây đầu game. |
| crop_04 | Cải xanh | 3 | 25 | 8 | 0.25 | 0.35 | 0.4 | 5 | 7 | =I5*J5 | =K5-D5 | 9 | 0.16 | 0.14 | 0.16 | 2 | 2 | 6 | Cân bằng giữa sản lượng và tốc độ. |
| crop_05 | Bí đỏ | 5 | 40 | 15 | 0.25 | 0.35 | 0.4 | 4 | 14 | =I6*J6 | =K6-D6 | 14 | 0.18 | 0.16 | 0.18 | 2 | 3 | 8 | Cây trung hạn, yêu cầu quay lại đúng nhịp. |
| crop_06 | Dưa hấu | 7 | 65 | 25 | 0.25 | 0.35 | 0.4 | 3 | 28 | =I7*J7 | =K7-D7 | 20 | 0.22 | 0.18 | 0.22 | 3 | 4 | 10 | Lợi nhuận tốt, phù hợp offline dài hơn. |
| crop_07 | Hoa hướng dương | 9 | 90 | 40 | 0.25 | 0.35 | 0.4 | 3 | 40 | =I8*J8 | =K8-D8 | 28 | 0.24 | 0.2 | 0.24 | 3 | 5 | 12 | Giá trị cao, sản lượng thấp. |
| crop_08 | Dâu vàng hiếm | 11 | 140 | 60 | 0.25 | 0.35 | 0.4 | 2 | 90 | =I9*J9 | =K9-D9 | 40 | 0.28 | 0.24 | 0.28 | 3 | 6 | 15 | Cây cao cấp/hiếm, dùng cho mid game trở đi. |



---

# AnimalData

- Rows exported: 5

- Columns exported: 25


| animal_id | ten_vat_nuoi | unlock_level | pen_type | pen_cost_gold | buy_cost_gold | initial_investment_gold | feed_type | hunger_interval_hours | feed_qty_grass | feed_qty_worm | stage1_age_days | sell_stage1_gold | profit_stage1_before_feed | stage2_age_days | sell_stage2_gold | profit_stage2_before_feed | stage3_age_days | sell_stage3_gold | profit_stage3_before_feed | lifetime_after_mature_days | xp_stage1 | xp_stage2 | xp_stage3 | ghi_chu |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| animal_01 | Gà | 4 | Chuồng Gà | 180 | 220 | =E2+F2 | Worm | 24 | 0 | 1 | 1 | 180 | =M2-F2 | 2 | 280 | =P2-F2 | 3 | 420 | =S2-F2 | 1 | 18 | 28 | 42 | Thú mở đầu; thức ăn chính là sâu. |
| animal_02 | Vịt | 7 | Chuồng Vịt | 280 | 320 | =E3+F3 | Worm + Grass | 24 | 1 | 1 | 1 | 250 | =M3-F3 | 3 | 420 | =P3-F3 | 4 | 620 | =S3-F3 | 1 | 25 | 40 | 62 | Ăn hỗn hợp, gắn chặt với loop chăm cây. |
| animal_03 | Heo | 10 | Chuồng Heo | 650 | 700 | =E4+F4 | Grass | 36 | 2 | 0 | 2 | 560 | =M4-F4 | 4 | 920 | =P4-F4 | 6 | 1380 | =S4-F4 | 2 | 56 | 92 | 138 | Bước nhảy lớn về đầu tư và lợi nhuận. |
| animal_04 | Bò | 13 | Chuồng Bò | 1100 | 1200 | =E5+F5 | Grass | 48 | 3 | 0 | 3 | 950 | =M5-F5 | 5 | 1550 | =P5-F5 | 7 | 2300 | =S5-F5 | 2 | 95 | 155 | 230 | Mục tiêu tuần 2, đầu tư dài hơn. |



---

# ItemData

- Rows exported: 8

- Columns exported: 11


| item_id | ten_item | category | stack_limit | buy_pack_qty | buy_price_gold | sell_price_gold | source | store_in_shared_warehouse | use_case | ghi_chu |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| item_001 | Grass | FeedMaterial | 99 |  |  | 2 | Crop care: remove weed | Y | Nguyên liệu cho vịt / heo / bò ăn | Tài nguyên phụ sinh ra từ chăm cây. |
| item_002 | Worm | FeedMaterial | 99 |  |  | 3 | Crop care: catch bug | Y | Nguyên liệu cho gà / vịt ăn | Tài nguyên phụ sinh ra từ chăm cây. |
| item_003 | Grass Bundle | ShopBundle | 99 | 5 | 30 |  | Shop / Mini game | Y | Mua nhanh cỏ để chống bí | Nguồn cứu hộ, không phải nguồn tối ưu. |
| item_004 | Worm Bundle | ShopBundle | 99 | 5 | 40 |  | Shop / Mini game | Y | Mua nhanh sâu để chống bí | Nguồn cứu hộ, không phải nguồn tối ưu. |
| item_005 | Speed-Up 5m | Consumable | 99 |  |  |  | Mini game / Event | Y | Rút ngắn 5 phút cho cây hoặc event setup | Có thể mở rộng về sau. |
| item_006 | Emergency Seed Pack | Recovery | 99 | 1 |  |  | Mini game comeback / Event cứu trợ | Y | Gói hạt giống cơ bản để dựng lại vòng chơi | Phục vụ cơ chế comeback khi thiếu vốn. |
| item_007 | Rare Seed Token | Event | 99 |  |  |  | Mini game / Event cao hơn | Y | Đổi hạt giống hiếm hoặc seed cao cấp | Dùng cho giai đoạn mở rộng sau. |



---

# LevelUnlocks

- Rows exported: 14

- Columns exported: 5


| level | xp_cumulative | unlock_group | unlock_name | unlock_detail |
| --- | --- | --- | --- | --- |
| 1 | 0 | Start | Cà rốt, Khoai tây, 6 ô đất đầu | Bắt đầu game với trồng trọt là lõi. |
| 2 | 40 | Crop | Ngô | Tăng lựa chọn cây ngắn hạn. |
| 3 | 100 | Crop + Land | Cải xanh + thêm 2 ô đất | Tổng 8 ô đất. |
| 4 | 180 | Animal + Shop | Mở Chuồng Gà + shop thức ăn cơ bản | Bắt đầu nhánh chăn nuôi. |
| 5 | 280 | Crop + Land | Bí đỏ + thêm 2 ô đất | Tổng 10 ô đất. |
| 6 | 400 | Balance | Nhịp chăm sóc dày hơn | Tăng áp lực quản lý. |
| 7 | 550 | Animal + Crop | Mở Chuồng Vịt + Dưa hấu | Farm bắt đầu có 2 lớp rõ hơn. |
| 8 | 730 | Storage + Land | Nâng kho lần 1 + thêm 4 ô đất | Kho 70 slot, tổng 14 ô đất. |
| 9 | 940 | Crop | Hoa hướng dương | Cây giá trị cao bắt đầu xuất hiện. |
| 10 | 1180 | Animal | Mở Chuồng Heo | Bước nhảy lớn về kinh tế. |
| 11 | 1450 | Crop | Dâu vàng hiếm | Mục tiêu cao cấp đầu mid game. |
| 12 | 1760 | Land | Thêm 4 ô đất | Tổng 18 ô đất. |
| 13 | 2110 | Animal | Mở Chuồng Bò | Mục tiêu tuần 2. |



---

# LandExpansion

- Rows exported: 6

- Columns exported: 4


| unlock_level | added_plots | total_plots | ghi_chu |
| --- | --- | --- | --- |
| 1 | 6 | =B2 | Số ô đất ban đầu. |
| 3 | 2 | =C2+B3 | Mở rộng sớm để người chơi có thêm nguồn thu. |
| 5 | 2 | =C3+B4 | Duy trì cảm giác farm đang lớn lên. |
| 8 | 4 | =C4+B5 | Chuẩn bị cho giai đoạn nuôi vịt / tối ưu kho. |
| 12 | 4 | =C5+B6 | Bước mở rộng cho mid game sớm. |



---

# CropStates

- Rows exported: 14

- Columns exported: 10


| state_id | state_type | state_name | description | entry_condition | exit_condition | player_action | affects_hp | harvest_allowed | ghi_chu |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| crop_state_01 | Primary | Empty | Ô đất trống, có thể gieo hạt. | Không có cây trên ô. | Người chơi gieo hạt. | Plant Seed | No | No | Trạng thái mặc định của ô trồng. |
| crop_state_02 | Primary | Seeded | Hạt đã gieo, bắt đầu timer tăng trưởng. | Gieo hạt thành công. | Chuyển sang Growing Phase 1. | None | No | No | Có thể hiện hạt/ầm mầm rất sớm. |
| crop_state_03 | Primary | Growing Phase 1 | Pha phát triển đầu tiên. | Timer tăng trưởng đạt 0-25%. | Sang Phase 2 hoặc Needs Care overlay. | Care / Wait | Yes | No | Bắt đầu có thể phát sinh cỏ/sâu/thiếu nước. |
| crop_state_04 | Primary | Growing Phase 2 | Pha phát triển giữa. | Timer đạt 25-60%. | Sang Phase 3 hoặc Needs Care overlay. | Care / Wait | Yes | No | Tiếp tục có thể phát sinh sự cố. |
| crop_state_05 | Primary | Growing Phase 3 | Pha phát triển cuối trước khi chín. | Timer đạt 60-100%. | Sang Ripe. | Care / Wait | Yes | No | Pha áp lực chăm sóc thường rõ hơn. |
| crop_state_06 | Primary | Ripe | Cây đã chín và vào khung thu đẹp. | Hoàn tất thời gian tăng trưởng. | Sang Late Harvest 1 hoặc bị thu hoạch. | Harvest | No | Yes | Khi mới chín sẽ bắt đầu Perfect Window. |
| crop_state_07 | Primary | Late Harvest 1 | Đã qua khung thu đẹp nhưng chưa quá già. | Hết Perfect Window. | Sang Late Harvest 2 hoặc bị thu hoạch. | Harvest | No | Yes | Hệ số thời gian giảm xuống 0.92. |
| crop_state_08 | Primary | Late Harvest 2 | Đã trễ hơn, sản lượng giảm mạnh hơn. | Qua nửa đầu Post-Ripe Life. | Sang Dead hoặc bị thu hoạch. | Harvest | No | Yes | Hệ số thời gian giảm xuống 0.80. |
| crop_state_09 | Primary | Dead | Cây chết, mất trắng. | HP về 0 hoặc quá tuổi thọ sau chín. | Dọn ô và quay về Empty. | Clear Dead Crop | No | No | Mất toàn bộ giá trị đầu tư của cây đó. |
| crop_state_10 | Overlay | Needs Care | Cờ cảnh báo chung: cây đang có ít nhất một vấn đề cần xử lý. | Có cỏ, sâu hoặc thiếu nước. | Xử lý hết mọi vấn đề đang tồn tại. | Open care actions | Yes | Depends | Có thể chỉ hiện 1 icon tổng ở phase đầu. |
| crop_state_11 | Overlay | Weed Present | Cỏ xuất hiện trên ô trồng. | Random event trong vòng đời cây. | Người chơi cắt cỏ. | Remove Weed | Yes | Depends | Khi cắt cỏ sẽ nhận item Grass. |
| crop_state_12 | Overlay | Bug Present | Sâu xuất hiện trên cây. | Random event trong vòng đời cây. | Người chơi bắt sâu. | Catch Bug | Yes | Depends | Khi bắt sâu sẽ nhận item Worm. |
| crop_state_13 | Overlay | Water Needed | Cây bị thiếu nước. | Random event trong vòng đời cây. | Người chơi tưới nước. | Water Plant | Yes | Depends | Không cho item, chỉ xóa trạng thái thiếu nước. |



---

# AnimalStates

- Rows exported: 8

- Columns exported: 10


| state_id | state_type | state_name | description | entry_condition | exit_condition | player_action | growth_allowed | sell_allowed | ghi_chu |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| animal_state_01 | Primary | Empty Pen | Ô chuồng trống, có thể mua con giống. | Chưa có vật nuôi trong chuồng. | Mua con giống. | Buy Animal | No | No | Một chuồng chỉ chứa 1 đơn vị vật nuôi ở phase đầu. |
| animal_state_02 | Primary | Stage 1 | Vật nuôi ở giai đoạn đầu. | Mua con giống thành công. | Đủ thời gian và được cho ăn đúng để lên Stage 2. | Feed / Wait | Yes | Yes | Có thể bán non ở giai đoạn này. |
| animal_state_03 | Primary | Stage 2 | Vật nuôi phát triển trung gian. | Đạt mốc tuổi Stage 2. | Đủ thời gian và được cho ăn đúng để lên Stage 3. | Feed / Wait | Yes | Yes | Giá bán tốt hơn Stage 1. |
| animal_state_04 | Primary | Stage 3 / Mature | Vật nuôi trưởng thành theo phase đầu. | Đạt mốc tuổi Stage 3. | Bị bán hoặc quá tuổi thọ sau trưởng thành. | Sell / Wait | Yes | Yes | Điểm bán đẹp nhất ở phase đầu. |
| animal_state_05 | Overlay | Hungry | Vật nuôi đang đói, cần đúng loại thức ăn. | Đến mốc hunger interval. | Được cho ăn đúng loại. | Feed Animal | No | Yes | Không chết ngay vì đói, nhưng growth bị dừng. |
| animal_state_06 | Overlay | Growth Paused | Tăng trưởng tạm dừng do chưa cho ăn. | Hungry kéo dài. | Được cho ăn đúng loại. | Feed Animal | No | Yes | Giữ đơn giản cho prototype phase đầu. |
| animal_state_07 | Primary | Dead | Vật nuôi chết do quá tuổi thọ sau trưởng thành mà không bán. | Hết lifetime_after_mature_days. | Dọn chuồng. | Clear Pen | No | No | Phase đầu chưa thêm nguyên nhân chết khác. |



---

# StorageStates

- Rows exported: 5

- Columns exported: 7


| state_id | state_name | description | condition | player_impact | suggested_ui_message | ghi_chu |
| --- | --- | --- | --- | --- | --- | --- |
| storage_state_01 | Available | Kho còn nhiều chỗ trống. | Số slot đã dùng thấp hơn ngưỡng cảnh báo. | Thu hoạch và nhận item bình thường. |  | Trạng thái bình thường. |
| storage_state_02 | Near Full | Kho sắp đầy. | Slot đã dùng chạm ngưỡng cảnh báo nội bộ, ví dụ 80%. | Người chơi nên được gợi ý bán hoặc dùng bớt vật phẩm. | Kho sắp đầy. Hãy chuẩn bị bán hoặc sử dụng bớt vật phẩm. | Chỉ là cảnh báo, chưa chặn gameplay. |
| storage_state_03 | Full | Kho đã đầy. | Không còn slot trống khả dụng. | Không cho thu hoạch cây, không cho nhận cỏ/sâu, không cho nhận item event. | Kho đã đầy. Hãy bán hoặc sử dụng bớt vật phẩm. | Rule đã chốt cho prototype phase đầu. |
| storage_state_04 | Blocked For Collect | Hành động thu nhận bị chặn vì kho đầy. | Người chơi cố thực hiện collect khi Full. | Phải rời thao tác, vào kho hoặc popup bán. | Kho đã đầy. Vui lòng dọn chỗ trống trước khi tiếp tục. | Có thể coi như một sub-state khi người chơi bấm collect. |



---

# BalanceRules

- Rows exported: 22

- Columns exported: 6


| rule_group | rule_key | threshold_or_value | unit | description | notes |
| --- | --- | --- | --- | --- | --- |
| Crop HP | health_factor_90_100 | 1 | multiplier | HP 90-100 cho hệ số sản lượng 1.00 | Thu hoạch đẹp nhất khi cây khỏe. |
| Crop HP | health_factor_70_89 | 0.9 | multiplier | HP 70-89 cho hệ số sản lượng 0.90 | Giảm nhẹ, vẫn lời tốt. |
| Crop HP | health_factor_50_69 | 0.78 | multiplier | HP 50-69 cho hệ số sản lượng 0.78 | Giảm vừa phải. |
| Crop HP | health_factor_30_49 | 0.65 | multiplier | HP 30-49 cho hệ số sản lượng 0.65 | Gần mức rủi ro cao. |
| Crop HP | health_factor_1_29 | 0.5 | multiplier | HP 1-29 cho hệ số sản lượng 0.50 | Vẫn còn cây sống nhưng hiệu quả thấp. |
| Crop Time | perfect_window_factor | 1.1 | multiplier | Thu trong perfect window được bonus 110% | Mức bonus đang chốt cho phase đầu. |
| Crop Time | late_harvest_1_factor | 0.92 | multiplier | Thu trễ mức 1 sau perfect window | Nửa đầu post-ripe. |
| Crop Time | late_harvest_2_factor | 0.8 | multiplier | Thu trễ mức 2 sau perfect window | Nửa sau post-ripe. |
| Crop Drain | weed_hp_loss | 2 | HP / 10 sec | Cỏ làm mất HP theo thời gian | Cộng dồn nếu có nhiều vấn đề. |
| Crop Drain | bug_hp_loss | 3 | HP / 10 sec | Sâu làm mất HP theo thời gian | Áp lực hơn cỏ. |
| Crop Drain | water_hp_loss | 2 | HP / 10 sec | Thiếu nước làm mất HP theo thời gian | Áp lực tương đương cỏ. |
| Crop Safety | min_sale_value_if_alive | 0.85 | ratio of seed cost | Nếu cây còn sống và thu được thì giá trị tối thiểu ≈ 85% giá hạt | Tránh cảm giác lỗ quá nặng ở phase đầu. |
| Crop Random | yield_random_min | 0.95 | multiplier | Biên dao động thấp của random yield | Tạo cảm giác tự nhiên. |
| Crop Random | yield_random_max | 1.05 | multiplier | Biên dao động cao của random yield | Tạo cảm giác tự nhiên. |
| Storage | base_capacity | 50 | slots | Sức chứa kho ban đầu | Kho chung cho mọi item lưu trữ được. |
| Storage | upgrade_1_capacity | 70 | slots | Sức chứa sau nâng cấp 1 | Mở ở level 8. |
| Storage | upgrade_1_cost | 150 | gold | Giá nâng cấp kho lần 1 | Mở ở level 8. |
| Storage | upgrade_2_capacity | 95 | slots | Sức chứa sau nâng cấp 2 | Mở ở level 12. |
| Storage | upgrade_2_cost | 350 | gold | Giá nâng cấp kho lần 2 | Mở ở level 12. |
| Storage | upgrade_3_capacity | 125 | slots | Sức chứa sau nâng cấp 3 | Mở ở level 16. |
| Storage | upgrade_3_cost | 700 | gold | Giá nâng cấp kho lần 3 | Mở ở level 16. |



---

# MiniGameRewards

- Rows exported: 8

- Columns exported: 9


| reward_id | reward_name | reward_type | weight_pct | min_qty | max_qty | primary_role | unlock_phase | notes |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| reward_01 | Hạt giống thường | Seed | 0.35 | 2 | 5 | Comeback + fun | Phase 1 | Nguồn chính để cứu người chơi thiếu vốn đầu game. |
| reward_02 | Grass Bundle | FeedBundle | 0.18 | 1 | 1 | Support | Phase 1 | Hỗ trợ chăn nuôi khi thiếu cỏ. |
| reward_03 | Worm Bundle | FeedBundle | 0.18 | 1 | 1 | Support | Phase 1 | Hỗ trợ chăn nuôi khi thiếu sâu. |
| reward_04 | Coin nhỏ | Currency | 0.15 | 20 | 40 | Comeback | Phase 1 | Không nên cho quá nhiều vàng trực tiếp. |
| reward_05 | Speed-Up 5m | Consumable | 0.08 | 1 | 1 | Tempo boost | Phase 1 | Giúp rút ngắn chờ đợi, tạo cảm giác tích cực. |
| reward_06 | Hạt giống cao cấp / hiếm | RareSeed | 0.05 | 1 | 1 | Aspirational | Phase 2+ | Gợi mở progression cao hơn. |
| reward_07 | Item đặc biệt khác | Special | 0.01 | 1 | 1 | Event | Phase 2+ | Dành cho event hoặc live-ops sau này. |

