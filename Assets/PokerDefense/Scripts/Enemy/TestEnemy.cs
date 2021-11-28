using PokerDefense.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{
    protected override void Init()
    {
        enemyInfo.name = "A";
        enemyInfo.speed = 1f;
        enemyInfo.hp = 100;
        enemyInfo.round = 1;
        collider = gameObject.GetComponent<BoxCollider2D>();

        wayPointParent = GameObject.FindGameObjectWithTag("WayPointParent").transform;
        wayPoints = new Transform[wayPointParent.childCount];
        int i = 0;
        foreach (Transform child in wayPointParent)
        {
            wayPoints[i++] = child;
        }
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
        if (wayPoints.Length >= 2)
        {
            Vector3 moveDirection = (wayPoints[1].position - wayPoints[0].position).normalized;
            while (curIndex < wayPoints.Length)
            {
                transform.Translate(moveDirection * enemyInfo.speed * Time.deltaTime);
                if (isInWayPoint)
                {
                    isInWayPoint = false;
                    curIndex++;
                    moveDirection = (wayPoints[(curIndex + 1) % wayPoints.Length].position - wayPoints[curIndex].position).normalized;
                }
                yield return null;
            }
        }

    }

    IEnumerator MoveLegacy()
    {
        while (true)
        {
            transform.Translate(Vector3.up * enemyInfo.speed * Time.deltaTime);
            if (isInWayPoint)
            {
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
        }
        else if (collision.gameObject.CompareTag("Respawn"))
        {
            if (curIndex >= wayPoints.Length - 1) // ����
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
            // if (curIndex == 4) // ����
            // {
            //     Die();
            // }
        }
    }
}
