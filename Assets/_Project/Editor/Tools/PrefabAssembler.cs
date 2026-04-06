using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using NTVV.Data.ScriptableObjects;
using NTVV.World.Views;
using NTVV.UI.HUD;
using NTVV.UI.Common;
using TMPro;

namespace NTVV.Editor.Tools
{
    public class PrefabAssembler : EditorWindow
    {
        private const string UI_ROOT = "Assets/_Project/Resources/UI/Default/";
        private const string ATOM_ROOT = "Assets/_Project/Prefabs/UI/Components/";
        private const string WORLD_ROOT = "Assets/_Project/Prefabs/World/";
        private const string SHAPE_ROOT = "Assets/_Project/Textures/Shapes/";

        [MenuItem("NTVV/Setup/Assemble All Placeholders")]
        public static void AssembleAll()
        {
            AssetDatabase.StartAssetEditing();
            try
            {
                RepairData();
                UpgradeAtomicPrefabs(); // Step 1: Fix the small components
                AssembleHUD();          // Step 2: Assemble Top Bar
                AssembleBottomNav();    // Step 3: Assemble Bottom Nav
                CreateWorldPlaceholders();
                Debug.Log("<color=green>[PrefabAssembler]</color> All placeholders assembled and fully WIRED!");
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private static void UpgradeAtomicPrefabs()
        {
            // Wire UI_Resource_Chip
            UpgradePrefab<UIResourceChip>(ATOM_ROOT + "UI_Resource_Chip.prefab", (instance, so) => {
                so.FindProperty("_icon").objectReferenceValue = instance.transform.Find("Icon")?.GetComponent<Image>();
                so.FindProperty("_label").objectReferenceValue = instance.transform.Find("Text")?.GetComponent<TMP_Text>();
            });

            // Wire UI_Nav_Button
            UpgradePrefab<UINavButton>(ATOM_ROOT + "UI_Nav_Button.prefab", (instance, so) => {
                so.FindProperty("_icon").objectReferenceValue = instance.GetComponent<Image>();
                so.FindProperty("_label").objectReferenceValue = instance.transform.Find("Label")?.GetComponent<TMP_Text>();
                so.FindProperty("_button").objectReferenceValue = instance.GetComponent<Button>();
            });

            // Wire UI_XP_ProgressBar
            UpgradePrefab<UIProgressBar>(ATOM_ROOT + "UI_XP_ProgressBar.prefab", (instance, so) => {
                so.FindProperty("_fillImage").objectReferenceValue = instance.transform.Find("Fill")?.GetComponent<Image>();
            });
        }

        private static void AssembleHUD()
        {
            string path = UI_ROOT + "HUDTopBar.prefab";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) return;

            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            var controller = instance.GetComponent<HUDTopBarController>() ?? instance.AddComponent<HUDTopBarController>();
            SerializedObject so = new SerializedObject(controller);
            
            for (int i = instance.transform.childCount - 1; i >= 0; i--)
                Object.DestroyImmediate(instance.transform.GetChild(i).gameObject);

            var layout = instance.GetComponent<HorizontalLayoutGroup>() ?? instance.AddComponent<HorizontalLayoutGroup>();
            layout.childControlWidth = true;
            layout.childForceExpandWidth = false;
            layout.spacing = 20;

            GameObject chipPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ATOM_ROOT + "UI_Resource_Chip.prefab");
            GameObject xpPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ATOM_ROOT + "UI_XP_ProgressBar.prefab");

            if (chipPrefab != null)
            {
                var gold = InstantiateAndLink(chipPrefab, instance.transform, "Gold_Chip");
                so.FindProperty("_goldLabel").objectReferenceValue = gold.transform.Find("Text")?.GetComponent<TMP_Text>();

                var storage = InstantiateAndLink(chipPrefab, instance.transform, "Storage_Chip");
                so.FindProperty("_storageLabel").objectReferenceValue = storage.transform.Find("Text")?.GetComponent<TMP_Text>();
            }

            if (xpPrefab != null)
            {
                var xp = InstantiateAndLink(xpPrefab, instance.transform, "XP_Bar");
                so.FindProperty("_xpBarFill").objectReferenceValue = xp.transform.Find("Fill")?.GetComponent<Image>();
                so.FindProperty("_levelLabel").objectReferenceValue = xp.transform.Find("LevelText")?.GetComponent<TMP_Text>();
            }

