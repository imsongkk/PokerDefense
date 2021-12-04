using PokerDefense.UI.Popup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PokerDefense.Managers;

namespace PokerDefense.UI.Popup
{
    public class UI_Poker : UI_Popup
    {
        private enum GameObjects
        {
            CardDeck,
            CardHand
        }

        private Transform cardDeck;
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

            Bind<GameObject>(typeof(GameObjects));
            BindObject();

            cardsSpriteList = GameManager.Poker.cardsSpriteList;
            cardList = GameManager.Poker.CardList;

            cardPrefab = GameManager.Poker.CardPrefab;
        }

        private void BindObject()
        {
            cardDeck = GetObject((int)GameObjects.CardDeck).transform;
            cardHand = GetObject((int)GameObjects.CardHand).transform;
        }

        public Card InstantiateCard((CardShape shape, int number) cardTuple, Transform parent)
        {
            Card card = Instantiate(cardPrefab, parent).GetComponent<Card>();
            card.InitCard(cardTuple.number, cardTuple.shape, cardsSpriteList[(int)cardTuple.shape][cardTuple.number]);
            return card;
        }

        public void PokerUIStart()
        {
            //TODO 카드뽑기 애니메이션
            for (int i = 0; i < 5; i++)
            {
                InstantiateCard(cardList[i], cardHand.GetChild(i));
            }
        }

    }
}