using UnityEngine;
using NTVV.Data;

namespace NTVV.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Animal Data", menuName = "NTVV/Data/Animal Data")]
    public class AnimalDataSO : ItemData
    {
        public AnimalData data;

        [Header("Visual Assets")]
        public Sprite animalIcon;
        public GameObject animalPrefab; // Prefab chứa SpriteAnimation/Spine cho 2.5D
        
        [Header("Audio")]
        public AudioClip sfxHappy;
        public AudioClip sfxHungry;
        public AudioClip sfxMature;
    }
}
