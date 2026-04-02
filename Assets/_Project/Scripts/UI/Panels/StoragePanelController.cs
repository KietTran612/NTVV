namespace NTVV.UI.Panels
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using System.Collections.Generic;
    using NTVV.Data;
    using NTVV.Data.ScriptableObjects;
    using NTVV.Gameplay.Economy;
    using NTVV.Gameplay.Storage;
    using NTVV.UI.Common;

    /// <summary>
    /// Controller for the Storage and Sell screen using uGUI.
    /// Handles item grid population and sale logic with actual item prices.
    /// </summary>
    public class StoragePanelController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform _storageContentContainer;
        [SerializeField] private Button _btnClose;
        
        [Header("Sell Panel References")]
        [SerializeField] private TMP_Text _selectedItemNameText;
        [SerializeField] private TMP_Text _quantityText;
        [SerializeField] private TMP_Text _totalPriceText;
        [SerializeField] private Button _btnPlus;
        [SerializeField] private Button _btnMinus;
        [SerializeField] private Button _btnSellNow;

        [Header("Templates & Data")]
        [SerializeField] private GameObject _itemCardPrefab;
        [SerializeField] private List<ItemData> _itemDatabase; // Assign all game items in inspector for prototype lookup

        private string _selectedItemId;
        private int _sellQuantity = 1;
        private int _currentItemSellPrice = 0;

        private void OnEnable()
        {
            if (_btnClose != null) _btnClose.onClick.AddListener(() => PopupManager.Instance.CloseActiveModal());
            if (_btnPlus != null) _btnPlus.onClick.AddListener(() => AdjustQuantity(1));
            if (_btnMinus != null) _btnMinus.onClick.AddListener(() => AdjustQuantity(-1));
            if (_btnSellNow != null) _btnSellNow.onClick.AddListener(OnSellClick);

            RefreshStorage();
        }

        private void OnDisable()
        {
            _btnClose?.onClick.RemoveAllListeners();
            _btnPlus?.onClick.RemoveAllListeners();
            _btnMinus?.onClick.RemoveAllListeners();
            _btnSellNow?.onClick.RemoveAllListeners();
        }

        public void RefreshStorage()
        {
            if (_storageContentContainer == null || StorageSystem.Instance == null) return;
            
            foreach (Transform child in _storageContentContainer) Destroy(child.gameObject);

            var items = StorageSystem.Instance.GetAllItems();
            foreach (var item in items)
            {
                ItemData data = GetItemData(item.Key);
                string displayName = data != null ? data.itemName : item.Key;

                var go = Instantiate(_itemCardPrefab, _storageContentContainer);
                TMP_Text nameLabel = go.transform.Find("ItemName")?.GetComponent<TMP_Text>();
                Button cardBtn = go.GetComponent<Button>();

                if (nameLabel != null) nameLabel.text = $"{displayName} x{item.Value}";
                
                if (cardBtn != null) 
                {
                    string capturedKey = item.Key;
                    cardBtn.onClick.AddListener(() => SelectItem(capturedKey, data));
                }
            }
        }

        private ItemData GetItemData(string itemId)
        {
            if (_itemDatabase == null) return null;
            return _itemDatabase.Find(x => x.itemId == itemId);
        }

        private void SelectItem(string itemId, ItemData data)
        {
            _selectedItemId = itemId;
            _sellQuantity = 1;
            
            _currentItemSellPrice = 0;
            // Lookup real price
            if (data != null)
            {
                if (data is CropDataSO cropDataSO) _currentItemSellPrice = cropDataSO.data.sellPriceGold;
                // Add overrides for other item types here if needed
            }
            
            if (_selectedItemNameText != null) _selectedItemNameText.text = data != null ? data.itemName : itemId;
            UpdateSellInfo();
        }

        private void AdjustQuantity(int delta)
        {
            if (StorageSystem.Instance == null) return;
            
            int maxStock = StorageSystem.Instance.GetItemCount(_selectedItemId);
            _sellQuantity = Mathf.Clamp(_sellQuantity + delta, 1, Mathf.Max(1, maxStock));
            UpdateSellInfo();
        }

        private void UpdateSellInfo()
        {
            if (_quantityText != null) _quantityText.text = _sellQuantity.ToString();
            if (_totalPriceText != null) _totalPriceText.text = $"{_sellQuantity * _currentItemSellPrice}g";
        }

        private void OnSellClick()
        {
            if (string.IsNullOrEmpty(_selectedItemId) || _currentItemSellPrice <= 0) return;
            
            if (StorageSystem.Instance != null && StorageSystem.Instance.GetItemCount(_selectedItemId) >= _sellQuantity)
            {
                StorageSystem.Instance.AddItem(_selectedItemId, -_sellQuantity);
                if (EconomySystem.Instance != null) EconomySystem.Instance.AddGold(_sellQuantity * _currentItemSellPrice);
                
                // Reset Selection
                _selectedItemId = null;
                _currentItemSellPrice = 0;
                
                RefreshStorage();
                UpdateSellInfo();
            }
        }
    }
}
