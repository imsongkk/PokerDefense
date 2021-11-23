using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokerDefense.Utils;

public class Tower : MonoBehaviour
{
    [SerializeField] protected TowerAsset towerAsset;
    protected float towerDamage;    // 계산된 실제 대미지

    void Start() => Init();

    protected virtual void Init()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = towerAsset.TowerSprite;
    }

    protected virtual void Attack()
    {

    }
    protected virtual void DamageCalculate()
    {

    }

    public virtual void SetGridPosition(int posX, int posY)
    {
        // 좌상단부터 (0, 0) ~ (7, 7)
        Vector3 transformPosition = new Vector3(((float)posX - 4f) / 2f + 0.25f, (7f - (float)posY) / 2f - 0.25f);
        SetPosition(transformPosition);
    }

    protected virtual void SetPosition(Vector3 towerPosition)
    {
        /*
        gridXPosition = (xPos - 0.25) * 2 to int
        gridYPosition = (yPos + 0.25) * 2 to int
        */

        transform.position = towerPosition;
        //// towerAsset.PlaceTile(towerPosition);
    }
}