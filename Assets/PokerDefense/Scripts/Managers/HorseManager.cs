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
    const int randomNumberCount = 60;
    const int horseCount = 4;
    
    WaitForSeconds updateDelay;

    int[,] horseSnapshots = new int[horseCount, randomNumberCount];
    int[,] horseSnapshotSum = new int[horseCount, randomNumberCount]; // [60, 1200], 말이 달린 거리의 누적합
    float originHorsePositionX;
    int winnerHorseIndex = 0;

    int? playerHorseIndex = null;
    int? lastSnapShotIndex = null;

    private int? PlayerHorseIndex
    {
        get => playerHorseIndex;
        set
        {
            playerHorseIndex = value;
            if(playerHorseIndex != null)
                RunHorse();

            GameManager.Round.BreakState();
        }
    }

    private int? BettingPrice { get; set; } = null;

    public int HorseCount => horseCount;

    public void InitHorseManager()
    {
        originHorsePositionX = horseList[0].position.x;
        updateDelay = new WaitForSeconds(updateInterval);

        GameManager.Round.RoundFinished.AddListener(OnRoundFinished);
    }

    private void ResetHorseAndPrice()
    {
        ResetHorse();
        BettingPrice = null;
    }

    private void ResetHorse()
    {
        lastSnapShotIndex = null;

        for (int i = 0; i < horseCount; i++)
            horseList[i].position = new Vector3(originHorsePositionX, horseList[i].position.y, horseList[i].position.z);

        ResetPlayerHorsePointer();
    }

    public void OnBettingFinished(int? playerHorseIndex, int? bettingPrice)
    {
        PlayerHorseIndex = playerHorseIndex;
        BettingPrice = bettingPrice;
        if (BettingPrice.HasValue)
            GameManager.Round.Gold -= BettingPrice.Value;
    }

    private void OnRoundFinished()
    {
        StopHorse();

        var popup = GameManager.UI.ShowPopupUI<UI_HorseResultPopup>();
        int playerRank = GetPlayerRank();
        int price = GetPlayerPrice(playerRank);
        popup.InitUI(playerRank, price, ()=>
        {
            GameManager.Round.Gold += price;
            ResetHorseAndPrice();
        });
    }

    public void RunHorse()
    {
        CalculateWinnerInAdvance();
        ActivePlayerHorsePointer();
        StartCoroutine("Run");
    }

    IEnumerator Run()
    {
        for (int i = 0; i < horseCount; i++)
            horseAnimatorList[i].SetBool("IsRunning", true);

        float distance = Vector3.Distance(startLine.position, endLine.position);
        float ratio = distance / horseSnapshotSum[winnerHorseIndex, randomNumberCount - 1];
        
        lastSnapShotIndex = 0;

        while(lastSnapShotIndex < randomNumberCount)
        {
            for(int i=0; i<horseCount; i++)
            {
                horseList[i].position += ratio * new Vector3(horseSnapshots[i, lastSnapShotIndex.Value], 0, 0);
            }
            yield return updateDelay;
            lastSnapShotIndex++;
        }
        lastSnapShotIndex--;

        StopHorse();
    }

    private void StopHorse()
    {
        StopCoroutine("Run");

        for (int i = 0; i < horseCount; i++)
            horseAnimatorList[i].SetBool("IsRunning", false);
    }

    private void CalculateWinnerInAdvance()
    {
        for (int i = 0; i < horseCount; i++)
        {
            horseSnapshotSum[i, 0] = 0;

            for (int j = 0; j < randomNumberCount; j++)
            {
                horseSnapshots[i, j] = Range(1, 21); // [1, 20]
                if (j == 0)
                    horseSnapshotSum[i, j] = horseSnapshots[i, j];
                else
                    horseSnapshotSum[i, j] = horseSnapshotSum[i, j - 1] + horseSnapshots[i, j];
            }
        }

        for (int i = 0; i < horseCount; i++)
        {
            if (horseSnapshotSum[i, randomNumberCount - 1] > horseSnapshotSum[winnerHorseIndex, randomNumberCount - 1])
                winnerHorseIndex = i;
        }
    }

    private void ActivePlayerHorsePointer()
        => horseList[PlayerHorseIndex.Value].GetChild(1).gameObject.SetActive(true);

    private void ResetPlayerHorsePointer()
        => horseList[PlayerHorseIndex.Value].GetChild(1).gameObject.SetActive(false);

    private int GetPlayerRank()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < horseCount; i++)
            list.Add(horseSnapshotSum[i, lastSnapShotIndex.Value]);
        list.Sort();

        for (int i = horseCount - 1; i >= 0; i--)
        {
            if (list[i] == horseSnapshotSum[PlayerHorseIndex.Value, lastSnapShotIndex.Value])
                return horseCount - i;
        }
        return 0;
    }

    private int GetPlayerPrice(int playerRank)
    {
        // TODO : 임시
        if (playerRank != 1) return 0;
        if(BettingPrice.HasValue)
            return BettingPrice.Value * 2;
        return 0;
    }
}
