using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HeroItemInventory : UIBase
{
    public TMP_Text heroName;
    public Image currentHeroSlot;

    public Image WeaponSlot;
    public Image GloveSlot;
    public Image RingSlot;
    public Image NecklessSlot;
    public Image HelmetSlot;
    public Image TopSlot;
    public Image BottomSlot;
    public Image ShoesSlot;
    public Image ArtifactSlot;

    public ScrollRect scroll;

    private int currentHeroNumber;

    private int page;
    private bool isInitialized;

    public override void Hide()
    {
        scroll.verticalNormalizedPosition = 1.0f;
    }

    public void ChangeAll()
    {
        scroll.verticalNormalizedPosition = 1.0f;
        page = 0;

        int count = 0;
        for (int i = 0; i < GameManager.Instance.itemInventory.itemDatas.Count; i++)
        {
            Item item = GameManager.Instance.itemInventory.itemDatas[i];

            scroll.content.GetChild(i).GetComponent<ItemInventorySlot>().Initialize(item, i);
            count++;
        }
        if (GameManager.Instance.itemInventory.itemDatas.Count < 300)
        {
            for (int i = count; i < 300; i++)
            {
                scroll.content.GetChild(i).GetComponent<ItemInventorySlot>().Initialize(new Item(), i);
            }
        }
    }

    public void ChangeType(int type)
    {
        scroll.verticalNormalizedPosition = 1.0f;
        page = type;

        int count = 0;
        for (int i = 0; i < GameManager.Instance.itemInventory.itemDatas.Count; i++)
        {
            Item item = GameManager.Instance.itemInventory.itemDatas[i];

            if (item.id != 0 && DataManager.Instance.Item.Get(item.id).type == type)
            {
                scroll.content.GetChild(i).GetComponent<ItemInventorySlot>().Initialize(item, i);
            }
            else
            {
                scroll.content.GetChild(i).GetComponent<ItemInventorySlot>().Initialize(new Item(), i);
            }

            count++;
        }
        if (GameManager.Instance.itemInventory.itemDatas.Count < 300)
        {
            for (int i = count; i < 300; i++)
            {
                scroll.content.GetChild(i).GetComponent<ItemInventorySlot>().Initialize(new Item(), i);
            }
        }
    }

    public void UpdateSlot()
    {
        if (page == 0)
            ChangeAll();
        else
            ChangeType(page);
    }

    public override void Initialize()
    {
        base.Initialize();

        for (int i = 0; i < 300; i++)
        {
            Instantiate(Resources.Load<ItemInventorySlot>("UI/ItemInventorySlot"), scroll.content).Initialize(new Item(), i);
        }

        ChangeAll();
        isInitialized = true;
    }

    public void ChangeHero(Hero hero)
    {
        Hero currentHero = hero;
        currentHeroSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Heroes/" + DataManager.Instance.Hero.Get(hero.ID).name);

        heroName.text = DataManager.Instance.Hero.Get(hero.ID).name;
        WeaponSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = hero.Weapon != null ? DataManager.Instance.Item.Get(hero.Weapon.id).name : "";
        GloveSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = hero.Glove != null ? DataManager.Instance.Item.Get(hero.Glove.id).name : "";
        RingSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = hero.Ring != null ? DataManager.Instance.Item.Get(hero.Ring.id).name : "";
        NecklessSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = hero.Neckless != null ? DataManager.Instance.Item.Get(hero.Neckless.id).name : "";
        HelmetSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = hero.Helmet != null ? DataManager.Instance.Item.Get(hero.Helmet.id).name : "";
        TopSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = hero.Top != null ? DataManager.Instance.Item.Get(hero.Top.id).name : "";
        BottomSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = hero.Bottom != null ? DataManager.Instance.Item.Get(hero.Bottom.id).name : "";
        ShoesSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = hero.Shoes != null ? DataManager.Instance.Item.Get(hero.Shoes.id).name : "";
    }

    public void ChangeHeroInventory(int number)
    { 

    }

    private void OnEnable()
    {
        if (isInitialized)
            ChangeAll();
    }

    private void Start()
    {
        Initialize();
    }

}
