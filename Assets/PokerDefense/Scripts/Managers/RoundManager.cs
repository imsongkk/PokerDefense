using PokerDefense.Managers;
using PokerDefense.Utils;
using PokerDefense.UI.Scene;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

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

        public int Round { get; private set; } = 1;
        private int heart = 5;
        private int gold = 10;

        private float timeTowerSetLimit = 10f;
        private float timeLeft = 0;
        private bool towerSet = false;

        private void Start()
            => Init();

        public void Init()
        {
            InitPoints();
            StartCoroutine(SetTextUI());
            CurrentState = RoundState.READY;
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
                    timeLeft -= Time.deltaTime;
                    if (towerSet)
                    {
                        CurrentState = RoundState.POKER;
                        towerSet = false;
                        break;
                    }
                    if (timeLeft <= 0) CurrentState = RoundState.PLAY;
                    break;
                case RoundState.POKER:
                    if (stateChanged) { PokerStateStart(); }
                    //TODO poker
                    break;
                case RoundState.PLAY:
                    if (stateChanged) { PlayStateStart(); }
                    // SpawnTestEnemy();
                    break;
            }
        }

        public void BuildTower()
        {
            //CurrentState = RoundState.PLAY;
            towerSet = true;
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
            stateChanged = false;
        }

        private void PlayStateStart()
        {
            Debug.Log(state.ToString());
            stateChanged = false;
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