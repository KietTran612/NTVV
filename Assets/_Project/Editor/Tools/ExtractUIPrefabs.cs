using UnityEngine;
using UnityEditor;

/// <summary>
/// Extracts UI popup GameObjects as prefabs into Resources/UI/Default/
/// Run via: Tools > NTVV > Extract UI Prefabs
/// </summary>
public class ExtractUIPrefabs : EditorWindow
{
    [MenuItem("Tools/NTVV/Extract UI Prefabs")]
    public static void Extract()
    {
        // Ensure destination folder exists
        string destFolder = "Assets/_Project/Resources/UI/Default";
        if (!AssetDatabase.IsValidFolder("Assets/_Project/Resources"))
            AssetDatabase.CreateFolder("Assets/_Project", "Resources");
        if (!AssetDatabase.IsValidFolder("Assets/_Project/Resources/UI"))
            AssetDatabase.CreateFolder("Assets/_Project/Resources", "UI");
        if (!AssetDatabase.IsValidFolder(destFolder))
            AssetDatabase.CreateFolder("Assets/_Project/Resources/UI", "Default");

        // Map: (scene path, prefab filename)
        var extractions = new (string scenePath, string prefabName)[]
        {
            ("[POPUP_CANVAS]/ModalParent/ShopPopup",       "ShopPopup"),
            ("[POPUP_CANVAS]/ModalParent/StoragePopup",    "StoragePopup"),
            ("[POPUP_CANVAS]/ModalParent/AnimalDetailPanel", "AnimalDetailPopup"),
            ("[POPUP_CANVAS]/HUDParent/ContextActionPanel", "ContextActionPanel"),
        };

        int successCount = 0;
        foreach (var (scenePath, prefabName) in extractions)
        {
            GameObject go = GameObject.Find(scenePath);
            if (go == null)
            {
                Debug.LogError($"[ExtractUIPrefabs] GameObject not found: {scenePath}");
                continue;
            }

            string prefabPath = $"{destFolder}/{prefabName}.prefab";

            // Check if prefab already exists
            GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            GameObject prefab;

            if (existing != null)
            {
                // Update existing prefab
                prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(go, prefabPath, InteractionMode.AutomatedAction);
                Debug.Log($"[ExtractUIPrefabs] Updated: {prefabPath}");
            }
            else
            {
                // Create new prefab
                prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(go, prefabPath, InteractionMode.AutomatedAction);
                Debug.Log($"[ExtractUIPrefabs] Created: {prefabPath}");
            }

            if (prefab != null) successCount++;
            else Debug.LogError($"[ExtractUIPrefabs] Failed to create prefab: {prefabPath}");
        }

        AssetDatabase.Refresh();
        Debug.Log($"[ExtractUIPrefabs] Done — {successCount}/4 prefabs extracted to {destFolder}");
    }
}
