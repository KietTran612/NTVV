namespace NTVV.UI.Infrastructure
{
    using UnityEngine;
    using NTVV.UI.Styling;

    /// <summary>
    /// Contract for UI Asset loading. This allows swapping from Resources to Addressables later.
    /// </summary>
    public interface IUIAssetProvider
    {
        /// <summary>
        /// Loads a UI prefab by its logical key (e.g., "Shop", "Storage").
        /// </summary>
        GameObject LoadPrefab(string key);

        /// <summary>
        /// Global style data for the UI system.
        /// </summary>
        UIStyleDataSO CurrentStyle { get; }
    }
}
