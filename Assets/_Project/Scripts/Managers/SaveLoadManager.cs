namespace NTVV.Managers
{
    using UnityEngine;
    using System.IO;
    using NTVV.Core;
    using System;

    /// <summary>
    /// Manager responsible for local JSON persistence.
    /// Inherits from Singleton.
    /// </summary>
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        public string SavePath 
        {
            get 
            {
                return Path.Combine(Application.persistentDataPath, "ntvv_save.json");
            }
        }

        protected override void OnInitialize()
        {
            _isPersistent = true;
            Debug.Log($"<color=white>[SaveLoadManager]</color> System Initialized. SavePath: {SavePath}");
        }

        /// <summary>
        /// Serializes and writes player data to a local file.
        /// </summary>
        public void Save(PlayerSaveData data)
        {
            if (data == null) return;

            try
            {
                data.lastSaveTimestamp = DateTime.Now.Ticks;
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(SavePath, json);
                Debug.Log($"<color=cyan>[SaveLoadManager]</color> Progress saved to: {SavePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveLoadManager] Failed to SAVE data: {e.Message}");
            }
        }

        /// <summary>
        /// Reads and deserializes player data from a local file.
        /// Returns null if no file is found or on error.
        /// </summary>
        public PlayerSaveData Load()
        {
            if (!File.Exists(SavePath))
            {
                Debug.Log("[SaveLoadManager] No save file found. Initializing new game state.");
                return null;
            }

            try
            {
                string json = File.ReadAllText(SavePath);
                PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);
                Debug.Log("<color=cyan>[SaveLoadManager]</color> Data loaded successfully.");
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveLoadManager] Failed to LOAD data: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Deletes the local save file (use with caution).
        /// </summary>
        public void DeleteSave()
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
                Debug.Log("[SaveLoadManager] Local save file deleted.");
            }
        }
    }
}
