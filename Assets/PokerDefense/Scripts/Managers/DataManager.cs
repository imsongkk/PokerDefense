using PokerDefense.Data;
using System.Collections.Generic;

namespace PokerDefense.Managers
{
    public class DataManager
    {
        List<TowerData> towerDataList;
        PlayerData playerData;

        public PlayerData PlayerData { get; private set; }
        public Dictionary<string, TowerData> TowerDataDict { get; private set; } = new Dictionary<string, TowerData>(); // key : 타워 패에 맞는 고유 id


        public void InitDataManager()
        {
            InitPlayerData();
            InitTowerDataList();

            MakeTowerDataDict();
        }

        private void InitPlayerData()
        {
            // TODO : 저장된 json을 불러와 PlayerData 초기화
        }

        private void InitTowerDataList()
        {
            // TODO : 저장된 json을 불러와 TowerData에 입히기
        }

        private void MakeTowerDataDict()
        {

        }
    }
}
