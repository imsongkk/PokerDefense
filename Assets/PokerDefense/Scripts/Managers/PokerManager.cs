using PokerDefense.Managers;
using PokerDefense.Utils;
using PokerDefense.UI.Scene;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// 포커 룰: https://silk-ornament-d81.notion.site/Z7JCard-3825581ae9b442229ae51e410bf6fdb4
public class PokerManager : MonoBehaviour
{
    public enum CardShape
    {
        Spade,
        Heart,
        Clover,
        Diamond,
        Joker
    }

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

    //숫자: 1~8, Z는 8로 취급하여 저장
    public List<(CardShape shape, int number)> cardList = new List<(CardShape shape, int number)>();

    public List<HandRank> hiddenRanks = new List<HandRank>
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

    public Hand HandCalaulate(List<(CardShape shape, int number)> cardList)
    {
        cardList.Sort((x, y) => y.number.CompareTo(x.number));
        int topCard = cardList[4].number;

        List<int> sameNumbers = new List<int>();
        int sameNumber = 1;
        bool isFlush = true;
        bool isStraight = true;
        bool isBackStraight = false;

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
                    if ((i == 4) && isStraight && (cardList[i - 1].number == 4) && (cardList[i].number == 8))
                    {
                        isBackStraight = true;      // Back Straight
                    }
                    else isStraight = false;
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
                if (isBackStraight) handRank = HandRank.BackStraightFlush;
                else if (topCard == 8) handRank = HandRank.RoyalStraightFlush;
                else handRank = HandRank.StraightFlush;
            }
            else if (isStraight)
            {
                if (isBackStraight) handRank = HandRank.BackStraight;
                else if (topCard == 8) handRank = HandRank.RoyalStraight;
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
}