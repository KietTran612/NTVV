using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using NTVV.UI.Panels;

/// <summary>
/// Creates StoragePopup hierarchy inside [POPUP_CANVAS]/ModalParent.
/// Run via: Tools > NTVV > Setup StoragePopup
/// </summary>
public class SetupStoragePopup : EditorWindow
{
    [MenuItem("Tools/NTVV/Setup StoragePopup")]
    public static void Setup()
    {
        GameObject modalParent = GameObject.Find("[POPUP_CANVAS]/ModalParent");
        if (modalParent == null) { Debug.LogError("[SetupStorage] ModalParent not found"); return; }

        TMP_FontAsset dosisBold = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
            "Assets/_Project/Fonts/Dosis/Dosis-Bold SDF.asset");

        Sprite bgPlaque     = LoadSprite("Assets/_Project/Art/Sprites/UI/bg_Plaque_Wooden_Atomic.png");
        Sprite storageIcon  = LoadSprite("Assets/_Project/Art/Sprites/UI/Icons/Common/icon_Storage_Atomic.png");
        Sprite closeIcon    = LoadSprite("Assets/_Project/Art/Sprites/UI/btn_Close_Circle_Atomic.png");

        GameObject itemCardPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            "Assets/_Project/Prefabs/UI/Components/InventorySlot.prefab");

        // ── Root: StoragePopup ────────────────────────────────────────────
        GameObject popup = new GameObject("StoragePopup");
        Undo.RegisterCreatedObjectUndo(popup, "Create StoragePopup");
        popup.transform.SetParent(modalParent.transform, false);

        Image popupImg = popup.AddComponent<Image>();
        popupImg.color = HexColor("FFF7E8");

        VerticalLayoutGroup popupVLG = popup.AddComponent<VerticalLayoutGroup>();
        popupVLG.childForceExpandWidth = true;
        popupVLG.childControlWidth = true;
        popupVLG.childControlHeight = false;
        popupVLG.childForceExpandHeight = false;
        popupVLG.spacing = 0f;

        RectTransform popupRT = popup.GetComponent<RectTransform>();
        popupRT.anchorMin = new Vector2(0.5f, 0.5f);
        popupRT.anchorMax = new Vector2(0.5f, 0.5f);
        popupRT.pivot = new Vector2(0.5f, 0.5f);
        popupRT.sizeDelta = new Vector2(900f, 650f);
        popup.SetActive(false);

        // ── Header ────────────────────────────────────────────────────────
        GameObject header = new GameObject("Header");
        Undo.RegisterCreatedObjectUndo(header, "Create Header");
        header.transform.SetParent(popup.transform, false);

        Image headerImg = header.AddComponent<Image>();
        if (bgPlaque != null) { headerImg.sprite = bgPlaque; headerImg.type = Image.Type.Sliced; }
        else headerImg.color = HexColor("8B5E3C");

        HorizontalLayoutGroup headerHLG = header.AddComponent<HorizontalLayoutGroup>();
        headerHLG.padding = new RectOffset(12, 12, 8, 8);
        headerHLG.spacing = 8f;
        headerHLG.childAlignment = TextAnchor.MiddleLeft;
        headerHLG.childControlWidth = true;
        headerHLG.childControlHeight = true;
        headerHLG.childForceExpandWidth = false;
        headerHLG.childForceExpandHeight = false;

        LayoutElement headerLE = header.AddComponent<LayoutElement>();
        headerLE.minHeight = 60f; headerLE.preferredHeight = 60f;

        MakeIcon("StorageIcon_Icon", header.transform, storageIcon, 36f);

        // Title_Label
        GameObject titleGO = MakeTMPLabel("Title_Label", header.transform, "KHO ĐỒ", 24f, Color.white, dosisBold);
        titleGO.GetComponent<LayoutElement>()?.Destroy();
        LayoutElement titleLE = titleGO.AddComponent<LayoutElement>();
        titleLE.flexibleWidth = 1f;

        // Capacity_Label
        MakeTMPLabel("Capacity_Label", header.transform, "0/50", 20f, HexColor("FFD75E"), dosisBold);

        // Close_Button
        MakeButton("Close_Button", header.transform, closeIcon, 40f, 40f);

        // ── TabBar ────────────────────────────────────────────────────────
        GameObject tabBar = new GameObject("TabBar");
        Undo.RegisterCreatedObjectUndo(tabBar, "Create TabBar");
        tabBar.transform.SetParent(popup.transform, false);

        HorizontalLayoutGroup tabHLG = tabBar.AddComponent<HorizontalLayoutGroup>();
        tabHLG.padding = new RectOffset(8, 8, 4, 4);
        tabHLG.spacing = 4f;
        tabHLG.childForceExpandWidth = false;
        tabHLG.childControlWidth = true;
        tabHLG.childControlHeight = true;
        tabHLG.childForceExpandHeight = false;

        LayoutElement tabBarLE = tabBar.AddComponent<LayoutElement>();
        tabBarLE.minHeight = 44f; tabBarLE.preferredHeight = 44f;

        GameObject tabAll     = MakeTabButton("Tab_All_Button",     tabBar.transform, "Tất Cả",   HexColor("69C34D"), dosisBold);
        GameObject tabCrops   = MakeTabButton("Tab_Crops_Button",   tabBar.transform, "Nông Sản", HexColor("FFF7E8"), dosisBold, HexColor("B97A4A"));
        GameObject tabAnimals = MakeTabButton("Tab_Animals_Button", tabBar.transform, "Vật Nuôi", HexColor("FFF7E8"), dosisBold, HexColor("B97A4A"));

        // ── MainArea ──────────────────────────────────────────────────────
        GameObject mainArea = new GameObject("MainArea");
        Undo.RegisterCreatedObjectUndo(mainArea, "Create MainArea");
        mainArea.transform.SetParent(popup.transform, false);

        HorizontalLayoutGroup mainHLG = mainArea.AddComponent<HorizontalLayoutGroup>();
        mainHLG.spacing = 0f;
        mainHLG.childForceExpandWidth = false;
        mainHLG.childControlWidth = true;
        mainHLG.childControlHeight = true;
        mainHLG.childForceExpandHeight = true;

        LayoutElement mainLE = mainArea.AddComponent<LayoutElement>();
        mainLE.flexibleHeight = 1f;

        // ScrollView
        GameObject scrollView = new GameObject("ScrollView");
        Undo.RegisterCreatedObjectUndo(scrollView, "Create ScrollView");
        scrollView.transform.SetParent(mainArea.transform, false);

        ScrollRect scrollRect = scrollView.AddComponent<ScrollRect>();
        LayoutElement scrollLE = scrollView.AddComponent<LayoutElement>();
        scrollLE.flexibleWidth = 1f;

        // Viewport
        GameObject viewport = new GameObject("Viewport");
        Undo.RegisterCreatedObjectUndo(viewport, "Create Viewport");
        viewport.transform.SetParent(scrollView.transform, false);
        Image viewportImg = viewport.AddComponent<Image>();
        viewportImg.color = new Color(1f, 1f, 1f, 0f);
        viewport.AddComponent<RectMask2D>();
        RectTransform viewportRT = viewport.GetComponent<RectTransform>();
        viewportRT.anchorMin = Vector2.zero;
        viewportRT.anchorMax = Vector2.one;
        viewportRT.offsetMin = Vector2.zero;
        viewportRT.offsetMax = Vector2.zero;

        // Content
        GameObject content = new GameObject("Content");
        Undo.RegisterCreatedObjectUndo(content, "Create Content");
        content.transform.SetParent(viewport.transform, false);
        GridLayoutGroup grid = content.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(160f, 200f);
        grid.spacing = new Vector2(10f, 10f);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 5;
        grid.padding = new RectOffset(10, 10, 10, 10);
        ContentSizeFitter contentCSF = content.AddComponent<ContentSizeFitter>();
        contentCSF.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        RectTransform contentRT = content.GetComponent<RectTransform>();
        contentRT.anchorMin = new Vector2(0f, 1f);
        contentRT.anchorMax = new Vector2(1f, 1f);
        contentRT.pivot = new Vector2(0.5f, 1f);
        contentRT.offsetMin = Vector2.zero;
        contentRT.offsetMax = Vector2.zero;

        scrollRect.viewport = viewportRT;
        scrollRect.content = contentRT;
        scrollRect.horizontal = false;
        scrollRect.vertical = true;

        // SellSubPanel
        GameObject sellPanel = new GameObject("SellSubPanel");
        Undo.RegisterCreatedObjectUndo(sellPanel, "Create SellSubPanel");
        sellPanel.transform.SetParent(mainArea.transform, false);

        Image sellPanelImg = sellPanel.AddComponent<Image>();
        sellPanelImg.color = HexColor("F5EDD8");

        VerticalLayoutGroup sellVLG = sellPanel.AddComponent<VerticalLayoutGroup>();
        sellVLG.padding = new RectOffset(12, 12, 12, 12);
        sellVLG.spacing = 8f;
        sellVLG.childForceExpandWidth = true;
        sellVLG.childControlWidth = true;
        sellVLG.childControlHeight = false;
        sellVLG.childForceExpandHeight = false;

        LayoutElement sellLE = sellPanel.AddComponent<LayoutElement>();
        sellLE.minWidth = 220f; sellLE.preferredWidth = 220f;

        sellPanel.SetActive(false);

        // SelectedItem_Label
        MakeTMPLabel("SelectedItem_Label", sellPanel.transform, "Chọn vật phẩm", 20f, HexColor("4FA63A"), dosisBold);

        // Stepper
        GameObject stepper = new GameObject("Stepper");
        Undo.RegisterCreatedObjectUndo(stepper, "Create Stepper");
        stepper.transform.SetParent(sellPanel.transform, false);
        HorizontalLayoutGroup stepperHLG = stepper.AddComponent<HorizontalLayoutGroup>();
        stepperHLG.spacing = 8f;
        stepperHLG.childAlignment = TextAnchor.MiddleCenter;
        stepperHLG.childControlWidth = true;
        stepperHLG.childControlHeight = true;
        stepperHLG.childForceExpandWidth = false;
        stepperHLG.childForceExpandHeight = false;
        LayoutElement stepperLE = stepper.AddComponent<LayoutElement>();
        stepperLE.minHeight = 44f; stepperLE.preferredHeight = 44f;

        // Minus_Button
        GameObject minusBtn = MakeButton("Minus_Button", stepper.transform, null, 40f, 40f);
        minusBtn.GetComponent<Image>().color = HexColor("B97A4A");
        MakeTMPLabelStretched("Minus_Label", minusBtn.transform, "−", 22f, Color.white, dosisBold);

        // Quantity_Label
        GameObject qtyGO = MakeTMPLabel("Quantity_Label", stepper.transform, "1", 22f, Color.white, dosisBold);
        var qtyLE = qtyGO.GetComponent<LayoutElement>() ?? qtyGO.AddComponent<LayoutElement>();
        qtyLE.flexibleWidth = 1f;
        qtyGO.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        qtyGO.GetComponent<TextMeshProUGUI>().color = new Color(0.2f, 0.2f, 0.2f, 1f);

        // Plus_Button
        GameObject plusBtn = MakeButton("Plus_Button", stepper.transform, null, 40f, 40f);
        plusBtn.GetComponent<Image>().color = HexColor("69C34D");
        MakeTMPLabelStretched("Plus_Label", plusBtn.transform, "+", 22f, Color.white, dosisBold);

        // TotalPrice_Label
        GameObject totalPriceGO = MakeTMPLabel("TotalPrice_Label", sellPanel.transform, "0g", 24f, HexColor("FFD75E"), dosisBold);
        totalPriceGO.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        // SellNow_Button
        GameObject sellNowBtn = MakeButton("SellNow_Button", sellPanel.transform, null, 0f, 48f);
        sellNowBtn.GetComponent<Image>().color = HexColor("FFA94D");
        var sellNowLE = sellNowBtn.GetComponent<LayoutElement>() ?? sellNowBtn.AddComponent<LayoutElement>();
        sellNowLE.minHeight = 48f; sellNowLE.preferredHeight = 48f;
        MakeTMPLabelStretched("SellNow_Label", sellNowBtn.transform, "Bán Ngay", 20f, Color.white, dosisBold);

        // ── Footer ────────────────────────────────────────────────────────
        GameObject footer = new GameObject("Footer");
        Undo.RegisterCreatedObjectUndo(footer, "Create Footer");
        footer.transform.SetParent(popup.transform, false);

        Image footerImg = footer.AddComponent<Image>();
        footerImg.color = HexColor("F5EDD8");

        HorizontalLayoutGroup footerHLG = footer.AddComponent<HorizontalLayoutGroup>();
        footerHLG.padding = new RectOffset(12, 12, 8, 8);
        footerHLG.spacing = 8f;
        footerHLG.childAlignment = TextAnchor.MiddleLeft;
        footerHLG.childControlWidth = true;
        footerHLG.childControlHeight = true;
        footerHLG.childForceExpandWidth = false;
        footerHLG.childForceExpandHeight = false;

        LayoutElement footerLE = footer.AddComponent<LayoutElement>();
        footerLE.minHeight = 50f; footerLE.preferredHeight = 50f;

        // SellAll_Button
        GameObject sellAllBtn = MakeButton("SellAll_Button", footer.transform, null, 0f, 40f);
        sellAllBtn.GetComponent<Image>().color = HexColor("FFA94D");
        var sellAllLE = sellAllBtn.GetComponent<LayoutElement>() ?? sellAllBtn.AddComponent<LayoutElement>();
        sellAllLE.flexibleWidth = 1f; sellAllLE.minHeight = 40f;
        MakeTMPLabelStretched("SellAll_Label", sellAllBtn.transform, "Bán Tất Cả", 18f, Color.white, dosisBold);

        // Upgrade_Button
        GameObject upgradeBtn = new GameObject("Upgrade_Button");
        Undo.RegisterCreatedObjectUndo(upgradeBtn, "Create Upgrade_Button");
        upgradeBtn.transform.SetParent(footer.transform, false);
        upgradeBtn.AddComponent<Image>().color = HexColor("69C34D");
        upgradeBtn.AddComponent<Button>();
        VerticalLayoutGroup upgradeVLG = upgradeBtn.AddComponent<VerticalLayoutGroup>();
        upgradeVLG.childAlignment = TextAnchor.MiddleCenter;
        upgradeVLG.childControlWidth = true;
        upgradeVLG.childControlHeight = true;
        LayoutElement upgradeLE = upgradeBtn.AddComponent<LayoutElement>();
        upgradeLE.flexibleWidth = 1f; upgradeLE.minHeight = 40f;
        MakeTMPLabel("Upgrade_Label", upgradeBtn.transform, "Nâng Cấp Kho", 16f, Color.white, dosisBold);
        MakeTMPLabel("UpgradeCost_Label", upgradeBtn.transform, "500g", 14f, HexColor("FFD75E"), dosisBold);

        // ── Wire StoragePanelController ───────────────────────────────────
        StoragePanelController ctrl = popup.AddComponent<StoragePanelController>();
        SerializedObject so = new SerializedObject(ctrl);

        so.FindProperty("_capacityText").objectReferenceValue =
            header.transform.Find("Capacity_Label").GetComponent<TextMeshProUGUI>();
        so.FindProperty("_storageContentContainer").objectReferenceValue = contentRT;
        so.FindProperty("_btnClose").objectReferenceValue =
            header.transform.Find("Close_Button").GetComponent<Button>();
        so.FindProperty("_tabAll").objectReferenceValue = tabAll.GetComponent<Button>();
        so.FindProperty("_tabCrops").objectReferenceValue = tabCrops.GetComponent<Button>();
        so.FindProperty("_tabAnimals").objectReferenceValue = tabAnimals.GetComponent<Button>();
        so.FindProperty("_sellSubPanel").objectReferenceValue = sellPanel;
        so.FindProperty("_selectedItemNameText").objectReferenceValue =
            sellPanel.transform.Find("SelectedItem_Label").GetComponent<TextMeshProUGUI>();
        so.FindProperty("_quantityText").objectReferenceValue =
            sellPanel.transform.Find("Stepper/Quantity_Label").GetComponent<TextMeshProUGUI>();
        so.FindProperty("_totalPriceText").objectReferenceValue =
            sellPanel.transform.Find("TotalPrice_Label").GetComponent<TextMeshProUGUI>();
        so.FindProperty("_btnMinus").objectReferenceValue =
            sellPanel.transform.Find("Stepper/Minus_Button").GetComponent<Button>();
        so.FindProperty("_btnPlus").objectReferenceValue =
            sellPanel.transform.Find("Stepper/Plus_Button").GetComponent<Button>();
        so.FindProperty("_btnSellNow").objectReferenceValue =
            sellPanel.transform.Find("SellNow_Button").GetComponent<Button>();
        so.FindProperty("_btnSellAll").objectReferenceValue =
            footer.transform.Find("SellAll_Button").GetComponent<Button>();
        so.FindProperty("_btnUpgrade").objectReferenceValue =
            footer.transform.Find("Upgrade_Button").GetComponent<Button>();
        so.FindProperty("_upgradeCostText").objectReferenceValue =
            footer.transform.Find("Upgrade_Button/UpgradeCost_Label").GetComponent<TextMeshProUGUI>();
        if (itemCardPrefab != null)
            so.FindProperty("_itemCardPrefab").objectReferenceValue = itemCardPrefab;

        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(popup);
        Debug.Log("[SetupStorage] StoragePopup created and wired successfully.");
    }

    static GameObject MakeTabButton(string name, Transform parent, string label, Color bgColor, TMP_FontAsset font, Color? textColor = null)
    {
        GameObject go = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
        go.transform.SetParent(parent, false);
        go.AddComponent<Image>().color = bgColor;
        go.AddComponent<Button>();
        LayoutElement le = go.AddComponent<LayoutElement>();
        le.flexibleWidth = 1f;
        MakeTMPLabelStretched(label + "_Lbl", go.transform, label, 18f, textColor ?? Color.white, font);
        return go;
    }

    static GameObject MakeIcon(string name, Transform parent, Sprite sprite, float size)
    {
        GameObject go = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
        go.transform.SetParent(parent, false);
        Image img = go.AddComponent<Image>();
        img.raycastTarget = false;
        if (sprite != null) img.sprite = sprite;
        else img.color = new Color(0.8f, 0.8f, 0.8f, 1f);
        LayoutElement le = go.AddComponent<LayoutElement>();
        le.minWidth = size; le.minHeight = size;
        le.preferredWidth = size; le.preferredHeight = size;
        return go;
    }

    static GameObject MakeButton(string name, Transform parent, Sprite sprite, float w, float h)
    {
        GameObject go = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
        go.transform.SetParent(parent, false);
        Image img = go.AddComponent<Image>();
        if (sprite != null) img.sprite = sprite;
        go.AddComponent<Button>();
        if (w > 0 || h > 0)
        {
            LayoutElement le = go.AddComponent<LayoutElement>();
            if (w > 0) { le.minWidth = w; le.preferredWidth = w; }
            if (h > 0) { le.minHeight = h; le.preferredHeight = h; }
        }
        return go;
    }

    static GameObject MakeTMPLabel(string name, Transform parent, string text, float size, Color color, TMP_FontAsset font)
    {
        GameObject go = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
        go.transform.SetParent(parent, false);
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text; tmp.fontSize = size;
        tmp.fontStyle = FontStyles.Bold; tmp.color = color;
        tmp.alignment = TextAlignmentOptions.MidlineLeft;
        if (font != null) tmp.font = font;
        return go;
    }

    static GameObject MakeTMPLabelStretched(string name, Transform parent, string text, float size, Color color, TMP_FontAsset font)
    {
        GameObject go = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
        go.transform.SetParent(parent, false);
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text; tmp.fontSize = size;
        tmp.fontStyle = FontStyles.Bold; tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
        if (font != null) tmp.font = font;
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
        return go;
    }

    static Color HexColor(string hex)
    {
        ColorUtility.TryParseHtmlString("#" + hex, out Color c);
        return c;
    }

    static Sprite LoadSprite(string path)
    {
        Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (s == null) Debug.LogWarning($"[SetupStorage] Sprite not found: {path}");
        return s;
    }
}

// Extension to destroy component cleanly in editor
public static class ComponentExtensions
{
    public static void Destroy(this Component c)
    {
        if (c != null) Object.DestroyImmediate(c);
    }
}
