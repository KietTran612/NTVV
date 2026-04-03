using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NTVV.Managers;
using NTVV.UI.Common;
using NTVV.UI.Infrastructure;
using NTVV.UI.Styling;
using NTVV.Gameplay.Economy;
using NTVV.Gameplay.Progression;
using NTVV.Gameplay.Storage;
using NTVV.Gameplay.Quests;
using NTVV.World.Camera;
using NTVV.Core;
using NTVV.Data.ScriptableObjects;
using NTVV.Data;
using NTVV.UI.HUD;
using System.IO;

namespace NTVV.Editor.Tools
{
    public static class GameSceneInitializer
    {
        private const string ScenePath = "Assets/_Project/Scenes/SCN_Gameplay.unity";
        private const string RegistryPath = "Assets/_Project/Data/Registry/GameDataRegistry.asset";
        private const string StylePath = "Assets/_Project/Settings/UI/DefaultFarmStyle.asset";
        private const string StorageConfigPath = "Assets/_Project/Data/Configs/StorageUpgradeConfig.asset";
        private const string AnimalPenConfigPath = "Assets/_Project/Data/Configs/AnimalPenUpgradeConfig.asset";
        private const string LevelDataPath = "Assets/_Project/Data/Configs/SO_DefaultLevelData.asset";

        [MenuItem("NTVV/Setup Full Game Scene")]
        public static void CreateFullGameplayScene()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            // Ensure dependencies exist
            if (!ValidateDependencies()) return;

            // 1. Create New Scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            
            // 2. Setup World (Camera & Environment)
            SetupWorldRoot();

            // 3. Setup Systems Root
            GameObject systemsRoot = new GameObject("[SYSTEMS]");
            
            // Core Managers
            var saveMgr = systemsRoot.AddComponent<SaveLoadManager>();
            var gameMgr = systemsRoot.AddComponent<GameManager>();
            var timeMgr = systemsRoot.AddComponent<TimeManager>();

            // Assign Registry to GameManager
            SerializedObject gmSO = new SerializedObject(gameMgr);
            var registry = AssetDatabase.LoadAssetAtPath<GameDataRegistrySO>(RegistryPath);
            if (registry != null)
            {
                gmSO.FindProperty("_dataRegistry").objectReferenceValue = registry;
                gmSO.ApplyModifiedProperties();
            }

            // 4. Setup Gameplay Root
            GameObject gameplayRoot = new GameObject("[GAMEPLAY]");
            gameplayRoot.AddComponent<EconomySystem>();
            
            var levelSys = gameplayRoot.AddComponent<LevelSystem>();
            var storageSys = gameplayRoot.AddComponent<StorageSystem>();
            gameplayRoot.AddComponent<QuestManager>();

            // Note: Components will now self-heal from Registry in play mode, 
            // but we assign them here anyway for editor-time visibility.
            if (registry != null)
            {
                var levelSO = new SerializedObject(levelSys);
                levelSO.FindProperty("_levelData").objectReferenceValue = registry.levelData;
                levelSO.ApplyModifiedProperties();

                var storageSO = new SerializedObject(storageSys);
                storageSO.FindProperty("_upgradeConfig").objectReferenceValue = registry.storageUpgradeConfig;
                storageSO.ApplyModifiedProperties();
            }

            // 5. Setup UI Root
            SetupUISceneRoot();

            // 6. Final Cleanup & Save
            EditorSceneManager.SaveScene(scene, ScenePath);
            Debug.Log($"<color=green>[GameSceneInitializer]</color> Full Gameplay Scene created at: {ScenePath}");
        }

