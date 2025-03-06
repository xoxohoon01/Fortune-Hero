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

    public int currentHeroNumber = -1;
    public int currentHeroInventoryNumber = -1;

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

            scroll.content.GetChild(i).GetComponent<HeroItemInventorySlot>().Initialize(item, i);
            count++;
        }
        if (GameManager.Instance.itemInventory.itemDatas.Count < 300)
        {
            for (int i = count; i < 300; i++)
            {
                scroll.content.GetChild(i).GetComponent<HeroItemInventorySlot>().Initialize(new Item(), i);
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
                scroll.content.GetChild(i).GetComponent<HeroItemInventorySlot>().Initialize(item, i);
            }
            else
            {
                scroll.content.GetChild(i).GetComponent<HeroItemInventorySlot>().Initialize(new Item(), i);
            }

            count++;
        }
        if (GameManager.Instance.itemInventory.itemDatas.Count < 300)
        {
            for (int i = count; i < 300; i++)
            {
                scroll.content.GetChild(i).GetComponent<HeroItemInventorySlot>().Initialize(new Item(), i);
            }
        }
    }

    public void UpdateSlot()
    {
        if (currentHeroNumber != -1)
            ChangeHero(currentHeroNumber);
        else if (currentHeroInventoryNumber != -1)
            ChangeHeroInventory(currentHeroInventoryNumber);

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
            Instantiate(Resources.Load<HeroItemInventorySlot>("UI/HeroItemInventorySlot"), scroll.content).Initialize(new Item(), i);
        }

        ChangeAll();
        isInitialized = true;
    }

    public void ChangeHero(int number)
    {
        currentHeroNumber = number;
        currentHeroInventoryNumber = -1;
        Hero currentHero = GameManager.Instance.heroInventory.hero[number];
        currentHeroSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Heroes/" + DataManager.Instance.Hero.Get(currentHero.ID).name);

        heroName.text = DataManager.Instance.Hero.Get(currentHero.ID).name;
        WeaponSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Weapon != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Weapon.id).name}") : null;
        GloveSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Glove != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Glove.id).name}") : null;
        RingSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Ring != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Ring.id).name}") : null;
        NecklessSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Neckless != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Neckless.id).name}") : null;
        HelmetSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Helmet != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Helmet.id).name}") : null;
        TopSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Top != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Top.id).name}") : null;
        BottomSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Bottom != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Bottom.id).name}") : null;
        ShoesSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Shoes != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Shoes.id).name}") : null;
        ArtifactSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Artifact != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Artifact.id).name}") : null;
    }

    public void ChangeHeroInventory(int number)
    {
        currentHeroNumber = -1;
        currentHeroInventoryNumber = number;
        Hero currentHero = GameManager.Instance.heroInventory.heroDatas[number];
        currentHeroSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Heroes/" + DataManager.Instance.Hero.Get(currentHero.ID).name);

        heroName.text = DataManager.Instance.Hero.Get(currentHero.ID).name;
        WeaponSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Weapon != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Weapon.id).name}") : null;
        GloveSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Glove != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Glove.id).name}") : null;
        RingSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Ring != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Ring.id).name}") : null;
        NecklessSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Neckless != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Neckless.id).name}") : null;
        HelmetSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Helmet != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Helmet.id).name}") : null;
        TopSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Top != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Top.id).name}") : null;
        BottomSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Bottom != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Bottom.id).name}") : null;
        ShoesSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Shoes != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Shoes.id).name}") : null;
        ArtifactSlot.transform.GetChild(0).GetComponent<Image>().sprite = currentHero.Artifact != null ? Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(currentHero.Artifact.id).name}") : null;
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
