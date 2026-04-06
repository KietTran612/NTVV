namespace NTVV.UI.Common
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    /// <summary>
    /// A generic UI component for progress bars.
    /// Controls the fill amount of an Image.
    /// Follows ui-standardization suffix naming conventions.
    /// </summary>
    public class UIProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _xp_Fill;
        [SerializeField] private TMP_Text _label;

        private void Awake()
        {
            // Tier 1: Recursive Auto-Wiring (Self-Healing)
            if (_xp_Fill == null) _xp_Fill = FindNamed<Image>("XP_Fill");
            if (_label == null) _label = FindNamed<TMP_Text>("Label");
        }

        public void SetFill(float amount)
        {
            if (_xp_Fill != null) _xp_Fill.fillAmount = amount;
        }

        public Image FillImage => _xp_Fill;

        private T FindNamed<T>(string exactName) where T : Component
        {
            foreach (Transform t in GetComponentsInChildren<Transform>(true))
                if (t.name == exactName)
                    return t.GetComponent<T>();
            return null;
        }
    }
}
