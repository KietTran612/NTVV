namespace NTVV.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Root data structure for player state persistence.
    /// </summary>
    [Serializable]
    public class PlayerSaveData
    {
        public int gold;
        public int currentXP;
        public int currentLevel;
        public int storageCapacity;
        public int storageTier;
        public int gems;
        public long lastSaveTimestamp; // Ticks of the last save time for offline calculations
        public List<InventoryItemData> inventory = new List<InventoryItemData>();
        public List<TileSaveData> tiles = new List<TileSaveData>();
        public List<QuestSaveData> activeQuests = new List<QuestSaveData>();
        public List<string> completedQuestIds = new List<string>();
        public List<AnimalSaveData> animals = new List<AnimalSaveData>();
        
        public PlayerSaveData()
        {
            inventory = new List<InventoryItemData>();
            tiles = new List<TileSaveData>();
            activeQuests = new List<QuestSaveData>();
            completedQuestIds = new List<string>();
            animals = new List<AnimalSaveData>();
            gems = 25;
            lastSaveTimestamp = DateTime.Now.Ticks;
        }
    }

    /// <summary>
    /// Simplified item data for inventory storage.
    /// </summary>
    [Serializable]
    public class InventoryItemData
    {
        public string itemId;
        public int quantity;
    }

    /// <summary>
    /// Persistence data for a single crop tile.
    /// </summary>
    [Serializable]
    public class TileSaveData
    {
        public string tileId;       // Unique ID or index of the tile in the grid
        public string cropId;       // ID of the crop currently planted (empty if none)
        public string stateId;      // Current primary state (Empty, Growing, Ripe, etc.)
        public float currentHP;     // Health percentage for yield calculation
        public long plantTimestamp; // Time when the seed was planted
        public bool hasWeeds;
        public bool hasBugs;
        public bool needsWater;
    }

    /// <summary>
    /// Persistence data for an active quest.
    /// </summary>
    [Serializable]
    public class QuestSaveData
    {
        public string questId;
        public List<int> objectiveProgress = new List<int>();
    }

    /// <summary>
    /// Persistence data for a single animal.
    /// </summary>
    [Serializable]
    public class AnimalSaveData
    {
        public string animalId;
        public int    stage;
        public float  hp;
        public float  timeInCurrentStage;
        public float  timeSinceLastProduction;
    }
}
