using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Enemies;
using PokerDefense.Managers;
using static PokerDefense.Managers.TowerManager;
using static PokerDefense.Utils.Define;


namespace PokerDefense.Towers
{
    public class FourCardsTower : DirectAttackTower
    {
        private int fourCardsBuffStack = 1;
        private float fourCardsBuffPercent = 1f;

        public Sprite magicArea;

        protected override void InitDebuff()
        {
            base.InitDebuff();
        }

        protected override void DirectAttackTarget(Enemy target, float damage)
        {
            if (fourCardsBuffStack >= 4)
            {
                damage *= (1 + fourCardsBuffPercent);
                fourCardsBuffStack = 1;
            }
            else { fourCardsBuffStack++; }
            base.DirectAttackTarget(target, damage);
        }

    }
}