using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using PokerDefense.Utils;

namespace PokerDefense.Towers
{
    public class Projectile : MonoBehaviour
    {
        protected Enemy target;
        protected float damage;


        public void InitProjectile(Enemy target, float damage)
        {
            this.target = target;
            this.damage = damage;
        }

        protected virtual void DamageTarget()
        {
            target.OnDamage(damage);
        }
    }

}