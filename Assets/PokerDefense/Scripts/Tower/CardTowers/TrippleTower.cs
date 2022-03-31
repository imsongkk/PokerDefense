using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Enemies;
using PokerDefense.Managers;
using static PokerDefense.Managers.TowerManager;
using static PokerDefense.Utils.Define;

namespace PokerDefense.Towers
{
    public class TrippleTower : DirectAttackTower
    {
        private int trippleBuffStack = 1;

        private static float trippleBuffPercent = .5f;

        protected override void InitDebuff()
        {
            base.InitDebuff();
        }

        protected override void DirectAttackTarget(Enemy target, float damage)
        {
            if (trippleBuffStack == 3)
            {
                damage *= (1 + trippleBuffPercent);
                trippleBuffStack = 1;
            }
            else { trippleBuffStack++; }
            base.DirectAttackTarget(target, damage);
        }
    }
}