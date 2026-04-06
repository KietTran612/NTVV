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
            BodyText,
            ResourceChip      // NEW: Specific type for Chips
        }

        [Header("Style Strategy")]
        [SerializeField] private StyleType _styleType;
        [SerializeField] private bool _applyColor = true;
        [SerializeField] private bool _applyFont = true;
        [SerializeField] private bool _applySprite = true;
        [SerializeField] private bool _applyLayout = true; // NEW: Control layout application

        [Header("Optional Internal References")]
        [SerializeField] private Image _targetImage;
        [SerializeField] private TMP_Text _targetText;
        [SerializeField] private LayoutGroup _targetLayout;

        private void Awake()
        {
            if (_targetImage == null) _targetImage = GetComponent<Image>();
            if (_targetText == null) _targetText = GetComponent<TMP_Text>();
            if (_targetLayout == null) _targetLayout = GetComponent<LayoutGroup>();
        }

        public void ApplyStyle(UIStyleDataSO style)
        {
            if (style == null) return;

            // Apply Visuals
            if (_targetImage != null)
            {
                if (_applyColor) _targetImage.color = GetColor(style);
                if (_applySprite) {
                    Sprite s = GetSprite(style);
                    if (s != null) _targetImage.sprite = s;
                }
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

            // Apply Layout (NEW)
            if (_applyLayout && _targetLayout != null)
            {
                ApplyLayoutSettings(style);
            }
        }

        private void ApplyLayoutSettings(UIStyleDataSO style)
        {
            if (_styleType != StyleType.ResourceChip || _targetLayout == null) return;

            var config = style.ChipLayout;
            _targetLayout.padding = new RectOffset((int)config.Padding.x, (int)config.Padding.y, (int)config.Padding.z, (int)config.Padding.w);

            if (_targetLayout is HorizontalOrVerticalLayoutGroup hvLayout)
            {
                hvLayout.spacing = config.Spacing;
            }
        }

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
                case StyleType.Gold: 
                case StyleType.ResourceChip: return style.GoldColor;
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
                case StyleType.ResourceChip:
                    return style.ButtonBackground;
                case StyleType.Header:
                case StyleType.BodyText:
                    return style.PanelBackground;
                default: return null;
            }
        }
    }
}
