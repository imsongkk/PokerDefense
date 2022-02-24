using System;

namespace PokerDefense.Data
{
    //* 손패의 족보에 따라 결정되는 불변한 수치
    [Serializable]
    public class SkillData
    {
        public int skillIndex;
        public string skillName;

        public float skillTime;
        public float coolTime;
        public float slowPercent;
        public int skillCost;
        public float skillRange;
        public float skillDamage;
        public float skillTic;

        //public int skillLevel;
        public bool isInCoolTime = false;
    }
}