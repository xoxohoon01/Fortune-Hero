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

    public void OpenHeroUpgradeMenu()
    {
        UIManager.Instance.Show<HeroUpgrade>("FloatingUI");
    }

    public void OpenHeroMenu()
    {
        UIManager.Instance.Show<HeroInventory>("FloatingUI");
    }
}
