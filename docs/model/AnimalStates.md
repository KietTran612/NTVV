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
