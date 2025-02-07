using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroInventorySlot : MonoBehaviour, IPointerClickHandler
{
    public int heroSlotNumber;
    private Hero currentHero;

    public void Initialize(Hero hero, int number)
    {
        heroSlotNumber = number;
        currentHero = hero;
        if (currentHero.ID != 0)
            transform.GetChild(0).GetComponent<TMP_Text>().text = DataManager.Instance.Hero.Get(currentHero.ID).name;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentHero != null && currentHero.ID != 0)
        {
            UIManager.Instance.Get<HeroInventory>().FinishChangeSlotMode();
            UIManager.Instance.Get<HeroInventory>().ChangeHero(currentHero);
            UIManager.Instance.Get<HeroInventory>().currentSelectedHeroInventory = heroSlotNumber;
            UIManager.Instance.Get<HeroInventory>().currentSelectedHero = -1;
        }
    }
}
