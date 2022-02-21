using System;
using System.Collections.Generic;
using static PokerDefense.Utils.Define;

namespace PokerDefense.Data
{
    [Serializable]
    public class EnemyData
    {
        public float moveSpeed;
        public float hp;
        public bool isBoss;
        public int damage;
        public EnemyType enemyType;
    }
}