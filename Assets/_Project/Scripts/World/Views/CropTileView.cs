namespace NTVV.World.Views
{
    using UnityEngine;
    using NTVV.Data;
    using NTVV.Gameplay.Economy;
    using NTVV.Gameplay.Storage;
    using NTVV.Gameplay.Progression;
    using NTVV.Core;
    using System;

    /// <summary>
    /// Hiển thị và xử lý một ô đất/cây trồng trong thế giới.
    /// Đã được chuẩn hóa 100% camelCase.
    /// </summary>
    public class CropTileView : MonoBehaviour
    {
        public enum TileState { Empty, Growing, NeedsCare, Ripe, Dead }
        public enum GrowthStage { Phase1, Phase2, Phase3, Ripe }

        [Header("Config")]
        [SerializeField] private string _tileId; 
        [SerializeField] private CropData _currentCropData;
        [SerializeField] private TileState _currentState = TileState.Empty;
        [SerializeField] private GrowthStage _currentStage = GrowthStage.Phase1;

        [Header("State")]
        [SerializeField] private float _growthProgress; // 0-1
        [SerializeField] private float _elapsedTime;
        [SerializeField] private float _timeSinceRipe;
        [SerializeField] private float _currentHP = 100f;

        [Header("Ailments")]
        [SerializeField] private bool _hasWeeds;
        [SerializeField] private bool _hasPests;
        [SerializeField] private bool _needsWater;

        public string TileId => _tileId;
        public float ElapsedTime => _elapsedTime;
        public CropData CurrentCropData => _currentCropData;
        public TileState CurrentState => _currentState;
        public float CurrentHP => _currentHP;
        public bool HasWeeds => _hasWeeds;
        public bool HasPests => _hasPests;
        public bool NeedsWater => _needsWater;

        private void OnEnable() { TimeManager.OnTick += HandleTick; }
        private void OnDisable() { TimeManager.OnTick -= HandleTick; }

        public void Plant(CropData data)
        {
            if (_currentState != TileState.Empty && _currentState != TileState.Dead) return;

            if (EconomySystem.Instance != null && EconomySystem.Instance.CanAfford(data.seedCostGold))
            {
                EconomySystem.Instance.AddGold(-data.seedCostGold);
                _currentCropData = data;
                _currentState = TileState.Growing;
                _growthProgress = 0f;
                _elapsedTime = 0f;
                _timeSinceRipe = 0f;
                _currentHP = 100f;
                Debug.Log($"<color=green>[Plot]</color> Planted: {data.cropName}");
            }
        }

        public void RestoreState(TileSaveData data, CropData crop)
        {
            _tileId = data.tileId;
            _currentCropData = crop;
            
            if (Enum.TryParse(data.stateId, out TileState savedState))
                _currentState = savedState;
                
            _currentHP = data.currentHP;
            _hasWeeds = data.hasWeeds;
            _hasPests = data.hasBugs;
            _needsWater = data.needsWater;

            if (_currentCropData != null && _currentState != TileState.Empty)
            {
                long ticksSincePlanted = DateTime.Now.Ticks - data.plantTimestamp;
                _elapsedTime = (float)TimeSpan.FromTicks(ticksSincePlanted).TotalSeconds;
                _growthProgress = Mathf.Clamp01(_elapsedTime / _currentCropData.GrowthTimeInSeconds);
                
                if (_elapsedTime > _currentCropData.GrowthTimeInSeconds)
                {
                    _timeSinceRipe = _elapsedTime - _currentCropData.GrowthTimeInSeconds;
                    _currentState = TileState.Ripe;
                    
                    if (_timeSinceRipe > _currentCropData.LifeAfterRipeInSeconds)
                    {
                        _currentState = TileState.Dead;
                    }
                }
                UpdateStage();
            }
        }

        public void Harvest()
        {
            if (_currentState != TileState.Ripe) return;

            float hpFactor = GetHealthFactor();
            float timeFactor = GetTimeFactor();
            int finalYield = Mathf.CeilToInt(_currentCropData.baseYieldUnits * hpFactor * timeFactor);

            if (StorageSystem.Instance != null && StorageSystem.Instance.CanAddItem(_currentCropData.cropId, finalYield))
            {
                StorageSystem.Instance.AddItem(_currentCropData.cropId, finalYield);
                if (LevelSystem.Instance != null) LevelSystem.Instance.AddXP(_currentCropData.xpReward);
                
                Debug.Log($"<color=cyan>[Harvest]</color> Success! Received {finalYield} {_currentCropData.cropName}.");
                ResetTile();
            }
        }

        public void ClearDead()
        {
            if (_currentState != TileState.Dead) return;
            ResetTile();
        }

        #region Care Actions
        public void ClearWeeds() { _hasWeeds = false; StorageSystem.Instance.AddItem("item_grass", 1); }
        public void ClearPests() { _hasPests = false; StorageSystem.Instance.AddItem("item_worm", 1); }
        public void WaterPlant() { _needsWater = false; }
        #endregion

        #region Factors
        private float GetHealthFactor()
        {
            if (_currentHP >= 90) return 1.00f;
            if (_currentHP >= 70) return 0.90f;
            if (_currentHP >= 50) return 0.78f;
            if (_currentHP >= 30) return 0.65f;
            if (_currentHP > 0) return 0.50f;
            return 0f;
        }

        private float GetTimeFactor()
        {
            if (_timeSinceRipe <= _currentCropData.PerfectWindowInSeconds) return 1.10f;
            float postRipeMax = _currentCropData.LifeAfterRipeInSeconds;
            if (_timeSinceRipe <= postRipeMax * 0.5f) return 0.92f;
            if (_timeSinceRipe <= postRipeMax) return 0.80f;
            return 0f;
        }
        #endregion

        private void ResetTile()
        {
            _currentCropData = null;
            _currentState = TileState.Empty;
            _growthProgress = 0f;
            _elapsedTime = 0f;
            _timeSinceRipe = 0f;
            _currentHP = 100f;
            _hasWeeds = _hasPests = _needsWater = false;
        }

        private void UpdateStage()
        {
            if (_currentCropData == null) return;
            if (_growthProgress < _currentCropData.phase1Pct) _currentStage = GrowthStage.Phase1;
            else if (_growthProgress < (_currentCropData.phase1Pct + _currentCropData.phase2Pct)) _currentStage = GrowthStage.Phase2;
            else _currentStage = GrowthStage.Phase3;
        }

        private void HandleTick(float tickDelta)
        {
            // Logic tick đã được tối giản để tập trung vào naming
            if (_currentState == TileState.Empty || _currentState == TileState.Dead) return;
            if (_currentCropData == null) return;

            if (_currentState == TileState.Ripe)
            {
                _timeSinceRipe += tickDelta;
                if (_timeSinceRipe > _currentCropData.LifeAfterRipeInSeconds) _currentState = TileState.Dead;
                return;
            }

            _elapsedTime += tickDelta;
            _growthProgress = Mathf.Clamp01(_elapsedTime / _currentCropData.GrowthTimeInSeconds);
            UpdateStage();
            
            // HP Drain logic...
            float drainRate = (_hasWeeds ? _currentCropData.WeedsHPDrain : 0) + (_hasPests ? _currentCropData.PestsHPDrain : 0) + (_needsWater ? _currentCropData.WaterHPDrain : 0);
            if (drainRate > 0)
            {
                _currentHP = Mathf.Max(0, _currentHP - (drainRate * tickDelta / 10f));
                if (_currentHP <= 0) _currentState = TileState.Dead;
                else _currentState = TileState.NeedsCare;
            }
            else if (_growthProgress >= 1f)
            {
                _currentState = TileState.Ripe;
                _currentStage = GrowthStage.Ripe;
            }
        }
    }
}
