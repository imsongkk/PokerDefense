using PokerDefense.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokerDefense.Utils
{
    // ÄÄÆ÷³ÍÆ®¿¡ ºÙ¾î¾ß ÇÒ ½Ì±ÛÅæ
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;
        private static bool isDestroy = false;

        protected virtual void OnDestroy() => isDestroy = true;

        public static T Instance
        {
            get
            {
                if (isDestroy) return null;

                if (instance == null)
                {
                    instance = GameObject.Find("@Managers").AddComponent<T>();
                    /*
                    instance = FindObjectOfType(typeof(T)) as T;
                    if (FindObjectsOfType(typeof(T)).Length > 1) 
                        return instance;

                    if (instance == null)
                    {
                        GameObject g = new GameObject();
                        instance = g.AddComponent<T>();
                        g.name = typeof(T).Name;
                        //DontDestroyOnLoad(g);
                    }
                    */
                }
                return instance;
            }
        }
    }
}
