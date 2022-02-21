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

        public int Round { get; private set; }
        public string HardNess { get; private set; }
        public int Heart { get; private set; }
        public int Gold { get; private set; }
        public int Chance { get; private set; }

        private float timeTowerSetLimit = 8f;
        private float timeLeft = 0;

        private bool stateBreak = false;
        private bool timerBreak = false;        // false일때만 State 타이머가 흐르게 됨

        EnemyData enemyData;
        RoundData roundData;
        HardNessData hardNessData;

        public void InitRoundManager() // 처음으로 게임이 시작될 때 호출
        {
            /* TODO : 데이터 실체화
             * Round와 HardNess는 MainScene에서 받아옴
             */
            Round = 1;
            HardNess = "Easy";

            // 순서 주의
            InitWayPoints();
            InitHardNessData();
            InitRoundData();
            InitEnemyData();

            StartCoroutine(InitUIText());

            CurrentState = RoundState.READY;
        }

        // private void InitPoints()
        // {
        //     startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform;
        //     endPoint = GameObject.FindGameObjectWithTag("EndPoint").transform;
        //     GameObject.FindGameObjectsWithTag("WayPoint").ToList().ForEach((waypoint) => wayPoints.Add(waypoint.transform));
        // }

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

            Heart = hardNessData.heart;
            Gold = hardNessData.gold;
            Chance = hardNessData.chance;
        }

        private void InitRoundData()
        {
            Dictionary<string, RoundData> dict = new Dictionary<string, RoundData>();
            GameManager.Data.RoundDataDict.TryGetValue(HardNess, out dict);
            dict.TryGetValue(Round.ToString(), out roundData);
        }

        private void InitEnemyData()
        {
            GameManager.Data.EnemyDataDict.TryGetValue(roundData.enemyName, out enemyData);
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
                        StartCoroutine(SpawnTestEnemy());
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
            //TODO 타워 종류 결정
            GameManager.Tower.ConfirmTower(roundHand);
        }

        private void SpawnEnemy(GameObject enemy)
        {
            //Instantiate(enemy, spawnPoint.position, Quaternion.identity);
        }

        /*
        private void SpawnTestEnemy()
        {
            Debug.Log("A");
            //GameObject target = GameManager.Resource.Load<GameObject>($"Prefabs/TestEnemy");
            //SpawnEnemy(target);
        }
        */

        IEnumerator SpawnTestEnemy()
        {
            int remainEnemyCount = roundData.count;
            string currentEnemyName = roundData.enemyName;
            WaitForSeconds twoSecWait = new WaitForSeconds(2f);
            GameObject enemy = GameManager.Resource.Load<GameObject>($"Prefabs/Enemy/TestEnemy");

            while (remainEnemyCount > 0)
            {
                Instantiate(enemy, startPoint.position, Quaternion.identity, enemyGroup);
                remainEnemyCount--;
                yield return twoSecWait;
            }
            yield return null;
        }

        public void SetUIIngameScene(UI_InGameScene target)
            => ui_InGameScene = target;
    }
}