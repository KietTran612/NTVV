namespace NTVV.UI.Panels
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using System.Collections.Generic;
    using System.Linq;
    using NTVV.Data;
    using NTVV.Data.ScriptableObjects;
    using NTVV.Gameplay.Economy;
    using NTVV.Gameplay.Storage;
    using NTVV.Gameplay.Progression;
    using NTVV.UI.Common;
    using NTVV.World.Views;

    /// <summary>
    /// Controller cho bảng Cửa hàng tập trung (Hạt giống & Thú nuôi).
    /// </summary>
    public class ShopPanelController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text _goldBalanceLabel;
        [SerializeField] private Transform _shopContentContainer;
        [SerializeField] private Button _btnClose;
        [SerializeField] private Button _tabSeeds;
        [SerializeField] private Button _tabAnimals;

        [Header("Templates")]
        [SerializeField] private GameObject _shopItemPrefab;

        private string _currentCategory = "Seeds";

        private void OnEnable()
        {
            if (_btnClose != null) _btnClose.onClick.AddListener(() => PopupManager.Instance.CloseActiveModal());
            if (_tabSeeds != null) _tabSeeds.onClick.AddListener(() => PopulateShop("Seeds"));
            if (_tabAnimals != null) _tabAnimals.onClick.AddListener(() => PopulateShop("Animals"));

            EconomySystem.OnGoldChanged += UpdateGold;
            UpdateGold(EconomySystem.Instance?.CurrentGold ?? 0);
            PopulateShop(_currentCategory);
        }

        private void OnDisable()
        {
            if (_btnClose != null) _btnClose.onClick.RemoveAllListeners();
            if (_tabSeeds != null) _tabSeeds.onClick.RemoveAllListeners();
            if (_tabAnimals != null) _tabAnimals.onClick.RemoveAllListeners();
            EconomySystem.OnGoldChanged -= UpdateGold;
        }

        private void UpdateGold(int amount)
        {
            if (_goldBalanceLabel != null) _goldBalanceLabel.text = $"{amount:N0}g";
        }

        public void PopulateShop(string category)
        {
            _currentCategory = category;
            if (_shopContentContainer == null || _shopItemPrefab == null) return;
            
            // Clear current list
            foreach (Transform child in _shopContentContainer) Destroy(child.gameObject);

            var registry = Resources.FindObjectsOfTypeAll<GameDataRegistrySO>().FirstOrDefault();
            if (registry == null) return;

            if (category == "Seeds")
            {
                foreach (var cropSO in registry.crops)
                {
                    CreateShopItem(cropSO.data.cropId, cropSO.data.cropName, cropSO.data.seedCostGold, cropSO.data.unlockLevel, true);
                }
            }
            else if (category == "Animals")
            {
                foreach (var animalSO in registry.animals)
                {
                    CreateShopItem(animalSO.data.animalId, animalSO.data.animalName, animalSO.data.buyCostGold, animalSO.data.unlockLevel, false);
                }
            }
        }

        private void CreateShopItem(string id, string name, int cost, int unlockLevel, bool isSeed)
        {
            var go = Instantiate(_shopItemPrefab, _shopContentContainer);
            
            TMP_Text nameLabel = go.transform.Find("ItemName")?.GetComponent<TMP_Text>();
            Button buyBtn = go.transform.Find("BuyButton")?.GetComponent<Button>();
            TMP_Text priceLabel = buyBtn?.transform.Find("Text")?.GetComponent<TMP_Text>();
            TMP_Text lockLabel = go.transform.Find("LockLabel")?.GetComponent<TMP_Text>(); // Optional: add LockLabel in Prefab

            if (nameLabel != null) nameLabel.text = name;
            if (priceLabel != null) priceLabel.text = $"{cost}g";

            bool isUnlocked = (LevelSystem.Instance == null) || (LevelSystem.Instance.CurrentLevel >= unlockLevel);
            
            if (buyBtn != null)
            {
                buyBtn.interactable = isUnlocked;
                if (!isUnlocked)
                {
                    if (priceLabel != null) priceLabel.text = $"LVL {unlockLevel}";
                }

                buyBtn.onClick.AddListener(() => {
                    if (isSeed) TryBuySeed(id, cost);
                    else TryBuyAnimal(id, cost);
                });
            }
        }

        private void TryBuySeed(string cropId, int cost)
        {
            if (EconomySystem.Instance != null && EconomySystem.Instance.CanAfford(cost))
            {
                EconomySystem.Instance.AddGold(-cost);
                StorageSystem.Instance.AddItem(cropId, 1);
                Debug.Log($"<color=cyan>[Shop]</color> Bought Seed: {cropId}");
            }
            else
            {
                Debug.LogWarning("[Shop] Not enough gold!");
            }
        }

        private void TryBuyAnimal(string animalId, int cost)
        {
            // Find correct pen
            var pens = Resources.FindObjectsOfTypeAll<AnimalPenView>();
            var targetPen = pens.FirstOrDefault(p => p.gameObject.activeInHierarchy && p.AnimalType.animalId == animalId);

            if (targetPen == null)
            {
                Debug.LogError($"[Shop] No pen found for {animalId}!");
                return;
            }

            if (targetPen.IsFull)
            {
                Debug.LogWarning("[Shop] Pen is full!");
                return;
            }

            if (EconomySystem.Instance != null && EconomySystem.Instance.CanAfford(cost))
            {
                EconomySystem.Instance.AddGold(-cost);
                targetPen.PurchaseAnimalFromShop(); // Cần thêm một hàm Public khác không trừ tiền lại
                Debug.Log($"<color=cyan>[Shop]</color> Bought Animal: {animalId}");
            }
        }
    }
}
