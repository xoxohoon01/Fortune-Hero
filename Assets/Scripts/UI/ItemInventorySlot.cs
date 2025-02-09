using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DataTable_FortuneHero;
using UnityEngine.UI;

public class ItemInventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image sprite;
    public TMP_Text isEquip;
    public TMP_Text amount;

    public int itemSlotNumber;
    private Item currentItem;

    public void Initialize(Item item, int number)
    {
        itemSlotNumber = number;
        currentItem = item;
        ItemData itemData = DataManager.Instance.Item.Get(currentItem.id);
        if (itemData != null)
        {
            if (itemData.type == 1)
            {
                if (currentItem.equipedHeroNumber == -1)
                {
                    sprite.transform.GetChild(0).GetComponent<TMP_Text>().text = itemData.ID.ToString();
                    isEquip.text = "";
                    amount.text = "";
                }
                else
                {
                    sprite.transform.GetChild(0).GetComponent<TMP_Text>().text = itemData.ID.ToString();
                    isEquip.text = "E";
                    amount.text = "";
                }
            }
            else
            {
                sprite.transform.GetChild(0).GetComponent<TMP_Text>().text = itemData.ID.ToString();
                isEquip.text = "";
                amount.text = currentItem.amount.ToString();
            }

            
        }
        else
        {
            sprite.transform.GetChild(0).GetComponent<TMP_Text>().text = "";
            isEquip.text = "";
            amount.text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemData itemData = DataManager.Instance.Item.Get(currentItem.id);

        if (itemData != null)
        {
            UIManager.Instance.Show<ItemDescription>("FloatingUI");
            ItemDescription item = UIManager.Instance.Get<ItemDescription>();
            item.Initialize(currentItem, itemSlotNumber);
        }
    }
}
