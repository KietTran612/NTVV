using UnityEngine;
using UnityEditor;

/// <summary>
/// Fix TopHUDBar RectTransform: anchor top-stretch, height=80
/// Run via: Tools > NTVV > Fix TopHUDBar Layout
/// </summary>
public class FixTopHUDBarLayout : EditorWindow
{
    [MenuItem("Tools/NTVV/Fix TopHUDBar Layout")]
    public static void Fix()
    {
        GameObject topHUDBar = GameObject.Find("[HUD_CANVAS]/TopHUDBar");
        if (topHUDBar == null)
        {
            Debug.LogError("[FixTopHUDBarLayout] TopHUDBar not found");
            return;
        }

        RectTransform rt = topHUDBar.GetComponent<RectTransform>();
        if (rt == null)
        {
            Debug.LogError("[FixTopHUDBarLayout] No RectTransform on TopHUDBar");
            return;
        }

        Undo.RecordObject(rt, "Fix TopHUDBar Layout");

        // Anchor: top-stretch (anchorMin=(0,1), anchorMax=(1,1))
        rt.anchorMin = new Vector2(0f, 1f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);

        // Height = 80, full width
        rt.offsetMin = new Vector2(0f, -80f);
        rt.offsetMax = new Vector2(0f, 0f);

        EditorUtility.SetDirty(rt);
        Debug.Log("[FixTopHUDBarLayout] TopHUDBar RectTransform fixed: anchor top-stretch, height=80");

        // Also fix LevelChip Image color
        GameObject levelChip = GameObject.Find("[HUD_CANVAS]/TopHUDBar/LevelChip");
        if (levelChip != null)
        {
            var img = levelChip.GetComponent<UnityEngine.UI.Image>();
            if (img != null)
            {
                Undo.RecordObject(img, "Fix LevelChip Color");
                img.color = new Color(0.310f, 0.651f, 0.227f, 1f); // #4FA63A
                EditorUtility.SetDirty(img);
                Debug.Log("[FixTopHUDBarLayout] LevelChip color set to #4FA63A");
            }
        }

        // Fix GoldChip Image color
        GameObject goldChip = GameObject.Find("[HUD_CANVAS]/TopHUDBar/GoldChip");
        if (goldChip != null)
        {
            var img = goldChip.GetComponent<UnityEngine.UI.Image>();
            if (img != null)
            {
                Undo.RecordObject(img, "Fix GoldChip Color");
                img.color = new Color(1f, 0.969f, 0.910f, 1f); // #FFF7E8
                EditorUtility.SetDirty(img);
                Debug.Log("[FixTopHUDBarLayout] GoldChip color set to #FFF7E8");
            }
        }

        // Fix StorageChip Image color
        GameObject storageChip = GameObject.Find("[HUD_CANVAS]/TopHUDBar/StorageChip");
        if (storageChip != null)
        {
            var img = storageChip.GetComponent<UnityEngine.UI.Image>();
            if (img != null)
            {
                Undo.RecordObject(img, "Fix StorageChip Color");
                img.color = new Color(1f, 0.969f, 0.910f, 1f); // #FFF7E8
                EditorUtility.SetDirty(img);
                Debug.Log("[FixTopHUDBarLayout] StorageChip color set to #FFF7E8");
            }
        }

        // Fix TopHUDBar Image color
        var topImg = topHUDBar.GetComponent<UnityEngine.UI.Image>();
        if (topImg != null)
        {
            Undo.RecordObject(topImg, "Fix TopHUDBar Color");
            topImg.color = new Color(1f, 0.969f, 0.910f, 1f); // #FFF7E8
            EditorUtility.SetDirty(topImg);
            Debug.Log("[FixTopHUDBarLayout] TopHUDBar color set to #FFF7E8");
        }

        // Force canvas rebuild
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rt);

        Debug.Log($"[FixTopHUDBarLayout] Done. TopHUDBar rect: {rt.rect}");
    }
}
