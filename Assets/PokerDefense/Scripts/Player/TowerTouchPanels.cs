using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using PokerDefense.Utils;

public class TowerTouchPanels : GameObjectEventHandler
{
    protected PanelObject[,] panels;
    [SerializeField] protected GameObject panelPrefab;
    [SerializeField] private RoundManager roundManager;
    [SerializeField] private TowerManager towerManager;

    void Awake() => InitAwake();

    void InitAwake()
    {
        panels = new PanelObject[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                panels[i, j] = Instantiate(panelPrefab, this.transform).GetComponent<PanelObject>();
                panels[i, j].Init(i, j, roundManager, towerManager);
            }
        }
    }

    public void AddPanelEvents()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                panels[i, j].AddEvents();
            }
        }
    }

    public void DeletePanelEvents()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                panels[i, j].DeleteEvents();
            }
        }
    }


    // public void InitPanelPosition(Transform panel, int x, int y)
    // {
    //     panel.localPosition = new Vector3((x - 4) * 0.125f + 0.0625f, (4 - y) * 0.125f - 0.0625f, 0);
    // }

}