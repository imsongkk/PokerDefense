using System;

namespace PokerDefense.Data
{
    //* 손패의 족보에 따라 결정되는 불변한 수치
    [Serializable]
    public class TowerUniqueData
    {
        public int basePrice; // 기본 가격.
        public bool isHidden;   // 히든 여부
        public int rareNess;      // 수가 클 수록 높은 희귀도
    }
}