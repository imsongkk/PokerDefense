using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Managers;
using PokerDefense.Enemies;
using static PokerDefense.Managers.TowerManager;
using System.Collections;
using System.Collections.Generic;
using PokerDefense.Utils;
using System;
using PokerDefense.UI.Popup;
using static PokerDefense.Utils.Define;

namespace PokerDefense.Towers
{
    public class DebuffTestTower : Tower
    {
        private DebuffData slowDebuff;
        private DebuffData weakDebuff;

        private void Awake()
        {
            slowDebuff = new DebuffData(Debuff.Slow, 5f, .5f);
            weakDebuff = new DebuffData(Debuff.Weak, 3f, .3f);
        }

        protected override void DebuffTarget(Enemy target)
        {
            base.DebuffTarget(target);
            target.SetDebuff(slowDebuff);
            target.SetDebuff(weakDebuff);
        }
    }
}