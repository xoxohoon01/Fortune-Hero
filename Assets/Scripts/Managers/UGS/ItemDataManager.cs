using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;

public class ItemDataManager : ItemData
{
    public List<ItemData> Get()
    {
        return ItemDataList;
    }
    
    public ItemData Get(int id)
    {
        if (ItemDataMap.TryGetValue(id, out ItemData data))
            return data;
        else
            return null;
    }
}
