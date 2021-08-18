using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokerDefense.Managers
{
    public class ResourceManager
    {
        public T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        public GameObject Instantiate(string path, Transform parent = null)
        {
            GameObject go = Load<GameObject>($"Prefabs/{path}");
            if (go == null)
            {
                Debug.Log($"Failed to load prefab : Prefabs/{path} ");
                return null;
            }
            return Object.Instantiate(go, parent);
        }

        public void Destroy(GameObject go)
        {
            if (go == null) { return; }
            Object.Destroy(go);
        }
    }
}
