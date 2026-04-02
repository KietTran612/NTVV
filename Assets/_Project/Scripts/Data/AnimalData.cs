using System;
using UnityEngine;

namespace NTVV.Data
{
    /// <summary>
    /// Thông số thô của một loại vật nuôi (Data Model).
    /// Đã được chuẩn hóa 100% sang camelCase.
    /// </summary>
    [Serializable]
    public class AnimalData
    {
        public string animalId;
        public string animalName;
        public int unlockLevel;
        public string penType;
        public int penCostGold;
        public int buyCostGold;

        [Header("Feeding")]
        public string feedType;
        public float hungerIntervalHours;
        public int feedQtyGrass;
        public int feedQtyWorm;

        [Header("Timing Rules")]
        public float lifetimeAfterMatureDays;

        // Helper Properties for Gameplay Logic
        public float HungerCycleInSeconds => hungerIntervalHours * 3600f;
        public float LifeAfterMatureInSeconds => lifetimeAfterMatureDays * 86400f;
        
        public struct StageInfo
        {
            public float TimeToReachStageInSeconds;
            public int SalePrice;
            public int XPOnSale;
        }

        public StageInfo Stage1 => new StageInfo { TimeToReachStageInSeconds = stage1AgeDays * 86400f, SalePrice = sellStage1Gold, XPOnSale = xpStage1 };
        public StageInfo Stage2 => new StageInfo { TimeToReachStageInSeconds = stage2AgeDays * 86400f, SalePrice = sellStage2Gold, XPOnSale = xpStage2 };
        public StageInfo Stage3 => new StageInfo { TimeToReachStageInSeconds = 0, SalePrice = sellStage3Gold, XPOnSale = xpStage3 };

        [Header("Growth System")]
        public float stage1AgeDays;
        public float stage2AgeDays;
        public float stage3AgeDays;

        [Header("Selling Prices")]
        public int sellStage1Gold;
        public int sellStage2Gold;
        public int sellStage3Gold;

        [Header("Experience Rewards")]
        public int xpStage1;
        public int xpStage2;
        public int xpStage3;

        [TextArea]
        public string notes;
    }
}
