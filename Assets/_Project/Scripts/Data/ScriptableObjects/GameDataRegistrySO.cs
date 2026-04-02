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
            _cropLookup = crops.ToDictionary(c => c.data.cropId, c => c);
            _animalLookup = animals.ToDictionary(a => a.data.animalId, a => a);
            _questLookup = quests.ToDictionary(q => q.questId, q => q);
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
