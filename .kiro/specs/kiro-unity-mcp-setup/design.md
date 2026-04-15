# Design Document: kiro-unity-mcp-setup

## Overview

Feature này cập nhật file `.kiro/settings/mcp.json` để Kiro IDE kết nối được với hai MCP server:

1. **Unity MCP Server** — giao thức `streamableHttp`, chạy tại `http://localhost:21750`
2. **Pencil MCP Server** — giao thức `stdio`, chạy qua executable tại `C:\Users\Hoang.H\.pencil\mcp\antigravity\out\mcp-server-windows-x64.exe`

Đây là thay đổi cấu hình thuần túy (config-only). Không có code logic nào được viết mới. Không có file nào khác bị chạm vào ngoài `.kiro/settings/mcp.json`.

---

## Architecture

Kiro IDE đọc `.kiro/settings/mcp.json` khi khởi động để biết danh sách MCP servers cần kết nối. Mỗi entry trong `mcpServers` tương ứng với một server, và Kiro sẽ tự động thiết lập kết nối theo transport type được khai báo.

```
Kiro IDE
  └── reads .kiro/settings/mcp.json
        ├── "unity-mcp"  → streamableHttp → http://localhost:21750
        │                                     └── Unity Editor (com.ivanmurzak.unity.mcp v0.63.4)
        └── "pencil"     → stdio           → mcp-server-windows-x64.exe --app antigravity
                                              └── Pencil/Antigravity design tools
```

Các file sau **không bị thay đổi**:
- `~/.gemini/antigravity/mcp_config.json` (Antigravity/Gemini config)
- `Packages/manifest.json` (Unity package manifest)

---

## Components and Interfaces

### `.kiro/settings/mcp.json` — Cấu trúc JSON cuối cùng

```json
{
  "mcpServers": {
    "unity-mcp": {
      "url": "http://localhost:21750"
    },
    "pencil": {
      "command": "C:\\Users\\Hoang.H\\.pencil\\mcp\\antigravity\\out\\mcp-server-windows-x64.exe",
      "args": ["--app", "antigravity"]
    }
  }
}
```

**Giải thích từng field:**

| Field | Giá trị | Ý nghĩa |
|---|---|---|
| `mcpServers` | object | Root key bắt buộc của Kiro MCP config |
| `unity-mcp` | entry key | Tên định danh server trong Kiro |
| `unity-mcp.url` | `"http://localhost:21750"` | Endpoint của streamableHttp transport |
| `pencil` | entry key | Tên định danh server trong Kiro |
| `pencil.command` | đường dẫn exe | Executable được spawn cho stdio transport |
| `pencil.args` | `["--app", "antigravity"]` | Arguments truyền vào khi spawn process |

---

## Data Models

### Transport Type: StreamableHttp

Dùng cho Unity MCP Server. Kiro kết nối qua HTTP đến một server đang chạy sẵn.

```json
{
  "url": "<http-endpoint>"
}
```

- **Không có** `command` hay `args`
- Server phải đang chạy trước khi Kiro kết nối
- Nếu server chưa chạy, Kiro sẽ báo lỗi kết nối nhưng **không crash** — config vẫn hợp lệ

### Transport Type: Stdio

Dùng cho Pencil MCP Server. Kiro spawn một process con và giao tiếp qua stdin/stdout.

```json
{
  "command": "<path-to-executable>",
  "args": ["<arg1>", "<arg2>"]
}
```

- **Không có** `url`
- Kiro tự spawn process khi cần, tự quản lý lifecycle
- Đường dẫn trong `command` phải dùng double backslash (`\\`) trên Windows vì JSON escape

### Sự khác biệt giữa hai transport

| Tiêu chí | StreamableHttp | Stdio |
|---|---|---|
| Khai báo | `"url"` | `"command"` + `"args"` |
| Server lifecycle | Server tự chạy độc lập | Kiro spawn/kill process |
| Phụ thuộc | Unity Editor phải đang mở | Chỉ cần file exe tồn tại |
| Ví dụ | Unity MCP (`localhost:21750`) | Pencil MCP (exe file) |

---

## Error Handling

| Tình huống | Hành vi mong đợi |
|---|---|
| Unity Editor chưa mở, Unity MCP Server chưa chạy | Kiro báo lỗi kết nối cho entry `unity-mcp`, các server khác vẫn hoạt động bình thường |
| File exe của Pencil không tồn tại tại đường dẫn khai báo | Kiro báo lỗi spawn process cho entry `pencil`, không ảnh hưởng Unity MCP |
| `mcp.json` có syntax error | Kiro không load được bất kỳ MCP server nào — phải sửa JSON trước |
| Cả hai server đều không kết nối được | Kiro vẫn hoạt động bình thường, chỉ không có MCP tools |

**Nguyên tắc:** Lỗi kết nối của một MCP server không làm ảnh hưởng server còn lại và không làm crash Kiro.

---

## Testing Strategy

Feature này là config-only change. Property-based testing không áp dụng vì không có logic code nào để test — chỉ có một JSON file tĩnh.

**Verification sau khi apply config:**

### 1. Kiểm tra JSON hợp lệ

```bash
# Dùng node hoặc python để parse
node -e "JSON.parse(require('fs').readFileSync('.kiro/settings/mcp.json','utf8')); console.log('Valid JSON')"
# hoặc
python -c "import json; json.load(open('.kiro/settings/mcp.json')); print('Valid JSON')"
```

### 2. Kiểm tra Unity MCP kết nối

Sau khi mở Unity Editor (với plugin `com.ivanmurzak.unity.mcp` v0.63.4 đã cài):
- Reload Kiro IDE
- Mở Kiro chat, thử gọi một Unity tool, ví dụ: `list all GameObjects in the scene`
- Nếu Kiro trả về danh sách GameObjects → kết nối thành công

Hoặc kiểm tra trực tiếp:
```bash
curl http://localhost:21750
# Nếu Unity MCP đang chạy sẽ trả về response (không phải connection refused)
```

### 3. Kiểm tra Pencil MCP

- Reload Kiro IDE
- Mở Kiro chat, thử gọi một Pencil tool (ví dụ: mở file `.pen`)
- Nếu Kiro nhận diện được Pencil tools → stdio process spawn thành công

### 4. Kiểm tra không ảnh hưởng Antigravity

```bash
# Đảm bảo file Antigravity không thay đổi
cat ~/.gemini/antigravity/mcp_config.json
# Nội dung phải giữ nguyên như trước khi thực hiện feature
```

### 5. Kiểm tra không ảnh hưởng Unity Manifest

```bash
# Đảm bảo manifest không thay đổi
grep "com.ivanmurzak.unity.mcp" Packages/manifest.json
# Phải trả về: "com.ivanmurzak.unity.mcp": "0.63.4"
```

**Lưu ý:** Không cần unit test hay integration test tự động cho feature này. Verification thủ công sau khi apply là đủ.
