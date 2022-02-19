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
        public Dictionary<string, EnemyData> EnemyDataDict { get; private set; }
        public Dictionary<string, HardNessData> HardNessDataDict { get; private set; }
        public Dictionary<string, Dictionary<string, RoundData>> RoundDataDict { get; private set; } // outter key : 난이도, inner key : stage number

        private string jsonLocation = "Assets/PokerDefense/Data";
        private string towerJsonFileName = "TowerDataDict";
        private string roundJsonFileName = "RoundDataDict";
        private string hardNessJsonFileName = "HardNessDataDict";
        private string enemyJsonFileName = "EnemyDataDict";
        private string slotJsonFileName = "SlotData";

        public void InitDataManager()
        {
            InitPlayerData();

            InitTowerDataDict();
            InitRoundDataDict();
            InitEnemyDataDict();
            InitHardNessDataDict();
        }

        private void InitPlayerData()
        {

        }

        private void InitTowerDataDict()
        {

            TowerDataDict = LoadJsonFile<Dictionary<string, TowerData>>(jsonLocation, towerJsonFileName);

            //Debug.Log(TowerDataDict);
            //Debug.Log(JsonConvert.SerializeObject(TowerDataDict));
        }

        private void InitRoundDataDict()
        {
            RoundDataDict = LoadJsonFile<Dictionary<string, Dictionary<string, RoundData>>>(jsonLocation, roundJsonFileName);

            //Debug.Log(RoundDataList);
            //Debug.Log(RoundDataList["Easy"]["1"].enemyName);
            //Debug.Log(RoundDataList["Normal"]["1"].enemyName);
        }

        private void InitEnemyDataDict()
        {
            EnemyDataDict = LoadJsonFile<Dictionary<string, EnemyData>>(jsonLocation, enemyJsonFileName);
        }

        private void InitHardNessDataDict()
        {
            HardNessDataDict = LoadJsonFile<Dictionary<string, HardNessData>>(jsonLocation, hardNessJsonFileName);
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

        private void CreateJsonFile(string createPath, string fileName, string jsonData)
        {
            FileStream fileStream = new FileStream($"{createPath}/{fileName}.json", FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(jsonData);
            fileStream.Write(data, 0, data.Length);
            fileStream.Close();
        }

        public void SaveSlotData()
        {
            SlotData newData = MakeSlotData();
            string slotDataJson = JsonConvert.SerializeObject(newData, Formatting.Indented);
            CreateJsonFile(jsonLocation, slotJsonFileName, slotDataJson);
        }

        private SlotData MakeSlotData()
        {
            SlotData newSlotData = new SlotData();
            newSlotData.heart = GameManager.Round.Heart;
            newSlotData.gold = GameManager.Round.Gold;
            newSlotData.stageNumber = GameManager.Round.Round;
            newSlotData.hardNess = GameManager.Round.HardNess;

            return newSlotData;
        }

        public SlotData LoadSlotData()
        {
            return null;
        }
    }
}
