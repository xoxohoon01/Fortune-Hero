using System.Collections;
using UnityEngine;
using DataTable_FortuneHero;
using UnityEngine.UI;

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
        WeightedRandom newGacha = new WeightedRandom(DataManager.Instance.BasicHeroGacha.Get());
        int rewardID = newGacha.GetValue();
        Debug.Log(rewardID);
        GameManager.Instance.GetHero(rewardID, 0);
    }

    public void Gacha10times()
    {
        for (int i = 0; i< 10; i++)
        {
            WeightedRandom newGacha = new WeightedRandom(DataManager.Instance.BasicHeroGacha.Get());
            int rewardID = newGacha.GetValue();
            GameManager.Instance.GetHero(rewardID, 0);
        }
    }

    private void Start()
    {
        Initialize();
    }
}
