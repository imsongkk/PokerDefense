using UnityEngine;
using PokerDefense.Utils;

namespace PokerDefense.Managers
{
    public class GameManager : MonoBehaviour
    {
        static GameManager instance;
        public static GameManager Instance { get { Init(); return instance; } }

        UIManager uiManager = new UIManager();
        ResourceManager resourceManager = new ResourceManager();
        TowerManager towerManager = new TowerManager();
        DataManager dataManager = new DataManager();
        RoundManager roundManager = null;
        InputManager inputManager = null;
        PokerManager pokerManager = null;
        HorseManager horseManager = null;

        public static UIManager UI { get => Instance.uiManager; }
        public static ResourceManager Resource { get => Instance.resourceManager; }
        public static TowerManager Tower { get => Instance.towerManager; }
        public static DataManager Data { get => Instance.dataManager; }
        public static RoundManager Round { get => Instance.roundManager; }
        public static InputManager Input { get => Instance.inputManager; }
        public static PokerManager Poker { get => Instance.pokerManager; }
        public static HorseManager Horse { get => Instance.horseManager; }

        void Awake()
            => Init();

        static void Init()
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject { name = "@Managers" };
                    go.AddComponent<GameManager>();
                }
                instance = go.GetComponent<GameManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
        }

        public static void AddRoundManager(GameObject target)
        {
            if (instance.roundManager != null) return;

            instance.roundManager = target.AddComponent<RoundManager>();
        }

        public static void AddHorseManager(GameObject target)
        {
            if (instance.horseManager != null) return;

            instance.horseManager = target.GetComponent<HorseManager>();
        }

        public static void GetOrAddPokerManager(GameObject target)
        {
            if (instance.pokerManager != null) return;
            instance.pokerManager = Util.GetOrAddComponent<PokerManager>(target);
        }

        public static void AddInputManager()
        {
            if (instance.inputManager != null)
            {
                instance.inputManager.enabled = true;
                return;
            }
            instance.inputManager = instance.gameObject.AddComponent<InputManager>();
        }

        public static void DeleteInputManager()
        {
            if (instance.inputManager == null) return;

            instance.inputManager.enabled = false;
        }
    }
}
