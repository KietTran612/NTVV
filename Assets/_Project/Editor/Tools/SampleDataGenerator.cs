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
            string folder = "Assets/_Project/Data/Configs";
            if (!AssetDatabase.IsValidFolder(folder))
            {
                if (!AssetDatabase.IsValidFolder("Assets/_Project/Data")) AssetDatabase.CreateFolder("Assets/_Project", "Data");
                AssetDatabase.CreateFolder("Assets/_Project/Data", "Configs");
            }

            string path = folder + "/SampleStorageUpgradeConfig.asset";

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
            EditorUtility.DisplayDialog("Thành công!", $"Đã tạo xong bản Config mẫu tại {path}", "OK");
        }

        [MenuItem("NTVV/Tools/Generate Sample Animal Pen Config")]
        public static void GenerateAnimalPenConfig()
        {
            string folder = "Assets/_Project/Data/Configs";
            if (!AssetDatabase.IsValidFolder(folder))
            {
                if (!AssetDatabase.IsValidFolder("Assets/_Project/Data")) AssetDatabase.CreateFolder("Assets/_Project", "Data");
                AssetDatabase.CreateFolder("Assets/_Project/Data", "Configs");
            }

            string path = folder + "/SampleAnimalPenUpgradeConfig.asset";

            AnimalPenUpgradeDataSO asset = ScriptableObject.CreateInstance<AnimalPenUpgradeDataSO>();
            asset.tiers = new List<AnimalPenUpgradeTier>
            {
                new AnimalPenUpgradeTier { tierLevel = 0, upgradeCostGold = 0, maxCapacity = 4, minLevelToAccess = 1 },
                new AnimalPenUpgradeTier { tierLevel = 1, upgradeCostGold = 1000, maxCapacity = 8, minLevelToAccess = 3 },
                new AnimalPenUpgradeTier { tierLevel = 2, upgradeCostGold = 3000, maxCapacity = 16, minLevelToAccess = 8 },
                new AnimalPenUpgradeTier { tierLevel = 3, upgradeCostGold = 8000, maxCapacity = 32, minLevelToAccess = 15 }
            };

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

            Debug.Log($"<color=green>[NTVV]</color> Sample Animal Pen Config generated at: {path}");
            EditorUtility.DisplayDialog("Thành công!", $"Đã tạo xong bản Config mẫu cho Chuồng thú tại {path}", "OK");
        }

        [MenuItem("NTVV/Tools/Generate Sample Quests")]
        public static void GenerateSampleQuests()
        {
            string folderPath = "Assets/_Project/Data/Quests";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                if (!AssetDatabase.IsValidFolder("Assets/_Project/Data"))
                    AssetDatabase.CreateFolder("Assets/_Project", "Data");
                AssetDatabase.CreateFolder("Assets/_Project/Data", "Quests");
            }

            // Quest 1: Thu hoạch Lúa mì
            CreateQuest(folderPath, "q_harvest_wheat", "Người nông dân tập sự", "Hãy thu hoạch lúa mì đầu tiên của bạn để làm quen với nông trại.", 1, 
                new List<NTVV.Data.QuestObjective> {
                    new NTVV.Data.QuestObjective { actionType = NTVV.Data.QuestActionType.HarvestCrop, targetId = "crop_wheat", requiredAmount = 5, description = "Thu hoạch 5 Lúa mì" }
                }, 100, 50);

            // Quest 2: Đạt Level 2
            CreateQuest(folderPath, "q_reach_level_2", "Sự tiến bộ kinh ngạc", "Bạn đang làm rất tốt! Hãy tiếp tục tích lũy kinh nghiệm để lên cấp 2.", 2, 
                new List<NTVV.Data.QuestObjective> {
                    new NTVV.Data.QuestObjective { actionType = NTVV.Data.QuestActionType.ReachLevel, targetId = "Player", requiredAmount = 2, description = "Đạt cấp độ 2" }
                }, 200, 100);

            // Quest 3: Nuôi Gà
            CreateQuest(folderPath, "q_buy_chicken", "Người bạn lông vũ", "Nông trại sẽ vui hơn nếu có tiếng gà kêu. Hãy mua một con gà từ cửa hàng.", 3, 
                new List<NTVV.Data.QuestObjective> {
                    new NTVV.Data.QuestObjective { actionType = NTVV.Data.QuestActionType.BuyItem, targetId = "animal_chicken", requiredAmount = 1, description = "Mua 1 con Gà" }
                }, 500, 150);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            EditorUtility.DisplayDialog("Thành công!", "Đã tạo 3 nhiệm vụ mẫu tại Assets/_Project/Data/Quests.", "OK");
        }

        private static void CreateQuest(string folder, string id, string name, string desc, int level, List<NTVV.Data.QuestObjective> objectives, int gold, int xp)
        {
            string path = $"{folder}/{id}.asset";
            QuestDataSO quest = ScriptableObject.CreateInstance<QuestDataSO>();
            quest.questId = id;
            quest.questName = name;
            quest.questDescription = desc;
            quest.minLevelRequired = level;
            quest.objectives = objectives;
            quest.rewards = new NTVV.Data.QuestReward { goldReward = gold, xpReward = xp };

            AssetDatabase.CreateAsset(quest, path);
        }
    }
}
