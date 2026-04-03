namespace NTVV.UI.Styling
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    /// <summary>
    /// Component to automatically apply styles from UIStyleDataSO.
    /// This ensures all UI elements stay consistent with the theme.
    /// </summary>
    public class UIStyleApplier : MonoBehaviour
    {
        public enum StyleType
        {
            PrimaryAction,    // Orange
            CaringAction,     // Green
            Warning,          // Red
            Gold,             // Yellow
            Header,
            BackgroundDim,
            BodyText
        }

        [Header("Style Strategy")]
        [SerializeField] private StyleType _styleType;
        [SerializeField] private bool _applyColor = true;
        [SerializeField] private bool _applyFont = true;
        [SerializeField] private bool _applySprite = true;

        [Header("Optional Internal References")]
        [SerializeField] private Image _targetImage;
        [SerializeField] private TMP_Text _targetText;

        private void Awake()
        {
            if (_targetImage == null) _targetImage = GetComponent<Image>();
            if (_targetText == null) _targetText = GetComponent<TMP_Text>();
        }

        /// <summary>
        /// Apply style using the provided style data.
        /// Can be called from the Inspector or by a UI Manager.
        /// </summary>
        public void ApplyStyle(UIStyleDataSO style)
        {
            if (style == null) return;

            if (_targetImage != null)
            {
                if (_applyColor) _targetImage.color = GetColor(style);
                if (_applySprite) _targetImage.sprite = GetSprite(style);
            }

            if (_targetText != null)
            {
                if (_applyColor) _targetText.color = GetColor(style);
                if (_applyFont)
                {
                    _targetText.font = style.MainFont;
                    _targetText.fontSize = (_styleType == StyleType.Header) ? style.HeaderFontSize : style.BodyFontSize;
                }
            }
        }

        /// <summary>
        /// Update the style type and re-apply.
        /// </summary>
        public void ChangeStyle(StyleType newType, UIStyleDataSO style)
        {
            _styleType = newType;
            ApplyStyle(style);
        }

        private Color GetColor(UIStyleDataSO style)
        {
            switch (_styleType)
            {
                case StyleType.PrimaryAction: return style.PrimaryActionColor;
                case StyleType.CaringAction: return style.CaringActionColor;
                case StyleType.Warning: return style.WarningColor;
                case StyleType.Gold: return style.GoldColor;
                case StyleType.Header: return style.PanelHeaderColor;
                case StyleType.BackgroundDim: return style.BackgroundDimColor;
                default: return Color.white;
            }
        }

        private Sprite GetSprite(UIStyleDataSO style)
        {
            switch (_styleType)
            {
                case StyleType.PrimaryAction:
                case StyleType.CaringAction:
                case StyleType.Gold:
                    return style.ButtonBackground;
                case StyleType.Header:
                case StyleType.BodyText:
                    return style.PanelBackground;
                default: return null;
            }
        }
    }
}
