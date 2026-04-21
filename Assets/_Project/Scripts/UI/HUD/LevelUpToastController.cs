namespace NTVV.UI.HUD
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using System.Collections;
    using NTVV.Gameplay.Progression;

    public class LevelUpToastController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _message_Label;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _displayDuration = 2f;

        private Coroutine _fadeCoroutine;

        private void OnEnable()
        {
            LevelSystem.OnLevelUp += OnLevelUp;
        }

        private void OnDisable()
        {
            LevelSystem.OnLevelUp -= OnLevelUp;
        }

        private void OnLevelUp(int newLevel)
        {
            ShowMessage($"⬆ Lên cấp {newLevel}!");
        }

        public void ShowMessage(string msg)
        {
            if (_message_Label != null) _message_Label.text = msg;
            gameObject.SetActive(true);
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
        {
            if (_canvasGroup != null) _canvasGroup.alpha = 1f;
            yield return new WaitForSeconds(_displayDuration);
            float elapsed = 0f;
            float fadeDuration = 0.5f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                if (_canvasGroup != null)
                    _canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                yield return null;
            }
            gameObject.SetActive(false);
        }
    }
}
