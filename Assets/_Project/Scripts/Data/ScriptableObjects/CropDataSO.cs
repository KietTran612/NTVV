using UnityEngine;
using NTVV.Data;

namespace NTVV.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Crop Data", menuName = "NTVV/Data/Crop Data")]
    public class CropDataSO : ItemData
    {
        public CropData data;

        [Header("Visual Assets")]
        public Sprite seedIcon;
        public Sprite cropIcon;
        public GameObject growthStagesPrefab; // Prefab chứa các giai đoạn phát triển 2.5D
        
        [Header("Audio")]
        public AudioClip plantSfx;
        public AudioClip harvestSfx;
    }
}
