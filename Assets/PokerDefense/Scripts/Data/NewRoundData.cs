using System;
using System.Collections.Generic;

namespace PokerDefense.Data
{
    [Serializable]
    public class NewRoundData
    {
        public struct EnemySpawn
        {
            public string enemyName;
            public int enemyNumber;
            public float spawnCycle;
        }

        public List<EnemySpawn> enemyList;
        public bool popup;
        public int bonus;
    }
}