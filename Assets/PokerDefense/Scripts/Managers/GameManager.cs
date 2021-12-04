using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PokerDefense.Utils;

namespace PokerDefense.Managers
{
    public class GameManager : MonoBehaviour
    {
        static GameManager instance;
        public static GameManager Instance { get { Init(); return instance; } }

        UIManager uiManager = new UIManager();
        ResourceManager resourceManager = new ResourceManager();
        RoundManager roundManager = null;
        TowerManager towerManager = null;
        InputManager inputManager = null;
        PokerManager pokerManager = null;

        public static UIManager UI { get => Instance.uiManager; }
        public static ResourceManager Resource { get => Instance.resourceManager; }
        public static RoundManager Round { get => Instance.roundManager; }
        public static TowerManager Tower { get => Instance.towerManager; }
        public static InputManager Input { get => Instance.inputManager; }
        public static PokerManager Poker { get => Instance.pokerManager; }

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

        public static void AddTowerManager(GameObject target)
        {
            if (instance.towerManager != null) return;

            instance.towerManager = target.AddComponent<TowerManager>();
        }

        public static void GetOrAddRoundManager(GameObject target)
        {
            if (instance.roundManager != null) return;
            instance.roundManager = Util.GetOrAddComponent<RoundManager>(target);
        }

        public static void GetOrAddTowerManager(GameObject target)
        {
            if (instance.towerManager != null) return;
            instance.towerManager = Util.GetOrAddComponent<TowerManager>(target);
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
