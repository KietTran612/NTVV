using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NTVV.UI.Styling
{
    /// <summary>
    /// Centralized system to apply UI styles based on naming conventions.
    /// Supports recursive application to GameObjects and Prefabs.
    /// </summary>
    public static class UIStyleProcessor
    {
        public static void ApplyStyle(GameObject root, UIStyleDataSO style)
        {
            if (root == null || style == null) return;

            // Apply to the root itself
            ApplyToInternal(root, style);

            // Apply to all children recursively
            foreach (Transform child in root.transform)
            {
                ApplyStyle(child.gameObject, style);
            }
        }

        private static void ApplyToInternal(GameObject go, UIStyleDataSO style)
        {
            string name = go.name.ToLower();

            // 1. Process Buttons (Prefix: btn_)
            if (name.StartsWith("btn_"))
            {
                var btn = go.GetComponent<Button>();
                if (btn != null)
                {
                    ApplyButtonStyle(btn, style);
                }
            }

            // 2. Process Labels (Suffix: _label)
            if (name.EndsWith("_label"))
            {
                var text = go.GetComponent<TextMeshProUGUI>();
                if (text != null)
                {
                    ApplyTextStyle(text, style);
                }
            }

            // 3. Process Chips (Suffix: _chip)
            if (name.EndsWith("_chip"))
            {
                var img = go.GetComponent<Image>();
                if (img != null)
                {
                    ApplySpriteConfig(img, style.ItemFrame);
                }

                var layout = go.GetComponent<HorizontalOrVerticalLayoutGroup>();
                if (layout != null)
                {
                    ApplyLayoutConfig(layout, style.ChipLayout);
                }
            }

            // 4. Process Icons (Name: resource_icon or contains icon)
            if (name == "resource_icon" || name.Contains("_icon"))
            {
                var img = go.GetComponent<Image>();
                if (img != null)
                {
                    ApplyIconStyle(go, img, style);
                }
            }
        }

        private static void ApplyButtonStyle(Button btn, UIStyleDataSO style)
        {
            string name = btn.gameObject.name.ToLower();
            StatefulColor colors = style.PrimaryButtonColors;

            if (name.Contains("secondary")) colors = style.SecondaryButtonColors;
            else if (name.Contains("close")) colors = style.CloseButtonColors;

            // Apply background sprite
            var img = btn.GetComponent<Image>();
            if (img != null)
            {
                ApplySpriteConfig(img, style.ButtonBackground);
            }

            // Apply colors to transition
            btn.transition = Selectable.Transition.ColorTint;
            var cb = btn.colors;
            cb.normalColor = colors.Normal;
            cb.highlightedColor = colors.Hover;
            cb.pressedColor = colors.Pressed;
            cb.disabledColor = colors.Disabled;
            btn.colors = cb;
        }

        private static void ApplyTextStyle(TextMeshProUGUI text, UIStyleDataSO style)
        {
            string name = text.gameObject.name.ToLower();
            TextStyleConfig config = style.BodyStyle;

            if (name.Contains("title")) config = style.TitleStyle;
            else if (name.Contains("amount") || name.Contains("xp")) config = style.AmountStyle;
            else if (name.Contains("btn") || name.Contains("button")) config = style.ButtonTextStyle;

            text.font = config.Font;
            text.fontSize = config.FontSize;
            text.color = config.Color;
            text.fontStyle = config.Style;
            text.alignment = config.Alignment;
        }

        private static void ApplyIconStyle(GameObject go, Image img, UIStyleDataSO style)
        {
            string name = go.name.ToLower();
            SpriteConfig iconConfig = SpriteConfig.Default;

            if (name.Contains("gold")) iconConfig = style.GoldIcon;
            else if (name.Contains("energy") || name.Contains("lightning")) iconConfig = style.EnergyIcon;
            else if (name.Contains("xp")) iconConfig = style.XPIcon;
            else if (name.Contains("storage")) iconConfig = style.StorageIcon;
            else if (name.Contains("level")) iconConfig = style.LevelIcon;

            if (iconConfig.Sprite != null)
            {
                ApplySpriteConfig(img, iconConfig);
            }
        }

        private static void ApplySpriteConfig(Image img, SpriteConfig config)
        {
            if (config.Sprite == null) return;
            img.sprite = config.Sprite;
            img.type = config.ImageType;
            img.pixelsPerUnitMultiplier = config.PixelsPerUnitMultiplier;
            img.preserveAspect = config.PreserveAspect;
        }

        private static void ApplyLayoutConfig(LayoutGroup layout, LayoutConfig config)
        {
            layout.padding = new RectOffset((int)config.Padding.x, (int)config.Padding.y, (int)config.Padding.z, (int)config.Padding.w);
            if (layout is HorizontalOrVerticalLayoutGroup hv)
            {
                hv.spacing = config.Spacing;
            }
        }
    }
}
