namespace NTVV.Testing
{
    using UnityEngine;
    using NTVV.Managers;
    using NTVV.Gameplay.Economy;
    using NTVV.Gameplay.Storage;
    using NTVV.Gameplay.Progression;
    using NTVV.Core;
    using System.Collections.Generic;

    /// <summary>
    /// Utility to manually test Save/Load logic and persistence from the Inspector.
    /// Updated to match standard camelCase naming.
    /// </summary>
    public class PersistenceTestHarness : MonoBehaviour
    {
        [Header("Test Values to Add")]
        public int goldToAdd = 500;
        public int xpToAdd = 100;
        public string testItemId = "item_001";
        public int itemQuantity = 5;

        [Header("Debug Status")]
        public string activeSavePath;
        public PlayerSaveData lastSaveSnapshot;
        public PlayerSaveData lastLoadSnapshot;

        private void Start()
        {
            if (SaveLoadManager.Instance != null) activeSavePath = SaveLoadManager.Instance.SavePath;
        }

        [ContextMenu("1. Add Test Data (Gold, XP, Items)")]
        public void AddTestData()
        {
            if (EconomySystem.Instance != null) EconomySystem.Instance.AddGold(goldToAdd);
            if (LevelSystem.Instance != null) LevelSystem.Instance.AddXP(xpToAdd);
            if (StorageSystem.Instance != null) StorageSystem.Instance.AddItem(testItemId, itemQuantity);
            
            Debug.Log("<color=yellow>[PersistenceTest]</color> Test data added to runtime memory.");
        }

        [ContextMenu("2. Trigger MANUAL SAVE")]
        public void ManualSave()
        {
            if (NTVV.Managers.GameManager.Instance != null)
            {
                lastSaveSnapshot = CaptureCurrentData();
                NTVV.Managers.GameManager.Instance.TriggerSave();
                Debug.Log("<color=green>[PersistenceTest]</color> Save triggered successfully.");
            }
        }

        [ContextMenu("3. Clear Runtime Data (Reset)")]
        public void ClearData()
        {
            if (EconomySystem.Instance != null) EconomySystem.Instance.SetGold(0);
            if (LevelSystem.Instance != null) LevelSystem.Instance.LoadData(1, 0);
            if (StorageSystem.Instance != null) StorageSystem.Instance.LoadData(new Dictionary<string, int>(), 50);
            
            Debug.Log("<color=orange>[PersistenceTest]</color> Runtime systems cleared. Ready to test LOAD.");
        }

        [ContextMenu("4. Trigger MANUAL LOAD")]
        public void ManualLoad()
        {
            PlayerSaveData saveData = SaveLoadManager.Instance.Load();
            if (saveData != null)
            {
                lastLoadSnapshot = saveData;

                if (EconomySystem.Instance != null) EconomySystem.Instance.SetGold(saveData.gold);
                if (LevelSystem.Instance != null) LevelSystem.Instance.LoadData(saveData.currentLevel, saveData.currentXP);
                
                Dictionary<string, int> inventoryDict = new Dictionary<string, int>();
                foreach (var item in saveData.inventory)
                {
                    inventoryDict[item.itemId] = item.quantity;
                }
                if (StorageSystem.Instance != null) 
                    StorageSystem.Instance.LoadData(inventoryDict, saveData.storageCapacity);

                Debug.Log("<color=cyan>[PersistenceTest]</color> Data loaded and restored successfully.");
            }
        }

        private PlayerSaveData CaptureCurrentData()
        {
            PlayerSaveData data = new PlayerSaveData();
            if (EconomySystem.Instance != null) data.gold = EconomySystem.Instance.CurrentGold;
            if (LevelSystem.Instance != null)
            {
                data.currentLevel = LevelSystem.Instance.CurrentLevel;
                data.currentXP = LevelSystem.Instance.CurrentXP;
            }
            if (StorageSystem.Instance != null)
            {
                data.storageCapacity = StorageSystem.Instance.MaxCapacity;
                foreach (var kvp in StorageSystem.Instance.GetAllItems())
                {
                    data.inventory.Add(new InventoryItemData { itemId = kvp.Key, quantity = kvp.Value });
                }
            }
            return data;
        }
    }
}
