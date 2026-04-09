using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NTVV.Data;
using NTVV.Data.ScriptableObjects;
using NTVV.UI.Styling;
using NTVV.UI.Common;
using System.IO;

namespace NTVV.Editor.Tools
{
    /// <summary>
    /// Centralized Management Window for all game static data.
    /// Allows syncing from JSON and visual inspection of Crops/Animals.
    /// </summary>
    public class GameDataManagerWindow : EditorWindow
    {
        private enum Tab { Crops, Animals, Quests, UI, Settings }
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
            if (GUILayout.Toggle(_currentTab == Tab.UI, "UI/Themes", EditorStyles.toolbarButton)) _currentTab = Tab.UI;
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

            if (_registry == null && _currentTab != Tab.UI)
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
                else if (_currentTab == Tab.UI)
                {
                    DrawUITabSidebar();
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        private void DrawUITabSidebar()
        {
            if (GUILayout.Button("＋ Create New Theme", GUILayout.Height(30)))
            {
                CreateNewTheme();
            }
            
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Available Themes", EditorStyles.centeredGreyMiniLabel);
            
            string[] guids = AssetDatabase.FindAssets("t:UIStyleDataSO");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                UIStyleDataSO style = AssetDatabase.LoadAssetAtPath<UIStyleDataSO>(path);
                if (style != null)
                {
                    DrawItemButton(style.themeFolderName, style.name, style);
                }
            }
        }

