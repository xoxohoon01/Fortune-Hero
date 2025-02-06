using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroInventorySlot : MonoBehaviour, IPointerClickHandler
{
    private Hero currentHero;

    public void Initialize(Hero hero)
    {
        currentHero = hero;
        if (currentHero.ID != 0)
            transform.GetChild(0).GetComponent<TMP_Text>().text = DataManager.Instance.Hero.Get(currentHero.ID).name;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentHero.ID != 0)
            UIManager.Instance.Get<HeroInventory>().ChangeHero(currentHero);
    }
}
