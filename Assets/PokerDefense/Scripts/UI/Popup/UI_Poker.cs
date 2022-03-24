using PokerDefense.UI.Popup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PokerDefense.Managers;
using UnityEngine.EventSystems;
using PokerDefense.Utils;
using static PokerDefense.Utils.Define;

namespace PokerDefense.UI.Popup
{
    public class UI_Poker : UI_Popup
    {
        private enum GameObjects
        {
            CardList,
            CardDeck,
            PokerButton,
            ConfirmButton,
            HelpButton,
        }

        UI_CardItem[] cardItems = new UI_CardItem[5];

        GameObject pokerButton, confirmButton;

        private List<(CardShape shape, int number)> cardList;

        private void Start()
            => Init();

        public override void Init()
        {
            base.Init();

            BindObjects();
            InitCardItems();

            cardList = InGameManager.Poker.CardList; // 유저의 5개 카드 리스트
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            pokerButton = GetObject((int)GameObjects.PokerButton);
            AddUIEvent(pokerButton, OnClickPokerButton, Define.UIEvent.Click);
            AddButtonAnim(pokerButton);

            confirmButton = GetObject((int)GameObjects.ConfirmButton);
            AddUIEvent(confirmButton, OnClickConfirmButton, Define.UIEvent.Click);
            AddButtonAnim(confirmButton);

            GameObject helpButton = GetObject((int)GameObjects.HelpButton);
            AddUIEvent(helpButton, OnClickHelpButton, Define.UIEvent.Click);
            AddButtonAnim(helpButton);
        }

        private void InitCardItems()
        {
            Transform cardListTransform = GetObject((int)GameObjects.CardList).transform;

            for(int i=0; i<cardListTransform.childCount; i++)
            {
                UI_CardItem cardItem = cardListTransform.GetChild(i).GetComponent<UI_CardItem>();
                cardItems[i] = cardItem;
                cardItem.InitCard(i,(CardShape.Joker, 0)); // 모든 카드를 Joker Z(디폴트 스프라이트)로 교체
            }
        }

        private void OnClickPokerButton(PointerEventData evt)
        {
            InGameManager.Poker.GetHand((int index) =>
            {
                cardItems[index].OnPokerDrawed(cardList[index]);
            });

            pokerButton.SetActive(false);
            confirmButton.SetActive(true);
        }

        private void OnClickConfirmButton(PointerEventData evt)
        {
            // 포커 패 확정!
            // RoundManager에게 Poker State 종료 알리기
            InGameManager.Round.BreakState();
            ClosePopupUI();
        }

        private void OnClickHelpButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_PokerHelpPopup>();
        }
    }
}