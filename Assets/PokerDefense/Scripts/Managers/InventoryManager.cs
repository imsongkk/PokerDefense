using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager
{
    private int heart;
    private int gold;
    private int chance;

    public int Heart
    {
        get => heart;
        set
        {
            heart = value;
        }
    }
    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
        }
    }
    public int Chance
    {
        get => chance;
        set
        {
            chance = value;
        }
    }

    public void InitInventoryManager()
    {

    }
}
