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
        public int maxTargetAmount;     // 한번에 때릴 수 있는 적 수(전체 공격: -1)
        public bool isProjectile;        // 투사체 공격 여부
        public bool isCushion;          // 쿠션 공격 여부
    }
}