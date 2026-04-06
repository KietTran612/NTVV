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

        [MenuItem("NTVV/Setup/Assemble All (Create or Verify)")]
        public static void AssembleAll()
        {
            AssetDatabase.StartAssetEditing();
            try
            {
                RepairData();
                UpgradeAtomicPrefabs(); 
                AssembleShopItems();    
                AssembleInventorySlots(); 
                AssembleQuestItems();
                AssembleHUD();          
                AssembleBottomNav();    

                // NEW: Popup Assembly
                AssembleShopPopup();
                AssembleStoragePopup();
                AssembleQuestPopup();

                CreateWorldPlaceholders();
                Debug.Log("<color=green>[PrefabAssembler]</color> All prefabs VERIFIED and REPAIRED!");
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // ATOMIC PREFABS — Rebuild hierarchy from scratch for clean naming
        // ─────────────────────────────────────────────────────────────────

        private static void UpgradeAtomicPrefabs()
        {
            // UI_Resource_Chip
            string chipPath = ATOM_ROOT + "UI_Resource_Chip.prefab";
            GameObject chipPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(chipPath);
            if (chipPrefab != null)
            {
                VerifyAndRepair<UIResourceChip>(chipPrefab, chipPath, (instance, so) =>
                {
                    CreateUIObject("Resource_Icon", instance.transform, typeof(Image));
                    CreateUIObject("Value_Label",   instance.transform, typeof(TextMeshProUGUI));
                    VerifyField<Image>   (so, "_resource_Icon", instance.transform, "Resource_Icon");
                    VerifyField<TMP_Text>(so, "_value_Label",   instance.transform, "Value_Label");
                });
            }

            // UI_Nav_Button
            string navPath = ATOM_ROOT + "UI_Nav_Button.prefab";
            GameObject navPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(navPath);
            if (navPrefab != null)
            {
                VerifyAndRepair<UINavButton>(navPrefab, navPath, (instance, so) =>
                {
                    if (instance.GetComponent<Button>() == null) instance.AddComponent<Button>();
                    CreateUIObject("Nav_Icon",  instance.transform, typeof(Image));
                    CreateUIObject("Nav_Label", instance.transform, typeof(TextMeshProUGUI));
                    VerifyField<Image>   (so, "_nav_Icon",   instance.transform, "Nav_Icon");
                    VerifyField<TMP_Text>(so, "_nav_Label",  instance.transform, "Nav_Label");
                    if (so.FindProperty("_nav_Button").objectReferenceValue == null)
                         so.FindProperty("_nav_Button").objectReferenceValue = instance.GetComponent<Button>();
                });
            }

            // UI_XP_ProgressBar
            string xpPath = ATOM_ROOT + "UI_XP_ProgressBar.prefab";
            GameObject xpPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(xpPath);
            if (xpPrefab != null)
            {
                VerifyAndRepair<UIProgressBar>(xpPrefab, xpPath, (instance, so) =>
                {
                    CreateUIObject("XP_Fill",     instance.transform, typeof(Image));
                    CreateUIObject("Level_Label", instance.transform, typeof(TextMeshProUGUI)); // Visual child
                    VerifyField<Image>(so, "_xp_Fill", instance.transform, "XP_Fill");
                    // We don't wire _level_Label to UIProgressBar anymore, 
                    // HUDTopBarController will wire it directly to this child.
                });
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // NEW ATOM PREFABS — Built from scratch, correct types from start
        // ─────────────────────────────────────────────────────────────────

        private static void AssembleShopItems()
        {
            string path = ATOM_ROOT + "ShopEntry_Seed.prefab";
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (existing == null)
            {
                GameObject go = new GameObject("ShopEntry_Seed");
                go.AddComponent<RectTransform>().sizeDelta = new Vector2(320, 480);
                go.AddComponent<NTVV.UI.Panels.ShopEntryController>();
                
                CreateUIObject("Item_Icon",   go.transform, typeof(Image));
                CreateUIObject("Name_Label",  go.transform, typeof(TextMeshProUGUI));
                var priceRow = CreateUIObject("Price_Row", go.transform, typeof(HorizontalLayoutGroup));
                CreateUIObject("Currency_Icon", priceRow.transform, typeof(Image));
                CreateUIObject("Price_Label",   priceRow.transform, typeof(TextMeshProUGUI));
                CreateUIObject("Buy_Button", go.transform, typeof(Button));

                GameObject newPrefab = PrefabUtility.SaveAsPrefabAsset(go, path);
                Object.DestroyImmediate(go);
                existing = newPrefab;
            }

            VerifyAndRepair<NTVV.UI.Panels.ShopEntryController>(existing, path, (instance, so) =>
            {
                VerifyField<TMP_Text>(so, "_name_Label",  instance.transform, "Name_Label");
                VerifyField<TMP_Text>(so, "_price_Label", instance.transform, "Price_Label");
                VerifyField<Image>   (so, "_item_Icon",   instance.transform, "Item_Icon");
                VerifyField<Button>  (so, "_buy_Button",  instance.transform, "Buy_Button");
            });
        }

        private static void AssembleInventorySlots()
        {
            string path = ATOM_ROOT + "InventorySlot.prefab";
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (existing == null)
            {
                GameObject go = new GameObject("InventorySlot");
                go.AddComponent<RectTransform>().sizeDelta = new Vector2(200, 220);
                go.AddComponent<NTVV.UI.Panels.InventorySlotController>();

                CreateUIObject("Item_Icon",      go.transform, typeof(Image));
                CreateUIObject("Quantity_Label", go.transform, typeof(TextMeshProUGUI));
                CreateUIObject("Sell_Button",    go.transform, typeof(Button));

                GameObject newPrefab = PrefabUtility.SaveAsPrefabAsset(go, path);
                Object.DestroyImmediate(go);
                existing = newPrefab;
            }

            VerifyAndRepair<NTVV.UI.Panels.InventorySlotController>(existing, path, (instance, so) =>
            {
                VerifyField<Image>   (so, "_item_Icon",      instance.transform, "Item_Icon");
                VerifyField<TMP_Text>(so, "_quantity_Label", instance.transform, "Quantity_Label");
                VerifyField<Button>  (so, "_sell_Button",    instance.transform, "Sell_Button");
            });
        }

        private static void AssembleQuestItems()
        {
            string path = ATOM_ROOT + "QuestListItem.prefab";
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (existing == null)
            {
                GameObject go = new GameObject("QuestListItem");
                go.AddComponent<RectTransform>().sizeDelta = new Vector2(800, 150);
                go.AddComponent<NTVV.UI.Items.QuestListItem>();

                CreateUIObject("Quest_Icon",    go.transform, typeof(Image));
                CreateUIObject("Name_Label",    go.transform, typeof(TextMeshProUGUI));
                CreateUIObject("Desc_Label",    go.transform, typeof(TextMeshProUGUI));
                CreateUIObject("Progress_Label", go.transform, typeof(TextMeshProUGUI));
                CreateUIObject("Reward_Label",   go.transform, typeof(TextMeshProUGUI));
                CreateUIObject("Claim_Button",  go.transform, typeof(Button));

                GameObject newPrefab = PrefabUtility.SaveAsPrefabAsset(go, path);
                Object.DestroyImmediate(go);
                existing = newPrefab;
            }

            VerifyAndRepair<NTVV.UI.Items.QuestListItem>(existing, path, (instance, so) =>
            {
                VerifyField<TMP_Text>(so, "_questNameLabel", instance.transform, "Name_Label");
                VerifyField<TMP_Text>(so, "_questDescLabel", instance.transform, "Desc_Label");
                VerifyField<TMP_Text>(so, "_progressLabel",  instance.transform, "Progress_Label");
                VerifyField<Image>   (so, "_questIcon",      instance.transform, "Quest_Icon");
                VerifyField<Button>  (so, "_btnClaim",       instance.transform, "Claim_Button");
                VerifyField<TMP_Text>(so, "_rewardLabel",    instance.transform, "Reward_Label");
            });
        }

        // ─────────────────────────────────────────────────────────────────
        // HUD TOP BAR — Destroy & rebuild children from atom prefabs
        // ─────────────────────────────────────────────────────────────────

        private static void AssembleHUD()
        {
            string path = UI_ROOT + "HUDTopBar.prefab";
            GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing == null) return;

            VerifyAndRepair<HUDTopBarController>(existing, path, (instance, so) =>
            {
                var layout = instance.GetComponent<HorizontalLayoutGroup>() ?? instance.AddComponent<HorizontalLayoutGroup>();
                layout.childControlWidth = true;
                layout.childForceExpandWidth = false;
                layout.spacing = 20;

                GameObject chipPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ATOM_ROOT + "UI_Resource_Chip.prefab");
                GameObject xpPrefab   = AssetDatabase.LoadAssetAtPath<GameObject>(ATOM_ROOT + "UI_XP_ProgressBar.prefab");

                if (chipPrefab != null)
                {
                    Transform gold = EnsureChildFromPrefab(instance.transform, chipPrefab, "Gold_Chip");
                    VerifyField<TMP_Text>(so, "_gold_Label", gold, "Value_Label");

                    Transform storage = EnsureChildFromPrefab(instance.transform, chipPrefab, "Storage_Chip");
                    VerifyField<TMP_Text>(so, "_storage_Label", storage, "Value_Label");
                }

                if (xpPrefab != null)
                {
                    Transform xp = EnsureChildFromPrefab(instance.transform, xpPrefab, "XP_Bar");
                    // Wire HUDTopBar properties TO the children of the XP_Bar atom
                    VerifyField<Image>   (so, "_xp_Fill",     xp, "XP_Fill");
                    VerifyField<TMP_Text>(so, "_level_Label", xp, "Level_Label");
                }
            });
        }

        private static void AssembleBottomNav()
        {
            string path = UI_ROOT + "BottomNav.prefab";
            GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing == null) return;

            VerifyAndRepair<MonoBehaviour>(existing, path, (instance, so) =>
            {
                var layout = instance.GetComponent<HorizontalLayoutGroup>() ?? instance.AddComponent<HorizontalLayoutGroup>();
                layout.childAlignment = TextAnchor.MiddleCenter;
                layout.spacing = 40;

                GameObject btnPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ATOM_ROOT + "UI_Nav_Button.prefab");
                if (btnPrefab != null)
                {
                    string[] labels = { "Farm", "Storage", "Shop", "Quest", "Event" };
                    foreach (var label in labels)
                    {
                        var btn = EnsureChildFromPrefab(instance.transform, btnPrefab, "Btn_" + label);
                        var btnCtrl = btn.GetComponent<UINavButton>();
                        if (btnCtrl != null) btnCtrl.SetLabel(label);
                    }
                }
            });
        }

        // ─────────────────────────────────────────────────────────────────
        // POPUP ASSEMBLY
        // ─────────────────────────────────────────────────────────────────

        private static void AssembleShopPopup()
        {
            string path = UI_ROOT + "ShopPopup.prefab";
            GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing == null) return;

            VerifyAndRepair<NTVV.UI.Panels.ShopPanelController>(existing, path, (instance, so) =>
            {
                VerifyField<TMP_Text> (so, "_goldBalanceLabel",   instance.transform, "GoldBalance_Label");
                VerifyField<Transform>(so, "_shopContentContainer", instance.transform, "Items_Content");
                VerifyField<Button>   (so, "_btnClose",           instance.transform, "Close_Button");
                VerifyField<Button>   (so, "_tabSeeds",           instance.transform, "Tab_Seeds");
                VerifyField<Button>   (so, "_tabAnimals",         instance.transform, "Tab_Animals");

                // Templates
                GameObject itemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ATOM_ROOT + "ShopEntry_Seed.prefab");
                if (itemPrefab != null) so.FindProperty("_shopItemPrefab").objectReferenceValue = itemPrefab;
            });
        }

        private static void AssembleStoragePopup()
        {
            string path = UI_ROOT + "StoragePopup.prefab";
            GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing == null) return;

            VerifyAndRepair<NTVV.UI.Panels.StoragePanelController>(existing, path, (instance, so) =>
            {
                // Main Panel
                VerifyField<TMP_Text> (so, "_capacityText",      instance.transform, "Capacity_Label");
                VerifyField<Transform>(so, "_storageContentContainer", instance.transform, "Items_Content");
                VerifyField<Button>   (so, "_btnClose",          instance.transform, "Close_Button");
                VerifyField<Button>   (so, "_btnUpgrade",        instance.transform, "Upgrade_Button");
                VerifyField<TMP_Text> (so, "_upgradeCostText",   instance.transform, "UpgradeCost_Label");

                // Templates
                GameObject itemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ATOM_ROOT + "InventorySlot.prefab");
                if (itemPrefab != null) so.FindProperty("_itemCardPrefab").objectReferenceValue = itemPrefab;

                // Sell Panel
                VerifyField<TMP_Text>(so, "_selectedItemNameText", instance.transform, "SelectedName_Label");
                VerifyField<TMP_Text>(so, "_quantityText",         instance.transform, "Quantity_Label");
                VerifyField<TMP_Text>(so, "_totalPriceText",      instance.transform, "TotalPrice_Label");
                VerifyField<Button>  (so, "_btnPlus",              instance.transform, "Plus_Button");
                VerifyField<Button>  (so, "_btnMinus",             instance.transform, "Minus_Button");
                VerifyField<Button>  (so, "_btnSellNow",           instance.transform, "Sell_Button");
            });
        }

        private static void AssembleQuestPopup()
        {
            string path = UI_ROOT + "QuestPopup.prefab";
            GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing == null) return;

            VerifyAndRepair<NTVV.UI.Panels.QuestPanelController>(existing, path, (instance, so) =>
            {
                VerifyField<Transform>(so, "_questListContainer", instance.transform, "Quests_Content");
                VerifyField<Button>   (so, "_btnClose",           instance.transform, "Close_Button");

                // Templates & Additional
                CreateUIObject("Empty_Message", instance.transform, typeof(TextMeshProUGUI));
                VerifyField<TMP_Text>(so, "_emptyMessage", instance.transform, "Empty_Message");

                GameObject itemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ATOM_ROOT + "QuestListItem.prefab");
                if (itemPrefab != null) so.FindProperty("_questItemPrefab").objectReferenceValue = itemPrefab;
            });
        }

        // ─────────────────────────────────────────────────────────────────
        // HELPERS
        // ─────────────────────────────────────────────────────────────────

        private static void VerifyAndRepair<T>(GameObject prefab, string path, System.Action<GameObject, SerializedObject> action) where T : MonoBehaviour
        {
            if (prefab == null) { Debug.LogError($"Prefab at {path} is null. Cannot Repair."); return; }
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            T ctrl = instance.GetComponent<T>();
            if (ctrl == null && typeof(T) != typeof(MonoBehaviour)) 
                ctrl = instance.AddComponent<T>();

            SerializedObject so = ctrl != null ? new SerializedObject(ctrl) : null;
            
            action(instance, so);

            if (so != null && so.ApplyModifiedProperties())
            {
                // Safety: Clean up any missing scripts before saving (prevents SaveAsPrefabAsset errors)
                int removedCount = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(instance);
                if (removedCount > 0)
                    Debug.LogWarning($"<color=yellow>[PrefabAssembler]</color> Removed {removedCount} missing scripts from: {prefab.name}");

                PrefabUtility.SaveAsPrefabAsset(instance, path);
                Debug.Log($"<color=orange>[PrefabAssembler]</color> Repaired links on: {prefab.name}");
            }
            Object.DestroyImmediate(instance);
        }

        private static bool VerifyField<T>(SerializedObject so, string propName, Transform root, string childName) where T : Object
        {
            if (so == null) return false;
            var prop = so.FindProperty(propName);
            if (prop == null) { Debug.LogError($"Property {propName} not found on {so.targetObject.name}"); return false; }
            if (prop.objectReferenceValue != null) return false;

            T found = FindInHierarchy<T>(root, childName);
            if (found != null)
            {
                prop.objectReferenceValue = found;
                return true;
            }
            return false;
        }

        private static Transform EnsureChildFromPrefab(Transform parent, GameObject prefab, string name)
        {
            Transform existing = parent.Find(name);
            if (existing != null) return existing;

            GameObject go = PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
            go.name = name;
            return go.transform;
        }

        private static T FindInHierarchy<T>(Transform root, string exactName) where T : Object
        {
            foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
                if (t.name == exactName)
                {
                    if (typeof(Component).IsAssignableFrom(typeof(T))) return t.GetComponent(typeof(T)) as T;
                    if (typeof(GameObject).IsAssignableFrom(typeof(T))) return t.gameObject as T;
                }
            return null;
        }

        private static GameObject CreateUIObject(string name, Transform parent, System.Type mainType)
        {
            Transform existing = parent.Find(name);
            if (existing != null) return existing.gameObject;

            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.AddComponent(mainType);
            return go;
        }

        private static void RepairData()
        {
            Sprite square   = AssetDatabase.LoadAssetAtPath<Sprite>(SHAPE_ROOT + "Square.png");
            Sprite circle   = AssetDatabase.LoadAssetAtPath<Sprite>(SHAPE_ROOT + "Circle.png");
            Sprite squircle = AssetDatabase.LoadAssetAtPath<Sprite>(SHAPE_ROOT + "Squircle.png");

            foreach (var guid in AssetDatabase.FindAssets("t:CropDataSO"))
            {
                var asset = AssetDatabase.LoadAssetAtPath<CropDataSO>(AssetDatabase.GUIDToAssetPath(guid));
                if (asset.icon == null)     asset.icon = square;
                if (asset.seedIcon == null) asset.seedIcon = squircle;
                EditorUtility.SetDirty(asset);
            }

            foreach (var guid in AssetDatabase.FindAssets("t:AnimalDataSO"))
            {
                var asset = AssetDatabase.LoadAssetAtPath<AnimalDataSO>(AssetDatabase.GUIDToAssetPath(guid));
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
            plant.AddComponent<SpriteRenderer>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(SHAPE_ROOT + "Squircle.png");
            plant.AddComponent<CropTileView>();
            SavePrefab(plant, WORLD_ROOT + "Plant_Placeholder.prefab");

            GameObject animal = new GameObject("Animal_Placeholder");
            animal.AddComponent<SpriteRenderer>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(SHAPE_ROOT + "Circle.png");
            animal.AddComponent<AnimalView>();
            SavePrefab(animal, WORLD_ROOT + "Animal_Placeholder.prefab");
        }

        private static void SavePrefab(GameObject go, string path)
        {
            PrefabUtility.SaveAsPrefabAsset(go, path);
            Object.DestroyImmediate(go);
        }


    }
}
