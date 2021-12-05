using PokerDefense.UI.Popup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PokerDefense.Managers;
using UnityEngine.EventSystems;
using PokerDefense.Utils;

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
        }

        GameObject cardDeck;
        UI_CardItem[] cardItems = new UI_CardItem[5];
        bool isPokerDrawed = false;

        private Transform cardHand;
        private GameObject cardPrefab;

        private Card[] cardObjectHand = new Card[5];
        private List<Sprite>[] cardsSpriteList;
        private List<(CardShape shape, int number)> cardList;

        private void Start()
            => Init();

        public override void Init()
        {
            base.Init();
            GameManager.Poker.SetUIPoker(this);

            BindObjects();

            cardsSpriteList = GameManager.Poker.cardsSpriteList;
            cardList = GameManager.Poker.CardList;

            cardPrefab = GameManager.Poker.CardPrefab;

        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            Transform cardListTransform = GetObject((int)GameObjects.CardList).transform;
            InitCardItems(cardListTransform);

            cardDeck = GetObject((int)GameObjects.CardDeck);

            GameObject pokerButton = GetObject((int)GameObjects.PokerButton);
            AddUIEvent(pokerButton, OnClickPokerButton, Define.UIEvent.Click);
            AddButtonAnim(pokerButton);

            GameObject confirmButton = GetObject((int)GameObjects.ConfirmButton);
            AddUIEvent(confirmButton, OnClickConfirmButton, Define.UIEvent.Click);
            AddButtonAnim(confirmButton);
        }

        private void InitCardItems(Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                UI_CardItem cardItem = parent.GetChild(i).gameObject.AddComponent<UI_CardItem>();
                cardItems[i] = cardItem;
            }
        }

        /*
        public Card InstantiateCard((CardShape shape, int number) cardTuple, Transform parent)
        {
            Card card = Instantiate(cardPrefab, parent).GetComponent<Card>();
            card.InitCard(cardTuple.number, cardTuple.shape, cardsSpriteList[(int)cardTuple.shape][cardTuple.number]);
            return card;
        }
        */

        private void InstantiateCard(UI_CardItem card, (CardShape shape, int number) cardTuple)
        {
            card.InitCard(cardTuple.number, cardTuple.shape);
        }

        public void InstantiateCardIndex(int index, Transform hand)
        {
            //TODO 카드뽑기 애니메이션
            //cardObjectHand[index] = InstantiateCard(cardList[index], hand.GetChild(index));

            InstantiateCard(cardItems[index], cardList[index]);
        }

        public void PokerUIStart()
        {
            //TODO 카드뽑기 애니메이션
            
        }

        public void PokerUIReset()
        {
            //TODO 카드 넣기 & 섞기 애니메이션
            for (int i = 0; i < 5; i++)
            {
                Destroy(cardObjectHand[i]);
            }
        }

        public void ChangeCardUI(int index)
        {
            var oldCard = cardObjectHand[index];
            InstantiateCardIndex(index, cardHand);
            Destroy(oldCard);
        }

        private void OnClickPokerButton(PointerEventData evt)
        {
            /* RoundState, 찬스 개수에 따라 눌릴지 안눌릴지 결정 */

            for (int index = 0; index < 5; index++)
            {
                InstantiateCardIndex(index, cardHand);
            }
            isPokerDrawed = true;
        }

        private void OnClickConfirmButton(PointerEventData evt)
        {
            if(!isPokerDrawed)
            {
                GameManager.UI.ShowPopupUI<UI_PokerErrorPopup>();
                return;
            }

            // 포커 패 확정!
            // RoundManager에게 Poker State 종료 알리기
            ClosePopupUI();
        }
    }
}