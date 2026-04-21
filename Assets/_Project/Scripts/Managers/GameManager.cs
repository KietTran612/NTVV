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
        public GameDataRegistrySO DataRegistry => _dataRegistry;
        public static System.DateTime LastSaveTime { get; private set; } = System.DateTime.UtcNow;

        protected override void OnInitialize()
        {
            _isPersistent = true;
            if (_dataRegistry != null) _dataRegistry.Initialize();
            LevelSystem.OnLevelUp += OnPlayerLevelUp;
            StartCoroutine(BootSequence());
        }

        private IEnumerator BootSequence()
        {
            _currentState = GameState.Loading;
            Debug.Log("<color=cyan>[GameManager]</color> Boot sequence started...");

            PlayerSaveData saveData = SaveLoadManager.Instance.Load();

            // Fix FEAT-05: set LastSaveTime trước RestoreWorldState để AnimalView.RestoreState() dùng đúng
            if (saveData != null && saveData.lastSaveTimestamp != 0)
                LastSaveTime = new System.DateTime(saveData.lastSaveTimestamp);

            InitializeCoreSystems(saveData);
            RestoreWorldState(saveData);

            // Welcome toast nếu offline > 60s
            double offlineSeconds = (System.DateTime.UtcNow - LastSaveTime).TotalSeconds;
            if (offlineSeconds > 60)
            {
                var toast = FindFirstObjectByType<NTVV.UI.HUD.LevelUpToastController>();
                if (toast != null)
                {
                    int hours = (int)(offlineSeconds / 3600);
                    int minutes = (int)((offlineSeconds % 3600) / 60);
                    string timeStr = hours > 0 ? $"{hours}g {minutes}ph" : $"{minutes} phút";
                    toast.ShowMessage($"Chào mừng trở lại! Farm đã tiến triển trong {timeStr}");
                }
            }

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
            EconomySystem.Instance.SetGems(data.gems);
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
            if (data == null || _dataRegistry == null) return;

            // Restore crop tiles
            if (data.tiles != null)
            {
                CropTileView[] allTileViews = FindObjectsByType<CropTileView>(FindObjectsSortMode.None);
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

            // Restore animals
            if (data.animals != null && data.animals.Count > 0)
            {
                AnimalPenView[] allPens = FindObjectsByType<AnimalPenView>(FindObjectsSortMode.None);
                foreach (var saved in data.animals)
                {
                    var animalSO = _dataRegistry.animals?.FirstOrDefault(a => a != null && a.data != null && a.data.animalId == saved.animalId);
                    if (animalSO == null) continue; // stale save — skip
                    var pen = allPens.FirstOrDefault(p => !p.IsFull);
                    pen?.SpawnAndRestore(animalSO.data, saved);
                }
            }

            // FEAT-06: Restore tile lock state
            if (data.unlockedTileIds != null && data.unlockedTileIds.Count > 0)
            {
                CropTileView[] allTileViews = FindObjectsByType<CropTileView>(FindObjectsSortMode.None);
                foreach (var tile in allTileViews)
                {
                    if (!tile.IsLocked) continue; // already unlocked by default, skip
                    string id = string.IsNullOrEmpty(tile.TileId) ? tile.gameObject.name : tile.TileId;
                    if (data.unlockedTileIds.Contains(id))
                        tile.Unlock();
                }
            }
        }

        public void TriggerSave()
        {
            if (_currentState == GameState.Loading) return;
            LastSaveTime = System.DateTime.UtcNow;
            SaveLoadManager.Instance.Save(CaptureCurrentState());
        }

        public PlayerSaveData CaptureCurrentState()
        {
            PlayerSaveData data = new PlayerSaveData
            {
                gold = EconomySystem.Instance.CurrentGold,
                gems = EconomySystem.Instance.CurrentGems,
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

            CropTileView[] allTiles = FindObjectsByType<CropTileView>(FindObjectsSortMode.None);
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

            // FEAT-06: Save unlocked tile IDs
            data.unlockedTileIds = new System.Collections.Generic.List<string>();
            foreach (var tile in allTiles)
            {
                if (!tile.IsLocked)
                {
                    string id = string.IsNullOrEmpty(tile.TileId) ? tile.gameObject.name : tile.TileId;
                    data.unlockedTileIds.Add(id);
                }
            }

            // [Quest] Capture quests
            if (QuestManager.Instance != null)
                QuestManager.Instance.SaveData(data);

            // Collect animal save data
            data.animals = new System.Collections.Generic.List<AnimalSaveData>(
                FindObjectsByType<AnimalView>(FindObjectsSortMode.None)
                    .Select(a => a.GetSaveData())
            );

            return data;
        }

        private void OnPlayerLevelUp(int newLevel)
        {
            CropTileView[] tiles = FindObjectsByType<CropTileView>(FindObjectsSortMode.None);
            bool anyUnlocked = false;
            foreach (var tile in tiles)
            {
                if (tile.IsLocked && newLevel >= tile.RequiredLevel)
                {
                    tile.Unlock();
                    anyUnlocked = true;
                }
            }
            if (anyUnlocked) TriggerSave();
        }

        private void OnDestroy()
        {
            LevelSystem.OnLevelUp -= OnPlayerLevelUp;
        }
    }
}
