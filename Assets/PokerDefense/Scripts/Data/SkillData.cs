using System;

namespace PokerDefense.Data
{
    //* 손패의 족보에 따라 결정되는 불변한 수치
    [Serializable]
    public class SkillData
    {
        public float skillTime;
        public float coolTime;
        public SkillCostInfo skillCostInfo;
        public bool isInCoolTime = false;
    }

    [Serializable]
    public class SkillCostInfo
    {
        public string skillCostTarget;
        public int skillCost;
    }
}