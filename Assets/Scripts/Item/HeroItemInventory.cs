using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroItemInventory : UIBase
{
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

    public override void Hide()
    {
        scroll.verticalNormalizedPosition = 1.0f;
    }

    public void ChangeAll()
    {
        scroll.verticalNormalizedPosition = 1.0f;

        for (int i = 0; i < scroll.content.childCount; i++)
        {
            Destroy(scroll.content.GetChild(i).gameObject);
        }

        if (GameManager.Instance.itemInventory.itemDatas.Count < 300)
        {
            int count = 0;
            for (int i = 0; i < GameManager.Instance.itemInventory.itemDatas.Count; i++)
            {
                Instantiate(Resources.Load<ItemInventorySlot>("UI/ItemInventorySlot"), scroll.content).Initialize(GameManager.Instance.itemInventory.itemDatas[i], GameManager.Instance.itemInventory.itemDatas[i].amount);
                count++;
            }
            for (int i = count; i < 300; i++)
            {
                Instantiate(Resources.Load<ItemInventorySlot>("UI/ItemInventorySlot"), scroll.content).Initialize(new Item(0), 0);
            }
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.itemInventory.itemDatas.Count; i++)
            {
                Instantiate(Resources.Load<ItemInventorySlot>("UI/ItemInventorySlot"), scroll.content).Initialize(GameManager.Instance.itemInventory.itemDatas[i], GameManager.Instance.itemInventory.itemDatas[i].amount);
            }
        }
    }

    public void ChangeTab(int type)
    {
        scroll.verticalNormalizedPosition = 1.0f;

        for (int i = 0; i < scroll.content.childCount; i++)
        {
            Destroy(scroll.content.GetChild(i).gameObject);
        }

        if (GameManager.Instance.itemInventory.itemDatas.Count < 300)
        {
            int count = 0;
            for (int i = 0; i < GameManager.Instance.itemInventory.itemDatas.Count; i++)
            {
                if (GameManager.Instance.itemInventory.itemDatas[i].data.type == type)
                {
                    Instantiate(Resources.Load<ItemInventorySlot>("UI/ItemInventorySlot"), scroll.content).Initialize(GameManager.Instance.itemInventory.itemDatas[i], GameManager.Instance.itemInventory.itemDatas[i].amount);
                    count++;
                }
            }
            for (int i = count; i < 300; i++)
            {
                Instantiate(Resources.Load<ItemInventorySlot>("UI/ItemInventorySlot"), scroll.content).Initialize(new Item(0), 0);
            }
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.itemInventory.itemDatas.Count; i++)
            {
                if (GameManager.Instance.itemInventory.itemDatas[i].data.type == type)
                {
                    Instantiate(Resources.Load<ItemInventorySlot>("UI/ItemInventorySlot"), scroll.content).Initialize(GameManager.Instance.itemInventory.itemDatas[i], GameManager.Instance.itemInventory.itemDatas[i].amount);
                }
            }
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        ChangeAll();
    }

    public void UpdateSlot()
    {
        for (int i = 0; i < 300; i++)
        {
            scroll.content.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = GameManager.Instance.heroInventory.heroDatas[i].ID.ToString();
        }
    }

    public void ChangeHero(Hero hero)
    {
        WeaponSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = hero.Weapon?.data?.name;
        GloveSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = hero.Glove?.data?.name;
        RingSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = hero.Ring?.data?.name;
        NecklessSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = hero.Neckless?.data?.name;
        HelmetSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = hero.Helmet?.data?.name;
        TopSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = hero.Top?.data?.name;
        BottomSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = hero.Bottom?.data?.name;
        ShoesSlot.transform.GetChild(0).GetComponent<TMP_Text>().text = hero.Shoes?.data?.name;
    }

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        ChangeAll();
    }

}
