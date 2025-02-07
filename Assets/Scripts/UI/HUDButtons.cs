using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDButtons : MonoBehaviour
{
    public void OpenHeroUpgradeMenu()
    {
        UIManager.Instance.Show<HeroUpgrade>();
    }

    public void OpenHeroMenu()
    {
        UIManager.Instance.Show<HeroInventory>();
    }
}
