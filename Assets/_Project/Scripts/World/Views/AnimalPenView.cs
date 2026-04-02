namespace NTVV.World.Views
{
    using UnityEngine;
    using System.Collections.Generic;
    using NTVV.Data;
    using NTVV.Gameplay.Economy;
    using NTVV.Gameplay.Progression;
    using NTVV.Data.ScriptableObjects;
    using System.Linq;

    /// <summary>
    /// Quản lý chuồng gia súc gia cầm.
    /// Đã được chuẩn hóa 100% camelCase.
    /// </summary>
    public class AnimalPenView : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private AnimalData _animalType;
        [SerializeField] private int _capacity = 4;
        [SerializeField] private GameObject _animalPrefab; 

        [Header("State")]
        [SerializeField] private int _currentTier = 0;
        [SerializeField] private List<AnimalView> _currentAnimals = new List<AnimalView>();
        
        public AnimalData AnimalType => _animalType;
        public int Capacity => GetCurrentCapacity();
        public int CurrentCount => _currentAnimals.Count;
        public bool IsFull => _currentAnimals.Count >= Capacity;

        private int GetCurrentCapacity()
        {
            var registry = Resources.FindObjectsOfTypeAll<GameDataRegistrySO>().FirstOrDefault();
            if (registry != null && registry.animalPenUpgradeConfig != null)
            {
                var tier = registry.animalPenUpgradeConfig.GetTier(_currentTier);
                return tier.maxCapacity;
            }
            return _capacity; // Default if no data found
        }

        public bool Upgrade()
        {
            var registry = Resources.FindObjectsOfTypeAll<GameDataRegistrySO>().FirstOrDefault();
            if (registry == null || registry.animalPenUpgradeConfig == null) return false;

            int nextTier = _currentTier + 1;
            if (!registry.animalPenUpgradeConfig.HasNextTier(_currentTier)) return false;

            var upgradeData = registry.animalPenUpgradeConfig.GetTier(nextTier);

            // Kiểm tra Cấp độ
            if (LevelSystem.Instance != null && upgradeData.minLevelToAccess > 0 && LevelSystem.Instance.CurrentLevel < upgradeData.minLevelToAccess)
            {
                Debug.LogWarning($"[AnimalPen] Cần Level {upgradeData.minLevelToAccess} để nâng cấp lên Tier {nextTier}!");
                return false;
            }

            if (EconomySystem.Instance != null && EconomySystem.Instance.CanAfford(upgradeData.upgradeCostGold))
            {
                EconomySystem.Instance.AddGold(-upgradeData.upgradeCostGold);
                _currentTier = nextTier;
                Debug.Log($"<color=green>[AnimalPen]</color> Upgraded to Tier {_currentTier}. New Capacity: {upgradeData.maxCapacity}");
                return true;
            }

            return false;
        }

        public bool PurchaseAnimalFromShop()
        {
            if (IsFull) return false;
            SpawnAnimal();
            return true;
        }

        public bool PurchaseAnimal()
        {
            if (IsFull) return false;

            if (LevelSystem.Instance != null && LevelSystem.Instance.CurrentLevel < _animalType.unlockLevel)
            {
                Debug.LogWarning($"Cần cấp độ {_animalType.unlockLevel} để nuôi {_animalType.animalName}");
                return false;
            }

            if (EconomySystem.Instance != null && EconomySystem.Instance.CanAfford(_animalType.buyCostGold))
            {
                EconomySystem.Instance.AddGold(-_animalType.buyCostGold);
                SpawnAnimal();
                return true;
            }

            return false;
        }

        private void SpawnAnimal()
        {
            if (_animalPrefab == null) return;

            GameObject go = Instantiate(_animalPrefab, transform);
            go.transform.localPosition = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            
            AnimalView view = go.GetComponent<AnimalView>();
            if (view != null)
            {
                view.Initialize(_animalType);
                _currentAnimals.Add(view);
            }
        }

        public void RemoveAnimal(AnimalView entity)
        {
            if (_currentAnimals.Contains(entity))
            {
                _currentAnimals.Remove(entity);
            }
        }
    }
}
