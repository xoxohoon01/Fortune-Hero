using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGS;
using DataTable_FortuneHero;

public class DataManager : Singleton<DataManager>
{
    public HeroDataManager Hero;
    public ItemDataManager Item;
    public MonsterDataManager Monster;
    public StageDataManager Stage;
    public SkillDataManager Skill;

    public void Initialize()
    {
        UnityGoogleSheet.LoadAllData();
        Hero = new HeroDataManager();
        Item = new ItemDataManager();
        Monster = new MonsterDataManager();
        Stage = new StageDataManager();
        Skill = new SkillDataManager();
    }
}
