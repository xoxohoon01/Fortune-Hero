using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroSlot : MonoBehaviour, IPointerClickHandler
{
    public int currentSlot;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (UIManager.Instance.Get<HeroInventory>().isChangeSlotMode)
        {
            if (UIManager.Instance.Get<HeroInventory>().currentSelectedHeroInventory != -1)
            {
                Hero temp = GameManager.Instance.heroInventory.hero[currentSlot];
                GameManager.Instance.heroInventory.hero[currentSlot] = GameManager.Instance.heroInventory.heroDatas[UIManager.Instance.Get<HeroInventory>().currentSelectedHeroInventory];
                GameManager.Instance.heroInventory.heroDatas.Remove(GameManager.Instance.heroInventory.heroDatas[UIManager.Instance.Get<HeroInventory>().currentSelectedHeroInventory]);
                if (temp != null) GameManager.Instance.heroInventory.heroDatas.Add(temp);
            }
            else if (UIManager.Instance.Get<HeroInventory>().currentSelectedHero != -1)
            {
                Hero temp = GameManager.Instance.heroInventory.hero[currentSlot];
                GameManager.Instance.heroInventory.hero[currentSlot] = GameManager.Instance.heroInventory.hero[UIManager.Instance.Get<HeroInventory>().currentSelectedHero];
                GameManager.Instance.heroInventory.hero[UIManager.Instance.Get<HeroInventory>().currentSelectedHero] = temp;
            }
            
            UIManager.Instance.Get<HeroInventory>().currentSelectedHeroInventory = -1;
            UIManager.Instance.Get<HeroInventory>().currentSelectedHero = -1;
            UIManager.Instance.Get<HeroInventory>().UpdateHeroSlot();
            UIManager.Instance.Get<HeroInventory>().UpdateHeroInventory();
            UIManager.Instance.Get<HeroInventory>().FinishChangeSlotMode();
            UIManager.Instance.Get<HeroInventory>().ChangeHero(null);
            DatabaseManager.Instance.SaveData(GameManager.Instance.heroInventory, "HeroData");
        }
        else
        {
            if (GameManager.Instance.heroInventory.hero[currentSlot] != null)
            {
                UIManager.Instance.Get<HeroInventory>().ChangeHero(GameManager.Instance.heroInventory.hero[currentSlot]);
                UIManager.Instance.Get<HeroInventory>().currentSelectedHero = currentSlot;
                UIManager.Instance.Get<HeroInventory>().currentSelectedHeroInventory = -1;
            }
        }
    }
}
