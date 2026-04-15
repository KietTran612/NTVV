# Seed Shop UI Implementation Plan

> **For Antigravity:** REQUIRED WORKFLOW: Use `.agent/workflows/execute-plan.md` to execute this plan in single-flow mode.

**Goal:** Build the complete Seed Shop popup UI — hierarchy, prefab, controller updates, and seed icons — matching the approved design.

**Architecture:** `Shop_Popup` (empty in scene) will be fully populated from the existing `ShopPopup.prefab`. `ShopEntry_Seed.prefab` is rebuilt with stepper (−/+) and `ShopEntryController` is updated. `ShopPanelController` is updated to pass quantity to buy logic. Missing seed icons (Wheat, Corn, Tomato, Strawberry, Pumpkin) are generated via AI and placed in `Assets/_Project/Art/Sprites/UI/Icons/Crops/`.

**Tech Stack:** Unity uGUI, TextMeshPro, C# ScriptableObjects (`GameDataRegistrySO`), `@ui-standardization` naming convention, AI image generation for missing icons.

---

## Task 1: Update `ShopEntryController.cs` — Add Stepper Support

**Files:**
- Modify: `Assets/_Project/Scripts/UI/Panels/ShopEntryController.cs`

**Step 1: Read the current file**
Read `Assets/_Project/Scripts/UI/Panels/ShopEntryController.cs` to understand current fields.

**Step 2: Replace with updated version**

Add `_decrease_Button`, `_increase_Button`, `_qty_Label` fields and quantity logic:

```csharp
namespace NTVV.UI.Panels
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    /// <summary>
    /// Controller for an individual item entry in the Shop.
    /// Follows strict ui-standardization suffix naming (_Label, _Icon, _Button).
    /// Includes stepper (−/+) for quantity selection.
    /// </summary>
    public class ShopEntryController : MonoBehaviour
    {
        [Header("UI Standardized Components")]
        [SerializeField] private TMP_Text _name_Label;
        [SerializeField] private TMP_Text _price_Label;
        [SerializeField] private Image _item_Icon;
        [SerializeField] private Button _buy_Button;
        [SerializeField] private Button _decrease_Button;
        [SerializeField] private Button _increase_Button;
        [SerializeField] private TMP_Text _qty_Label;

        private const int MinQty = 1;
        private const int MaxQty = 99;

        private string _itemId;
        private int _unitPrice;
        private int _qty = 1;
        private System.Action<string, int> _onBuyClicked;

        private void Awake()
        {
            if (_name_Label == null) _name_Label = FindNamed<TMP_Text>("Name_Label");
            if (_price_Label == null) _price_Label = FindNamed<TMP_Text>("Price_Label");
            if (_item_Icon == null) _item_Icon = FindNamed<Image>("Item_Icon");
            if (_buy_Button == null) _buy_Button = FindNamed<Button>("Buy_Button");
            if (_decrease_Button == null) _decrease_Button = FindNamed<Button>("Decrease_Button");
            if (_increase_Button == null) _increase_Button = FindNamed<Button>("Increase_Button");
            if (_qty_Label == null) _qty_Label = FindNamed<TMP_Text>("Qty_Label");

            _buy_Button?.onClick.AddListener(HandleBuyClick);
            _decrease_Button?.onClick.AddListener(HandleDecrease);
            _increase_Button?.onClick.AddListener(HandleIncrease);
        }

        public void Initialize(string id, string name, int unitPrice, Sprite icon, System.Action<string, int> onBuy)
        {
            _itemId = id;
            _unitPrice = unitPrice;
            _qty = 1;
            _onBuyClicked = onBuy;

            if (_name_Label != null) _name_Label.text = name.ToUpper();
            if (_item_Icon != null) _item_Icon.sprite = icon;
            RefreshQtyUI();
        }

        private void HandleDecrease()
        {
            if (_qty > MinQty) { _qty--; RefreshQtyUI(); }
        }

        private void HandleIncrease()
        {
            if (_qty < MaxQty) { _qty++; RefreshQtyUI(); }
        }

        private void RefreshQtyUI()
        {
            if (_qty_Label != null) _qty_Label.text = $"×{_qty}";
            if (_price_Label != null) _price_Label.text = (_unitPrice * _qty).ToString();
            if (_decrease_Button != null) _decrease_Button.interactable = (_qty > MinQty);
        }

        private void HandleBuyClick() => _onBuyClicked?.Invoke(_itemId, _qty);

        private T FindNamed<T>(string exactName) where T : Component
        {
            foreach (Transform t in GetComponentsInChildren<Transform>(true))
                if (t.name == exactName) return t.GetComponent<T>();
            return null;
        }
    }
}
```

**Step 3: Refresh AssetDatabase**
Use `assets-refresh` to trigger recompilation.

**Step 4: Check console for errors**
Use `console-get-logs` filtering for Error. Fix any compilation errors before proceeding.

