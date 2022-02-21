using System;
using System.Collections.Generic;

namespace PokerDefense.Data
{
    [Serializable]
    public class EnemyData
    {
        public float moveSpeed;
        public float hp;
        public bool isBoss;
        public int damage;
    }
}