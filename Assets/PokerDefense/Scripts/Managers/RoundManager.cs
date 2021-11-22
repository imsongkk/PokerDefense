using PokerDefense.Managers;
using PokerDefense.UI.Scene;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    enum State
    {
        NONE,
        STOP,
        READY,
        PLAY,
        POKER,
    }

    State state = State.READY;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] wayPoint = new Transform[3];
    private UI_InGameScene ui_InGameScene;

    public int Round { get; private set; } = 1;
    private int heart = 5;
    private int gold = 10;

    private void Start()
        => Init();

    private void Init()
    {
        StartCoroutine(SetTextUI());
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
        if (state == State.READY) // �ð��� �� �ưų� ������ ��ŸƮ ����
        {
            GameObject target = GameManager.Resource.Load<GameObject>($"Prefabs/TestEnemy");
            Instantiate(target, spawnPoint.position, Quaternion.identity);
            state = State.STOP;
        }
    }

    public void SetUIIngameScene(UI_InGameScene target)
        => ui_InGameScene = target;
}
