namespace NTVV.UI.Common
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    /// <summary>
    /// A simple UI component that displays an icon and a text value.
    /// Used for HUD resource chips (Gold, Storage, etc.).
    /// Follows ui-standardization suffix naming conventions.
    /// </summary>
    public class UIResourceChip : MonoBehaviour
    {
        [SerializeField] private Image _resource_Icon;
        [SerializeField] private TMP_Text _amount_Label;

        private void Awake()
        {
            // Tier 1: Recursive Auto-Wiring (Self-Healing)
            if (_resource_Icon == null) _resource_Icon = FindNamed<Image>("Resource_Icon");
            if (_amount_Label == null) _amount_Label = FindNamed<TMP_Text>("Amount_Label");
        }

        public void SetValue(string text)
        {
            if (_amount_Label != null) _amount_Label.text = text;
        }

        public void SetIcon(Sprite sprite)
        {
            if (_resource_Icon != null) _resource_Icon.sprite = sprite;
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
