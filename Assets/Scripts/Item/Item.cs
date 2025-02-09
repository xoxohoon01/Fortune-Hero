using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;

public class Item
{
    public ItemData data;
    public int amount;
    public int upgrade;

    public Item(int id = 0, int newItemAmount = 1)
    {
        if (id != 0)
        {
            data = DataManager.Instance.Item.Get(id);
            amount = newItemAmount;
        }
        else
        {
            data = null;
            amount = 0;
        }
    }
}
