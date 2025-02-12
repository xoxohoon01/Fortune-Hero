using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;
using System.Runtime.InteropServices.WindowsRuntime;

public class MonsterDataManager : MonsterData
{
    public List<MonsterData> Get()
    {
        return MonsterDataList;
    }
    
    public MonsterData Get(int id)
    {
        if (MonsterDataMap.TryGetValue(id, out MonsterData data))
            return data;
        else
            return null;
    }
}
