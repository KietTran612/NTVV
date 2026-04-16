using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using NTVV.UI.Panels;
using NTVV.Data.ScriptableObjects;

/// <summary>
/// Creates ContextActionPanel hierarchy inside [POPUP_CANVAS]/HUDParent.
/// Run via: Tools > NTVV > Setup ContextActionPanel
/// </summary>
public class SetupContextActionPanel : EditorWindow
{
    [MenuItem("Tools/NTVV/Setup ContextActionPanel")]
    public static void Setup()
    {
        GameObject hudParent = GameObject.Find("[POPUP_CANVAS]/HUDParent");
        if (hudParent == null) { Debug.LogError("[SetupCAP] HUDParent not found"); return; }

        TMP_FontAsset dosisBold = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
            "Assets/_Project/Fonts/Dosis/Dosis-Bold SDF.asset");

        GameDataRegistrySO registry = AssetDatabase.LoadAssetAtPath<GameDataRegistrySO>(
            "Assets/_Project/ScriptableObjects/GameDataRegistry.asset");

        // ── Root: ContextActionPanel ──────────────────────────────────────
        GameObject cap = new GameObject("ContextActionPanel");
        Undo.RegisterCreatedObjectUndo(cap, "Create ContextActionPanel");
        cap.transform.SetParent(hudParent.transform, false);

        Image capImg = cap.AddComponent<Image>();
        capImg.color = new Color(1f, 0.969f, 0.910f, 1f); // #FFF7E8

        VerticalLayoutGroup capVLG = cap.AddComponent<VerticalLayoutGroup>();
        capVLG.padding = new RectOffset(12, 12, 12, 12);
        capVLG.spacing = 8f;
        capVLG.childForceExpandWidth = true;
        capVLG.childControlWidth = true;
        capVLG.childControlHeight = false;
        capVLG.childForceExpandHeight = false;

        ContentSizeFitter capCSF = cap.AddComponent<ContentSizeFitter>();
        capCSF.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        RectTransform capRT = cap.GetComponent<RectTransform>();
        capRT.sizeDelta = new Vector2(280f, 0f);
        capRT.pivot = new Vector2(0.5f, 0f);

        cap.SetActive(false);

        // ── Header ────────────────────────────────────────────────────────
        GameObject header = new GameObject("Header");
        Undo.RegisterCreatedObjectUndo(header, "Create Header");
        header.transform.SetParent(cap.transform, false);
        HorizontalLayoutGroup headerHLG = header.AddComponent<HorizontalLayoutGroup>();
        headerHLG.spacing = 8f;
        headerHLG.childAlignment = TextAnchor.MiddleLeft;
        headerHLG.childControlWidth = true;
        headerHLG.childControlHeight = true;
        headerHLG.childForceExpandWidth = false;
        headerHLG.childForceExpandHeight = false;
        LayoutElement headerLE = header.AddComponent<LayoutElement>();
        headerLE.minHeight = 48f;
        headerLE.preferredHeight = 48f;

        // CropIcon_Icon
        GameObject cropIcon = new GameObject("CropIcon_Icon");
        Undo.RegisterCreatedObjectUndo(cropIcon, "Create CropIcon_Icon");
        cropIcon.transform.SetParent(header.transform, false);
        Image cropIconImg = cropIcon.AddComponent<Image>();
        cropIconImg.color = new Color(0.8f, 0.8f, 0.8f, 1f);
        cropIconImg.raycastTarget = false;
        LayoutElement cropIconLE = cropIcon.AddComponent<LayoutElement>();
        cropIconLE.minWidth = 40f; cropIconLE.minHeight = 40f;
        cropIconLE.preferredWidth = 40f; cropIconLE.preferredHeight = 40f;

        // Header_Label
        GameObject headerLabel = new GameObject("Header_Label");
        Undo.RegisterCreatedObjectUndo(headerLabel, "Create Header_Label");
        headerLabel.transform.SetParent(header.transform, false);
        TextMeshProUGUI headerTMP = headerLabel.AddComponent<TextMeshProUGUI>();
        headerTMP.text = "Mảnh Đất";
        headerTMP.fontSize = 22f;
        headerTMP.fontStyle = FontStyles.Bold;
        headerTMP.color = new Color(0.310f, 0.651f, 0.227f, 1f); // #4FA63A
        if (dosisBold != null) headerTMP.font = dosisBold;
        LayoutElement headerLabelLE = headerLabel.AddComponent<LayoutElement>();
        headerLabelLE.flexibleWidth = 1f;

