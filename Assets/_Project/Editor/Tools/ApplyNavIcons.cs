using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// Apply Farm and Barn sprites to BottomNav icons.
/// Run via: Tools > NTVV > Apply Nav Icons
/// </summary>
public class ApplyNavIcons : EditorWindow
{
    [MenuItem("Tools/NTVV/Apply Nav Icons")]
    public static void Apply()
    {
        Sprite farmSprite = AssetDatabase.LoadAssetAtPath<Sprite>(
            "Assets/_Project/Art/Sprites/UI/Icons/Nav/icon_Farm_Atomic.png");
        Sprite barnSprite = AssetDatabase.LoadAssetAtPath<Sprite>(
            "Assets/_Project/Art/Sprites/UI/Icons/Nav/icon_Barn_Atomic.png");

        if (farmSprite == null) Debug.LogError("[ApplyNavIcons] icon_Farm_Atomic.png not found");
        if (barnSprite == null) Debug.LogError("[ApplyNavIcons] icon_Barn_Atomic.png not found");

        // Farm
        GameObject farmIcon = GameObject.Find("[HUD_CANVAS]/BottomNav/NavBtn_Farm/NavIcon_Farm");
        if (farmIcon != null)
        {
            Image img = farmIcon.GetComponent<Image>();
            if (img != null)
            {
                Undo.RecordObject(img, "Apply Farm Icon");
                img.sprite = farmSprite;
                img.color = Color.white;
                EditorUtility.SetDirty(img);
                Debug.Log("[ApplyNavIcons] Farm icon applied");
            }
        }
        else Debug.LogError("[ApplyNavIcons] NavIcon_Farm not found");

        // Barn
        GameObject barnIcon = GameObject.Find("[HUD_CANVAS]/BottomNav/NavBtn_Barn/NavIcon_Barn");
        if (barnIcon != null)
        {
            Image img = barnIcon.GetComponent<Image>();
            if (img != null)
            {
                Undo.RecordObject(img, "Apply Barn Icon");
                img.sprite = barnSprite;
                img.color = Color.white;
                EditorUtility.SetDirty(img);
                Debug.Log("[ApplyNavIcons] Barn icon applied");
            }
        }
        else Debug.LogError("[ApplyNavIcons] NavIcon_Barn not found");

        Debug.Log("[ApplyNavIcons] Done.");
    }
}
