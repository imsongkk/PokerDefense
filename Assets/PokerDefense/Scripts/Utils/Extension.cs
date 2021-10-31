using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PokerDefense.Utils;
using PokerDefense.UI;

namespace PokerDefense.Utils
{
    public static class Extension
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
        {
            return Util.GetOrAddComponent<T>(go);
        }
        public static void AddUIEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
        {
            UI_Base.AddUIEvent(go, action, type);
        }
        public static T DeepCopy<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var bformatter = new BinaryFormatter();
                bformatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)bformatter.Deserialize(ms);
            }
        }
    }
}
