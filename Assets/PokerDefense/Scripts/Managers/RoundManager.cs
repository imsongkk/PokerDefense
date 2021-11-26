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

    // IEnumerator SetState(State state)
    // {
    //     //! currentState set에서만 사용함. 독자적으로 사용하지 말것
    //     Debug.Log(state.ToString());
    //     switch (state)
    //     {
    //         case State.NONE:
    //             break;
    //         case State.READY:
    //             //TODO  Ready Animaiton
    //             yield return new WaitForSeconds(1f);
    //             yield return StartCoroutine(SetState(State.TOWER));
    //             break;
    //         case State.TOWER:
    //             TowerStateStart();
    //             //TODO set position or wait 2min
    //             float waitSeconds = timeTowerSetLimit;
    //             while (waitSeconds >= 0)
    //             {
    //                 waitSeconds -= Time.deltaTime;
    //                 if (towerSet)
    //                 {
    //                     yield return StartCoroutine(SetState(State.POKER));
    //                     towerSet = false;
    //                     break;
    //                 }
    //             }
    //             if (waitSeconds <= 0) yield return StartCoroutine(SetState(State.PLAY));
    //             break;
    //         case State.POKER:
    //             PokerStateStart();
    //             //TODO poker
    //             break;
    //         case State.PLAY:
    //             PlayStateStart();
    //             // SpawnTestEnemy();
    //             break;
    //     }
    //     yield return null;
    // }

    private void Update()
    {
        // Debug.Log(state.ToString());
        switch (state)
        {
            case State.NONE:
                if (stateChanged) { stateChanged = false; }
                break;
            case State.READY:
                if (stateChanged) { stateChanged = false; }
                //TODO  Ready Animaiton
                currentState = State.TOWER;
                break;
            case State.TOWER:
                if (stateChanged) { TowerStateStart(); stateChanged = false; }
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
                if (stateChanged) { PokerStateStart(); stateChanged = false; }
                //TODO poker
                break;
            case State.PLAY:
                if (stateChanged) { PlayStateStart(); stateChanged = false; }
                // SpawnTestEnemy();
                break;
        }


    }

    public void TowerSet(bool ts)
    {
        towerSet = ts;
    }

    private void TowerStateStart()
    {
        Debug.Log(state.ToString());
        timeLeft = timeTowerSetLimit;
        towerTouchPanels.AddPanelEvents();
    }

    private void PokerStateStart()
    {
        Debug.Log(state.ToString());
        towerTouchPanels.DeletePanelEvents();
    }

    private void PlayStateStart()
    {
        Debug.Log(state.ToString());
        towerTouchPanels.DeletePanelEvents();
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
