using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.IO;

[InitializeOnLoad]
public class URP2DInitializer
{
    private const string SettingsPath = "Assets/_Project/Settings/Rendering";
    private const string AssetName = "URP2DAsset";
    private const string RendererName = "URP2DRendererData";

    static URP2DInitializer()
    {
        // Delay execution until Unity is fully loaded
        EditorApplication.delayCall += Initialize;
    }

    [MenuItem("Tools/Setup URP 2D")]
    public static void Initialize()
    {
        if (!Directory.Exists(SettingsPath))
        {
            Directory.CreateDirectory(SettingsPath);
        }

        string rendererPath = Path.Combine(SettingsPath, RendererName + ".asset");
        string assetPath = Path.Combine(SettingsPath, AssetName + ".asset");

        Renderer2DData rendererData = AssetDatabase.LoadAssetAtPath<Renderer2DData>(rendererPath);
        if (rendererData == null)
        {
            rendererData = ScriptableObject.CreateInstance<Renderer2DData>();
            AssetDatabase.CreateAsset(rendererData, rendererPath);
            Debug.Log($"Created Renderer 2D Data at {rendererPath}");
        }

        UniversalRenderPipelineAsset urpAsset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(assetPath);
        if (urpAsset == null)
        {
            // For Unity 6, creating a URP asset with a 2D renderer needs specific handling
            urpAsset = UniversalRenderPipelineAsset.Create(rendererData);
            AssetDatabase.CreateAsset(urpAsset, assetPath);
            Debug.Log($"Created URP Asset at {assetPath}");
        }

        // Assign to Graphics Settings
        if (GraphicsSettings.defaultRenderPipeline != urpAsset)
        {
            GraphicsSettings.defaultRenderPipeline = urpAsset;
            Debug.Log("Assigned URP Asset to Graphics Settings.");
        }

        // Assign to Quality Settings
        if (QualitySettings.renderPipeline != urpAsset)
        {
            QualitySettings.renderPipeline = urpAsset;
            Debug.Log("Assigned URP Asset to Quality Settings.");
        }

        AssetDatabase.SaveAssets();
    }
}
