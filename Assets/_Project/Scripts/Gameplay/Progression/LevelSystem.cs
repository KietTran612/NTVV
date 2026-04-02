namespace NTVV.Gameplay.Progression
{
    using UnityEngine;
    using System;
    using NTVV.Data;
    using NTVV.Core;
    using NTVV.Managers;

    /// <summary>
    /// System for managing player level and XP progression.
    /// Inherits from generic Singleton.
    /// Follows v2 milestones.
    /// </summary>
    public class LevelSystem : Singleton<LevelSystem>
    {
        public static event Action<int, int> OnXPChanged; // CurrentXP, CurrentLevel
        public static event Action<int> OnLevelUp;

        [Header("Config")]
        [SerializeField] private PlayerLevelData _levelData;

        [Header("State")]
        [SerializeField] private int _currentLevel = 1;
        [SerializeField] private int _currentXP = 0;

        public int CurrentLevel => _currentLevel;
        public int CurrentXP => _currentXP;

        protected override void OnInitialize()
        {
            _isPersistent = true;
        }

        public void AddXP(int amount)
        {
            _currentXP += amount;
            CheckLevelUp();
            OnXPChanged?.Invoke(_currentXP, _currentLevel);
            Debug.Log($"<color=green>XP Added:</color> {amount}. Total: {_currentXP}");
        }

        /// <summary>
        /// Restore level and XP (used during save load initialization).
        /// </summary>
        public void LoadData(int level, int xp)
        {
            _currentLevel = level;
            _currentXP = xp;
            RefreshUI();
        }

        private void CheckLevelUp()
        {
            if (_levelData == null) return;

            // Simple threshold check for the next level
            foreach (var milestone in _levelData.Milestones)
            {
                if (milestone.Level > _currentLevel && _currentXP >= milestone.XPRequired)
                {
                    _currentLevel = milestone.Level;
                    OnLevelUp?.Invoke(_currentLevel);
                    
                    // Trigger save on milestone reach (Hybrid Save)
                    if (NTVV.Managers.GameManager.Instance != null)
                        NTVV.Managers.GameManager.Instance.TriggerSave();
                        
                    Debug.Log($"<color=green>Level Up!</color> Reached Level {_currentLevel}");
                }
            }
        }

        public void RefreshUI()
        {
            OnXPChanged?.Invoke(_currentXP, _currentLevel);
        }
    }
}
