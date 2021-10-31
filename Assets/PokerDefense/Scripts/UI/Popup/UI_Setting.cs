using PokerDefense.UI.Popup;
using PokerDefense.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PokerDefense.UI.Popup
{
    public class UI_Setting : UI_Popup
    {
        enum GameObjects
        {
            BackButton,

        }

        GameObject backButton;

        private void Start()
            => Init();

        public override void Init()
        {
            base.Init();
            Bind<GameObject>(typeof(GameObjects));
            BindObjects();
        }

        private void BindObjects()
        {
            backButton = GetObject((int)GameObjects.BackButton);
            AddUIEvent(backButton, OnClickBackButton, Define.UIEvent.Click);
            AddButtonAnim(backButton);
        }

        private void OnClickBackButton(PointerEventData evt)
        {
            //저장 할지 안할지 물음
            ClosePopupUI();
        }
    }
}