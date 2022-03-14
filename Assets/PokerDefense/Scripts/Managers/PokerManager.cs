using PokerDefense.Utils;
using PokerDefense.UI.Popup;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static PokerDefense.Utils.Define;

namespace PokerDefense.Managers
{

    // 포커 룰: https://silk-ornament-d81.notion.site/Z7JCard-3825581ae9b442229ae51e410bf6fdb4
    public class PokerManager : MonoBehaviour
    {
        protected List<HandRank> hiddenRanks = new List<HandRank>
        {
            HandRank.Beast,
            HandRank.BackStraight,
            HandRank.RoyalStraight,
            HandRank.FiveCards,
            HandRank.BackStraightFlush,
            HandRank.Jackpot
        };

        public List<HandRank> HiddenRanks
        {
            get { return hiddenRanks; }
        }

        //숫자: 0~7, Z는 0으로 취급하여 저장
        //! 에디터 직렬화! MonoBehaviour 빼지 말것!!!!!
        public List<Sprite> spadeSpriteList = new List<Sprite>();
        public List<Sprite> heartSpriteList = new List<Sprite>();
        public List<Sprite> cloverSpriteList = new List<Sprite>();
        public List<Sprite> diamondSpriteList = new List<Sprite>();
        public List<Sprite> jokerSpriteList = new List<Sprite>();
        // Joker Z카드는 존재하지 않음.
        // 패 초기화 시 덮은 카드는 Shape를 Null로

        public List<Sprite>[] cardsSpriteList = new List<Sprite>[5];

        Hand myHand;

        // 덱
        private List<(CardShape shape, int number)> deque = new List<(CardShape shape, int number)>();

        // 손패
        private List<(CardShape shape, int number)> cardList = new List<(CardShape shape, int number)>();
        public List<(CardShape shape, int number)> CardList
        {
            get { return cardList; }
        }

        // 각 라운드마다 카드 바꿀 기회 한번씩 제공
        // 추가로 카드를 바꿀때마나 chance 1씩 소모
        private bool[] isCardChanged = { false, false, false, false, false };

        private void Start()
        {
            cardsSpriteList[0] = spadeSpriteList;
            cardsSpriteList[1] = heartSpriteList;
            cardsSpriteList[2] = diamondSpriteList;
            cardsSpriteList[3] = cloverSpriteList;
            cardsSpriteList[4] = jokerSpriteList;
        }

        private void SetDeque()
        {
            deque = new List<(CardShape shape, int number)>();
            deque.Capacity = 32;
            for (int cardNum = 0; cardNum < 32; cardNum++)
            {
                deque.Add(((CardShape)(cardNum / 8), (cardNum % 8)));
            }

            ShuffleDeque();
        }

        private void ShuffleDeque()
        {
            var random = new System.Random();
            deque = deque.OrderBy(item => random.Next()).ToList();
        }

        private (CardShape shape, int number) PopCard()
        {
            var card = deque[deque.Count - 1];
            deque.RemoveAt(deque.Count - 1);
            return card;
        }

        public void ChangeCard(int index, Action<(CardShape, int)> OnChangeCardSuccess)
        {
            if (isCardChanged[index])
            {
                if (GameManager.Inventory.Chance <= 0)
                {
                    GameManager.UI.ShowPopupUI<UI_ChanceErrorPopup>();
                    return;
                }
                else GameManager.Inventory.Chance--;  // TODO: 찬스 개수 변할 경우 UI 업데이트 콜
            }
            else isCardChanged[index] = true;

            var oldCard = cardList[index];
            cardList[index] = PopCard();
            deque.Insert(0, oldCard);      //덱의 맨 밑에 넣기

            OnChangeCardSuccess?.Invoke(cardList[index]);
        }

        public void ResetInitialChance()
        {
            for (int i = 0; i < 5; i++)
            {
                isCardChanged[i] = false;
            }
        }

        public void ResetDeque(Action OnResetDequeSuccess)
        {
            for (int i = 4; i >= 0; i--)
            {
                deque.Add(cardList[i]);
                cardList.RemoveAt(i);
            }
            // ui_Poker.PokerUIReset();
            OnResetDequeSuccess?.Invoke();
            ShuffleDeque();
        }

