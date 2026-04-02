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
