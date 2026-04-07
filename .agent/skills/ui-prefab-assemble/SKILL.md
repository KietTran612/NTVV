---
name: ui-prefab-assemble
description: Assembles all UI prefabs by running PrefabAssembler.AssembleAll(). Fixes missing references and auto-wires structure.
---

# UI / Prefab Assemble

## How to Call

```bash
unity-mcp-cli run-tool ui-prefab-assemble --input '{}'
```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

This tool takes no input parameters.

### Input JSON Schema

```json
{
  "type": "object",
  "additionalProperties": false
}
```

## Output

This tool does not return structured output.

