using PokerDefense.Towers;
using System;
using System.Collections.Generic;
using static PokerDefense.Managers.TowerManager;
using static PokerDefense.Towers.Tower;

namespace PokerDefense.Data
{
    [Serializable]
    public class SlotData
    {
        public string hardNess;
        public int stageNumber;
        public int heart;
        public int gold;
        public int chance;
        public List<TowerSaveData> towerSaveDataList;
    }

    [Serializable]
    public class TowerSaveData
    {
        public string towerName;
        public int attackDamageLevel;
        public int attackSpeedLevel;
        public int attackRangeLevel;
        public int attackCriticalLevel;
        public TowerType towerType;
        public int topCard;
        public int towerIndex;

        // public TowerUniqueData towerData; -> towerName으로 불러오기

        public static List<TowerSaveData> ConvertTowerSaveData(List<Tower> userTowerList)
        {
            List<TowerSaveData> towerSaveDatas = new List<TowerSaveData>();

            foreach (Tower tower in userTowerList)
                towerSaveDatas.Add(ConvertTowerSaveData(tower));

            return towerSaveDatas;
        }

        public static TowerSaveData ConvertTowerSaveData(Tower userTower)
        {
            TowerIndivData towerIndivData = userTower.towerIndivData;

            TowerSaveData towerSaveData = new TowerSaveData();
            towerSaveData.towerName = towerIndivData.TowerName;
            towerSaveData.topCard = towerIndivData.TopCard;
            towerSaveData.towerType = towerIndivData.TowerType;
            towerSaveData.towerIndex = towerIndivData.Index;
            towerSaveData.attackDamageLevel = towerIndivData.DamageLevel;
            towerSaveData.attackSpeedLevel = towerIndivData.SpeedLevel;
            towerSaveData.attackRangeLevel = towerIndivData.RangeLevel;
            towerSaveData.attackCriticalLevel = towerIndivData.CriticalLevel;

            return towerSaveData;
        }
    }
}