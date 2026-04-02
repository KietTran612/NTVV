namespace NTVV.World.Views
{
    using UnityEngine;
    using System.Collections.Generic;
    using NTVV.Data;
    using NTVV.Gameplay.Economy;
    using NTVV.Gameplay.Progression;

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
        [SerializeField] private List<AnimalView> _currentAnimals = new List<AnimalView>();
        
        public AnimalData AnimalType => _animalType;
        public int Capacity => _capacity;
        public int CurrentCount => _currentAnimals.Count;
        public bool IsFull => _currentAnimals.Count >= _capacity;

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