        [MenuItem("NTVV/UI/Restore Core Assets & Repair Registry")]
        public static void RestoreCoreAssets()
        {
            EnsureUIThemeFolder("Default");
            
            // 1. Create Level Data if missing
            PlayerLevelData levelAsset = AssetDatabase.LoadAssetAtPath<PlayerLevelData>(LevelDataPath);
            if (levelAsset == null)
            {
                levelAsset = ScriptableObject.CreateInstance<PlayerLevelData>();
                for (int i = 1; i <= 10; i++)
                {
                    levelAsset.Milestones.Add(new LevelMilestone { Level = i, XPRequired = i * 1000 });
                }
                if (!AssetDatabase.IsValidFolder("Assets/_Project/Data"))
                    AssetDatabase.CreateFolder("Assets/_Project", "Data");
                AssetDatabase.CreateAsset(levelAsset, LevelDataPath);
                Debug.Log($"[GameSceneInitializer] Created default Level Data at: {LevelDataPath}");
            }

            // 2. Create Storage Config if missing
            StorageUpgradeDataSO storageAsset = AssetDatabase.LoadAssetAtPath<StorageUpgradeDataSO>(StorageConfigPath);
            if (storageAsset == null)
            {
                storageAsset = ScriptableObject.CreateInstance<StorageUpgradeDataSO>();
                AssetDatabase.CreateAsset(storageAsset, StorageConfigPath);
                Debug.Log($"[GameSceneInitializer] Created default Storage Config at: {StorageConfigPath}");
            }

            // 3. Create Animal Pen Config if missing
            AnimalPenUpgradeDataSO animalPenAsset = AssetDatabase.LoadAssetAtPath<AnimalPenUpgradeDataSO>(AnimalPenConfigPath);
            if (animalPenAsset == null)
            {
                animalPenAsset = ScriptableObject.CreateInstance<AnimalPenUpgradeDataSO>();
                AssetDatabase.CreateAsset(animalPenAsset, AnimalPenConfigPath);
                Debug.Log($"[GameSceneInitializer] Created default Animal Pen Config at: {AnimalPenConfigPath}");
            }

            // 4. Update GameDataRegistry Links
            GameDataRegistrySO registry = AssetDatabase.LoadAssetAtPath<GameDataRegistrySO>(RegistryPath);
            if (registry != null)
            {
                SerializedObject rSO = new SerializedObject(registry);
                rSO.FindProperty("levelData").objectReferenceValue = levelAsset;
                rSO.FindProperty("storageUpgradeConfig").objectReferenceValue = storageAsset;
                rSO.FindProperty("animalPenUpgradeConfig").objectReferenceValue = animalPenAsset;
                rSO.ApplyModifiedProperties();
                Debug.Log("<color=cyan>[GameSceneInitializer]</color> GameDataRegistry repaired and linked.");
            }

            // 5. Create HUD/Nav Prefabs if missing
            RestoreUIPrefabs();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void RestoreUIPrefabs()
        {
            string hudPath = "Assets/_Project/Resources/UI/Default/HUDTopBar.prefab";
            if (!AssetDatabase.LoadAssetAtPath<GameObject>(hudPath))
            {
                GameObject hud = new GameObject("HUDTopBar", typeof(RectTransform));
                var rt = hud.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(0, 100);
                rt.anchorMin = new Vector2(0, 1);
                rt.anchorMax = new Vector2(1, 1);
                rt.pivot = new Vector2(0.5f, 1);
                rt.anchoredPosition = Vector2.zero;
                hud.AddComponent<HUDTopBarController>();
                hud.AddComponent<UIStyleApplier>();
                PrefabUtility.SaveAsPrefabAsset(hud, hudPath);
                Object.DestroyImmediate(hud);
            }

            string navPath = "Assets/_Project/Resources/UI/Default/BottomNav.prefab";
            if (!AssetDatabase.LoadAssetAtPath<GameObject>(navPath))
            {
                GameObject nav = new GameObject("BottomNav", typeof(RectTransform));
                var rt = nav.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(0, 120);
                rt.anchorMin = new Vector2(0, 0);
                rt.anchorMax = new Vector2(1, 0);
                rt.pivot = new Vector2(0.5f, 0);
                rt.anchoredPosition = Vector2.zero;
                nav.AddComponent<BottomNavController>();
                nav.AddComponent<UIStyleApplier>();
                PrefabUtility.SaveAsPrefabAsset(nav, navPath);
                Object.DestroyImmediate(nav);
            }
        }

        private static bool ValidateDependencies()
        {
            GameDataRegistrySO registry = AssetDatabase.LoadAssetAtPath<GameDataRegistrySO>(RegistryPath);
            bool registryOK = registry != null && registry.levelData != null && registry.storageUpgradeConfig != null;
            bool uiOK = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Project/Resources/UI/Default/HUDTopBar.prefab") != null;

            if (!registryOK || !uiOK)
            {
                if (EditorUtility.DisplayDialog("Incomplete Assets", 
                    "Core configurations or UI prefabs are incomplete/unlinked in the Registry. Repair them now?", "Yes", "No"))
                {
                    RestoreCoreAssets();
                    return true;
                }
                return false;
            }
            return true;
        }

        private static void SetupWorldRoot()
        {
            GameObject worldRoot = new GameObject("[WORLD]");
            GameObject camGO = new GameObject("Main Camera");
            camGO.transform.SetParent(worldRoot.transform);
            camGO.transform.position = new Vector3(0, 5, -10);
            camGO.transform.rotation = Quaternion.Euler(30, 0, 0);
            var cam = camGO.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 5f;
            camGO.AddComponent<AudioListener>();
            camGO.AddComponent<FarmCameraController>();
            camGO.tag = "MainCamera";

            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.SetParent(worldRoot.transform);
            ground.transform.localScale = new Vector3(5, 1, 5);
        }

        private static void SetupUISceneRoot()
        {
            if (Object.FindFirstObjectByType<EventSystem>() == null)
            {
                 new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }

            GameObject canvasGO = new GameObject("[UI_CANVAS]");
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            var scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            canvasGO.AddComponent<GraphicRaycaster>();

            var popupMgr = canvasGO.AddComponent<PopupManager>();
            SerializedObject pSO = new SerializedObject(popupMgr);
            var style = AssetDatabase.LoadAssetAtPath<UIStyleDataSO>(StylePath);
            if (style != null) pSO.FindProperty("_activeStyle").objectReferenceValue = style;
            pSO.FindProperty("_mainOverlayCanvas").objectReferenceValue = canvas;

            GameObject hrt = new GameObject("HUDLayer", typeof(RectTransform));
            hrt.transform.SetParent(canvasGO.transform, false);
            RectTransform hudLayer = hrt.GetComponent<RectTransform>();
            Stretch(hudLayer);
            
            GameObject mrt = new GameObject("ModalLayer", typeof(RectTransform));
            mrt.transform.SetParent(canvasGO.transform, false);
            RectTransform modalLayer = mrt.GetComponent<RectTransform>();
            Stretch(modalLayer);

            pSO.FindProperty("_modalParent").objectReferenceValue = modalLayer;
            pSO.FindProperty("_hudParent").objectReferenceValue = hudLayer;
            pSO.ApplyModifiedProperties();

            InstantiateThemedUI("HUDTopBar", hudLayer, style);
            InstantiateThemedUI("BottomNav", hudLayer, style);
        }

        private static void InstantiateThemedUI(string name, Transform parent, UIStyleDataSO style)
        {
            string path = $"UI/Default/{name}";
            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab == null) return;

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
            instance.name = name;
            
            if (style != null)
            {
                var appliers = instance.GetComponentsInChildren<UIStyleApplier>(true);
                foreach (var applier in appliers) applier.ApplyStyle(style);
            }
        }

        private static void EnsureUIThemeFolder(string folderName)
        {
            string parent = "Assets/_Project/Resources/UI";
            if (!AssetDatabase.IsValidFolder(parent))
            {
                if (!AssetDatabase.IsValidFolder("Assets/_Project/Resources"))
                    AssetDatabase.CreateFolder("Assets/_Project", "Resources");
                AssetDatabase.CreateFolder("Assets/_Project/Resources", "UI");
            }
            if (!AssetDatabase.IsValidFolder($"{parent}/{folderName}")) AssetDatabase.CreateFolder(parent, folderName);
        }

        private static void Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
    }
}