---

## Task 2: Update `ShopPanelController.cs` — Pass Quantity to Buy Logic

**Files:**
- Modify: `Assets/_Project/Scripts/UI/Panels/ShopPanelController.cs`

**Step 1: Read the current file**
Read `Assets/_Project/Scripts/UI/Panels/ShopPanelController.cs`.

**Step 2: Update `CreateShopItem` and `TryBuySeed` signatures**

Change the `entry.Initialize(...)` call to pass `(string id, int qty) =>` callback:

```csharp
entry.Initialize(id, name, cost, icon, (clickedId, qty) => {
    if (isUnlocked)
    {
        if (isSeed) TryBuySeed(id, cost, qty);
        else TryBuyAnimal(id, cost);
    }
    else
    {
        Debug.LogWarning($"[Shop] Item {name} is locked until level {unlockLevel}");
    }
});
```

Update `TryBuySeed` to accept and use `qty`:

```csharp
private void TryBuySeed(string cropId, int unitCost, int qty)
{
    int totalCost = unitCost * qty;
    if (EconomySystem.Instance != null && EconomySystem.Instance.CanAfford(totalCost))
    {
        EconomySystem.Instance.AddGold(-totalCost);
        StorageSystem.Instance.AddItem(cropId, qty);
        NTVV.Gameplay.Quests.QuestEvents.InvokeActionPerformed(Data.QuestActionType.BuyItem, cropId, qty);
        Debug.Log($"<color=cyan>[Shop]</color> Bought Seed: {cropId} ×{qty}");
    }
    else
    {
        Debug.LogWarning("[Shop] Not enough gold!");
    }
}
```

Also add `_tabAnimals` → rename to `_tabSpecial` and set `interactable = false`:

```csharp
[SerializeField] private Button _tabSeeds;
[SerializeField] private Button _tabSpecial;  // Was _tabAnimals — locked for now

private void OnEnable()
{
    if (_btnClose != null) _btnClose.onClick.AddListener(() => PopupManager.Instance.CloseActiveModal());
    if (_tabSeeds != null) _tabSeeds.onClick.AddListener(() => PopulateShop("Seeds"));
    if (_tabSpecial != null) _tabSpecial.interactable = false;  // Special tab disabled
    // ...rest unchanged
}
```

**Step 3: Refresh and verify no compilation errors.**

---

## Task 3: Rebuild `ShopEntry_Seed.prefab`

**Files:**
- Modify: `Assets/_Project/Prefabs/UI/Components/ShopEntry_Seed.prefab`

**Step 1: Open prefab for editing**
Use `assets-prefab-open` on the ShopEntry_Seed prefab.

**Step 2: Inspect current structure**
Use `gameobject-find` at root to see what's already inside.

**Step 3: Clear existing children and rebuild**

Target hierarchy (card size ~200×260):
```
ShopEntry_Seed (root)         ← Image (bg card kem #FDF4D7, rounded), ShopEntryController
├── Name_Label                ← TMP_Text (tên hạt giống, bold)
├── Item_Icon                 ← Image (sprite hạt, raycastTarget=false)
├── Stepper                   ← HorizontalLayoutGroup
│   ├── Decrease_Button       ← Button "−"
│   ├── Qty_Label             ← TMP_Text "×1"
│   └── Increase_Button       ← Button "+"
└── Buy_Button                ← Button (xanh lá #5BA826)
    ├── Coin_Icon             ← Image (icon vàng, raycastTarget=false)
    └── Price_Label           ← TMP_Text "10"
```

**Step 4: Add all components using `gameobject-component-add`**
- Root: `Image`, `ShopEntryController` (sau khi script đã compile)
- Name_Label: `TextMeshProUGUI`
- Item_Icon: `Image`
- Decrease_Button, Increase_Button: `Button` + child `TextMeshProUGUI`
- Qty_Label: `TextMeshProUGUI`
- Buy_Button: `Button` + Image
- Coin_Icon: `Image`
- Price_Label: `TextMeshProUGUI`

**Step 5: Save and close prefab**
Use `assets-prefab-save` then `assets-prefab-close`.

---

## Task 4: Generate Missing Seed Icons

**Files:**
- Create: `Assets/_Project/Art/Sprites/UI/Icons/Crops/Wheat/icon_Seeds_Wheat_Atomic.png`
- Create: `Assets/_Project/Art/Sprites/UI/Icons/Crops/Corn/icon_Seeds_Corn_Atomic.png`
- Create: `Assets/_Project/Art/Sprites/UI/Icons/Crops/Tomato/icon_Seeds_Tomato_Atomic.png`
- Create: `Assets/_Project/Art/Sprites/UI/Icons/Crops/Strawberry/icon_Seeds_Strawberry_Atomic.png`
- Create: `Assets/_Project/Art/Sprites/UI/Icons/Crops/Pumpkin/icon_Seeds_Pumpkin_Atomic.png`

