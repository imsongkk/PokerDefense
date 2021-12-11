using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokerDefense.Utils
{
    public class Define
    {
        public enum Scene
        {
            Unknown,
            Loading,
            Main,
            InGame,
        }

        public enum UIEvent
        {
            Click,
            Drag,
        }

        public enum MouseEvent
        {
            Press,
            Click,
            Drag,
            PointerDown,
            PointerUp
        }

        public enum AttackType
        {
            AD,
            AP,
        }

        public enum CardShape
        {
            Spade,      // 0
            Heart,      // 1
            Diamond,    // 2
            Clover,     // 3
            Joker       // 4
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

        public struct Hand
        {
            private HandRank handRank;  // 패의 종류
            private bool isHidden;      // 히든 패인지 여부
            private int topCard;        // 가장 높은 숫자
            private CardShape topShape;       // 탑카드의 문양

            public HandRank Rank { get { return handRank; } }
            public bool IsHidden { get { return isHidden; } }
            public int TopCard { get { return topCard; } }
            public CardShape TopShape { get { return topShape; } }

            public Hand(HandRank handRank, bool isHidden, int topCard, CardShape topShape)
            {
                this.handRank = handRank;
                this.isHidden = isHidden;
                this.topCard = topCard;
                this.topShape = topShape;
            }
        }
    }
}
