using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using NTVV.Data.ScriptableObjects;
using NTVV.World.Views;

namespace NTVV.World
{
    [ExecuteInEditMode]
    public class CropGridSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _cropTilePrefab;
        [SerializeField] private int _rows = 2;
        [SerializeField] private int _cols = 3;
        [SerializeField] private float _cellWidth = 1.2f;
        [SerializeField] private float _cellHeight = 0.7f;
        [SerializeField] private GameDataRegistrySO _registry;

        [ContextMenu("Generate Grid")]
        public void GenerateGrid()
        {
            // Destroy all existing children
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _cols; col++)
                {
                    GameObject go = Instantiate(_cropTilePrefab, transform);
                    go.name = $"tile_r{row}_c{col}";
                    go.transform.localPosition = new Vector3(col * _cellWidth, row * _cellHeight, 0f);

#if UNITY_EDITOR
                    CropTileView view = go.GetComponent<CropTileView>();
                    if (view != null && _registry != null)
                    {
                        SerializedObject so = new SerializedObject(view);
                        so.FindProperty("_registry").objectReferenceValue = _registry;
                        so.ApplyModifiedProperties();
                    }
#endif
                }
            }
        }

        [ContextMenu("Clear Grid")]
        public void ClearGrid()
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }
}
