using PokerDefense.Managers;
using PokerDefense.UI.Popup;
using PokerDefense.Utils;
using static PokerDefense.Utils.Define;
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

        int cardIndex;

        enum GameObjects
        {
            CardItem,
        }

        private void Awake()    // Self Component Reference
        {
            cardImage = gameObject.GetComponent<Image>();
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
            AddUIEvent(gameObject, OnClickCardItem, Define.UIEvent.Click);
        }

        public void SetUIPoker(UI_Poker pokerUI)
        {
            this.ui_Poker = pokerUI;
        }

        private void OnClickCardItem(PointerEventData evt)
        {
            //TODO 찬스 있을 경우 다시 뽑기
            Debug.Log($"{shape.ToString()} {number} 터치 됨");
            GameManager.Poker.ChangeCard(cardIndex, (int index) =>
            {
                ui_Poker.InstantiateCardIndex(index);
            });
        }

        public void InitCard(int index, int num, CardShape shape)
        {
            this.cardIndex = index;
            this.number = num;
            this.shape = shape;

            RefreshImage();
        }

        private void RefreshImage() // Poker 패에 맞는 스프라이트 가져오기
        {
            cardImage.sprite = GameManager.Poker.GetSprite((shape, number));
        }
    }
}
