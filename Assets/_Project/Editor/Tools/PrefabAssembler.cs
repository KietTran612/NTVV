using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using NTVV.Data.ScriptableObjects;
using NTVV.World.Views;
using NTVV.UI.HUD;
using NTVV.UI.Common;
using NTVV.UI.Styling;
using TMPro;

namespace NTVV.Editor.Tools
{
    public class PrefabAssembler : EditorWindow
    {
        private const string UI_ROOT = "Assets/_Project/Resources/UI/Default/";
        private const string ATOM_ROOT = "Assets/_Project/Prefabs/UI/Components/";
        private const string WORLD_ROOT = "Assets/_Project/Prefabs/World/";
        private const string SHAPE_ROOT = "Assets/_Project/Textures/Shapes/";
        private const string DEFAULT_ICON_PATH = "Assets/_Project/Textures/Icons/Gems/Gem-128.png";

        private static bool _hierarchyChanged = false;

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

        private static void UpgradeAtomicPrefabs()
        {
            // UI_Resource_Chip
            string chipPath = ATOM_ROOT + "UI_Resource_Chip.prefab";
            GameObject chipPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(chipPath);
            if (chipPrefab != null)
            {
                VerifyAndRepair<UIResourceChip>(chipPrefab, chipPath, (instance, so) =>
                {
                    var layout = instance.GetComponent<HorizontalLayoutGroup>() ?? instance.AddComponent<HorizontalLayoutGroup>();
                    layout.padding = new RectOffset(10, 20, 8, 8);
                    layout.spacing = 15;
                    layout.childAlignment = TextAnchor.MiddleLeft;
                    layout.childControlWidth = true;
                    layout.childForceExpandWidth = false;
                    _hierarchyChanged = true;

                    var bgImage = instance.GetComponent<Image>() ?? instance.AddComponent<Image>();
                    if (bgImage.sprite == null || bgImage.sprite.name == "Square") {
                        bgImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(SHAPE_ROOT + "Squircle.png");
                        bgImage.type = Image.Type.Sliced;
                        bgImage.color = new Color(0.15f, 0.08f, 0.02f, 0.8f); // Dark Chocolate Wood
                        _hierarchyChanged = true;
                    }

                    CreateUIObject("Resource_Icon", instance.transform, typeof(Image));
                    CreateUIObject("Amount_Label",  instance.transform, typeof(TextMeshProUGUI), "Value_Label");
                    VerifyField<Image>   (so, "_resource_Icon", instance.transform, "Resource_Icon");
                    VerifyField<TMP_Text>(so, "_amount_Label",   instance.transform, "Amount_Label");
                    
                    EnsureStyleApplier(instance.transform, UIStyleApplier.StyleType.ResourceChip);
                    EnsureStyleApplier(instance.transform.Find("Amount_Label"), UIStyleApplier.StyleType.BodyText);
                });
            }

            // UI_Nav_Button
            string navPath = ATOM_ROOT + "UI_Nav_Button.prefab";
            GameObject navPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(navPath);
            if (navPrefab != null)
            {
                VerifyAndRepair<UINavButton>(navPrefab, navPath, (instance, so) =>
                {
                    if (instance.GetComponent<Button>() == null) {
                        instance.AddComponent<Button>();
                        _hierarchyChanged = true;
                    }
                    CreateUIObject("Nav_Icon",  instance.transform, typeof(Image));
                    CreateUIObject("Nav_Label", instance.transform, typeof(TextMeshProUGUI));
                    VerifyField<Image>   (so, "_nav_Icon",   instance.transform, "Nav_Icon");
                    VerifyField<TMP_Text>(so, "_nav_Label",  instance.transform, "Nav_Label");
                    if (so.FindProperty("_nav_Button").objectReferenceValue == null)
                         so.FindProperty("_nav_Button").objectReferenceValue = instance.GetComponent<Button>();

                    EnsureStyleApplier(instance.transform, UIStyleApplier.StyleType.PrimaryAction);
                    EnsureStyleApplier(instance.transform.Find("Nav_Label"), UIStyleApplier.StyleType.BodyText);
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
                    CreateUIObject("Level_Label", instance.transform, typeof(TextMeshProUGUI));
                    VerifyField<Image>(so, "_xp_Fill", instance.transform, "XP_Fill");

                    EnsureStyleApplier(instance.transform.Find("XP_Fill"), UIStyleApplier.StyleType.CaringAction);
                    EnsureStyleApplier(instance.transform.Find("Level_Label"), UIStyleApplier.StyleType.BodyText);
                });
            }
        }

