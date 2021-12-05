using PokerDefense.Managers;
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

public enum CardShape
{
    Spade,      // 0
    Heart,      // 1
    Diamond,    // 2
    Clover,     // 3
    Joker       // 4
}

// 포커 룰: https://silk-ornament-d81.notion.site/Z7JCard-3825581ae9b442229ae51e410bf6fdb4
public class PokerManager : MonoBehaviour
{
    public enum HandRank
    {
        HighCard,
        Beast,
        OnePair,
        TwoPair,
        Tripple,
        Straight,
        BackStraight,
        RoyalStraight,
        FullHouse,
        Flush,
        FourCards,
        StraightFlush,
        BackStraightFlush,
        RoyalStraightFlush,
        FiveCards,
        Jackpot
    }

    protected List<HandRank> hiddenRanks = new List<HandRank>
    {
        HandRank.Beast,
        HandRank.BackStraight,
        HandRank.RoyalStraight,
        HandRank.FiveCards,
        HandRank.BackStraightFlush,
        HandRank.Jackpot
    };

    public struct Hand
    {
        HandRank handRank;  // 패의 종류
        bool isHidden;      // 히든 패인지 여부
        int topCard;        // 가장 높은 숫자

        public Hand(HandRank handRank, bool isHidden, int topCard)
        {
            this.handRank = handRank;
            this.isHidden = isHidden;
            this.topCard = topCard;
        }
    }


    //숫자: 0~7, Z는 0으로 취급하여 저장
    public List<Sprite> spadeSpriteList = new List<Sprite>();
    public List<Sprite> heartSpriteList = new List<Sprite>();
    public List<Sprite> cloverSpriteList = new List<Sprite>();
    public List<Sprite> diamondSpriteList = new List<Sprite>();
    public List<Sprite> jokerSpriteList = new List<Sprite>();

    public List<Sprite>[] cardsSpriteList = new List<Sprite>[5];

    UI_Poker ui_Poker;
    [SerializeField] private GameObject cardPrefab;
    public GameObject CardPrefab
    {
        get { return cardPrefab; }
    }

    // 덱
    private List<(CardShape shape, int number)> deque = new List<(CardShape shape, int number)>();

    // 손패
    private List<(CardShape shape, int number)> cardList = new List<(CardShape shape, int number)>();
    public List<(CardShape shape, int number)> CardList
    {
        get { return cardList; }
    }
    // 각 카드를 라운드마다 1번씩 바꿈
    // 카드를 바꿀때마나 chance 1씩 소모
    private bool[] isCardChanged = { false, false, false, false, false };
    private int chance = 5;
    public int Chance
    {
        get { return chance; }
    }

    private void Start()
    {
        cardsSpriteList[0] = spadeSpriteList;
        cardsSpriteList[1] = heartSpriteList;
        cardsSpriteList[2] = diamondSpriteList;
        cardsSpriteList[3] = cloverSpriteList;
    }

    public void SetUIPoker(UI_Poker target)
        => ui_Poker = target;

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

    public void ChangeCard(int index)
    {
        if (!isCardChanged[index])
        {
            if (chance <= 0) return;
            chance--;
            var oldCard = cardList[index];
            cardList[index] = PopCard();
            deque.Insert(0, oldCard);      //덱의 맨 밑에 넣기
            ui_Poker.InstantiateCardIndex(index);
            isCardChanged[index] = true;
        }

    }

    private void ResetDeque()
    {
        for (int i = 4; i >= 0; i--)
        {
            deque.Add(cardList[i]);
            cardList.RemoveAt(i);
        }
        ui_Poker.PokerUIReset();
        ShuffleDeque();
    }

    public void PokerStart()
    {
        SetDeque();
        for (int i = 0; i < 5; i++)
        {
            cardList.Add(PopCard());
        }
        ui_Poker.PokerUIStart();
    }

    public Hand HandCalaulate(List<(CardShape shape, int number)> cardList)
    {
        cardList.Sort((x, y) => y.number.CompareTo(x.number));
        int topCard = cardList[4].number;

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

        Hand returnHand = new Hand(handRank, hiddenRanks.Contains(handRank), topCard);

        return returnHand;
    }

    public Sprite GetSprite((CardShape shape, int number) cardTuple) // Poker패에 맞는 카드 Image 리소스를 반환
    {
        return cardsSpriteList[(int)cardTuple.shape][cardTuple.number];
    }

}