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

        public float GetXPProgress()
        {
            if (_levelData == null || _levelData.Milestones == null || _levelData.Milestones.Count == 0) return 0f;

            int prevXp = 0;
            int nextXp = 0;

            foreach (var milestone in _levelData.Milestones)
            {
                if (milestone.Level == _currentLevel) prevXp = milestone.XPRequired;
                if (milestone.Level == _currentLevel + 1) nextXp = milestone.XPRequired;
            }

            if (nextXp == 0) return 1f; // Max Level reached

            float progress = (float)(_currentXP - prevXp) / (nextXp - prevXp);
            return Mathf.Clamp01(progress);
        }

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
                    // [Quest] Báo cáo thăng cấp
                    NTVV.Gameplay.Quests.QuestEvents.InvokeActionPerformed(Data.QuestActionType.ReachLevel, "Player", _currentLevel);

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
