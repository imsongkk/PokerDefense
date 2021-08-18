using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PokerDefense.Managers;
using PokerDefense.Utils;

namespace PokerDefense.Scene
{
    public abstract class BaseScene : MonoBehaviour
    {
        public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;
        protected virtual void Init()
        {
            Object o = GameObject.FindObjectOfType(typeof(EventSystem));
            if (o == null)
                GameManager.Resource.Instantiate("EventSystem").name = "@EventSystem";
        }
        public abstract void Clear();
    }
}
