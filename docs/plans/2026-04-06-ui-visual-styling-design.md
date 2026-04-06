# Technical Design Doc: UI Visual Styling System

**Topic**: Decorator Layer, StyleApplier, and PrefabAssembler "Create or Verify" Pattern
**Date**: 2026-04-06
**Status**: Approved

---

## 1. Mục tiêu (Goals)

Tách biệt hoàn toàn hai tầng trách nhiệm trong hệ thống UI:
- **Tầng Logic** (đã ổn định): `PrefabAssembler` tạo cấu trúc hierarchy và nối dây Controller.
- **Tầng Visual** (mới): `UIStyleApplier` + `UIStyleDataSO` xử lý màu sắc, sprite, font, layout trang trí.

**Hai tầng này không bao giờ can thiệp vào phần việc của nhau.**

---

## 2. Hệ thống Đặt tên Hoàn chỉnh (Full Naming Convention)

### Functional Children — Hậu tố (Controller territory)
Những object này do `PrefabAssembler` tạo ra và `Controller` quản lý.

| Pattern | Component | Ví dụ |
|---|---|---|
| `Name_Label` | TMP_Text | `Price_Label`, `Name_Label` |
| `Name_Icon` | Image (sprite data) | `Item_Icon`, `Currency_Icon` |
| `Name_Button` | Button | `Buy_Button`, `Close_Button` |
| `Name_Fill` | Image (fillAmount) | `XP_Fill`, `HP_Fill` |
| `Name_Content` | RectTransform (layout) | `Items_Content`, `Slots_Content` |

### Decorator Children — Tiền tố (StyleApplier territory)
Những object này do `UIStyleApplier` tạo ra và quản lý.

| Prefix | Ý nghĩa | Ví dụ |
|---|---|---|
| `bg_` | Background / nền | `bg_Panel`, `bg_Button`, `bg_Card` |
| `shadow_` | Bóng đổ | `shadow_Card`, `shadow_Button` |
| `border_` | Viền trang trí | `border_Frame`, `border_Panel` |
| `overlay_` | Hiệu ứng phủ / shine | `overlay_Shine`, `overlay_Highlight` |
| `fx_` | Hiệu ứng đặc biệt | `fx_Sparkle`, `fx_Glow` |

### Quy tắc bất di bất dịch
- `Controller` chỉ gọi `FindNamed<T>()` với `_Suffix` names → **không bao giờ tìm prefix objects**.
- `StyleApplier` chỉ tìm và chỉnh sửa `prefix_Name` objects → **không bao giờ chạm _Suffix objects**.

---

## 3. Ví dụ Hierarchy đầy đủ

```
ShopEntry_Seed (root)
  ├── ShopEntryController      ← Logic, FindNamed _Suffix
  ├── UIStyleApplier           ← Visual, chỉ tìm prefix_
  │
  ├── bg_Card                  ← StyleApplier: panel background sprite
  ├── shadow_Card              ← StyleApplier: drop shadow sprite
  │
  ├── Item_Icon                ← Controller: item sprite (data)
  ├── Name_Label               ← Controller: item name text
  │
  ├── Price_Row (group)
  │   ├── bg_PriceChip         ← StyleApplier: price chip background
  │   ├── Currency_Icon        ← Controller: gold icon sprite
  │   └── Price_Label          ← Controller: price text
  │
  └── Buy_Button               ← Controller: Button component
      ├── bg_Button            ← StyleApplier: button background sprite
      ├── shadow_Button        ← StyleApplier: button shadow
      └── Buy_Label            ← Controller: button text (FindNamed)
```

---

## 4. PrefabAssembler — "Create or Verify" Pattern

### Vấn đề cũ
`AssembleShopItems()` luôn tạo `new GameObject(...)` → rebuild từ đầu → **xóa sạch mọi decorator đã thêm**.

### Giải pháp: Create or Verify

