using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class GameManager : MonoSingleton<GameManager>
{
    public HeroInventoryData heroInventory;
    public ItemInventoryData itemInventory;

    public void Initialize()
    {
        DataManager.Instance.Initialize();
        UIManager.Instance.Initialize();

        // ���� �ҷ�����
        HeroInventoryData heroData = DatabaseManager.Instance.LoadData<HeroInventoryData>("HeroData");
        if (heroData != null)
        {
            heroInventory = heroData;
        }
        else
        {
            heroInventory = new HeroInventoryData();
            heroInventory.heroDatas = new List<Hero>();
            heroInventory.hero = new Hero[4];
        }

        // ������ �ҷ�����
        ItemInventoryData itemData = DatabaseManager.Instance.LoadData<ItemInventoryData>("ItemData");
        if (itemData != null)
        {
            itemInventory = itemData;
        }
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
                if (itemInventory.itemDatas[i].data.ID == id)
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

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetItem(10001, 2);
        }
    }

}
