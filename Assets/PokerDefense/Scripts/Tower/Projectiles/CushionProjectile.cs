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
        protected int maxCushionTimes;
        protected int cushionTimes;     // 쿠션 횟수

        private void OnEnable()
        {
            cushionTimes = maxCushionTimes;
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
                if (target == null) base.OnHit();
            }
        }
    }
}