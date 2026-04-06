namespace NTVV.UI.Common
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    /// <summary>
    /// A simple UI component for navigation buttons.
    /// Manages the label and icon of the button.
    /// </summary>
    public class UINavButton : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Button _button;

        public Button Button => _button;

        public void SetLabel(string text)
        {
            if (_label != null) _label.text = text;
        }

        public void SetIcon(Sprite sprite)
        {
            if (_icon != null) _icon.sprite = sprite;
        }
    }
}
