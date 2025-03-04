using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;

public class GachaShopDataManager : GachaShopData
{
    public List<GachaShopData> Get()
    {
        return GachaShopDataList;
    }

    public GachaShopData Get(int id)
    {
        return GachaShopDataMap[id];
    }
}
