namespace NTVV.World.Interactions
{
    using UnityEngine;

    /// <summary>
    /// Script đánh dấu một đối tượng là Cửa hàng để có thể tương tác.
    /// </summary>
    public class ShopTrigger : MonoBehaviour
    {
        [SerializeField] private string _shopId = "MainShop";
        
        public string ShopId => _shopId;

        public void OpenShop()
        {
            if (NTVV.UI.Common.PopupManager.Instance != null)
            {
                NTVV.UI.Common.PopupManager.Instance.ShowScreen("Shop");
            }
        }
    }
}
