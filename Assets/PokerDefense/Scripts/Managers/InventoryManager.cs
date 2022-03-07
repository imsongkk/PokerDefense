using PokerDefense.Data;
using PokerDefense.Managers;
using PokerDefense.UI.Popup;
using PokerDefense.UI.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager
{
    public int Heart
    {
        get => InventoryData.heart;
        set
        {
            InventoryData.heart = value;
            ui_InGameScene.SetHeartText(InventoryData.heart);
            if (InventoryData.heart <= 0)
                GameManager.Round.GameOver();
        }
    }
    public int Gold
    {
        get => InventoryData.gold;
        set
        {
            InventoryData.gold = value;
            ui_InGameScene.SetGoldText(InventoryData.gold);
        }
    }
    public int Chance
    {
        get => InventoryData.chance;
        set
        {
            InventoryData.chance = value;
            ui_InGameScene.SetChanceText(InventoryData.chance);
        }
    }


    public Dictionary<int, int> ItemDict { get; private set; } = new Dictionary<int, int> (); // key : itemId, value : count

    public UnityEvent<int, int> ItemPurchased = new UnityEvent<int, int>(); // itemId, count
    public UnityEvent<int> ItemUsed = new UnityEvent<int>(); // itemId
    public UnityEvent<int> ItemDeleted = new UnityEvent<int>(); // itemId

    public InventoryData InventoryData { get; private set; }

    UI_InGameScene ui_InGameScene;

    public void InitInventoryManager(UI_InGameScene _ui_InGameScene)
    {
        ui_InGameScene = _ui_InGameScene;

        ItemPurchased.AddListener(OnItemPurchased);
        ItemUsed.AddListener(OnItemUsed);
        ItemDeleted.AddListener(OnItemDeleted);

        InitInventory();
        InitItemDict();
    }

    private void OnItemPurchased(int itemId, int count)
    {
        if (ItemDict.ContainsKey(itemId))
            ItemDict[itemId] += count;
        else
            ItemDict.Add(itemId, count);
    }

    private void OnItemUsed(int itemId)
    {
        if (ItemDict.ContainsKey(itemId))
        {
            ItemDict[itemId] -= 1;
        }
        else // TODO : 에러대응
            return;
    }

    private void OnItemDeleted(int itemId)
    {
        if (ItemDict.ContainsKey(itemId))
            ItemDict.Remove(itemId);
    }

    private void InitInventory()
    {
        if(GameManager.Data.SlotData != null) // 로드 된 데이터가 있다면
            InventoryData = GameManager.Data.SlotData.inventory;
        else // 새 게임이면
        {
            InventoryData = new InventoryData();

            string hardNess = GameManager.Round.HardNess;
            GameManager.Data.HardNessDataDict.TryGetValue(hardNess, out var hardNessData);

            Heart = hardNessData.heart;
            Gold = hardNessData.gold;
            Chance = hardNessData.chance;
        }
    }

    private void InitItemDict()
    {
        foreach (var item in InventoryData.itemHave)
            ItemDict.Add(item.Key, item.Value);
    }

    public void OnClickPuchase(int itemId, int count)
    {
        GameManager.Data.ItemDataDict.TryGetValue(itemId, out var itemData);

        if(Gold >= itemData.price)
        {
            Gold -= itemData.price;

            // TODO : 리팩토링 필요
            GameManager.Data.ItemNameDict.TryGetValue(itemId, out var itemName);

            if (itemName == "heart")
                Heart += count;
            else if (itemName == "chance")
                Chance += count;

            ItemPurchased?.Invoke(itemId, count);
        }
        else
            GameManager.UI.ShowPopupUI<UI_ShopErrorPopup>();
    }

    public void OnClickUse(int itemId)
    {
        ItemUsed?.Invoke(itemId);

        if (ItemDict[itemId] <= 0)
        {
            ItemDict.Remove(itemId);
            ItemDeleted?.Invoke(itemId);
        }
    }

    public InventoryData GetSaveData()
    {
        InventoryData.itemHave = ItemDict;
        return InventoryData;
    }
}
