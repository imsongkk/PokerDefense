using System;

namespace PokerDefense.Data
{
    [Serializable]
    public class ItemData // 아이템 원시 데이터
    {
        public int itemId; // ItemIdData에서 가져옴
        public string itemName;
        public int price;
        public bool hasSlot;
    }
}
