using PokerDefense.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{
    protected override void Init()
    {
        enemyInfo.name = "A";
        enemyInfo.speed = 3;
        enemyInfo.hp = 100;
        enemyInfo.round = 1;
        collider = gameObject.GetComponent<BoxCollider2D>();
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void OnDamage(Define.AttackType attackType, float damage)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnSpawn()
    {
        
    }

    IEnumerator Move()
    {
        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WayPoint"))
        {
            isInWayPoint = true;
            if(curIndex == 3) // Á×À½
            {
                Die();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WayPoint"))
        {
            isInWayPoint = false;
            if (curIndex == 3) // Á×À½
            {
                Die();
            }
        }
    }
}
