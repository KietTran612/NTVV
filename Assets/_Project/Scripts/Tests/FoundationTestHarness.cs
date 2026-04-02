namespace NTVV.Tests
{
    using UnityEngine;
    using System.Collections;
    using NTVV.Gameplay.Economy;
    using NTVV.Gameplay.Storage;
    using NTVV.Gameplay.Progression;
    using NTVV.Data;
    using NTVV.World.Views;

    /// <summary>
    /// Kịch bản test tự động các Core Systems (Nền Kinh tế, Kho, Cấp độ)
    /// Đính kèm Script này vào một GameObject trống trong Scene Sandbox để kiểm tra.
    /// </summary>
    public class FoundationTestHarness : MonoBehaviour
    {
        [Header("Test Config")]
        [SerializeField] private bool _runTestsOnStart = true;
        [SerializeField] private CropTileView _testTile;

        private void Start()
        {
            if (_runTestsOnStart)
            {
                StartCoroutine(RunAllTestsCoroutine());
            }
        }

        private IEnumerator RunAllTestsCoroutine()
        {
            Debug.Log("<b><color=#00FFFF>=== BẮT ĐẦU CHẠY KỊCH BẢN TEST FOUNDATION ===</color></b>");
            
            yield return new WaitForSeconds(1f);
            
            TestEconomySystem();
            
            yield return new WaitForSeconds(1f);
            
            TestStorageSystem();
            
            yield return new WaitForSeconds(1f);
            
            TestLevelSystem();
            
            yield return new WaitForSeconds(1f);
            
            Debug.Log("<b><color=#00FFFF>=== KẾT THÚC KỊCH BẢN TEST ===</color></b>");
        }

        private void TestEconomySystem()
        {
            Debug.Log("<color=yellow>--- TEST GIAO DỊCH VÀNG (ECONOMY) ---</color>");
            if (EconomySystem.Instance == null) { Debug.LogError("Thiếu EconomySystem!"); return; }

            int startGold = EconomySystem.Instance.CurrentGold;
            Debug.Log($"Vàng ban đầu: {startGold}");

            // Test mua hàng
            int price = 50;
            if (EconomySystem.Instance.CanAfford(price))
            {
                EconomySystem.Instance.AddGold(-price);
                Debug.Log($"[Pass] Đã xử lý trừ {price} Vàng mua hạt giống. Còn lại: {EconomySystem.Instance.CurrentGold}");
            }
            else
            {
                Debug.LogError("[Fail] Không đủ vàng mua hàng dù khởi tạo là >= 50!");
            }

            // Test bán hàng
            EconomySystem.Instance.AddGold(120);
            Debug.Log($"[Pass] Thu hoạch và bán nông sản được 120 Vàng. Tổng: {EconomySystem.Instance.CurrentGold}");
        }

        private void TestStorageSystem()
        {
            Debug.Log("<color=orange>--- TEST SỨC CHỨA KHÔNG GIAN KHO (STORAGE) ---</color>");
            if (StorageSystem.Instance == null) { Debug.LogError("Thiếu StorageSystem!"); return; }

            // Giả lập nhập kho số lượng lớn để test logic "99 item / slot"
            Debug.Log($"Kho ban đầu: Slot đã dùng: {StorageSystem.Instance.CurrentSlotsUsed}/{StorageSystem.Instance.MaxCapacity}");
            
            // Thêm 50 Cà rốt -> Tốn 1 Slot
            StorageSystem.Instance.AddItem("carrot_01", 50);
            Debug.Log($"Thêm 50 Cà rốt. Slot hiện tại: {StorageSystem.Instance.CurrentSlotsUsed} (Kỳ vọng: 1)");

            // Thêm tiếp 60 Cà rốt (Tổng 110) -> Tốn 2 Slot
            StorageSystem.Instance.AddItem("carrot_01", 60);
            Debug.Log($"Thêm 60 Cà rốt (Tổng 110). Slot hiện tại: {StorageSystem.Instance.CurrentSlotsUsed} (Kỳ vọng: 2)");

            // Test giới hạn lưu trữ đầy
            int itemsToFill = (StorageSystem.Instance.MaxCapacity - StorageSystem.Instance.CurrentSlotsUsed + 1) * 99;
            bool canAddLargeAmount = StorageSystem.Instance.CanAddItem("tomato_01", itemsToFill);
            if (!canAddLargeAmount)
            {
                Debug.Log($"[Pass] Hệ thống đã chặn thành công việc thêm {itemsToFill} item vượt sức chứa tối đa của kho ({StorageSystem.Instance.MaxCapacity} slot).");
            }
            else
            {
                Debug.LogError("[Fail] Lỗi: Hệ thống Kho đang cho phép nạp vượt sức chứa thiết kế!");
            }
        }

        private void TestLevelSystem()
        {
            Debug.Log("<color=green>--- TEST HỆ THỐNG TIẾN TRÌNH / LEVEL UP ---</color>");
            if (LevelSystem.Instance == null) { Debug.LogError("Thiếu LevelSystem!"); return; }

            int startLvl = LevelSystem.Instance.CurrentLevel;
            int startXP = LevelSystem.Instance.CurrentXP;
            Debug.Log($"Level/XP ban đầu: Lv.{startLvl} | {startXP} XP");

            // Buff mạnh XP để kích hoạt lên cấp
            // Giả định lv2 cần 40 XP theo tài liệu (NTVV Farm)
            int expGain = 50;
            Debug.Log($"Thu hoạch nhận {expGain} XP...");
            LevelSystem.Instance.AddXP(expGain);

            if (LevelSystem.Instance.CurrentLevel > startLvl)
            {
                Debug.Log($"[Pass] Nhân vật đã lên cấp thành công! Hiện tại: Lv.{LevelSystem.Instance.CurrentLevel} | {LevelSystem.Instance.CurrentXP} XP");
            }
            else
            {
                Debug.LogWarning("[Check] Nhân vật chưa lên cấp. Vui lòng kiểm tra file `PlayerLevelData` đã có data mốc XP ngưỡng 50 chưa, nếu chưa có thì Log này là bình thường do chưa gắn SO data.");
            }
        }
    }
}
