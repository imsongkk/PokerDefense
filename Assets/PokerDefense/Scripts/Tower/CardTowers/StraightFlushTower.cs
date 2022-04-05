using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Enemies;
using PokerDefense.Managers;
using static PokerDefense.Utils.Define;
using static PokerDefense.Managers.TowerManager;

namespace PokerDefense.Towers
{
    public class StraightFlushTower : FlushTower
    {
        DebuffData weakDebuffData;
        protected override void ProjectileAttackTarget(Enemy target, float damage)
        {
            base.ProjectileAttackTarget(target, damage);
        }

        protected override void AddDebuff()
        {
            weakDebuffData = new DebuffData(Debuff.Weak, 5f, .3f);
            projectilePool.AddDebuff(weakDebuffData);
        }
    }
}