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

        public event EventHandler RoundStarted, RoundFinished;

        private int round;
        private string hardNess;
        private int heart;
        private int gold;
        private int chance;

        public int Round
        {
            get { return round; }
            set
            {
                round = value;
                ui_InGameScene.SetRoundText(round);
                //TODO 라운드 변경 시 작용
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

        private float timeTowerSetLimit = 8f;
        private float timeLeft = 0;

        private bool stateBreak = false;
        private bool timerBreak = false;        // false일때만 State 타이머가 흐르게 됨

        int enemyKillCount = 0;

        EnemyData currentRoundEnemyData;
        RoundData roundData;
        HardNessData hardNessData;

        private void OnChangeRound() // Round가 바뀔 때 마다 해줘야 하는 작업들
        {
            CurrentState = RoundState.READY;
            // TODO : currentRoundEnemyData, roundData update
            ui_InGameScene.SetRoundText(Round);
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
                        CurrentState = RoundState.PLAY;
                        stateBreak = false;
                        break;
                    }
                    break;
                case RoundState.PLAY:
                    if (stateChanged)
                    {
                        PlayStateStart();
                        StartCoroutine(SpawnCurrentRoundEnemy());
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
            Debug.Log(state.ToString());
            timeLeft = 2f;
            stateChanged = false;
        }

        private void TowerStateStart()
        {
            Debug.Log(state.ToString());
            timeLeft = timeTowerSetLimit;
            stateChanged = false;
        }

        private void PokerStateStart()
        {
            Debug.Log(state.ToString());
            GameManager.UI.ShowPopupUI<UI_Poker>();
            GameManager.Poker.PokerStart();
            stateChanged = false;
        }

        private void PlayStateStart()
        {
            Debug.Log(state.ToString());
            RoundStarted?.Invoke(this, null);
            // SpawnTestEnemy();
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

            WaitForSeconds twoSecWait = new WaitForSeconds(2f);
            GameObject enemyPrefab = GameManager.Resource.Load<GameObject>($"Prefabs/Enemy/{currentEnemyName}");

            while (remainEnemyCount > 0)
            {
                GameObject enemyObject = Instantiate(enemyPrefab, startPoint.position, Quaternion.identity, enemyGroup);
                Enemy enemy = enemyObject.GetComponent<Enemy>();
                enemy.InitEnemy(currentEnemyName, currentRoundEnemyData);

                remainEnemyCount--;
                yield return twoSecWait;
            }
            yield return null;
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
            if (enemyKillCount == roundData.count)
            {
                Debug.Log("Round Clear!");
                OnChangeRound();
            }
        }

        private void GameOver()
        {

        }

        public void SetUIIngameScene(UI_InGameScene target)
            => ui_InGameScene = target;
    }
}