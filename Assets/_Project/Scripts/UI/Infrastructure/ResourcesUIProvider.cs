namespace NTVV.UI.Infrastructure
{
    using UnityEngine;

    /// <summary>
    /// File-based implementation of IUIAssetProvider using Unity's Resources system.
    /// Acts as a bridge to future Addressables migration.
    /// </summary>
    public class ResourcesUIProvider : IUIAssetProvider
    {
        public GameObject LoadPrefab(string key)
        {
            string path = $"UI/Default/{key}";
            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab == null)
                Debug.LogError($"[ResourcesUIProvider] Failed to load UI prefab '{key}' from '{path}'.");
            return prefab;
        }
    }
}
