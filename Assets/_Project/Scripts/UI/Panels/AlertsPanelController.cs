namespace NTVV.UI.Panels
{
    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;
    using System.Collections;

    /// <summary>
    /// Controller for the System Alerts Overlay using uGUI.
    /// Manages the queueing and display of dynamic notifications.
    /// Renamed from AlertsController and updated.
    /// </summary>
    public class AlertsPanelController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform _alertQueueContainer;
        
        [Header("Templates")]
        [SerializeField] private GameObject _alertItemPrefab;

        public void ShowAlert(string title, string message, Color? highlightColor = null)
        {
            if (_alertQueueContainer == null || _alertItemPrefab == null) return;

            var alertGo = Instantiate(_alertItemPrefab, _alertQueueContainer);
            
            var titleLabel = alertGo.transform.Find("TitleText")?.GetComponent<TMP_Text>();
            var msgLabel = alertGo.transform.Find("MessageText")?.GetComponent<TMP_Text>();
            var background = alertGo.GetComponent<Image>(); // If you want to color the border/bg

            if (titleLabel != null) 
            {
                titleLabel.text = title;
                titleLabel.color = highlightColor ?? Color.green;
            }
            if (msgLabel != null) msgLabel.text = message;

            StartCoroutine(DismissAlertAfterDelay(alertGo, 3f));
        }

        private IEnumerator DismissAlertAfterDelay(GameObject alertGo, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (alertGo != null)
            {
                Destroy(alertGo);
            }
        }
    }
}
