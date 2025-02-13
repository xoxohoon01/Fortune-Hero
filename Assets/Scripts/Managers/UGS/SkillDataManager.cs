using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;

public class SkillDataManager : SkillData
{
    public List<SkillData> Get()
    {
        return SkillDataList;
    }
    
    public SkillData Get(int id)
    {
        if (SkillDataMap.TryGetValue(id, out SkillData data))
            return data;
        else
            return null;
    }
}
