# farm_game_unity_folder_structure_naming_scripts_v1

_Source:_ `farm_game_unity_folder_structure_naming_scripts_v1.docx`

Farm Game - Unity Folder Structure, Naming Convention & Script File List

Production reference v1 • phase đầu • landscape 1920×1080 • one-scene gameplay architecture

| Mục tiêu tài liệu<br>Thống nhất cấu trúc project Unity, quy ước đặt tên và danh sách script khởi đầu để team dev vào project sạch, ít lệch, dễ mở rộng.<br>Tài liệu này bám theo các chốt hiện tại của dự án: 1 gameplay scene chính, world 2.5D isometric giả 3D bằng asset 2D, UI 2D, interaction theo từng object + popup/panel theo ngữ cảnh. |
| --- |

1. Scope & nguyên tắc chung

- Game được triển khai theo hướng landscape-first với base resolution 1920×1080.

- Phase đầu chỉ có một gameplay scene chính: MainFarmScene. Các thao tác như chọn hạt giống, chăm cây, xem vật nuôi, kho, bán hàng và event đều mở bằng popup/panel UI 2D.

- Tên folder, prefab, scene, ScriptableObject và script phải đủ rõ để dev nhìn tên là đoán đúng vai trò. Ưu tiên nhất quán hơn là ngắn quá mức.

- Không dùng tên mơ hồ như Temp, Manager2, TestFinal, NewScript hoặc UIThing.

2. Folder structure đề xuất

Cấu trúc dưới đây ưu tiên: dễ tìm file, dễ handoff, tách rõ world / UI / data / systems, và thuận tiện cho việc tăng content về sau.

Folder tree đề xuất

| Assets/<br>├─ _Project/<br>│  ├─ Art/<br>│  │  ├─ UI/<br>│  │  │  ├─ Atlas/<br>│  │  │  ├─ Icons/<br>│  │  │  ├─ Panels/<br>│  │  │  ├─ Buttons/<br>│  │  │  ├─ HUD/<br>│  │  │  └─ Popups/<br>│  │  ├─ World/<br>│  │  │  ├─ Terrain/<br>│  │  │  ├─ Crops/<br>│  │  │  ├─ Animals/<br>│  │  │  ├─ Buildings/<br>│  │  │  ├─ Decorations/<br>│  │  │  ├─ VFX/<br>│  │  │  └─ Shadows/<br>│  │  ├─ Fonts/<br>│  │  └─ Marketing/<br>│  ├─ Audio/<br>│  │  ├─ BGM/<br>│  │  ├─ SFX/<br>│  │  └─ UI/<br>│  ├─ Data/<br>│  │  ├─ ScriptableObjects/<br>│  │  │  ├─ Crops/<br>│  │  │  ├─ Animals/<br>│  │  │  ├─ Items/<br>│  │  │  ├─ Levels/<br>│  │  │  ├─ Economy/<br>│  │  │  └─ Events/<br>│  │  ├─ Tables/<br>│  │  └─ Config/<br>│  ├─ Materials/<br>│  ├─ Prefabs/<br>│  │  ├─ World/<br>│  │  │  ├─ Crops/<br>│  │  │  ├─ Animals/<br>│  │  │  ├─ Buildings/<br>│  │  │  ├─ Decorations/<br>│  │  │  └─ Interaction/<br>│  │  ├─ UI/<br>│  │  │  ├─ Common/<br>│  │  │  ├─ HUD/<br>│  │  │  ├─ Panels/<br>│  │  │  ├─ Popups/<br>│  │  │  └─ Screens/<br>│  │  └─ FX/<br>│  ├─ Scenes/<br>│  │  ├─ Boot/<br>│  │  ├─ Gameplay/<br>│  │  └─ Sandbox/<br>│  ├─ Scripts/<br>│  │  ├─ Core/<br>│  │  ├─ Data/<br>│  │  ├─ Gameplay/<br>│  │  │  ├─ Crops/<br>│  │  │  ├─ Animals/<br>│  │  │  ├─ Storage/<br>│  │  │  ├─ Economy/<br>│  │  │  ├─ Progression/<br>│  │  │  └─ Events/<br>│  │  ├─ World/<br>│  │  │  ├─ Camera/<br>│  │  │  ├─ Interaction/<br>│  │  │  └─ Views/<br>│  │  ├─ UI/<br>│  │  │  ├─ HUD/<br>│  │  │  ├─ Panels/<br>│  │  │  ├─ Popups/<br>│  │  │  ├─ Screens/<br>│  │  │  └─ Common/<br>│  │  ├─ Managers/<br>│  │  └─ Utilities/<br>│  ├─ Settings/<br>│  ├─ ThirdParty/<br>│  └─ Tests/<br>└─ AddressableAssetsData/    (nếu dùng Addressables sau này) |
| --- |

