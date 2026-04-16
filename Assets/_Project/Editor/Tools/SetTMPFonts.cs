using UnityEngine;
using UnityEditor;
using TMPro;

/// <summary>
/// Editor utility to set Dosis-Bold SDF font on all TMP labels in TopHUDBar.
/// Run via: Tools > NTVV > Set TMP Fonts (TopHUDBar)
/// </summary>
public class SetTMPFonts : EditorWindow
{
    [MenuItem("Tools/NTVV/Set TMP Fonts (TopHUDBar)")]
    public static void SetFonts()
    {
        TMP_FontAsset dosisBold = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
            "Assets/_Project/Fonts/Dosis/Dosis-Bold SDF.asset");

        if (dosisBold == null)
        {
            Debug.LogError("[SetTMPFonts] Could not find Dosis-Bold SDF.asset");
            return;
        }

        // Find all TMP labels under TopHUDBar
        GameObject topHUDBar = GameObject.Find("[HUD_CANVAS]/TopHUDBar");
        if (topHUDBar == null)
        {
            Debug.LogError("[SetTMPFonts] TopHUDBar not found in scene");
            return;
        }

        var labels = topHUDBar.GetComponentsInChildren<TextMeshProUGUI>(true);
        int count = 0;
        foreach (var label in labels)
        {
            Undo.RecordObject(label, "Set Dosis-Bold Font");
            label.font = dosisBold;
            EditorUtility.SetDirty(label);
            count++;
            Debug.Log($"[SetTMPFonts] Set font on: {label.gameObject.name}");
        }

        Debug.Log($"[SetTMPFonts] Done — set Dosis-Bold on {count} TMP labels.");
    }
}
