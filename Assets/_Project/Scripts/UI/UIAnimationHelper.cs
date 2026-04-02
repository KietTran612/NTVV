namespace NTVV.UI
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;

    /// <summary>
    /// Static helper for smooth UI animations without external plugins.
    /// handles Pop-in and Button bounce effects.
    /// </summary>
    public class UIAnimationHelper : MonoBehaviour
    {
        public static UIAnimationHelper Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Pop-in effect (Scale 0 to 1.1 to 1.0).
        /// </summary>
        public void PopIn(RectTransform target, float duration = 0.3f)
        {
            target.localScale = Vector3.zero;
            StopAllCoroutines();
            StartCoroutine(PopInCoroutine(target, duration));
        }

        private IEnumerator PopInCoroutine(RectTransform target, float duration)
        {
            float elapsed = 0;
            // Phase 1: 0 to 1.05
            while (elapsed < duration * 0.7f)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / (duration * 0.7f);
                target.localScale = Vector3.one * Mathf.Lerp(0, 1.05f, t);
                yield return null;
            }
            // Phase 2: 1.05 to 1.0
            elapsed = 0;
            while (elapsed < duration * 0.3f)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / (duration * 0.3f);
                target.localScale = Vector3.one * Mathf.Lerp(1.05f, 1.0f, t);
                yield return null;
            }
            target.localScale = Vector3.one;
        }

        /// <summary>
        /// Simple bounce effect for buttons (Scale 1.0 to 0.9 to 1.0).
        /// </summary>
        public void ButtonBounce(RectTransform target)
        {
            StartCoroutine(ButtonBounceCoroutine(target));
        }

        private IEnumerator ButtonBounceCoroutine(RectTransform target)
        {
            float duration = 0.1f;
            float elapsed = 0;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / duration;
                target.localScale = Vector3.one * Mathf.Lerp(1.0f, 0.92f, t);
                yield return null;
            }
            elapsed = 0;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / duration;
                target.localScale = Vector3.one * Mathf.Lerp(0.92f, 1.0f, t);
                yield return null;
            }
            target.localScale = Vector3.one;
        }
    }
}