        // ── Action Buttons ────────────────────────────────────────────────
        // (name, label, color hex)
        var btnDefs = new (string name, string label, Color color)[]
        {
            ("Plant_Button",   "Gieo Hạt",      HexColor("69C34D")),
            ("Harvest_Button", "Thu Hoạch",     HexColor("69C34D")),
            ("Reset_Button",   "Dọn Đất",       HexColor("B97A4A")),
            ("Water_Button",   "Tưới Nước",     HexColor("67DCC8")),
            ("Cure_Button",    "Bắt Sâu",       HexColor("FFB547")),
            ("Weed_Button",    "Cắt Cỏ",        HexColor("FFB547")),
            ("Buy_Button",     "Mua Giống",     HexColor("69C34D")),
            ("Feed_Button",    "Cho Ăn",        HexColor("69C34D")),
            ("Sell_Button",    "Bán Ngay",      HexColor("FFA94D")),
            ("Collect_Button", "Thu Sản Phẩm",  HexColor("69C34D")),
            ("Close_Button",   "✕",             HexColor("B97A4A")),
        };

        foreach (var (btnName, btnLabel, btnColor) in btnDefs)
        {
            GameObject btnGO = new GameObject(btnName);
            Undo.RegisterCreatedObjectUndo(btnGO, "Create " + btnName);
            btnGO.transform.SetParent(cap.transform, false);

            Image btnImg = btnGO.AddComponent<Image>();
            btnImg.color = btnColor;
            btnGO.AddComponent<Button>();

            LayoutElement btnLE = btnGO.AddComponent<LayoutElement>();
            btnLE.minHeight = 44f;
            btnLE.preferredHeight = 44f;

            // Label child
            GameObject labelGO = new GameObject(btnName.Replace("_Button", "_Label"));
            Undo.RegisterCreatedObjectUndo(labelGO, "Create label");
            labelGO.transform.SetParent(btnGO.transform, false);
            TextMeshProUGUI tmp = labelGO.AddComponent<TextMeshProUGUI>();
            tmp.text = btnLabel;
            tmp.fontSize = 20f;
            tmp.fontStyle = FontStyles.Bold;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Center;
            if (dosisBold != null) tmp.font = dosisBold;

            // Stretch label to fill button
            RectTransform labelRT = labelGO.GetComponent<RectTransform>();
            labelRT.anchorMin = Vector2.zero;
            labelRT.anchorMax = Vector2.one;
            labelRT.offsetMin = Vector2.zero;
            labelRT.offsetMax = Vector2.zero;
        }

        // ── Wire CropActionPanelController ────────────────────────────────
        CropActionPanelController ctrl = cap.AddComponent<CropActionPanelController>();

        // Use SerializedObject to wire fields
        SerializedObject so = new SerializedObject(ctrl);
        so.FindProperty("_headerText").objectReferenceValue =
            cap.transform.Find("Header/Header_Label").GetComponent<TextMeshProUGUI>();
        so.FindProperty("_plantButton").objectReferenceValue =
            cap.transform.Find("Plant_Button").GetComponent<Button>();
        so.FindProperty("_harvestButton").objectReferenceValue =
            cap.transform.Find("Harvest_Button").GetComponent<Button>();
        so.FindProperty("_resetButton").objectReferenceValue =
            cap.transform.Find("Reset_Button").GetComponent<Button>();
        so.FindProperty("_waterButton").objectReferenceValue =
            cap.transform.Find("Water_Button").GetComponent<Button>();
        so.FindProperty("_cureButton").objectReferenceValue =
            cap.transform.Find("Cure_Button").GetComponent<Button>();
        so.FindProperty("_weedButton").objectReferenceValue =
            cap.transform.Find("Weed_Button").GetComponent<Button>();
        so.FindProperty("_buyButton").objectReferenceValue =
            cap.transform.Find("Buy_Button").GetComponent<Button>();
        so.FindProperty("_feedButton").objectReferenceValue =
            cap.transform.Find("Feed_Button").GetComponent<Button>();
        so.FindProperty("_sellButton").objectReferenceValue =
            cap.transform.Find("Sell_Button").GetComponent<Button>();
        so.FindProperty("_collectButton").objectReferenceValue =
            cap.transform.Find("Collect_Button").GetComponent<Button>();
        so.FindProperty("_closeButton").objectReferenceValue =
            cap.transform.Find("Close_Button").GetComponent<Button>();
        if (registry != null)
            so.FindProperty("_registry").objectReferenceValue = registry;
        so.ApplyModifiedProperties();

        EditorUtility.SetDirty(cap);
        Debug.Log("[SetupCAP] ContextActionPanel created and wired successfully.");
    }

    static Color HexColor(string hex)
    {
        ColorUtility.TryParseHtmlString("#" + hex, out Color c);
        return c;
    }
}
