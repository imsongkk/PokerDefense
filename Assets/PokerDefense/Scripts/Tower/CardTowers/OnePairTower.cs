using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Managers;
using static PokerDefense.Managers.TowerManager;
using PokerDefense.Utils;

namespace PokerDefense.Towers
{
    public class OnePairTower : Tower
    {
        protected override void Attack()
        {
            Debug.Log("OnePair Attack");
            float calculatedDamage = Define.CalculateDamage(Define.AttackType.AD, Define.EnemyType.Middle, TowerIndivData.Damage);
            if(enemies.Count > 0)
            {
                enemies[0].OnDamage(calculatedDamage);
            }
        }

        protected override void DamageCalculate()
        {
            base.DamageCalculate();
        }
    }
}