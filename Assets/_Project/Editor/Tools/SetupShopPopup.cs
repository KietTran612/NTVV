using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using NTVV.UI.Panels;

/// <summary>
/// Creates ShopPopup hierarchy inside [POPUP_CANVAS]/ModalParent.
/// Run via: Tools > NTVV > Setup ShopPopup
/// </summary>
public class SetupShopPopup : EditorWindow
{
    [MenuItem("Tools/NTVV/Setup ShopPopup")]
    public static void Setup()
    {
        GameObject modalParent = GameObject.Find("[POPUP_CANVAS]/ModalParent");
        if (modalParent == null) { Debug.LogError("[SetupShop] ModalParent not found"); return; }

        TMP_FontAsset dosisBold = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
            "Assets/_Project/Fonts/Dosis/Dosis-Bold SDF.asset");

        // Sprites
        Sprite bgPlaque    = LoadSprite("Assets/_Project/Art/Sprites/UI/bg_Plaque_Wooden_Atomic.png");
        Sprite sproutIcon  = LoadSprite("Assets/_Project/Art/Sprites/UI/icon_Sprout_Header_Atomic.png");
        Sprite closeIcon   = LoadSprite("Assets/_Project/Art/Sprites/UI/btn_Close_Circle_Atomic.png");
        Sprite tabLeaf     = LoadSprite("Assets/_Project/Art/Sprites/UI/icon_Tab_Leaf_Atomic.png");
        Sprite tabStar     = LoadSprite("Assets/_Project/Art/Sprites/UI/icon_Tab_Star_Atomic.png");
        Sprite goldIcon    = LoadSprite("Assets/_Project/Art/Sprites/UI/Icons/Common/icon_Gold_Atomic.png");

