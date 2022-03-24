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
                InGameManager.Round.GameOver();
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


    public Dictionary<string, int> ItemDict { get; private set; } = new Dictionary<string, int> (); // key : itemId, value : count

    public UnityEvent<string, int> ItemPurchased = new UnityEvent<string, int>(); // itemId, count
    public UnityEvent<string> ItemUsed = new UnityEvent<string>(); // itemId
    public UnityEvent<string> ItemDeleted = new UnityEvent<string>(); // itemId

    public InventoryData InventoryData { get; private set; }

    UI_InGameScene ui_InGameScene;

    public InventoryManager()
    {
        /*
        Debug.Log("INVENINIT");
        ItemPurchased.AddListener(OnItemPurchased);
        ItemUsed.AddListener(OnItemUsed);
        ItemDeleted.AddListener(OnItemDeleted);

        InitInventory();
        InitItemDict();

        ui_InGameScene = InGameManager.UI_InGameScene;
        */
    }

    public void InitInventoryManager()
    {
        ui_InGameScene = InGameManager.UI_InGameScene;
        ItemPurchased.AddListener(OnItemPurchased);
        ItemUsed.AddListener(OnItemUsed);
        ItemDeleted.AddListener(OnItemDeleted);

        InitInventory();
        InitItemDict();
    }

    private void OnItemPurchased(string itemId, int count)
    {
        if (ItemDict.ContainsKey(itemId))
            ItemDict[itemId] += count;
        else
            ItemDict.Add(itemId, count);
    }

    private void OnItemUsed(string itemId)
    {
        if (ItemDict.ContainsKey(itemId))
        {
            ItemDict[itemId] -= 1;
        }
        else // TODO : 에러대응
            return;
    }

    private void OnItemDeleted(string itemId)
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

            string hardNess = InGameManager.Round.HardNess;
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

    public void OnClickPuchase(string itemId, int count)
    {
        GameManager.Data.ItemDataDict.TryGetValue(itemId, out var itemData);

        if(Gold >= itemData.price)
        {
            Gold -= itemData.price;

            if (itemId == "heart")
                Heart += count;
            else if (itemId == "chance")
                Chance += count;

            ItemPurchased?.Invoke(itemId, count);
        }
        else
            GameManager.UI.ShowPopupUI<UI_ShopErrorPopup>();
    }

    public void OnClickUse(string itemId)
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
