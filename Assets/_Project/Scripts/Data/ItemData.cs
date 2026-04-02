namespace NTVV.Data
{
    using UnityEngine;

    /// <summary>
    /// Base ScriptableObject for all game items (Seeds, Crops, Animals, Food).
    /// Standardized to camelCase.
    /// </summary>
    public abstract class ItemData : ScriptableObject
    {
        [Header("Identity")]
        public string itemId;
        public string itemName;
        public int unlockLevel = 1;
        public Sprite icon;

        [Header("World Representation")]
        public GameObject worldPrefab; // Prefab for dropped items or world representation
    }
}
