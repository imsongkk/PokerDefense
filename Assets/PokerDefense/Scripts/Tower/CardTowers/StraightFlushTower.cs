using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Enemies;
using PokerDefense.Managers;
using static PokerDefense.Managers.TowerManager;

namespace PokerDefense.Towers
{
    public class StraightFlushTower : ProjectileTower
    {
        protected override void ProjectileAttackTarget(Enemy target, float damage)
        {
            base.ProjectileAttackTarget(target, damage);
        }

        protected override void AddDebuff()
        {

        }
    }
}