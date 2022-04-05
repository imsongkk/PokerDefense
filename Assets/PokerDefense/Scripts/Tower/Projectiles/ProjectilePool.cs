using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Managers;
using PokerDefense.Enemies;
using static PokerDefense.Managers.TowerManager;
using static PokerDefense.Utils.Define;
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
        private List<DebuffData> debuffDatas;
        private BuffStackDelegate buffStackDelegate;

        public ProjectilePool(GameObject projectilePrefab, int size, Transform tower, BuffStackDelegate buffStack)
        {
            this.projectilePool = new Queue<Projectile>();
            this.poolSize = size;
            this.parentTower = tower;

            debuffDatas = new List<DebuffData>();
            buffStackDelegate = buffStack;

            for (int i = 0; i < poolSize; i++)
            {
                Projectile projectile = GameObject.Instantiate(projectilePrefab, parentTower).GetComponent<Projectile>();
                projectile.SetBuffStack(this.buffStackDelegate);
                projectile.SetPool(this);
                projectilePool.Enqueue(projectile);
                projectile.gameObject.SetActive(false);
            }
        }

        public void AddDebuff(DebuffData debuffData)
        {
            debuffDatas.Add(debuffData);
        }

        public void AddDebuff(Debuff debuff, float debuffTime, float debuffPercent)
        {
            DebuffData debuffData = new DebuffData(debuff, debuffTime, debuffPercent);
            AddDebuff(debuffData);
        }

        public void Enqueue(Projectile projectile)
        {
            projectilePool.Enqueue(projectile);
            projectile.transform.position = parentTower.position;
            projectile.gameObject.SetActive(false);
        }

        public void Dequeue(Enemy target)
        {
            Projectile projectile = projectilePool.Dequeue();
            projectile.SetTarget(target);
            projectile.transform.position = parentTower.position;
            projectile.gameObject.SetActive(true);
        }
        public void Dequeue(Enemy target, float damage)
        {
            Projectile projectile = projectilePool.Dequeue();
            projectile.SetDamage(damage);
            projectile.SetTarget(target);
            projectile.transform.position = parentTower.position;
            projectile.gameObject.SetActive(true);
        }
    }
}