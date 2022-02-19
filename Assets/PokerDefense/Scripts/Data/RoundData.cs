using System;
using System.Collections.Generic;

namespace PokerDefense.Data
{
    [Serializable]
    public class RoundData
    {
        public int stageNumber;
        public Dictionary<string, int> enemyDictionary;
    }
}