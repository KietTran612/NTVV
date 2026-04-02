using UnityEngine;
using UnityEditor;
using System.IO;
using NTVV.Data;
using NTVV.Data.ScriptableObjects;
using System.Collections.Generic;

namespace NTVV.Editor.Tools
{
    /// <summary>
    /// Utility tool to sync JSON source data into Unity ScriptableObject assets.
    /// Renamed to DataImportUtility to reflect its role as a static process.
    /// </summary>
    public static class DataImportUtility
    {
        private const string JSON_PATH = "Assets/_Project/Settings/DataSources/JSON/";
        private const string CROPS_OUTPUT_PATH = "Assets/_Project/Data/StaticData/Crops/";
        private const string ANIMALS_OUTPUT_PATH = "Assets/_Project/Data/StaticData/Animals/";
        private const string REGISTRY_PATH = "Assets/_Project/Data/StaticData/GameDataRegistry.asset";

        [MenuItem("NTVV/Tools/Import Static Data")]
        public static void ImportAll()
        {
            GameDataRegistrySO registry = GetOrCreateRegistry();
            
            int cropsCount = ImportCrops(registry);
            int animalsCount = ImportAnimals(registry);
            
            EditorUtility.SetDirty(registry);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log($"<color=green>[DataImporter]</color> Sync Complete! Imported {cropsCount} Crops and {animalsCount} Animals.");
        }

        public static GameDataRegistrySO GetOrCreateRegistry()
        {
            GameDataRegistrySO registry = AssetDatabase.LoadAssetAtPath<GameDataRegistrySO>(REGISTRY_PATH);
            if (registry == null)
            {
                // Ensure directory exists
                string dir = Path.GetDirectoryName(REGISTRY_PATH);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                registry = ScriptableObject.CreateInstance<GameDataRegistrySO>();
                AssetDatabase.CreateAsset(registry, REGISTRY_PATH);
            }
            return registry;
        }

        private static int ImportCrops(GameDataRegistrySO registry)
        {
            string jsonPath = Path.Combine(JSON_PATH, "Crops.json");
            if (!File.Exists(jsonPath)) return 0;

            string json = File.ReadAllText(jsonPath);
            CropDataList wrapper = JsonUtility.FromJson<CropDataList>(json);

            if (wrapper == null || wrapper.items == null) return 0;

            // Ensure directory exists
            if (!Directory.Exists(CROPS_OUTPUT_PATH)) Directory.CreateDirectory(CROPS_OUTPUT_PATH);

            registry.crops.Clear();
            int count = 0;

            foreach (var item in wrapper.items)
            {
                string assetPath = $"{CROPS_OUTPUT_PATH}{item.cropId}.asset";
                CropDataSO so = AssetDatabase.LoadAssetAtPath<CropDataSO>(assetPath);

                if (so == null)
                {
                    so = ScriptableObject.CreateInstance<CropDataSO>();
                    AssetDatabase.CreateAsset(so, assetPath);
                }

                so.itemId = item.cropId;
                so.data = item;
                so.itemName = item.cropName; // Sync common name
                EditorUtility.SetDirty(so);
                
                if (!registry.crops.Contains(so)) registry.crops.Add(so);
                count++;
            }
            return count;
        }

        private static int ImportAnimals(GameDataRegistrySO registry)
        {
            string jsonPath = Path.Combine(JSON_PATH, "Animals.json");
            if (!File.Exists(jsonPath)) return 0;

            string json = File.ReadAllText(jsonPath);
            AnimalDataList wrapper = JsonUtility.FromJson<AnimalDataList>(json);

            if (wrapper == null || wrapper.items == null) return 0;

            // Ensure directory exists
            if (!Directory.Exists(ANIMALS_OUTPUT_PATH)) Directory.CreateDirectory(ANIMALS_OUTPUT_PATH);

            registry.animals.Clear();
            int count = 0;

            foreach (var item in wrapper.items)
            {
                string assetPath = $"{ANIMALS_OUTPUT_PATH}{item.animalId}.asset";
                AnimalDataSO so = AssetDatabase.LoadAssetAtPath<AnimalDataSO>(assetPath);

                if (so == null)
                {
                    so = ScriptableObject.CreateInstance<AnimalDataSO>();
                    AssetDatabase.CreateAsset(so, assetPath);
                }

                so.itemId = item.animalId;
                so.data = item;
                so.itemName = item.animalName; // Sync common name
                EditorUtility.SetDirty(so);

                if (!registry.animals.Contains(so)) registry.animals.Add(so);
                count++;
            }
            return count;
        }

        [System.Serializable]
        private class CropDataList { public List<CropData> items; }
        
        [System.Serializable]
        private class AnimalDataList { public List<AnimalData> items; }
    }
}
