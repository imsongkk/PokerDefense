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
using PokerDefense.UI.Popup;

namespace PokerDefense.Towers
{
    public class ProjectileTower : Tower
    {
        [SerializeField]
        protected GameObject projectilePrefab;

        protected ProjectilePool projectilePool;
        protected int poolSize = 5;
        protected float projectileSpeed = 0.5f;
        protected BuffStackDelegate buffStackDelegate;

        public override void InitTower(string towerName, TowerType towerType, int topCard, int index)
        {
            base.InitTower(towerName, towerType, topCard, index);

            InitPool();
        }

        protected void InitPool()
        {
            projectilePool = new ProjectilePool(projectilePrefab, poolSize, this.transform, buffStackDelegate);
            AddDebuff();
        }

        protected virtual void AddDebuff()
        {
            //* override 전용, projectiletower 전용
            //* 부여할 모든 디버프를 pool에 추가
            //  projectilePool.AddDebuff(debuffData)
        }

        protected override void AttackMethod(Enemy target, float damage)
        {
            ProjectileAttackTarget(target, damage);
        }

        protected virtual void ProjectileAttackTarget(Enemy target, float damage)
        {
            //TODO 투사체 발사 및 해당 투사체에서 적에게 대미지 전달하도록 변경(직접데미지 X)
            projectilePool.Dequeue(target, damage);
        }
    }
}