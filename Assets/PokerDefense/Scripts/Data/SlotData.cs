using System;
using System.Collections.Generic;

namespace PokerDefense.Data
{
    [Serializable]
    public class SlotData
    {
        public string hardNess;
        public int stageNumber;
        public int heart;
        public int gold;
        public int diamond;
        public List<TowerSaveData> towerSaveDataList;
    }

    [Serializable]
    public class TowerSaveData
    {
        public string towerName;
        public string towerLevel;
        public int xIndex;
        public int yIndex;
    }
}