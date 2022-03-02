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
        private int heart;
        private int gold;
        private int chance;
        private int enemyKillCount;
        private int enemyEndPointCount;

        public string HardNess
        {
            get => hardNess;
            private set
            {
                hardNess = value;
                // 순서 주의
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
        public int Heart
        {
            get => heart;
            private set
            {
                heart = value;
                ui_InGameScene.SetHeartText(heart);
                if (heart <= 0)
                    GameOver();
            }
        }
        public int Gold
        {
            get => gold;
            set
            {
                gold = value;
                ui_InGameScene.SetGoldText(gold);
            }
        }
        public int Chance
        {
            get => chance;
            set
            {
                chance = value;
                ui_InGameScene.SetChanceText(chance);
            }
        }
        public int EnemyKillCount // 플레이어에게 죽은 에너미 수
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
        public int EnemyEndPointCount // 엔드포인트에 도달한 에너미 수
        {
            get => enemyEndPointCount;
            set
            {
                if (value != 0) // 라운드 별 초기화가 아닌 실제 에너미가 엔드포인트에 닿을 때
                    Heart--;
                enemyEndPointCount = value;
            }
        } 
        public int EnemyEntireCount { get; private set; } // 라운드 종결 판정

        private bool stateBreak = false;

        List<NewRoundData> roundDataList;
        NewRoundData currentRoundData;
        HardNessData hardNessData;

        private void InitCurrentRound()
        {
            currentRoundData = roundDataList[Round - 1];
            EnemyEntireCount = CountRoundEnemy();
            // 순서 주의
            EnemyKillCount = 0;

            CurrentState = RoundState.READY;
            BreakState();
        }

        public void InitRoundManager(UI_InGameScene ui_InGameScene) // 처음으로 게임이 시작될 때 호출
        {
            /* TODO : 데이터 실체화
             * Round와 HardNess는 MainScene에서 받아옴
             * Round는 초기값이 1이 아닐 수 있음(세이브 데이터)
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

            Heart = hardNessData.heart;
            Gold = hardNessData.gold;
            Chance = hardNessData.chance;
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
            /* ReadyState 일단 기능 없앰
             * 추후 필요에 의하면 사용
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
            // 손패 계산
            Hand roundHand = GameManager.Poker.CalculateMyHand();
            GameManager.Poker.ResetInitialChance(); // 라운드 찬스 초기화
            Debug.Log(roundHand.Rank);
            // 타워 종류 결정
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

        private void GameOver()
        {

        }

        private void GameClear()
        {
            // TODO : 라운드가 더 이상 없을 경우 게임 클리어
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