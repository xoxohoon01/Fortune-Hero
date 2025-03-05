using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using DataTable_FortuneHero;
using System.Linq;
using Unity.VisualScripting.FullSerializer;

public class GameManager : MonoSingleton<GameManager>
{
    public HeroInventoryData heroInventory;
    public ItemInventoryData itemInventory;
    public PlayerStageData playerStage;

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

        // �������� �ҷ�����
        PlayerStageData stageData = DatabaseManager.Instance.LoadData<PlayerStageData>("StageData");
        if (stageData != null)
        {
            playerStage = stageData;
            StageManager.Instance.ChangeStage(playerStage.currentStage);
        }
        // ���� �����Ͱ� ���� ���
        else
        {
            playerStage = new PlayerStageData();
        }

        DatabaseManager.Instance.SaveData(heroInventory, "HeroData");
        DatabaseManager.Instance.SaveData(itemInventory, "ItemData");
        DatabaseManager.Instance.SaveData(playerStage, "StageData");
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
                if (itemInventory.itemDatas[i].id == id)
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
            itemInventory.itemDatas.Add(new Item(id, amount));
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
        GetItem(item.id, item.amount);
    }

    public void GetItems(List<Item> items)
    {
        foreach (var item in items)
        {
            GetItem(item.id, 1);
        }
    }

    public void GetItems(List<int> items)
    {
        foreach (var item in items)
        {
            GetItem(item, 1);
        }
    }

    public void HeroSwapItem(ref Item itemSlot, int heroNumber, int itemNumber)
    {
        switch (DataManager.Instance.Item.Get(itemInventory.itemDatas[itemNumber].id).equipSlot)
        {
            case 1:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Weapon = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Weapon = null;
                break;
            case 2:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Glove = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Glove = null;
                break;
            case 3:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Ring = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Ring = null;
                break;
            case 4:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Neckless = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Neckless = null;
                break;
            case 5:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Helmet = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Helmet = null;
                break;
            case 6:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Top = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Top = null;
                break;
            case 7:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Bottom = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Bottom = null;
                break;
            case 8:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Shoes = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Shoes = null;
                break;
            case 9:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Artifact = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Artifact = null;
                break;
        }
        if (itemSlot != null)
        {
            itemSlot.equipedHeroNumber = -1;
            itemSlot.equipedHeroInventoryNumber = -1;
        }
        itemSlot = Instance.itemInventory.itemDatas[itemNumber];
        itemInventory.itemDatas[itemNumber].equipedHeroNumber = heroNumber;
        itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber = -1;

        // �κ��丮 ����
        DatabaseManager.Instance.SaveData(heroInventory, "HeroData");
        DatabaseManager.Instance.SaveData(itemInventory, "ItemData");

        // UI ������Ʈ
        UIManager.Instance.Get<ItemInventory>()?.UpdateSlot();
        UIManager.Instance.Get<HeroItemInventory>()?.UpdateSlot();
    }

    public void HeroInventorySwapItem(ref Item itemSlot, int heroInventoryNumber, int itemNumber)
    {
        switch (DataManager.Instance.Item.Get(itemInventory.itemDatas[itemNumber].id).equipSlot)
        {
            case 1:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Weapon = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Weapon = null;
                break;
            case 2:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Glove = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Glove = null;
                break;
            case 3:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Ring = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Ring = null;
                break;
            case 4:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Neckless = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Neckless = null;
                break;
            case 5:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Helmet = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Helmet = null;
                break;
            case 6:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Top = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Top = null;
                break;
            case 7:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Bottom = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Bottom = null;
                break;
            case 8:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Shoes = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Shoes = null;
                break;
            case 9:
                if (itemInventory.itemDatas[itemNumber].equipedHeroNumber != -1)
                    heroInventory.hero[itemInventory.itemDatas[itemNumber].equipedHeroNumber].Artifact = null;
                if (itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber != -1)
                    heroInventory.heroDatas[itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber].Artifact = null;
                break;
        }
        if (itemSlot != null)
        {
            itemSlot.equipedHeroNumber = -1;
            itemSlot.equipedHeroInventoryNumber = -1;
        }
        itemSlot = Instance.itemInventory.itemDatas[itemNumber];
        Instance.itemInventory.itemDatas[itemNumber].equipedHeroNumber = -1;
        Instance.itemInventory.itemDatas[itemNumber].equipedHeroInventoryNumber = heroInventoryNumber;

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
