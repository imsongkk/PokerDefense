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
        [SerializeField]
        protected float flushBuffRange;
        [SerializeField]
        protected float flushBuffPercent;
        protected FlushBuff flushBuff;

        public override void InitTower(string towerName, TowerType towerType, int topCard, int index)
        {
            base.InitTower(towerName, towerType, topCard, index);

            InitFlushBuff();
        }

        protected virtual void InitFlushBuff()
        {
            flushBuff = transform.GetChild(2).GetComponent<FlushBuff>();
            flushBuff.InitFlushBuff(flushBuffRange, flushBuffPercent);
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