| Ghi chú cấu trúc<br>Dùng một root riêng như Assets/_Project để gom toàn bộ tài nguyên dự án, tránh lẫn với package hoặc third-party.<br>Art và Prefabs tách riêng để artist tìm asset gốc nhanh, dev tìm prefab nhanh.<br>Scripts tách theo domain thay vì dồn toàn bộ vào một folder. Phase đầu tuy nhỏ nhưng nên chia ngay từ đầu để tránh project rối khi thêm crop/animal/event. |
| --- |

3. Naming convention

3.1 Folder & file chung

- Folder dùng PascalCase hoặc TitleCase không dấu, không khoảng trắng. Ví dụ: Scripts, Gameplay, CropData.

- Không dùng khoảng trắng trong tên file hoặc folder.

- Tên file script phải trùng 100% với tên class public bên trong.

- Tên asset nên có prefix chức năng khi điều đó giúp tìm nhanh hơn trong Project Search.

3.2 Scene naming

| Loại | Ví dụ | Ghi chú |
| --- | --- | --- |
| Boot Scene | SCN_Boot | Load config, preload nhẹ, điều hướng vào gameplay |
| Main Gameplay | SCN_MainFarm | Scene chính của phase đầu |
| Sandbox / Test | SCN_Sandbox_Crops | Scene test nội bộ, không ship |

3.3 Prefab naming

| Nhóm | Pattern | Ví dụ |
| --- | --- | --- |
| UI | PFB_UI_[Name] | PFB_UI_PrimaryButton |
| HUD | PFB_HUD_[Name] | PFB_HUD_TopBar |
| Popup | PFB_POP_[Name] | PFB_POP_StorageFull |
| World crop | PFB_CROP_[Name] | PFB_CROP_Tile |
| World animal | PFB_ANM_[Name] | PFB_ANM_ChickenPen |
| Building | PFB_BLD_[Name] | PFB_BLD_StorageHouse |
| FX | PFB_FX_[Name] | PFB_FX_HarvestBurst |

3.4 ScriptableObject naming

| Loại data | Pattern | Ví dụ |
| --- | --- | --- |
| Crop data | SO_Crop_[Name] | SO_Crop_Carrot |
| Animal data | SO_Animal_[Name] | SO_Animal_Chicken |
| Item data | SO_Item_[Name] | SO_Item_GrassBundle |
| Level data | SO_Level_[Name or Id] | SO_Level_04 |
| Economy config | SO_Config_[Name] | SO_Config_Economy |

3.5 Script/class naming

- System-level class dùng hậu tố rõ nghĩa: CropSystem, AnimalSystem, StorageSystem, EconomySystem.

- UI controller dùng hậu tố Controller hoặc Presenter tùy team chọn, nhưng phải nhất quán. Ví dụ: SeedShopPanelController, HUDTopBarController.

- View class cho object world/UI hiển thị dữ liệu dùng hậu tố View. Ví dụ: CropTileView, AnimalPenView, StorageSlotView.

- Popup class dùng hậu tố Popup. Ví dụ: StorageFullPopup, LevelUpPopup.

- Config/data helper dùng hậu tố Config, Data, State hoặc Model theo đúng vai trò.

4. Script file list khởi đầu

Danh sách dưới đây là bộ script tối thiểu hợp lý để dựng prototype phase đầu. Có thể chưa dùng ngay toàn bộ, nhưng nên tạo cấu trúc tên từ đầu để sau này không đổi loạn.

