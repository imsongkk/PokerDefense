using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Managers;
using PokerDefense.Enemies;
using static PokerDefense.Managers.TowerManager;
using System.Collections;
using System.Collections.Generic;
using PokerDefense.Utils;
using System;

namespace PokerDefense.Towers
{
    public class ProjectilePool
    {
        private Queue<Projectile> projectilePool;
        private int poolSize;
        private Transform parentTower;

        public ProjectilePool(GameObject projectilePrefab, int size, Transform tower)
        {
            this.projectilePool = new Queue<Projectile>();
            this.poolSize = size;
            this.parentTower = tower;

            for (int i = 0; i < poolSize; i++)
            {
                Projectile projectile = GameObject.Instantiate(projectilePrefab, parentTower).GetComponent<Projectile>();
                projectile.SetPool(this);
                projectilePool.Enqueue(projectile);
                projectile.gameObject.SetActive(false);
            }
        }


        public void Enqueue(Projectile projectile)
        {
            projectilePool.Enqueue(projectile);
            projectile.gameObject.SetActive(false);
            projectile.transform.position = parentTower.position;
        }

        public void Dequeue(Enemy target)
        {
            Projectile projectile = projectilePool.Dequeue();
            projectile.SetTarget(target);
            projectile.transform.position = parentTower.position;
            projectile.gameObject.SetActive(true);
        }
        public void Dequeue(Enemy target, float damage, float speed)
        {
            Projectile projectile = projectilePool.Dequeue();
            projectile.SetDamage(damage);
            projectile.SetTarget(target);
            projectile.transform.position = parentTower.position;
            projectile.gameObject.SetActive(true);
        }
    }
}