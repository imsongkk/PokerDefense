using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using PokerDefense.Utils;

public class Tower : MonoBehaviour
{
    protected Vector3Int gridPosition;
    [SerializeField] protected TowerAsset towerAsset;

    public void SetPositionGrid(int posX, int posY)
    {
        // 좌상단부터 (0, 0) ~ (7, 7)
        gridPosition = new Vector3Int(posX, posY, 0);
        Vector3 transformPosition = new Vector3(((float)posX - 4f) / 2f + 0.25f, (7f - (float)posY) / 2f - 0.25f);
        SetPosition(transformPosition);
    }

    protected void SetPosition(Vector3 towerPosition)
    {
        /*
        gridXPosition = (xPos - 0.25) * 2 to int
        gridYPosition = (yPos + 0.25) * 2 to int
        */

        transform.position = towerPosition;
        towerAsset.PlaceTile(towerPosition);
    }
}