```csharp
private static void AssembleShopItems()
{
    string path = ATOM_ROOT + "ShopEntry_Seed.prefab";
    var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);

    if (existing == null)
        CreateShopEntryFromScratch(path);   // Lần đầu: tạo mới
    else
        VerifyAndRepairShopEntry(existing, path); // Đã có: chỉ verify
}

private static void VerifyAndRepairShopEntry(GameObject prefab, string path)
{
    var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
    var ctrl = instance.GetComponent<ShopEntryController>();
    if (ctrl == null) { Object.DestroyImmediate(instance); return; }

    var so = new SerializedObject(ctrl);
    bool repaired = false;

    repaired |= VerifyField<TMP_Text>(so, "_name_Label",  instance.transform, "Name_Label");
    repaired |= VerifyField<TMP_Text>(so, "_price_Label", instance.transform, "Price_Label");
    repaired |= VerifyField<Image>   (so, "_item_Icon",   instance.transform, "Item_Icon");
    repaired |= VerifyField<Button>  (so, "_buy_Button",  instance.transform, "Buy_Button");

    if (repaired)
    {
        so.ApplyModifiedProperties();
        PrefabUtility.SaveAsPrefabAsset(instance, path);
        Debug.LogWarning($"[PrefabAssembler] Repaired broken links on: {prefab.name}");
    }

    Object.DestroyImmediate(instance);
}

// Helper: kiểm tra field, nếu null thì tìm và repair, trả true nếu có sửa
private static bool VerifyField<T>(SerializedObject so, string propName,
                                   Transform root, string childName) where T : Component
{
    var prop = so.FindProperty(propName);
    if (prop.objectReferenceValue != null) return false; // OK, không cần sửa

    var found = FindInHierarchy<T>(root, childName);
    if (found != null)
    {
        prop.objectReferenceValue = found;
        return true; // Đã repair
    }

    Debug.LogWarning($"[PrefabAssembler] WARN: Cannot find '{childName}' in '{root.name}'. Link remains null.");
    return false;
}
```

### Nguyên tắc
- **Lần đầu**: tạo đầy đủ structure + wire.
- **Lần sau**: chỉ verify và repair link broken. **Không xóa, không rebuild.**
- Decorator children (`bg_`, `shadow_`...) được **bảo toàn hoàn toàn**.

---

## 5. UIStyleApplier System

### 5a. UIStyleDataSO (ScriptableObject)

```csharp
// Assets/_Project/Scripts/UI/Common/UIStyleDataSO.cs

[CreateAssetMenu(menuName = "NTVV/UI Style Data")]
public class UIStyleDataSO : ScriptableObject
{
    [Header("Decorator Sprites")]
    public List<SpriteStyleEntry> spriteEntries;

    [Header("Decorator Colors")]
    public List<ColorStyleEntry> colorEntries;

    [Header("Button States")]
    public List<ButtonStyleEntry> buttonEntries;

    [Header("Font Mapping")]
    public TMP_FontAsset defaultFont;
    public List<FontStyleEntry> fontEntries; // per element override
}

[System.Serializable]
public class SpriteStyleEntry
{
    public string targetName;   // "bg_Panel", "shadow_Card"
    public Sprite sprite;
    public Color tint = Color.white;
    public Vector4 border;      // 9-slice border (left, bottom, right, top)
}

[System.Serializable]
public class ColorStyleEntry
{
    public string targetName;   // "bg_Button" (nếu chỉ dùng color, không dùng sprite)
    public Color color;
}

[System.Serializable]
public class ButtonStyleEntry
{
    public string targetName;   // "Buy_Button"
    public ColorBlock colorBlock; // Normal/Highlighted/Pressed/Disabled
}

[System.Serializable]
public class FontStyleEntry
{
    public string targetName;   // "Name_Label", "Price_Label"
    public TMP_FontAsset font;
    public float fontSize;
    public FontStyles fontStyle;
}
```

### 5b. UIStyleApplier (MonoBehaviour)

```csharp
// Assets/_Project/Scripts/UI/Common/UIStyleApplier.cs

public class UIStyleApplier : MonoBehaviour
{
    [SerializeField] private UIStyleDataSO _styleData;

    private void Awake() => ApplyStyle();

    public void ApplyStyle()
    {
        if (_styleData == null) return;

        ApplySpriteEntries();
        ApplyColorEntries();
        ApplyButtonEntries();
        ApplyFontEntries();
    }

    private void ApplySpriteEntries()
    {
        foreach (var entry in _styleData.spriteEntries)
        {
            var img = FindOrCreateDecorator(entry.targetName);
            if (img == null) continue;
            img.sprite = entry.sprite;
            img.color = entry.tint;
            // 9-slice
            if (entry.border != Vector4.zero)
            {
                img.type = Image.Type.Sliced;
                // border được set qua sprite import settings
            }
        }
    }

    private Image FindOrCreateDecorator(string name)
    {
        // Tìm existing
        foreach (Transform t in GetComponentsInChildren<Transform>(true))
            if (t.name == name) return t.GetComponent<Image>() ?? t.gameObject.AddComponent<Image>();

        // Không có → tạo mới ở vị trí đúng (TRƯỚC functional children)
        var go = new GameObject(name);
        go.transform.SetParent(transform, false);
        go.transform.SetAsFirstSibling(); // decorator luôn ở dưới cùng (render trước)
        go.AddComponent<RectTransform>().anchorMin = Vector2.zero;
        go.GetComponent<RectTransform>().anchorMax = Vector2.one;
        go.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        go.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        return go.AddComponent<Image>();
    }

    // Safety: không bao giờ chạm _Suffix objects
    private bool IsDecoratorName(string name) =>
        name.StartsWith("bg_") || name.StartsWith("shadow_") ||
        name.StartsWith("border_") || name.StartsWith("overlay_") ||
        name.StartsWith("fx_");
}
```

