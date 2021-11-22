using PokerDefense.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{
    protected override void Init()
    {
        enemyInfo.name = "A";
        enemyInfo.speed = 0.7f;
        enemyInfo.hp = 100;
        enemyInfo.round = 1;
        collider = gameObject.GetComponent<BoxCollider2D>();

        OnSpawn();
    }

    public override void Die()
    {
        // throw new System.NotImplementedException();
        Destroy(gameObject);
    }

    public override void OnDamage(Define.AttackType attackType, float damage)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnSpawn()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            transform.Translate(Vector3.up * enemyInfo.speed * Time.deltaTime);
            if (isInWayPoint)
            {
                curIndex++;
                RotatePath();
                isInWayPoint = false;
            }
            yield return null;
        }
    }

    protected void RotatePath()
    {
        transform.Rotate(0, 0, -90);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WayPoint"))
        {
            isInWayPoint = true;
            if (curIndex == 4) // ����
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
            if (curIndex == 4) // ����
            {
                Die();
            }
        }
    }
}
