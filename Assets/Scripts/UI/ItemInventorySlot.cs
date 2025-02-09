using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInventorySlot : MonoBehaviour, IPointerClickHandler
{
    public int itemSlotNumber;
    private Item currentItem;

    public void Initialize(Item item, int number)
    {
        itemSlotNumber = number;
        currentItem = item;
        if (currentItem.data != null)
        {
            transform.GetChild(0).GetComponent<TMP_Text>().text = currentItem.data.ID.ToString();
            transform.GetChild(1).GetComponent<TMP_Text>().text = currentItem.amount.ToString();
        }
        else
        {
            transform.GetChild(0).GetComponent<TMP_Text>().text = "";
            transform.GetChild(1).GetComponent<TMP_Text>().text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem.data != null)
        {
            UIManager.Instance.Show<ItemDescription>("FloatingUI");
            UIManager.Instance.Get<ItemDescription>().Initialize(currentItem);
        }
    }
}
