using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGS;
using DataTable_FortuneHero;

public class DataManager : Singleton<DataManager>
{
    public HeroDataManager Hero;
    public ItemDataManager Item;

    public void Initialize()
    {
        UnityGoogleSheet.LoadAllData();
        Hero = new HeroDataManager();
        Item = new ItemDataManager();
    }
}
