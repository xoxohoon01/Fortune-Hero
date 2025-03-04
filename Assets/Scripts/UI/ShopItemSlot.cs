using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItemSlot : MonoBehaviour, IPointerClickHandler
{
    public Image sprite;
    private int currentShopID;

    public void Initialize(int id)
    {
        currentShopID = id;
        sprite.transform.GetChild(0).GetComponent<TMP_Text>().text = DataManager.Instance.Shop.Get(currentShopID).itemID.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.Get<Shop>().SelectItem(currentShopID);
    }
}
