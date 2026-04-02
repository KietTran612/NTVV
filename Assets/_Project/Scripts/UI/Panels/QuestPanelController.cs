using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using NTVV.Gameplay.Quests;
using NTVV.UI.Common;
using NTVV.UI.Items; // For QuestListItem
using NTVV.Data.ScriptableObjects;
using System.Linq;

namespace NTVV.UI.Panels
{
    /// <summary>
    /// Controller for the Quest List Panel.
    /// Lists active quests and allows claiming rewards.
    /// </summary>
    public class QuestPanelController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform _questListContainer;
        [SerializeField] private Button _btnClose;
        [SerializeField] private TMP_Text _emptyMessage;

        [Header("Templates")]
        [SerializeField] private GameObject _questItemPrefab;

        private void OnEnable()
        {
            if (_btnClose != null) _btnClose.onClick.AddListener(() => PopupManager.Instance.CloseActiveModal());
            QuestEvents.OnQuestStateChanged += RefreshUI;
            RefreshUI("");
        }

        private void OnDisable()
        {
            if (_btnClose != null) _btnClose.onClick.RemoveAllListeners();
            QuestEvents.OnQuestStateChanged -= RefreshUI;
        }

        public void RefreshUI(string questId)
        {
            if (_questListContainer == null || _questItemPrefab == null) return;

            // Clear current list
            foreach (Transform child in _questListContainer) Destroy(child.gameObject);

            var activeQuests = QuestManager.Instance.ActiveQuests;
            
            if (activeQuests == null || activeQuests.Count == 0)
            {
                if (_emptyMessage != null) _emptyMessage.gameObject.SetActive(true);
                return;
            }

            if (_emptyMessage != null) _emptyMessage.gameObject.SetActive(false);

            foreach (var quest in activeQuests)
            {
                var go = Instantiate(_questItemPrefab, _questListContainer);
                var item = go.GetComponent<QuestListItem>();
                if (item != null)
                {
                    item.Setup(quest);
                }
            }
        }
    }
}
