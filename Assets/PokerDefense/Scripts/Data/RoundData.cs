using System;
using System.Collections.Generic;

namespace PokerDefense.Data
{
    [Serializable]
    public class RoundData
    {
        public string enemyName;
        public float spawnCycle;
        public int count;
        public bool popup;
        public int bonus;
    }
}