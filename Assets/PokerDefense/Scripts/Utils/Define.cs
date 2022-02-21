using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PokerDefense.Managers.TowerManager;

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

        public enum EnemyType
        {
            Small,
            Middle,
            Large,
        }

        public enum CardShape
        {
            Spade,      // 0
            Heart,      // 1
            Diamond,    // 2
            Clover,     // 3
            Joker,      // 4
            Null        // 5
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

        public static float CalculateDamage(TowerType towerType, EnemyType enemyType, float originDamage)
        {
            float calculatedDamage = originDamage;
            // TODO : towerType과 enemyType에 따라 damage 증감식 적용
            return calculatedDamage;
        }
    }
}
