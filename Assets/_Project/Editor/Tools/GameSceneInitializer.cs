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
using NTVV.Core;
using NTVV.Data.ScriptableObjects;

namespace NTVV.Editor.Tools
{
    public static class GameSceneInitializer
    {
        private const string ScenePath = "Assets/_Project/Scenes/SCN_Gameplay.unity";
        private const string RegistryPath = "Assets/_Project/Data/StaticData/GameDataRegistry.asset";
        private const string StylePath = "Assets/_Project/Settings/UI/DefaultFarmStyle.asset";

        [MenuItem("NTVV/Setup Full Game Scene")]
        public static void CreateFullGameplayScene()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            // 1. Create New Scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            
            // 2. Setup Systems Root
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

            // 3. Setup Gameplay Root
            GameObject gameplayRoot = new GameObject("[GAMEPLAY]");
            gameplayRoot.AddComponent<EconomySystem>();
            gameplayRoot.AddComponent<LevelSystem>();
            gameplayRoot.AddComponent<StorageSystem>();
            gameplayRoot.AddComponent<QuestManager>();

            // 4. Setup UI Root
            SetupUISceneRoot();

            // 5. Environmental Cleanup
            CleanupDefaultScene();

            // 6. Save Scene
            EditorSceneManager.SaveScene(scene, ScenePath);
            Debug.Log($"<color=green>[GameSceneInitializer]</color> Full Gameplay Scene created at: {ScenePath}");
        }

        private static void SetupUISceneRoot()
        {
            // Create Canvas
            GameObject canvasGO = new GameObject("[UI_CANVAS]");
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasGO.AddComponent<GraphicRaycaster>();

            // Popup Manager
            var popupMgr = canvasGO.AddComponent<PopupManager>();
            
            // Assign Style to Popup Manager
            SerializedObject pSO = new SerializedObject(popupMgr);
            var style = AssetDatabase.LoadAssetAtPath<UIStyleDataSO>(StylePath);
            if (style != null)
            {
                pSO.FindProperty("_activeStyle").objectReferenceValue = style;
            }

            // Setup Layers/Parents
            GameObject modalParent = new GameObject("ModalLayer", typeof(RectTransform));
            modalParent.transform.SetParent(canvasGO.transform, false);
            Stretch(modalParent.GetComponent<RectTransform>());
            
            GameObject hudParent = new GameObject("HUDLayer", typeof(RectTransform));
            hudParent.transform.SetParent(canvasGO.transform, false);
            Stretch(hudParent.GetComponent<RectTransform>());

            pSO.FindProperty("_modalParent").objectReferenceValue = modalParent.GetComponent<RectTransform>();
            pSO.FindProperty("_hudParent").objectReferenceValue = hudParent.GetComponent<RectTransform>();
            pSO.ApplyModifiedProperties();

            // Create HUD Mockup (Top Bar)
            CreateHUDMockup(hudParent.transform);
        }

        private static void CreateHUDMockup(Transform parent)
        {
            GameObject hudRoot = new GameObject("HUD_TopBar", typeof(RectTransform));
            hudRoot.transform.SetParent(parent, false);
            RectTransform rt = hudRoot.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(0.5f, 1);
            rt.sizeDelta = new Vector2(0, 100);
            rt.anchoredPosition = Vector2.zero;

            // Optional: Instantiate real Resource Chips if available
            string chipPath = "Assets/_Project/Prefabs/UI/Components/UI_Resource_Chip.prefab";
            GameObject chipPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(chipPath);
            if (chipPrefab != null)
            {
                GameObject goldChip = (GameObject)PrefabUtility.InstantiatePrefab(chipPrefab, hudRoot.transform);
                goldChip.name = "GoldChip";
                goldChip.GetComponent<RectTransform>().anchoredPosition = new Vector2(150, -50);
            }
        }

        private static void Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        private static void CleanupDefaultScene()
        {
            // Any specific cleanup for the New Scene defaults
        }
    }
}