        private static void AssembleShopItems()
        {
            string path = ATOM_ROOT + "ShopEntry_Seed.prefab";
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing == null) {
                GameObject go = new GameObject("ShopEntry_Seed");
                go.AddComponent<RectTransform>().sizeDelta = new Vector2(320, 480);
                go.AddComponent<NTVV.UI.Panels.ShopEntryController>();
                CreateUIObject("Item_Icon",   go.transform, typeof(Image));
                CreateUIObject("Name_Label",  go.transform, typeof(TextMeshProUGUI));
                var priceRow = CreateUIObject("Price_Row", go.transform, typeof(HorizontalLayoutGroup));
                CreateUIObject("Currency_Icon", priceRow.transform, typeof(Image));
                CreateUIObject("Price_Label",   priceRow.transform, typeof(TextMeshProUGUI));
                CreateUIObject("Buy_Button", go.transform, typeof(Button));
                existing = PrefabUtility.SaveAsPrefabAsset(go, path);
                Object.DestroyImmediate(go);
            }
            VerifyAndRepair<NTVV.UI.Panels.ShopEntryController>(existing, path, (instance, so) => {
                VerifyField<TMP_Text>(so, "_name_Label",  instance.transform, "Name_Label");
                VerifyField<TMP_Text>(so, "_price_Label", instance.transform, "Price_Label");
                VerifyField<Image>   (so, "_item_Icon",   instance.transform, "Item_Icon");
                VerifyField<Button>  (so, "_buy_Button",  instance.transform, "Buy_Button");

                EnsureStyleApplier(instance.transform.Find("Name_Label"), UIStyleApplier.StyleType.BodyText);
                EnsureStyleApplier(instance.transform.Find("Price_Label"), UIStyleApplier.StyleType.BodyText);
                EnsureStyleApplier(instance.transform.Find("Buy_Button"), UIStyleApplier.StyleType.PrimaryAction);
            });
        }

