using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Creates 5 NavButtons inside BottomNav with correct structure, colors, fonts.
/// Run via: Tools > NTVV > Setup BottomNav Buttons
/// </summary>
public class SetupBottomNavButtons : EditorWindow
{
    [MenuItem("Tools/NTVV/Setup BottomNav Buttons")]
    public static void Setup()
    {
        GameObject bottomNav = GameObject.Find("[HUD_CANVAS]/BottomNav");
        if (bottomNav == null)
        {
            Debug.LogError("[SetupBottomNavButtons] BottomNav not found");
            return;
        }

        TMP_FontAsset dosisBold = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
            "Assets/_Project/Fonts/Dosis/Dosis-Bold SDF.asset");

        if (dosisBold == null)
            Debug.LogWarning("[SetupBottomNavButtons] Dosis-Bold SDF not found, using default font");

        // Sprite paths
        string storagePath = "Assets/_Project/Art/Sprites/UI/Icons/Common/icon_Storage_Atomic.png";
        string sproutPath  = "Assets/_Project/Art/Sprites/UI/icon_Sprout_Header_Atomic.png";
        string starPath    = "Assets/_Project/Art/Sprites/UI/icon_Tab_Star_Atomic.png";

        Sprite storageSprite = AssetDatabase.LoadAssetAtPath<Sprite>(storagePath);
        Sprite sproutSprite  = AssetDatabase.LoadAssetAtPath<Sprite>(sproutPath);
        Sprite starSprite    = AssetDatabase.LoadAssetAtPath<Sprite>(starPath);

        // Button data: (goName, iconName, labelText, sprite)
        var buttons = new (string goName, string iconName, string label, Sprite sprite)[]
        {
            ("NavBtn_Farm",    "NavIcon_Farm",    "Farm",   null),
            ("NavBtn_Storage", "NavIcon_Storage", "Kho",    storageSprite),
            ("NavBtn_Shop",    "NavIcon_Shop",    "Shop",   sproutSprite),
            ("NavBtn_Barn",    "NavIcon_Barn",    "Chuồng", null),
            ("NavBtn_Event",   "NavIcon_Event",   "Event",  starSprite),
        };

        Color cream    = new Color(1f, 0.969f, 0.910f, 1f); // #FFF7E8
        Color brown    = new Color(0.725f, 0.478f, 0.290f, 1f); // #B97A4A

        foreach (var (goName, iconName, label, sprite) in buttons)
        {
            // Create button GO
            GameObject btnGO = new GameObject(goName);
            Undo.RegisterCreatedObjectUndo(btnGO, "Create " + goName);
            btnGO.transform.SetParent(bottomNav.transform, false);

            // Add Image + Button
            Image btnImg = btnGO.AddComponent<Image>();
            btnImg.color = cream;
            btnGO.AddComponent<Button>();

            // Add VerticalLayoutGroup
            VerticalLayoutGroup vlg = btnGO.AddComponent<VerticalLayoutGroup>();
            vlg.childAlignment = TextAnchor.MiddleCenter;
            vlg.spacing = 4f;
            vlg.padding = new RectOffset(4, 4, 4, 4);
            vlg.childControlWidth = true;
            vlg.childControlHeight = true;
            vlg.childForceExpandWidth = false;
            vlg.childForceExpandHeight = false;

            // Create Icon child
            GameObject iconGO = new GameObject(iconName);
            Undo.RegisterCreatedObjectUndo(iconGO, "Create " + iconName);
            iconGO.transform.SetParent(btnGO.transform, false);
            Image iconImg = iconGO.AddComponent<Image>();
            iconImg.raycastTarget = false;
            if (sprite != null) iconImg.sprite = sprite;
            else iconImg.color = new Color(0.8f, 0.8f, 0.8f, 1f); // placeholder grey
            LayoutElement iconLE = iconGO.AddComponent<LayoutElement>();
            iconLE.minWidth = 32f;
            iconLE.minHeight = 32f;
            iconLE.preferredWidth = 32f;
            iconLE.preferredHeight = 32f;

            // Create Label child
            string labelGoName = "NavLabel_" + goName.Replace("NavBtn_", "");
            GameObject labelGO = new GameObject(labelGoName);
            Undo.RegisterCreatedObjectUndo(labelGO, "Create " + labelGoName);
            labelGO.transform.SetParent(btnGO.transform, false);
            TextMeshProUGUI tmp = labelGO.AddComponent<TextMeshProUGUI>();
            tmp.text = label;
            tmp.fontSize = 16f;
            tmp.fontStyle = FontStyles.Bold;
            tmp.color = brown;
            tmp.alignment = TextAlignmentOptions.Center;
            if (dosisBold != null) tmp.font = dosisBold;

            EditorUtility.SetDirty(btnGO);
            Debug.Log($"[SetupBottomNavButtons] Created {goName}");
        }

        EditorUtility.SetDirty(bottomNav);
        Debug.Log("[SetupBottomNavButtons] Done — 5 NavButtons created.");
    }
}
