using System.Collections;
using UnityEngine;
using DataTable_FortuneHero;
using UnityEngine.UI;
using System.Collections.Generic;

public class GachaShop : UIBase
{
    public ScrollRect gachaCatalog;
    public Image gachaPage;

    public override void Initialize()
    {
        base.Initialize();
    }

    public void GachaOnce()
    {
        List<Hero> newHeroes = new List<Hero>();

        WeightedRandom newGacha = new WeightedRandom(DataManager.Instance.BasicHeroGacha.Get());
        int rewardID = newGacha.GetValue();
        Debug.Log(rewardID);
        GameManager.Instance.GetHero(rewardID, 0);
        newHeroes.Add(new Hero(rewardID));

        UIManager.Instance.Show<GachaReward>("FloatingUI");
        UIManager.Instance.Get<GachaReward>().Initialize(newHeroes);
    }

    public void Gacha10times()
    {
        List<Hero> newHeroes = new List<Hero>();
        for (int i = 0; i< 10; i++)
        {
            WeightedRandom newGacha = new WeightedRandom(DataManager.Instance.BasicHeroGacha.Get());
            int rewardID = newGacha.GetValue();
            GameManager.Instance.GetHero(rewardID, 0);
            newHeroes.Add(new Hero(rewardID));
        }

        UIManager.Instance.Show<GachaReward>("FloatingUI");
        UIManager.Instance.Get<GachaReward>().Initialize(newHeroes);
    }

    private void Start()
    {
        Initialize();
    }
}
