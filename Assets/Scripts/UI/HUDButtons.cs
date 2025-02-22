using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDButtons : UIBase
{
    public void OpenInventoryMenu()
    {
        UIManager.Instance.Show<ItemInventory>("FloatingUI");
    }
    public void OpenHeroMenu()
    {
        UIManager.Instance.Show<HeroInventory>("FloatingUI");
    }

    public void OpenGachaShopMenu()
    {
        UIManager.Instance.Show<GachaShop>("FloatingUI");
    }
}
