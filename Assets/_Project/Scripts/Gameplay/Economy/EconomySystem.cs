namespace NTVV.Gameplay.Economy
{
    using UnityEngine;
    using System;
    using NTVV.Core;

    /// <summary>
    /// System for managing player currency (Gold).
    /// Inherits from generic Singleton.
    /// </summary>
    public class EconomySystem : Singleton<EconomySystem>
    {
        public static event Action<int> OnGoldChanged;
        public static event Action<int> OnGemsChanged;

        [Header("State")]
        [SerializeField] private int _currentGold = 100;
        [SerializeField] private int _currentGems = 25;

        public int CurrentGold => _currentGold;
        public int CurrentGems => _currentGems;

        protected override void OnInitialize()
        {
            _isPersistent = true;
        }

        public void AddGold(int amount)
        {
            _currentGold += amount;
            OnGoldChanged?.Invoke(_currentGold);
            Debug.Log($"<color=yellow>Gold Changed:</color> {amount}. Total: {_currentGold}");
        }

        public void AddGems(int amount)
        {
            _currentGems += amount;
            OnGemsChanged?.Invoke(_currentGems);
            Debug.Log($"<color=cyan>Gems Changed:</color> {amount}. Total: {_currentGems}");
        }

        /// <summary>
        /// Update values (used during save load initialization).
        /// </summary>
        public void SetGold(int amount)
        {
            _currentGold = amount;
            RefreshUI();
        }

        public void SetGems(int amount)
        {
            _currentGems = amount;
            RefreshUI();
        }

        public bool CanAfford(int amount)
        {
            return _currentGold >= amount;
        }

        public bool CanAffordGems(int amount)
        {
            return _currentGems >= amount;
        }

        public void RefreshUI()
        {
            OnGoldChanged?.Invoke(_currentGold);
            OnGemsChanged?.Invoke(_currentGems);
        }
    }
}
