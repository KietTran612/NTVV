namespace NTVV.World.Views
{
    using UnityEngine;
    using NTVV.Data;
    using NTVV.Gameplay.Economy;
    using NTVV.Gameplay.Progression;
    using NTVV.Gameplay.Storage;
    using NTVV.Core;

    /// <summary>
    /// Hiển thị và xử lý một con vật nuôi trong thế giới.
    /// Đã được chuẩn hóa 100% camelCase.
    /// </summary>
    public class AnimalView : MonoBehaviour
    {
        public enum GrowthStage { Baby, Stage2, Mature, Dead }

        [Header("State")]
        [SerializeField] private AnimalData _data;
        [SerializeField] private GrowthStage _currentStage = GrowthStage.Baby;
        [SerializeField] private bool _isHungry;
        [SerializeField] private float _growthProgress; 
        [SerializeField] private float _hungerTimer;
        [SerializeField] private float _timeSinceMature;

        public AnimalData CurrentData => _data;
        public GrowthStage CurrentStage => _currentStage;
        public bool IsHungry => _isHungry;

        public void Initialize(AnimalData data)
        {
            _data = data;
            _currentStage = GrowthStage.Baby;
            _isHungry = false;
            _hungerTimer = 0f;
            _growthProgress = 0f;
        }

        private void OnEnable() { TimeManager.OnTick += HandleTick; }
        private void OnDisable() { TimeManager.OnTick -= HandleTick; }

        private void HandleTick(float tickDelta)
        {
            if (_currentStage == GrowthStage.Dead) return;

            if (!_isHungry)
            {
                _hungerTimer += tickDelta;
                if (_hungerTimer >= _data.HungerCycleInSeconds) _isHungry = true;
            }

            if (!_isHungry && _currentStage != GrowthStage.Mature) 
            {
                UpdateGrowth(tickDelta);
            }

            if (_currentStage == GrowthStage.Mature)
            {
                _timeSinceMature += tickDelta;
                if (_timeSinceMature >= _data.LifeAfterMatureInSeconds) _currentStage = GrowthStage.Dead;
            }
        }

        private void UpdateGrowth(float tickDelta)
        {
            float timeNeeded = 0;
            switch (_currentStage)
            {
                case GrowthStage.Baby: timeNeeded = _data.Stage1.TimeToReachStageInSeconds; break;
                case GrowthStage.Stage2: timeNeeded = _data.Stage2.TimeToReachStageInSeconds; break;
            }

            if (timeNeeded <= 0) return;

            _growthProgress += (tickDelta / timeNeeded);
            if (_growthProgress >= 1f)
            {
                _growthProgress = 0f;
                AdvanceStage();
            }
        }

        private void AdvanceStage()
        {
            if (_currentStage == GrowthStage.Baby) _currentStage = GrowthStage.Stage2;
            else if (_currentStage == GrowthStage.Stage2) _currentStage = GrowthStage.Mature;
        }

        public void Feed()
        {
            if (!_isHungry) return;

            // Logic ăn uống dùng chuẩn camelCase (itemId, feedQtyGrass, feedQtyWorm)
            bool hasGrass = _data.feedQtyGrass <= 0 || (StorageSystem.Instance.GetItemCount("item_grass") >= _data.feedQtyGrass);
            bool hasWorm = _data.feedQtyWorm <= 0 || (StorageSystem.Instance.GetItemCount("item_worm") >= _data.feedQtyWorm);

            if (hasGrass && hasWorm)
            {
                if (_data.feedQtyGrass > 0) StorageSystem.Instance.AddItem("item_grass", -_data.feedQtyGrass);
                if (_data.feedQtyWorm > 0) StorageSystem.Instance.AddItem("item_worm", -_data.feedQtyWorm);
                
                _isHungry = false;
                _hungerTimer = 0f;
                Debug.Log($"<color=green>[Animal]</color> Fed {_data.animalName}");
            }
        }

        public void Sell()
        {
            if (_currentStage == GrowthStage.Dead) { Destroy(gameObject); return; }

            int price = 0;
            int xp = 0;

            switch (_currentStage)
            {
                case GrowthStage.Baby: 
                    price = _data.Stage1.SalePrice; 
                    xp = _data.Stage1.XPOnSale; 
                    break;
                case GrowthStage.Stage2: 
                    price = _data.Stage2.SalePrice; 
                    xp = _data.Stage2.XPOnSale; 
                    break;
                case GrowthStage.Mature: 
                    price = _data.Stage3.SalePrice; 
                    xp = _data.Stage3.XPOnSale; 
                    break;
            }

            if (EconomySystem.Instance != null) EconomySystem.Instance.AddGold(price);
            if (LevelSystem.Instance != null) LevelSystem.Instance.AddXP(xp);

            Destroy(gameObject);
        }
    }
}
