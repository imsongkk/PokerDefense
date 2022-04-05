using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Enemies;
using PokerDefense.Managers;
using static PokerDefense.Managers.TowerManager;
using static PokerDefense.Utils.Define;


namespace PokerDefense.Towers
{
    public class RoyalStraightFlushTower : FlushTower
    {
        private int goldBuffStack;
        protected override void ProjectileAttackTarget(Enemy target, float damage)
        {
            //TODO buffStack 10 될떄마다 골드 제공
            if (goldBuffStack == 10) { }
            base.ProjectileAttackTarget(target, damage);
        }

        protected override void InitBuffStack()
        {
            buffStackDelegate = (() =>
            {
                if (goldBuffStack >= 10) goldBuffStack = 1;
                else goldBuffStack++;
            });
        }


        protected override void AddDebuff()
        {

        }
    }
}