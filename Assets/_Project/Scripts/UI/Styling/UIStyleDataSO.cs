namespace NTVV.UI.Styling
{
    using UnityEngine;
    using TMPro;

    /// <summary>
    /// ScriptableObject to hold UI theme data (Colors, Fonts, Sprites).
    /// </summary>
    [CreateAssetMenu(fileName = "NewUIStyle", menuName = "NTVV/UI/Style Data")]
    public class UIStyleDataSO : ScriptableObject
    {
        [Header("Theme Configuration")]
        [Tooltip("Subfolder name in Resources/UI/ where this theme's overrides are located.")]
        public string themeFolderName = "Default";

        [Header("Colors - Action Primary")]
        public Color PrimaryActionColor = new Color(1f, 0.65f, 0f); // Orange (#FFA500)
        public Color CaringActionColor = new Color(0.3f, 0.7f, 0.3f); // Green (#4CAF50)
        public Color WarningColor = Color.red;
        public Color GoldColor = new Color(1f, 0.84f, 0f); // Sun Yellow

        [Header("General Colors")]
        public Color PanelHeaderColor = new Color(0.2f, 0.2f, 0.2f);
        public Color BackgroundDimColor = new Color(0f, 0f, 0f, 0.5f);

        [Header("Typography")]
        public TMP_FontAsset MainFont;
        public float HeaderFontSize = 36f;
        public float BodyFontSize = 24f;

        [Header("Common Sprites")]
        public Sprite ButtonBackground;
        public Sprite PanelBackground;
        public Sprite ItemFrame;
    }
}
