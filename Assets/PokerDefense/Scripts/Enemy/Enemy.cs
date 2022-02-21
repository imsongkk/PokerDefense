using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokerDefense.Utils;

public abstract class Enemy : MonoBehaviour
{
    protected struct EnemyInfo
    {
        public float speed, hp;
        public int round;
        public string name;
    }

    // public static void WayPointSet()
    // {
    //     wayPointParent = GameObject.FindGameObjectWithTag("WayPointParent").transform;
    //     wayPoints = new Transform[wayPointParent.childCount];
    //     int i = 0;
    //     foreach (Transform child in wayPointParent)
    //     {
    //         wayPoints[i++] = child;
    //     }
    // }

    void Start() => Init();

    protected EnemyInfo enemyInfo;


    protected int curIndex = 0;
    protected bool isInWayPoint = false;
    protected bool isOutWayPoint = true;

    public static List<Transform> wayPoints;
    public static Transform endPoint;

    public abstract void Die();
    public abstract void OnDamage(float damage);
    protected abstract void Init();
    protected abstract void OnSpawn();

    protected float waypointDistance;   // 다음 웨이포인트와 거리
}
