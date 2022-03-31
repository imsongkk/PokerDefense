using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Managers;
using PokerDefense.Enemies;
using static PokerDefense.Managers.TowerManager;

namespace PokerDefense.Towers
{
    public class StraightTower : ProjectileTower
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