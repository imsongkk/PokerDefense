using PokerDefense.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TowerManager : MonoBehaviour
{
    private Tower tempTower;
    public Tower[,] towersArray;

    void Start() => Init();

    public void Init()
    {
        towersArray = new Tower[8, 8];
    }

    public void SetTempTowerPosition(int x, int y)
    {
        SetTowerPosition(tempTower, x, y);
    }

    public void SetTowerPosition(Tower towerPrefab, int x, int y)
    {
        if ((x >= 8) || (y >= 8) || (x < 0) || (y < 0))
        {
            throw new System.IndexOutOfRangeException();
        }
        else
        {
            if (towersArray[x, y] != null)
            {
                //TODO Place Occupied Exception
            }
            else
            {
                Tower tower = GameManager.Resource.Instantiate("TestTower").GetComponent<Tower>();
                tower.SetGridPosition(x, y);
                tower.transform.SetParent(this.transform, true);
                towersArray[x, y] = tower;
            }
            GameManager.Round.TowerSet(true);
        }
    }

    public void UpdateTower(Tower towerPrefab, int x, int y)
    {
        if ((x >= 8) || (y >= 8) || (x < 0) || (y < 0))
        {
            throw new System.IndexOutOfRangeException();
        }
        else
        {
            if (towersArray[x, y] == null)
            {
                //TODO Tower Not Set Exception
            }
            else
            {
                //TODO
            }
        }
    }

}