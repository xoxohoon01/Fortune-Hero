using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeroInventorySlot : UIBase, IPointerClickHandler
{
    public int heroSlotNumber;
    private Hero currentHero;

    public void Initialize(Hero hero, int number)
    {
        heroSlotNumber = number;
        currentHero = hero;
        if (currentHero.ID != 0)
            transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Heroes/" + DataManager.Instance.Hero.Get(currentHero.ID).name);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentHero != null && currentHero.ID != 0)
        {
            if (UIManager.Instance.Get<HeroInventory>() != null)
            {
                UIManager.Instance.Get<HeroInventory>().FinishChangeSlotMode();
                UIManager.Instance.Get<HeroInventory>().ChangeHero(currentHero);
                UIManager.Instance.Get<HeroInventory>().currentSelectedHeroInventory = heroSlotNumber;
                UIManager.Instance.Get<HeroInventory>().currentSelectedHero = -1;
            }
            
            if (UIManager.Instance.Get<HeroTranscendence>() != null)
            {
                UIManager.Instance.Get<HeroTranscendence>()?.SelectHero(heroSlotNumber);
            }
        }
    }
}
