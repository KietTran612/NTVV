# Requirements Document

## Introduction

Feature này cấu hình Kiro IDE để kết nối với Unity MCP server và Pencil MCP server trong dự án NTVV (Nông Trại Vui Vẻ). Hiện tại `.kiro/settings/mcp.json` rỗng, khiến Kiro không thể dùng bất kỳ Unity tool nào. Mục tiêu là cập nhật file cấu hình MCP của Kiro để tích hợp đầy đủ với Unity MCP server (streamableHttp tại `http://localhost:21750`) và Pencil MCP server (stdio), đồng thời không làm ảnh hưởng đến cấu hình Antigravity/Gemini hiện có.

## Glossary

- **Kiro_Config**: File `.kiro/settings/mcp.json` — cấu hình MCP servers cho Kiro IDE
- **Unity_MCP_Server**: Server MCP của Unity plugin `com.ivanmurzak.unity.mcp` v0.63.4, chạy tại `http://localhost:21750` theo giao thức streamableHttp
- **Pencil_MCP_Server**: MCP server cho Pencil/Antigravity, chạy dưới dạng stdio process tại `C:\Users\Hoang.H\.pencil\mcp\antigravity\out\mcp-server-windows-x64.exe`
- **Antigravity_Config**: File `~/.gemini/antigravity/mcp_config.json` — cấu hình MCP servers cho Antigravity/Gemini agent, KHÔNG được thay đổi
- **Unity_Manifest**: File `Packages/manifest.json` — danh sách Unity packages, KHÔNG được thay đổi
- **StreamableHttp_Transport**: Giao thức kết nối MCP qua HTTP, dùng field `url` trong config
- **Stdio_Transport**: Giao thức kết nối MCP qua stdin/stdout process, dùng fields `command` và `args` trong config
- **Unity_Tools**: Tập hợp 60+ MCP tools do Unity MCP plugin cung cấp (gameobject-create, assets-find, scene-open, v.v.)

---

## Requirements

### Requirement 1: Cấu hình Unity MCP Server cho Kiro

**User Story:** As a developer, I want Kiro to connect to the Unity MCP server, so that I can use all 60+ Unity tools directly from Kiro IDE while working on the NTVV project.

#### Acceptance Criteria

1. THE Kiro_Config SHALL contain an entry named `"unity-mcp"` với transport streamableHttp trỏ đến `http://localhost:21750`
2. WHEN Kiro_Config được cập nhật, THE Kiro_Config SHALL có đúng format JSON hợp lệ với field `"url": "http://localhost:21750"` bên trong entry `"unity-mcp"`
3. WHEN Kiro IDE load cấu hình, THE Kiro_Config SHALL cho phép Kiro kết nối đến Unity_MCP_Server mà không cần thêm bước cấu hình thủ công nào khác
4. THE Kiro_Config SHALL không chứa field `command` hoặc `args` trong entry `"unity-mcp"` vì đây là streamableHttp transport

---

### Requirement 2: Cấu hình Pencil MCP Server cho Kiro

**User Story:** As a developer, I want Kiro to have access to the Pencil MCP server, so that I can use Pencil design tools from within Kiro IDE with the same configuration as Antigravity.

#### Acceptance Criteria

1. THE Kiro_Config SHALL contain an entry named `"pencil"` với transport stdio trỏ đến `C:\Users\Hoang.H\.pencil\mcp\antigravity\out\mcp-server-windows-x64.exe`
2. WHEN Kiro_Config được cập nhật, THE Kiro_Config SHALL có field `"command"` với giá trị đường dẫn exe của Pencil_MCP_Server
3. THE Kiro_Config SHALL có field `"args"` với giá trị `["--app", "antigravity"]` trong entry `"pencil"`
4. THE Kiro_Config SHALL không chứa field `url` trong entry `"pencil"` vì đây là stdio transport

---

### Requirement 3: Bảo toàn cấu hình Antigravity

**User Story:** As a developer, I want the Antigravity/Gemini configuration to remain unchanged, so that my existing AI agent workflow is not disrupted.

#### Acceptance Criteria

1. THE Antigravity_Config SHALL giữ nguyên toàn bộ nội dung sau khi thực hiện feature này
2. WHEN Kiro_Config được cập nhật, THE Antigravity_Config SHALL không bị sửa đổi, xóa, hoặc ghi đè
3. THE Kiro_Config SHALL là file độc lập, không tham chiếu hay phụ thuộc vào Antigravity_Config

---

### Requirement 4: Bảo toàn Unity Package Manifest

**User Story:** As a developer, I want the Unity package manifest to remain unchanged, so that the Unity project dependencies are not affected.

#### Acceptance Criteria

1. THE Unity_Manifest SHALL giữ nguyên toàn bộ nội dung, bao gồm entry `"com.ivanmurzak.unity.mcp": "0.63.4"` và tất cả dependencies khác
2. WHEN Kiro_Config được cập nhật, THE Unity_Manifest SHALL không bị sửa đổi dưới bất kỳ hình thức nào
3. THE Unity_Manifest SHALL vẫn chứa đầy đủ `scopedRegistries` cho `package.openupm.com` sau khi thực hiện feature này

---

### Requirement 5: Tính hợp lệ của file cấu hình Kiro

**User Story:** As a developer, I want the Kiro MCP config file to be valid and well-formed, so that Kiro can parse and load it without errors.

#### Acceptance Criteria

1. THE Kiro_Config SHALL là JSON hợp lệ có thể parse được bởi bất kỳ JSON parser chuẩn nào
2. THE Kiro_Config SHALL có cấu trúc `{ "mcpServers": { ... } }` với đúng 2 entries: `"unity-mcp"` và `"pencil"`
3. IF Kiro_Config chứa syntax error, THEN THE Kiro_Config SHALL được sửa để trở thành JSON hợp lệ trước khi hoàn thành feature
4. THE Kiro_Config SHALL không chứa trailing commas, comment, hoặc bất kỳ cú pháp nào không hợp lệ theo JSON spec (RFC 8259)

---

### Requirement 6: Khả năng sử dụng Unity Tools từ Kiro

**User Story:** As a developer, I want to use all enabled Unity tools from Kiro, so that I can perform Unity Editor operations (create GameObjects, find assets, open scenes, etc.) directly through Kiro's AI assistant.

#### Acceptance Criteria

1. WHEN Unity Editor đang chạy với plugin `com.ivanmurzak.unity.mcp` v0.63.4 và Unity_MCP_Server đang lắng nghe tại `http://localhost:21750`, THE Kiro_Config SHALL cho phép Kiro gọi các Unity_Tools mà không cần cấu hình thêm
2. WHEN Unity_MCP_Server không chạy, THE Kiro_Config SHALL vẫn hợp lệ và Kiro SHALL không bị crash hay lỗi load config do thiếu kết nối
3. THE Kiro_Config SHALL hỗ trợ tất cả Unity_Tools đã được enable trong project config, bao gồm nhưng không giới hạn: `gameobject-create`, `assets-find`, `scene-open`, `console-get-logs`, `assets-prefab-create`
