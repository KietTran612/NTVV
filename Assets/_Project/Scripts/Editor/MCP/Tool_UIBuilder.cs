#nullable enable
using System.ComponentModel;
using com.IvanMurzak.McpPlugin;
using com.IvanMurzak.ReflectorNet.Utils;
using NTVV.Editor.Tools;
using UnityEditor;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    [McpPluginToolType]
    public partial class Tool_UIBuilder
    {
        [McpPluginTool("ui-prefab-assemble", Title = "UI / Prefab Assemble")]
        [Description("Assembles all UI prefabs by running PrefabAssembler.AssembleAll(). Fixes missing references and auto-wires structure.")]
        public void AssembleAll()
        {
            MainThread.Instance.Run(() =>
            {
                PrefabAssembler.AssembleAll();
                EditorUtility.DisplayDialog("Done", "Assemble complete", "OK");
            });
        }
    }
}
