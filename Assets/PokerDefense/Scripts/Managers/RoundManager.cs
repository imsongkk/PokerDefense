using PokerDefense.Data;
using PokerDefense.UI.Popup;
using PokerDefense.UI.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static PokerDefense.Utils.Define;

namespace PokerDefense.Managers
{
    public class RoundManager : MonoBehaviour
    {
        public enum RoundState
        {
            NONE,
            STOP,
            READY,
            TOWER,
            POKER,
            HORSE,
            PLAY
        }

        private RoundState state = RoundState.NONE;
        private bool stateChanged = false;

        public RoundState CurrentState
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    state = value;
                    stateChanged = true;
                }
            }
        }

        Transform startPoint, endPoint;
        Transform wayPointParent;
        Transform enemyGroup;

        [SerializeField]
        List<Transform> wayPoints = new List<Transform>();

        UI_InGameScene ui_InGameScene;

        public UnityEvent RoundStarted = new UnityEvent();
        public UnityEvent RoundFinished = new UnityEvent();

        private string hardNess;
        private int round;
        private int enemyKillCount;
        private int enemyEndPointCount;

        public string HardNess
        {
            get => hardNess;
            private set
            {
                hardNess = value;
                // μμ μ£Όμ
                InitHardNessData();
                InitGameRoundData();
            }
        }
        public int Round
        {
            get => round;
            private set
            {
                round = value;
                if (round == roundDataList.Count + 1)
                    GameClear();
                ui_InGameScene.SetCurrentRoundCountText(round, roundDataList.Count);
                //ui_InGameScene.SetRoundText(round);
                InitCurrentRound();
            }
        }
        public int EnemyKillCount // νλ μ΄μ΄μκ² μ£½μ μλλ―Έ μ
        {
            get => enemyKillCount;
            set
            {
                enemyKillCount = value;
                ui_InGameScene.SetDiedEnemyCountText(EnemyKillCount, EnemyEntireCount);
                if (EnemyKillCount >= EnemyEntireCount)
                    RoundClear();
            }
        }
        public int EnemyEndPointCount // μλν¬μΈνΈμ λλ¬ν μλλ―Έ μ
        {
            get => enemyEndPointCount;
            set
            {
                if (value != 0) // λΌμ΄λ λ³ μ΄κΈ°νκ° μλ μ€μ  μλλ―Έκ° μλν¬μΈνΈμ λΏμ λ
                    GameManager.Inventory.Heart--;
                enemyEndPointCount = value;
            }
        } 
        public int EnemyEntireCount { get; private set; } // λΌμ΄λ μ’κ²° νμ 

        private bool stateBreak = false;

        List<NewRoundData> roundDataList;
        NewRoundData currentRoundData;
        HardNessData hardNessData;

        private void InitCurrentRound()
        {
            currentRoundData = roundDataList[Round - 1];
            EnemyEntireCount = CountRoundEnemy();
            // μμ μ£Όμ
            EnemyKillCount = 0;

            CurrentState = RoundState.READY;
            BreakState();
        }

        public void InitRoundManager(UI_InGameScene ui_InGameScene) // μ²μμΌλ‘ κ²μμ΄ μμλ  λ νΈμΆ
        {
            /* TODO : λ°μ΄ν° μ€μ²΄ν
             * Roundμ HardNessλ MainSceneμμ λ°μμ΄
             * Roundλ μ΄κΈ°κ°μ΄ 1μ΄ μλ μ μμ(μΈμ΄λΈ λ°μ΄ν°)
             */
            this.ui_InGameScene = ui_InGameScene;

            InitWayPoints();
            InitTimeStopSkill();
            InitEnemySpawner();
            InitTimer();

            HardNess = "Easy";
            Round = 1;
        }

        public void InitWayPoints()
        {
            wayPointParent = GameObject.FindGameObjectWithTag("WayPointParent").transform;
            foreach (Transform child in wayPointParent)
                wayPoints.Add(child);

            startPoint = wayPoints[0];
            endPoint = wayPoints[wayPoints.Count - 1];

            Enemy.wayPoints = this.wayPoints;
            Enemy.endPoint = this.endPoint;

            enemyGroup = GameObject.FindGameObjectWithTag("EnemyGroup").transform;
        }

        private void InitHardNessData()
        {
            GameManager.Data.HardNessDataDict.TryGetValue(HardNess, out hardNessData);
        }

        private void InitGameRoundData()
        {
            GameManager.Data.SelectGameData(HardNess);
            roundDataList = GameManager.Data.CurrentGameData.gameRounds;
        }

        private void InitTimeStopSkill()
        {
            GameManager.Data.SkillIndexDict.TryGetValue("TimeStop", out var timeStopSkillIndex);
            GameManager.Skill.skillStarted[timeStopSkillIndex].AddListener((a, b) => { isStoppedEnemySpawn = true; interruptedSpawnTime = Time.time; });
            GameManager.Skill.skillFinished[timeStopSkillIndex].AddListener(() => { isStoppedEnemySpawn = false; });
        }

        private void InitEnemySpawner()
        {
            EnemySpawnCoroutine = SpawnRoundEnemy();
            RoundStarted.AddListener(() => StartCoroutine(EnemySpawnCoroutine));
        }

        private void InitTimer()
        {
            TimerCoroutine = Timer();
            RoundStarted.AddListener(() => StartCoroutine(TimerCoroutine));
            RoundFinished.AddListener(() =>
            {
                StopCoroutine(TimerCoroutine);
                ui_InGameScene.SetElapsedTimeCountText(0);
            });
        }

        private void Update()
        {
            switch (state)
            {
                case RoundState.NONE:
                    if (stateChanged) { stateChanged = false; }
                    break;
                case RoundState.READY:
                    if (stateChanged) { ReadyStateStart(); }
                    if(stateBreak)
                    {
                        CurrentState = RoundState.TOWER;
                        stateBreak = false;
                    }
                    break;
                case RoundState.TOWER:
                    if (stateChanged) { TowerStateStart(); }
                    if (stateBreak)
                    {
                        CurrentState = RoundState.POKER;
                        stateBreak = false;
                    }
                    break;
                case RoundState.POKER:
                    if (stateChanged) { PokerStateStart(); }
                    if (stateBreak)
                    {
                        PokerConfirm();
                        CurrentState = RoundState.HORSE;
                        stateBreak = false;
                    }
                    break;
                case RoundState.HORSE:
                    if (stateChanged) { HorseStateStart(); }
                    if (stateBreak)
                    {
                        CurrentState = RoundState.PLAY;
                        stateBreak = false;
                    }
                    break;
                case RoundState.PLAY:
                    if (stateChanged) { PlayStateStart(); }
                    if (stateBreak)
                    {
                        Round++;
                        stateBreak = false;
                    }
                    break;
            }
        }

        public void BreakState()
        {
            stateBreak = true;
        }

        private void ReadyStateStart()
        {
            /* ReadyState μΌλ¨ κΈ°λ₯ μμ°
             * μΆν νμμ μνλ©΄ μ¬μ©
             */
            
            //GameManager.SystemText.SetSystemMessage(SystemMessage.ReadyStateStart);
            BreakState();
            stateChanged = false;
        }

        private void TowerStateStart()
        {
            GameManager.SystemText.SetSystemMessage(SystemMessage.TowerStateStart);
            stateChanged = false;
        }

        private void PokerStateStart()
        {
            GameManager.SystemText.SetSystemMessage(SystemMessage.PokerStateStart);
            GameManager.Poker.PokerStart();
            GameManager.UI.ShowPopupUI<UI_Poker>();
            stateChanged = false;
        }

        private void HorseStateStart()
        {
            GameManager.SystemText.SetSystemMessage(SystemMessage.HorseStateStart);
            ui_InGameScene.ActivateBottomUI();
            GameManager.UI.ShowPopupUI<UI_HorseSelectPopup>();
            stateChanged = false;
        }

        private void PlayStateStart()
        {
            GameManager.SystemText.SetSystemMessage(SystemMessage.PlayStateStart);
            RoundStarted?.Invoke();
            stateChanged = false;
        }

        private void PokerConfirm()
        {
            // μν¨ κ³μ°
            Hand roundHand = GameManager.Poker.CalculateMyHand();
            GameManager.Poker.ResetInitialChance(); // λΌμ΄λ μ°¬μ€ μ΄κΈ°ν
            Debug.Log(roundHand.Rank);
            // νμ μ’λ₯ κ²°μ 
            GameManager.Tower.ConfirmTower(roundHand);
        }

        private int CountRoundEnemy()
        {
            int enemyCount = 0;
            foreach (var spawn in currentRoundData.enemyList)
                enemyCount += spawn.enemyNumber;
            return enemyCount;
        }

        IEnumerator EnemySpawnCoroutine;
        bool isStoppedEnemySpawn = false;
        float? lastSpawnTime = null;
        float? interruptedSpawnTime = null;

        IEnumerator SpawnRoundEnemy()
        {
            List<NewRoundData.EnemySpawn> enemyList = currentRoundData.enemyList;

            string currentEnemyName;
            int remainEnemyNumber = 0;
            WaitForSeconds spawnDelay;
            GameObject enemyPrefab;
            EnemyData currentEnemyData;

            foreach (var spawn in enemyList)
            {
                currentEnemyName = spawn.enemyName;
                remainEnemyNumber = spawn.enemyNumber;
                spawnDelay = new WaitForSeconds(spawn.spawnCycle);
                enemyPrefab = GameManager.Resource.Load<GameObject>($"Prefabs/Enemy/{currentEnemyName}");

                while (remainEnemyNumber > 0)
                {
                    if (isStoppedEnemySpawn)
                        yield return new WaitUntil(() => isStoppedEnemySpawn == false);
                    if (interruptedSpawnTime != null)
                    {
                        yield return new WaitForSeconds(spawn.spawnCycle - (interruptedSpawnTime.Value - lastSpawnTime.Value));
                        interruptedSpawnTime = null;
                    }
                    lastSpawnTime = Time.time;

                    GameObject enemyObject = Instantiate(enemyPrefab, startPoint.position, Quaternion.identity, enemyGroup);
                    Enemy enemy = enemyObject.GetComponent<Enemy>();
                    GameManager.Data.EnemyDataDict.TryGetValue(currentEnemyName, out currentEnemyData);
                    enemy.InitEnemy(currentEnemyName, currentEnemyData);

                    remainEnemyNumber--;
                    yield return spawnDelay;
                }
                yield return null;
            }
            yield break;
        }

        public List<Enemy> GetEnemyInRange(Vector2 screenSpaceRangeOffset, float range)
        {
            List<Enemy> ret = new List<Enemy>();
            for (int i = 0; i < enemyGroup.childCount; i++)
            {
                Enemy enemy = enemyGroup.GetChild(i).GetComponent<Enemy>();
                if(enemy.IsInRange(screenSpaceRangeOffset, range))
                    ret.Add(enemy);
            }
            return ret;
        }

        private void RoundClear()
        {
            Debug.Log("Round Clear!");
            RoundFinished?.Invoke();

            Round++;
        }

        public void GameOver()
        {

        }

        private void GameClear()
        {
            // TODO : λΌμ΄λκ° λ μ΄μ μμ κ²½μ° κ²μ ν΄λ¦¬μ΄
        }

        public void OnClickReadyButton()
        {
            if (CurrentState == RoundState.PLAY)
                return;
            CurrentState = RoundState.PLAY;
            stateChanged = true;
        }


        IEnumerator TimerCoroutine;

        IEnumerator Timer()
        {
            int elapsedTime = 0;
            WaitForSeconds timeDelay = new WaitForSeconds(1f);
            while(true)
            {
                elapsedTime++;
                yield return timeDelay;
                ui_InGameScene.SetElapsedTimeCountText(elapsedTime);
            }
        }
    }
}