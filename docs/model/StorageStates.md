# StorageStates

- Rows exported: 5

- Columns exported: 7


| state_id | state_name | description | condition | player_impact | suggested_ui_message | ghi_chu |
| --- | --- | --- | --- | --- | --- | --- |
| storage_state_01 | Available | Kho còn nhiều chỗ trống. | Số slot đã dùng thấp hơn ngưỡng cảnh báo. | Thu hoạch và nhận item bình thường. |  | Trạng thái bình thường. |
| storage_state_02 | Near Full | Kho sắp đầy. | Slot đã dùng chạm ngưỡng cảnh báo nội bộ, ví dụ 80%. | Người chơi nên được gợi ý bán hoặc dùng bớt vật phẩm. | Kho sắp đầy. Hãy chuẩn bị bán hoặc sử dụng bớt vật phẩm. | Chỉ là cảnh báo, chưa chặn gameplay. |
| storage_state_03 | Full | Kho đã đầy. | Không còn slot trống khả dụng. | Không cho thu hoạch cây, không cho nhận cỏ/sâu, không cho nhận item event. | Kho đã đầy. Hãy bán hoặc sử dụng bớt vật phẩm. | Rule đã chốt cho prototype phase đầu. |
| storage_state_04 | Blocked For Collect | Hành động thu nhận bị chặn vì kho đầy. | Người chơi cố thực hiện collect khi Full. | Phải rời thao tác, vào kho hoặc popup bán. | Kho đã đầy. Vui lòng dọn chỗ trống trước khi tiếp tục. | Có thể coi như một sub-state khi người chơi bấm collect. |
