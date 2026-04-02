using UnityEngine;
using System;
using System.Collections.Generic;

namespace NTVV.Data.ScriptableObjects
{
    [Serializable]
    public struct StorageUpgradeTier
    {
        public int tierLevel;
        public int upgradeCostGold;
        public int bonusCapacity;
        public int minLevelToAccess; // -1 for no requirement
    }

    [CreateAssetMenu(fileName = "Storage Upgrade Config", menuName = "NTVV/Data/Storage Upgrade Config")]
    public class StorageUpgradeDataSO : ScriptableObject
    {
        public List<StorageUpgradeTier> tiers;

        public StorageUpgradeTier GetTier(int currentTier)
        {
            if (tiers == null || currentTier >= tiers.Count) return default;
            return tiers[currentTier];
        }

        public bool HasNextTier(int currentTier)
        {
            return tiers != null && currentTier < tiers.Count;
        }
    }
}
