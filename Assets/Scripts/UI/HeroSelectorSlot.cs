using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroSelectorSlot : MonoBehaviour, IPointerClickHandler
{
    public int heroNumber = -1;
    public int heroInventoryNumber = -1;

    public void InitializeHero(int number)
    {
        heroNumber = number;
        heroInventoryNumber = -1;

        if (GameManager.Instance.heroInventory.hero[number] != null)
            transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Heroes/{DataManager.Instance.Hero.Get(GameManager.Instance.heroInventory.hero[heroNumber].ID).name}");
    }

    public void InitializeHeroInventory(int number)
    {
        heroNumber = -1;
        heroInventoryNumber = number;

        transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Heroes/{DataManager.Instance.Hero.Get(GameManager.Instance.heroInventory.heroDatas[heroInventoryNumber].ID).name}");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (heroNumber != -1)
            UIManager.Instance.Get<ItemDescription>().SetHero(heroNumber);
        else if (heroInventoryNumber != -1)
            UIManager.Instance.Get<ItemDescription>().SetHeroInventory(heroInventoryNumber);

        UIManager.Instance.Get<ItemDescription>().SwapItem();
        UIManager.Instance.Hide<HeroSelector>();
    }
}
