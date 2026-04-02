using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using NTVV.Data.ScriptableObjects;

namespace NTVV.Editor.Tools
{
    public class SampleDataGenerator
    {
        [MenuItem("NTVV/Tools/Generate Sample Storage Config")]
        public static void GenerateStorageConfig()
        {
            string path = "Assets/_Project/Data/SampleStorageUpgradeConfig.asset";
            
            // Ensure directory exists
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Data"))
            {
                AssetDatabase.CreateFolder("Assets/_Project", "Data");
            }

            StorageUpgradeDataSO asset = ScriptableObject.CreateInstance<StorageUpgradeDataSO>();
            asset.tiers = new List<StorageUpgradeTier>
            {
                new StorageUpgradeTier { tierLevel = 1, upgradeCostGold = 500, bonusCapacity = 20, minLevelToAccess = 1 },
                new StorageUpgradeTier { tierLevel = 2, upgradeCostGold = 1200, bonusCapacity = 30, minLevelToAccess = 3 },
                new StorageUpgradeTier { tierLevel = 3, upgradeCostGold = 2500, bonusCapacity = 50, minLevelToAccess = 5 },
                new StorageUpgradeTier { tierLevel = 4, upgradeCostGold = 6000, bonusCapacity = 100, minLevelToAccess = 10 },
                new StorageUpgradeTier { tierLevel = 5, upgradeCostGold = 15000, bonusCapacity = 200, minLevelToAccess = 15 }
            };

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

            Debug.Log($"<color=green>[NTVV]</color> Sample Storage Config generated at: {path}");
            EditorUtility.DisplayDialog("Thành công!", "Đã tạo xong bản Config mẫu tại Assets/_Project/Data.", "OK");
        }
    }
}
