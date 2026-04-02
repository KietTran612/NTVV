namespace NTVV.Editor
{
    using UnityEngine;
    using UnityEditor;
    using NTVV.Testing;

    /// <summary>
    /// Editor tool to quickly spawn a PersistenceTestHarness object in the active scene.
    /// Access via: NTVV -> Setup -> Create Persistence Test Object.
    /// </summary>
    public class PersistenceTestEditor : EditorWindow
    {
        [MenuItem("NTVV/Setup/Create Persistence Test Object")]
        public static void CreateTestObject()
        {
            // Check for existing harness
            GameObject testObj = GameObject.Find("PersistenceTest_Harness");
            if (testObj != null)
            {
                Selection.activeGameObject = testObj;
                Debug.Log("<color=yellow>[NTVV Editor]</color> PersistenceTest_Harness already exists in the scene. Selected it.");
                return;
            }

            // Create new harness
            testObj = new GameObject("PersistenceTest_Harness");
            testObj.AddComponent<PersistenceTestHarness>();
            
            // Register undo and select
            Undo.RegisterCreatedObjectUndo(testObj, "Create Persistence Test Object");
            Selection.activeGameObject = testObj;
            
            Debug.Log("<color=green>[NTVV Editor]</color> Successfully created PersistenceTest_Harness in the active scene.");
        }
    }
}
