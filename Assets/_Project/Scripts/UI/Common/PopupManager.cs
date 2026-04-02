namespace NTVV.UI.Common
{
    using UnityEngine;
    using NTVV.World.Views;
    using NTVV.UI.Panels;
    using NTVV.Core;

    /// <summary>
    /// Singleton manager for screen stack and modal popups.
    /// Renamed from UIManager to PopupManager to align with v1 patterns.
    /// </summary>
    public class PopupManager : Singleton<PopupManager>
    {
        [Header("uGUI Layers")]
        [SerializeField] private Canvas _mainOverlayCanvas;
        [SerializeField] private RectTransform _modalParent;

        [Header("Gameplay Panels")]
        [SerializeField] private CropActionPanelController _cropActionPanel;

        [Header("System Screen Prefabs")]
        [SerializeField] private GameObject _storagePrefab;
        [SerializeField] private GameObject _shopPrefab;
        [SerializeField] private GameObject _alertsPrefab;
        [SerializeField] private GameObject _animalDetailPrefab;

        private GameObject _activeModal;

        protected override void OnInitialize()
        {
            _isPersistent = true;
        }

        #region Action Panels
        public void ShowContextAction(CropTileView target)
        {
            if (_cropActionPanel != null)
            {
                _cropActionPanel.gameObject.SetActive(true);
                _cropActionPanel.Setup(target);
            }
        }

        public void ShowContextAction(AnimalPenView target)
        {
            if (_cropActionPanel != null)
            {
                _cropActionPanel.gameObject.SetActive(true);
                _cropActionPanel.Setup(target);
            }
        }

        public void ShowContextAction(AnimalView target)
        {
            if (_cropActionPanel != null)
            {
                _cropActionPanel.gameObject.SetActive(true);
                _cropActionPanel.Setup(target);
            }
        }

        public void CloseContextAction()
        {
            if (_cropActionPanel != null)
            {
                _cropActionPanel.gameObject.SetActive(false);
                // Trigger save when closing action panel (Hybrid Save)
                if (Managers.GameManager.Instance != null)
                    Managers.GameManager.Instance.TriggerSave();
            }
        }
        #endregion

        #region Screen Management
        public void ShowScreen(string screenName)
        {
            if (_activeModal != null) Destroy(_activeModal);

            GameObject prefabToSpawn = null;
            switch (screenName)
            {
                case "Storage": prefabToSpawn = _storagePrefab; break;
                case "Shop": prefabToSpawn = _shopPrefab; break;
                case "Alerts": prefabToSpawn = _alertsPrefab; break;
                case "Animal": prefabToSpawn = _animalDetailPrefab; break;
            }

            if (prefabToSpawn != null && _modalParent != null)
            {
                _activeModal = Instantiate(prefabToSpawn, _modalParent);
            }
        }

        public void CloseActiveModal()
        {
            if (_activeModal != null)
            {
                Destroy(_activeModal);
                _activeModal = null;
                // Trigger save when closing Shop/Storage/etc. (Hybrid Save)
                if (Managers.GameManager.Instance != null)
                    Managers.GameManager.Instance.TriggerSave();
            }
        }
        #endregion
    }
}
