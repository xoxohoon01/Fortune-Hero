using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using DataTable_FortuneHero;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using static UnityEditor.Progress;

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
        UIManager.Instance.Get<HeroInventory>()?.UpdateHeroInventory();
    }

    public void RemoveHero(int slotNumber)
    {
        heroInventory.heroDatas.Remove(heroInventory.heroDatas[slotNumber]);
    }

    public bool hasItem(Item item)
    {
        int count = 0;
        for (int i = 0; i < itemInventory.itemDatas.Count; i++)
        {
            if (itemInventory.itemDatas[i].id == item.id)
            {
                count += itemInventory.itemDatas[i].amount;
            }
            if (count >= item.amount) return true;
        }
        return false;
    }

    public bool hasItems(List<Item> items)
    {
        List<Item> targetItems = new List<Item>();
        foreach(var item in items)
        {
            targetItems.Add(new Item(item.id, item.amount));
        }

        List<Item> deletedItems = new List<Item>();
        for (int j = 0; j < targetItems.Count; j++)
        {
            int count = 0;
            for (int i = 0; i < itemInventory.itemDatas.Count; i++)
            {
                if (itemInventory.itemDatas[i].id == targetItems[j].id)
                {
                    count += itemInventory.itemDatas[i].amount;
                }
            }
            if (count >= targetItems[j].amount)
            {
                deletedItems.Add(targetItems[j]);
            }
        }

        foreach(var item in deletedItems)
        {
            targetItems.Remove(item);
        }

        if (targetItems.Count <= 0) return true;
        return false;
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < itemInventory.itemDatas.Count; i++)
        {
            if (itemInventory.itemDatas[i].id == item.id)
            {
                if (itemInventory.itemDatas[i].amount > item.amount)
                    itemInventory.itemDatas[i].amount -= item.amount;
                else
                {
                    item.amount -= itemInventory.itemDatas[i].amount;
                    itemInventory.itemDatas[i].amount = 0;
                }
            }
        }

        for (int i = 0; i < itemInventory.itemDatas.Count; i++)
        {
            if (itemInventory.itemDatas[i].amount <= 0)
            {
                itemInventory.itemDatas.Remove(itemInventory.itemDatas[i]);
            }
        }
    }

    public void RemoveItems(List<Item> items)
    {
        for (int j = 0; j < items.Count; j++)
        {
            for (int i = 0; i < itemInventory.itemDatas.Count; i++)
            {
                if (itemInventory.itemDatas[i].id == items[j].id)
                {
                    if (itemInventory.itemDatas[i].amount > items[j].amount)
                    {
                        itemInventory.itemDatas[i].amount -= items[j].amount;
                        continue;
                    }
                    else
                    {
                        items[j].amount -= itemInventory.itemDatas[i].amount;
                        itemInventory.itemDatas[i].amount = 0;
                    }
                }
            }

            for (int i = 0; i < itemInventory.itemDatas.Count; i++)
            {
                if (itemInventory.itemDatas[i].amount <= 0)
                {
                    itemInventory.itemDatas.Remove(itemInventory.itemDatas[i]);
                }
            }
        }
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

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GetHero(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GetItem(1001);
        }
    }

}
