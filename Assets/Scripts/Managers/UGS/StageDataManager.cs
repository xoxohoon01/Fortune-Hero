using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;

public class StageDataManager : StageData
{
    public List<StageData> Get()
    {
        return StageDataList;
    }
    
    public StageData Get(int id)
    {
        if (StageDataMap.TryGetValue(id, out StageData data))
            return data;
        else
            return null;
    }
}
