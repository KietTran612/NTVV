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
    /// Controller for the centralised Shop panel (Seeds and future categories).
    /// </summary>
    public class ShopPanelController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text _goldBalanceLabel;
        [SerializeField] private TMP_Text _gemsBalanceLabel;
        [SerializeField] private Transform _shopContentContainer;
        [SerializeField] private Button _btnClose;
        [SerializeField] private Button _btnRefresh;
        [SerializeField] private Button _tabSeeds;
        /// <summary>Special tab — kept in hierarchy but not yet functional.</summary>
        [SerializeField] private Button _tabSpecial;

        [Header("Settings")]
        [SerializeField] private int _refreshCostGold = 50;

        [Header("Templates")]
        [SerializeField] private GameObject _shopItemPrefab;

        private string _currentCategory = "Seeds";

        private void OnEnable()
        {
            if (_btnClose   != null) _btnClose.onClick.AddListener(() => PopupManager.Instance.CloseActiveModal());
            if (_tabSeeds   != null) _tabSeeds.onClick.AddListener(() => PopulateShop("Seeds"));
            if (_btnRefresh != null) _btnRefresh.onClick.AddListener(TryRefreshItems);

            // Special tab is intentionally locked for the current build
            if (_tabSpecial != null) _tabSpecial.interactable = false;

            EconomySystem.OnGoldChanged += UpdateGold;
            EconomySystem.OnGemsChanged += UpdateGems;

            UpdateGold(EconomySystem.Instance?.CurrentGold ?? 0);
            UpdateGems(EconomySystem.Instance?.CurrentGems ?? 0);
            PopulateShop(_currentCategory);
        }

        private void OnDisable()
        {
            if (_btnClose   != null) _btnClose.onClick.RemoveAllListeners();
            if (_tabSeeds   != null) _tabSeeds.onClick.RemoveAllListeners();
            if (_btnRefresh != null) _btnRefresh.onClick.RemoveAllListeners();
            
            EconomySystem.OnGoldChanged -= UpdateGold;
            EconomySystem.OnGemsChanged -= UpdateGems;
        }

        private void UpdateGold(int amount)
        {
            if (_goldBalanceLabel != null) _goldBalanceLabel.text = $"{amount:N0}";
        }

        private void UpdateGems(int amount)
        {
            if (_gemsBalanceLabel != null) _gemsBalanceLabel.text = $"{amount:N0}";
        }

        // ── Shop population ───────────────────────────────────────────────────

        public void PopulateShop(string category)
        {
            _currentCategory = category;
            if (_shopContentContainer == null || _shopItemPrefab == null) return;

            foreach (Transform child in _shopContentContainer) Destroy(child.gameObject);

            var registry = Resources.FindObjectsOfTypeAll<GameDataRegistrySO>().FirstOrDefault();
            if (registry == null)
            {
                Debug.LogError("[Shop] GameDataRegistrySO not found in Resources!");
                return;
            }

            if (category == "Seeds")
            {
                foreach (var cropSO in registry.crops)
                    CreateShopItem(cropSO.data.cropId, cropSO.data.cropName,
                                   cropSO.data.seedCostGold, cropSO.data.unlockLevel,
                                   cropSO.data.seedIcon, isSeed: true);
            }
            // "Special" category intentionally unimplemented for v1
        }

        private void CreateShopItem(string id, string name, int unitCost, int unlockLevel, Sprite icon, bool isSeed)
        {
            var go    = Instantiate(_shopItemPrefab, _shopContentContainer);
            var entry = go.GetComponent<ShopEntryController>();

            if (entry == null)
            {
                Debug.LogError($"[Shop] Prefab '{_shopItemPrefab.name}' is missing ShopEntryController!");
                return;
            }

            bool isUnlocked = (LevelSystem.Instance == null) ||
                              (LevelSystem.Instance.CurrentLevel >= unlockLevel);

            entry.Initialize(id, name, unitCost, icon, (clickedId, qty) =>
            {
                if (isUnlocked)
                {
                    if (isSeed) TryBuySeed(id, unitCost, qty);
                }
                else
                {
                    Debug.LogWarning($"[Shop] '{name}' is locked until level {unlockLevel}");
                }
            });
        }
        // ── Refresh Logic ─────────────────────────────────────────────────────

        private void TryRefreshItems()
        {
            if (EconomySystem.Instance != null && EconomySystem.Instance.CanAfford(_refreshCostGold))
            {
                EconomySystem.Instance.AddGold(-_refreshCostGold);
                // Implementation note: v1 uses static list from registry,
                // Shuffle logic would go here if registry supported filtering.
                PopulateShop(_currentCategory);
                Managers.GameManager.Instance?.TriggerSave();
                Debug.Log($"<color=green>[Shop]</color> Refreshed list for {_refreshCostGold}g");
            }
            else
            {
                Debug.LogWarning("[Shop] Not enough gold to refresh!");
            }
        }

        // ── Economy operations ────────────────────────────────────────────────

        private void TryBuySeed(string cropId, int unitCost, int qty)
        {
            int totalCost = unitCost * qty;

            if (EconomySystem.Instance != null && EconomySystem.Instance.CanAfford(totalCost))
            {
                EconomySystem.Instance.AddGold(-totalCost);
                StorageSystem.Instance.AddItem(cropId, qty);
                NTVV.Gameplay.Quests.QuestEvents.InvokeActionPerformed(Data.QuestActionType.BuyItem, cropId, qty);
                Managers.GameManager.Instance?.TriggerSave();
                Debug.Log($"<color=cyan>[Shop]</color> Bought Seed: {cropId} ×{qty} for {totalCost}g");
            }
            else
            {
                Debug.LogWarning("[Shop] Not enough gold!");
            }
        }
    }
}
