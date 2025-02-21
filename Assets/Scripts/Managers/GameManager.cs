using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using DataTable_FortuneHero;

public class GameManager : MonoSingleton<GameManager>
{
    public HeroInventoryData heroInventory;
    public ItemInventoryData itemInventory;
    public void Initialize()
    {
        Application.targetFrameRate = 60;
        DataManager.Instance.Initialize();
        UIManager.Instance.Initialize();
        StageManager.Instance.Initialize();
        CameraManager.Instance.Initialize();
        ObjectPoolManager.Instance.Initialize();

        // ���� �ҷ�����
        HeroInventoryData heroData = DatabaseManager.Instance.LoadData<HeroInventoryData>("HeroData");
        if (heroData != null)
        {
            heroInventory = heroData;
        }
        // ���� �����Ͱ� ���� ���
        else
        {
            heroInventory = new HeroInventoryData();
            heroInventory.heroDatas = new List<Hero>();
            heroInventory.hero = new Hero[4];

            heroInventory.hero[0] = new Hero(1);
        }

        // ������ �ҷ�����
        ItemInventoryData itemData = DatabaseManager.Instance.LoadData<ItemInventoryData>("ItemData");
        if (itemData != null)
        {
            itemInventory = itemData;
        }
        // ���� �����Ͱ� ���� ���
        else
        {
            itemInventory = new ItemInventoryData();
            itemInventory.itemDatas = new List<Item>();
        }

        DatabaseManager.Instance.SaveData(heroInventory, "HeroData");
    }

    public void GetItem(int id, int amount = 1)
    {
        bool isAdded = false;
        ItemData itemData = DataManager.Instance.Item.Get(id);
        #region ������ �߰� ����

        // ��� �������� ���
        if (DataManager.Instance.Item.Get(id).type == 1)
        {
            for (int i = 0; i < amount; i++)
            {
                itemInventory.itemDatas.Add(new Item(id));
            }
            isAdded = true;
        }

        // �ƴ� ���
        else
        {
            // �κ��丮�� ���� �������� �ִ� ���
            for (int i = 0; i < itemInventory.itemDatas.Count; i++)
            {
                if (itemData.ID == id)
                {
                    if (itemInventory.itemDatas[i].amount < 99)
                    {
                        if (itemInventory.itemDatas[i].amount + amount <= 99)
                        {
                            itemInventory.itemDatas[i].amount += amount;
                            isAdded = true;
                            break;
                        }
                        else
                        {
                            itemInventory.itemDatas.Add(new Item(id, amount - (99 - itemInventory.itemDatas[i].amount)));
                            itemInventory.itemDatas[i].amount = 99;
                            isAdded = true;
                            break;
                        }
                    }
                }
            }
        }

        // �κ��丮�� ���� �������� ���� ���
        if (!isAdded) 
        {
            itemInventory.itemDatas.Add(new Item(id));
        }

        #endregion

        // �κ��丮 ����
        DatabaseManager.Instance.SaveData(itemInventory, "ItemData");

        // UI ������Ʈ
        UIManager.Instance.Get<ItemInventory>()?.UpdateSlot();
        UIManager.Instance.Get<HeroItemInventory>()?.UpdateSlot();
    }
    
    public void GetItem(Item item)
    {
        itemInventory.itemDatas.Add(item);
    }

    public void SwapItem(ref Item itemSlot, int heroNumber, int itemNumber)
    {
        if (itemSlot != null)
        {
            itemSlot.equipedHeroNumber = -1;
        }
        itemSlot = Instance.itemInventory.itemDatas[itemNumber];
        Instance.itemInventory.itemDatas[itemNumber].equipedHeroNumber = heroNumber;

        // �κ��丮 ����
        DatabaseManager.Instance.SaveData(heroInventory, "HeroData");
        DatabaseManager.Instance.SaveData(itemInventory, "ItemData");

        // UI ������Ʈ
        UIManager.Instance.Get<ItemInventory>()?.UpdateSlot();
        UIManager.Instance.Get<HeroItemInventory>()?.UpdateSlot();
    }

    public void GetHero(int id, int grade)
    {
        HeroData heroData = DataManager.Instance.Hero.Get(id);

        heroInventory.heroDatas.Add(new Hero(id, grade));

        DatabaseManager.Instance.SaveData(heroInventory, "HeroData");
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = Time.timeScale == 1 ? 2 : 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StageManager.Instance.EndStage();
            StageManager.Instance.StartStage();
        }
    }

}
