using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Managers;
using static PokerDefense.Managers.TowerManager;

namespace PokerDefense.Towers
{
    public class HighCardTower : Tower
    {
        protected override void Attack()
        {
            Debug.Log("HighCard Attack");
        }

        protected override void DamageCalculate()
        {
            base.DamageCalculate();
        }
    }
}