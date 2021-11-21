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

    protected EnemyInfo enemyInfo;
    protected Transform[] wayPoint = new Transform[3];
    protected int curIndex = 0;
    protected bool isInWayPoint = true;
    protected new BoxCollider2D collider;

    public abstract void Die();
    public abstract void OnDamage(Define.AttackType attackType, float damage);
    protected abstract void Init();
    protected abstract void OnSpawn();
}
