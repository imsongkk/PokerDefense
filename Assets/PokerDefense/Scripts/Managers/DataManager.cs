using PokerDefense.Data;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using PokerDefense.Towers;
using UnityEngine.Events;

namespace PokerDefense.Managers
{
    public class DataManager
    {
        // List<TowerData> towerDataList;
        PlayerData playerData;

        public PlayerData PlayerData { get; private set; }

        public Dictionary<string, TowerUniqueData> TowerUniqueDataDict { get; private set; }
        public Dictionary<string, TowerUpgradeData> TowerUpgradeDataDict { get; private set; }
        public Dictionary<string, EnemyData> EnemyDataDict { get; private set; }
        public Dictionary<string, HardNessData> HardNessDataDict { get; private set; }
        public Dictionary<int, SkillData> SkillDataDict { get; private set; } = new Dictionary<int, SkillData>(); // key : skillIndex
        public Dictionary<string, int> SkillIndexDict { get; private set; } = new Dictionary<string, int>(); // key : skillName
        public Dictionary<string, string> SystemMessageDict { get; private set; } = new Dictionary<string, string>(); // key : Define.SystemMessage
        public List<string> ShopItemList { get; private set; } = new List<string>();
        public Dictionary<string, GameData> GameDataDict { get; private set; } = new Dictionary<string, GameData>();
        public Dictionary<string, ItemData> ItemDataDict { get; private set; } = new Dictionary<string, ItemData>(); // key : itemId


        public GameData CurrentGameData { get; private set; }
        public SlotData SlotData { get; private set; }

        private string jsonLocation = "Assets/PokerDefense/Data";
        private string towerUniqueDataJsonFileName = "TowerUniqueData";
        private string towerUpgradeDataJsonFileName = "TowerUpgradeData";
        private string hardNessJsonFileName = "HardNessDataDict";
        private string enemyJsonFileName = "EnemyDataDict";
        private string slotJsonFileName = "SlotData";
        private string skilJsonFileName = "SkillData";
        private string systemMessageJsonFileName = "SystemMessageData";
        private string shopJsonFileName = "ShopData";
        private string ItemJsonFileName = "ItemData";
        private string gameDataJsonFileName = "GameData_";

        public void InitDataManager()
        {
            InitPlayerData();            

            InitTowerData();
            InitHardNessDataDict();
            InitGameDataDict();
            InitEnemyDataDict();
            InitSkillDataDict();
            InitSystemMessageDict();
            InitItemDataDict();
            InitShopItemList();
        }

        private void InitPlayerData()
        {

        }

        private void InitTowerData()
        {

            TowerUniqueDataDict = LoadJsonFile<Dictionary<string, TowerUniqueData>>(jsonLocation, towerUniqueDataJsonFileName);
            TowerUpgradeDataDict = LoadJsonFile<Dictionary<string, TowerUpgradeData>>(jsonLocation, towerUpgradeDataJsonFileName);

            //Debug.Log(TowerDataDict);
            //Debug.Log(JsonConvert.SerializeObject(TowerDataDict));
        }

        private void InitEnemyDataDict()
        {
            EnemyDataDict = LoadJsonFile<Dictionary<string, EnemyData>>(jsonLocation, enemyJsonFileName);
        }

        private void InitHardNessDataDict()
        {
            HardNessDataDict = LoadJsonFile<Dictionary<string, HardNessData>>(jsonLocation, hardNessJsonFileName);
        }

        // ^ old rounddata
        // v new rounddata

        private void InitGameDataDict()
        {
            GameDataDict = new Dictionary<string, GameData>();
            //* After HardnessData init
            foreach (var difficulty in HardNessDataDict.Keys)
            {
                GameData gameData = new GameData(difficulty);
                string difficultyFileName = gameDataJsonFileName + difficulty;
                gameData.gameRounds = LoadJsonFile<List<NewRoundData>>(jsonLocation, difficultyFileName);
                GameDataDict.Add(difficulty, gameData);
            }
        }

        public void SelectGameData(string difficulty)
        {
            // difficulty: Easy, Normal, Hard, Crazy
            CurrentGameData = GameDataDict[difficulty];
        }

        private void InitSkillDataDict()
        {
            var skillDataDict = LoadJsonFile<Dictionary<string, SkillData>>(jsonLocation, skilJsonFileName);

            foreach (var skill in skillDataDict)
            {
                SkillDataDict.Add(skill.Value.skillIndex, skill.Value);
                SkillIndexDict.Add(skill.Value.skillName, skill.Value.skillIndex);
            }
        }

        private void InitSystemMessageDict()
        {
            SystemMessageDict = LoadJsonFile<Dictionary<string, string>>(jsonLocation, systemMessageJsonFileName);
        }

        private void InitShopItemList()
        {
            ShopItemList = LoadJsonFile<List<string>>(jsonLocation, shopJsonFileName);
        }

        private void InitItemDataDict()
        {
            var itemList = LoadJsonFile<List<ItemData>>(jsonLocation, ItemJsonFileName);

            foreach(var item in itemList)
                ItemDataDict.Add(item.itemId, item);
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
            newSlotData.stageNumber = GameManager.Round.Round;
            newSlotData.hardNess = GameManager.Round.HardNess;
            newSlotData.inventory = GameManager.Inventory.GetSaveData();

            List<Tower> userTowerList = GameManager.Tower.GetUserTowerList();
            newSlotData.towerSaveDataList = TowerSaveData.ConvertTowerSaveData(userTowerList);

            return newSlotData;
        }

        public SlotData LoadSlotData()
        {
            return null;
        }
    }
}