        private void DrawItemButton(string id, string name, Object obj)
        {
            bool isSelected = (_selectedObject == obj);
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

                if (_currentTab == Tab.UI && _selectedObject is UIStyleDataSO style)
                {
                    DrawUIStyleDetails(style);
                }
                else
                {
                    DrawGenericEditor();
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        private List<GameObject> _manualTargets = new List<GameObject>();
        private bool _showManualGroup = true;
        private bool _showThemePrefabsGroup = true;
        private bool _showStyleProperties = true;
        private List<GameObject> _themePrefabs = new List<GameObject>();

        private void DrawUIStyleDetails(UIStyleDataSO style)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Theme Actions", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Set as Active Style", GUILayout.Height(25)))
            {
                SetThemeActive(style);
            }
            if (GUILayout.Button("Clone Theme", GUILayout.Height(25)))
            {
                CloneTheme(style);
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(10);
            DrawTargetedApplicationPanel(style);
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(10);
            
            _showStyleProperties = EditorGUILayout.BeginFoldoutHeaderGroup(_showStyleProperties, "3. Style Properties (Master Config)");
            if (_showStyleProperties)
            {
                DrawGenericEditor();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void DrawTargetedApplicationPanel(UIStyleDataSO style)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            // --- 1. Manual Targets Group ---
            _showManualGroup = EditorGUILayout.BeginFoldoutHeaderGroup(_showManualGroup, "1. Manual Scene Objects / Prefabs (UI Canvas)");
            if (_showManualGroup)
            {
                Rect dropArea = EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                HandleManualDragAndDrop(dropArea);
                
                if (_manualTargets.Count == 0)
                {
                    EditorGUILayout.HelpBox("Danh sách trống. Kéo thả GameObject hoặc Prefab vào đây.", MessageType.Info);
                }

                for (int i = 0; i < _manualTargets.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    
                    // Object Field
                    _manualTargets[i] = (GameObject)EditorGUILayout.ObjectField(_manualTargets[i], typeof(GameObject), true);
                    
                    // Apply Button
                    GUI.enabled = _manualTargets[i] != null;
                    if (GUILayout.Button("Apply", GUILayout.Width(50)))
                    {
                        ApplyStyleToSingleTarget(style, _manualTargets[i]);
                    }
                    
                    // Ping Button
                    if (GUILayout.Button("🔍", GUILayout.Width(30)))
                    {
                        EditorGUIUtility.PingObject(_manualTargets[i]);
                    }
                    
                    // Remove Button
                    GUI.enabled = true;
                    GUI.backgroundColor = new Color(1f, 0.7f, 0.7f);
                    if (GUILayout.Button("X", GUILayout.Width(25)))
                    {
                        _manualTargets.RemoveAt(i);
                        i--; // Adjust index
                    }
                    GUI.backgroundColor = Color.white;
                    
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.Space(5);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("＋ Add New Slot", GUILayout.Height(20)))
                {
                    _manualTargets.Add(null);
                }
                if (GUILayout.Button("Clear Nulls", GUILayout.Width(80)))
                {
                    _manualTargets.RemoveAll(x => x == null);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(5);
                GUI.backgroundColor = new Color(0.7f, 1f, 0.7f);
                if (GUILayout.Button("Apply Style to ALL Manual Targets", GUILayout.Height(30)))
                {
                    foreach (var target in _manualTargets)
                    {
                        if (target != null) ApplyStyleToSingleTarget(style, target);
                    }
                    EditorUtility.DisplayDialog("Success", "Applied style to all valid manual targets.", "OK");
                }
                GUI.backgroundColor = Color.white;
                
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space(5);

            // --- 2. Theme Prefabs Group ---
            _showThemePrefabsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(_showThemePrefabsGroup, "2. Related Theme Prefabs (UI World)");
            if (_showThemePrefabsGroup)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                RefreshThemePrefabs(style);
                
                if (_themePrefabs.Count == 0)
                {
                    EditorGUILayout.HelpBox("Không tìm thấy Prefab nào trong thư mục Theme.", MessageType.None);
                }
                else
                {
                    foreach (var pf in _themePrefabs)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(pf.name, EditorStyles.wordWrappedLabel);
                        
                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("Apply", GUILayout.Width(60)))
                        {
                            ApplyStyleToPrefab(style, pf);
                        }

                        if (GUILayout.Button("🔍", GUILayout.Width(30)))
                        {
                            EditorGUIUtility.PingObject(pf);
                        }
                        
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space(5);
                    GUI.backgroundColor = new Color(0.7f, 1.0f, 0.8f);
                    if (GUILayout.Button("Apply Style to ALL Theme Prefabs", GUILayout.Height(30)))
                    {
                        foreach (var pf in _themePrefabs) ApplyStyleToPrefab(style, pf);
                        EditorUtility.DisplayDialog("Success", "Applied style to all prefabs in theme folder.", "OK");
                    }
                    GUI.backgroundColor = Color.white;
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.EndVertical();
        }

        private void RefreshThemePrefabs(UIStyleDataSO style)
        {
            if (Event.current.type != EventType.Layout) return;
            
            _themePrefabs.Clear();
            string themePath = $"Assets/_Project/Resources/UI/{style.themeFolderName}";
            if (AssetDatabase.IsValidFolder(themePath))
            {
                string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { themePath });
                foreach (var guid in guids)
                {
                    GameObject pf = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
                    if (pf != null) _themePrefabs.Add(pf);
                }
            }
        }

        private void ApplyStyleToSingleTarget(UIStyleDataSO style, GameObject target)
        {
            if (target == null) return;

            UIStyleProcessor.ApplyStyle(target, style);
            EditorUtility.SetDirty(target);
            Debug.Log($"[UIStyle] Applied style to {target.name}");
        }

        private void HandleManualDragAndDrop(Rect dropArea)
        {
            Event evt = Event.current;
            if (!dropArea.Contains(evt.mousePosition)) return;

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        foreach (var obj in DragAndDrop.objectReferences)
                        {
                            if (obj is GameObject go)
                            {
                                if (!_manualTargets.Contains(go))
                                    _manualTargets.Add(go);
                            }
                        }
                        evt.Use();
                    }
                    break;
            }
        }

        private void ApplyStyleToPrefab(UIStyleDataSO style, GameObject prefab)
        {
            // For prefabs, we need to open them, edit, and save
            string path = AssetDatabase.GetAssetPath(prefab);
            GameObject instance = PrefabUtility.LoadPrefabContents(path);
            
            UIStyleProcessor.ApplyStyle(instance, style);
            
            PrefabUtility.SaveAsPrefabAsset(instance, path);
            PrefabUtility.UnloadPrefabContents(instance);
            
            Debug.Log($"[UIStyle] Applied style to Prefab: {prefab.name}");
        }

