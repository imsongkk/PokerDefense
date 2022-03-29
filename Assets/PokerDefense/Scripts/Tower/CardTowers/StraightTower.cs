using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Managers;
using PokerDefense.Enemies;
using static PokerDefense.Managers.TowerManager;

namespace PokerDefense.Towers
{
    public class StraightTower : ProjectileTower
    {
        protected override void AttackTarget(Enemy target)
        {
            base.AttackTarget(target);
        }

        protected override void AddDebuff()
        {

        }
    }
}