namespace NTVV.UI.Panels
{
    using UnityEngine;
    using TMPro;
    using NTVV.World.Views;

    /// <summary>
    /// Controller for the animal detail panel.
    /// Renamed from AnimalDetailPopup to align with v1 patterns.
    /// </summary>
    public class AnimalDetailPanelController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _animalName;
        [SerializeField] private TMP_Text _growthText;
        [SerializeField] private GameObject _feedButton;
        [SerializeField] private GameObject _sellButton;

        private AnimalView _target;

        public void Setup(AnimalView target)
        {
            _target = target;
            RefreshUI();
        }

        private void RefreshUI()
        {
            if (_target == null) return;
            _animalName.text = _target.CurrentData.animalName;
            _growthText.text = $"GĐ: {_target.CurrentStage}";
            _feedButton.SetActive(_target.IsHungry);
        }

        public void OnFeedClick()
        {
            _target?.Feed();
            RefreshUI();
        }

        public void OnSellClick()
        {
            _target?.Sell();
            gameObject.SetActive(false);
        }
    }
}
