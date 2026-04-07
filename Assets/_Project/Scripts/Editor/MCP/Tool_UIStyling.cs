#nullable enable
using System.ComponentModel;
using com.IvanMurzak.McpPlugin;
using com.IvanMurzak.ReflectorNet.Utils;
using NTVV.UI.Styling;
using UnityEditor;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    [McpPluginToolType]
    public partial class Tool_UIStyling
    {
        [McpPluginTool("ui-prefab-style", Title = "UI / Prefab Style")]
        [Description("Bakes visual styles from UIStyleData into all UIStyleAppliers in the active scene or currently opened prefab.")]
        public void ApplyStyles(
            [Description("Force apply to target object instead of looking for all. Leave empty to find all in active scene/prefab.")]
            string? targetObjectName = null
        )
        {
            MainThread.Instance.Run(() =>
            {
                UIStyleApplier[] appliers;
                
                if (!string.IsNullOrEmpty(targetObjectName))
                {
                    var go = GameObject.Find(targetObjectName);
                    if (go == null) 
                    {
                        Debug.LogError($"[UIStyling] Target '{targetObjectName}' not found.");
                        return;
                    }
                    appliers = go.GetComponentsInChildren<UIStyleApplier>(true);
                }
                else
                {
#if UNITY_2023_1_OR_NEWER
                    appliers = Object.FindObjectsByType<UIStyleApplier>(FindObjectsInactive.Include, FindObjectsSortMode.None);
#else
                    appliers = Object.FindObjectsOfType<UIStyleApplier>(true);
#endif
                }

                int count = 0;
                foreach (var applier in appliers)
                {
                    // Force the applier to apply layout and styles (uses Reflection to invoke OnValidate since it applies logic there, or just call ApplyStyle manually if we had access to the data).
                    // Actually, UIStyleApplier does validation on script load, but to force it, we can just set dirty.
                    EditorUtility.SetDirty(applier);
                    applier.gameObject.SetActive(!applier.gameObject.activeSelf);
                    applier.gameObject.SetActive(!applier.gameObject.activeSelf);
                    count++;
                }

                Debug.Log($"<color=green>[UIStyling]</color> Triggered style update on {count} appliers.");
            });
        }
    }
}