| Script | Folder | Vai trò | Milestone gợi ý |
| --- | --- | --- | --- |
| BootFlowController.cs | Scripts/Core | Khởi tạo app, load config, chuyển scene | M1 |
| GameManager.cs | Scripts/Managers | Điều phối state game cấp cao | M1 |
| SaveLoadManager.cs | Scripts/Managers | Lưu/đọc save local | M2 |
| FarmCameraController.cs | Scripts/World/Camera | Pan, clamp, focus camera trong farm | M1 |
| WorldInteractionController.cs | Scripts/World/Interaction | Raycast/tap object world, gửi sự kiện chọn | M1 |
| CropSystem.cs | Scripts/Gameplay/Crops | Sinh vòng đời cây, timer, HP, event care | M1-M2 |
| CropTileView.cs | Scripts/World/Views | Hiển thị một ô đất/cây trong world | M1 |
| CropActionPanelController.cs | Scripts/UI/Panels | Panel chăm cây theo trạng thái | M2 |
| SeedShopPanelController.cs | Scripts/UI/Panels | Hiển thị seed shop và mua/gieo hạt | M1 |
| HarvestResolver.cs | Scripts/Gameplay/Crops | Tính sản lượng cuối theo HP/time/random | M2 |
| AnimalSystem.cs | Scripts/Gameplay/Animals | Timer lớn lên, đói, sell window, tử vong | M3 |
| AnimalPenView.cs | Scripts/World/Views | Hiển thị chuồng và thú nuôi trong world | M3 |
| AnimalDetailPanelController.cs | Scripts/UI/Panels | Panel chi tiết thú, cho ăn, bán | M3 |
| StorageSystem.cs | Scripts/Gameplay/Storage | Stack, slot, full-block rule | M2 |
| StoragePanelController.cs | Scripts/UI/Panels | Hiển thị kho, filter, chọn item | M2 |
| SellPanelController.cs | Scripts/UI/Panels | Bán nông sản/item và cập nhật vàng | M2 |
| EconomySystem.cs | Scripts/Gameplay/Economy | Vàng, giá mua/bán, chi phí thức ăn | M1-M2 |
| LevelSystem.cs | Scripts/Gameplay/Progression | EXP, level up, unlock ruộng/chuồng | M2 |
| HUDTopBarController.cs | Scripts/UI/HUD | Gold / EXP / Storage display | M1 |
| PopupManager.cs | Scripts/UI/Common | Mở/đóng popup, stack thứ tự popup | M1 |
| LevelUpPopup.cs | Scripts/UI/Popups | Popup lên level và unlock | M2 |
| StorageFullPopup.cs | Scripts/UI/Popups | Popup kho đầy, điều hướng sang kho/bán | M2 |
| BasicFoodShopPanelController.cs | Scripts/UI/Panels | Shop cỏ/sâu chống bí | M3 |
| MiniGameEventSystem.cs | Scripts/Gameplay/Events | Trigger random mini game/event cứu hộ | M4 |
| MiniGamePopupController.cs | Scripts/UI/Popups | Entry popup cho mini game/event | M4 |

5. Recommended hierarchy - MainFarmScene

Hierarchy tree đề xuất

| SCN_MainFarm<br>├─ Managers<br>│  ├─ GameManager<br>│  ├─ SaveLoadManager<br>│  ├─ PopupManager<br>│  └─ EventBus / MessageHub (optional)<br>├─ CameraRoot<br>│  └─ MainCamera<br>├─ WorldRoot<br>│  ├─ TerrainRoot<br>│  ├─ CropAreaRoot<br>│  ├─ AnimalAreaRoot<br>│  ├─ BuildingsRoot<br>│  │  └─ StorageBuildingRoot<br>│  ├─ DecorRoot<br>│  └─ InteractionMarkersRoot<br>├─ HUDCanvas<br>│  ├─ TopBar<br>│  ├─ BottomNav (optional early build)<br>│  └─ FloatingAlerts<br>├─ PopupCanvas<br>│  ├─ PanelRoot<br>│  └─ PopupRoot<br>└─ SystemsRoot (optional if team tách systems bằng GameObject) |
| --- |

6. Folder-by-folder guideline

| Folder | Nên chứa | Không nên chứa |
| --- | --- | --- |
| Art/UI | Sprite nguồn UI, atlas, icon, panel base | Prefab UI hoặc script |
| Art/World | Sprite nền farm, crop phases, animals, buildings | Config gameplay |
| Data/ScriptableObjects | CropData, AnimalData, ItemData, LevelData | Prefab scene object |
| Prefabs/UI | Component/panel/popup prefab tái sử dụng | Texture gốc hoặc sprite source |
| Prefabs/World | Crop tile, pen, building, decor prefab | Asset chỉ để concept chưa dùng |
| Scripts/Gameplay | Logic gameplay thuần, không phụ thuộc UI trực tiếp | Layout UI hoặc sprite references thô |
| Scripts/UI | Controller/presenter cho panel, HUD, popup | Logic economy/crop loop cốt lõi |

7. Production naming rules chi tiết

- Biến private serializable trong MonoBehaviour: camelCase có [SerializeField], ví dụ: [SerializeField] private CropTileView cropTileView;

- Property / public method / public class: PascalCase, ví dụ: CurrentHealth, OpenSeedShop(), AnimalSystem.

