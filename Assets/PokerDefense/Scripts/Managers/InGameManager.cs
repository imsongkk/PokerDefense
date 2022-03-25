using PokerDefense.UI.Scene;
using UnityEngine;

namespace PokerDefense.Managers
{
    // Singleton
    public class InGameManager : MonoBehaviour
    {
        static InGameManager instance;
        public static InGameManager Instance { get { Init(); return instance; } }

        UI_InGameScene inGameScene = null;
        SystemMessageManager systemMessageManager = null;
        RoundManager roundManager = null;
        InputManager inputManager = null;
        PokerManager pokerManager = null;
        HorseManager horseManager = null;
        SkillManager skillManager = null;

        TowerManager towerManager = new TowerManager();
        InventoryManager inventoryManager = new InventoryManager();

        // Non-Mono
        public static InventoryManager Inventory { get => Instance.inventoryManager; }
        public static TowerManager Tower { get => Instance.towerManager; }
        // UI
        public static UI_InGameScene UI_InGameScene { get { return Instance.inGameScene; } set { Instance.inGameScene = value; } }
        // Mono
        public static SystemMessageManager SystemMessage { get { return Instance.systemMessageManager; } }
        public static PokerManager Poker { get { return Instance.pokerManager;} }
        public static HorseManager Horse { get { return Instance.horseManager;} }
        public static SkillManager Skill { get { return Instance.skillManager;} }
        public static RoundManager Round { get { return Instance.roundManager;} }
        public static InputManager Input { get { return Instance.inputManager;} }

        private void Awake()
        {
            UI_InGameScene = GameManager.UI.ShowSceneUI<UI_InGameScene>();
        }

        private static void Init()
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("InGameManager");
                instance = go.GetComponent<InGameManager>();
            }
        }

        public static void InitInGameManager() // InGameScene에 의해 호출
		{
            Instance.GetManagers();

            switch (GameManager.InGameSceneMode)
            {
                case GameManager.inGameSceneMode.NewGame:
                    Instance.InitManagersNewGame();
                    break;
                case GameManager.inGameSceneMode.LoadGame:
                    Instance.InitManagersLoadGame();
                    break;
                case GameManager.inGameSceneMode.EditorMode:
                    break;
            }
		}

        public void GetManagers() 
        {
            // Mono
            Instance.skillManager = gameObject.GetComponentInChildren<SkillManager>();
            Instance.roundManager = gameObject.GetComponentInChildren<RoundManager>();
            Instance.pokerManager = gameObject.GetComponentInChildren<PokerManager>();
            Instance.horseManager = gameObject.GetComponentInChildren<HorseManager>();
            Instance.inputManager = gameObject.GetComponentInChildren<InputManager>();
            Instance.systemMessageManager = gameObject.GetComponentInChildren<SystemMessageManager>();
        }

        private void InitManagersNewGame()
		{
            Debug.Log("New");
            // Mono중에 Init 필요한 애들
            Skill.InitSkillManager();
            Round.InitRoundManager();
            Horse.InitHorseManager();
            SystemMessage.InitSystemMessageManager();

            // Non-Mono
            Inventory.InitInventoryManager();
            Tower.InitTowerManager();
        }

        private void InitManagersLoadGame()
		{
            Debug.Log("Load");
            // Mono중에 Init 필요한 애들
            Skill.InitSkillManager();
            Round.InitRoundManager();
            Horse.InitHorseManager();
            SystemMessage.InitSystemMessageManager();

            // Non-Mono
            Inventory.InitInventoryManager();
            Tower.InitTowerManager();
        }

        private void InitManagersEditorMode()
		{

		}

        public void Reset()
        {

        }
    }
}
