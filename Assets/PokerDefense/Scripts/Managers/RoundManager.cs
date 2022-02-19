using PokerDefense.UI.Scene;
using PokerDefense.UI.Popup;
using PokerDefense.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static PokerDefense.Utils.Define;
using PokerDefense.Data;

namespace PokerDefense.Managers
{
    public class RoundManager : MonoBehaviour
    {
        [Flags]
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

        Transform spawnPoint;
        List<Transform> wayPoints = new List<Transform>();
        UI_InGameScene ui_InGameScene;

        public int Round { get; private set; }
        public string HardNess { get; private set; }
        public int Heart { get; private set; }
        public int Gold { get; private set; }
        public int Chance { get; private set; }

        private int heart = 5;
        private int gold = 10;

        private float timeTowerSetLimit = 8f;
        private float timeLeft = 0;

        private bool stateBreak = false;
        private bool timerBreak = false;        // false일때만 State 타이머가 흐르게 됨

        private void Start()
            => Init();

        public void Init()
        {
            InitPoints();
            StartCoroutine(SetTextUI());
            CurrentState = RoundState.READY;
        }

        public void InitRoundManager() // 처음으로 게임이 시작될 때 호출
        {
            /* TODO : 데이터 실체화
             * Round와 HardNess는 MainScene에서 받아옴
             */

            Round = 1;
            HardNess = "Easy";

            HardNessData hardNessData = new HardNessData();
            GameManager.Data.HardNessDataDict.TryGetValue(HardNess, out hardNessData);

            Heart = hardNessData.heart;
            Gold = hardNessData.gold;
            Chance = hardNessData.chance;
        }

        IEnumerator SetTextUI()
        {
            yield return new WaitUntil(() => ui_InGameScene != null);
            ui_InGameScene.InitText(heart, gold, Round);
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
                    if (stateChanged) { PlayStateStart(); }
                    // SpawnTestEnemy();
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
            Instantiate(enemy, spawnPoint.position, Quaternion.identity);
        }

        private void SpawnTestEnemy()
        {
            GameObject target = GameManager.Resource.Load<GameObject>($"Prefabs/TestEnemy");
            SpawnEnemy(target);
        }

        private void InitPoints()
        {
            GameObject respawn = GameObject.FindGameObjectWithTag("Respawn");
            GameObject.FindGameObjectsWithTag("WayPoint").ToList().ForEach((waypoint) => wayPoints.Add(waypoint.transform));
        }

        public void SetUIIngameScene(UI_InGameScene target)
            => ui_InGameScene = target;
    }
}