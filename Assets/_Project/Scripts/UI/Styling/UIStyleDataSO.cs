namespace NTVV.UI.Styling
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    /// <summary>
    /// Configuration for a sprite including layout properties.
    /// </summary>
    [System.Serializable]
    public struct SpriteConfig
    {
        public Sprite Sprite;
        public Image.Type ImageType;
        [Range(0.1f, 100f)] public float PixelsPerUnitMultiplier;
        public bool PreserveAspect;

        public static SpriteConfig Default => new SpriteConfig 
        { 
            ImageType = Image.Type.Simple,
            PixelsPerUnitMultiplier = 1f, 
            PreserveAspect = true 
        };
    }

    /// <summary>
    /// Configuration for colors based on interaction states.
    /// </summary>
    [System.Serializable]
    public struct StatefulColor
    {
        public Color Normal;
        public Color Hover;
        public Color Pressed;
        public Color Disabled;

        public static StatefulColor Default => new StatefulColor 
        { 
            Normal = Color.white, 
            Hover = new Color(0.9f, 0.9f, 0.9f), 
            Pressed = new Color(0.7f, 0.7f, 0.7f), 
            Disabled = new Color(0.5f, 0.5f, 0.5f, 0.5f) 
        };
    }

    /// <summary>
    /// Configuration for text styles.
    /// </summary>
    [System.Serializable]
    public struct TextStyleConfig
    {
        public TMP_FontAsset Font;
        public float FontSize;
        public Color Color;
        public FontStyles Style;
        public TextAlignmentOptions Alignment;

        public static TextStyleConfig Default => new TextStyleConfig 
        { 
            FontSize = 24f, 
            Color = Color.white, 
            Style = FontStyles.Normal,
            Alignment = TextAlignmentOptions.Center
        };
    }

    /// <summary>
    /// ScriptableObject to hold UI theme data (Colors, Fonts, Sprites).
    /// </summary>
    [CreateAssetMenu(fileName = "NewUIStyle", menuName = "NTVV/UI/Style Data")]
    public class UIStyleDataSO : ScriptableObject
    {
        [Header("Theme Configuration")]
        [Tooltip("Subfolder name in Resources/UI/ where this theme's overrides are located.")]
        public string themeFolderName = "Default";

        [Header("Brand Colors - Logic Mappings")]
        public Color PrimaryActionColor = new Color(1f, 0.65f, 0f); // Orange (#FFA500)
        public Color CaringActionColor = new Color(0.3f, 0.7f, 0.3f); // Green (#4CAF50)
        public Color WarningColor = Color.red;
        

        [Header("Button State Colors")]
        public StatefulColor PrimaryButtonColors = StatefulColor.Default;
        public StatefulColor SecondaryButtonColors = StatefulColor.Default;
        public StatefulColor CloseButtonColors = StatefulColor.Default;

        [Header("Typography Presets")]
        public TextStyleConfig TitleStyle = TextStyleConfig.Default;
        public TextStyleConfig BodyStyle = TextStyleConfig.Default;
        public TextStyleConfig AmountStyle = TextStyleConfig.Default;
        public TextStyleConfig ButtonTextStyle = TextStyleConfig.Default;

        [Header("Common Containers")]
        public SpriteConfig ButtonBackground = SpriteConfig.Default;
        public SpriteConfig PanelBackground = SpriteConfig.Default;
        public SpriteConfig BannerPopup = SpriteConfig.Default;
        public SpriteConfig ItemFrame = SpriteConfig.Default;

        [Header("Resource Icons")]
        public SpriteConfig GoldIcon = SpriteConfig.Default;
        public SpriteConfig EnergyIcon = SpriteConfig.Default;
        public SpriteConfig StorageIcon = SpriteConfig.Default;
        public SpriteConfig XPIcon = SpriteConfig.Default;
        public SpriteConfig LevelIcon = SpriteConfig.Default;

        [Header("General Colors")]
        public Color PanelHeaderColor = new Color(0.2f, 0.2f, 0.2f);
        public Color BackgroundDimColor = new Color(0f, 0f, 0f, 0.5f);

        [Header("Layout & HUD Specifics")]
        public LayoutConfig ChipLayout = new LayoutConfig { Padding = new Vector4(16, 16, 8, 8), Spacing = 8 };
        public float ChipIconSize = 40f;
        public float ChipHeight = 60f;

        [Header("World UI (Billboards)")]
        public Color WorldNameplateColor = Color.white;
        public float WorldLabelScale = 0.1f;
    }

    [System.Serializable]
    public struct LayoutConfig
    {
        public Vector4 Padding; // x=Left, y=Right, z=Top, w=Bottom
        public float Spacing;
    }
}
