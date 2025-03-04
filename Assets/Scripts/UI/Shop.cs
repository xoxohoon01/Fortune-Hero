using DataTable_FortuneHero;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : UIBase
{
    public TMP_Text itemName;
    public Image currentItemSlot;
    public ScrollRect scroll;
    public TMP_Text itemValue;
    public TMP_Text finalItemValue;
    public TMP_Text buyCountText;
    public TMP_Text amountText;
    public TMP_Text maxAmountText;
    public TMP_Text currentGold;

    private int selectedShopID = -1;
    private int selectedItemAmount;

    public override void Hide()
    {
        selectedShopID = -1;
        selectedItemAmount = 0;
    }

    public override void Initialize()
    {
        for (int i = 0; i < scroll.content.childCount; i++)
        {
            Destroy(scroll.content.GetChild(i).gameObject);
        }

        for (int i = 0; i < DataManager.Instance.Shop.Get().Count; i++)
        {
            Instantiate(Resources.Load<ShopItemSlot>("UI/ShopItemSlot"), scroll.content).Initialize(DataManager.Instance.Shop.Get()[i].ID);
        }

        UpdateShop();
    }

    public void MinusBuyCount()
    {
        selectedItemAmount = Mathf.Max(selectedItemAmount - 1, 0);

        UpdateShop();
    }

    public void PlusBuyCount()
    {
        ShopData shopData = DataManager.Instance.Shop.Get(selectedShopID);
        int maxAmount = GameManager.Instance.itemInventory.gold / shopData.value;
        if (shopData.maxAmount != 0)
            maxAmount = Mathf.Min(maxAmount, shopData.maxAmount);
        selectedItemAmount = Mathf.Min(selectedItemAmount + 1, maxAmount);

        UpdateShop();
    }

    public void Buy()
    {
        ShopData shopData = DataManager.Instance.Shop.Get(selectedShopID);
        if (selectedShopID != -1)
        {
            if (GameManager.Instance.itemInventory.gold >= shopData.value * selectedItemAmount)
            {
                GameManager.Instance.itemInventory.gold -= shopData.value * selectedItemAmount;
                GameManager.Instance.GetItem(shopData.itemID, selectedItemAmount);
            }

            selectedShopID = -1;
            selectedItemAmount = 0;
            UpdateShop();
        }
    }

    public void SelectItem(int shopID)
    {
        selectedShopID = shopID;
        selectedItemAmount = 0;
        UpdateShop();
    }

    public void UpdateShop()
    {
        if (selectedShopID != -1)
        {
            ShopData currentShop = DataManager.Instance.Shop.Get(selectedShopID);
            ItemData currentItem = DataManager.Instance.Item.Get(currentShop.itemID);
            itemName.text = currentItem.name;
            itemValue.text = currentShop.value.ToString();
            finalItemValue.text = (currentShop.value * selectedItemAmount).ToString();
            buyCountText.text = selectedItemAmount.ToString();

            int maxAmount = GameManager.Instance.itemInventory.gold / DataManager.Instance.Shop.Get(selectedShopID).value;
            if (currentShop.maxAmount != 0)
                maxAmount = Mathf.Min(maxAmount, currentShop.maxAmount);
            amountText.text = selectedItemAmount.ToString();
            maxAmountText.text = maxAmount.ToString();
        }
        else
        {
            itemName.text = "";
            itemValue.text = "";
            finalItemValue.text = "";
            buyCountText.text = "0";
            amountText.text = "0";
            maxAmountText.text = "0";
        }
        currentGold.text = GameManager.Instance.itemInventory.gold.ToString();
    }

    private void Start()
    {
        base.Initialize();
    }
}
