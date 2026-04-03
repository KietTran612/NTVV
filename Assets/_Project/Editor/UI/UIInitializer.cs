namespace NTVV.Editor.UI
{
    using UnityEngine;
    using UnityEditor;
    using NTVV.UI.Common;
    using NTVV.UI.Styling;
    using System.IO;

    /// <summary>
    /// Editor tool to automate the initialization of the UI Phase 1 system.
    /// Handles directory creation, style asset generation, and scene wiring.
    /// </summary>
    public static class UIInitializer
    {
        [MenuItem("NTVV/UI/Setup Phase 1")]
        public static void SetupUIPhase1()
        {
            Debug.Log("[UIInitializer] Starting Phase 1 UI Setup...");

            // 1. Ensure Folder Structure
            EnsureDirectory("Assets/_Project/Resources/UI/Default"); // New standard
            EnsureDirectory("Assets/_Project/Settings/UI");

            // 2. Setup Style Asset
            UIStyleDataSO activeStyle = SetupDefaultStyle();

            // 3. Scene Wiring
            WirePopupManager(activeStyle);

            // 4. Create Placeholder Prefabs (in Default folder)
            CreatePlaceholderPrefabs("Default");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[UIInitializer] Phase 1 UI Setup Complete!");
        }

        private static void EnsureDirectory(string path)
        {
            string[] folders = path.Split('/');
            string current = "";
            foreach (string folder in folders)
            {
                string parent = current;
                current = string.IsNullOrEmpty(current) ? folder : $"{current}/{folder}";
                if (!AssetDatabase.IsValidFolder(current))
                {
                    AssetDatabase.CreateFolder(parent, folder);
                    Debug.Log($"[UIInitializer] Created directory: {current}");
                }
            }
        }

        private static UIStyleDataSO SetupDefaultStyle()
        {
            string stylePath = "Assets/_Project/Settings/UI/DefaultFarmStyle.asset";
            UIStyleDataSO style = AssetDatabase.LoadAssetAtPath<UIStyleDataSO>(stylePath);

            if (style == null)
            {
                style = ScriptableObject.CreateInstance<UIStyleDataSO>();
                style.themeFolderName = "Default"; // Set folder binding
                // Set default colors matching the discussion
                style.PrimaryActionColor = new Color(1f, 0.65f, 0f); // Orange #FFA500
                style.CaringActionColor = new Color(0.3f, 0.7f, 0.3f); // Green #4CAF50
                style.WarningColor = new Color(0.9f, 0.2f, 0.2f); // Red
                style.GoldColor = new Color(1f, 0.84f, 0f); // Sun Yellow

                AssetDatabase.CreateAsset(style, stylePath);
                Debug.Log($"[UIInitializer] Created Default Style Asset at: {stylePath}");
            }

            return style;
        }

        private static void WirePopupManager(UIStyleDataSO style)
        {
            PopupManager manager = GameObject.FindFirstObjectByType<PopupManager>();
            if (manager == null)
            {
                Debug.LogWarning("[UIInitializer] PopupManager not found in current scene. Skipping scene wiring.");
                return;
            }

            SerializedObject so = new SerializedObject(manager);
            SerializedProperty styleProp = so.FindProperty("_activeStyle");
            
            if (styleProp != null)
            {
                styleProp.objectReferenceValue = style;
                so.ApplyModifiedProperties();
                Debug.Log("[UIInitializer] Successfully assigned Active Style to PopupManager in scene.");
            }
            else
            {
                Debug.LogError("[UIInitializer] Could not find field '_activeStyle' in PopupManager. Please ensure the field name matches code.");
            }
        }

        private static void CreatePlaceholderPrefabs(string themeFolder)
        {
            string[] screens = { "ShopPopup", "StoragePopup", "QuestPopup" };
            string resourcePath = $"Assets/_Project/Resources/UI/{themeFolder}/";

            foreach (string screen in screens)
            {
                string path = $"{resourcePath}{screen}.prefab";
                if (AssetDatabase.LoadAssetAtPath<GameObject>(path) == null)
                {
                    GameObject go = new GameObject(screen, typeof(RectTransform));
                    // Basic placeholder visually
                    PrefabUtility.SaveAsPrefabAsset(go, path);
                    Object.DestroyImmediate(go);
                    Debug.Log($"[UIInitializer] Created placeholder prefab: {themeFolder}/{screen}");
                }
            }
        }
    }
}
