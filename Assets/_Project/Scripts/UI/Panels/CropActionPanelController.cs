namespace NTVV.UI.Panels
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using NTVV.World.Views;
    using NTVV.UI.Common;
    using NTVV.Data.ScriptableObjects;
    using NTVV.Gameplay.Economy;

    /// <summary>
    /// Controller for the World-space Context Action Panel.
    /// Manages button visibility and colors based on tile/animal state.
    /// </summary>
    public class CropActionPanelController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text _headerText;
        [SerializeField] private Button _plantButton;
        [SerializeField] private Button _harvestButton;
        [SerializeField] private Button _resetButton;
        
        [Header("Animal Actions")]
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _feedButton;
        [SerializeField] private Button _sellButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _collectButton;

        [Header("Caring Actions")]
        [SerializeField] private Button _waterButton;
        [SerializeField] private Button _cureButton;
        [SerializeField] private Button _weedButton;

        [Header("Data (Required for Plant)")]
        [SerializeField] private GameDataRegistrySO _registry;

        private CropTileView _targetTile;
        private AnimalPenView _targetPen;
        private AnimalView _targetAnimal;

        private void Start()
        {
            _closeButton?.onClick.AddListener(() => {
                PopupManager.Instance?.CloseContextAction();
                if (PopupManager.Instance == null) { gameObject.SetActive(false); Managers.GameManager.Instance?.TriggerSave(); }
            });
            
            // FIX BUG-01: Plant button now calls TryAutoPlant() instead of Plant(null)
            _plantButton?.onClick.AddListener(() => { TryAutoPlant(); RefreshUI(); });
            _harvestButton?.onClick.AddListener(() => {
                _targetTile?.Harvest();
                PopupManager.Instance?.CloseContextAction();
                if (PopupManager.Instance == null) { gameObject.SetActive(false); Managers.GameManager.Instance?.TriggerSave(); }
            });
            _resetButton?.onClick.AddListener(() => {
                _targetTile?.ClearDead();
                PopupManager.Instance?.CloseContextAction();
                if (PopupManager.Instance == null) { gameObject.SetActive(false); Managers.GameManager.Instance?.TriggerSave(); }
            });
            
            _buyButton?.onClick.AddListener(() => { _targetPen?.PurchaseAnimal(); RefreshUI(); });
            _feedButton?.onClick.AddListener(() => { _targetAnimal?.Feed(); RefreshUI(); });
            _sellButton?.onClick.AddListener(() => { _targetAnimal?.Sell(); gameObject.SetActive(false); });
            _collectButton?.onClick.AddListener(() => { _targetAnimal?.CollectProduct(); RefreshUI(); });

            _waterButton?.onClick.AddListener(() => { _targetTile?.WaterPlant(); RefreshUI(); });
            _cureButton?.onClick.AddListener(() => { _targetTile?.ClearPests(); RefreshUI(); });
            _weedButton?.onClick.AddListener(() => { _targetTile?.ClearWeeds(); RefreshUI(); });
        }

        /// <summary>
        /// Automatically plants the first affordable crop found in the registry.
        /// If none affordable, opens the Shop for the player to buy seeds first.
        /// </summary>
        private void TryAutoPlant()
        {
            if (_targetTile == null) return;

            if (_registry == null)
            {
                Debug.LogError("[CropAction] _registry is not assigned! Cannot auto-plant.");
                return;
            }

            foreach (var cropSO in _registry.crops)
            {
                if (cropSO?.data == null) continue;
                if (EconomySystem.Instance?.CanAfford(cropSO.data.seedCostGold) == true)
                {
                    _targetTile.Plant(cropSO.data);
                    return;
                }
            }

            // No affordable crop found — open shop so player can buy seeds
            Debug.LogWarning("[CropAction] No affordable crop found. Opening Shop...");
            PopupManager.Instance?.ShowScreen("Shop");
        }

        public void Setup(CropTileView target) { ClearTargets(); _targetTile = target; RefreshUI(); }
        public void Setup(AnimalPenView target) { ClearTargets(); _targetPen = target; RefreshUI(); }
        public void Setup(AnimalView target) { ClearTargets(); _targetAnimal = target; RefreshUI(); }

        private void ClearTargets() { _targetTile = null; _targetPen = null; _targetAnimal = null; }

        private void RefreshUI()
        {
            SetAllButtonsActive(false);

            if (_targetTile != null)
            {
                string cropName = _targetTile.CurrentCropData?.cropName ?? "[Unknown]";
                _headerText.text = _targetTile.CurrentState == CropTileView.TileState.Empty ? "Mảnh Đất" : cropName;
                _plantButton.gameObject.SetActive(_targetTile.CurrentState == CropTileView.TileState.Empty);
                _harvestButton.gameObject.SetActive(_targetTile.CurrentState == CropTileView.TileState.Ripe);
                _resetButton.gameObject.SetActive(_targetTile.CurrentState == CropTileView.TileState.Dead);

                _waterButton.gameObject.SetActive(_targetTile.NeedsWater);
                _cureButton.gameObject.SetActive(_targetTile.HasPests);
                _weedButton.gameObject.SetActive(_targetTile.HasWeeds);
            }
            else if (_targetPen != null)
            {
                _headerText.text = $"Chuồng {_targetPen.AnimalType.animalName}";
                _buyButton.gameObject.SetActive(!_targetPen.IsFull);
            }
            else if (_targetAnimal != null)
            {
                _headerText.text = _targetAnimal.CurrentData.animalName;
                _feedButton.gameObject.SetActive(_targetAnimal.IsHungry);
                _collectButton.gameObject.SetActive(_targetAnimal.IsReadyToProduce);
                _sellButton.gameObject.SetActive(true);
            }
        }

        private void SetAllButtonsActive(bool val)
        {
            _plantButton.gameObject.SetActive(val);
            _harvestButton.gameObject.SetActive(val);
            _resetButton.gameObject.SetActive(val);
            _buyButton.gameObject.SetActive(val);
            _feedButton.gameObject.SetActive(val);
            _sellButton.gameObject.SetActive(val);
            _waterButton.gameObject.SetActive(val);
            _cureButton.gameObject.SetActive(val);
            _weedButton.gameObject.SetActive(val);
            _collectButton.gameObject.SetActive(val);
        }
    }
}
