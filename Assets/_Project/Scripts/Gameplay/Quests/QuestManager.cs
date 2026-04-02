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
    /// </summary>
    public class QuestManager : Singleton<QuestManager>
    {
        [Header("State")]
        [SerializeField] private List<QuestDataSO> _activeQuests = new List<QuestDataSO>();
        [SerializeField] private HashSet<string> _completedQuestIds = new HashSet<string>();

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
        /// Accept a new quest.
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

            _activeQuests.Add(quest);
            Debug.Log($"<color=cyan>[Quest Accepted]</color>: {quest.questName}");
            QuestEvents.InvokeQuestStateChanged(quest.questId);
        }

        private void HandleActionPerformed(QuestActionType action, string targetId, int amount)
        {
            bool anyChanged = false;
            foreach (var quest in _activeQuests)
            {
                for (int i = 0; i < quest.objectives.Count; i++)
                {
                    var objective = quest.objectives[i];
                    if (objective.actionType == action && (string.IsNullOrEmpty(targetId) || objective.targetId == targetId))
                    {
                        if (!objective.IsReached)
                        {
                            objective.currentAmount += amount;
                            quest.objectives[i] = objective;
                            anyChanged = true;
                            Debug.Log($"[Quest Update] {quest.questName}: {objective.currentAmount}/{objective.requiredAmount}");
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
            return quest.objectives.All(o => o.IsReached);
        }

        public void ClaimReward(QuestDataSO quest)
        {
            if (!_activeQuests.Contains(quest) || !IsQuestComplete(quest)) return;

            // Grant rewards
            if (EconomySystem.Instance != null) EconomySystem.Instance.AddGold(quest.rewards.goldReward);
            if (LevelSystem.Instance != null) LevelSystem.Instance.AddXP(quest.rewards.xpReward);

            // Handle feature unlocks
            HandleUnlock(quest.rewards.unlockType, quest.rewards.unlockId);

            // Move to completed
            _activeQuests.Remove(quest);
            _completedQuestIds.Add(quest.questId);

            Debug.Log($"<color=green>[Quest Reward Claimed]</color>: {quest.questName}");
            QuestEvents.InvokeQuestStateChanged(quest.questId);
        }

        private void HandleUnlock(QuestUnlockType unlockType, string unlockId)
        {
            if (unlockType == QuestUnlockType.None) return;
            
            Debug.Log($"<color=magenta>[Feature Unlocked]</color>: {unlockType} ({unlockId})");
        }

        #region Persistence
        public void LoadData(List<QuestSaveData> activeSaves, List<string> completedIds, GameDataRegistrySO registry)
        {
            _activeQuests.Clear();
            _completedQuestIds.Clear();

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
                        // Restore progress (Note: This modifies the ScriptableObject instance in memory, 
                        // which is fine for runtime but we should be careful with original assets).
                        for (int i = 0; i < questSO.objectives.Count && i < s.objectiveProgress.Count; i++)
                        {
                            var obj = questSO.objectives[i];
                            obj.currentAmount = s.objectiveProgress[i];
                            questSO.objectives[i] = obj;
                        }
                        _activeQuests.Add(questSO);
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
                var qSave = new QuestSaveData
                {
                    questId = quest.questId,
                    objectiveProgress = quest.objectives.Select(o => o.currentAmount).ToList()
                };
                data.activeQuests.Add(qSave);
            }

            data.completedQuestIds = _completedQuestIds.ToList();
        }
        #endregion
    }
}
