using PokerDefense.Managers;
using PokerDefense.UI.Popup;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

public class HorseManager : MonoBehaviour
{
    [SerializeField] List<Transform> horseList = new List<Transform>();
    [SerializeField] List<Animator> horseAnimatorList = new List<Animator>();
    [SerializeField] Transform startLine;
    [SerializeField] Transform endLine;

    const float updateInterval = 0.5f;
    const int randomNumberCount = 120;
    const int horseCount = 4;
    
    WaitForSeconds updateDelay;

    int[,] horseSnapshots = new int[horseCount, randomNumberCount];
    int[] horseSum = new int[horseCount]; // [120, 1200]
    int winnerHorseIndex = 0;
    int playerHorseIndex;
    float originHorsePositionX;
    float ratio;

    private void Start()
    {
        originHorsePositionX = horseList[0].position.x;
    }

    public void RunHorse(int playerHorseIndex)
    {
        this.playerHorseIndex = playerHorseIndex;

        CalculateWinner();
        ActivePlayerHorsePointer();

        updateDelay = new WaitForSeconds(updateInterval);

        StartCoroutine("Run");
    }

    IEnumerator Run()
    {
        for (int i = 0; i < horseCount; i++)
            horseAnimatorList[i].SetBool("IsRunning", true);

        float distance = Vector3.Distance(startLine.position, endLine.position);
        ratio = distance / horseSum[winnerHorseIndex];
        int snapshotIndex = 0;

        while(snapshotIndex < randomNumberCount)
        {
            for(int i=0; i<horseCount; i++)
            {
                horseList[i].position += ratio * new Vector3(horseSnapshots[i, snapshotIndex], 0, 0);
            }
            snapshotIndex++;
            yield return updateDelay;
        }

        StopHorse();
    }

    private void StopHorse()
    {
        StopCoroutine("Run");

        for (int i = 0; i < horseCount; i++)
            horseAnimatorList[i].SetBool("IsRunning", false);
    }

    private void SetHorseLastSnapShot()
    {
        for (int i = 0; i < horseCount; i++)
            horseList[i].position = new Vector3(originHorsePositionX + ratio * horseSum[i], horseList[i].position.y, horseList[i].position.z);
    }

    public void InterruptRace(Action OnConfirm) // 경주가 다 끝나기 전에 라운드 종료 됐을 때 호출
    {
        StopHorse();
        SetHorseLastSnapShot();

        var popup = GameManager.UI.ShowPopupUI<UI_HorseResultPopup>();
        int playerRank = GetPlayerRank();

        OnConfirm += ResetHorse;

        popup.InitUI(playerRank, OnConfirm);
    }

    private void ResetHorse() // 경주 결과 확인 창이 닫힐 때 호출
    {
        ResetPlayerHorsePointer();

        Debug.Log("Horse Reset");
        for (int i = 0; i < horseCount; i++)
            horseList[i].position = new Vector3(originHorsePositionX, horseList[i].position.y, horseList[i].position.z);
    }

    private void CalculateWinner()
    {
        for (int i = 0; i < horseCount; i++)
        {
            for (int j = 0; j < randomNumberCount; j++)
            {
                horseSnapshots[i, j] = Range(1, 11); // [1, 10]
                horseSum[i] += horseSnapshots[i, j];
            }
        }

        for (int i = 0; i < horseCount; i++)
        {
            if (horseSum[i] > horseSum[winnerHorseIndex])
                winnerHorseIndex = i;
        }
    }

    private void ActivePlayerHorsePointer()
        => horseList[playerHorseIndex].GetChild(1).gameObject.SetActive(true);

    private void ResetPlayerHorsePointer()
        => horseList[playerHorseIndex].GetChild(1).gameObject.SetActive(false);

    private int GetPlayerRank()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < horseCount; i++)
            list.Add(horseSum[i]);
        list.Sort();

        for(int i = horseCount - 1; i >= 0; i--)
        {
            if (list[i] == horseSum[playerHorseIndex])
                return horseCount - i;
        }
        return 0;
    }
}
