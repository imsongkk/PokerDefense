using System;
using System.Collections.Generic;

namespace PokerDefense.Data
{
    [Serializable]
    public class GameData
    {
        public string difficulty;
        public List<NewRoundData> gameRounds;

        public GameData(string difficulty)
        {
            this.difficulty = difficulty;
        }
    }
}