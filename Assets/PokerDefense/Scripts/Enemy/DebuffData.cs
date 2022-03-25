using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static PokerDefense.Utils.Define;

namespace PokerDefense.Enemies
{
    public class DebuffData
    {
        public DebuffData(Debuff debuff, float time, float percent)
        {
            this.debuff = debuff;
            this.debuffTime = time;
            this.debuffPercent = percent;
        }

        public Debuff debuff;          // 디버프 종류
        public float debuffTime;       // 디버프 시간
        public float debuffPercent;    // 디버프 강도
    }
}
