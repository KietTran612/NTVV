using UnityEngine;
using UnityEditor;

/// <summary>
/// Setup BottomNav RectTransform: anchor bottom-stretch, height=80
/// Run via: Tools > NTVV > Setup BottomNav Layout
/// </summary>
public class SetupBottomNav : EditorWindow
{
    [MenuItem("Tools/NTVV/Setup BottomNav Layout")]
    public static void Setup()
    {
        GameObject bottomNav = GameObject.Find("[HUD_CANVAS]/BottomNav");
        if (bottomNav == null)
        {
            Debug.LogError("[SetupBottomNav] BottomNav not found");
            return;
        }

        RectTransform rt = bottomNav.GetComponent<RectTransform>();
        if (rt == null)
        {
            Debug.LogError("[SetupBottomNav] No RectTransform on BottomNav");
            return;
        }

        Undo.RecordObject(rt, "Setup BottomNav Layout");

        // Anchor: bottom-stretch (anchorMin=(0,0), anchorMax=(1,0))
        rt.anchorMin = new Vector2(0f, 0f);
        rt.anchorMax = new Vector2(1f, 0f);
        rt.pivot = new Vector2(0.5f, 0f);

        // Height = 80, full width, pos Y = 0
        rt.offsetMin = new Vector2(0f, 0f);
        rt.offsetMax = new Vector2(0f, 80f);

        EditorUtility.SetDirty(rt);

        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rt);

        Debug.Log($"[SetupBottomNav] BottomNav RectTransform set: anchor bottom-stretch, height=80, rect={rt.rect}");
    }
}
