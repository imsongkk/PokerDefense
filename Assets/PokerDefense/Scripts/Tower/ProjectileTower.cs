using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Managers;
using static PokerDefense.Managers.TowerManager;
using System.Collections;
using System.Collections.Generic;
using PokerDefense.Utils;
using System;
using PokerDefense.UI.Popup;

namespace PokerDefense.Towers
{
    public class ProjectileTower : Tower
    {
        protected ProjectilePool projectilePool;
        protected GameObject projectilePrefab;
        protected int poolSize = 5;
        protected float projectileSpeed = 5;

        public override void InitTower(string towerName, TowerType towerType, int topCard, int index)
        {
            base.InitTower(towerName, towerType, topCard, index);

            InitPool();
        }

        protected void InitPool()
        {
            projectilePool = new ProjectilePool(projectilePrefab, poolSize, this.transform);
        }

        protected override void ProjectileAttackTarget(Enemy target, float damage, bool isCushion)
        {
            //TODO 투사체 발사 및 해당 투사체에서 적에게 대미지 전달하도록 변경(직접데미지 X)
            projectilePool.Dequeue(target, damage, projectileSpeed);
        }
    }
}