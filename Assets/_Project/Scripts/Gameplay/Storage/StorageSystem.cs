namespace NTVV.Gameplay.Storage
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using NTVV.Core;
    using NTVV.Data.ScriptableObjects;
    using NTVV.Gameplay.Economy;
    using NTVV.Gameplay.Progression;
    using NTVV.Managers;

    /// <summary>
    /// System for managing player inventory and storage capacity.
    /// Inherits from generic Singleton.
    /// Follows v2 rules: 99 per slot, capacity-bound.
    /// </summary>
    public class StorageSystem : Singleton<StorageSystem>
    {
        public static event Action<int, int> OnStorageChanged; // SlotsUsed, MaxSlots
        public static event Action<bool> OnNearFullWarning;

        [Header("Config")]
        [SerializeField] private StorageUpgradeDataSO _upgradeConfig;

        [Header("State")]
        [SerializeField] private int _maxCapacity = 50;
        [SerializeField] private int _currentTier = 0;
        private Dictionary<string, int> _items = new Dictionary<string, int>();
        private int _currentSlotsUsed = 0;

        public int CurrentSlotsUsed => _currentSlotsUsed;
        public int MaxCapacity => _maxCapacity;
        public int CurrentTier => _currentTier;
        public bool IsFull => _currentSlotsUsed >= _maxCapacity;
        public StorageUpgradeDataSO UpgradeConfig => _upgradeConfig;

        protected override void OnInitialize()
        {
            _isPersistent = true;

            // Self-healing: if no upgrade config assigned, fetch from Registry
            if (_upgradeConfig == null && GameManager.Instance != null && GameManager.Instance.DataRegistry != null)
            {
                _upgradeConfig = GameManager.Instance.DataRegistry.storageUpgradeConfig;
                if (_upgradeConfig != null) Debug.Log("<color=cyan>[StorageSystem]</color> Automatically linked storage upgrade config from Registry.");
            }
        }

        public bool CanAddItem(string itemId, int quantity)
        {
            if (quantity <= 0) return true;

            int currentOfItem = _items.ContainsKey(itemId) ? _items[itemId] : 0;
            int newTotalOfItem = currentOfItem + quantity;

            int currentSlotsOfItem = CalculateSlotsForItem(currentOfItem);
            int newSlotsOfItem = CalculateSlotsForItem(newTotalOfItem);

            int slotDiff = newSlotsOfItem - currentSlotsOfItem;

            return _currentSlotsUsed + slotDiff <= _maxCapacity;
        }

        public void AddItem(string itemId, int quantity)
        {
            if (quantity > 0 && !CanAddItem(itemId, quantity))
            {
                Debug.LogWarning($"<color=red>Storage Full!</color> Cannot add {itemId}");
                return;
            }

            if (_items.ContainsKey(itemId)) _items[itemId] += quantity;
            else _items[itemId] = quantity;

            if (_items[itemId] <= 0) _items.Remove(itemId);

            RecalculateTotalSlots();
            NotifyStorageChanged();
        }

        /// <summary>
        /// Restore items and capacity (used during save load initialization).
        /// </summary>
        public void LoadData(Dictionary<string, int> items, int maxCapacity, int currentTier)
        {
            _items = items ?? new Dictionary<string, int>();
            _maxCapacity = maxCapacity;
            _currentTier = currentTier;
            RecalculateTotalSlots();
            RefreshUI();
        }

        public int GetItemCount(string itemId)
        {
            return _items.ContainsKey(itemId) ? _items[itemId] : 0;
        }

        public Dictionary<string, int> GetAllItems()
        {
            return new Dictionary<string, int>(_items);
        }

        public bool TryUpgradeStorage()
        {
            if (_upgradeConfig == null || !_upgradeConfig.HasNextTier(_currentTier)) return false;

            var nextTier = _upgradeConfig.GetTier(_currentTier);
            
            // Per-tier Level Check (Skip if -1)
            if (nextTier.minLevelToAccess != -1 && LevelSystem.Instance != null)
            {
                if (LevelSystem.Instance.CurrentLevel < nextTier.minLevelToAccess)
                {
                    Debug.LogWarning($"<color=red>Upgrade Failed:</color> Level {nextTier.minLevelToAccess} required!");
                    return false;
                }
            }

            // Check Gold
            if (EconomySystem.Instance != null && EconomySystem.Instance.CurrentGold >= nextTier.upgradeCostGold)
            {
                EconomySystem.Instance.AddGold(-nextTier.upgradeCostGold);
                _maxCapacity += nextTier.bonusCapacity;
                _currentTier++;
                
                NotifyStorageChanged();
                Debug.Log($"<color=cyan>[Storage]</color> Upgraded to Tier {_currentTier}. Max Capacity: {_maxCapacity}");
                return true;
            }

            Debug.LogWarning("<color=red>Upgrade Failed:</color> Not enough Gold!");
            return false;
        }

        public void UpgradeCapacity(int extraSlots)
        {
            _maxCapacity += extraSlots;
            NotifyStorageChanged();
        }

        private int CalculateSlotsForItem(int count)
        {
            if (count <= 0) return 0;
            return Mathf.CeilToInt(count / 99f); 
        }

        private void RecalculateTotalSlots()
        {
            int slots = 0;
            foreach (var item in _items)
            {
                slots += CalculateSlotsForItem(item.Value);
            }
            _currentSlotsUsed = slots;
        }

        private void NotifyStorageChanged()
        {
            OnStorageChanged?.Invoke(_currentSlotsUsed, _maxCapacity);
            float fillRatio = (float)_currentSlotsUsed / _maxCapacity;
            OnNearFullWarning?.Invoke(fillRatio >= 0.9f);
        }

        public void RefreshUI()
        {
            NotifyStorageChanged();
        }
    }
}
