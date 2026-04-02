namespace NTVV.Data
{
    using UnityEngine;

    /// <summary>
    /// Base ScriptableObject for all game items (Seeds, Crops, Animals, Food).
    /// </summary>
    public abstract class ItemData : ScriptableObject
    {
        [Header("Identity")]
        public string itemId;
        public string itemName;
        public int unlockLevel = 1;
        public Sprite Icon;

        [Header("Visuals")]
        public GameObject Prefab;
    }
}
