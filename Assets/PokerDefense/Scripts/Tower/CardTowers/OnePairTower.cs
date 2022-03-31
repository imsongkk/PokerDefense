using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Managers;
using static PokerDefense.Managers.TowerManager;
using PokerDefense.Utils;
using PokerDefense.Enemies;

namespace PokerDefense.Towers
{
    public class OnePairTower : DirectAttackTower
    {
        protected override void InitDebuff()
        {
            base.InitDebuff();
        }

        protected override void DirectAttackTarget(Enemy target, float damage)
        {
            base.DirectAttackTarget(target, damage);
        }

    }
}