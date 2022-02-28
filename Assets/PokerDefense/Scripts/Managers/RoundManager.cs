using PokerDefense.Data;
using PokerDefense.UI.Popup;
using PokerDefense.UI.Scene;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

        public List<Enemy> CurrentEnemies
        {
            get => enemyGroup.GetComponentsInChildren<Enemy>().ToList();
        }

        Transform startPoint, endPoint;
        Transform wayPointParent;
        Transform enemyGroup;

        [SerializeField]
        List<Transform> wayPoints = new List<Transform>();

        UI_InGameScene ui_InGameScene;

        public event EventHandler RoundStarted, RoundFinished;

        private int round;
        private string hardNess;
        private int heart;
        private int gold;
        private int chance;
        private int horse;

        public int Round
        {
            get { return round; }
            set
            {
                round = value;
                ui_InGameScene.SetRoundText(round);
            }
        }
        public string HardNess
        {
            get { return hardNess; }
            set
            {
                hardNess = value;
            }
        }
        public int Heart
        {
            get { return heart; }
            set
            {
                heart = value;
                ui_InGameScene.SetHeartText(heart);
            }
        }
        public int Gold
        {
            get { return gold; }
            set
            {
                gold = value;
                ui_InGameScene.SetGoldText(gold);
            }
        }
        public int Chance
        {
            get { return chance; }
            set
            {
                chance = value;
                ui_InGameScene.SetChanceText(chance);
            }
        }
        public int Horse
        {
            get { return horse; }
            set
            {
                horse = value;
                ui_InGameScene.SetHorseIndex(horse);
            }
        }

        private float timeTowerSetLimit = 8f;
        private float timeLeft = 0;

        private bool stateBreak = false;
        private bool timerBreak = false;        // false일때만 State 타이머가 흐르게 됨

        int enemyKillCount = 0;

        EnemyData currentRoundEnemyData;
        RoundData roundData;
        HardNessData hardNessData;

        IEnumerator EnemySpawnCoroutine;
        bool isStoppedEnemySpawn = false;
        float? lastSpawnTime = null;
        float? interruptedSpawnTime = null;

        private void ChangeRound() // Round가 바뀔 때 마다 해줘야 하는 작업들
        {
            CurrentState = RoundState.READY;
            Round = 2;
            // TODO : currentRoundEnemyData, roundData update
            ui_InGameScene.SetRoundText(Round);
            ui_InGameScene.SetDiedEnemyCountText(enemyKillCount, roundData.count);
            enemyKillCount = 0;
        }

        public void InitRoundManager() // 처음으로 게임이 시작될 때 호출
        {
            /* TODO : 데이터 실체화
             * Round와 HardNess는 MainScene에서 받아옴
             */
            round = 1;
            HardNess = "Easy";

            // 순서 주의
            InitWayPoints();
            InitHardNessData();
            InitRoundData();
            InitEnemyData();

            StartCoroutine(InitUIText());

            EnemySpawnCoroutine = SpawnCurrentRoundEnemy();

            GameManager.Data.SkillIndexDict.TryGetValue("TimeStop", out var timeStopSkillIndex);
            GameManager.Skill.skillStarted[timeStopSkillIndex].AddListener((a, b) => { isStoppedEnemySpawn = true; interruptedSpawnTime = Time.time; });
            GameManager.Skill.skillFinished[timeStopSkillIndex].AddListener(() => { isStoppedEnemySpawn = false; });

            CurrentState = RoundState.READY;
        }

        public void InitWayPoints()
        {
            wayPointParent = GameObject.FindGameObjectWithTag("WayPointParent").transform;
            foreach (Transform child in wayPointParent)
            {
                wayPoints.Add(child);
            }

            startPoint = wayPoints[0];
            endPoint = wayPoints[wayPoints.Count - 1];

            Enemy.wayPoints = this.wayPoints;
            Enemy.endPoint = this.endPoint;

            enemyGroup = GameObject.FindGameObjectWithTag("EnemyGroup").transform;
        }

        private void InitHardNessData()
        {
            GameManager.Data.HardNessDataDict.TryGetValue(HardNess, out hardNessData);

            heart = hardNessData.heart;
            gold = hardNessData.gold;
            chance = hardNessData.chance;
        }

        private void InitRoundData()
        {
            Dictionary<string, RoundData> dict = new Dictionary<string, RoundData>();
            GameManager.Data.RoundDataDict.TryGetValue(HardNess, out dict);
            dict.TryGetValue(Round.ToString(), out roundData);
        }

        private void InitEnemyData()
        {
            GameManager.Data.EnemyDataDict.TryGetValue(roundData.enemyName, out currentRoundEnemyData);
        }

        IEnumerator InitUIText()
        {
            yield return new WaitUntil(() => ui_InGameScene != null);
            ui_InGameScene.InitText(Round, Heart, Gold, Chance);
        }

        private void Update()
        {
            // Debug.Log(state.ToString());
            switch (state)
            {
                case RoundState.NONE:
                    if (stateChanged) { stateChanged = false; }
                    break;
                case RoundState.READY:
                    if (stateChanged) { ReadyStateStart(); }
                    //TODO  Ready Animaiton
                    timeLeft -= Time.deltaTime;
                    if (timeLeft <= 0) CurrentState = RoundState.TOWER;
                    break;
                case RoundState.TOWER:
                    if (stateChanged) { TowerStateStart(); }
                    //set position or wait 2min
                    if (!timerBreak) { timeLeft -= Time.deltaTime; }
                    if (stateBreak)
                    {
                        CurrentState = RoundState.POKER;
                        stateBreak = false;
                        break;
                    }
                    if (timeLeft <= 0) CurrentState = RoundState.PLAY;
                    break;
                case RoundState.POKER:
                    if (stateChanged) { PokerStateStart(); }
                    if (stateBreak)
                    {
                        PokerConfirm();
                        CurrentState = RoundState.HORSE;
                        stateBreak = false;
                        break;
                    }
                    break;
                case RoundState.HORSE:
                    if (stateChanged) { HorseStateStart(); }
                    if (stateBreak)
                    {
                        CurrentState = RoundState.PLAY;
                        stateBreak = false;
                        break;
                    }
                    break;
                case RoundState.PLAY:
                    if (stateChanged)
                    {
                        PlayStateStart();
                        StartCoroutine(EnemySpawnCoroutine);
                    }
                    break;
            }
        }

        public void BreakState()
        {
            stateBreak = true;
        }

        public void BreakTimer(bool b)
        {
            timerBreak = b;
        }

        private void ReadyStateStart()
        {
            GameManager.SystemText.SetSystemMessage(SystemMessage.ReadyStateStart);
            Debug.Log(state.ToString());
            timeLeft = 2f;
            stateChanged = false;
        }

        private void TowerStateStart()
        {
            GameManager.SystemText.SetSystemMessage(SystemMessage.TowerStateStart);
            Debug.Log(state.ToString());
            timeLeft = timeTowerSetLimit;
            stateChanged = false;
        }

        private void PokerStateStart()
        {
            GameManager.SystemText.SetSystemMessage(SystemMessage.PokerStateStart);
            Debug.Log(state.ToString());
            GameManager.UI.ShowPopupUI<UI_Poker>();
            GameManager.Poker.PokerStart();
            stateChanged = false;
        }

        private void HorseStateStart()
        {
            GameManager.SystemText.SetSystemMessage(SystemMessage.HorseStateStart);
            Debug.Log(state.ToString());
            ui_InGameScene.ActivateBottomUI();
            GameManager.UI.ShowPopupUI<UI_HorseSelectPopup>();
            stateChanged = false;
        }

        private void PlayStateStart()
        {
            GameManager.SystemText.SetSystemMessage(SystemMessage.PlayStateStart);
            Debug.Log(state.ToString());
            RoundStarted?.Invoke(this, null);
            stateChanged = false;
        }

        private void PokerConfirm()
        {
            //TODO 손패 계산
            Hand roundHand = GameManager.Poker.CalculateMyHand();
            GameManager.Poker.ResetInitialChance(); // 라운드 찬스 초기화
            Debug.Log(roundHand.Rank);
            //TODO 타워 종류 결정
            GameManager.Tower.ConfirmTower(roundHand);
        }

        IEnumerator SpawnCurrentRoundEnemy()
        {
            int remainEnemyCount = roundData.count;
            string currentEnemyName = roundData.enemyName;
            float spawnCycle = roundData.spawnCycle;

            WaitForSeconds spawnDelay = new WaitForSeconds(spawnCycle);

            while (remainEnemyCount > 0)
            {
                // TimeStop Skill 때문에 작성됨
                if (isStoppedEnemySpawn)
                    yield return new WaitUntil(() => isStoppedEnemySpawn == false);
                if (interruptedSpawnTime != null)
                {
                    yield return new WaitForSeconds(spawnCycle - (interruptedSpawnTime.Value - lastSpawnTime.Value));
                    interruptedSpawnTime = null;
                }

                GameObject enemyObject = GameManager.Resource.Instantiate($"Enemy/{currentEnemyName}", enemyGroup);
                enemyObject.transform.position = startPoint.position;

                Enemy enemy = enemyObject.GetComponent<Enemy>();
                enemy.InitEnemy(currentEnemyName, currentRoundEnemyData);

                remainEnemyCount--;
                lastSpawnTime = Time.time;
                yield return spawnDelay;
            }
            yield break;
        }

        public List<Enemy> GetEnemyInRange(Vector2 screenSpaceRangeOffset, float range)
        {
            // range * 100이 screen상의 길이
            float pixelsPerUnitInScreenSpace = Utils.Util.GetPixelsPerUnitInScreenSpace();
            List<Enemy> ret = new List<Enemy>();
            for(int i=0; i<enemyGroup.childCount; i++)
            {
                Enemy enemy = enemyGroup.GetChild(i).GetComponent<Enemy>();
                Vector2 enemyScreenPos = Camera.main.WorldToScreenPoint(enemy.transform.position);
                float distance = Vector2.Distance(screenSpaceRangeOffset, enemyScreenPos);

                if (distance <= range * pixelsPerUnitInScreenSpace) // TODO : 왜 100을 곱해줘야할까 PixelsPerUnit도 아닌데 뭐지
                    ret.Add(enemy);
            }
            return ret;
        }

        public void OnEnemyGetEndPoint()
        {
            OnEnemyDied();
            Heart -= 1;
            if (Heart <= 0)
            {
                GameOver();
                return;
            }
            ui_InGameScene.SetHeartText(Heart);
        }

        public void OnEnemyDied()
        {
            enemyKillCount++;
            ui_InGameScene.SetDiedEnemyCountText(enemyKillCount, roundData.count);
            if (enemyKillCount == roundData.count)
            {
                RoundClear();
            }
        }

        private void RoundClear()
        {
            Debug.Log("Round Clear!");
            GameManager.Horse.InterruptRace(ChangeRound);
        }


        private void GameOver()
        {

        }

        public void SetUIIngameScene(UI_InGameScene target)
        {
            ui_InGameScene = target;
            ui_InGameScene.SetDiedEnemyCountText(enemyKillCount, roundData.count);
        }
    }
}