namespace NTVV.UI.Common
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    /// <summary>
    /// A simple UI component that displays an icon and a text value.
    /// Used for HUD resource chips (Gold, Storage, etc.).
    /// </summary>
    public class UIResourceChip : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _label;

        public void SetValue(string text)
        {
            if (_label != null) _label.text = text;
        }

        public void SetIcon(Sprite sprite)
        {
            if (_icon != null) _icon.sprite = sprite;
        }
    }
}
