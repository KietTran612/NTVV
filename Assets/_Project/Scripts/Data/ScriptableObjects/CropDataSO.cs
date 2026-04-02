using UnityEngine;
using NTVV.Data;

namespace NTVV.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Crop Data", menuName = "NTVV/Data/Crop Data")]
    public class CropDataSO : ItemData
    {
        public CropData data;

        [Header("Growth Assets (Required)")]
        [Tooltip("Assign 4 stages: Seedling, Growing, Large, Ripe")]
        public Sprite[] growthStageSprites; 
        public Sprite deadSprite;

        [Header("Shop & UI")]
        public Sprite seedIcon; // Icon shown in Seed Shop
        public Sprite cropIcon; // Icon shown in Storage/Inventory (item icon)
        
        [Header("Audio (Optional)")]
        public AudioClip plantSfx;
        public AudioClip harvestSfx;
    }
}
