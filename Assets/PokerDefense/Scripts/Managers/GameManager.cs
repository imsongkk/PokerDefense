using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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



        public static UIManager UI { get => Instance.uiManager; }
        public static ResourceManager Resource { get => Instance.resourceManager; }
        public static TowerManager Tower { get => Instance.towerManager; }
        public static DataManager Data { get => Instance.dataManager; }
        public static RoundManager Round { get => Instance.roundManager; }
        public static InputManager Input { get => Instance.inputManager; }

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
