using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NTVV.UI.Panels
{
    /// <summary>
    /// Controller for an individual item entry in the Shop.
    /// Follows strict ui-standardization suffix naming (_Label, _Icon, _Button).
    /// </summary>
    public class ShopEntryController : MonoBehaviour
    {
        [Header("UI Standardized Components")]
        [SerializeField] private TMP_Text _name_Label;
        [SerializeField] private TMP_Text _price_Label;
        [SerializeField] private Image _item_Icon;
        [SerializeField] private Button _buy_Button;

        public delegate void ShopEntryAction(string itemId);
        private string _itemId;
        private ShopEntryAction _onBuyClicked;

        private void Awake()
        {
            // Recursive Auto-Wiring (Tier 1) - Ensures self-healing at runtime
            if (_name_Label == null) _name_Label = FindNamed<TMP_Text>("Name_Label");
            if (_price_Label == null) _price_Label = FindNamed<TMP_Text>("Price_Label");
            if (_item_Icon == null) _item_Icon = FindNamed<Image>("Item_Icon");
            if (_buy_Button == null) _buy_Button = FindNamed<Button>("Buy_Button");

            if (_buy_Button != null)
            {
                _buy_Button.onClick.AddListener(HandleBuyClick);
            }
        }

        public void Initialize(string id, string name, int price, Sprite icon, ShopEntryAction onBuy)
        {
            _itemId = id;
            _onBuyClicked = onBuy;

            if (_name_Label != null) _name_Label.text = name.ToUpper();
            if (_price_Label != null) _price_Label.text = price.ToString();
            if (_item_Icon != null) _item_Icon.sprite = icon;
        }

        private void HandleBuyClick()
        {
            _onBuyClicked?.Invoke(_itemId);
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
