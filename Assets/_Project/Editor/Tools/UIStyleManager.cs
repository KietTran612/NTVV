using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using NTVV.UI.Styling;
using NTVV.UI.Common;
using TMPro;

namespace NTVV.Editor.Tools
{
    public class UIStyleManager : EditorWindow
    {
        private const string ATOM_ROOT = "Assets/_Project/Prefabs/UI/Components/";
        private const string STYLE_DATA_PATH = "Assets/_Project/Settings/UI/DefaultFarmStyle.asset";

        [MenuItem("NTVV/Styling/Apply Visual Styles")]
        public static void ApplyVisualStyles()
        {
            AssetDatabase.StartAssetEditing();
            try
            {
                // 1. Load Style Data
                UIStyleDataSO style = AssetDatabase.LoadAssetAtPath<UIStyleDataSO>(STYLE_DATA_PATH);
                if (style == null)
                {
                    Debug.LogError($"[Stylist] Style Data NOT FOUND at {STYLE_DATA_PATH}");
                    return;
                }

                // 2. Process Atomic Prefabs
                ApplyToResourceChip(style);
                
                // Add more components here as we progress...
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            Debug.Log("<color=cyan>[Stylist]</color> Visual Styling Applied successfully!");
        }

        private static void ApplyToResourceChip(UIStyleDataSO style)
        {
            string path = ATOM_ROOT + "UI_Resource_Chip.prefab";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) return;

            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            
            // Ensure Style Applier exists
            UIStyleApplier applier = instance.GetComponent<UIStyleApplier>();
            if (applier == null) applier = instance.AddComponent<UIStyleApplier>();

            // Configure for Resource Chip
            var so = new SerializedObject(applier);
            so.FindProperty("_styleType").enumValueIndex = (int)UIStyleApplier.StyleType.ResourceChip;
            so.FindProperty("_applyLayout").boolValue = true;
            so.ApplyModifiedProperties();

            // Execute Styling
            applier.ApplyStyle(style);

            // Save back to Prefab
            PrefabUtility.SaveAsPrefabAsset(instance, path);
            Object.DestroyImmediate(instance);
            
            Debug.Log($"<color=cyan>[Stylist]</color> Applied skin to: UI_Resource_Chip");
        }
    }
}
