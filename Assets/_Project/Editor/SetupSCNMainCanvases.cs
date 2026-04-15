// TEMP SETUP SCRIPT — delete after use
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SetupSCNMainCanvases
{
    [MenuItem("Tools/NTVV/Setup SCN_Main Canvases")]
    public static void Setup()
    {
        // Configure [HUD_CANVAS]
        ConfigureCanvas("[HUD_CANVAS]", 10);
        // Configure [POPUP_CANVAS]
        ConfigureCanvas("[POPUP_CANVAS]", 20);
        // Configure [SYSTEM_CANVAS]
        ConfigureCanvas("[SYSTEM_CANVAS]", 30);

        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("[SetupSCNMainCanvases] All 3 canvases configured.");
    }

    [MenuItem("Tools/NTVV/Setup SCN_Main UI Children")]
    public static void SetupUIChildren()
    {
        // Find [POPUP_CANVAS] by traversing root objects
        var popupCanvas = FindRootByName("[POPUP_CANVAS]");
        if (popupCanvas == null) { Debug.LogError("[SetupSCNMainCanvases] [POPUP_CANVAS] not found"); return; }

        // Process each child separately to avoid reference issues after DestroyImmediate
        // DimOverlay — process first (already has RectTransform + Image from previous run)
        var dimOverlay = popupCanvas.transform.Find("DimOverlay");
        if (dimOverlay != null)
        {
            EnsureRectTransform(dimOverlay.gameObject);
            // Re-find after potential recreation
            dimOverlay = popupCanvas.transform.Find("DimOverlay");
            if (dimOverlay != null)
            {
                var img = dimOverlay.GetComponent<Image>();
                if (img == null) img = dimOverlay.gameObject.AddComponent<Image>();
                img.color = new Color(0f, 0f, 0f, 0.5f);
                SetStretchRect(dimOverlay.GetComponent<RectTransform>(), "DimOverlay");
                dimOverlay.gameObject.SetActive(false);
            }
        }
        else Debug.LogError("[SetupSCNMainCanvases] DimOverlay not found under [POPUP_CANVAS]");

        // ModalParent
        var modalParent = popupCanvas.transform.Find("ModalParent");
        if (modalParent != null)
        {
            EnsureRectTransform(modalParent.gameObject);
            // Re-find after potential recreation
            modalParent = popupCanvas.transform.Find("ModalParent");
            if (modalParent != null)
                SetStretchRect(modalParent.GetComponent<RectTransform>(), "ModalParent");
        }
        else Debug.LogError("[SetupSCNMainCanvases] ModalParent not found under [POPUP_CANVAS]");

        // HUDParent
        var hudParent = popupCanvas.transform.Find("HUDParent");
        if (hudParent != null)
        {
            EnsureRectTransform(hudParent.gameObject);
            // Re-find after potential recreation
            hudParent = popupCanvas.transform.Find("HUDParent");
            if (hudParent != null)
                SetStretchRect(hudParent.GetComponent<RectTransform>(), "HUDParent");
        }
        else Debug.LogError("[SetupSCNMainCanvases] HUDParent not found under [POPUP_CANVAS]");

        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("[SetupSCNMainCanvases] UI children configured.");
    }

    [MenuItem("Tools/NTVV/Wire GameManager DataRegistry")]
    public static void WireGameManagerDataRegistry()
    {
        // Find GameManager GO
        var gameManagerGO = GameObject.Find("[SYSTEMS]/GameManager");
        if (gameManagerGO == null)
        {
            // Try finding by name
            var systems = FindRootByName("[SYSTEMS]");
            if (systems != null)
            {
                var t = systems.transform.Find("GameManager");
                if (t != null) gameManagerGO = t.gameObject;
            }
        }
        if (gameManagerGO == null) { Debug.LogError("[SetupSCNMainCanvases] GameManager not found"); return; }

        // Find GameManager component
        var gmComp = gameManagerGO.GetComponent("GameManager");
        if (gmComp == null) { Debug.LogError("[SetupSCNMainCanvases] GameManager component not found on GameManager GO"); return; }

        // Find GameDataRegistry asset
        var guids = AssetDatabase.FindAssets("t:ScriptableObject GameDataRegistry", new[] { "Assets/_Project" });
        if (guids.Length == 0)
        {
            guids = AssetDatabase.FindAssets("GameDataRegistry", new[] { "Assets/_Project" });
        }
        if (guids.Length == 0) { Debug.LogError("[SetupSCNMainCanvases] GameDataRegistry asset not found"); return; }

        var assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
        var registry = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
        if (registry == null) { Debug.LogError($"[SetupSCNMainCanvases] Could not load asset at {assetPath}"); return; }

        // Use SerializedObject to set the field
        var so = new SerializedObject(gmComp);
        var prop = so.FindProperty("_dataRegistry");
        if (prop == null)
        {
            // Try without underscore
            prop = so.FindProperty("dataRegistry");
        }
        if (prop == null)
        {
            Debug.LogError("[SetupSCNMainCanvases] _dataRegistry field not found on GameManager component. Available fields logged.");
            so.GetIterator();
            var iter = so.GetIterator();
            iter.Next(true);
            while (iter.Next(false))
                Debug.Log($"  Field: {iter.name} ({iter.propertyType})");
            return;
        }

        prop.objectReferenceValue = registry;
        so.ApplyModifiedProperties();
        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log($"[SetupSCNMainCanvases] GameManager._dataRegistry wired to {assetPath}");
    }

    // Unity doesn't auto-convert Transform→RectTransform via MCP parenting.
    // We destroy the old Transform and add RectTransform by re-creating the GO properly.
    static void EnsureRectTransform(GameObject go)
    {
        if (go.GetComponent<RectTransform>() != null) return;
        // RectTransform can't be added via AddComponent if Transform already exists.
        // The correct approach: destroy the GO and recreate it as a UI element.
        var parent = go.transform.parent;
        var name = go.name;
        var siblingIndex = go.transform.GetSiblingIndex();
        Object.DestroyImmediate(go);

        var newGo = new GameObject(name, typeof(RectTransform));
        newGo.transform.SetParent(parent, false);
        newGo.transform.SetSiblingIndex(siblingIndex);
        Debug.Log($"[SetupSCNMainCanvases] Recreated {name} with RectTransform");
    }

    static GameObject FindRootByName(string name)
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        foreach (var root in scene.GetRootGameObjects())
            if (root.name == name) return root;
        return null;
    }

    static void SetStretchRect(RectTransform rt, string label)
    {
        if (rt == null) { Debug.LogError($"[SetupSCNMainCanvases] No RectTransform on {label}"); return; }
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.pivot = new Vector2(0.5f, 0.5f);
        Debug.Log($"[SetupSCNMainCanvases] {label} → stretch-stretch RectTransform");
    }

    static void ConfigureCanvas(string goName, int sortOrder)
    {
        var go = GameObject.Find(goName);
        if (go == null) { Debug.LogError($"[SetupSCNMainCanvases] Could not find {goName}"); return; }

        var canvas = go.GetComponent<Canvas>();
        if (canvas == null) { Debug.LogError($"[SetupSCNMainCanvases] No Canvas on {goName}"); return; }

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = sortOrder;

        var scaler = go.GetComponent<CanvasScaler>();
        if (scaler != null)
        {
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
        }

        Debug.Log($"[SetupSCNMainCanvases] {goName} → renderMode=Overlay, sortOrder={sortOrder}, scaler=1920x1080 match=0.5");
    }
}
