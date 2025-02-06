using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;

public class HeroDataManager : HeroData
{
    public List<HeroData> Get()
    {
        return HeroDataList;
    }
    
    public HeroData Get(int id)
    {
        return HeroDataMap[id];
    }
}
