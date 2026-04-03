namespace NTVV.Managers
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using NTVV.Core;
    using NTVV.Gameplay.Economy;
    using NTVV.Gameplay.Storage;
    using NTVV.Gameplay.Progression;
    using NTVV.Gameplay.Quests;
    using NTVV.World.Views;
    using NTVV.Data;
    using NTVV.Data.ScriptableObjects;
    using System;
    using System.Linq;

    /// <summary>
    /// Điều phối chính trạng thái game và vòng đời.
    /// Đã được chuẩn hóa 100% camelCase.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        public enum GameState { Loading, Playing, Paused }
        
        [Header("Runtime State")]
        [SerializeField] private GameState _currentState = GameState.Loading;
        
        [Header("Data Registry")]
        [SerializeField] private GameDataRegistrySO _dataRegistry;

        public GameState CurrentState => _currentState;

        protected override void OnInitialize()
        {
            _isPersistent = true;
            if (_dataRegistry != null) _dataRegistry.Initialize();
            StartCoroutine(BootSequence());
        }

        private IEnumerator BootSequence()
        {
            _currentState = GameState.Loading;
            Debug.Log("<color=cyan>[GameManager]</color> Boot sequence started...");

            PlayerSaveData saveData = SaveLoadManager.Instance.Load();
            InitializeCoreSystems(saveData);
            RestoreWorldState(saveData);

            _currentState = GameState.Playing;
            Debug.Log("<color=cyan>[GameManager]</color> Boot sequence complete.");
            yield return null;
        }

        private void InitializeCoreSystems(PlayerSaveData data)
        {
            if (data == null)
            {
                EconomySystem.Instance.RefreshUI();
                StorageSystem.Instance.RefreshUI();
                LevelSystem.Instance.RefreshUI();
                
                // [Quest] Khởi tạo QuestManager nếu không có save
                if (QuestManager.Instance != null) 
                    QuestManager.Instance.LoadData(null, null, _dataRegistry);
                    
                return;
            }

            EconomySystem.Instance.SetGold(data.gold);
            LevelSystem.Instance.LoadData(data.currentLevel, data.currentXP);
            
            Dictionary<string, int> inventoryDict = new Dictionary<string, int>();
            foreach (var item in data.inventory)
            {
                if (item == null || string.IsNullOrEmpty(item.itemId)) continue;
                inventoryDict[item.itemId] = item.quantity;
            }
            StorageSystem.Instance.LoadData(inventoryDict, data.storageCapacity > 0 ? data.storageCapacity : 50, data.storageTier);

            // [Quest] Restore state
            if (QuestManager.Instance != null)
                QuestManager.Instance.LoadData(data.activeQuests, data.completedQuestIds, _dataRegistry);
        }

        private void RestoreWorldState(PlayerSaveData data)
        {
            if (data == null || data.tiles == null || _dataRegistry == null) return;

            CropTileView[] allTileViews = FindObjectsOfType<CropTileView>();
            foreach (var tileData in data.tiles)
            {
                CropTileView match = allTileViews.FirstOrDefault(t => t.TileId == tileData.tileId || t.gameObject.name == tileData.tileId);
                if (match != null)
                {
                    CropDataSO cropSO = _dataRegistry.GetCrop(tileData.cropId);
                    match.RestoreState(tileData, cropSO != null ? cropSO.data : null);
                }
            }
        }

        public void TriggerSave()
        {
            if (_currentState == GameState.Loading) return;
            SaveLoadManager.Instance.Save(CaptureCurrentState());
        }

        public PlayerSaveData CaptureCurrentState()
        {
            PlayerSaveData data = new PlayerSaveData
            {
                gold = EconomySystem.Instance.CurrentGold,
                currentXP = LevelSystem.Instance.CurrentXP,
                currentLevel = LevelSystem.Instance.CurrentLevel,
                storageCapacity = StorageSystem.Instance.MaxCapacity,
                storageTier = StorageSystem.Instance.CurrentTier,
                lastSaveTimestamp = DateTime.Now.Ticks
            };

            foreach (var item in StorageSystem.Instance.GetAllItems())
            {
                data.inventory.Add(new InventoryItemData { itemId = item.Key, quantity = item.Value });
            }

            CropTileView[] allTiles = FindObjectsOfType<CropTileView>();
            foreach (var tile in allTiles)
            {
                TileSaveData tData = new TileSaveData
                {
                    tileId = string.IsNullOrEmpty(tile.TileId) ? tile.gameObject.name : tile.TileId,
                    cropId = tile.CurrentCropData != null ? tile.CurrentCropData.cropId : "",
                    stateId = tile.CurrentState.ToString(),
                    currentHP = tile.CurrentHP,
                    hasWeeds = tile.HasWeeds,
                    hasBugs = tile.HasPests,
                    needsWater = tile.NeedsWater
                };
                
                long elapsedTicks = TimeSpan.FromSeconds(tile.ElapsedTime).Ticks;
                tData.plantTimestamp = DateTime.Now.Ticks - elapsedTicks;
                
            data.tiles.Add(tData);
            }

            // [Quest] Capture quests
            if (QuestManager.Instance != null)
                QuestManager.Instance.SaveData(data);

            return data;
        }
    }
}
