using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NTVV.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameDataRegistry", menuName = "NTVV/Data/Registry")]
    public class GameDataRegistrySO : ScriptableObject
    {
        [Header("Crops")]
        public List<CropDataSO> crops = new List<CropDataSO>();
        
        [Header("Animals")]
        public List<AnimalDataSO> animals = new List<AnimalDataSO>();

        [Header("Upgrades")]
        public AnimalPenUpgradeDataSO animalPenUpgradeConfig;

        [Header("Quests")]
        public List<QuestDataSO> quests = new List<QuestDataSO>();

        private Dictionary<string, CropDataSO> _cropLookup;
        private Dictionary<string, AnimalDataSO> _animalLookup;
        private Dictionary<string, QuestDataSO> _questLookup;

        public void Initialize()
        {
            // 1. Safe Crop Initialization
            if (crops != null)
            {
                foreach (var c in crops.Where(c => c != null && (c.data == null || string.IsNullOrEmpty(c.data.cropId))))
                    Debug.LogWarning($"[Data Error] Crop asset '{c.name}' is missing a cropId or has null data.");

                _cropLookup = crops
                    .Where(c => c != null && c.data != null && !string.IsNullOrEmpty(c.data.cropId))
                    .ToDictionary(c => c.data.cropId, c => c);
            }
            else _cropLookup = new Dictionary<string, CropDataSO>();

            // 2. Safe Animal Initialization
            if (animals != null)
            {
                foreach (var a in animals.Where(a => a != null && (a.data == null || string.IsNullOrEmpty(a.data.animalId))))
                    Debug.LogWarning($"[Data Error] Animal asset '{a.name}' is missing an animalId or has null data.");

                _animalLookup = animals
                    .Where(a => a != null && a.data != null && !string.IsNullOrEmpty(a.data.animalId))
                    .ToDictionary(a => a.data.animalId, a => a);
            }
            else _animalLookup = new Dictionary<string, AnimalDataSO>();

            // 3. Safe Quest Initialization (Source of the original crash)
            if (quests != null)
            {
                foreach (var q in quests.Where(q => q != null && string.IsNullOrEmpty(q.questId)))
                    Debug.LogWarning($"[Data Error] Quest asset '{q.name}' is missing a questId.");

                _questLookup = quests
                    .Where(q => q != null && !string.IsNullOrEmpty(q.questId))
                    .ToDictionary(q => q.questId, q => q);
            }
            else _questLookup = new Dictionary<string, QuestDataSO>();
        }

        public CropDataSO GetCrop(string cropId)
        {
            if (_cropLookup == null) Initialize();
            _cropLookup.TryGetValue(cropId, out CropDataSO result);
            return result;
        }

        public AnimalDataSO GetAnimal(string animalId)
        {
            if (_animalLookup == null) Initialize();
            _animalLookup.TryGetValue(animalId, out AnimalDataSO result);
            return result;
        }

        public QuestDataSO GetQuest(string questId)
        {
            if (_questLookup == null) Initialize();
            _questLookup.TryGetValue(questId, out QuestDataSO result);
            return result;
        }
    }
}
