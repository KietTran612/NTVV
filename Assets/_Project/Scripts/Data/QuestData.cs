using System;
using UnityEngine;

namespace NTVV.Data
{
    [Serializable]
    public enum QuestActionType
    {
        HarvestCrop,
        CollectProduct, // From animals
        FeedAnimal,
        BuyItem,
        ReachLevel,
        SpendGold,
        UpgradePen,
        UpgradeStorage
    }

    [Serializable]
    public enum QuestUnlockType
    {
        None,
        ShopTab_Animals,
        Building_NewNPC,
        System_Crafting // Future expansion
    }

    [Serializable]
    public struct QuestObjective
    {
        public QuestActionType actionType;
        public string targetId; // e.g. "crop_wheat"
        public int requiredAmount;
        public string description;
        
        [HideInInspector] public int currentAmount;
        public bool IsReached => currentAmount >= requiredAmount;
    }

    [Serializable]
    public struct QuestReward
    {
        public int goldReward;
        public int xpReward;
        public QuestUnlockType unlockType;
        public string unlockId;
    }
}