**Step 1: Generate each icon**

Use `generate_image` for each crop. Style reference: Match `icon_Seeds_Carrot_Atomic.png` — kawaii/chibi 3D render style, transparent background, warm colors.

Prompts:
- Wheat: `"kawaii 3D rendered wheat seeds bundle, golden yellow, cute farming game icon, transparent background, chibi style"`
- Corn: `"kawaii 3D rendered corn seeds, yellow, cute farming game icon, transparent background, chibi style"`
- Tomato: `"kawaii 3D rendered tomato seeds, red round, cute farming game icon, transparent background, chibi style"`
- Strawberry: `"kawaii 3D rendered strawberry seeds, red with seeds, cute farming game icon, transparent background, chibi style"`
- Pumpkin: `"kawaii 3D rendered pumpkin seeds, orange-tan flat oval, cute farming game icon, transparent background, chibi style"`

**Step 2: Save to correct folders**
Save each file to the corresponding `Crops/<CropName>/` folder.

**Step 3: Refresh AssetDatabase**
Use `assets-refresh` so Unity imports the new textures.

---

## Task 5: Build `Shop_Popup` Hierarchy in Scene

**Files:**
- Modify: Scene `SCN_Gameplay.unity` via MCP tools

**Step 1: Open the Shop_Popup prefab**
Use `assets-prefab-open` on `Assets/_Project/Resources/UI/Default/ShopPopup.prefab` (instID: 41882).

**Step 2: Inspect current state**
`gameobject-find` at root.

**Step 3: Build hierarchy using `gameobject-create`**

Create in order:
```
Shop_Popup (root) — already exists
├── Panel_bg              (Image: rounded kem, viền nâu)
├── Header
│   ├── bg_HeaderBar      (Image: nâu #6B3A1F, rounded pill)
│   │   ├── SeedShop_Icon (Image)
│   │   └── Title_Label   (TMP_Text "SEED SHOP")
│   └── Close_Button      (Button, vòng tròn đỏ)
│       └── X_Label       (TMP_Text "✕")
├── TabBar
│   ├── Tab_Seeds_Button  (Button active xanh lá)
│   │   ├── TabSeed_Icon  (Image)
│   │   └── TabSeed_Label (TMP_Text "Seeds")
│   └── Tab_Special_Button (Button interactable=false)
│       ├── TabSpecial_Icon (Image)
│       └── TabSpecial_Label (TMP_Text "Special")
├── Items_Content         (ScrollRect → Viewport → Content → GridLayoutGroup)
└── Footer
    ├── Gold_Chip         (Image nâu + Gold_Icon + GoldBalance_Label)
    └── Refresh_Button    (Button xanh lá + Refresh_Icon + RefreshCost_Label)
```

**Step 4: Add components batch**
- Header: `HorizontalLayoutGroup`
- TabBar: `HorizontalLayoutGroup`
- Items_Content: `ScrollRect`, child `Viewport` → `Image` (mask), child `Content` → `GridLayoutGroup`
- Footer: `HorizontalLayoutGroup`
- Gold_Chip: `Image`, `HorizontalLayoutGroup`
- Buttons: `Button` + `Image`

**Step 5: Wire `ShopPanelController`**
Use `gameobject-component-get` + `object-modify` to set:
- `_goldBalanceLabel` → `Footer/Gold_Chip/GoldBalance_Label`
- `_shopContentContainer` → `Items_Content/Viewport/Content`
- `_btnClose` → `Header/Close_Button`
- `_tabSeeds` → `TabBar/Tab_Seeds_Button`
- `_tabSpecial` → `TabBar/Tab_Special_Button`
- `_shopItemPrefab` → `ShopEntry_Seed` prefab (by GUID: `550b8636beb7cb0419493ed513159c5a`)

**Step 6: Save prefab**
Use `assets-prefab-save` then `assets-prefab-close`.

---

## Task 6: Apply Visual Styling

After structure is built, invoke `@ui-visual-styling` skill to apply:
- Colors: Kem `#FDF4D7`, Nâu `#6B3A1F`, Xanh lá `#5BA826`
- Fonts: Dosis-Bold for all TMP_Text
- Card rounded corners via sprite masking

---

## Task 7: Screenshot Verification

**Step 1:** Take screenshot of `Shop_Popup` in Scene View using `screenshot-scene-view`.
**Step 2:** Compare side-by-side with `Design/Seed shop menu in farming game.png`.
**Step 3:** Note any discrepancies in a brief list and fix before marking complete.

---

## Task 8: Integration Smoke Test

**Step 1:** Enter Play Mode using `editor-application-set-state` (`isPlaying: true`).
**Step 2:** Check console logs — no NullRef or Missing Component errors.
**Step 3:** Exit Play Mode.
