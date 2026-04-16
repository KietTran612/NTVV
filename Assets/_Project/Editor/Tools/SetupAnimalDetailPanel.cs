using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using NTVV.UI.Panels;

/// <summary>
/// Creates AnimalDetailPanel hierarchy inside [POPUP_CANVAS]/ModalParent.
/// Run via: Tools > NTVV > Setup AnimalDetailPanel
/// </summary>
public class SetupAnimalDetailPanel : EditorWindow
{
    [MenuItem("Tools/NTVV/Setup AnimalDetailPanel")]
    public static void Setup()
    {
        GameObject modalParent = GameObject.Find("[POPUP_CANVAS]/ModalParent");
        if (modalParent == null) { Debug.LogError("[SetupADP] ModalParent not found"); return; }

        TMP_FontAsset dosisBold = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
            "Assets/_Project/Fonts/Dosis/Dosis-Bold SDF.asset");

        // ── Root: AnimalDetailPanel ───────────────────────────────────────
        GameObject panel = new GameObject("AnimalDetailPanel");
        Undo.RegisterCreatedObjectUndo(panel, "Create AnimalDetailPanel");
        panel.transform.SetParent(modalParent.transform, false);

        Image panelImg = panel.AddComponent<Image>();
        panelImg.color = HexColor("FFF7E8");

        VerticalLayoutGroup panelVLG = panel.AddComponent<VerticalLayoutGroup>();
        panelVLG.childForceExpandWidth = true;
        panelVLG.childControlWidth = true;
        panelVLG.childControlHeight = false;
        panelVLG.childForceExpandHeight = false;
        panelVLG.spacing = 0f;

        // anchor right-stretch, width=400
        RectTransform panelRT = panel.GetComponent<RectTransform>();
        panelRT.anchorMin = new Vector2(1f, 0f);
        panelRT.anchorMax = new Vector2(1f, 1f);
        panelRT.pivot = new Vector2(1f, 0.5f);
        panelRT.offsetMin = new Vector2(-400f, 0f);
        panelRT.offsetMax = new Vector2(0f, 0f);

        panel.SetActive(false);

        // ── Header ────────────────────────────────────────────────────────
        GameObject header = new GameObject("Header");
        Undo.RegisterCreatedObjectUndo(header, "Create Header");
        header.transform.SetParent(panel.transform, false);

        Image headerImg = header.AddComponent<Image>();
        headerImg.color = HexColor("4FA63A"); // Farm Green

        VerticalLayoutGroup headerVLG = header.AddComponent<VerticalLayoutGroup>();
        headerVLG.padding = new RectOffset(16, 16, 16, 16);
        headerVLG.childAlignment = TextAnchor.MiddleLeft;
        headerVLG.childControlWidth = true;
        headerVLG.childControlHeight = true;
        headerVLG.childForceExpandWidth = true;
        headerVLG.childForceExpandHeight = false;

        LayoutElement headerLE = header.AddComponent<LayoutElement>();
        headerLE.minHeight = 80f;
        headerLE.preferredHeight = 80f;

        // AnimalName
        GameObject animalNameGO = new GameObject("AnimalName");
        Undo.RegisterCreatedObjectUndo(animalNameGO, "Create AnimalName");
        animalNameGO.transform.SetParent(header.transform, false);
        TextMeshProUGUI animalNameTMP = animalNameGO.AddComponent<TextMeshProUGUI>();
        animalNameTMP.text = "Gà";
        animalNameTMP.fontSize = 26f;
        animalNameTMP.fontStyle = FontStyles.Bold;
        animalNameTMP.color = Color.white;
        animalNameTMP.alignment = TextAlignmentOptions.MidlineLeft;
        if (dosisBold != null) animalNameTMP.font = dosisBold;

        // ── StatusBlock ───────────────────────────────────────────────────
        GameObject statusBlock = new GameObject("StatusBlock");
        Undo.RegisterCreatedObjectUndo(statusBlock, "Create StatusBlock");
        statusBlock.transform.SetParent(panel.transform, false);

        VerticalLayoutGroup statusVLG = statusBlock.AddComponent<VerticalLayoutGroup>();
        statusVLG.padding = new RectOffset(16, 16, 16, 16);
        statusVLG.spacing = 8f;
        statusVLG.childControlWidth = true;
        statusVLG.childControlHeight = true;
        statusVLG.childForceExpandWidth = true;
        statusVLG.childForceExpandHeight = false;

        LayoutElement statusLE = statusBlock.AddComponent<LayoutElement>();
        statusLE.flexibleHeight = 1f;

