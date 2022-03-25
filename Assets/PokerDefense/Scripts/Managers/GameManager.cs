using UnityEngine;
using PokerDefense.Utils;

namespace PokerDefense.Managers
{
    public class GameManager : MonoBehaviour
    {
        static GameManager instance;
        public static GameManager Instance { get { Init(); return instance; } }

        public enum inGameSceneMode
		{
            None,

            NewGame,
            LoadGame,
            EditorMode
		}

        inGameSceneMode mode = inGameSceneMode.None;
        UIManager uiManager = new UIManager();
        ResourceManager resourceManager = new ResourceManager();
        DataManager dataManager = new DataManager();

        public static inGameSceneMode InGameSceneMode { get => Instance.mode; set => Instance.mode = value; }
        public static UIManager UI { get => Instance.uiManager; }
        public static ResourceManager Resource { get => Instance.resourceManager; }
        public static DataManager Data { get => Instance.dataManager; }

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
    }
}
