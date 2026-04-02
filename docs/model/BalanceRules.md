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
