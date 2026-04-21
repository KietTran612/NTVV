namespace NTVV.UI.Panels
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using NTVV.UI.Common;

    public class LockInfoPopupController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _message_Label;
        [SerializeField] private Button _btnOk;

        private void OnEnable()
        {
            if (_btnOk != null)
                _btnOk.onClick.AddListener(() => PopupManager.Instance?.CloseActiveModal());
        }

        private void OnDisable()
        {
            if (_btnOk != null) _btnOk.onClick.RemoveAllListeners();
        }

        public void Setup(int requiredLevel)
        {
            if (_message_Label != null)
                _message_Label.text = $"Cần Level {requiredLevel} để mở tile này";
        }
    }
}
