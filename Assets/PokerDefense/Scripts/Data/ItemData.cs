using System;

namespace PokerDefense.Data
{
    [Serializable]
    public class ItemData // ������ ���� ������
    {
        public int itemId; // ItemIdData���� ������
        public string itemName;
        public int price;
        public bool hasSlot;
    }
}
