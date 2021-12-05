using PokerDefense.Managers;
using PokerDefense.UI.Popup;
using PokerDefense.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PokerDefense.UI
{
    public class UI_CardItem : UI_Base
    {
        UI_Poker ui_Poker = null;

        Image cardImage;

        int number;
        CardShape shape;

        enum GameObjects
        {
            CardItem,
        }

        private void Start()
            => Init();

        public override void Init()
        {
            BindObjects();
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            cardImage = GetObject((int)GameObjects.CardItem).GetComponent<Image>();

            AddUIEvent(gameObject, OnClickCardItem, Define.UIEvent.Click);
        }

        private void OnClickCardItem(PointerEventData evt)
        {
            Debug.Log($"{shape.ToString()} {number} 카드 터치");
        }

        public void InitCard(int num, CardShape shape)
        {
            this.number = num;
            this.shape = shape;

            RefreshImage();
        }

        private void RefreshImage() // 카드의 이미지 바꿈(맨 처음 초기화 or 찬스로 카드 바꿈 or 유저 클릭 이벤트)
        {
            cardImage = GameManager.Poker.GetImage((shape, number));
        }
    }
}
