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
        protected ProjectilePool pool;
        protected Enemy target;

        [SerializeField]
        protected float damage;
        [SerializeField]
        protected float speed;
        [SerializeField]
        protected bool isCushion;

        protected Vector3 targetPosition;

        private void Start()
        {

        }

        private void Update()
        {
            if (target != null)
            {
                targetPosition = target.transform.position;
                transform.Translate((targetPosition - this.transform.position).normalized * speed);
            }

        }

        public void InitProjectile(float speed, bool isCushion, ProjectilePool pool)
        {
            this.speed = speed;
            this.isCushion = isCushion;
            this.pool = pool;
        }

        public void SetDamage(float damage)
        {
            this.damage = damage;
        }

        public void SetPool(ProjectilePool pool)
        {
            this.pool = pool;
        }

        public void SetTarget(Enemy target)
        {
            this.target = target;
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                DamageTarget(collision.GetComponent<Enemy>());
                OnHit();
            }
        }

        protected virtual void DamageTarget(Enemy target)
        {
            //TODO debuff target
            target.OnDamage(this.damage);
        }

        protected virtual void OnHit()
        {
            //override on cushion projectile
            pool.Enqueue(this);
        }
    }

}