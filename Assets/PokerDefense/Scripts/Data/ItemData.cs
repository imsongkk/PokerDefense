using System;

namespace PokerDefense.Data
{
    [Serializable]
    public class ItemData // Heart, Chance �� ��Ÿ ��� ������ ������
    {
        public int itemId; // ItemIdData���� ������
        public string itemName;
        public int price;
        public bool hasSlot;
    }
}
