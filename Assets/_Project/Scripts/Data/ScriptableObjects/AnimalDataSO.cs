using UnityEngine;
using NTVV.Data;

namespace NTVV.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Animal Data", menuName = "NTVV/Data/Animal Data")]
    public class AnimalDataSO : ItemData
    {
        public AnimalData data;

        [Header("Growth Assets (Required)")]
        [Tooltip("Assign 3 stages: Baby, Stage 2, Mature")]
        public Sprite[] stageSprites; 
        public Sprite deadSprite;

        [Header("Production Visuals")]
        public Sprite readyToCollectIcon; // Icon shown when egg/milk is ready

        [Header("Audio")]
        public AudioClip sfxHappy;
        public AudioClip sfxHungry;
        public AudioClip sfxMature;
        public AudioClip sfxCollect;
    }
}
