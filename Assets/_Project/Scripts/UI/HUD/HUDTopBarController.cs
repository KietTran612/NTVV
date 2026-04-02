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
    /// </summary>
    public class HUDTopBarController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text _goldLabel;
        [SerializeField] private TMP_Text _levelLabel;
        [SerializeField] private TMP_Text _storageLabel;
        [SerializeField] private Image _xpBarFill;

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
            if (_goldLabel != null) _goldLabel.text = amount.ToString("N0");
        }

        private void UpdateXP(int xp, int level)
        {
            if (_levelLabel != null) _levelLabel.text = $"Level {level}";
            
            // Visual progress logic - refined in LevelSystem, here just UI mapping
            float progress = (xp % 100) / 100f; // Placeholder mapping
            if (_xpBarFill != null) _xpBarFill.fillAmount = progress;
        }

        private void UpdateStorage(int used, int max)
        {
            if (_storageLabel != null) _storageLabel.text = $"{used}/{max}";
        }
    }
}
