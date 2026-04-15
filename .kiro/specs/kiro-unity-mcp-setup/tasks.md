# Implementation Plan: kiro-unity-mcp-setup

## Overview

Config-only change: cập nhật `.kiro/settings/mcp.json` để Kiro kết nối với Unity MCP Server (streamableHttp) và Pencil MCP Server (stdio), đồng thời đảm bảo không ảnh hưởng đến các file cấu hình khác.

## Tasks

- [x] 1. Verify trạng thái hiện tại trước khi thay đổi
  - Đọc nội dung hiện tại của `.kiro/settings/mcp.json` và xác nhận đây là file rỗng/thiếu config
  - Đọc `Packages/manifest.json` và ghi nhận checksum/nội dung để so sánh sau
  - _Requirements: 3.1, 4.1_

- [ ] 2. Cập nhật `.kiro/settings/mcp.json`
  - Ghi đúng nội dung JSON sau vào file:
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
  - Entry `unity-mcp` chỉ có field `url`, không có `command`/`args` — _Requirements: 1.1, 1.4_
  - Entry `pencil` chỉ có `command` và `args`, không có `url` — _Requirements: 2.1, 2.2, 2.3, 2.4_

- [x] 3. Verify JSON hợp lệ
  - Parse file vừa ghi bằng JSON parser để xác nhận không có syntax error
  - Xác nhận cấu trúc `{ "mcpServers": { "unity-mcp": {...}, "pencil": {...} } }` với đúng 2 entries
  - Xác nhận không có trailing comma, comment, hoặc cú pháp ngoài JSON spec (RFC 8259)
  - _Requirements: 5.1, 5.2, 5.4_

- [ ] 4. Verify Antigravity config không bị thay đổi
  - Đọc `~/.gemini/antigravity/mcp_config.json` và xác nhận nội dung giữ nguyên
  - _Requirements: 3.1, 3.2_

- [ ] 5. Verify Unity manifest không bị thay đổi
  - Đọc `Packages/manifest.json` và xác nhận entry `"com.ivanmurzak.unity.mcp": "0.63.4"` vẫn còn nguyên
  - Xác nhận `scopedRegistries` cho `package.openupm.com` không bị thay đổi
  - _Requirements: 4.1, 4.2, 4.3_
