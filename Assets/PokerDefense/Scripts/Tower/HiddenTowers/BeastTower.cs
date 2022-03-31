using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Managers;
using PokerDefense.Enemies;
using static PokerDefense.Managers.TowerManager;
using static PokerDefense.Utils.Define;

namespace PokerDefense.Towers
{
    public class BeastTower : DirectAttackTower
    {
        private static DebuffData slowDebuffData = new DebuffData(Debuff.Slow, 6f, 0.5f);

        protected override void InitDebuff()
        {
            base.InitDebuff();
            debuffDatas.Add(slowDebuffData);
        }

        protected override void DirectAttackTarget(Enemy target, float damage)
        {
            base.DirectAttackTarget(target, damage);
        }
    }
}