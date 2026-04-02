using UnityEngine;
using UnityEditor;
using NTVV.Data;
using NTVV.Data.ScriptableObjects;

namespace NTVV.Editor.Tools
{
    /// <summary>
    /// Centralized Management Window for all game static data.
    /// Allows syncing from JSON and visual inspection of Crops/Animals.
    /// </summary>
    public class GameDataManagerWindow : EditorWindow
    {
        private enum Tab { Crops, Animals, Quests, Settings }
        private Tab _currentTab = Tab.Crops;
        private Vector2 _sidebarScroll;
        private Vector2 _detailScroll;
        
        private GameDataRegistrySO _registry;
        private string _selectedId;
        private Object _selectedObject;
        private UnityEditor.Editor _cachedEditor;

        [MenuItem("NTVV/Game Data Manager")]
        public static void ShowWindow()
        {
            GetWindow<GameDataManagerWindow>("Data Manager");
        }

        private void OnEnable()
        {
            RefreshRegistry();
        }

        private void OnDisable()
        {
            if (_cachedEditor != null) DestroyImmediate(_cachedEditor);
        }

        private void RefreshRegistry()
        {
            _registry = DataImportUtility.GetOrCreateRegistry();
            if (_registry != null)
            {
                DataImportUtility.SyncQuestsFromAssets(_registry);
                _registry.Initialize();
            }
        }

