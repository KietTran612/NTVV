# Design Doc: Enhanced Persistence Test Data

This design adds a dynamic `testInstructions` field to the persistence system to allow developers and QA to save/load specific test notes or scenarios directly in the JSON save file, with immediate feedback in the `PersistenceTestHarness`.

## User Review Required

> [!IMPORTANT]
> The `testInstructions` field will be part of the PRODUCTION `PlayerSaveData` class. While harmless, it's intended specifically for debugging and testing scenarios in these early milestones.

## Proposed Design

### 1. Data Schema Changes

Modify `NTVV.Core.PlayerSaveData` in `Assets/_Project/Scripts/Core/SaveData.cs` to include a new field:

```csharp
[Serializable]
public class PlayerSaveData
{
    // ... existing fields ...
    public string testInstructions; // New field for test-specific notes
}
```

### 2. GameManager Role

The `GameManager` will act as a bridge, storing the "last known" test instructions in runtime memory.

- **Storage**: Add a `string _currentTestInstructions` field to `GameManager`.
- **Capture**: In `CaptureCurrentState()`, map `data.testInstructions = _currentTestInstructions`.
- **Injection**: In `InitializeCoreSystems()`, map `_currentTestInstructions = data.testInstructions`.

### 3. PersistenceTestHarness UI Update

Modify `PersistenceTestHarness.cs` to distinguish between instructions (how to test) and data (what is currently in the save):

- **Renaming**: Rename current `instructions` to `testGuide`.
- **New Field**: Add `public string instructionsData` as a `[TextArea]`. This will be the field saved to/loaded from disk.
- **Dynamic Feedback**: Update `ManualLoad` to refresh `instructionsData` from the loaded `PlayerSaveData`.

## Data Flow

1. **User Input**: User types "Test scenario: Max gold" into `instructionsData` in the Inspector.
2. **Setup**: Clicking "Add Test Data" pushes `instructionsData` value to `GameManager`.
3. **Persist**: "Manual Save" captures the current state, including the string.
4. **Restore**: "Manual Load" retrieves the JSON, and `PersistenceTestHarness` updates its `instructionsData` field with the stored value.

## Verification Plan

1. **Manual Verification**:
   - Open `PersistenceTestHarness` in the Inspector.
   - Enter a unique string in `instructionsData`.
   - Click "Add Test Data" -> "Trigger MANUAL SAVE".
   - Clear runtime data (Click "Clear Data").
   - Click "Trigger MANUAL LOAD".
   - Verify `instructionsData` is restored to the unique string typed earlier.
   - Verify the JSON output in the console contains the `testInstructions` key.
