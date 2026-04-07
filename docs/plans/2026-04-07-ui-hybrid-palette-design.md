# 🎨 UI Design Doc: NTVV Hybrid Palette (Tactile x Vibrant)

**Ngày lập**: 2026-04-07
**Trạng thái**: Đang chờ duyệt (Design Section)

## 1. Triết lý Thiết kế (Design Philosophy)
Sự kết hợp giữa **Hướng B (Tactile Farm)** và **Hướng C (Vibrant Party)** nhằm mục tiêu:
- **Hướng B (Nền tảng)**: Sử dụng các chất liệu hữu cơ (Gỗ mộc, Giấy tay) để tạo cảm giác "Nông trại" ấm áp, bền vững.
- **Hướng C (Tương tác)**: Sử dụng màu sắc bồng đều, rực rỡ và viền trắng dày để tạo cảm giác "Game", kích thích người chơi nhấn và thu hoạch.

## 2. Bảng phối màu chi tiết (The Palette)

### A. Lớp Nền tảng (Tactile - Hướng B)
| Thành phần | Tên Màu | Mã Hex | Mô tả Style |
| :--- | :--- | :--- | :--- |
| **Panel Base** | Warm Butter | `#FFF9E5` | Nền giấy hơi nhám, mang lại cảm giác dễ chịu cho mắt. |
| **Panel Border** | Caramel Wood | `#8B5A2B` | Viền gỗ đậm màu, bo tròn mạnh. |
| **Sub-Panel** | Pale Parchment | `#F5F5DC` | Dùng cho các Item Slot hoặc nội dung phân cấp 2. |

### B. Lớp Tương tác (Vibrant - Hướng C)
| Thành phần | Tên Màu | Mã Hex | Mô tả Style |
| :--- | :--- | :--- | :--- |
| **Primary (OK)** | Lush Meadow | `#4CAF50` | Xanh lục rực rỡ nhưng mượt mà. |
| **Upgrade (Alt)** | Honey Glaze | `#FFC107` | Màu mật ong bóng bẩy, cực kỳ bắt mắt. |
| **Cancel/Danger** | Berry Smash | `#E91E63` | Đỏ hồng phong cách trái cây, không quá gắt như đỏ máu. |
| **Info/Link** | Sky Splash | `#00BCD4` | Xanh Cyan tạo cảm giác tin cậy và mới mẻ. |

## 3. Quy chuẩn Thị giác (Visual Standards)
- **Outlines**: Tất cả các Sprite (Nút, Panel) phải có **Viền trắng dày (White Stroke)** từ 2-4px để tạo hiệu ứng "Sticker".
- **Shadows**: Sử dụng đổ bóng mềm (Soft Shadows) màu nâu đậm (#4D3319) thay vì màu đen để giữ tính organic.
- **Typography**: Kết hợp với font **Dosis ExtraBold** màu **Deep Brown (#2D1B08)** để giữ sự ấm áp.

## 4. Prompt Template cho Generation
Khi sử dụng `generate_image`, Agent sẽ tuân thủ cấu trúc prompt:
> "Kawaii farm game [Item Name], [Primary Color] color, tactile [Material] texture, thick white sticker outline, vibrant cartoon style, soft professional shadows, game UI asset, 2D sprite, white background"

---
**Duyệt từng phần:**
- Bạn có thấy bảng màu này hài hoà với thông số "Nông trại" mà bạn tưởng tượng không?
- Bạn có muốn mình điều chỉnh lại mã màu nào đặc biệt (ví dụ: thích màu hồng hơn màu đỏ) không?
