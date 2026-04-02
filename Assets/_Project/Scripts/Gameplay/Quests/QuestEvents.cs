using System;
using NTVV.Data;

namespace NTVV.Gameplay.Quests
{
    /// <summary>
    /// Static messaging system for Quest actions.
    /// Decouples gameplay logic (Harvest) from Quest logic (Process progress).
    /// </summary>
    public static class QuestEvents
    {
        /// <summary>
        /// Triggered when a quest-relevant action is performed.
        /// (QuestActionType action, string targetId, int amount)
        /// </summary>
        public static event Action<QuestActionType, string, int> OnActionPerformed;

        public static void InvokeActionPerformed(QuestActionType action, string targetId, int amount)
        {
            OnActionPerformed?.Invoke(action, targetId, amount);
        }

        /// <summary>
        /// Triggered when a quest state changes (Accept, Complete, Claim).
        /// </summary>
        public static event Action<string> OnQuestStateChanged; // QuestID

        public static void InvokeQuestStateChanged(string questId)
        {
            OnQuestStateChanged?.Invoke(questId);
        }
    }
}
