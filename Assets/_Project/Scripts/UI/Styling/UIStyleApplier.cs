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
            ResourceChip,
            Gold,
            Energy,
            Storage,
            XP,
            Level,
            PrimaryAction,
            CaringAction,
            Warning,
            Header,
            BackgroundDim,
            BodyText,
            BannerPopup
        }

        [Header("Style Strategy")]
        [SerializeField] private StyleType _styleType;
        [SerializeField] private bool _applyColor = true;
        [SerializeField] private bool _applyFont = true;
        [SerializeField] private bool _applySprite = true;
        [SerializeField] private bool _applyLayout = true;

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
                
                if (_applySprite) 
                {
                    SpriteConfig config = GetSpriteConfig(style);
                    if (config.Sprite != null) 
                    {
                        _targetImage.sprite = config.Sprite;
                        
                        // Apply advanced configuration from SpriteConfig
                        _targetImage.type = config.ImageType;
                        _targetImage.pixelsPerUnitMultiplier = config.PixelsPerUnitMultiplier;
                        _targetImage.preserveAspect = config.PreserveAspect;
                    }
                }
            }

            if (_targetText != null)
            {
                if (_applyColor) _targetText.color = GetColor(style);
                if (_applyFont)
                {
                    TextStyleConfig config = GetTextStyleConfig(style);
                    _targetText.font = config.Font;
                    _targetText.fontSize = config.FontSize;
                    _targetText.fontStyle = config.Style;
                }
            }

            // Apply Layout
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
                case StyleType.Energy:
                case StyleType.XP:
                case StyleType.Storage:
                case StyleType.Level:
                case StyleType.ResourceChip: return Color.white;
                case StyleType.Header: return style.PanelHeaderColor;
                case StyleType.BannerPopup: return Color.white;
                case StyleType.BackgroundDim: return style.BackgroundDimColor;
                default: return Color.white;
            }
        }

        private SpriteConfig GetSpriteConfig(UIStyleDataSO style)
        {
            switch (_styleType)
            {
                case StyleType.PrimaryAction:
                case StyleType.CaringAction:
                case StyleType.Gold:
                    return style.ButtonBackground;
                case StyleType.ResourceChip:
                    return style.ItemFrame;
                case StyleType.Energy: return style.EnergyIcon;
                case StyleType.XP: return style.XPIcon;
                case StyleType.Storage: return style.StorageIcon;
                case StyleType.Level: return style.LevelIcon;
                case StyleType.Header:
                case StyleType.BodyText:
                    return style.PanelBackground;
                case StyleType.BannerPopup:
                    return style.BannerPopup;
                default: return default;
            }
        }

        private TextStyleConfig GetTextStyleConfig(UIStyleDataSO style)
        {
            switch (_styleType)
            {
                case StyleType.Header:
                case StyleType.BannerPopup:
                    return style.TitleStyle;
                case StyleType.Gold:
                case StyleType.Energy:
                case StyleType.XP:
                case StyleType.Level:
                    return style.AmountStyle;
                case StyleType.PrimaryAction:
                case StyleType.CaringAction:
                case StyleType.Warning:
                    return style.ButtonTextStyle;
                default:
                    return style.BodyStyle;
            }
        }
    }
}
