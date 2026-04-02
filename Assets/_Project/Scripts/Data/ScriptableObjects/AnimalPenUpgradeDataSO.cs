using UnityEngine;
using System;
using System.Collections.Generic;

namespace NTVV.Data.ScriptableObjects
{
    [Serializable]
    public struct AnimalPenUpgradeTier
    {
        public int tierLevel;
        public int upgradeCostGold;
        public int maxCapacity;
        public int minLevelToAccess; // -1 for no requirement
    }

    /// <summary>
    /// Config tập trung cho việc nâng cấp chuồng thú.
    /// Một file chứa toàn bộ danh sách các cấp độ nâng cấp.
    /// </summary>
    [CreateAssetMenu(fileName = "Animal Pen Upgrade Config", menuName = "NTVV/Data/Animal Pen Upgrade Config")]
    public class AnimalPenUpgradeDataSO : ScriptableObject
    {
        public List<AnimalPenUpgradeTier> tiers = new List<AnimalPenUpgradeTier>();

        public AnimalPenUpgradeTier GetTier(int currentTier)
        {
            if (tiers == null || currentTier >= tiers.Count) return default;
            return tiers[currentTier];
        }

        public bool HasNextTier(int currentTier)
        {
            return tiers != null && (currentTier + 1) < tiers.Count;
        }
    }
}