        private void DrawGenericEditor()
        {
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

        private void CreateNewTheme()
        {
            string path = EditorUtility.SaveFilePanelInProject("Create New Theme", "NewFarmStyle", "asset", "Enter name for new UI theme", "Assets/_Project/Settings/UI");
            if (string.IsNullOrEmpty(path)) return;

            UIStyleDataSO style = ScriptableObject.CreateInstance<UIStyleDataSO>();
            style.themeFolderName = Path.GetFileNameWithoutExtension(path);
            
            AssetDatabase.CreateAsset(style, path);
            EnsureUIThemeFolder(style.themeFolderName);
            
            AssetDatabase.SaveAssets();
            _selectedObject = style;
        }

        private void CloneTheme(UIStyleDataSO source)
        {
            string path = EditorUtility.SaveFilePanelInProject("Clone Theme", $"{source.name}_Copy", "asset", "Enter name for cloned theme", "Assets/_Project/Settings/UI");
            if (string.IsNullOrEmpty(path)) return;

            UIStyleDataSO newStyle = Instantiate(source);
            newStyle.themeFolderName = Path.GetFileNameWithoutExtension(path);
            
            AssetDatabase.CreateAsset(newStyle, path);
            
            if (EditorUtility.DisplayDialog("Clone Prefabs?", $"Would you like to copy all UI prefabs from '{source.themeFolderName}' to the new theme folder '{newStyle.themeFolderName}'?", "Yes", "No"))
            {
                CopyThemePrefabs(source.themeFolderName, newStyle.themeFolderName);
            }
            else
            {
                EnsureUIThemeFolder(newStyle.themeFolderName);
            }

            AssetDatabase.SaveAssets();
            _selectedObject = newStyle;
        }

        private void CopyThemePrefabs(string sourceFolder, string targetFolder)
        {
            string sourcePath = $"Assets/_Project/Resources/UI/{sourceFolder}";
            string targetPath = $"Assets/_Project/Resources/UI/{targetFolder}";
            
            EnsureUIThemeFolder(targetFolder);
            
            if (AssetDatabase.IsValidFolder(sourcePath))
            {
                string[] files = Directory.GetFiles(sourcePath);
                foreach (string file in files)
                {
                    if (file.EndsWith(".meta")) continue;
                    string fileName = Path.GetFileName(file);
                    AssetDatabase.CopyAsset(file, $"{targetPath}/{fileName}");
                }
                Debug.Log($"[UIThemes] Successfully copied prefabs from {sourceFolder} to {targetFolder}");
            }
        }

        private void EnsureUIThemeFolder(string folderName)
        {
            string parent = "Assets/_Project/Resources/UI";
            if (!AssetDatabase.IsValidFolder(parent))
            {
                if (!AssetDatabase.IsValidFolder("Assets/_Project/Resources"))
                    AssetDatabase.CreateFolder("Assets/_Project", "Resources");
                AssetDatabase.CreateFolder("Assets/_Project/Resources", "UI");
            }
            
            if (!AssetDatabase.IsValidFolder($"{parent}/{folderName}"))
            {
                AssetDatabase.CreateFolder(parent, folderName);
            }
        }

        private void SetThemeActive(UIStyleDataSO style)
        {
            PopupManager manager = GameObject.FindFirstObjectByType<PopupManager>();
            if (manager == null)
            {
                EditorUtility.DisplayDialog("Error", "PopupManager not found in current scene.", "OK");
                return;
            }

            SerializedObject so = new SerializedObject(manager);
            so.FindProperty("_activeStyle").objectReferenceValue = style;
            so.ApplyModifiedProperties();
            
            Debug.Log($"[UIThemes] Theme '{style.name}' set as active in scene.");
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
            string folder = "Assets/_Project/Data/Configs";
            if (!AssetDatabase.IsValidFolder(folder))
            {
                if (!AssetDatabase.IsValidFolder("Assets/_Project/Data")) AssetDatabase.CreateFolder("Assets/_Project", "Data");
                AssetDatabase.CreateFolder("Assets/_Project/Data", "Configs");
            }

            string path = folder + "/StorageUpgradeConfig.asset";
            StorageUpgradeDataSO asset = ScriptableObject.CreateInstance<StorageUpgradeDataSO>();
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        private void CreateAnimalPenConfig()
        {
            string folder = "Assets/_Project/Data/Configs";
            if (!AssetDatabase.IsValidFolder(folder))
            {
                if (!AssetDatabase.IsValidFolder("Assets/_Project/Data")) AssetDatabase.CreateFolder("Assets/_Project", "Data");
                AssetDatabase.CreateFolder("Assets/_Project/Data", "Configs");
            }

            string path = folder + "/AnimalPenUpgradeConfig.asset";
            AnimalPenUpgradeDataSO asset = ScriptableObject.CreateInstance<AnimalPenUpgradeDataSO>();
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        private void CreateQuestAsset()
        {
            string path = "Assets/_Project/Data/Quests/NewQuest.asset";
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Data/Quests"))
            {
                if (!AssetDatabase.IsValidFolder("Assets/_Project/Data")) AssetDatabase.CreateFolder("Assets/_Project", "Data");
                AssetDatabase.CreateFolder("Assets/_Project/Data", "Quests");
            }
            QuestDataSO asset = ScriptableObject.CreateInstance<QuestDataSO>();
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}
