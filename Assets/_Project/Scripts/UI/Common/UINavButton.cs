namespace NTVV.UI.Common
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    /// <summary>
    /// A simple UI component for navigation buttons.
    /// Manages the label and icon of the button.
    /// Follows ui-standardization suffix naming conventions.
    /// </summary>
    public class UINavButton : MonoBehaviour
    {
        [SerializeField] private Image _nav_Icon;
        [SerializeField] private TMP_Text _nav_Label;
        [SerializeField] private Button _nav_Button;

        public Button Button => _nav_Button;

        private void Awake()
        {
            // Tier 1: Recursive Auto-Wiring (Self-Healing)
            if (_nav_Icon == null) _nav_Icon = FindNamed<Image>("Nav_Icon");
            if (_nav_Label == null) _nav_Label = FindNamed<TMP_Text>("Nav_Label");
            if (_nav_Button == null) _nav_Button = GetComponent<Button>();
        }

        public void SetLabel(string text)
        {
            if (_nav_Label != null) _nav_Label.text = text;
        }

        public void SetIcon(Sprite sprite)
        {
            if (_nav_Icon != null) _nav_Icon.sprite = sprite;
        }

        private T FindNamed<T>(string exactName) where T : Component
        {
            foreach (Transform t in GetComponentsInChildren<Transform>(true))
                if (t.name == exactName)
                    return t.GetComponent<T>();
            return null;
        }
    }
}
