using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PokerDefense.Utils;

public class PanelObject : MonoBehaviour
{
    public int gridCoordinateX { get; private set; }
    public int gridCoordinateY { get; private set; }
    private RoundManager roundManager;
    private TowerManager towerManager;

    public void Init(int x, int y, RoundManager roundManager, TowerManager towerManager)
    {
        InitPosition(x, y);
        SetManager(roundManager, towerManager);
    }

    public void AddEvents()
    {
        GameObjectEventHandler.AddObjectEvent(gameObject, HighlightPanel, Define.MouseEvent.Drag);
        GameObjectEventHandler.AddObjectEvent(gameObject, SetTower, Define.MouseEvent.PointerUp);
    }

    public void DeleteEvents()
    {
        GameObjectEventHandler.DeleteObjectEvent(gameObject, HighlightPanel, Define.MouseEvent.Drag);
        GameObjectEventHandler.DeleteObjectEvent(gameObject, SetTower, Define.MouseEvent.PointerUp);
    }

    public void InitPosition(int x, int y)
    {
        gridCoordinateX = x;
        gridCoordinateY = y;
        this.transform.localPosition = new Vector3((x - 4) * 0.125f + 0.0625f, (4 - y) * 0.125f - 0.0625f, 0);
    }

    public void SetManager(RoundManager r, TowerManager t)
    {
        roundManager = r;
        towerManager = t;
    }

    private void HighlightPanel(PointerEventData eventData)
    {

    }

    private void SetTower(PointerEventData eventData)
    {
        towerManager.SetTempTowerPosition(gridCoordinateX, gridCoordinateY);
    }


}