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
    private string targetUI;

    public void Initialize(Hero hero, int number, string target = "")
    {
        heroSlotNumber = number;
        currentHero = hero;
        targetUI = target;
        if (currentHero.ID != 0)
            transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Heroes/" + DataManager.Instance.Hero.Get(currentHero.ID).name);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentHero != null && currentHero.ID != 0)
        {
            if (targetUI == "HeroInventory")
            {
                if (UIManager.Instance.Get<HeroInventory>() != null)
                {
                    UIManager.Instance.Get<HeroInventory>().FinishChangeSlotMode();
                    UIManager.Instance.Get<HeroInventory>().ChangeHero(currentHero);
                    UIManager.Instance.Get<HeroInventory>().currentSelectedHeroInventory = heroSlotNumber;
                    UIManager.Instance.Get<HeroInventory>().currentSelectedHero = -1;
                }
            }
            else if (targetUI == "HeroTranscendence")
            {
                if (UIManager.Instance.Get<HeroTranscendence>() != null)
                {
                    UIManager.Instance.Get<HeroTranscendence>()?.SelectHero(heroSlotNumber);
                }
            }
            
        }
    }
}
