using PokerDefense.Managers;
using PokerDefense.Utils;
using PokerDefense.UI.Scene;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoundManager : MonoBehaviour
{
    enum State
    {
        NONE,
        STOP,
        READY,
        TOWER,
        POKER,
        PLAY

    }

    private State state = State.NONE;
    private bool stateChanged = false;

    private State currentState
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

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] wayPoint = new Transform[3];
    private UI_InGameScene ui_InGameScene;

    [SerializeField] private TowerTouchPanels towerTouchPanels;

    public int Round { get; private set; } = 1;
    private int heart = 5;
    private int gold = 10;

    private float timeTowerSetLimit = 10f;
    private float timeLeft = 0;
    private bool towerSet = false;

    private void Start()
        => Init();

    private void Init()
    {
        StartCoroutine(SetTextUI());
        currentState = State.READY;
    }

    IEnumerator SetTextUI()
    {
        yield return new WaitUntil(() => ui_InGameScene != null);
        ui_InGameScene.SetHeartText(heart);
        ui_InGameScene.SetGoldText(gold);
        ui_InGameScene.SetRoundText(Round);
    }

    private void Update()
    {
        // Debug.Log(state.ToString());
        switch (state)
        {
            case State.NONE:
                if (stateChanged) { stateChanged = false; }
                break;
            case State.READY:
                if (stateChanged) { ReadyStateStart(); }
                //TODO  Ready Animaiton
                timeLeft -= Time.deltaTime;
                if (timeLeft <= 0) currentState = State.TOWER;
                break;
            case State.TOWER:
                if (stateChanged) { TowerStateStart(); }
                //set position or wait 2min
                timeLeft -= Time.deltaTime;
                if (towerSet)
                {
                    currentState = State.POKER;
                    towerSet = false;
                    break;
                }
                if (timeLeft <= 0) currentState = State.PLAY;
                break;
            case State.POKER:
                if (stateChanged) { PokerStateStart(); }
                //TODO poker
                break;
            case State.PLAY:
                if (stateChanged) { PlayStateStart(); }
                // SpawnTestEnemy();
                break;
        }
    }

    public void TowerSet(bool ts)
    {
        towerSet = ts;
    }

    private void ReadyStateStart()
    {
        Debug.Log(state.ToString());
        timeLeft = 2f;
        towerTouchPanels.DeletePanelEvents();
        stateChanged = false;
    }

    private void TowerStateStart()
    {
        Debug.Log(state.ToString());
        timeLeft = timeTowerSetLimit;
        towerTouchPanels.AddPanelEvents();
        stateChanged = false;
    }

    private void PokerStateStart()
    {
        Debug.Log(state.ToString());
        towerTouchPanels.DeletePanelEvents();
        stateChanged = false;
    }

    private void PlayStateStart()
    {
        Debug.Log(state.ToString());
        towerTouchPanels.DeletePanelEvents();
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

    public void SetUIIngameScene(UI_InGameScene target)
        => ui_InGameScene = target;
}
