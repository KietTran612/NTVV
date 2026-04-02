namespace NTVV.UI.Panels
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using NTVV.World.Views;
    using NTVV.Data;

    /// <summary>
    /// Controller cho bảng hành động ngữ cảnh (Context Action Panel).
    /// Hỗ trợ Ô đất, Chuồng và Vật nuôi.
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

        private CropTileView _targetTile;
        private AnimalPenView _targetPen;
        private AnimalView _targetAnimal;

        private void Start()
        {
            _closeButton?.onClick.AddListener(() => gameObject.SetActive(false));
            
            _plantButton?.onClick.AddListener(() => { _targetTile?.Plant(null); RefreshUI(); });
            _harvestButton?.onClick.AddListener(() => { _targetTile?.Harvest(); gameObject.SetActive(false); });
            _resetButton?.onClick.AddListener(() => { _targetTile?.ClearDead(); gameObject.SetActive(false); });
            
            _buyButton?.onClick.AddListener(() => { _targetPen?.PurchaseAnimal(); RefreshUI(); });
            _feedButton?.onClick.AddListener(() => { _targetAnimal?.Feed(); RefreshUI(); });
            _sellButton?.onClick.AddListener(() => { _targetAnimal?.Sell(); gameObject.SetActive(false); });
        }

        public void Setup(CropTileView target) { ClearTargets(); _targetTile = target; Open(); }
        public void Setup(AnimalPenView target) { ClearTargets(); _targetPen = target; Open(); }
        public void Setup(AnimalView target) { ClearTargets(); _targetAnimal = target; Open(); }

        private void Open()
        {
            gameObject.SetActive(true);
            RefreshUI();
        }

        private void ClearTargets() { _targetTile = null; _targetPen = null; _targetAnimal = null; }

        private void RefreshUI()
        {
            SetAllButtonsActive(false);

            if (_targetTile != null)
            {
                _headerText.text = _targetTile.CurrentState == CropTileView.TileState.Empty ? "Mảnh Đất" : _targetTile.CurrentCropData.cropName;
                _plantButton.gameObject.SetActive(_targetTile.CurrentState == CropTileView.TileState.Empty);
                _harvestButton.gameObject.SetActive(_targetTile.CurrentState == CropTileView.TileState.Ripe);
                _resetButton.gameObject.SetActive(_targetTile.CurrentState == CropTileView.TileState.Dead);
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
        }
    }
}
