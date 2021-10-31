using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokerDefense.Utils;

public abstract class Enemy : MonoBehaviour
{
    public abstract void Die();
    public abstract void OnDamage(Define.AttackType attackType, float damage);
    protected List<Transform> wayPoint;
}
