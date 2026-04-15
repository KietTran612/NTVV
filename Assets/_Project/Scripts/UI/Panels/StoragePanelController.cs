namespace NTVV.UI.Panels
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using System.Collections.Generic;
    using System.Linq;
    using NTVV.Data.ScriptableObjects;
    using NTVV.Gameplay.Economy;
    using NTVV.Gameplay.Storage;
    using NTVV.Gameplay.Progression;
    using NTVV.UI.Common;

    /// <summary>
    /// Controller for the Storage popup (grid view + sell sub-panel + upgrade).
    /// Tabs: All / Crops / Animals. Sell sub-panel appears on slot click.
    /// </summary>
    public class StoragePanelController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text _capacityText;
        [SerializeField] private Transform _storageContentContainer;
        [SerializeField] private Button _btnClose;
        [SerializeField] private Button _btnUpgrade;
        [SerializeField] private TMP_Text _upgradeCostText;

        [Header("Tab Buttons")]
        [SerializeField] private Button _tabAll;
        [SerializeField] private Button _tabCrops;
        [SerializeField] private Button _tabAnimals;

        [Header("Sell Sub-Panel")]
        [SerializeField] private GameObject _sellSubPanel;
        [SerializeField] private TMP_Text _selectedItemNameText;
        [SerializeField] private TMP_Text _quantityText;
        [SerializeField] private TMP_Text _totalPriceText;
        [SerializeField] private Button _btnPlus;
        [SerializeField] private Button _btnMinus;
        [SerializeField] private Button _btnSellNow;

        [Header("Footer")]
        [SerializeField] private Button _btnSellAll;

        [Header("Templates")]
        [SerializeField] private GameObject _itemCardPrefab;

        private string _currentCategory = "All";
        private string _selectedItemId;
        private int _sellQuantity = 1;
        private int _currentItemSellPrice = 0;

        private void OnEnable()
        {
            if (_btnClose   != null) _btnClose.onClick.AddListener(() => PopupManager.Instance.CloseActiveModal());
            if (_btnPlus    != null) _btnPlus.onClick.AddListener(() => AdjustQuantity(1));
            if (_btnMinus   != null) _btnMinus.onClick.AddListener(() => AdjustQuantity(-1));
            if (_btnSellNow != null) _btnSellNow.onClick.AddListener(OnSellClick);
            if (_btnUpgrade != null) _btnUpgrade.onClick.AddListener(OnUpgradeClick);
            if (_tabAll     != null) _tabAll.onClick.AddListener(() => PopulateGrid("All"));
            if (_tabCrops   != null) _tabCrops.onClick.AddListener(() => PopulateGrid("Crops"));
            if (_tabAnimals != null) _tabAnimals.onClick.AddListener(() => PopulateGrid("Animals"));
            if (_btnSellAll != null) _btnSellAll.onClick.AddListener(OnSellAllClick);

            StorageSystem.OnStorageChanged += HandleStorageChanged;

            if (_sellSubPanel != null) _sellSubPanel.SetActive(false);
            PopulateGrid(_currentCategory);
        }

        private void OnDisable()
        {
            _btnClose?.onClick.RemoveAllListeners();
            _btnPlus?.onClick.RemoveAllListeners();
            _btnMinus?.onClick.RemoveAllListeners();
            _btnSellNow?.onClick.RemoveAllListeners();
            _btnUpgrade?.onClick.RemoveAllListeners();
            _tabAll?.onClick.RemoveAllListeners();
            _tabCrops?.onClick.RemoveAllListeners();
            _tabAnimals?.onClick.RemoveAllListeners();
            _btnSellAll?.onClick.RemoveAllListeners();

            StorageSystem.OnStorageChanged -= HandleStorageChanged;
        }

        private void HandleStorageChanged(int used, int max)
        {
            UpdateCapacityText(used, max);
            UpdateUpgradeInfo();
        }

        private void UpdateCapacityText(int used, int max)
        {
            if (_capacityText != null) _capacityText.text = $"{used} / {max} slots";
        }

        // ── Grid population ───────────────────────────────────────────────────

        public void PopulateGrid(string category = null)
        {
            if (category != null) _currentCategory = category;
            if (_storageContentContainer == null || StorageSystem.Instance == null) return;

            foreach (Transform child in _storageContentContainer) Destroy(child.gameObject);

            var registry = Resources.FindObjectsOfTypeAll<GameDataRegistrySO>().FirstOrDefault();
            var items    = StorageSystem.Instance.GetAllItems();

            foreach (var item in items)
            {
                bool isCrop = registry != null && registry.crops.Any(c => c.data.cropId == item.Key);
                if (_currentCategory == "Crops"   && !isCrop) continue;
                if (_currentCategory == "Animals" &&  isCrop) continue;

                string displayName = item.Key;
                Sprite icon        = null;
                int    sellPrice   = 0;

                if (registry != null)
                {
                    var cropSO = registry.crops.FirstOrDefault(c => c.data.cropId == item.Key);
                    if (cropSO != null)
                    {
                        displayName = cropSO.data.cropName;
                        icon        = cropSO.data.seedIcon;
                        sellPrice   = cropSO.data.sellPriceGold;
                    }
                }

                var go   = Instantiate(_itemCardPrefab, _storageContentContainer);
                var slot = go.GetComponent<InventorySlotController>();
                if (slot == null) { Debug.LogError($"[Storage] {_itemCardPrefab.name} missing InventorySlotController!"); continue; }

                string capturedId    = item.Key;
                string capturedName  = displayName;
                int    capturedPrice = sellPrice;
                slot.Initialize(capturedId, capturedName, item.Value, icon,
                    (_) => SelectItem(capturedId, capturedName, capturedPrice));
            }

            if (StorageSystem.Instance != null)
                UpdateCapacityText(StorageSystem.Instance.CurrentSlotsUsed, StorageSystem.Instance.MaxCapacity);
            UpdateUpgradeInfo();
        }

        // ── Item selection / sell sub-panel ───────────────────────────────────

        private void SelectItem(string itemId, string displayName, int sellPrice)
        {
            _selectedItemId       = itemId;
            _sellQuantity         = 1;
            _currentItemSellPrice = sellPrice;

            if (_sellSubPanel        != null) _sellSubPanel.SetActive(true);
            if (_selectedItemNameText != null) _selectedItemNameText.text = displayName;
            UpdateSellInfo();
        }

        private void AdjustQuantity(int delta)
        {
            if (StorageSystem.Instance == null) return;
            int maxStock  = StorageSystem.Instance.GetItemCount(_selectedItemId);
            _sellQuantity = Mathf.Clamp(_sellQuantity + delta, 1, Mathf.Max(1, maxStock));
            UpdateSellInfo();
        }

        private void UpdateSellInfo()
        {
            if (_quantityText  != null) _quantityText.text  = _sellQuantity.ToString();
            if (_totalPriceText != null) _totalPriceText.text = $"{_sellQuantity * _currentItemSellPrice}g";
        }

        private void OnSellClick()
        {
            if (string.IsNullOrEmpty(_selectedItemId) || _currentItemSellPrice <= 0) return;
            if (StorageSystem.Instance == null) return;
            if (StorageSystem.Instance.GetItemCount(_selectedItemId) < _sellQuantity) return;

            StorageSystem.Instance.AddItem(_selectedItemId, -_sellQuantity);
            EconomySystem.Instance?.AddGold(_sellQuantity * _currentItemSellPrice);
            Managers.GameManager.Instance?.TriggerSave();

            Debug.Log($"<color=cyan>[Storage]</color> Sold {_selectedItemId} ×{_sellQuantity} for {_sellQuantity * _currentItemSellPrice}g");

            _selectedItemId       = null;
            _currentItemSellPrice = 0;
            if (_sellSubPanel != null) _sellSubPanel.SetActive(false);
            PopulateGrid();
        }

        // ── Sell All ──────────────────────────────────────────────────────────

        private void OnSellAllClick()
        {
            if (StorageSystem.Instance == null || EconomySystem.Instance == null) return;

            var registry  = Resources.FindObjectsOfTypeAll<GameDataRegistrySO>().FirstOrDefault();
            var snapshot  = StorageSystem.Instance.GetAllItems().ToList();
            int totalGold = 0;

            foreach (var item in snapshot)
            {
                bool isCrop = registry != null && registry.crops.Any(c => c.data.cropId == item.Key);
                if (_currentCategory == "Crops"   && !isCrop) continue;
                if (_currentCategory == "Animals" &&  isCrop) continue;

                int price = 0;
                if (registry != null)
                {
                    var cropSO = registry.crops.FirstOrDefault(c => c.data.cropId == item.Key);
                    if (cropSO != null) price = cropSO.data.sellPriceGold;
                }

                totalGold += item.Value * price;
                StorageSystem.Instance.AddItem(item.Key, -item.Value);
            }

            EconomySystem.Instance.AddGold(totalGold);
            Managers.GameManager.Instance?.TriggerSave();
            if (_sellSubPanel != null) _sellSubPanel.SetActive(false);
            PopulateGrid();
            Debug.Log($"<color=cyan>[Storage]</color> Sold All for {totalGold}g");
        }

        // ── Upgrade ───────────────────────────────────────────────────────────

        private void UpdateUpgradeInfo()
        {
            if (StorageSystem.Instance == null || _btnUpgrade == null) return;

            var config = StorageSystem.Instance.UpgradeConfig;
            if (config == null) { _btnUpgrade.gameObject.SetActive(false); return; }

            int currentTier = StorageSystem.Instance.CurrentTier;
            _btnUpgrade.interactable = true;

            if (config.HasNextTier(currentTier))
            {
                var nextTier = config.GetTier(currentTier);
                _btnUpgrade.gameObject.SetActive(true);

                string costText   = $"{nextTier.upgradeCostGold}g";
                string levelText  = nextTier.minLevelToAccess != -1 ? $" (Req. Lvl {nextTier.minLevelToAccess})" : "";
                if (_upgradeCostText != null) _upgradeCostText.text = $"Upgrade: {costText}{levelText}";

                bool levelLocked = nextTier.minLevelToAccess != -1
                    && LevelSystem.Instance != null
                    && LevelSystem.Instance.CurrentLevel < nextTier.minLevelToAccess;

                if (levelLocked)
                {
                    _btnUpgrade.interactable = false;
                    if (_upgradeCostText != null)
                        _upgradeCostText.text = $"LOCKED (Req. Lvl {nextTier.minLevelToAccess})";
                }
            }
            else
            {
                _btnUpgrade.gameObject.SetActive(false);
                if (_upgradeCostText != null) _upgradeCostText.text = "Max Level Reached";
            }
        }

        private void OnUpgradeClick()
        {
            if (StorageSystem.Instance == null) return;
            if (StorageSystem.Instance.TryUpgradeStorage())
            {
                Managers.GameManager.Instance?.TriggerSave();
                Debug.Log("<color=green>[Storage]</color> Upgraded successfully.");
            }
        }
    }
}
