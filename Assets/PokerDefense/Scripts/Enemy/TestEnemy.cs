using PokerDefense.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{
    Vector3 moveDirection;

    protected override void Init()
    {
        enemyInfo.name = "A";
        enemyInfo.speed = 3f;
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
        if (wayPoints.Count >= 2)
        {

            while (curIndex < wayPoints.Count)
            {
                moveDirection = (wayPoints[(curIndex + 1) % wayPoints.Count].position - wayPoints[curIndex].position).normalized;
                transform.Translate(moveDirection * enemyInfo.speed * Time.deltaTime);
                //isInWayPoint = CheckWaypoint();
                /*
                if (isInWayPoint)
                {
                    isInWayPoint = false;
                    if (curIndex == (wayPoints.Count - 1))
                    {
                        EndPoint();
                    }
                    else
                    {
                        curIndex++;
                    }

                }
                */
                yield return null;
            }
        }

    }
    /*
    private bool CheckWaypoint()
    {
        waypointDistance = Vector3.Distance(wayPoints[curIndex + 1].position, this.transform.position);
        if (waypointDistance < 0.05f * enemyInfo.speed) return true;
        else return false;
    }

    private void EndPoint()
    {
        //TODO damage to player
        Die();
    }


    //* Legacy Codes
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
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WayPoint"))
        {
            curIndex++;
            //isInWayPoint = true;
        }
        else if (collision.gameObject.CompareTag("EndPoint"))
        {
            if (curIndex + 2 >= wayPoints.Count) 
            {
                Die();
            }
        }
    }
}
