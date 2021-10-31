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

        UIManager ui = new UIManager();
        ResourceManager resouce = new ResourceManager();

        public static UIManager UI { get => Instance.ui; }
        public static ResourceManager Resource { get => Instance.resouce; }

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
                /*
                if (EventSystem.current)
                    DontDestroyOnLoad(EventSystem.current);
                */
            }
        }
    }
}
