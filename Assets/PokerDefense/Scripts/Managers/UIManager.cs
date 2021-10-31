using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokerDefense.UI.Popup;
using PokerDefense.UI.Scene;
using PokerDefense.Utils;

namespace PokerDefense.Managers
{
    public class UIManager
    {
        int popupOrder = 10; 
        Stack<UI_Popup> popupStack = new Stack<UI_Popup>();
        UI_Scene sceneUI = null;

        public UI_Scene CurrentSceneUI { get { return sceneUI; } }

        public GameObject Root
        {
            get
            {
                GameObject root = GameObject.Find("@UI_Root");
                if (root == null)
                    root = new GameObject { name = "@UI_Root" };
                return root;
            }
        }
        public void SetCanvas(GameObject go, bool sort = true)
        {
            Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
            if (sort)
                canvas.sortingOrder = popupOrder++;
            else
                canvas.sortingOrder = 0;
        }
        public T ShowSceneUI<T>(string name = null) where T : UI_Scene
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = GameManager.Resource.Instantiate($"UI/Scene/{name}");

            T sceneUI = Util.GetOrAddComponent<T>(go);
            this.sceneUI = sceneUI;
            go.transform.SetParent(Root.transform);

            return sceneUI;
        }

        public T ShowPopupUI<T>(string name = null) where T : UI_Popup
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = GameManager.Resource.Instantiate($"UI/Popup/{name}");

            T popupUI = Util.GetOrAddComponent<T>(go);
            popupStack.Push(popupUI);
            go.transform.SetParent(Root.transform);

            return popupUI;
        }

        public void ClosePopupUI(UI_Popup popupUI)
        {
            if (popupStack.Count == 0)
                return;
            if (popupStack.Peek() != popupUI)
            {
                Debug.Log("Close Popup Failed!");
                return;
            }
            ClosePopupUI();
        }

        public void ClosePopupUI()
        {
            if (popupStack.Count == 0)
                return;
            UI_Popup popupUI = popupStack.Pop();
            GameManager.Resource.Destroy(popupUI.gameObject);
            popupUI = null;
            popupOrder--;
        }

        public void CloseAllPopupUI()
        {
            while (popupStack.Count > 0)
                ClosePopupUI();
        }

        public GameObject ShowFadeUI()
        {
            GameObject fadeUI = GameObject.Find("UI_Fade(Clone)");
            if (fadeUI == null)
                fadeUI = GameManager.Resource.Instantiate("UI/Popup/UI_Fade");
            return fadeUI;
        }
    }
}
