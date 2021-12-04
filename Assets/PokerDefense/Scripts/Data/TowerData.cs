using System;

namespace PokerDefense.Data
{
    [Serializable]
    public class TowerData
    {
        public float damage;
        public float attackSpeed;
        public float attackRange;
        public int price; // 되팔기 가격
        public string towerName; // 요걸로 sprite도 가져오기
        public string towerType;
    }
}