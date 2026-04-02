using UnityEngine;
using System.Collections.Generic;
using NTVV.Data.ScriptableObjects;
using NTVV.UI.Common;
using NTVV.Gameplay.Quests;

namespace NTVV.World.Interactions
{
    /// <summary>
    /// Component for NPC or Notice Board to give quests.
    /// </summary>
    public class QuestGiver : MonoBehaviour
    {
        [Header("Config")]
        public string giverName = "NPC";
        public List<QuestDataSO> availableQuests = new List<QuestDataSO>();

        public void Interact()
        {
            Debug.Log($"<color=cyan>[QuestGiver]</color> Interacting with {giverName}");
            
            // Logic: Open Quest Panel
            if (PopupManager.Instance != null)
            {
                // We'll pass the list of quests to the UI eventually.
                // For now, just show the Quest panel.
                PopupManager.Instance.ShowScreen("Quests");
            }
        }
    }
}
