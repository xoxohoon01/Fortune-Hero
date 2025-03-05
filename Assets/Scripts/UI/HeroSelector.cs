using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelector : UIBase
{
    public ScrollRect scroll;
    public HeroSelectorSlot hero1;
    public HeroSelectorSlot hero2;
    public HeroSelectorSlot hero3;
    public HeroSelectorSlot hero4;

    public override void Initialize()
    {
        hero1.InitializeHero(0);
        hero2.InitializeHero(1);
        hero3.InitializeHero(2);
        hero4.InitializeHero(3);
        for (int i = 0; i < scroll.content.childCount; i++)
        {
            Destroy(scroll.content.GetChild(i).gameObject);
        }

        for (int i = 0; i < GameManager.Instance.heroInventory.heroDatas.Count; i++)
        {
            Instantiate(Resources.Load<HeroSelectorSlot>("UI/HeroSelectorSlot"), scroll.content).InitializeHeroInventory(i);
        }
    }

    private void Start()
    {
        base.Initialize();
    }
}
