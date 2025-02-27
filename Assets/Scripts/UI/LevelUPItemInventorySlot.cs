using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DataTable_FortuneHero;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class LevelUPItemInventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image sprite;
    public TMP_Text isEquip;
    public TMP_Text amount;
    
    private Item currentItem;
    private bool isSelected;

    public void Initialize(Item item, bool isSelect)
    {
        currentItem = item;
        isSelected = isSelect;
        ItemData itemData = DataManager.Instance.Item.Get(currentItem.id);

        if (itemData != null)
        {
            sprite.transform.GetChild(0).GetComponent<TMP_Text>().text = itemData.ID.ToString();
            isEquip.text = "";
            amount.text = currentItem.amount.ToString();
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
        if (!isSelected)
        {
            if (UIManager.Instance.Get<HeroLevelUP>() != null)
            {
                bool isAdded = false;
                foreach (var item in UIManager.Instance.Get<HeroLevelUP>().selectedItem)
                {
                    if (item.id == currentItem.id)
                    {
                        item.amount++;
                        currentItem.amount--;
                        isAdded = true;
                        break;
                    }
                }
                if (!isAdded)
                {
                    UIManager.Instance.Get<HeroLevelUP>().selectedItem.Add(new Item(currentItem.id, 1));
                    currentItem.amount--;
                }
                UIManager.Instance.Get<HeroLevelUP>().UpdateInventory();
            }
        }
        else
        {
            bool isReleased = false;
            foreach (var item in UIManager.Instance.Get<HeroLevelUP>().inventoryItemList)
            {
                if (item.id == currentItem.id && item.amount < 999)
                {
                    item.amount++;
                    currentItem.amount--;
                    isReleased = true;
                    break;
                }
            }
            if (!isReleased)
            {
                UIManager.Instance.Get<HeroLevelUP>().inventoryItemList.Add(new Item(currentItem.id, 1));
                currentItem.amount--;
            }
            UIManager.Instance.Get<HeroLevelUP>().UpdateInventory();
        }
    }
}