        private void OnGUI()
        {
            DrawToolbar();
            
            EditorGUILayout.BeginHorizontal();
            
            if (_currentTab == Tab.Settings)
            {
                DrawSettingsTab();
            }
            else
            {
                DrawSidebar();
                DrawDetailView();
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            
            Tab oldTab = _currentTab;
            if (GUILayout.Toggle(_currentTab == Tab.Crops, "Crops", EditorStyles.toolbarButton)) _currentTab = Tab.Crops;
            if (GUILayout.Toggle(_currentTab == Tab.Animals, "Animals", EditorStyles.toolbarButton)) _currentTab = Tab.Animals;
            if (GUILayout.Toggle(_currentTab == Tab.Quests, "Quests", EditorStyles.toolbarButton)) _currentTab = Tab.Quests;
            if (GUILayout.Toggle(_currentTab == Tab.Settings, "Settings", EditorStyles.toolbarButton)) _currentTab = Tab.Settings;

            if (oldTab != _currentTab)
            {
                _selectedId = string.Empty;
                _selectedObject = null;
                if (_cachedEditor != null) DestroyImmediate(_cachedEditor);
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Sync from JSON", EditorStyles.toolbarButton))
            {
                DataImportUtility.ImportAll();
                RefreshRegistry();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawSidebar()
        {
            _sidebarScroll = EditorGUILayout.BeginScrollView(_sidebarScroll, GUILayout.Width(250), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            if (_registry == null)
            {
                EditorGUILayout.LabelField("Registry not found.");
            }
            else
            {
                if (_currentTab == Tab.Crops)
                {
                    foreach (var crop in _registry.crops)
                    {
                        if (crop == null) continue;
                        DrawItemButton(crop.data.cropId, crop.data.cropName, crop);
                    }
                }
                else if (_currentTab == Tab.Animals)
                {
                    foreach (var animal in _registry.animals)
                    {
                        if (animal == null) continue;
                        DrawItemButton(animal.data.animalId, animal.data.animalName, animal);
                    }
                }
                else if (_currentTab == Tab.Quests)
                {
                    if (GUILayout.Button("Scan & Register Quests", EditorStyles.miniButton))
                    {
                        DataImportUtility.SyncQuestsFromAssets(_registry);
                        RefreshRegistry();
                    }
                    EditorGUILayout.Space(5);

                    foreach (var quest in _registry.quests)
                    {
                        if (quest == null) continue;
                        DrawItemButton(quest.questId, quest.questName, quest);
                    }
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        private void DrawItemButton(string id, string name, Object obj)
        {
            bool isSelected = (_selectedId == id);
            GUI.backgroundColor = isSelected ? Color.cyan : Color.white;
            
            if (GUILayout.Button($"{name} ({id})", EditorStyles.miniButton, GUILayout.Height(25)))
            {
                _selectedId = id;
                _selectedObject = obj;
                GUI.FocusControl(null);
            }
            
            GUI.backgroundColor = Color.white;
        }

        private void DrawDetailView()
        {
            _detailScroll = EditorGUILayout.BeginScrollView(_detailScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(10, 10, 10, 10) });

            if (_selectedObject == null)
            {
                EditorGUILayout.LabelField("Select an item to view details.", EditorStyles.centeredGreyMiniLabel);
            }
            else
            {
                DrawDetailHeader();
                EditorGUILayout.Space(10);
                
                // Optimized: Cache the editor to avoid creating it every frame
                if (_cachedEditor == null || _cachedEditor.target != _selectedObject)
                {
                    if (_cachedEditor != null) DestroyImmediate(_cachedEditor);
                    _cachedEditor = UnityEditor.Editor.CreateEditor(_selectedObject);
                }

                if (_cachedEditor != null)
                {
                    _cachedEditor.OnInspectorGUI();
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        private void DrawDetailHeader()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Selected: {_selectedId}", EditorStyles.boldLabel);
            if (GUILayout.Button("Ping Asset", GUILayout.Width(100)))
            {
                EditorGUIUtility.PingObject(_selectedObject);
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(2);
            Handles.color = Color.gray;
            Handles.DrawLine(new Vector3(0, GUILayoutUtility.GetLastRect().yMax + 5), new Vector3(position.width, GUILayoutUtility.GetLastRect().yMax + 5));
        }

        private void DrawSettingsTab()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 20, 20) });
            
            EditorGUILayout.LabelField("Global Game Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Storage System", EditorStyles.miniBoldLabel);
            EditorGUILayout.Space(5);

            if (GUILayout.Button("Create Storage Upgrade Config Asset", GUILayout.Height(30)))
            {
                CreateStorageConfig();
            }
            
            EditorGUILayout.HelpBox("Use this to define Gold costs and Level requirements for expanding the warehouse.", MessageType.Info);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(15);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Animal Pen System", EditorStyles.miniBoldLabel);
            EditorGUILayout.Space(5);

            if (GUILayout.Button("Create Animal Pen Upgrade Config Asset", GUILayout.Height(30)))
            {
                CreateAnimalPenConfig();
            }
            
            EditorGUILayout.HelpBox("Use this to define capacity tiers and upgrade costs for animal coops and barns.", MessageType.Info);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(15);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Quest System", EditorStyles.miniBoldLabel);
            EditorGUILayout.Space(5);

            if (GUILayout.Button("Create New Quest Asset", GUILayout.Height(30)))
            {
                CreateQuestAsset();
            }
            
            EditorGUILayout.HelpBox("Use this to create a single Quest configuration asset.", MessageType.Info);
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
        }

        private void CreateStorageConfig()
        {
            string path = "Assets/_Project/Data/StorageUpgradeConfig.asset";
            
            // Check if directory exists
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Data"))
            {
                AssetDatabase.CreateFolder("Assets/_Project", "Data");
            }

            StorageUpgradeDataSO asset = ScriptableObject.CreateInstance<StorageUpgradeDataSO>();
            
            // TẠO ĐƯỜNG DẪN DUY NHẤT (tránh ghi đè file cũ)
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            
            Debug.Log($"<color=green>[NTVV]</color> Storage Upgrade Config created at: {path}");
        }

        private void CreateAnimalPenConfig()
        {
            string path = "Assets/_Project/Data/AnimalPenUpgradeConfig.asset";
            
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Data"))
            {
                AssetDatabase.CreateFolder("Assets/_Project", "Data");
            }

            AnimalPenUpgradeDataSO asset = ScriptableObject.CreateInstance<AnimalPenUpgradeDataSO>();
            
            // TẠO ĐƯỜNG DẪN DUY NHẤT (tránh ghi đè file cũ)
            path = AssetDatabase.GenerateUniqueAssetPath(path);

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            
            Debug.Log($"<color=green>[NTVV]</color> Animal Pen Upgrade Config created at: {path}");
        }

        private void CreateQuestAsset()
        {
            string path = "Assets/_Project/Data/Quests/NewQuest.asset";
            
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Data/Quests"))
            {
                if (!AssetDatabase.IsValidFolder("Assets/_Project/Data"))
                    AssetDatabase.CreateFolder("Assets/_Project", "Data");
                AssetDatabase.CreateFolder("Assets/_Project/Data", "Quests");
            }

            QuestDataSO asset = ScriptableObject.CreateInstance<QuestDataSO>();
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            
            Debug.Log($"<color=green>[NTVV]</color> Quest Asset created at: {path}");
        }
    }
}
