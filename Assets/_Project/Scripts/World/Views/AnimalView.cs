namespace NTVV.World.Views
{
    using UnityEngine;
    using NTVV.Data;
    using NTVV.Data.ScriptableObjects;
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
        
        private AnimalPenView _pen;
        
        [Header("Production State")]
        [SerializeField] private float _productionTimer;
        [SerializeField] private bool _isReadyToProduce;

        [Header("Visual References")]
        [SerializeField] private SpriteRenderer _bodyRenderer;
        [SerializeField] private GameObject _hungerVisual;
        [SerializeField] private GameObject _produceVisual;
        [SerializeField] private GameDataRegistrySO _registry;

        public AnimalData CurrentData => _data;
        public GrowthStage CurrentStage => _currentStage;
        public bool IsHungry => _isHungry;
        public bool IsReadyToProduce => _isReadyToProduce;

        public void Initialize(AnimalData data, AnimalPenView pen = null)
        {
            _data = data;
            _pen = pen;
            _currentStage = GrowthStage.Baby;
            _isHungry = false;
            _hungerTimer = 0f;
            _growthProgress = 0f;
            _productionTimer = 0f;
            _isReadyToProduce = false;
            RefreshVisuals();
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
                if (_timeSinceMature >= _data.LifeAfterMatureInSeconds) 
                {
                    _currentStage = GrowthStage.Dead;
                    RefreshVisuals();
                    _pen?.RemoveAnimal(this); // BUG-02: remove from pen on natural death
                    return;                   // BUG-01: prevent production block from running
                }

                // Production Logic: Only if not hungry and mature
                if (!_isHungry)
                {
                    _productionTimer += tickDelta;
                    if (_productionTimer >= (_data.produceTimeMin * 60f))
                    {
                        CollectProduct(); // auto-collect
                        _productionTimer = 0f;
                    }
                }
            }
        }
        
        private void RefreshVisuals()
        {
            UpdateGrowthVisuals();
            UpdateStatusVisuals();
        }

        private void UpdateGrowthVisuals()
        {
            if (_bodyRenderer == null) return;

            if (_currentStage == GrowthStage.Dead)
            {
                _bodyRenderer.color = Color.gray;
                return;
            }

            _bodyRenderer.color = Color.white;

            if (_registry != null && _data != null)
            {
                var so = _registry.GetAnimal(_data.animalId);
                if (so != null && so.stageSprites != null)
                {
                    int idx = (int)_currentStage;
                    if (idx < so.stageSprites.Length)
                    {
                        _bodyRenderer.sprite = so.stageSprites[idx];
                    }

                    // Fallback Color Coding if sprite is missing
                    if (_bodyRenderer.sprite == null)
                    {
                        _bodyRenderer.color = GetStageColor(_currentStage);
                    }
                }
            }
        }

        private Color GetStageColor(GrowthStage stage)
        {
            switch (stage)
            {
                case GrowthStage.Baby:   return new Color(1f, 0.8f, 0.8f); // Light Pink
                case GrowthStage.Stage2: return new Color(1f, 0.6f, 0f);    // Orange
                case GrowthStage.Mature: return new Color(1f, 0.4f, 0f);    // Deep Orange
                default: return Color.white;
            }
        }

        private void UpdateStatusVisuals()
        {
            if (_hungerVisual != null) _hungerVisual.SetActive(_isHungry);
            if (_produceVisual != null) _produceVisual.SetActive(_isReadyToProduce);
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
            RefreshVisuals();
        }

        public void Feed()
        {
            if (!_isHungry) return;

            if (StorageSystem.Instance == null)
            {
                Debug.LogWarning("[AnimalView] StorageSystem not ready.");
                return;
            }

            int grassCount = StorageSystem.Instance.GetItemCount("item_grass");
            int wormCount  = StorageSystem.Instance.GetItemCount("item_worm");

            if (grassCount < _data.feedQtyGrass || wormCount < _data.feedQtyWorm)
            {
                Debug.LogWarning($"[Animal] Not enough food to feed {_data.animalName}. Need {_data.feedQtyGrass}x grass + {_data.feedQtyWorm}x worm.");
                return;
            }

            if (_data.feedQtyGrass > 0) StorageSystem.Instance.AddItem("item_grass", -_data.feedQtyGrass);
            if (_data.feedQtyWorm > 0) StorageSystem.Instance.AddItem("item_worm", -_data.feedQtyWorm);
            
            _isHungry = false;
            _hungerTimer = 0f;

            NTVV.Gameplay.Quests.QuestEvents.InvokeActionPerformed(Data.QuestActionType.FeedAnimal, _data.animalId, 1);
            Debug.Log($"<color=green>[Animal]</color> Fed {_data.animalName}");
            RefreshVisuals();
        }

        public void CollectProduct()
        {
            if (!_isReadyToProduce) return;

            if (StorageSystem.Instance != null && StorageSystem.Instance.CanAddItem(_data.produceItemId, 1))
            {
                StorageSystem.Instance.AddItem(_data.produceItemId, 1);
                if (LevelSystem.Instance != null) LevelSystem.Instance.AddXP(_data.produceXp);
                
                _isReadyToProduce = false;
                _productionTimer = 0f;

                NTVV.Gameplay.Quests.QuestEvents.InvokeActionPerformed(Data.QuestActionType.CollectProduct, _data.produceItemId, 1);
                RefreshVisuals();
                Debug.Log($"<color=cyan>[Production]</color> Collected product from {_data.animalName}");
            }
        }

        public void Sell()
        {
            if (_currentStage == GrowthStage.Dead)
            {
                _pen?.RemoveAnimal(this);
                Destroy(gameObject);
                return;
            }

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

            // BUG-07: fire quest event before destroy
            NTVV.Gameplay.Quests.QuestEvents.InvokeActionPerformed(Data.QuestActionType.SellAnimal, _data.animalId, 1);
            // BUG-02: save + remove from pen before destroy
            Managers.GameManager.Instance?.TriggerSave();
            _pen?.RemoveAnimal(this);
            Destroy(gameObject);
        }

        public AnimalSaveData GetSaveData() => new AnimalSaveData
        {
            animalId                = _data.animalId,
            stage                   = (int)_currentStage,
            hp                      = _hungerTimer,
            timeInCurrentStage      = _growthProgress,
            timeSinceLastProduction = _productionTimer,
        };

        public void RestoreState(AnimalSaveData data)
        {
            _currentStage    = (GrowthStage)data.stage;
            _hungerTimer     = data.hp;
            _growthProgress  = data.timeInCurrentStage;
            _productionTimer = data.timeSinceLastProduction;

            float offline = (float)(System.DateTime.UtcNow - Managers.GameManager.LastSaveTime).TotalSeconds;
            _growthProgress  += offline;
            _productionTimer += offline;

            RefreshVisuals();
        }
    }
}
