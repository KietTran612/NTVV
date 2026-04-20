using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using NTVV.Core;
using NTVV.Data;
using NTVV.Data.ScriptableObjects;
using NTVV.Gameplay.Economy;
using NTVV.Gameplay.Progression;

namespace NTVV.Gameplay.Quests
{
    /// <summary>
    /// Singleton Manager for the Quest system.
    /// Tracks active quests and progress.
    /// FIX: Progress is stored in a runtime Dictionary (_questProgress) 
    /// instead of mutating ScriptableObject assets directly.
    /// This prevents quest data corruption in Editor Play Mode.
    /// </summary>
    public class QuestManager : Singleton<QuestManager>
    {
        [Header("State")]
        [SerializeField] private List<QuestDataSO> _activeQuests = new List<QuestDataSO>();
        [SerializeField] private HashSet<string> _completedQuestIds = new HashSet<string>();

        // Runtime progress tracker: questId -> list of objective currentAmounts
        // SO assets are NEVER modified — they are read-only templates.
        private Dictionary<string, List<int>> _questProgress = new Dictionary<string, List<int>>();

        public List<QuestDataSO> ActiveQuests => _activeQuests;
        public HashSet<string> CompletedQuestIds => _completedQuestIds;

        protected override void OnInitialize()
        {
            _isPersistent = true;
            QuestEvents.OnActionPerformed += HandleActionPerformed;
        }

        private void OnDestroy()
        {
            QuestEvents.OnActionPerformed -= HandleActionPerformed;
        }

        /// <summary>
        /// Accept a new quest. Initializes runtime progress for all objectives.
        /// </summary>
        public void AcceptQuest(QuestDataSO quest)
        {
            if (quest == null || _activeQuests.Contains(quest) || _completedQuestIds.Contains(quest.questId)) return;

            // Check level prerequisite
            if (LevelSystem.Instance != null && LevelSystem.Instance.CurrentLevel < quest.minLevelRequired)
            {
                Debug.LogWarning($"[Quest] Cần cấp {quest.minLevelRequired} để nhận nhiệm vụ {quest.questName}");
                return;
            }

            // Check prerequisite quest (BUG-Q2 fix)
            if (!string.IsNullOrEmpty(quest.prerequisiteQuestId) &&
                !_completedQuestIds.Contains(quest.prerequisiteQuestId))
            {
                Debug.LogWarning($"[Quest] Prerequisite not completed: {quest.prerequisiteQuestId}");
                return;
            }

            _activeQuests.Add(quest);

            // Initialize runtime progress (all zeros) — never touch the SO
            _questProgress[quest.questId] = new List<int>(new int[quest.objectives.Count]);

            Debug.Log($"<color=cyan>[Quest Accepted]</color>: {quest.questName}");
            QuestEvents.InvokeQuestStateChanged(quest.questId);
        }

        private void HandleActionPerformed(QuestActionType action, string targetId, int amount)
        {
            bool anyChanged = false;
            foreach (var quest in _activeQuests)
            {
                if (!_questProgress.TryGetValue(quest.questId, out var progress)) continue;

                for (int i = 0; i < quest.objectives.Count; i++)
                {
                    var objective = quest.objectives[i];
                    if (objective.actionType == action && (string.IsNullOrEmpty(targetId) || objective.targetId == targetId))
                    {
                        int current = i < progress.Count ? progress[i] : 0;
                        if (current < objective.requiredAmount)
                        {
                            // Clamp so we don't exceed required amount
                            progress[i] = Mathf.Min(current + amount, objective.requiredAmount);
                            anyChanged = true;
                            Debug.Log($"[Quest Update] {quest.questName}: {progress[i]}/{objective.requiredAmount}");
                        }
                    }
                }
            }

            if (anyChanged)
            {
                QuestEvents.InvokeQuestStateChanged(""); // Any Quest update
            }
        }

        public bool IsQuestComplete(QuestDataSO quest)
        {
            if (!_questProgress.TryGetValue(quest.questId, out var progress)) return false;

            for (int i = 0; i < quest.objectives.Count; i++)
            {
                int current = i < progress.Count ? progress[i] : 0;
                if (current < quest.objectives[i].requiredAmount) return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the total (current, required) progress across all objectives for display in UI.
        /// </summary>
        public (int current, int required) GetQuestTotalProgress(QuestDataSO quest)
        {
            int totalRequired = 0, totalCurrent = 0;
            _questProgress.TryGetValue(quest.questId, out var progress);

            for (int i = 0; i < quest.objectives.Count; i++)
            {
                totalRequired += quest.objectives[i].requiredAmount;
                totalCurrent += (progress != null && i < progress.Count) ? progress[i] : 0;
            }
            return (totalCurrent, totalRequired);
        }

        public void ClaimReward(QuestDataSO quest)
        {
            if (!_activeQuests.Contains(quest) || !IsQuestComplete(quest)) return;

            // Grant rewards
            if (EconomySystem.Instance != null) EconomySystem.Instance.AddGold(quest.rewards.goldReward);
            if (LevelSystem.Instance != null) LevelSystem.Instance.AddXP(quest.rewards.xpReward);

            // Handle feature unlocks
            HandleUnlock(quest.rewards.unlockType, quest.rewards.unlockId);

            // Move to completed and clear runtime progress
            _activeQuests.Remove(quest);
            _completedQuestIds.Add(quest.questId);
            _questProgress.Remove(quest.questId);

            Debug.Log($"<color=green>[Quest Reward Claimed]</color>: {quest.questName}");
            QuestEvents.InvokeQuestStateChanged(quest.questId);
        }

        private void HandleUnlock(QuestUnlockType unlockType, string unlockId)
        {
            switch (unlockType)
            {
                case QuestUnlockType.None:
                    return;

                case QuestUnlockType.ShopTab_Animals:
                case QuestUnlockType.Building_NewNPC:
                case QuestUnlockType.System_Crafting:
                default:
                    Debug.LogWarning($"[Quest] HandleUnlock: unlockType {unlockType} not supported in v1. Skipping.");
                    break;
            }
        }

        #region Persistence
        public void LoadData(List<QuestSaveData> activeSaves, List<string> completedIds, GameDataRegistrySO registry)
        {
            _activeQuests.Clear();
            _completedQuestIds.Clear();
            _questProgress.Clear();

            if (completedIds != null)
            {
                foreach (var id in completedIds) _completedQuestIds.Add(id);
            }

            if (activeSaves != null && registry != null)
            {
                foreach (var s in activeSaves)
                {
                    var questSO = registry.GetQuest(s.questId);
                    if (questSO != null)
                    {
                        _activeQuests.Add(questSO);

                        // Restore runtime progress — SO is never touched
                        var progress = new List<int>(s.objectiveProgress);
                        while (progress.Count < questSO.objectives.Count) progress.Add(0);
                        _questProgress[questSO.questId] = progress;
                    }
                }
            }
        }

        public void SaveData(PlayerSaveData data)
        {
            if (data == null) return;

            data.activeQuests = new List<QuestSaveData>();
            foreach (var quest in _activeQuests)
            {
                _questProgress.TryGetValue(quest.questId, out var progress);
                var qSave = new QuestSaveData
                {
                    questId = quest.questId,
                    objectiveProgress = progress != null
                        ? new List<int>(progress)
                        : new List<int>(new int[quest.objectives.Count])
                };
                data.activeQuests.Add(qSave);
            }

            data.completedQuestIds = _completedQuestIds.ToList();
        }
        #endregion
    }
}