        private static void AssembleInventorySlots()
        {
            string path = ATOM_ROOT + "InventorySlot.prefab";
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing == null) {
                GameObject go = new GameObject("InventorySlot");
                go.AddComponent<RectTransform>().sizeDelta = new Vector2(200, 220);
                go.AddComponent<NTVV.UI.Panels.InventorySlotController>();
                CreateUIObject("Item_Icon",      go.transform, typeof(Image));
                CreateUIObject("Quantity_Label", go.transform, typeof(TextMeshProUGUI));
                CreateUIObject("Sell_Button",    go.transform, typeof(Button));
                existing = PrefabUtility.SaveAsPrefabAsset(go, path);
                Object.DestroyImmediate(go);
            }
            VerifyAndRepair<NTVV.UI.Panels.InventorySlotController>(existing, path, (instance, so) => {
                VerifyField<Image>   (so, "_item_Icon",      instance.transform, "Item_Icon");
                VerifyField<TMP_Text>(so, "_quantity_Label", instance.transform, "Quantity_Label");
                VerifyField<Button>  (so, "_sell_Button",    instance.transform, "Sell_Button");

                EnsureStyleApplier(instance.transform.Find("Quantity_Label"), UIStyleApplier.StyleType.BodyText);
                EnsureStyleApplier(instance.transform.Find("Sell_Button"),     UIStyleApplier.StyleType.PrimaryAction);
            });
        }

        private static void AssembleQuestItems()
        {
            string path = ATOM_ROOT + "QuestListItem.prefab";
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing == null) {
                GameObject go = new GameObject("QuestListItem");
                go.AddComponent<RectTransform>().sizeDelta = new Vector2(800, 150);
                go.AddComponent<NTVV.UI.Items.QuestListItem>();
                CreateUIObject("Quest_Icon",    go.transform, typeof(Image));
                CreateUIObject("Name_Label",    go.transform, typeof(TextMeshProUGUI));
                CreateUIObject("Desc_Label",    go.transform, typeof(TextMeshProUGUI));
                CreateUIObject("Progress_Label", go.transform, typeof(TextMeshProUGUI));
                CreateUIObject("Reward_Label",   go.transform, typeof(TextMeshProUGUI));
                CreateUIObject("Claim_Button",  go.transform, typeof(Button));
                existing = PrefabUtility.SaveAsPrefabAsset(go, path);
                Object.DestroyImmediate(go);
            }
            VerifyAndRepair<NTVV.UI.Items.QuestListItem>(existing, path, (instance, so) => {
                VerifyField<TMP_Text>(so, "_questNameLabel", instance.transform, "Name_Label");
                VerifyField<TMP_Text>(so, "_questDescLabel", instance.transform, "Desc_Label");
                VerifyField<TMP_Text>(so, "_progressLabel",  instance.transform, "Progress_Label");
                VerifyField<Image>   (so, "_questIcon",      instance.transform, "Quest_Icon");
                VerifyField<Button>  (so, "_btnClaim",       instance.transform, "Claim_Button");
                VerifyField<TMP_Text>(so, "_rewardLabel",    instance.transform, "Reward_Label");

                EnsureStyleApplier(instance.transform.Find("Name_Label"),   UIStyleApplier.StyleType.BodyText);
                EnsureStyleApplier(instance.transform.Find("Desc_Label"),   UIStyleApplier.StyleType.BodyText);
                EnsureStyleApplier(instance.transform.Find("Claim_Button"), UIStyleApplier.StyleType.CaringAction);
            });
        }

        private static void AssembleHUD()
        {
            string path = UI_ROOT + "HUDTopBar.prefab";
            GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing == null) return;
            VerifyAndRepair<HUDTopBarController>(existing, path, (instance, so) => {
                var layout = instance.GetComponent<HorizontalLayoutGroup>() ?? instance.AddComponent<HorizontalLayoutGroup>();
                layout.childControlWidth = true;
                layout.childForceExpandWidth = false;
                layout.spacing = 20;
                GameObject chipPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ATOM_ROOT + "UI_Resource_Chip.prefab");
                GameObject xpPrefab   = AssetDatabase.LoadAssetAtPath<GameObject>(ATOM_ROOT + "UI_XP_ProgressBar.prefab");
                if (chipPrefab != null) {
                    EnsureChildFromPrefab(instance.transform, chipPrefab, "Gold_Chip");
                    EnsureChildFromPrefab(instance.transform, chipPrefab, "Storage_Chip");
                }
                if (xpPrefab != null) {
                    EnsureChildFromPrefab(instance.transform, xpPrefab, "XP_Bar");
                }
            });
        }

        private static void AssembleBottomNav()
        {
            string path = UI_ROOT + "BottomNav.prefab";
            GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing == null) return;
            VerifyAndRepair<MonoBehaviour>(existing, path, (instance, so) => {
                var layout = instance.GetComponent<HorizontalLayoutGroup>() ?? instance.AddComponent<HorizontalLayoutGroup>();
                layout.childAlignment = TextAnchor.MiddleCenter;
                layout.spacing = 40;
                GameObject btnPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ATOM_ROOT + "UI_Nav_Button.prefab");
                if (btnPrefab != null) {
                    string[] labels = { "Farm", "Storage", "Shop", "Quest", "Event" };
                    foreach (var label in labels) {
                        EnsureChildFromPrefab(instance.transform, btnPrefab, "Btn_" + label);
                    }
                }
            });
        }

        private static void AssembleShopPopup()
        {
            string path = UI_ROOT + "ShopPopup.prefab";
            GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing == null) return;
            VerifyAndRepair<NTVV.UI.Panels.ShopPanelController>(existing, path, (instance, so) => {
                VerifyField<TMP_Text> (so, "_goldBalanceLabel",   instance.transform, "GoldBalance_Label");
                VerifyField<Transform>(so, "_shopContentContainer", instance.transform, "Items_Content");
                VerifyField<Button>   (so, "_btnClose",           instance.transform, "Close_Button");
                
                EnsureStyleApplier(instance.transform.Find("GoldBalance_Label"), UIStyleApplier.StyleType.Header);
                EnsureStyleApplier(instance.transform.Find("Close_Button"),      UIStyleApplier.StyleType.Warning);
                EnsureStyleApplier(instance.transform.Find("Tab_Seeds"),        UIStyleApplier.StyleType.PrimaryAction);
                EnsureStyleApplier(instance.transform.Find("Tab_Animals"),      UIStyleApplier.StyleType.CaringAction);
            });
        }

        private static void AssembleStoragePopup()
        {
            string path = UI_ROOT + "StoragePopup.prefab";
            GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing == null) return;
            VerifyAndRepair<NTVV.UI.Panels.StoragePanelController>(existing, path, (instance, so) => {
                VerifyField<TMP_Text> (so, "_capacityText",      instance.transform, "Capacity_Label");
                VerifyField<Button>   (so, "_btnClose",          instance.transform, "Close_Button");
                VerifyField<Button>   (so, "_btnUpgrade",        instance.transform, "Upgrade_Button");

                EnsureStyleApplier(instance.transform.Find("Capacity_Label"), UIStyleApplier.StyleType.Header);
                EnsureStyleApplier(instance.transform.Find("Upgrade_Button"), UIStyleApplier.StyleType.CaringAction);
                EnsureStyleApplier(instance.transform.Find("Close_Button"),    UIStyleApplier.StyleType.Warning);
                EnsureStyleApplier(instance.transform.Find("Sell_Button"),     UIStyleApplier.StyleType.PrimaryAction);
            });
        }

        private static void AssembleQuestPopup()
        {
            string path = UI_ROOT + "QuestPopup.prefab";
            GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing == null) return;
            VerifyAndRepair<NTVV.UI.Panels.QuestPanelController>(existing, path, (instance, so) => {
                VerifyField<Button>   (so, "_btnClose",           instance.transform, "Close_Button");
                EnsureStyleApplier(instance.transform.Find("Empty_Message"), UIStyleApplier.StyleType.BodyText);
                EnsureStyleApplier(instance.transform.Find("Close_Button"),  UIStyleApplier.StyleType.Warning);
            });
        }

        private static void VerifyAndRepair<T>(GameObject prefab, string path, System.Action<GameObject, SerializedObject> action) where T : MonoBehaviour
        {
            if (prefab == null) return;
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            T ctrl = instance.GetComponent<T>();
            if (ctrl == null && typeof(T) != typeof(MonoBehaviour)) {
                ctrl = instance.AddComponent<T>();
                _hierarchyChanged = true;
            }
            SerializedObject so = ctrl != null ? new SerializedObject(ctrl) : null;

            _hierarchyChanged = false; 
            action(instance, so);

            bool dataChanged = (so != null && so.ApplyModifiedProperties());
            
            if (_hierarchyChanged || dataChanged) {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(instance);
                PrefabUtility.SaveAsPrefabAsset(instance, path);
                Debug.Log($"<color=orange>[Builder]</color> Saved structural changes to: {prefab.name} (H: {_hierarchyChanged}, D: {dataChanged})");
            }
            Object.DestroyImmediate(instance);
        }

        private static bool VerifyField<T>(SerializedObject so, string propName, Transform root, string childName) where T : Object
        {
            if (so == null) return false;
            var prop = so.FindProperty(propName);
            if (prop == null) return false;
            if (prop.objectReferenceValue != null) return false;
            T found = FindInHierarchy<T>(root, childName);
            if (found != null) { prop.objectReferenceValue = found; return true; }
            return false;
        }

        private static Transform EnsureChildFromPrefab(Transform parent, GameObject prefab, string name)
        {
            Transform existing = parent.Find(name);
            if (existing != null) return existing;
            GameObject go = PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
            go.name = name;
            _hierarchyChanged = true;
            return go.transform;
        }

        private static T FindInHierarchy<T>(Transform root, string exactName) where T : Object
        {
            foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
                if (t.name == exactName) {
                    if (typeof(Component).IsAssignableFrom(typeof(T))) return t.GetComponent(typeof(T)) as T;
                    if (typeof(GameObject).IsAssignableFrom(typeof(T))) return t.gameObject as T;
                }
            return null;
        }

        private static GameObject CreateUIObject(string name, Transform parent, System.Type mainType, string legacyName = null)
        {
            Transform target = parent.Find(name);

            if (target == null && !string.IsNullOrEmpty(legacyName)) {
                target = parent.Find(legacyName);
                if (target != null) {
                    target.name = name;
                    _hierarchyChanged = true;
                }
            }

            if (target == null) {
                GameObject go = new GameObject(name);
                go.transform.SetParent(parent, false);
                target = go.transform;
                _hierarchyChanged = true;
            }

            if (typeof(Graphic).IsAssignableFrom(mainType) || mainType.Name.Contains("TextMeshPro"))
            {
                if (target.GetComponent<CanvasRenderer>() == null) {
                    target.gameObject.AddComponent<CanvasRenderer>();
                    _hierarchyChanged = true;
                }
            }

            var component = target.GetComponent(mainType);
            if (component == null) {
                component = target.gameObject.AddComponent(mainType);
                _hierarchyChanged = true;
            }

            ApplyPlaceholderDefaults(target, component);
            AutoAssignStyle(target);

            return target.gameObject;
        }

        private static void AutoAssignStyle(Transform target)
        {
            string n = target.name.ToLower();
            if (n.Contains("header") || n.Contains("title"))
                EnsureStyleApplier(target, UIStyleApplier.StyleType.Header);
            else if (n.Contains("label") || n.Contains("text") || n.Contains("desc"))
                EnsureStyleApplier(target, UIStyleApplier.StyleType.BodyText);
            else if (n.Contains("button") || n.Contains("btn"))
                EnsureStyleApplier(target, UIStyleApplier.StyleType.PrimaryAction);
            else if (n.Contains("close"))
                EnsureStyleApplier(target, UIStyleApplier.StyleType.Warning);
        }

        private static void EnsureStyleApplier(Transform target, UIStyleApplier.StyleType type)
        {
            if (target == null) return;
            var applier = target.GetComponent<UIStyleApplier>();
            if (applier == null) {
                applier = target.gameObject.AddComponent<UIStyleApplier>();
                _hierarchyChanged = true;
            }

            SerializedObject so = new SerializedObject(applier);
            var prop = so.FindProperty("_styleType");
            if (prop.enumValueIndex != (int)type) {
                prop.enumValueIndex = (int)type;
                so.ApplyModifiedProperties();
                _hierarchyChanged = true;
            }
        }

        private static void ApplyPlaceholderDefaults(Transform t, Component comp)
        {
            if (comp is Image img) {
                if (t.name.ToLower().Contains("icon")) {
                    if (img.sprite == null) {
                        img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DEFAULT_ICON_PATH);
                        img.color = Color.white;
                        _hierarchyChanged = true;
                    }
                    var le = t.GetComponent<LayoutElement>();
                    if (le == null) {
                        le = t.gameObject.AddComponent<LayoutElement>();
                        le.preferredWidth = 40;
                        le.preferredHeight = 40;
                        le.minWidth = 32;
                        le.minHeight = 32;
                        _hierarchyChanged = true;
                    }
                } else {
                    if (img.sprite == null && img.color == Color.white) {
                        img.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
                        _hierarchyChanged = true;
                    }
                }
            }
            if (comp is TextMeshProUGUI tmp && string.IsNullOrEmpty(tmp.text)) {
                tmp.text = "No Text";
                _hierarchyChanged = true;
            }
        }

        private static void RepairData()
        {
            Sprite square   = AssetDatabase.LoadAssetAtPath<Sprite>(SHAPE_ROOT + "Square.png");
            Sprite circle   = AssetDatabase.LoadAssetAtPath<Sprite>(SHAPE_ROOT + "Circle.png");
            Sprite squircle = AssetDatabase.LoadAssetAtPath<Sprite>(SHAPE_ROOT + "Squircle.png");
            foreach (var guid in AssetDatabase.FindAssets("t:CropDataSO")) {
                var asset = AssetDatabase.LoadAssetAtPath<CropDataSO>(AssetDatabase.GUIDToAssetPath(guid));
                if (asset.icon == null) asset.icon = square;
                if (asset.seedIcon == null) asset.seedIcon = squircle;
                EditorUtility.SetDirty(asset);
            }
            foreach (var guid in AssetDatabase.FindAssets("t:AnimalDataSO")) {
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