### 5c. Apply to Prefab (Editor Bake)

```csharp
// Trong Editor script hoặc Custom Inspector
#if UNITY_EDITOR
[ContextMenu("Apply Style to Prefab NOW")]
private void BakeToEditor()
{
    ApplyStyle();
    PrefabUtility.RecordPrefabInstancePropertyModifications(this);
    UnityEditor.EditorUtility.SetDirty(gameObject);
    AssetDatabase.SaveAssets();
    Debug.Log($"[UIStyleApplier] Baked styles to prefab: {gameObject.name}");
}
#endif
```

---

## 6. Cấu trúc File

```
Assets/_Project/
├── Scripts/UI/Common/
│   ├── UIStyleApplier.cs           ← Script chung
│   └── UIStyleDataSO.cs            ← ScriptableObject definition
│
└── Data/UI/Styles/
    ├── Default/
    │   ├── ShopEntry_StyleData.asset
    │   ├── InventorySlot_StyleData.asset
    │   ├── UI_Resource_Chip_StyleData.asset
    │   └── UI_Nav_Button_StyleData.asset
    └── Cartoon/                         ← Theme tương lai
        └── ShopEntry_StyleData.asset
```

---

## 7. Workflow cho Skill khi làm UI một Prefab

```
1. USER chỉ định prefab cần style (ví dụ: ShopEntry_Seed)
2. USER cung cấp mô tả: văn bản / ảnh mockup / cả hai
3. AI phân tích:
   a. Đọc hierarchy hiện tại của prefab
   b. Xác định decorator nào cần: bg_Card, shadow_Card, bg_Button...
   c. Trích xuất màu sắc, sprite cần thiết từ mô tả/ảnh
4. AI tạo/cập nhật [PrefabName]_StyleData.asset
5. Gán StyleData vào UIStyleApplier trên prefab
6. [Apply to Prefab NOW] → bake vào Editor để xem liền
7. Verify: kiểm tra các _Suffix links còn nguyên vẹn không
```

---

## 8. Đảm bảo An toàn (Safety Matrix)

| Hành động | PrefabAssembler | UIStyleApplier |
|---|---|---|
| Tạo `_Label`, `_Icon`, `_Button` | ✅ | ❌ Không bao giờ |
| Wire [SerializeField] Controller | ✅ | ❌ Không bao giờ |
| Rebuild structure nếu đã tồn tại | ❌ Không bao giờ | ❌ Không bao giờ |
| Tạo `bg_`, `shadow_`, `border_`... | ❌ | ✅ |
| Set sprite/color/font decorator | ❌ | ✅ |
| Tìm/chạm `_Suffix` children | ✅ (verify only) | ❌ Không bao giờ |
| Tìm/chạm `prefix_` children | ❌ | ✅ |

---

## 9. Các trường hợp đặc biệt (Edge Cases)

### 9a. PrefabAssembler chạy lại sau khi đã style
→ **Verify mode** chỉ kiểm tra `_Suffix` links. `bg_`, `shadow_` không bị chạm.
→ Safe ✅

### 9b. UIStyleApplier tìm không thấy decorator đã đặt tên
→ `FindOrCreateDecorator()` tự tạo object mới với tên đó.
→ Safe ✅

### 9c. Đổi Theme (Default → Cartoon)
→ Chỉ cần đổi `_styleData` reference sang `ShopEntry_StyleData_Cartoon.asset`.
→ Nhấn [Apply to Prefab NOW].
→ Controller links không bị ảnh hưởng ✅

### 9d. Controller thêm field mới sau khi đã style
→ PrefabAssembler chạy Verify mode → tìm thấy field mới null → repair và warn.
→ StyleApplier không liên quan ✅
