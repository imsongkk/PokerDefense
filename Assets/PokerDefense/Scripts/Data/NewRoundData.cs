using System;
using System.Collections.Generic;

namespace PokerDefense.Data
{
    [Serializable]
    public class NewRoundData
    {
        public Dictionary<string, int> enemyDict;
        public bool popup;
        public int bonus;
    }
}