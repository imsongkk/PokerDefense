using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PokerDefense.Utils;


public class GameObjectEventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Action<PointerEventData> ObjectPointerDownHandler = null;
    public Action<PointerEventData> ObjectPointerUpHandler = null;
    public Action<PointerEventData> ObjectDragHandler = null;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (ObjectPointerDownHandler != null) { ObjectPointerDownHandler.Invoke(eventData); }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (ObjectPointerUpHandler != null) { ObjectPointerUpHandler.Invoke(eventData); }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ObjectPointerUpHandler != null) { ObjectDragHandler.Invoke(eventData); }
    }

    public static void AddObjectEvent(GameObject go, Action<PointerEventData> action, Define.MouseEvent type)
    {
        GameObjectEventHandler evt = Util.GetOrAddComponent<GameObjectEventHandler>(go);

        switch (type)
        {
            case Define.MouseEvent.PointerDown:
                evt.ObjectPointerDownHandler -= action;
                evt.ObjectPointerDownHandler += action;
                break;
            case Define.MouseEvent.PointerUp:
                evt.ObjectPointerUpHandler -= action;
                evt.ObjectPointerUpHandler += action;
                break;
        }
    }
}