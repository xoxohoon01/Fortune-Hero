using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;

[System.Serializable]
public class Item
{
    public int id;
    public int amount;
    public int upgrade;
    public int equipedHeroNumber = -1;
    public int equipedHeroInventoryNumber = -1;

    public Item(int itemID = 0, int newItemAmount = 1)
    {
        id = itemID;
        if (itemID != 0)
        {
            amount = newItemAmount;
        }
        else
        {
            amount = 0;
        }
    }

    public bool IsEquiped()
    {
        if (equipedHeroNumber != -1)
            return true;
        else
            return false;
    }
}
