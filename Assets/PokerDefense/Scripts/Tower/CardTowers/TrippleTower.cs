using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Managers;
using static PokerDefense.Managers.TowerManager;
using static PokerDefense.Utils.Define;
using PokerDefense.Enemies;

namespace PokerDefense.Towers
{
    public class TrippleTower : Tower
    {
        private int trippleBuffStack = 1;

        [SerializeField]
        private float trippleBuffPercent = .3f;

        protected override void DirectAttackTarget(Enemy target, float damage)
        {
            Debug.Log($"{trippleBuffStack}번째 공격!");
            if (trippleBuffStack == 3)
            {
                damage *= (1 + trippleBuffPercent);
                trippleBuffStack = 1;
            }
            else { trippleBuffStack++; }
            base.DirectAttackTarget(target, damage);
        }

        protected override void DebuffTarget(Enemy target)
        {
            base.DebuffTarget(target);
        }
    }
}