using PokerDefense.Data;
using PokerDefense.Managers;
using PokerDefense.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PokerDefense.Utils.Define;

namespace PokerDefense.Enemies
{
    public class Buff
    {
        public float weakDebuffRatio { get; private set; }
        public float slowDebuffRatio { get; private set; }

        public Buff(float weak = 0, float slow = 0)
        {
            this.weakDebuffRatio = weak;
            this.slowDebuffRatio = slow;
        }
    }
}