using PokerDefense.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public Tower testTower;
    void TestPosition()
    {
        Instantiate(testTower.gameObject);
        testTower.SetPositionGrid(0, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TestPosition();
        }
    }

}