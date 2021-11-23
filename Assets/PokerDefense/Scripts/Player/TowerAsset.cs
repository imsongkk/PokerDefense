using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using PokerDefense.Utils;

public enum TowerType
{
    Spade,
    Clover,
    Diamond,
    Heart
}

[CreateAssetMenu(fileName = "New Tower", menuName = "Tower", order = 0)]
public class TowerAsset : ScriptableObject
{
    [SerializeField] protected Sprite towerSprite;
    [SerializeField] protected float damage;
    [SerializeField] protected TowerType towerType;

    public Sprite TowerSprite
    {
        get { return towerSprite; }
    }

    // protected void OnEnable()
    // {
    //     towerGrid = GameObject.FindWithTag("TowerGrid").GetComponent<Tilemap>();
    // }

    // public void PlaceTile(Vector3 towerPosition)
    // {
    //     Vector3Int gridPosition = new Vector3Int((int)Mathf.Round((towerPosition.x - 0.25f) * 2), (int)Mathf.Round((towerPosition.y + 0.25f) * 2), 0);
    //     Vector3Int targetCell = gridPosition;
    //     towerGrid.SetTile(targetCell, towerTile);
    //     Debug.Log("Set Tile!@");
    // }
}