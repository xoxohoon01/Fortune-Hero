using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DataTable_FortuneHero;

public class ItemDescription : UIBase
{
    public TMP_Text itemName;
    public TMP_Text itemUpgrade;
    public Image sprite;
    public TMP_Text itemDescription;

    private Item currentItem;
    private int currentItemNumber;
    private Hero targetHero;
    private int heroNumber;
    private int heroInventoryNumber;

    private bool isInitialized;

    public void Initialize(Item item, int itemNumber)
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);

        currentItem = item;
        currentItemNumber = itemNumber;

        ItemData itemData = DataManager.Instance.Item.Get(item.id);
        itemName.text = itemData.name;
        itemUpgrade.text = $"+{item.upgrade}";
        sprite.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Items/{DataManager.Instance.Item.Get(itemData.ID).name}");
        itemDescription.text = itemData.description;

        isInitialized = true;
    }
    
    public void SetHero(int number)
    {
        targetHero = GameManager.Instance.heroInventory.hero[number];
        heroNumber = number;
        heroInventoryNumber = -1;
    }

    public void SetHeroInventory(int number)
    {
        targetHero = GameManager.Instance.heroInventory.heroDatas[number];
        heroNumber = -1;
        heroInventoryNumber = number;
    }

    public void UseItem()
    {
        ItemData itemData = DataManager.Instance.Item.Get(currentItem.id);

        if (targetHero != null)
        {
            SwapItem();
        }
        else
        {
            UIManager.Instance.Show<HeroSelector>("FloatingUI");
            UIManager.Instance.Get<HeroSelector>().Initialize();
        }
    }

    public void SwapItem()
    {
        ItemData itemData = DataManager.Instance.Item.Get(currentItem.id);

        if (itemData.type == 1)
        {
            switch (itemData.equipSlot)
            {
                case 1:
                    if (heroNumber != -1)
                        GameManager.Instance.HeroSwapItem(ref targetHero.Weapon, heroNumber, currentItemNumber);
                    else if (heroInventoryNumber != -1)
                        GameManager.Instance.HeroInventorySwapItem(ref targetHero.Weapon, heroInventoryNumber, currentItemNumber);
                    break;
                case 2:
                    if (heroNumber != -1)
                        GameManager.Instance.HeroSwapItem(ref targetHero.Glove, heroNumber, currentItemNumber);
                    else if (heroInventoryNumber != -1)
                        GameManager.Instance.HeroInventorySwapItem(ref targetHero.Glove, heroInventoryNumber, currentItemNumber);
                    break;
                case 3:
                    if (heroNumber != -1)
                        GameManager.Instance.HeroSwapItem(ref targetHero.Ring, heroNumber, currentItemNumber);
                    else if (heroInventoryNumber != -1)
                        GameManager.Instance.HeroInventorySwapItem(ref targetHero.Ring, heroInventoryNumber, currentItemNumber);
                    break;
                case 4:
                    if (heroNumber != -1)
                        GameManager.Instance.HeroSwapItem(ref targetHero.Neckless, heroNumber, currentItemNumber);
                    else if (heroInventoryNumber != -1)
                        GameManager.Instance.HeroInventorySwapItem(ref targetHero.Neckless, heroInventoryNumber, currentItemNumber);
                    break;
                case 5:
                    if (heroNumber != -1)
                        GameManager.Instance.HeroSwapItem(ref targetHero.Helmet, heroNumber, currentItemNumber);
                    else if (heroInventoryNumber != -1)
                        GameManager.Instance.HeroInventorySwapItem(ref targetHero.Helmet, heroInventoryNumber, currentItemNumber);
                    break;
                case 6:
                    if (heroNumber != -1)
                        GameManager.Instance.HeroSwapItem(ref targetHero.Top, heroNumber, currentItemNumber);
                    else if (heroInventoryNumber != -1)
                        GameManager.Instance.HeroInventorySwapItem(ref targetHero.Top, heroInventoryNumber, currentItemNumber);
                    break;
                case 7:
                    if (heroNumber != -1)
                        GameManager.Instance.HeroSwapItem(ref targetHero.Bottom, heroNumber, currentItemNumber);
                    else if (heroInventoryNumber != -1)
                        GameManager.Instance.HeroInventorySwapItem(ref targetHero.Bottom, heroInventoryNumber, currentItemNumber);
                    break;
                case 8:
                    if (heroNumber != -1)
                        GameManager.Instance.HeroSwapItem(ref targetHero.Shoes, heroNumber, currentItemNumber);
                    else if (heroInventoryNumber != -1)
                        GameManager.Instance.HeroInventorySwapItem(ref targetHero.Shoes, heroInventoryNumber, currentItemNumber);
                    break;
                case 9:
                    if (heroNumber != -1)
                        GameManager.Instance.HeroSwapItem(ref targetHero.Artifact, heroNumber, currentItemNumber);
                    else if (heroInventoryNumber != -1)
                        GameManager.Instance.HeroInventorySwapItem(ref targetHero.Artifact, heroInventoryNumber, currentItemNumber);
                    break;
            }

            UIManager.Instance.Get<HeroItemInventory>()?.UpdateSlot();
            UIManager.Instance.Get<ItemInventory>()?.UpdateSlot();
            UIManager.Instance.Hide(gameObject.name);
        }

        targetHero = null;
        heroNumber = -1;
        heroInventoryNumber = -1;
    }

    private void Start()
    {
        Initialize();
    }
}
