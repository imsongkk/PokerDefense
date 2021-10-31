using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PokerDefense.Managers;
using PokerDefense.Utils;

namespace PokerDefense.Scene
{
    public class BaseScene : MonoBehaviour
    {
        public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;
        protected virtual void Init()
        {
            Object eventSystem = GameObject.FindObjectOfType(typeof(EventSystem));
            if (eventSystem == null)
            {
                eventSystem = GameManager.Resource.Instantiate("EventSystem");
                eventSystem.name = "@EventSystem";
                DontDestroyOnLoad(eventSystem);
            }
        }
        //public abstract void Clear();
    }
}
