using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Enemies;
using PokerDefense.Managers;
using static PokerDefense.Managers.TowerManager;
using static PokerDefense.Utils.Define;


namespace PokerDefense.Towers
{
    public class FiveCardsTower : DirectAttackTower
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