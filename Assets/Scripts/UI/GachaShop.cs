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

    public TMP_Text onceGold;
    public TMP_Text tenGold;

    private int currentGacha;
    private int valuePerOnce;
    private int valuePerTen;

    public override void Initialize()
    {
        for (int i = 0; i < gachaCatalog.content.childCount; i++)
        {
            Destroy(gachaCatalog.content.GetChild(i).gameObject);
        }

        for (int i = 0; i < DataManager.Instance.GachaShop.Get().Count; i++)
        {
            Instantiate(Resources.Load<GachaCatalogSlot>("UI/GachaCatalogSlot"), gachaCatalog.content).Initialize(6001 + i);
        }

        currentGacha = DataManager.Instance.GachaShop.Get()[0].ID;
        SelectCacha(currentGacha);
        UpdateGachaShop();
    }

    public void GachaOnce()
    {
        if (GameManager.Instance.itemInventory.gold >= valuePerOnce)
        {
            GameManager.Instance.itemInventory.gold -= valuePerOnce;
            WeightedRandom newGacha;
            int rewardID;

            switch (currentGacha)
            {
                case 6001:
                    List<Hero> newHeroes = new List<Hero>();

                    newGacha = new WeightedRandom(DataManager.Instance.BasicHeroGacha.Get());
                    rewardID = newGacha.GetValue();
                    Debug.Log(rewardID);
                    GameManager.Instance.GetHero(rewardID, 0);
                    newHeroes.Add(new Hero(rewardID));

                    UIManager.Instance.Show<GachaReward>("FloatingUI");
                    UIManager.Instance.Get<GachaReward>().Initialize(newHeroes);
                    break;

                case 6002:
                    List<Item> newItems = new List<Item>();

                    newGacha = new WeightedRandom(DataManager.Instance.BasicItemGacha.Get());
                    rewardID = newGacha.GetValue();
                    Debug.Log(rewardID);
                    GameManager.Instance.GetItem(rewardID, 1);
                    newItems.Add(new Item(rewardID));

                    UIManager.Instance.Show<GachaReward>("FloatingUI");
                    UIManager.Instance.Get<GachaReward>().Initialize(newItems);
                    break;
            }
            

            UpdateGachaShop();
        }
    }

    public void Gacha10times()
    {
        if (GameManager.Instance.itemInventory.gold >= valuePerTen)
        {
            GameManager.Instance.itemInventory.gold -= valuePerTen;

            WeightedRandom newGacha;
            int rewardID;

            switch (currentGacha)
            {
                case 6001:
                    List<Hero> newHeroes = new List<Hero>();
                    for (int i = 0; i < 10; i++)
                    {
                        newGacha = new WeightedRandom(DataManager.Instance.BasicHeroGacha.Get());
                        rewardID = newGacha.GetValue();
                        GameManager.Instance.GetHero(rewardID, 0);
                        newHeroes.Add(new Hero(rewardID));
                    }

                    UIManager.Instance.Show<GachaReward>("FloatingUI");
                    UIManager.Instance.Get<GachaReward>().Initialize(newHeroes);
                    break;

                case 6002:
                    List<Item> newItems = new List<Item>();
                    for (int i = 0; i < 10; i++)
                    {
                        newGacha = new WeightedRandom(DataManager.Instance.BasicItemGacha.Get());
                        rewardID = newGacha.GetValue();
                        GameManager.Instance.GetItem(rewardID, 1);
                        newItems.Add(new Item(rewardID));
                    }

                    UIManager.Instance.Show<GachaReward>("FloatingUI");
                    UIManager.Instance.Get<GachaReward>().Initialize(newItems);
                    break;
            }

            

            UpdateGachaShop();
        }
    }

    public void UpdateGachaShop()
    {
        currentGold.text = GameManager.Instance.itemInventory.gold.ToString();
        onceGold.text = valuePerOnce.ToString();
        tenGold.text = valuePerTen.ToString();
    }

    public void SelectCacha(int gachaID)
    {
        currentGacha = gachaID;
        GachaShopData currentGachaData = DataManager.Instance.GachaShop.Get(currentGacha);
        gachaPage.sprite = Resources.Load<Sprite>($"Sprites/GachaPages/{currentGachaData.name}");
        valuePerOnce = currentGachaData.valuePerOnce;
        valuePerTen = currentGachaData.valuePerTen;
        UpdateGachaShop();
    }

    private void Start()
    {
        base.Initialize();
    }
}
