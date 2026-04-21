namespace NTVV.UI.Common
{
    using UnityEngine;
    using NTVV.World.Views;
    using NTVV.UI.Panels;
    using NTVV.Core;
    using NTVV.UI.Infrastructure;

    /// <summary>
    /// Singleton manager for screen stack and modal popups.
    /// Updated to use IUIAssetProvider for managed loading.
    /// </summary>
    public class PopupManager : Singleton<PopupManager>
    {
        [Header("uGUI Layers")]
        [SerializeField] private Canvas _mainOverlayCanvas;
        [SerializeField] private RectTransform _modalParent;
        [SerializeField] private RectTransform _hudParent;

        [Header("Context Management")]
        [SerializeField] private Vector3 _contextOffset = new Vector3(0, 100, 0);
        private CropActionPanelController _cropActionPanel;

        private IUIAssetProvider _provider;
        private GameObject _activeModal;

        protected override void OnInitialize()
        {
            _isPersistent = true;
            _provider = new ResourcesUIProvider();
        }

        #region Action Panels
        private void EnsureContextPanel()
        {
            if (_cropActionPanel != null) return;

            // 1. Try to find in scene (e.g. if already spawned or placed by hand)
            _cropActionPanel = FindFirstObjectByType<CropActionPanelController>();
            if (_cropActionPanel != null) return;

            // 2. Load from theme-aware provider
            if (_provider != null)
            {
                GameObject prefab = _provider.LoadPrefab("ContextActionPanel");
                if (prefab != null && _hudParent != null)
                {
                    GameObject instance = Instantiate(prefab, _hudParent);
                    _cropActionPanel = instance.GetComponent<CropActionPanelController>();
                    instance.gameObject.SetActive(false);
                }
            }
        }

        public void ShowContextAction(CropTileView target)
        {
            EnsureContextPanel();
            if (_cropActionPanel != null)
            {
                _cropActionPanel.gameObject.SetActive(true);
                _cropActionPanel.Setup(target);
                
                // Update position (World to Screen)
                PositionContextPanel(target.transform.position);
            }
        }

        public void ShowContextAction(AnimalPenView target)
        {
            EnsureContextPanel();
            if (_cropActionPanel != null)
            {
                _cropActionPanel.gameObject.SetActive(true);
                _cropActionPanel.Setup(target);
                
                PositionContextPanel(target.transform.position);
            }
        }

        public void ShowContextAction(AnimalView target)
        {
            EnsureContextPanel();
            if (_cropActionPanel != null)
            {
                _cropActionPanel.gameObject.SetActive(true);
                _cropActionPanel.Setup(target);
                
                PositionContextPanel(target.transform.position);
            }
        }

        private void PositionContextPanel(Vector3 worldPos)
        {
            if (_cropActionPanel == null || Camera.main == null) return;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            // Apply offset based on screen-space (Y up)
            _cropActionPanel.GetComponent<RectTransform>().position = screenPos + _contextOffset;
        }

        public void CloseContextAction()
        {
            if (_cropActionPanel != null)
            {
                _cropActionPanel.gameObject.SetActive(false);
                if (Managers.GameManager.Instance != null)
                    Managers.GameManager.Instance.TriggerSave();
            }
        }
        #endregion

        #region Screen Management (Managed)
        /// <summary>
        /// Show a screen by its name. 
        /// Prefabs are loaded from Resources/UI/ via Provider.
        /// </summary>
        public void ShowScreen(string screenName)
        {
            if (_activeModal != null) Destroy(_activeModal);

            if (_provider != null)
            {
                // Key naming convention: "ShopPopup", "StoragePopup", etc.
                GameObject prefab = _provider.LoadPrefab(screenName + "Popup");
                if (prefab != null && _modalParent != null)
                {
                    _activeModal = Instantiate(prefab, _modalParent);
                }
            }
        }

        public void CloseActiveModal()
        {
            if (_activeModal != null)
            {
                Destroy(_activeModal);
                _activeModal = null;
                if (Managers.GameManager.Instance != null)
                    Managers.GameManager.Instance.TriggerSave();
            }
        }

        public void ShowLockInfo(int requiredLevel)
        {
            if (_activeModal != null) Destroy(_activeModal);
            GameObject prefab = _provider?.LoadPrefab("LockInfoPopup");
            if (prefab != null && _modalParent != null)
            {
                _activeModal = Instantiate(prefab, _modalParent);
                var ctrl = _activeModal.GetComponent<LockInfoPopupController>();
                ctrl?.Setup(requiredLevel);
            }
            else
            {
                Debug.LogWarning($"[PopupManager] LockInfoPopup prefab not found for level {requiredLevel}");
            }
        }

        #endregion
    }
}
