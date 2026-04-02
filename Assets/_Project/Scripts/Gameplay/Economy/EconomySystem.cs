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

        [Header("State")]
        [SerializeField] private int _currentGold = 100;

        public int CurrentGold => _currentGold;

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

        /// <summary>
        /// Update gold value (used during save load initialization).
        /// </summary>
        public void SetGold(int amount)
        {
            _currentGold = amount;
            RefreshUI();
        }

        public bool CanAfford(int amount)
        {
            return _currentGold >= amount;
        }

        public void RefreshUI()
        {
            OnGoldChanged?.Invoke(_currentGold);
        }
    }
}
