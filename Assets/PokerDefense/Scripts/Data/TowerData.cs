using System;

namespace PokerDefense.Data
{
    [Serializable]
    public class TowerData
    {
        public float damage;
        public float attackSpeed;
        public float attackRange;
        public int price; // ���ȱ� ����
        public string towerName; // ��ɷ� sprite�� ��������
        public string towerType;
    }
}