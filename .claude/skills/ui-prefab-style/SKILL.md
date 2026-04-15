---
name: ui-prefab-style
description: Bakes visual styles from UIStyleData into all UIStyleAppliers in the active scene or currently opened prefab.
---

# UI / Prefab Style

## How to Call

```bash
unity-mcp-cli run-tool ui-prefab-style --input '{
  "targetObjectName": "string_value"
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool ui-prefab-style --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool ui-prefab-style --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `targetObjectName` | `string` | No | Force apply to target object instead of looking for all. Leave empty to find all in active scene/prefab. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "targetObjectName": {
      "type": "string"
    }
  }
}
```

## Output

This tool does not return structured output.

