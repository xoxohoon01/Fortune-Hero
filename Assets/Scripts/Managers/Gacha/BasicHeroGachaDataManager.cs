using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;

public class BasicHeroGachaDataManager : BasicHeroGachaData
{
    public List<KeyValuePair<int, float>> Get()
    {
        List<KeyValuePair<int, float>> newList = new List<KeyValuePair<int, float>>();
        foreach (var item in BasicHeroGachaDataList)
        {
            KeyValuePair<int, float> newPair = new KeyValuePair<int, float>(item.ID, item.weight);
            newList.Add(newPair);
        }

        return newList;
    }

    public BasicHeroGachaData Get(int id)
    {
        if (BasicHeroGachaDataMap.TryGetValue(id, out BasicHeroGachaData data))
            return data;
        else
            return null;
    }

    
}
