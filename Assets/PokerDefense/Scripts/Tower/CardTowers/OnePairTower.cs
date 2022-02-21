using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Managers;
using static PokerDefense.Managers.TowerManager;

namespace PokerDefense.Towers
{
    public class OnePairTower : Tower
    {
        protected override void Attack()
        {
            Debug.Log("OnePair Attack");
        }

        protected override void DamageCalculate()
        {
            base.DamageCalculate();
        }
    }
}