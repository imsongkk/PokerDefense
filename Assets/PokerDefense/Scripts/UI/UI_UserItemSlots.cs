using PokerDefense.Managers;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace PokerDefense.UI
{
    public class UI_UserItemSlots : UI_Base
    {
        [SerializeField] Transform content;
        [SerializeField] ScrollRect scrollRect;

        Dictionary<int, UI_UserItem> userItemDict = new Dictionary<int, UI_UserItem>(); // key : itemId

        public override void Init() { }

        public void InitItemSlotsUI()
        {
            InitUserItemsUI();

            GameManager.Inventory.ItemPurchased.AddListener(ItemPurchased);
            GameManager.Inventory.ItemUsed.AddListener(ItemUsed);
            GameManager.Inventory.ItemDeleted.AddListener(ItemDeleted);
        }

        private void InitUserItemsUI()
        {
            var dict = GameManager.Inventory.ItemDict;
            foreach (var item in dict)
            {
                GameManager.Data.ItemDataDict.TryGetValue(item.Key, out var itemData);

                if(itemData.hasSlot) // slot에 들어가는 item
                    MakeUserItemUI(item.Key);
                else // slot에 들어가지 않는 item ex)Heart, Chance등등
                {

                }
            }
        }

        private void MakeUserItemUI(int itemId)
        {
            UI_UserItem userItem = GameManager.Resource.Instantiate("UI/UI_UserItem", content).GetComponent<UI_UserItem>();
            userItemDict.Add(itemId, userItem);
            userItem.InitItem(this, itemId);
        }

        private void ItemDeleted(int itemId)
        {
            GameManager.Data.ItemDataDict.TryGetValue(itemId, out var itemData);

            if(itemData.hasSlot) // Heart, Gold, Chance 제외
            {
                userItemDict.TryGetValue(itemId, out var uI_UserItem);
                uI_UserItem.ItemDeleted();
                userItemDict.Remove(itemId);
            }
        }

        private void ItemPurchased(int itemId, int count)
        {
            GameManager.Data.ItemDataDict.TryGetValue(itemId, out var itemData);

            if(itemData.hasSlot) // Heart, Gold, Chance 제외
            {
                if( ! userItemDict.TryGetValue(itemId, out var uI_UserItem))
                {
                    MakeUserItemUI(itemId);
                    uI_UserItem = userItemDict[itemId];
                }
                uI_UserItem.ItemPurchased(count);
            }
        }

        private void ItemUsed(int itemId)
        {
            GameManager.Data.ItemDataDict.TryGetValue(itemId, out var itemData);

            if(itemData.hasSlot) // Heart, Gold, Chance 제외
            {
                if( ! userItemDict.TryGetValue(itemId, out var uI_UserItem))
                {
                    MakeUserItemUI(itemId);
                    uI_UserItem = userItemDict[itemId];
                }
                uI_UserItem.ItemUsed();
            }
        }
    }
}
