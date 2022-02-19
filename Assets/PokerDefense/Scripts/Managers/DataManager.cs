using PokerDefense.Data;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace PokerDefense.Managers
{
    public class DataManager
    {
        // List<TowerData> towerDataList;
        PlayerData playerData;

        public PlayerData PlayerData { get; private set; }
        public Dictionary<string, TowerData> TowerDataDict { get; private set; }
        public List<RoundData> RoundDataList { get; private set; }

        private string jsonLocation = "Assets/PokerDefense/Data";
        private string towerJsonFileName = "TowerDataDict";
        private string roundJsonFileName = "RoundDataList";

        public void InitDataManager()
        {
            InitPlayerData();
            InitTowerDataList();

            MakeTowerDataDict();
        }

        private void InitPlayerData()
        {

        }

        private void InitTowerDataList()
        {
            // TODO : ����� json�� �ҷ��� TowerData�� ������
            // TEST
            // TowerData towerData = new TowerData();
            // towerData.damage = 30;
            // towerData.attackSpeed = 5;
            // towerData.attackRange = 10;
            // towerData.basePrice = 5;
            // towerData.towerName = "HighCard";
            // towerData.rareNess = 1;

            TowerDataDict = LoadJsonFile<Dictionary<string, TowerData>>(jsonLocation, towerJsonFileName);

            Debug.Log(TowerDataDict);
            Debug.Log(JsonConvert.SerializeObject(TowerDataDict));
        }

        private void MakeTowerDataDict()
        {

        }

        private void InitRoundDataList()
        {
            RoundDataList = LoadJsonFile<List<RoundData>>(jsonLocation, roundJsonFileName);

            Debug.Log(RoundDataList);
            Debug.Log(JsonConvert.SerializeObject(RoundDataList));
        }

        private T LoadJsonFile<T>(string loadPath, string fileName)
        {
            FileStream fileStream = new FileStream($"{loadPath}/{fileName}.json", FileMode.Open);
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();

            string jsonData = Encoding.UTF8.GetString(data);
            return JsonToObject<T>(jsonData);
        }

        private T JsonToObject<T>(string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}
