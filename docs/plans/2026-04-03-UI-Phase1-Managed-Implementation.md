# UI Phase 1: Managed & Data-driven Implementation Plan

> **For Antigravity:** REQUIRED WORKFLOW: Use `.agent/workflows/execute-plan.md` to execute this plan in single-flow mode.

**Goal:** Establish a functional, themeable, and Addressables-ready UI system for NTVV using uGUI placeholders.

**Architecture:** Provider-based asset loading (`IUIAssetProvider`) and ScriptableObject-based styling (`UIStyleData`).

**Tech Stack:** uGUI, TextMeshPro, ScriptableObjects.

---

### Task 1: UI Styling System

**Files:**
- Create: `Assets/_Project/Scripts/UI/Styling/UIStyleDataSO.cs`
- Create: `Assets/_Project/Scripts/UI/Styling/UIStyleApplier.cs`
- Modify: `Assets/_Project/Scripts/UI/Common/PopupManager.cs`

**Step 1: Define UIStyleDataSO**
```csharp
[CreateAssetMenu(menuName = "NTVV/UI/StyleData")]
public class UIStyleDataSO : ScriptableObject {
    public Color PrimaryActionColor = new Color(1f, 0.65f, 0f); // Orange
    public Color CaringActionColor = new Color(0.3f, 0.7f, 0.3f); // Green
    public Sprite DefaultPanelBg;
    public Sprite DefaultButtonBg;
    public TMP_FontAsset MainFont;
}
```

**Step 2: Create UIStyleApplier for automatic updates**
```csharp
public class UIStyleApplier : MonoBehaviour {
    public enum StyleType { PrimaryAction, CaringAction, Background }
    public StyleType type;
}
```

**Step 3: Commit**
`git add ... && git commit -m "feat: add UI styling system basis"`

---

### Task 2: Managed Provider Pattern

**Files:**
- Create: `Assets/_Project/Scripts/UI/Infrastructure/IUIAssetProvider.cs`
- Create: `Assets/_Project/Scripts/UI/Infrastructure/ResourcesUIProvider.cs`

**Step 1: Implement IUIAssetProvider**
Define `LoadPrefab(string key)` and `StyleData` property.

**Step 2: Implement ResourcesUIProvider**
Uses `Resources.Load<GameObject>("UI/" + key)`.

**Step 3: Update PopupManager**
Modify `PopupManager` to hold an `IUIAssetProvider` and use it in `ShowScreen`.

---

### Task 3: World-Space Context Action Panel

**Files:**
- Modify: `Assets/_Project/Scripts/UI/Common/PopupManager.cs`
- Modify: `Assets/_Project/Scripts/UI/Panels/CropActionPanelController.cs`
- Modify: `Assets/_Project/Resources/UI/ContextActionPanel.prefab`

**Step 1: Implement World-to-Screen mapping**
In `PopupManager.ShowContextAction(target)`:
`var screenPos = Camera.main.WorldToScreenPoint(target.transform.position);`
`panel.RectTransform.position = screenPos + offset;`

**Step 2: Apply Auto-Layout to Panel**
Add `VerticalLayoutGroup` or `GridLayoutGroup` to `ContextActionPanel` to handle dynamic button visibility.

---

### Task 4: UI Screens Implementation (Placeholder)

**Files:**
- Create/Update: `Assets/_Project/Resources/UI/HUDTopBar.prefab`
- Create/Update: `Assets/_Project/Resources/UI/ShopPopup.prefab`
- Create/Update: `Assets/_Project/Resources/UI/StoragePopup.prefab`
- Create/Update: `Assets/_Project/Resources/UI/QuestPopup.prefab`

**Step 1: HUD Wiring**
Wire `HUDTopBarController` to the new `HUDTopBar.prefab`.

**Step 2: Modal Base Integration**
Ensure all popups use `UI_Modal_Base` for consistent "Dim" and "Close" behavior.

---

## Verification Plan

### Manual Verification
1. **Theming**: Change `PrimaryActionColor` in `UIStyleDataSO` and verify all Orange buttons change instantly.
2. **Managed Loading**: Move a prefab out of `Resources/UI/` and verify the system logs an error but doesn't crash.
3. **World-to-Screen**: Click different crop tiles and verify the `ContextActionPanel` moves to stay near the clicked tile.
4. **Resizing**: Change Screen Resolution to 2560x1080 (Ultrawide) and verify HUD/Popups scale correctly.