        // Prefab
        GameObject shopItemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            "Assets/_Project/Prefabs/UI/Components/ShopEntry_Seed.prefab");

        // ── Root: ShopPopup ───────────────────────────────────────────────
        GameObject popup = new GameObject("ShopPopup");
        Undo.RegisterCreatedObjectUndo(popup, "Create ShopPopup");
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
        popupRT.sizeDelta = new Vector2(800f, 600f);

        popup.SetActive(false);

        // ── Header ────────────────────────────────────────────────────────
        GameObject header = new GameObject("Header");
        Undo.RegisterCreatedObjectUndo(header, "Create Header");
        header.transform.SetParent(popup.transform, false);

        Image headerImg = header.AddComponent<Image>();
        if (bgPlaque != null) headerImg.sprite = bgPlaque;
        else headerImg.color = HexColor("8B5E3C");
        headerImg.type = Image.Type.Sliced;

        HorizontalLayoutGroup headerHLG = header.AddComponent<HorizontalLayoutGroup>();
        headerHLG.padding = new RectOffset(12, 12, 8, 8);
        headerHLG.spacing = 8f;
        headerHLG.childAlignment = TextAnchor.MiddleLeft;
        headerHLG.childControlWidth = true;
        headerHLG.childControlHeight = true;
        headerHLG.childForceExpandWidth = false;
        headerHLG.childForceExpandHeight = false;

        LayoutElement headerLE = header.AddComponent<LayoutElement>();
        headerLE.minHeight = 60f;
        headerLE.preferredHeight = 60f;

        // ShopIcon_Icon
        GameObject shopIcon = MakeIcon("ShopIcon_Icon", header.transform, sproutIcon, 36f);

        // Title_Label
        GameObject titleGO = new GameObject("Title_Label");
        Undo.RegisterCreatedObjectUndo(titleGO, "Create Title_Label");
        titleGO.transform.SetParent(header.transform, false);
        TextMeshProUGUI titleTMP = titleGO.AddComponent<TextMeshProUGUI>();
        titleTMP.text = "SEED SHOP";
        titleTMP.fontSize = 24f;
        titleTMP.fontStyle = FontStyles.Bold;
        titleTMP.color = Color.white;
        titleTMP.alignment = TextAlignmentOptions.MidlineLeft;
        if (dosisBold != null) titleTMP.font = dosisBold;
        LayoutElement titleLE = titleGO.AddComponent<LayoutElement>();
        titleLE.flexibleWidth = 1f;

        // Close_Button
        GameObject closeBtn = MakeButton("Close_Button", header.transform, closeIcon, 40f, 40f);

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
        tabBarLE.minHeight = 44f;
        tabBarLE.preferredHeight = 44f;

        // Tab_Seeds_Button
        GameObject tabSeeds = new GameObject("Tab_Seeds_Button");
        Undo.RegisterCreatedObjectUndo(tabSeeds, "Create Tab_Seeds_Button");
        tabSeeds.transform.SetParent(tabBar.transform, false);
        Image tabSeedsImg = tabSeeds.AddComponent<Image>();
        tabSeedsImg.color = HexColor("69C34D");
        tabSeeds.AddComponent<Button>();
        LayoutElement tabSeedsLE = tabSeeds.AddComponent<LayoutElement>();
        tabSeedsLE.flexibleWidth = 1f;
        HorizontalLayoutGroup tabSeedsHLG = tabSeeds.AddComponent<HorizontalLayoutGroup>();
        tabSeedsHLG.childAlignment = TextAnchor.MiddleCenter;
        tabSeedsHLG.spacing = 6f;
        tabSeedsHLG.childControlWidth = true;
        tabSeedsHLG.childControlHeight = true;
        // Icon + Label
        MakeIcon("TabSeeds_Icon", tabSeeds.transform, tabLeaf, 24f);
        MakeTMPLabel("TabSeeds_Label", tabSeeds.transform, "Hạt Giống", 18f, Color.white, dosisBold);

        // Tab_Special_Button
        GameObject tabSpecial = new GameObject("Tab_Special_Button");
        Undo.RegisterCreatedObjectUndo(tabSpecial, "Create Tab_Special_Button");
        tabSpecial.transform.SetParent(tabBar.transform, false);
        Image tabSpecialImg = tabSpecial.AddComponent<Image>();
        tabSpecialImg.color = HexColor("AAAAAA");
        Button tabSpecialBtn = tabSpecial.AddComponent<Button>();
        tabSpecialBtn.interactable = false;
        LayoutElement tabSpecialLE = tabSpecial.AddComponent<LayoutElement>();
        tabSpecialLE.flexibleWidth = 1f;
        HorizontalLayoutGroup tabSpecialHLG = tabSpecial.AddComponent<HorizontalLayoutGroup>();
        tabSpecialHLG.childAlignment = TextAnchor.MiddleCenter;
        tabSpecialHLG.spacing = 6f;
        tabSpecialHLG.childControlWidth = true;
        tabSpecialHLG.childControlHeight = true;
        MakeIcon("TabSpecial_Icon", tabSpecial.transform, tabStar, 24f);
        MakeTMPLabel("TabSpecial_Label", tabSpecial.transform, "Đặc Biệt", 18f, HexColor("888888"), dosisBold);

        // ── ScrollView ────────────────────────────────────────────────────
        GameObject scrollView = new GameObject("ScrollView");
        Undo.RegisterCreatedObjectUndo(scrollView, "Create ScrollView");
        scrollView.transform.SetParent(popup.transform, false);

        ScrollRect scrollRect = scrollView.AddComponent<ScrollRect>();
        LayoutElement scrollLE = scrollView.AddComponent<LayoutElement>();
        scrollLE.flexibleHeight = 1f;

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
        grid.cellSize = new Vector2(180f, 220f);
        grid.spacing = new Vector2(12f, 12f);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 4;
        grid.padding = new RectOffset(12, 12, 12, 12);
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
        footerLE.minHeight = 50f;
        footerLE.preferredHeight = 50f;

        // GoldChip in Footer
        GameObject goldChip = new GameObject("GoldChip");
        Undo.RegisterCreatedObjectUndo(goldChip, "Create GoldChip");
        goldChip.transform.SetParent(footer.transform, false);
        Image goldChipImg = goldChip.AddComponent<Image>();
        goldChipImg.color = HexColor("B97A4A");
        HorizontalLayoutGroup goldChipHLG = goldChip.AddComponent<HorizontalLayoutGroup>();
        goldChipHLG.padding = new RectOffset(8, 8, 4, 4);
        goldChipHLG.spacing = 6f;
        goldChipHLG.childAlignment = TextAnchor.MiddleLeft;
        goldChipHLG.childControlWidth = true;
        goldChipHLG.childControlHeight = true;
        goldChipHLG.childForceExpandWidth = false;
        goldChipHLG.childForceExpandHeight = false;
        ContentSizeFitter goldChipCSF = goldChip.AddComponent<ContentSizeFitter>();
        goldChipCSF.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        goldChipCSF.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        MakeIcon("GoldIcon_Icon", goldChip.transform, goldIcon, 24f);
        MakeTMPLabel("GoldBalance_Label", goldChip.transform, "0", 20f, HexColor("FFD75E"), dosisBold);

        // ── Wire ShopPanelController ──────────────────────────────────────
        ShopPanelController ctrl = popup.AddComponent<ShopPanelController>();
        SerializedObject so = new SerializedObject(ctrl);
        so.FindProperty("_goldBalanceLabel").objectReferenceValue =
            footer.transform.Find("GoldChip/GoldBalance_Label").GetComponent<TextMeshProUGUI>();
        so.FindProperty("_shopContentContainer").objectReferenceValue = contentRT;
        so.FindProperty("_btnClose").objectReferenceValue = closeBtn.GetComponent<Button>();
        so.FindProperty("_tabSeeds").objectReferenceValue = tabSeeds.GetComponent<Button>();
        so.FindProperty("_tabSpecial").objectReferenceValue = tabSpecial.GetComponent<Button>();
        if (shopItemPrefab != null)
            so.FindProperty("_shopItemPrefab").objectReferenceValue = shopItemPrefab;
        so.ApplyModifiedProperties();

        EditorUtility.SetDirty(popup);
        Debug.Log("[SetupShop] ShopPopup created and wired successfully.");
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
        LayoutElement le = go.AddComponent<LayoutElement>();
        le.minWidth = w; le.minHeight = h;
        le.preferredWidth = w; le.preferredHeight = h;
        return go;
    }

    static GameObject MakeTMPLabel(string name, Transform parent, string text, float size, Color color, TMP_FontAsset font)
    {
        GameObject go = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
        go.transform.SetParent(parent, false);
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.fontStyle = FontStyles.Bold;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.MidlineLeft;
        if (font != null) tmp.font = font;
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
        if (s == null) Debug.LogWarning($"[SetupShop] Sprite not found: {path}");
        return s;
    }
}
