namespace NTVV.UI.Panels
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using System.Collections.Generic;
    using NTVV.Data;
    using NTVV.Gameplay.Economy;
    using NTVV.Gameplay.Storage;
    using NTVV.UI.Common;

    /// <summary>
    /// Controller cho bảng Cửa hàng hạt giống.
    /// Đã được cập nhật chuẩn camelCase.
    /// </summary>
    public class SeedShopPanelController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text _goldBalanceLabel;
        [SerializeField] private Transform _shopContentContainer;
        [SerializeField] private Button _btnClose;
        [SerializeField] private Button _tabSeeds;

        [Header("Templates & Data")]
        [SerializeField] private GameObject _shopItemPrefab;
        [SerializeField] private List<CropData> _availableCrops; 

        private void OnEnable()
        {
            if (_btnClose != null) _btnClose.onClick.AddListener(() => PopupManager.Instance.CloseActiveModal());
            if (_tabSeeds != null) _tabSeeds.onClick.AddListener(() => PopulateShop("Seeds"));

            EconomySystem.OnGoldChanged += UpdateGold;
            UpdateGold(EconomySystem.Instance?.CurrentGold ?? 0);
            PopulateShop("Seeds");
        }

        private void OnDisable()
        {
            if (_btnClose != null) _btnClose.onClick.RemoveAllListeners();
            if (_tabSeeds != null) _tabSeeds.onClick.RemoveAllListeners();
            EconomySystem.OnGoldChanged -= UpdateGold;
        }

        private void UpdateGold(int amount)
        {
            if (_goldBalanceLabel != null) _goldBalanceLabel.text = $"{amount:N0}g";
        }

        public void PopulateShop(string category)
        {
            if (_shopContentContainer == null || _shopItemPrefab == null) return;
            foreach (Transform child in _shopContentContainer) Destroy(child.gameObject);

            if (category == "Seeds" && _availableCrops != null)
            {
                foreach (var crop in _availableCrops)
                {
                    var go = Instantiate(_shopItemPrefab, _shopContentContainer);
                    
                    TMP_Text nameLabel = go.transform.Find("ItemName")?.GetComponent<TMP_Text>();
                    Button buyBtn = go.transform.Find("BuyButton")?.GetComponent<Button>();
                    TMP_Text priceLabel = buyBtn?.transform.Find("Text")?.GetComponent<TMP_Text>();

                    if (nameLabel != null) nameLabel.text = crop.cropName;
                    if (priceLabel != null) priceLabel.text = $"MUA ({crop.seedCostGold}g)";
                    
                    if (buyBtn != null) 
                    {
                        CropData capturedCrop = crop;
                        buyBtn.onClick.AddListener(() => TryBuy(capturedCrop.cropId, capturedCrop.seedCostGold));
                    }
                }
            }
        }

        private void TryBuy(string itemId, int cost)
        {
            if (EconomySystem.Instance != null && EconomySystem.Instance.CanAfford(cost))
            {
                EconomySystem.Instance.AddGold(-cost);
                StorageSystem.Instance.AddItem(itemId, 1);
                Debug.Log($"<color=cyan>[Shop]</color> Bought: {itemId}");
            }
        }
    }
}
