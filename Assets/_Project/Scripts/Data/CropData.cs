using System;
using UnityEngine;

namespace NTVV.Data
{
    /// <summary>
    /// Thông số thô của một loại cây trồng (Data Model).
    /// Đã được chuẩn hóa 100% sang camelCase.
    /// </summary>
    [Serializable]
    public class CropData
    {
        public string cropId;
        public string cropName;
        public int unlockLevel;
        public int seedCostGold;
        public float growTimeMin;
        
        [Header("Growth Phases (%)")]
        public float phase1Pct = 0.25f;
        public float phase2Pct = 0.35f;
        public float phase3Pct = 0.40f;

        [Header("Yield & Economy")]
        public int baseYieldUnits;
        public int sellPriceGold;
        public int xpReward;

        [Header("Shop Display")]
        public Sprite seedIcon;

        [Header("Caring Events (Chance 0-1)")]
        public float weedChancePct;
        public float bugChancePct;
        public float waterChancePct;
        public int maxCareEvents;

        [Header("Timing Rules")]
        public float perfectWindowMin;
        public float postRipeLifeMin;

        // Helper Properties for Gameplay Logic
        public float GrowthTimeInSeconds => growTimeMin * 60f;
        public float PerfectWindowInSeconds => perfectWindowMin * 60f;
        public float LifeAfterRipeInSeconds => postRipeLifeMin * 60f;

        // HP Drain values based on BalanceRules.md
        public float WeedsHPDrain => 2f; 
        public float PestsHPDrain => 3f;
        public float WaterHPDrain => 2f;

        [TextArea]
        public string notes;
    }
}