            so.ApplyModifiedProperties();
            PrefabUtility.SaveAsPrefabAsset(instance, path);
            Object.DestroyImmediate(instance);
        }

        private static void AssembleBottomNav()
        {
            string path = UI_ROOT + "BottomNav.prefab";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) return;

            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            
            for (int i = instance.transform.childCount - 1; i >= 0; i--)
                Object.DestroyImmediate(instance.transform.GetChild(i).gameObject);

            var layout = instance.GetComponent<HorizontalLayoutGroup>() ?? instance.AddComponent<HorizontalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.spacing = 40;

            GameObject btnPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ATOM_ROOT + "UI_Nav_Button.prefab");
            if (btnPrefab != null)
            {
                string[] labels = { "Farm", "Storage", "Shop", "Quest", "Event" };
                foreach (var label in labels)
                {
                    var btn = InstantiateAndLink(btnPrefab, instance.transform, "Btn_" + label);
                    var btnCtrl = btn.GetComponent<UINavButton>();
                    if (btnCtrl != null) btnCtrl.SetLabel(label);
                }
            }

            PrefabUtility.SaveAsPrefabAsset(instance, path);
            Object.DestroyImmediate(instance);
        }

        #region Helpers
        private static void UpgradePrefab<T>(string path, System.Action<GameObject, SerializedObject> wireAction) where T : MonoBehaviour
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) return;

            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            T ctrl = instance.GetComponent<T>() ?? instance.AddComponent<T>();
            SerializedObject so = new SerializedObject(ctrl);
            
            wireAction(instance, so);
            
            so.ApplyModifiedProperties();
            PrefabUtility.SaveAsPrefabAsset(instance, path);
            Object.DestroyImmediate(instance);
        }

        private static GameObject InstantiateAndLink(GameObject prefab, Transform parent, string name)
        {
            GameObject go = PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
            go.name = name;
            return go;
        }

        private static void RepairData()
        {
            Sprite square = AssetDatabase.LoadAssetAtPath<Sprite>(SHAPE_ROOT + "Square.png");
            Sprite circle = AssetDatabase.LoadAssetAtPath<Sprite>(SHAPE_ROOT + "Circle.png");
            Sprite squircle = AssetDatabase.LoadAssetAtPath<Sprite>(SHAPE_ROOT + "Squircle.png");

            string[] cropGuids = AssetDatabase.FindAssets("t:CropDataSO");
            foreach (var guid in cropGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<CropDataSO>(path);
                if (asset.icon == null) asset.icon = square;
                if (asset.seedIcon == null) asset.seedIcon = squircle;
                EditorUtility.SetDirty(asset);
            }

            string[] animalGuids = AssetDatabase.FindAssets("t:AnimalDataSO");
            foreach (var guid in animalGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<AnimalDataSO>(path);
                if (asset.icon == null) asset.icon = circle;
                EditorUtility.SetDirty(asset);
            }
        }

        private static void CreateWorldPlaceholders()
        {
            if (!Directory.Exists(WORLD_ROOT)) Directory.CreateDirectory(WORLD_ROOT);

            GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tile.name = "Tile_Base";
            tile.transform.localScale = new Vector3(1, 0.1f, 1);
            SavePrefab(tile, WORLD_ROOT + "Tile_Base.prefab");

            GameObject plant = new GameObject("Plant_Placeholder");
            var sr = plant.AddComponent<SpriteRenderer>();
            sr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(SHAPE_ROOT + "Squircle.png");
            plant.AddComponent<CropTileView>();
            SavePrefab(plant, WORLD_ROOT + "Plant_Placeholder.prefab");

            GameObject animal = new GameObject("Animal_Placeholder");
            var asr = animal.AddComponent<SpriteRenderer>();
            asr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(SHAPE_ROOT + "Circle.png");
            animal.AddComponent<AnimalView>();
            SavePrefab(animal, WORLD_ROOT + "Animal_Placeholder.prefab");
        }

        private static void SavePrefab(GameObject go, string path)
        {
            PrefabUtility.SaveAsPrefabAsset(go, path);
            Object.DestroyImmediate(go);
        }
        #endregion
    }
}
