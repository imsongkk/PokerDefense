using System;

namespace PokerDefense.Data
{
    [Serializable]
    public class ItemData // Heart, Chance 및 기타 사용 가능한 아이템
    {
        public int itemId; // ItemIdData에서 가져옴
        public string itemName;
        public int price;
        public bool hasSlot;
    }
}
