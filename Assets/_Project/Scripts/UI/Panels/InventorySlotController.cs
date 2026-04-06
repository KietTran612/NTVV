using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NTVV.UI.Panels
{
    /// <summary>
    /// Controller for a single inventory slot in the Storage UI.
    /// Follows strict ui-standardization suffix naming (_Label, _Icon, _Button).
    /// </summary>
    public class InventorySlotController : MonoBehaviour
    {
        [Header("UI Standardized Components")]
        [SerializeField] private Image _item_Icon;
        [SerializeField] private TMP_Text _quantity_Label;
        [SerializeField] private Button _sell_Button;

        private string _itemId;
        private System.Action<string> _onSellClicked;

        private void Awake()
        {
            // Recursive Auto-Wiring (Tier 1) - Self-Healing Pattern
            if (_item_Icon == null) _item_Icon = FindNamed<Image>("Item_Icon");
            if (_quantity_Label == null) _quantity_Label = FindNamed<TMP_Text>("Quantity_Label");
            if (_sell_Button == null) _sell_Button = FindNamed<Button>("Sell_Button");

            if (_sell_Button != null)
            {
                _sell_Button.onClick.AddListener(() => _onSellClicked?.Invoke(_itemId));
            }
        }

        public void Initialize(string id, int quantity, Sprite icon, System.Action<string> onSell)
        {
            _itemId = id;
            _onSellClicked = onSell;

            if (_item_Icon != null) _item_Icon.sprite = icon;
            if (_quantity_Label != null) 
            {
                _quantity_Label.text = quantity > 1 ? $"x{quantity}" : "";
                _quantity_Label.enabled = quantity > 0;
            }
        }

        private T FindNamed<T>(string exactName) where T : Component
        {
            foreach (Transform t in GetComponentsInChildren<Transform>(true))
                if (t.name == exactName)
                    return t.GetComponent<T>();
            return null;
        }
    }
}
