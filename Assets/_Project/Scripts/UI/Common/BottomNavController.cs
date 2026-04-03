namespace NTVV.UI.Common
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Controller for the bottom navigation bar.
    /// Renamed from BottomNavView.
    /// </summary>
    public class BottomNavController : MonoBehaviour
    {
        [Header("Nav Buttons")]
        [SerializeField] private Button _btnFarm;
        [SerializeField] private Button _btnStorage;
        [SerializeField] private Button _btnShop;
        [SerializeField] private Button _btnBarn;
        [SerializeField] private Button _btnEvent;

        private void Start()
        {
            _btnFarm?.onClick.AddListener(() => OnNavClick("Farm"));
            _btnStorage?.onClick.AddListener(() => OnNavClick("Storage"));
            _btnShop?.onClick.AddListener(() => OnNavClick("Shop"));
            _btnBarn?.onClick.AddListener(() => OnNavClick("Barn"));
            _btnEvent?.onClick.AddListener(() => OnNavClick("Event"));
        }

        private void OnNavClick(string destination)
        {
            Debug.Log($"[BottomNav] Navigating to: {destination}");
            if (PopupManager.Instance != null)
            {
                PopupManager.Instance.ShowScreen(destination);
            }
        }
    }
}
