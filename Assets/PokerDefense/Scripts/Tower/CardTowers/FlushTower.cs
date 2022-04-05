using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Enemies;
using PokerDefense.Managers;
using static PokerDefense.Managers.TowerManager;
using static PokerDefense.Utils.Define;


namespace PokerDefense.Towers
{
    public class FlushTower : ProjectileTower
    {
        public override void InitTower(string towerName, TowerType towerType, int topCard, int index)
        {
            FlushBuff();
            base.InitTower(towerName, towerType, topCard, index);
        }

        protected override void ProjectileAttackTarget(Enemy target, float damage)
        {
            base.ProjectileAttackTarget(target, damage);
        }

        protected override void AddDebuff()
        {
            base.AddDebuff();
        }

        protected virtual void FlushBuff()
        {

        }
    }
}