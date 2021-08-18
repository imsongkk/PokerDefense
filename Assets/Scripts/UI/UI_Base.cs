using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PokerDefense.Utils;

namespace PokerDefense.UI
{
    public abstract class UI_Base : MonoBehaviour
    {
        protected bool IsInit { get; set; } = false;
        protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

        public abstract void Init();

        protected virtual void OnDestroy()
        {
            _objects.Clear();
        }

        protected void Bind<T>(Type type) where T : UnityEngine.Object
        {
            string[] names = Enum.GetNames(type);
            UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];

            _objects.Add(typeof(T), objects);

            for (int i = 0; i < names.Length; i++)
            {
                if (typeof(T) == typeof(GameObject))
                    objects[i] = Util.FindChild(gameObject, names[i], true);
                else
                    objects[i] = Util.FindChild<T>(gameObject, names[i], true);

                if (objects[i] == null)
                    Debug.Log($"Failed to bind({names[i]})");
            }
        }

        protected T Get<T>(int idx) where T : UnityEngine.Object
        {
            UnityEngine.Object[] objects = null;
            if (_objects.TryGetValue(typeof(T), out objects) == false) { return null; }

            return objects[idx] as T;
        }

        protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
        protected Text GetText(int idx) { return Get<Text>(idx); }
        protected Button GetButton(int idx) { return Get<Button>(idx); }
        protected Image GetImage(int idx) { return Get<Image>(idx); }

        public static void AddUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type)
        {
            UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

            switch (type)
            {
                case Define.UIEvent.Click:
                    evt.OnClickHandler -= action;
                    evt.OnClickHandler += action;
                    break;
                case Define.UIEvent.Drag:
                    evt.OnDragHandler -= action;
                    evt.OnDragHandler += action;
                    break;
            }
        }
        public static void AddButtonAnim(GameObject go, bool hasAnim = true)
        {
            UI_Button uiButton = go.GetOrAddComponent<UI_Button>();
            uiButton.buttonType = UI_Button.BUTTONTYPE.Button;
            uiButton.HasAnim = hasAnim;
        }
    }
}
