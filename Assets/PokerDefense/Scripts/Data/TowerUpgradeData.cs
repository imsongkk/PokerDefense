using System;
using System.Collections.Generic;

namespace PokerDefense.Data
{
    [Serializable]
    public class TowerUpgradeData
    {
        public List<float> attackDamageTable;
        public List<float> attackSpeedTable;
        public List<float> attackRangeTable;
        public List<float> attackCriticalTable;
    }
}