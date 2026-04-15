namespace NTVV.UI.Panels
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    /// <summary>
    /// Controller for an individual item entry in the Shop.
    /// Follows strict ui-standardization suffix naming (_Label, _Icon, _Button).
    /// Fixed quantity purchase (X5) as per design requirements.
    /// </summary>
    public class ShopEntryController : MonoBehaviour
    {
        [Header("UI Standardized Components")]
        [SerializeField] private TMP_Text _name_Label;
        [SerializeField] private TMP_Text _price_Label;
        [SerializeField] private Image _item_Icon;
        [SerializeField] private Button _buy_Button;
        [SerializeField] private TMP_Text _qty_Label;
        [SerializeField] private GameObject _multiplier_Badge;
        [SerializeField] private TMP_Text _multiplier_Label;

        private string _itemId;
        private int _unitPrice;
        private int _qty = 5;
        private System.Action<string, int> _onBuyClicked;

        private void Awake()
        {
            // Recursive Auto-Wiring (Tier 1) — ensures self-healing at runtime
            if (_name_Label == null)      _name_Label      = FindNamed<TMP_Text>("Name_Label");
            if (_price_Label == null)     _price_Label     = FindNamed<TMP_Text>("Price_Label");
            if (_item_Icon == null)       _item_Icon       = FindNamed<Image>("Item_Icon");
            if (_buy_Button == null)      _buy_Button      = FindNamed<Button>("Buy_Button");
            if (_qty_Label == null)       _qty_Label       = FindNamed<TMP_Text>("Qty_Label");
            if (_multiplier_Badge == null) _multiplier_Badge = FindNamed<Transform>("Multiplier_Badge")?.gameObject;
            if (_multiplier_Label == null) _multiplier_Label = FindNamed<TMP_Text>("Multiplier_Label");

            _buy_Button?.onClick.AddListener(HandleBuyClick);
        }

        /// <summary>
        /// Initialises this entry. Callback receives (itemId, quantity).
        /// Default quantity is fixed to 5.
        /// </summary>
        public void Initialize(string id, string name, int unitPrice, Sprite icon, System.Action<string, int> onBuy, int multiplier = 1)
        {
            _itemId     = id;
            _unitPrice  = unitPrice;
            _qty        = 5;
            _onBuyClicked = onBuy;

            if (_name_Label != null) _name_Label.text = name.ToUpper();
            if (_item_Icon  != null) _item_Icon.sprite = icon;

            if (_multiplier_Badge != null)
            {
                _multiplier_Badge.SetActive(multiplier > 1);
                if (_multiplier_Label != null) _multiplier_Label.text = $"×{multiplier}";
            }

            RefreshQtyUI();
        }

        private void RefreshQtyUI()
        {
            if (_qty_Label   != null) _qty_Label.text   = $"×{_qty}";
            if (_price_Label != null) _price_Label.text = (_unitPrice * _qty).ToString();
        }

        // ── Buy ──────────────────────────────────────────────────────────────

        private void HandleBuyClick() => _onBuyClicked?.Invoke(_itemId, _qty);

        // ── Helpers ──────────────────────────────────────────────────────────

        private T FindNamed<T>(string exactName) where T : Component
        {
            foreach (Transform t in GetComponentsInChildren<Transform>(true))
                if (t.name == exactName) return t.GetComponent<T>();
            return null;
        }
    }
}