        // GrowthInfo
        GameObject growthGO = new GameObject("GrowthInfo");
        Undo.RegisterCreatedObjectUndo(growthGO, "Create GrowthInfo");
        growthGO.transform.SetParent(statusBlock.transform, false);
        TextMeshProUGUI growthTMP = growthGO.AddComponent<TextMeshProUGUI>();
        growthTMP.text = "GĐ: 1";
        growthTMP.fontSize = 16f;
        growthTMP.fontStyle = FontStyles.Bold;
        growthTMP.color = HexColor("B97A4A");
        growthTMP.alignment = TextAlignmentOptions.MidlineLeft;
        if (dosisBold != null) growthTMP.font = dosisBold;

        // ── ActionFooter ──────────────────────────────────────────────────
        GameObject actionFooter = new GameObject("ActionFooter");
        Undo.RegisterCreatedObjectUndo(actionFooter, "Create ActionFooter");
        actionFooter.transform.SetParent(panel.transform, false);

        VerticalLayoutGroup footerVLG = actionFooter.AddComponent<VerticalLayoutGroup>();
        footerVLG.padding = new RectOffset(16, 16, 16, 16);
        footerVLG.spacing = 8f;
        footerVLG.childControlWidth = true;
        footerVLG.childControlHeight = false;
        footerVLG.childForceExpandWidth = true;
        footerVLG.childForceExpandHeight = false;

        // FeedButtonGO
        GameObject feedBtnGO = new GameObject("FeedButtonGO");
        Undo.RegisterCreatedObjectUndo(feedBtnGO, "Create FeedButtonGO");
        feedBtnGO.transform.SetParent(actionFooter.transform, false);
        Image feedBtnImg = feedBtnGO.AddComponent<Image>();
        feedBtnImg.color = HexColor("69C34D");
        LayoutElement feedLE = feedBtnGO.AddComponent<LayoutElement>();
        feedLE.minHeight = 52f; feedLE.preferredHeight = 52f;

        // Feed_Button child
        GameObject feedBtn = new GameObject("Feed_Button");
        Undo.RegisterCreatedObjectUndo(feedBtn, "Create Feed_Button");
        feedBtn.transform.SetParent(feedBtnGO.transform, false);
        feedBtn.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0f); // transparent
        feedBtn.AddComponent<Button>();
        RectTransform feedBtnRT = feedBtn.GetComponent<RectTransform>();
        feedBtnRT.anchorMin = Vector2.zero;
        feedBtnRT.anchorMax = Vector2.one;
        feedBtnRT.offsetMin = Vector2.zero;
        feedBtnRT.offsetMax = Vector2.zero;
        MakeTMPLabelStretched("Feed_Label", feedBtn.transform, "Cho Ăn", 20f, Color.white, dosisBold);

        // SellButtonGO
        GameObject sellBtnGO = new GameObject("SellButtonGO");
        Undo.RegisterCreatedObjectUndo(sellBtnGO, "Create SellButtonGO");
        sellBtnGO.transform.SetParent(actionFooter.transform, false);
        Image sellBtnImg = sellBtnGO.AddComponent<Image>();
        sellBtnImg.color = HexColor("FFA94D");
        LayoutElement sellLE = sellBtnGO.AddComponent<LayoutElement>();
        sellLE.minHeight = 52f; sellLE.preferredHeight = 52f;

        // Sell_Button child
        GameObject sellBtn = new GameObject("Sell_Button");
        Undo.RegisterCreatedObjectUndo(sellBtn, "Create Sell_Button");
        sellBtn.transform.SetParent(sellBtnGO.transform, false);
        sellBtn.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        sellBtn.AddComponent<Button>();
        RectTransform sellBtnRT = sellBtn.GetComponent<RectTransform>();
        sellBtnRT.anchorMin = Vector2.zero;
        sellBtnRT.anchorMax = Vector2.one;
        sellBtnRT.offsetMin = Vector2.zero;
        sellBtnRT.offsetMax = Vector2.zero;
        MakeTMPLabelStretched("Sell_Label", sellBtn.transform, "Bán Ngay", 20f, Color.white, dosisBold);

        // ── Wire AnimalDetailPanelController ──────────────────────────────
        AnimalDetailPanelController ctrl = panel.AddComponent<AnimalDetailPanelController>();
        SerializedObject so = new SerializedObject(ctrl);
        so.FindProperty("_animalName").objectReferenceValue = animalNameTMP;
        so.FindProperty("_growthText").objectReferenceValue = growthTMP;
        so.FindProperty("_feedButton").objectReferenceValue = feedBtnGO;
        so.FindProperty("_sellButton").objectReferenceValue = sellBtnGO;
        so.ApplyModifiedProperties();

        EditorUtility.SetDirty(panel);
        Debug.Log("[SetupADP] AnimalDetailPanel created and wired successfully.");
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
}
