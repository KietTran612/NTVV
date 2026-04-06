namespace NTVV.UI.Common
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A generic UI component for progress bars.
    /// Controls the fill amount of an Image.
    /// </summary>
    public class UIProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;

        public void SetFill(float amount)
        {
            if (_fillImage != null) _fillImage.fillAmount = amount;
        }

        public Image FillImage => _fillImage;
    }
}
