using PokerDefense.Managers;
using PokerDefense.Utils;
using PokerDefense.UI.Scene;
using System;
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
        Devil,
        OnePair,
        TwoPair,
        Tripple,
        Straight,
        BackStraight,
        RoyalStraight,
        Flush,
        FullHouse,
        FourCards,
        FiveCards,
        StraightFlush,
        BackStraightFlush,
        RoyalStraightFlush,
        Jackpot
    }

    public (CardShape, int)[] cardList = new (CardShape, int)[6];

    public HandRank[] hiddenRanks = {
        HandRank.Devil,
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
    }

    public Hand HandCalaulate((CardShape, int)[] cardList)
    {
        //TODO cardList의 카드를 읽어 어떤 패인지 계산하고 해당하는 Hand 구조체를 반환
    }
}