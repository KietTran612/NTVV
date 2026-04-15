namespace NTVV.Editor
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine.SceneManagement;
    using NTVV.Gameplay.Economy;
    using NTVV.Gameplay.Storage;
    using NTVV.Gameplay.Progression;
    using NTVV.Tests;

    /// <summary>
    /// Công cụ tự động tạo Scene kiểm thử cho hệ thống Foundation.
    /// Truy cập từ Menu: Tools > NTVV > Create Test Core Scene
    /// </summary>
    public static class TestSceneAutoSetup
    {
        [MenuItem("NTVV/Setup/Create Test Core Scene")]
        public static void CreateTestScene()
        {
            // 1. Tạo Scene mới
            Scene testScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            testScene.name = "SCN_TestCore";

            // 2. Tạo Root Object chứa các System
            GameObject root = new GameObject("--- TEST_FOUNDATION ---");
            
            // 3. Add các System Singleton
            root.AddComponent<EconomySystem>();
            root.AddComponent<StorageSystem>();
            LevelSystem levelSystem = root.AddComponent<LevelSystem>();

            // 4. Add Test Harness
            root.AddComponent<FoundationTestHarness>();

            // 5. Tự động tìm kiếm Data nếu có
            string[] guids = AssetDatabase.FindAssets("t:PlayerLevelData");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                var data = AssetDatabase.LoadAssetAtPath<Object>(path);
                
                SerializedObject so = new SerializedObject(levelSystem);
                so.FindProperty("_levelData").objectReferenceValue = data;
                so.ApplyModifiedProperties();
                
                Debug.Log($"[Test Setup] Đã tự động gán dữ liệu Level từ: {path}");
            }

            // 6. Lưu Scene
            string scenePath = "Assets/_Project/Scenes/SCN_TestCore.unity";
            
            // Đảm bảo thư mục tồn tại
            if (!System.IO.Directory.Exists("Assets/_Project/Scenes"))
            {
                System.IO.Directory.CreateDirectory("Assets/_Project/Scenes");
            }

            EditorSceneManager.SaveScene(testScene, scenePath);
            
            Debug.Log($"<color=green>[Test Setup] Đã tạo thành công Scene tại: {scenePath}</color>");
            Debug.Log("Hãy nhấn Play để bắt đầu chạy kịch bản Test!");
        }
    }
}
