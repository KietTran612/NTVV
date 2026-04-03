using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NTVV.Data.ScriptableObjects;
using NTVV.Gameplay.Quests;

namespace NTVV.UI.Items
{
    /// <summary>
    /// UI Component for a single quest item in the list.
    /// Handles display and claim button.
    /// </summary>
    public class QuestListItem : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text _questNameLabel;
        [SerializeField] private TMP_Text _questDescLabel;
        [SerializeField] private TMP_Text _progressLabel;
        [SerializeField] private Image _questIcon;
        [SerializeField] private Button _btnClaim;
        [SerializeField] private TMP_Text _rewardLabel;

        private QuestDataSO _questData;

        public void Setup(QuestDataSO quest)
        {
            _questData = quest;
            if (_questNameLabel != null) _questNameLabel.text = quest.questName;
            if (_questDescLabel != null) _questDescLabel.text = quest.questDescription;
            if (_questIcon != null && quest.questIcon != null) _questIcon.sprite = quest.questIcon;

            // Get runtime progress from QuestManager (never read from SO directly)
            var (totalCurrent, totalRequired) = QuestManager.Instance.GetQuestTotalProgress(quest);

            if (_progressLabel != null)
                _progressLabel.text = $"{totalCurrent}/{totalRequired}";

            if (_rewardLabel != null)
                _rewardLabel.text = $"{quest.rewards.goldReward}g / {quest.rewards.xpReward}xp";

            bool isComplete = QuestManager.Instance.IsQuestComplete(quest);
            if (_btnClaim != null)
            {
                _btnClaim.interactable = isComplete;
                _btnClaim.onClick.RemoveAllListeners();
                _btnClaim.onClick.AddListener(() => QuestManager.Instance.ClaimReward(quest));
                
                // Visual feedback for complete
                var btnText = _btnClaim.GetComponentInChildren<TMP_Text>();
                if (btnText != null) btnText.text = isComplete ? "CLAIM" : "DOING";
            }
        }
    }
}
