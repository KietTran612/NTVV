using UnityEngine;
using System.Collections.Generic;
using NTVV.Data;

namespace NTVV.Data.ScriptableObjects
{
    /// <summary>
    /// Config for a specific Quest (ScriptableObject).
    /// </summary>
    [CreateAssetMenu(fileName = "New Quest", menuName = "NTVV/Data/Quest")]
    public class QuestDataSO : ScriptableObject
    {
        [Header("Basic Info")]
        public string questId;
        public string questName;
        [TextArea(3, 5)] public string questDescription;
        public int minLevelRequired = 1;
        public string prerequisiteQuestId;

        [Header("Objectives")]
        public List<QuestObjective> objectives = new List<QuestObjective>();

        [Header("Rewards")]
        public QuestReward rewards;

        [Header("Visuals (Optional)")]
        public Sprite questIcon;
    }
}
