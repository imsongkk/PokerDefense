using PokerDefense.Data;
using System.Collections.Generic;

namespace PokerDefense.Managers
{
    public class DataManager
    {
        List<TowerData> towerDataList;
        PlayerData playerData;

        public PlayerData PlayerData { get; private set; }
        public Dictionary<string, TowerData> TowerDataDict { get; private set; } = new Dictionary<string, TowerData>(); // key : Ÿ�� �п� �´� ���� id


        public void InitDataManager()
        {
            InitPlayerData();
            InitTowerDataList();

            MakeTowerDataDict();
        }

        private void InitPlayerData()
        {
            // TODO : ����� json�� �ҷ��� PlayerData �ʱ�ȭ
        }

        private void InitTowerDataList()
        {
            // TODO : ����� json�� �ҷ��� TowerData�� ������
        }

        private void MakeTowerDataDict()
        {

        }
    }
}
