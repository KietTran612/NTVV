namespace NTVV.Data
{
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// ScriptableObject defining general player level milestones and unlocks.
    /// </summary>
    [CreateAssetMenu(fileName = "SO_PlayerLevelData", menuName = "NTVV/Player/LevelData")]
    public class PlayerLevelData : ScriptableObject
    {
        public List<LevelMilestone> Milestones = new List<LevelMilestone>();
    }

    [System.Serializable]
    public struct LevelMilestone
    {
        public int Level;
        public int XPRequired;
        public string UnlockDescription;
        public int LandExpansionCount;
        public List<string> UnlockBuildings;
        public List<ItemData> UnlockedItems;
    }
}