- Const: UPPER_SNAKE_CASE chỉ dùng khi team muốn tách hẳn hằng số. Nếu không, dùng PascalCase cho readonly static cũng được; nhưng phải thống nhất.

- Bool nên đọc được như câu hỏi: isHungry, isLocked, canHarvest, hasStorageSpace.

- Event/callback nên có tiền tố On hoặc hậu tố Changed/Requested/Completed: OnCropSelected, StorageFullRequested, HarvestCompleted.

8. Recommended file list for ScriptableObjects

| Asset | Folder | Tên ví dụ | Ghi chú |
| --- | --- | --- | --- |
| CropData assets | Data/ScriptableObjects/Crops | SO_Crop_Carrot | 1 asset / 1 crop |
| AnimalData assets | Data/ScriptableObjects/Animals | SO_Animal_Cow | 1 asset / 1 animal |
| ItemData assets | Data/ScriptableObjects/Items | SO_Item_WormBundle | gồm seed / produce / feed / event item |
| LevelData assets | Data/ScriptableObjects/Levels | SO_Level_04 | unlock ruộng, chuồng, crop |
| EconomyConfig | Data/ScriptableObjects/Economy | SO_Config_Economy | buy/sell multipliers, safety floor |
| EventConfig | Data/ScriptableObjects/Events | SO_Config_MiniGameRewards | reward pool & trigger chance |

9. File list theo milestone

| Milestone 1 - crop loop tối thiểu<br>Bắt buộc có: BootFlowController, GameManager, FarmCameraController, WorldInteractionController, CropSystem, CropTileView, SeedShopPanelController, HUDTopBarController, EconomySystem.<br>Mục tiêu: gieo trồng, timer lớn, thu hoạch, cập nhật vàng/EXP, pan camera, mở panel cơ bản. |
| --- |

| Milestone 2 - care, kho, bán<br>Bổ sung: HarvestResolver, CropActionPanelController, StorageSystem, StoragePanelController, SellPanelController, SaveLoadManager, StorageFullPopup, LevelSystem, LevelUpPopup.<br>Mục tiêu: HP cây, cỏ/sâu/thiếu nước, kho đầy chặn thu hoạch, bán item để giải phóng kho. |
| --- |

| Milestone 3 - animal loop<br>Bổ sung: AnimalSystem, AnimalPenView, AnimalDetailPanelController, BasicFoodShopPanelController.<br>Mục tiêu: mở chuồng bằng level + cost, cho ăn, timer lớn lên, bán vật nuôi. |
| --- |

| Milestone 4 - event & polish<br>Bổ sung: MiniGameEventSystem, MiniGamePopupController và các popup/warning mở rộng.<br>Mục tiêu: comeback loop, event reward, polish feedback/UI. |
| --- |

10. Nên tránh trong project structure

- Không để script test tạm trong cùng folder với script ship chính. Nếu cần test, dùng Scenes/Sandbox và Scripts/Utilities hoặc Tests.

- Không đặt script UI đọc trực tiếp logic crop/animal phức tạp rồi tự tính toán. UI chỉ nên gọi service/system hoặc đọc state đã chuẩn hóa.

- Không giữ nhiều bản prefab gần giống nhau chỉ khác một màu nhỏ. Dùng prefab variant hoặc style token.

- Không tạo nhiều singleton không kiểm soát. Chỉ manager thực sự toàn cục mới nên singleton.

11. Definition of done cho phần structure

| Hạng mục | Done khi | Người check |
| --- | --- | --- |
| Folder structure | Project tree tạo đúng root, không file lạc ngoài quy ước | Lead dev |
| Naming convention | Tên scene/script/prefab mới đều bám pattern đã chốt | Lead dev / reviewer |
| Core scripts | Danh sách script M1 tồn tại và compile sạch | Dev |
| Prefab map | Prefab UI/world cốt lõi tạo đúng folder | UI dev |
| SO assets | Crop/Animal/Item/Level asset đầu tiên tạo đúng chuẩn | Game designer / dev |

| Khuyến nghị cuối<br>Nếu team nhỏ hoặc 1 người làm, vẫn nên giữ cấu trúc này ngay từ đầu. Chi phí tạo folder và tên chuẩn rất thấp, nhưng lợi ích về sau rất lớn.<br>Khi đã bắt đầu code, nên xem tài liệu này như chuẩn tham chiếu. Nếu có đổi convention, phải đổi đồng bộ một lần và cập nhật lại tài liệu. |
| --- |
