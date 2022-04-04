using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Utils;
using PokerDefense.Managers;
using PokerDefense.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using static PokerDefense.Utils.Define;

namespace PokerDefense.Towers
{
    public class CushionProjectile : Projectile
    {
        private CushionCollider cushionCollider;

        [SerializeField]
        protected float cushionDamageDrop;      // 1쿠션마다 대미지 감소 비율

        [SerializeField]
        protected int maxCushionTimes;
        protected int cushionTimes;     // 쿠션 횟수

        [SerializeField]
        protected float cushionColliderRadius;      // Cushion Collider의 반지름

        protected override void OnEnable()
        {
            base.OnEnable();
            cushionTimes = maxCushionTimes;
            cushionCollider.GetComponent<CircleCollider2D>().radius = cushionColliderRadius;
        }

        protected virtual void OnDisable()
        {
            base.OnDisable();
            this.realDamage = this.damage;
        }

        protected override void Awake()
        {
            base.Awake();
            cushionTimes = maxCushionTimes;
            cushionCollider = this.transform.GetChild(0).GetComponent<CushionCollider>();
        }

        protected override void OnHit()
        {
            if (--cushionTimes <= 0) base.OnHit();
            else
            {
                //TODO cushion
                target = cushionCollider.NextTarget(target);
                this.realDamage *= (1 - cushionDamageDrop);
                if (target == null)
                {
                    base.OnHit();
                }
            }
        }
    }
}