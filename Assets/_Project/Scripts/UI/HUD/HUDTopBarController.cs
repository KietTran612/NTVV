namespace NTVV.UI.HUD
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using NTVV.Gameplay.Economy;
    using NTVV.Gameplay.Progression;
    using NTVV.Gameplay.Storage;

    /// <summary>
    /// Controller for the Top HUD using uGUI.
    /// Manages data binding for Gold, XP, Level, and Storage.
    /// Follows ui-standardization suffix naming conventions.
    /// </summary>
    public class HUDTopBarController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text _gold_Label;
        [SerializeField] private TMP_Text _level_Label;
        [SerializeField] private TMP_Text _storage_Label;
        [SerializeField] private Image _xp_Fill;

        private void OnEnable()
        {
            EconomySystem.OnGoldChanged += UpdateGold;
            LevelSystem.OnXPChanged += UpdateXP;
            StorageSystem.OnStorageChanged += UpdateStorage;

            RefreshUI();
        }

        private void OnDisable()
        {
            EconomySystem.OnGoldChanged -= UpdateGold;
            LevelSystem.OnXPChanged -= UpdateXP;
            StorageSystem.OnStorageChanged -= UpdateStorage;
        }

        private void RefreshUI()
        {
            if (EconomySystem.Instance != null) UpdateGold(EconomySystem.Instance.CurrentGold);
            if (LevelSystem.Instance != null) UpdateXP(LevelSystem.Instance.CurrentXP, LevelSystem.Instance.CurrentLevel);
            if (StorageSystem.Instance != null) UpdateStorage(StorageSystem.Instance.CurrentSlotsUsed, StorageSystem.Instance.MaxCapacity);
        }

        private void UpdateGold(int amount)
        {
            if (_gold_Label != null) _gold_Label.text = amount.ToString("N0");
        }

        private void UpdateXP(int xp, int level)
        {
            if (_level_Label != null) _level_Label.text = $"Level {level}";
            
            if (LevelSystem.Instance != null && _xp_Fill != null)
            {
                _xp_Fill.fillAmount = LevelSystem.Instance.GetXPProgress();
            }
        }

        private void UpdateStorage(int used, int max)
        {
            if (_storage_Label != null) _storage_Label.text = $"{used}/{max}";
        }
    }
}
