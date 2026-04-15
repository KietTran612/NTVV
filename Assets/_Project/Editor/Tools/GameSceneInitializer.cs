using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NTVV.Managers;
using NTVV.UI.Common;
using NTVV.Gameplay.Economy;
using NTVV.Gameplay.Progression;
using NTVV.Gameplay.Storage;
using NTVV.Gameplay.Quests;
using NTVV.World.Camera;
using NTVV.Core;
using NTVV.Data.ScriptableObjects;
using NTVV.Data;
using NTVV.UI.HUD;

namespace NTVV.Editor.Tools
{
    public static class GameSceneInitializer
    {
        private const string ScenePath = "Assets/_Project/Scenes/SCN_Gameplay.unity";
        private const string RegistryPath = "Assets/_Project/Data/Registry/GameDataRegistry.asset";
        private const string StorageConfigPath = "Assets/_Project/Data/Configs/StorageUpgradeConfig.asset";
        private const string AnimalPenConfigPath = "Assets/_Project/Data/Configs/AnimalPenUpgradeConfig.asset";
        private const string LevelDataPath = "Assets/_Project/Data/Configs/SO_DefaultLevelData.asset";

        [MenuItem("NTVV/Setup Full Game Scene")]
        public static void CreateFullGameplayScene()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            if (!ValidateDependencies()) return;

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            SetupWorldRoot();

            GameObject systemsRoot = new GameObject("[SYSTEMS]");

            var saveMgr = systemsRoot.AddComponent<SaveLoadManager>();
            var gameMgr = systemsRoot.AddComponent<GameManager>();
            var timeMgr = systemsRoot.AddComponent<TimeManager>();

            SerializedObject gmSO = new SerializedObject(gameMgr);
            var registry = AssetDatabase.LoadAssetAtPath<GameDataRegistrySO>(RegistryPath);
            if (registry != null)
            {
                gmSO.FindProperty("_dataRegistry").objectReferenceValue = registry;
                gmSO.ApplyModifiedProperties();
            }

            GameObject gameplayRoot = new GameObject("[GAMEPLAY]");
            gameplayRoot.AddComponent<EconomySystem>();

            var levelSys = gameplayRoot.AddComponent<LevelSystem>();
            var storageSys = gameplayRoot.AddComponent<StorageSystem>();
            gameplayRoot.AddComponent<QuestManager>();

            if (registry != null)
            {
                var levelSO = new SerializedObject(levelSys);
                levelSO.FindProperty("_levelData").objectReferenceValue = registry.levelData;
                levelSO.ApplyModifiedProperties();

                var storageSO = new SerializedObject(storageSys);
                storageSO.FindProperty("_upgradeConfig").objectReferenceValue = registry.storageUpgradeConfig;
                storageSO.ApplyModifiedProperties();
            }

            SetupUISceneRoot();

            EditorSceneManager.SaveScene(scene, ScenePath);
            Debug.Log($"<color=green>[GameSceneInitializer]</color> Full Gameplay Scene created at: {ScenePath}");
        }

        private static bool ValidateDependencies()
        {
            GameDataRegistrySO registry = AssetDatabase.LoadAssetAtPath<GameDataRegistrySO>(RegistryPath);
            bool registryOK = registry != null && registry.levelData != null && registry.storageUpgradeConfig != null;

            if (!registryOK)
            {
                EditorUtility.DisplayDialog("Incomplete Assets",
                    "Core configurations are missing or unlinked in the Registry. Please run the data setup tools first.", "OK");
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
