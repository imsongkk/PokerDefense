using System;

namespace PokerDefense.Data
{
    [Serializable]
    public class ItemData // 아이템 원시 데이터
    {
        public string itemId;
        public int price;
        public bool hasSlot;
    }
}
