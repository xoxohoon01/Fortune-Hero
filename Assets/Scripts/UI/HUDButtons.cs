using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDButtons : MonoBehaviour
{
    public void OpenHeroMenu()
    {
        UIManager.Instance.Show<HeroInventory>();
    }
}
