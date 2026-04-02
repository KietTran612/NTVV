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
