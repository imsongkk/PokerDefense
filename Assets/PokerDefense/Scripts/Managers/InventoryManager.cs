using PokerDefense.Data;
using PokerDefense.Managers;
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

    public UnityEvent<int, int> ItemAdded = new UnityEvent<int, int>(); // itemId, count
    public UnityEvent<int> ItemUsed = new UnityEvent<int>(); // itemId
    public UnityEvent<int> ItemDeleted = new UnityEvent<int>(); // itemId

    public InventoryData InventoryData { get; private set; }

    UI_InGameScene ui_InGameScene;

    public void InitInventoryManager(UI_InGameScene _ui_InGameScene)
    {
        ui_InGameScene = _ui_InGameScene;

        InitInventory();
        InitItemDict();
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

    private bool IsInItemDict(int itemId)
        => ItemDict.ContainsKey(itemId);

    public void AddItem(int itemId, int count)
    {
        if (IsInItemDict(itemId))
            ItemDict[itemId] += count;
        else
            ItemDict.Add(itemId, count);

        ItemAdded?.Invoke(itemId, count);
    }

    public void UseItem(int itemId)
    {
        if (IsInItemDict(itemId))
        {
            ItemDict[itemId] -= 1;
            ItemUsed?.Invoke(itemId);
        }
        if (IsZeroCount(itemId))
            ItemDeleted?.Invoke(itemId);
    }

    private bool IsZeroCount(int itemId)
    {
        ItemDict.TryGetValue(itemId, out var count);
        return count == 0;
    }

    public InventoryData GetSaveData()
    {
        InventoryData.itemHave = ItemDict;
        return InventoryData;
    }
}
