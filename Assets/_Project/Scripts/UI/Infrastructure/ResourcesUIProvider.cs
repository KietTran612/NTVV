namespace NTVV.UI.Infrastructure
{
    using UnityEngine;
    using NTVV.UI.Styling;

    /// <summary>
    /// File-based implementation of IUIAssetProvider using Unity's Resources system.
    /// Acts as a bridge to future Addressables migration.
    /// </summary>
    public class ResourcesUIProvider : IUIAssetProvider
    {
        private UIStyleDataSO _style;
        private const string _resourcePrefix = "UI/";

        public ResourcesUIProvider(UIStyleDataSO style)
        {
            _style = style;
        }

        public UIStyleDataSO CurrentStyle => _style;

        /// <summary>
        /// Loads a prefab from Resources using a fallback hierarchy:
        /// 1. UI/{ActiveTheme}/{Key}
        /// 2. UI/Default/{Key}
        /// 3. UI/{Key} (Legacy/Generic)
        /// </summary>
        public GameObject LoadPrefab(string key)
        {
            if (_style == null)
            {
                Debug.LogWarning("[ResourcesUIProvider] ActiveStyle is null, falling back to root UI folder.");
                return Resources.Load<GameObject>($"UI/{key}");
            }

            // Tier 1: Current Theme Overrides
            string themePath = $"UI/{_style.themeFolderName}/{key}";
            GameObject prefab = Resources.Load<GameObject>(themePath);
            if (prefab != null) return prefab;

            // Tier 2: Default Theme Folders
            if (_style.themeFolderName != "Default")
            {
                string defaultPath = $"UI/Default/{key}";
                prefab = Resources.Load<GameObject>(defaultPath);
                if (prefab != null) return prefab;
            }

            // Tier 3: Legacy/Root Root Folder
            string rootPath = $"UI/{key}";
            prefab = Resources.Load<GameObject>(rootPath);
            
            if (prefab == null)
                Debug.LogError($"[ResourcesUIProvider] Failed to load UI prefab '{key}' from any fallback path.");

            return prefab;
        }
    }
}