        public void PokerStart()
        {
            SetDeque();
        }

        public void GetHand(Action<int> OnGetHandSuccess)
        {
            for (int i = 0; i < 5; i++)
            {
                cardList.Add(PopCard());
                OnGetHandSuccess?.Invoke(i);
            }
        }

        public Hand CalculateMyHand()
        {
            return HandCalaulate(cardList);
        }

        public Hand HandCalaulate(List<(CardShape shape, int number)> cardList)
        {
            cardList.Sort((x, y) => x.number.CompareTo(y.number));
            int topCard = cardList[4].number;

            //! Debug
            foreach (var card in cardList)
            {
                Debug.Log($"Deck: {card.shape} {card.number}");
            }
            Debug.Log($"Topcard: {topCard}");

            List<int> sameNumbers = new List<int>();
            int sameNumber = 1;
            bool isFlush = true;
            bool isStraight = true;
            bool isRoyalStraight = false;

            HandRank handRank;

            for (int i = 1; i < 5; i++)
            {
                if (cardList[i].number == cardList[i - 1].number)
                {
                    sameNumber++;
                    isStraight = false;
                }
                else
                {
                    if (cardList[i].number != cardList[i - 1].number + 1)
                    {
                        if ((i == 1) && (cardList[i - 1].number == 0) && (cardList[i].number == 4))
                        {
                            isRoyalStraight = true;      // Back Straight
                        }
                        else
                        {
                            isStraight = false;
                            isRoyalStraight = false;
                        }
                    }
                    sameNumbers.Add(sameNumber);
                    sameNumber = 1;
                }
                if ((cardList[i].shape != CardShape.Joker) && (cardList[i - 1].shape != CardShape.Joker) && (cardList[i].shape != cardList[i - 1].shape))
                {
                    // Joker는 임의의 무늬로 취급
                    isFlush = false;
                }
            }

            sameNumbers.Add(sameNumber);

            if (sameNumber == 5)
            {
                if (topCard == 7) handRank = HandRank.Jackpot;
                else handRank = HandRank.FiveCards;
            }
            else if (sameNumbers.Contains(4))
            {
                handRank = HandRank.FourCards;
            }
            else if (sameNumbers.Contains(3))
            {
                if (sameNumbers.Contains(2)) handRank = HandRank.FullHouse;
                else handRank = HandRank.Tripple;
            }
            else if (sameNumbers.Contains(2))
            {
                int pairs = 0;
                foreach (int counts in sameNumbers)
                {
                    if (counts == 2) pairs++;
                }
                if (pairs == 2) handRank = HandRank.TwoPair;
                else handRank = HandRank.OnePair;
            }
            else
            {
                if (isStraight && isFlush)
                {
                    if (isRoyalStraight) handRank = HandRank.RoyalStraightFlush;
                    else if (topCard == 4) handRank = HandRank.BackStraightFlush;
                    else handRank = HandRank.StraightFlush;
                }
                else if (isStraight)
                {
                    if (isRoyalStraight) handRank = HandRank.RoyalStraight;
                    else if (topCard == 4) handRank = HandRank.BackStraight;
                    else handRank = HandRank.Straight;
                }
                else if (isFlush)
                {
                    handRank = HandRank.Flush;
                }
                else
                {
                    if (topCard == 6) handRank = HandRank.Beast;
                    else handRank = HandRank.HighCard;
                }
            }

            // 플러시 계열 패이고 탑카드가 조커일 경우 탑셰이프가 조커가 아닌 다른 모든 카드의 문양으로 결정
            CardShape topShape = ((cardList[4].shape == CardShape.Joker) && isFlush) ? cardList[3].shape : cardList[4].shape;
            if (cardList[4].number == cardList[3].number) topShape = CardShape.Null;

            Hand returnHand = new Hand(handRank, hiddenRanks.Contains(handRank), topCard, topShape);

            return returnHand;
        }

        public Sprite GetSprite((CardShape shape, int number) cardTuple) // Poker패에 맞는 카드 Image 리소스를 반환
        {
            return cardsSpriteList[(int)cardTuple.shape][cardTuple.number];
        }

    }
}
