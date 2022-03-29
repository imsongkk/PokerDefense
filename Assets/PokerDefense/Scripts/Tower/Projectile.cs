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
        // protected DebuffData debuffData;
        protected static List<DebuffData> debuffDatas = new List<DebuffData>();
        protected BuffStackDelegate buffStackDelegate;

        protected Vector2 targetPosition;

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

        private Rigidbody2D rigidbody;

        private void Awake()
        {
            this.rigidbody = this.GetComponent<Rigidbody2D>();
            this.buffStackDelegate = null;
        }

        private void FixedUpdate()
        {
            if (gameObject.activeSelf && (target != null))
            {
                targetPosition = (Vector2)target.transform.position;
                rigidbody.MovePosition(this.rigidbody.position + (targetPosition - this.rigidbody.position).normalized * speed);
            }

        }

        public void InitProjectile(float speed, bool isCushion, ProjectilePool pool, BuffStackDelegate buffStackDelegate)
        {
            this.speed = speed;
            this.isCushion = isCushion;
            this.pool = pool;
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

        public void AddDebuff(DebuffData debuff)
        {
            debuffDatas.Add(debuff);
        }

        public void SetBuffStack(BuffStackDelegate buffStack)
        {
            this.buffStackDelegate = buffStack;
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Enemy>() == target)
            {
                DamageTarget(target);
                OnHit();
            }
        }

        protected virtual void DamageTarget(Enemy target)
        {
            foreach (var debuffData in debuffDatas)
            {
                target.SetDebuff(debuffData);
            }
            target.OnDamage(this.damage);
        }

        protected virtual void OnHit()
        {
            //override on cushion projectile
            if (buffStackDelegate != null) this.buffStackDelegate();
            pool.Enqueue(this);
        }
    }

}