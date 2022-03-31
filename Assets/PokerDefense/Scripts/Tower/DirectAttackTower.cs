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
    public class DirectAttackTower : Tower
    {
        protected static List<DebuffData> debuffDatas = new List<DebuffData>();

        public override void InitTower(string towerName, TowerType towerType, int topCard, int index)
        {
            base.InitTower(towerName, towerType, topCard, index);
            InitDebuff();
        }

        protected virtual void InitDebuff() { return; }

        protected override void AttackMethod(Enemy target, float damage)
        {
            DirectAttackTarget(target, damage);
        }

        protected virtual void DirectAttackTarget(Enemy target, float damage)
        {
            DebuffTarget(target);
            target.OnDamage(damage);
        }

        private void DebuffTarget(Enemy target)
        {
            foreach (var debuffData in debuffDatas)
            {
                target.SetDebuff(debuffData);
            }
        }
    }
}