using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;

public class ShopDataManager : ShopData
{
    public List<ShopData> Get()
    {
        return ShopDataList;
    }

    public ShopData Get(int id)
    {
        return ShopDataMap[id];
    }
}
