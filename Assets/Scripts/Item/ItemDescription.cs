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
    private int currentHeroNumber;

    private bool isInitialized;

    public void Initialize(Item item, int itemNumber)
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);

        currentItem = item;
        currentItemNumber = itemNumber;
        currentHeroNumber = 0;

        ItemData itemData = DataManager.Instance.Item.Get(item.id);
        itemName.text = itemData.name;
        itemUpgrade.text = $"+{item.upgrade}";
        sprite.transform.GetChild(0).GetComponent<TMP_Text>().text = itemData.ID.ToString();
        itemDescription.text = itemData.description;

        isInitialized = true;
    }
    
    public void UseItem()
    {
        ItemData itemData = DataManager.Instance.Item.Get(currentItem.id);

        if (itemData.type == 1)
        {
            switch (itemData.equipSlot)
            {
                case 1:
                    GameManager.Instance.SwapItem(ref GameManager.Instance.heroInventory.hero[currentHeroNumber].Weapon, currentHeroNumber, currentItemNumber);
                    break;
            }

            UIManager.Instance.Get<HeroItemInventory>()?.UpdateSlot();
            UIManager.Instance.Get<ItemInventory>()?.UpdateSlot();
            UIManager.Instance.Hide(gameObject.name);
        }
    }

    private void OnEnable()
    {
        if (isInitialized)
        {

        }
    }

    private void Start()
    {
        Initialize();
    }
}
