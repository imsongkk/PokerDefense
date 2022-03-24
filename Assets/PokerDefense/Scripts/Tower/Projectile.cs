using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Utils;
using PokerDefense.Managers;
using PokerDefense.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using PokerDefense.Utils;
using static PokerDefense.Utils.Define;

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
        protected DebuffData debuffData;
        protected BuffStackDelegate buffStackDelegate;

        protected Vector3 targetPosition;

        [SerializeField]
        private Sprite projectileSprite;
        public Sprite ProjectileSprite
        {
            get { return projectileSprite; }
            set
            {
                projectileSprite = value;
                this.GetComponent<SpriteRenderer>().sprite = projectileSprite;
            }
        }

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

        public void InitProjectile(float speed, bool isCushion, ProjectilePool pool, DebuffData debuffData, BuffStackDelegate buffStackDelegate)
        {
            this.speed = speed;
            this.isCushion = isCushion;
            this.pool = pool;
            this.debuffData = debuffData;
            this.buffStackDelegate = buffStackDelegate;
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

        public void SetDebuff(Debuff debuff, float time, float percent)
        {
            this.debuffData.debuff = debuff;
            this.debuffData.debuffTime = time;
            this.debuffData.debuffPercent = percent;
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
            target.OnDamage(this.damage, this.buffStackDelegate);
            target.SetDebuff(this.debuffData);
        }

        protected virtual void OnHit()
        {
            //override on cushion projectile
            pool.Enqueue(this);
        }
    }

}