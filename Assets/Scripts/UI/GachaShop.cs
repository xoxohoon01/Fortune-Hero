using System.Collections;
using UnityEngine;
using DataTable_FortuneHero;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GachaShop : UIBase
{
    public ScrollRect gachaCatalog;
    public Image gachaPage;
    public TMP_Text currentGold;

    public override void Initialize()
    {
        Initialize();
    }

    public void GachaOnce()
    {
        if (GameManager.Instance.itemInventory.gold >= 100)
        {
            GameManager.Instance.itemInventory.gold -= 100;

            List<Hero> newHeroes = new List<Hero>();

            WeightedRandom newGacha = new WeightedRandom(DataManager.Instance.BasicHeroGacha.Get());
            int rewardID = newGacha.GetValue();
            Debug.Log(rewardID);
            GameManager.Instance.GetHero(rewardID, 0);
            newHeroes.Add(new Hero(rewardID));

            UIManager.Instance.Show<GachaReward>("FloatingUI");
            UIManager.Instance.Get<GachaReward>().Initialize(newHeroes);

            UpdateGachaShop();
        }
    }

    public void Gacha10times()
    {
        if (GameManager.Instance.itemInventory.gold >= 950)
        {
            GameManager.Instance.itemInventory.gold -= 950;

            List<Hero> newHeroes = new List<Hero>();
            for (int i = 0; i < 10; i++)
            {
                WeightedRandom newGacha = new WeightedRandom(DataManager.Instance.BasicHeroGacha.Get());
                int rewardID = newGacha.GetValue();
                GameManager.Instance.GetHero(rewardID, 0);
                newHeroes.Add(new Hero(rewardID));
            }

            UIManager.Instance.Show<GachaReward>("FloatingUI");
            UIManager.Instance.Get<GachaReward>().Initialize(newHeroes);

            UpdateGachaShop();
        }
    }

    public void UpdateGachaShop()
    {
        currentGold.text = GameManager.Instance.itemInventory.gold.ToString();
    }

    private void Start()
    {
        base.Initialize();
    }
}
