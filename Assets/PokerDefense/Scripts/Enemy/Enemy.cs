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

    void Start() => Init();

    protected EnemyInfo enemyInfo;
    protected Transform wayPointParent;
    [SerializeField] protected Transform[] wayPoints;

    protected int curIndex = 0;
    protected bool isInWayPoint = false;
    protected new BoxCollider2D collider;

    public abstract void Die();
    public abstract void OnDamage(Define.AttackType attackType, float damage);
    protected abstract void Init();
    protected abstract void OnSpawn();
}
