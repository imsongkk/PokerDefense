using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Enemies;
using PokerDefense.Managers;
using static PokerDefense.Managers.TowerManager;
using static PokerDefense.Utils.Define;


namespace PokerDefense.Towers
{
    public class FullHouseTower : ProjectileTower
    {
        private int fullHouseStack = 1;

        private static float thirdBuffPercent = .5f;
        private static float fifthBuffPercent = 1f;

        protected override void InitBuffStack()
        {
            buffStackDelegate = (() =>
            {
                if (fullHouseStack >= 5) fullHouseStack = 1;
                else fullHouseStack++;
            });
        }

        protected override void ProjectileAttackTarget(Enemy target, float damage)
        {
            if (fullHouseStack == 3) damage *= (1 + thirdBuffPercent);
            else if (fullHouseStack == 5) damage *= (1 + fifthBuffPercent);
            base.ProjectileAttackTarget(target, damage);
        }

        protected override void AddDebuff()
        {
            base.AddDebuff();
        }
    }
}