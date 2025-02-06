using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;
using System.Runtime.InteropServices.WindowsRuntime;

public class HeroDataManager : HeroData
{
    public List<HeroData> Get()
    {
        return HeroDataList;
    }
    
    public HeroData Get(int id)
    {
        if (HeroDataMap.TryGetValue(id, out HeroData data))
            return data;
        else
            return null;
    }
}
