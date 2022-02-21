using PokerDefense.Towers;
using System;
using System.Collections.Generic;
using static PokerDefense.Managers.TowerManager;

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
        //public int towerLevel;
        public int towerIndex;
        public TowerData towerData;
        public TowerType towerType;
        public int topCard;

        public static List<TowerSaveData> ConvertTowerSaveData(List<Tower> userTowerList)
        {
            List<TowerSaveData> towerSaveDatas = new List<TowerSaveData>();

            foreach (Tower tower in userTowerList)
                towerSaveDatas.Add(ConvertTowerSaveData(tower));

            return towerSaveDatas;
        }

        public static TowerSaveData ConvertTowerSaveData(Tower userTower)
        {
            TowerIndivData towerIndivData = userTower.TowerIndivData;

            TowerData towerData = new TowerData();
            towerData.damage = towerIndivData.Damage;
            towerData.isHidden = towerIndivData.IsHidden;
            towerData.rareNess = towerIndivData.RareNess;
            towerData.attackRange = towerIndivData.Range;
            towerData.attackSpeed = towerIndivData.Speed;
            towerData.basePrice = towerIndivData.Price;

            TowerSaveData towerSaveData = new TowerSaveData();
            towerSaveData.towerName = towerIndivData.TowerName;
            towerSaveData.topCard = towerIndivData.TopCard;
            towerSaveData.towerType = towerIndivData.TowerType;
            towerSaveData.towerIndex = towerIndivData.Index;
            towerSaveData.towerData = towerData;

            return towerSaveData;
        }
    }
}