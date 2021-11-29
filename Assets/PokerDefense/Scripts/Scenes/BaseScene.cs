using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PokerDefense.Managers;
using PokerDefense.Utils;
using System;

namespace PokerDefense.Scene
{
    public class BaseScene : MonoBehaviour
    {
        public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;
        
        protected Action OnDestroyAction = null;

        protected virtual void Init()
        {
            UnityEngine.Object eventSystem = GameObject.FindObjectOfType(typeof(EventSystem));
            if (eventSystem == null)
            {
                eventSystem = GameManager.Resource.Instantiate("EventSystem");
                eventSystem.name = "@EventSystem";
                DontDestroyOnLoad(eventSystem);
            }
        }

        private void OnDestroy()
            => OnDestroyAction?.Invoke();

        public void AddOnDestroyAction(Action action)
            => OnDestroyAction += action;
        //public abstract void Clear();
    }
}
