namespace NTVV.World
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    using NTVV.UI.Common;
    using NTVV.World.Views;
    using NTVV.World.Interactions;

    /// <summary>
    /// Handles Raycasting from screen-space (taps/clicks) to world objects.
    /// Uses New Input System.
    /// Updated to support v1 UI flow (PopupManager) and renamed Views.
    /// </summary>
    public class WorldObjectPicker : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private UnityEngine.Camera _mainCamera;
        [SerializeField] private LayerMask _interactableLayer;

        /// <summary>
        /// Triggered by PlayerInput component (New Input System).
        /// </summary>
        public void OnTap(InputValue value)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            HandlePick(screenPos);
        }

        private void HandlePick(Vector2 screenPosition)
        {
            if (_mainCamera == null) return;

            Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, _interactableLayer))
            {
                // Try to find a CropTileView component on the object
                CropTileView tile = hit.collider.GetComponentInParent<CropTileView>();
                if (tile != null)
                {
                    OnTileSelected(tile);
                    return;
                }

                // Try to find an AnimalPenView component
                AnimalPenView pen = hit.collider.GetComponentInParent<AnimalPenView>();
                if (pen != null)
                {
                    OnPenSelected(pen);
                    return;
                }

                // Try to find an AnimalView component
                AnimalView animal = hit.collider.GetComponentInParent<AnimalView>();
                if (animal != null)
                {
                    OnAnimalSelected(animal);
                    return;
                }

                // Try to find a ShopTrigger component
                ShopTrigger shop = hit.collider.GetComponentInParent<ShopTrigger>();
                if (shop != null)
                {
                    OnShopSelected(shop);
                    return;
                }

                // Try to find a QuestGiver component
                QuestGiver giver = hit.collider.GetComponentInParent<QuestGiver>();
                if (giver != null)
                {
                    OnQuestGiverSelected(giver);
                    return;
                }
            }
        }

        private void OnShopSelected(ShopTrigger shop)
        {
            Debug.Log($"<color=cyan>Shop Selected: </color> {shop.name}");
            shop.OpenShop();
        }

        private void OnQuestGiverSelected(QuestGiver giver)
        {
            Debug.Log($"<color=cyan>Quest Giver Selected: </color> {giver.name}");
            giver.Interact();
        }

        private void OnTileSelected(CropTileView tile)
        {
            Debug.Log($"<color=yellow>Tile Selected: </color> {tile.name}");
            if (PopupManager.Instance != null)
            {
                PopupManager.Instance.ShowContextAction(tile);
            }
        }

        private void OnPenSelected(AnimalPenView pen)
        {
            Debug.Log($"<color=yellow>Pen Selected: </color> {pen.name}");
            if (PopupManager.Instance != null)
            {
                PopupManager.Instance.ShowContextAction(pen);
            }
        }

        private void OnAnimalSelected(AnimalView animal)
        {
            Debug.Log($"<color=yellow>Animal Selected: </color> {animal.name}");
            if (PopupManager.Instance != null)
            {
                PopupManager.Instance.ShowContextAction(animal);
            }
        }
    }
}
