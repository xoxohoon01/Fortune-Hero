using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class HeroLevelUP : UIBase
{
    public ScrollRect inventoryScroll;
    public ScrollRect selectedItemScroll;

    public Image currentHeroSlot;
    public TMP_Text heroName;
    public TMP_Text heroLevel;
    public TMP_Text heroTranscendence;
    public TMP_Text heroGrade;

    private int currentHeroSlotNumber = -1;
    private int currentHeroInventorySlotNumber = -1;
    public List<Item> inventoryItemList;
    public List<Item> selectedItem;

    public override void Hide()
    {
        selectedItem = new List<Item>();
    }

    public void Initialize(int slotNumber)
    {
        inventoryItemList = new List<Item>();
        foreach (var item in GameManager.Instance.itemInventory.itemDatas)
        {
            if (item.id == 1001 || item.id == 1002 || item.id == 1003)
                inventoryItemList.Add(new Item(item.id, item.amount));
        }

        selectedItem = new List<Item>();

        currentHeroSlotNumber = slotNumber;
        currentHeroInventorySlotNumber = -1;

        UpdateHero();
        UpdateInventory();
    }

    public void InitializeInventory(int slotNumber)
    {
        inventoryItemList = new List<Item>();
        foreach (var item in GameManager.Instance.itemInventory.itemDatas)
        {
            if (item.id == 1001 || item.id == 1002 || item.id == 1003)
                inventoryItemList.Add(new Item(item.id, item.amount));
        }

        currentHeroSlotNumber = -1;
        currentHeroInventorySlotNumber = slotNumber;

        UpdateHero();
        UpdateInventory();
    }

    public void UpdateHero()
    {
        Hero currentHero;
        if (currentHeroSlotNumber != -1)
        {
            currentHero = GameManager.Instance.heroInventory.hero[currentHeroSlotNumber];
        }
        else if (currentHeroInventorySlotNumber != 1)
        {
            currentHero = GameManager.Instance.heroInventory.heroDatas[currentHeroInventorySlotNumber];
        }
        else
        {
            return;
        }
        currentHeroSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Heroes/{DataManager.Instance.Hero.Get(currentHero.ID).name}");
        heroName.text = DataManager.Instance.Hero.Get(currentHero.ID).name;
        heroLevel.text = currentHero.level.ToString();
        heroTranscendence.text = currentHero.transcendence.ToString();
        heroGrade.text = currentHero.grade.ToString();
    }

    public void UpdateInventory()
    {
        if (selectedItemScroll.content.childCount > 0)
        {
            for (int i = 0; i < selectedItemScroll.content.childCount; i++)
            {
                Destroy(selectedItemScroll.content.GetChild(i).gameObject);
            }
        }
        if (selectedItem.Count > 0)
        {
            for (int i = 0; i < selectedItem.Count; i++)
            {
                if (selectedItem[i].amount <= 0)
                    selectedItem.Remove(selectedItem[i]);
            }
            for (int i = 0; i < selectedItem.Count; i++)
            {
                Instantiate(Resources.Load<LevelUPItemInventorySlot>("UI/LevelUPItemInventorySlot"), selectedItemScroll.content).Initialize(selectedItem[i], true);
            }
        }
        
        if (inventoryScroll.content.childCount > 0)
        {
            for (int i = 0; i < inventoryScroll.content.childCount; i++)
            {
                Destroy(inventoryScroll.content.GetChild(i).gameObject);
            }
        }
        if (inventoryItemList.Count > 0)
        {
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                if (inventoryItemList[i].amount <= 0)
                    inventoryItemList.Remove(inventoryItemList[i]);
            }
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                Instantiate(Resources.Load<LevelUPItemInventorySlot>("UI/LevelUPItemInventorySlot"), inventoryScroll.content).Initialize(inventoryItemList[i], false);
            }
        }
        
    }

    public void LevelUP()
    {
        if (selectedItem.Count > 0)
        {
            if (GameManager.Instance.hasItems(selectedItem))
            {
                if (currentHeroSlotNumber != -1)
                {
                    for (int i = 0; i < selectedItem.Count; i++)
                    {
                        if (selectedItem[i].id == 1001)
                        {
                            GameManager.Instance.heroInventory.hero[currentHeroSlotNumber].level += selectedItem[i].amount;
                        }
                        else if (selectedItem[i].id == 1002)
                        {
                            GameManager.Instance.heroInventory.hero[currentHeroSlotNumber].level += selectedItem[i].amount * 3;
                        }
                        else if (selectedItem[i].id == 1003)
                        {
                            GameManager.Instance.heroInventory.hero[currentHeroSlotNumber].level += selectedItem[i].amount * 5;
                        }
                    }
                }
                else if (currentHeroInventorySlotNumber != -1)
                {
                    for (int i = 0; i < selectedItem.Count; i++)
                    {
                        if (selectedItem[i].id == 1001)
                        {
                            GameManager.Instance.heroInventory.heroDatas[currentHeroInventorySlotNumber].level += selectedItem[i].amount;
                        }
                        else if (selectedItem[i].id == 1002)
                        {
                            GameManager.Instance.heroInventory.heroDatas[currentHeroInventorySlotNumber].level += selectedItem[i].amount * 3;
                        }
                        else if (selectedItem[i].id == 1003)
                        {
                            GameManager.Instance.heroInventory.heroDatas[currentHeroInventorySlotNumber].level += selectedItem[i].amount * 5;
                        }
                    }
                }
                else
                {
                    return;
                }

                GameManager.Instance.RemoveItems(selectedItem);
                selectedItem = new List<Item>();

                UpdateHero();
                UpdateInventory();

                DatabaseManager.Instance.SaveData(GameManager.Instance.heroInventory, "HeroData");
                DatabaseManager.Instance.SaveData(GameManager.Instance.itemInventory, "ItemData");
            }
        }
    }

    private void Start()
    {
        base.Initialize();
    }
}
